Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Public Class EPGItemListViewModel
    Inherits ViewModelBase

    Private Property _NoEventsAvailableVisibility As Visibility
    Public Property NoEventsAvailableVisibility As Visibility
        Get
            Return _NoEventsAvailableVisibility
        End Get
        Set(value As Visibility)
            _NoEventsAvailableVisibility = value
            RaisePropertyChanged("NoEventsAvailableVisibility")
        End Set
    End Property

    Public Property groupeditems As ObservableCollection(Of Group(Of EPGItemViewModel))
        Get
            Return _groupeditems
        End Get
        Set(value As ObservableCollection(Of Group(Of EPGItemViewModel)))
            _groupeditems = value
            RaisePropertyChanged("groupeditems")
            If value.Count = 0 Then NoEventsAvailableVisibility = Visibility.Visible Else NoEventsAvailableVisibility = Visibility.Collapsed
        End Set
    End Property
    Private Property _groupeditems As ObservableCollection(Of Group(Of EPGItemViewModel))

    Public Sub New()
        groupeditems = New ObservableCollection(Of [Group](Of EPGItemViewModel))
    End Sub

    Public Async Function AddEvent(e As EPGItemViewModel) As Task
        WriteToDebug("EPGItemListViewModel.AddEvent()", "start")

        'FIND / INSERT THE GROUP IN WHICH THE EVENT SHOULD BELONG
        Dim dayGroup = groupeditems.Where(Function(x) x.Key = e.startDate.Date).FirstOrDefault()
        Dim dayGroupIndex As Integer
        If Not dayGroup Is Nothing Then
            dayGroupIndex = groupeditems.IndexOf(dayGroup)
        Else
            dayGroup = groupeditems.Where(Function(x) x.Key > e.startDate.Date).FirstOrDefault()
            If Not dayGroup Is Nothing Then
                'We found the first group which is > than the event. Get the index and create a new group
                dayGroupIndex = groupeditems.IndexOf(dayGroup)
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems.Insert(dayGroupIndex, New Group(Of EPGItemViewModel)(e.startDate.Date, New ObservableCollection(Of EPGItemViewModel)))
                                                                                                             End Sub)
            Else
                '    Any group that does exist is < than the one we want to create, therefore just add a new group to the end.
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems.Add(New Group(Of EPGItemViewModel)(e.startDate.Date, New ObservableCollection(Of EPGItemViewModel)))
                                                                                                             End Sub)
            End If
        End If

        'The daygroup should now exist, re-catch the Index 
        dayGroup = groupeditems.Where(Function(x) x.Key = e.startDate.Date).FirstOrDefault()
        dayGroupIndex = groupeditems.IndexOf(dayGroup)

        'INSERT THE EPG EVENT
        Dim insertEvent = (groupeditems(dayGroupIndex).Where(Function(x) x.startDate > e.startDate).OrderBy(Function(x) x.startDate)).FirstOrDefault()
        If Not insertEvent Is Nothing Then
            'We found a event that starts later than the one we want to insert. Use this event's index to insert our new recording
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             groupeditems(dayGroupIndex).Insert(groupeditems(dayGroupIndex).IndexOf(insertEvent), e)
                                                                                                         End Sub)
        Else
            'No event was found with a time > than the event we want to add. Therefore just add our event to the end
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             groupeditems(dayGroupIndex).Add(e)
                                                                                                         End Sub)
        End If
        WriteToDebug("EPGItemListViewModel.AddEvent()", "stop")
    End Function

    Public Async Function RemoveEvent(eventid As String) As Task
        WriteToDebug("EPGItemListViewModel.RemoveEvent()", "start")
        Dim eventToDelete As EPGItemViewModel
        Dim groupIndex As Integer
        If Not Me.groupeditems Is Nothing AndAlso Me.groupeditems.Count > 0 Then
            For Each g In Me.groupeditems
                Dim tmpEvent As EPGItemViewModel = (From epgevent In g Where epgevent.eventId = eventid Select epgevent).FirstOrDefault()
                If Not tmpEvent Is Nothing Then
                    eventToDelete = tmpEvent
                    groupIndex = groupeditems.IndexOf(g)
                End If
            Next

            If Not eventToDelete Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems(groupIndex).Remove(eventToDelete)
                                                                                                             End Sub)
            End If
            If groupeditems(groupIndex).Count = 0 Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems.RemoveAt(groupIndex)
                                                                                                             End Sub)
            End If
        End If
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         If groupeditems.Count = 0 Then NoEventsAvailableVisibility = Visibility.Visible
                                                                                                     End Sub)

        WriteToDebug("EPGItemListViewModel.RemoveEvent()", "stop")
    End Function

    Public Async Function UpdateEvent(e As EPGItemViewModel) As Task
        'WriteToDebug("EPGItemListViewModel.UpdateEvent()", "start")
        Dim oldEvent As EPGItemViewModel
        Dim dayGroupIndex As Integer

        Dim dayGroup = groupeditems.Where(Function(x) x.Key = e.startDate.Date).FirstOrDefault()
        If Not dayGroup Is Nothing Then
            dayGroupIndex = groupeditems.IndexOf(dayGroup)
            oldEvent = (From epgevent In groupeditems(dayGroupIndex) Where epgevent.eventId = e.eventId).FirstOrDefault()
            If Not oldEvent Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 oldEvent.dvrState = e.dvrState
                                                                                                                 oldEvent.dvrUuid = e.dvrUuid
                                                                                                                 oldEvent.percentcompleted = e.percentcompleted
                                                                                                             End Sub)
            Else
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                 Await AddEvent(e)
                                                                                                             End Sub)

            End If
        End If
        'WriteToDebug("EPGItemListViewModel.UpdateEvent()", "stop")
    End Function



End Class