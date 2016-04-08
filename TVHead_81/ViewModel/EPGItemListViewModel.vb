Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Public Class EPGItemListViewModel
    Inherits ViewModelBase

    Public Property items As List(Of EPGItemViewModel)
        Get
            Return _items
        End Get
        Set(value As List(Of EPGItemViewModel))
            _items = value
            RaisePropertyChanged("items")
            RaisePropertyChanged("groupeditems")
            RaisePropertyChanged("currentEPGItem")
        End Set
    End Property
    Private Property _items As List(Of EPGItemViewModel)

    Public ReadOnly Property groupeditems As List(Of Group(Of EPGItemViewModel))
        Get
            Return (From epgevent In items
                    Group By Day = epgevent.startDate.Date Into Group
                    Select New Group(Of EPGItemViewModel)(Day, Group)).ToList()
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
        items = New List(Of EPGItemViewModel)
    End Sub



    Public Sub EPGItem_Clicked(sender As Object, e As ItemClickEventArgs)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        WriteToDebug("EPGItemListViewModel.EPGItem_Clicked", "executed")
        Dim clickedEPGItem As EPGItemViewModel = TryCast(e.ClickedItem, EPGItemViewModel)
        If Not clickedEPGItem Is Nothing Then
            For Each i In vm.SelectedChannel.epgitems.items
                If i.eventId = clickedEPGItem.eventId Then i.IsSelected = True Else i.IsSelected = False
            Next
        End If
        vm.selectedEPGItem = clickedEPGItem
        vm.SelectedPivotIndex = 2
    End Sub

    Public Async Function AddEvent(e As EPGItemViewModel) As Task
        Dim insertEvent = (items.Where(Function(x) x.startDate > e.startDate).OrderBy(Function(x) x.startDate)).FirstOrDefault()
        If Not insertEvent Is Nothing Then
            'We found a event that starts later than the one we want to insert. Use this event's index to insert our new recording
            Await RunOnUIThread(Sub()
                                    items.Insert(items.IndexOf(insertEvent), e)
                                End Sub)
        Else
            'No event was found with a time > than the event we want to add. Therefore just add our event to the end
            Await RunOnUIThread(Sub()
                                    items.Add(e)
                                End Sub)
        End If
    End Function

    Public Async Function RemoveEvent(eventid As String) As Task
        WriteToDebug("EPGItemListViewModel.RemoveEvent()", "executed")
        Dim eventToRemove As EPGItemViewModel = items.Where(Function(x) x.eventId = eventid).FirstOrDefault
        If Not eventToRemove Is Nothing Then
            Await RunOnUIThread(Sub()
                                    items.Remove(eventToRemove)
                                End Sub)
        End If
    End Function

    ''' <summary>
    ''' Searches for and then updates a given EPGItem within the EPGItemListViewModel
    ''' </summary>
    ''' <param name="e">Updated EPG Event</param>
    ''' <returns></returns>
    Public Async Function UpdateEvent(e As EPGItemViewModel) As Task
        e.Update()
    End Function


    Public Function GetEvent(eventid As String) As EPGItemViewModel
        Dim tmpEvent As EPGItemViewModel
        tmpEvent = (From epgevent In items Where epgevent.eventId = eventid Select epgevent).FirstOrDefault()
        If Not tmpEvent Is Nothing Then Return tmpEvent Else Return Nothing
    End Function

    Public Sub ClearAllButCurrent()
        If items.Count > 1 Then
            ' items.RemoveRange(1, items.Count - 1)
        End If
    End Sub

End Class