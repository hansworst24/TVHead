Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports TVHead_81.Common

Public Class SearchPageViewModel
    Inherits ViewModelBase

    Private Property _UseFullTextSearch As Boolean
    Public Property UseFullTextSearch As Boolean
        Get
            Return _UseFullTextSearch
        End Get
        Set(value As Boolean)
            _UseFullTextSearch = value
            RaisePropertyChanged("UseFullTextSearch")
        End Set
    End Property

    Private Property _SearchValue As String
    Public Property SearchValue As String
        Get
            Return _SearchValue
        End Get
        Set(value As String)
            _SearchValue = value
            RaisePropertyChanged("SearchValue")
        End Set
    End Property

    Public Property SearchResults As New ObservableCollection(Of ChannelViewModel)

    Private Property _GroupedSearchResults As ObservableCollection(Of Group(Of ChannelViewModel))
    Public Property GroupedSearchResults As ObservableCollection(Of Group(Of ChannelViewModel))
        Get
            Return _GroupedSearchResults
        End Get
        Set(value As ObservableCollection(Of Group(Of ChannelViewModel)))
            _GroupedSearchResults = value
            RaisePropertyChanged("GroupedSearchResults")
        End Set
    End Property

    Private Property _SearchIsActive As String
    Public Property SearchIsActive As String
        Get
            Return _SearchIsActive
        End Get
        Set(value As String)
            _SearchIsActive = value
            RaisePropertyChanged("SearchIsActive")
        End Set
    End Property

    Private Property _SearchProgressPercentage As Double
    Public Property SearchProgressPercentage As Double
        Get
            Return _SearchProgressPercentage
        End Get
        Set(value As Double)
            _SearchProgressPercentage = value
            RaisePropertyChanged("SearchProgressPercentage")
        End Set
    End Property

    Public Property SearchTextChangedCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("SearchPageViewModel.SearchTextChangedCommand", "start")
                                        Await Task.Delay(100)
                                        If Not SearchValue = "" And SearchValue.Length >= 3 Then
                                            WriteToDebug("SearchPageViewModel.SearchTextChangedCommand", String.Format("SearchKeyUpCommand Executed : Text = {0}", SearchValue))
                                            Me.SearchIsActive = "True"
                                            Await StartSearch(SearchValue)
                                            Me.SearchIsActive = "False"
                                            WriteToDebug("SearchPageViewModel.SearchTextChangedCommand", "stop")
                                        End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property SearchButtonCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("SearchPageViewModel.SearchButtonCommand", "start")
                                        If Not SearchValue = "" And SearchValue.Length >= 3 Then
                                            WriteToDebug("SearchPageViewModel.SearchTextChangedCommand", String.Format("SearchKeyUpCommand Executed : Text = {0}", SearchValue))
                                            SearchIsActive = True
                                            Await StartSearch(SearchValue)
                                            SearchIsActive = False
                                            WriteToDebug("SearchPageViewModel.SearchTextChangedCommand", "stop")
                                        End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property



    Public Async Function StartSearch(searchvalue As String) As Task
        WriteToDebug("SearchPageViewModel.StartSearch", "start")
        Dim i As IEnumerable(Of ChannelViewModel)
        i = Await SearchEPGEntry(searchvalue, UseFullTextSearch)
        For Each result In i
            'result.loadEPGButtonEnabled = False
        Next
        GroupedSearchResults = (From e In i Group By Day = e.epgitems.currentEPGItem.startDate.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern) Into Group
                                Select New Group(Of ChannelViewModel)(Day, Group)).ToObservableCollection()
        WriteToDebug("SearchPageViewModel.StartSearch", "stop")

    End Function
End Class
