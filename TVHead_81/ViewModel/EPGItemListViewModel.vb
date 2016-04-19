Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Public Class EPGItemListViewModel
    Inherits ViewModelBase

    Public Property items As ObservableCollection(Of EPGItemViewModel)
        Get
            Return _items
        End Get
        Set(value As ObservableCollection(Of EPGItemViewModel))
            _items = value
            RaisePropertyChanged("items")
            RaisePropertyChanged("groupeditems")
            RaisePropertyChanged("currentEPGItem")
        End Set
    End Property
    Private Property _items As ObservableCollection(Of EPGItemViewModel)

    Public ReadOnly Property groupeditems As ObservableCollection(Of Group(Of EPGItemViewModel))
        Get
            Return (From epgevent In items
                    Group By Day = epgevent.startDate.Date Into Group
                    Select New Group(Of EPGItemViewModel)(Day, Group)).ToObservableCollection()
        End Get
    End Property

    Public ReadOnly Property currentEPGItem As EPGItemViewModel
        Get
            If Not items Is Nothing And Not items.Count = 0 Then
                Return items(0)
            Else
                Return New EPGItemViewModel
            End If
        End Get
    End Property


    Public Sub New()
        items = New ObservableCollection(Of EPGItemViewModel)
    End Sub

    ''' <summary>
    ''' Selects an EPGItem in the EPGItemList. Depending on if we're running on a widescreen device or not it will also expand or collapse the EPGItem
    ''' </summary>
    ''' <param name="e"></param>
    Public Sub SelectEPGItem(e As EPGItemViewModel)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        For Each epgitem In items
            If epgitem.eventId = e.eventId Then
                epgitem.IsSelected = True
                epgitem.IsExpanded = If(vm.IsRunningOnWideScreen, False, Not epgitem.IsExpanded)
            Else
                epgitem.IsSelected = False
                epgitem.IsExpanded = False
            End If
        Next
    End Sub


    ''' <summary>
    ''' Selects the clicked EPGItem in the EPGItemListViewModel.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub EPGItem_Clicked(sender As Object, e As ItemClickEventArgs)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        WriteToDebug("EPGItemListViewModel.EPGItem_Clicked", "executed")
        If vm.IsRunningOnWideScreen Then vm.Channels.ClearSelection()
        Dim clickedEPGItem As EPGItemViewModel = TryCast(e.ClickedItem, EPGItemViewModel)
        If Not clickedEPGItem Is Nothing Then SelectEPGItem(clickedEPGItem)
        vm.selectedEPGItem = clickedEPGItem
    End Sub

    Public Sub AddEvent(e As EPGItemViewModel)
        Dim insertEvent = (items.Where(Function(x) x.startDate > e.startDate).OrderBy(Function(x) x.startDate)).FirstOrDefault()
        If Not insertEvent Is Nothing Then
            'We found a event that starts later than the one we want to insert. Use this event's index to insert our new recording
            RunOnUIThread(Sub()
                              items.Insert(items.IndexOf(insertEvent), e)
                          End Sub)
        Else
            'No event was found with a time > than the event we want to add. Therefore just add our event to the end
            RunOnUIThread(Sub()
                              items.Add(e)
                              RaisePropertyChanged("currentEPGItem")
                              RaisePropertyChanged("groupeditems")
                          End Sub)
        End If
    End Sub

    Public Sub RemoveEvent(eventid As String)
        WriteToDebug("EPGItemListViewModel.RemoveEvent()", "executed")
        Dim eventToRemove As EPGItemViewModel = items.Where(Function(x) x.eventId = eventid).FirstOrDefault
        If Not eventToRemove Is Nothing Then
            RunOnUIThread(Sub()
                              items.Remove(eventToRemove)
                              RaisePropertyChanged("currentEPGItem")
                              RaisePropertyChanged("groupeditems")
                          End Sub)
        End If
    End Sub

    ''' <summary>
    ''' Searches for and then updates a given EPGItem within the EPGItemListViewModel
    ''' </summary>
    ''' <param name="e">Updated EPG Event</param>
    Public Sub UpdateEvent(e As EPGItemViewModel)
        e.Update()
    End Sub

    Public Function GetEvent(eventid As String) As EPGItemViewModel
        Dim tmpEvent As EPGItemViewModel
        tmpEvent = (From epgevent In items Where epgevent.eventId = eventid Select epgevent).FirstOrDefault()
        If Not tmpEvent Is Nothing Then Return tmpEvent Else Return Nothing
    End Function

    ''' <summary>
    ''' Removes all EPGItems except the first one (currentEPGItem) from the list, to save memory. Used when browsing to another channel
    ''' </summary>
    Public Sub ClearAllButCurrent()
        If items.Count > 1 Then
            For i As Integer = items.Count - 1 To 1 Step -1
                items.RemoveAt(i)
            Next
        End If
    End Sub

End Class