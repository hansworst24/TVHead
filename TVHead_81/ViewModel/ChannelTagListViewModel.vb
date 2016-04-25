Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json

Public Class ChannelTagListViewModel
    Inherits ViewModelBase
    Public Property items As ObservableCollection(Of ChannelTagViewModel)
    Public Property dataLoaded As Boolean

    ''' <summary>
    ''' The selectedChannelTag is the Tag which is selected on the Channels View, and for which the channels are shown
    ''' </summary>
    ''' <returns></returns>
    Public Property selectedChannelTag As ChannelTagViewModel
        Get
            Return _selectedChannelTag
        End Get
        Set(value As ChannelTagViewModel)
            _selectedChannelTag = value
            RaisePropertyChanged("selectedChannelTag")
        End Set
    End Property
    Private Property _selectedChannelTag As ChannelTagViewModel

    ''' <summary>
    ''' The favouriteChannelTag is the Tag which is used to select the channels for when TVHead starts up. The UUID of the ChannelTag is stored in the apps localStorage
    ''' </summary>
    ''' <returns></returns>
    Public Property favouriteChannelTag As ChannelTagViewModel
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Dim tag As ChannelTagViewModel = (From t In items Where t.uuid = vm.TVHeadSettings.FavouriteChannelTag Select t).FirstOrDefault
            If Not tag Is Nothing Then
                Return tag
            Else
                If Not items.Count = 0 Then Return items(0) Else Return Nothing
            End If
        End Get
        Set(value As ChannelTagViewModel)
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            vm.TVHeadSettings.FavouriteChannelTag = value.uuid
        End Set
    End Property
    Public Async Function Load() As Task
        WriteToDebug("ChannelTagListViewModel.Load()", "executed")
        items.Clear()
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON((New api40).apiGetChannelTags())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("ChannelTagListViewModel.Load()", "stop-error")
            Return
        End Try
        If Not json_result = "" Then
            Dim dsChannelTagList = JsonConvert.DeserializeObject(Of TVHChannelTagList)(json_result)
            For Each retrievedChannelTag In dsChannelTagList.entries.OrderBy(Function(x) x.name)
                items.Add(New ChannelTagViewModel(retrievedChannelTag))
            Next
        End If
        RaisePropertyChanged("favouriteChannelTag")
        If Not favouriteChannelTag Is Nothing Then
            selectedChannelTag = favouriteChannelTag
        ElseIf Not items.Count = 0 Then
            selectedChannelTag = items(0)
        Else
            selectedChannelTag = Nothing
        End If
        'Set the Selected Channel Tag to the one stored in localsettings, if it exists
        dataLoaded = True
        WriteToDebug("Modules.LoadChannelTags()", "stop")
    End Function

    ''' <summary>
    ''' Sets the favouriteChannelTag to the one chosen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub FavouriteChanged(sender As Object, e As SelectionChangedEventArgs)
        WriteToDebug("ChannelTagListViewModel.FavouriteChanged", "executed")
        Dim sChannelTag As ChannelTagViewModel = TryCast(sender, ComboBox).SelectedItem
        If Not sChannelTag Is Nothing Then
            favouriteChannelTag = sChannelTag
        End If
    End Sub

    ''' <summary>
    ''' Changes the selectedChannelTag to the one chosen, and re-loads the Channels for this ChannelTag
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Async Sub SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        WriteToDebug("ChannelTagListViewModel.SelectionChanged", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim sChannelTag As ChannelTagViewModel = TryCast(sender, ComboBox).SelectedItem
        If Not sChannelTag Is Nothing AndAlso selectedChannelTag.uuid <> sChannelTag.uuid Then
            selectedChannelTag = sChannelTag
            Await vm.Channels.Load()
        End If
        vm.Notify.Clear()
    End Sub

    Public Sub New()
        items = New ObservableCollection(Of ChannelTagViewModel)
    End Sub
End Class