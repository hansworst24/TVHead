Imports GalaSoft.MvvmLight
Imports TVHead_81.Common
Imports TVHead_81.ViewModels
Imports Windows.UI.Popups
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports GalaSoft.MvvmLight.Command

Public Class RecordingListViewModel
    Inherits ViewModelBase

    Public Const upcomingRecordings = "upcomingRecordings"
    Public Const finishedRecordings = "finishedRecordings"
    Public Const failedRecordings = "failedRecordings"
    Public Const Ascending = "Ascending"
    Public Const Descending = "Descending"


    Public Sub New()
        SortingOrder = Descending
        groupeditems = New ObservableCollection(Of Group(Of RecordingViewModel))
        'vm = app.DefaultViewModel
    End Sub

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
        End Get

    End Property


    Public Property dataLoaded As Boolean
    Public Property SortingOrder As String 'Is used to identify in which way the RecordingList should be sorted, ascending or descending. It defaults to Descending.
    Public Property RecordingType As String

    Private Property _NoRecordingsAvailableVisibility As Visibility
    Public Property NoRecordingsAvailableVisibility As Visibility
        Get
            Return _NoRecordingsAvailableVisibility
        End Get
        Set(value As Visibility)
            _NoRecordingsAvailableVisibility = value
            RaisePropertyChanged("NoRecordingsAvailableVisibility")
        End Set
    End Property

    Private Property _MultiSelectMode As ListViewSelectionMode
    Public Property MultiSelectMode As ListViewSelectionMode
        Get
            Return _MultiSelectMode
        End Get
        Set(value As ListViewSelectionMode)
            _MultiSelectMode = value
            RaisePropertyChanged("MultiSelectMode")
        End Set
    End Property

    Private Property _groupeditems As ObservableCollection(Of Group(Of RecordingViewModel))
    Public Property groupeditems As ObservableCollection(Of Group(Of RecordingViewModel))
        Get
            Return _groupeditems
        End Get
        Set(value As ObservableCollection(Of Group(Of RecordingViewModel)))
            _groupeditems = value
            If Not _groupeditems Is Nothing AndAlso _groupeditems.Count > 0 Then NoRecordingsAvailableVisibility = Visibility.Collapsed Else NoRecordingsAvailableVisibility = Visibility.Visible
            RaisePropertyChanged("groupeditems")
        End Set
    End Property

    Public Function SetExpanseCollapseEnabled(b As Boolean)
        If Not Me.groupeditems Is Nothing Then
            For Each g In groupeditems
                For Each item In g
                    item.ExpanseCollapseEnabled = b
                Next
            Next
        End If
    End Function

    Public Property RecordingsSelectionChanged As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        ''WINRT APPARENTLY DOESNT SUPPORT SETTER PROPERTY ISSELECTED, SO WE HAVE TO CAPTURE EACH SELECT/DESELECT HERE
                                        'WriteToDebug("RecordingListViewModel.RecordingSelectionChanged()", "start")
                                        ''Dim app As App = CType(Application.Current, App)
                                        ''If Not Me.items Is Nothing Then
                                        'For Each item In CType(x, SelectionChangedEventArgs).AddedItems
                                        '    If TypeOf (item) Is RecordingViewModel Then
                                        '        Dim recording = CType(item, RecordingViewModel)
                                        '        recording.IsSelected = True
                                        '    End If
                                        'Next
                                        'For Each item In CType(x, SelectionChangedEventArgs).RemovedItems
                                        '    If TypeOf (item) Is RecordingViewModel Then
                                        '        Dim recording = CType(item, RecordingViewModel)
                                        '        recording.IsSelected = False
                                        '    End If
                                        'Next
                                        ''If Me.items.Where(Function(y) y.IsSelected = True).Count > 0 Then
                                        ''    vm.AppBar.ButtonEnabled.deleteButton = True
                                        ''Else
                                        ''    vm.AppBar.ButtonEnabled.deleteButton = False
                                        ''End If

                                        'Dim amountSelected As Integer = 0
                                        'For Each g In groupeditems
                                        '    For Each i In g
                                        '        If i.IsSelected Then
                                        '            amountSelected += 1
                                        '            If amountSelected > 0 Then vm.AppBar.ButtonEnabled.deleteButton = True
                                        '        End If
                                        '    Next
                                        'Next
                                        'If amountSelected = 0 Then vm.AppBar.ButtonEnabled.deleteButton = False
                                        'WriteToDebug("RecordingListViewModel.RecordingSelectionChanged()", String.Format("{0} recordings selected", amountSelected.ToString))
                                        'WriteToDebug("RecordingListViewModel.RecordingSelectionChanged()", "Stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Async Function RemoveRecording(uuid As String, Optional fromComet As Boolean = False) As Task
        WriteToDebug("RecordingListViewModel.RemoveRecording()", Me.RecordingType.ToString() + "-start")
        Dim recordingToDelete As RecordingViewModel
        Dim groupIndex As Integer
        If Not Me.groupeditems Is Nothing AndAlso Me.groupeditems.Count > 0 Then
            For Each g In Me.groupeditems
                Dim tmpRecording As RecordingViewModel = (From rec In g Where rec.recording_id = uuid Select rec).FirstOrDefault()
                If Not tmpRecording Is Nothing Then
                    recordingToDelete = tmpRecording
                    groupIndex = groupeditems.IndexOf(g)
                End If
            Next

            If Not recordingToDelete Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 If fromComet Then
                                                                                                                     Dim strRecording As String = vm.loader.GetString("Recording")
                                                                                                                     Dim strDeleted As String = vm.loader.GetString("deleted")
                                                                                                                     vm.ToastMessages.AddMessage(New ToastMessageViewModel With {
                                                                                                                                                     .msg = strRecording + " """ + recordingToDelete.title + """ " + strDeleted,
                                                                                                                                                     .isGoing = False,
                                                                                                                                                     .secondsToShow = 2,
                                                                                                                                                     .isError = False})

                                                                                                                 End If
                                                                                                                 groupeditems(groupIndex).Remove(recordingToDelete)
                                                                                                             End Sub)
            End If
            If groupeditems(groupIndex).Count = 0 Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems.RemoveAt(groupIndex)
                                                                                                             End Sub)
            End If
        End If
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         If groupeditems.Count = 0 Then NoRecordingsAvailableVisibility = Visibility.Visible
                                                                                                     End Sub)

        WriteToDebug("RecordingListViewModel.RemoveRecording", Me.RecordingType.ToString() + "-stop")
    End Function

    Public Async Function UpdateRecording(updatedRecording As RecordingViewModel) As Task
        'WriteToDebug("RecordingListViewModel.UpdateRecording()", Me.RecordingType.ToString() + "start")
        Dim oldRecording As RecordingViewModel
        Dim dayGroupIndex As Integer
        If updatedRecording Is Nothing Then
            WriteToDebug("", "")
        End If
        Dim dayGroup = groupeditems.Where(Function(x) x.Key = updatedRecording.startDate.Date).FirstOrDefault()
        If Not dayGroup Is Nothing Then
            dayGroupIndex = groupeditems.IndexOf(dayGroup)
            oldRecording = (From r In groupeditems(dayGroupIndex) Where r.recording_id = updatedRecording.recording_id).FirstOrDefault()
            If Not oldRecording Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 If oldRecording.status <> updatedRecording.status Then
                                                                                                                     Dim strRecording As String = vm.loader.GetString("Recording")
                                                                                                                     Dim strDeleted As String = vm.loader.GetString("deleted")
                                                                                                                     vm.ToastMessages.AddMessage(New ToastMessageViewModel With {
                                                                                                                                                     .msg = strRecording + " """ + oldRecording.title + """ " + updatedRecording.status,
                                                                                                                                                     .isGoing = False,
                                                                                                                                                     .secondsToShow = 2,
                                                                                                                                                     .isError = False})
                                                                                                                 End If
                                                                                                                 oldRecording.status = updatedRecording.status
                                                                                                                 oldRecording.filesize = updatedRecording.filesize
                                                                                                                 oldRecording.schedstate = updatedRecording.schedstate
                                                                                                                 oldRecording.percentcompleted = updatedRecording.percentcompleted
                                                                                                             End Sub)
            Else
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                 Await AddRecording(updatedRecording, True)
                                                                                                             End Sub)

            End If
        End If
    End Function

    Public Async Function AddRecording(rec As RecordingViewModel, Optional fromComet As Boolean = False) As Task
        'WriteToDebug("RecordingListViewModel.AddRecording()", "start")
        'WriteToDebug("RecordingListViewModel.AddRecording()", SortingOrder)

        'FIND / INSERT THE GROUP IN WHICH THE RECORDING SHOULD BELONG
        Dim dayGroup = groupeditems.Where(Function(x) x.Key = rec.startDate.Date).FirstOrDefault()
        Dim dayGroupIndex As Integer
        If Not dayGroup Is Nothing Then
            dayGroupIndex = groupeditems.IndexOf(dayGroup)
        Else
            Select Case SortingOrder
                Case Ascending
                    dayGroup = groupeditems.Where(Function(x) x.Key > rec.startDate.Date).FirstOrDefault()
                    If Not dayGroup Is Nothing Then
                        'We found the first group which is > than the recording. Get the index and create a new group
                        dayGroupIndex = groupeditems.IndexOf(dayGroup)
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         groupeditems.Insert(dayGroupIndex, New Group(Of RecordingViewModel)(rec.startDate.Date, New ObservableCollection(Of RecordingViewModel)))
                                                                                                                     End Sub)

                    Else
                        '    Any group that does exist is < than the one we want to create, therefore just add a new group to the end.
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         groupeditems.Add(New Group(Of RecordingViewModel)(rec.startDate.Date, New ObservableCollection(Of RecordingViewModel)))
                                                                                                                     End Sub)


                    End If
                Case Descending
                    dayGroup = groupeditems.Where(Function(x) x.Key < rec.startDate.Date).OrderByDescending(Function(y) y.Key).FirstOrDefault()
                    If Not dayGroup Is Nothing Then
                        'We found the first group which is < than the recording. Get the index and create a new group
                        dayGroupIndex = groupeditems.IndexOf(dayGroup)
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         groupeditems.Insert(dayGroupIndex, New Group(Of RecordingViewModel)(rec.startDate.Date, New ObservableCollection(Of RecordingViewModel)))
                                                                                                                     End Sub)



                    Else
                        'Any group that does exist is < than the one we want to create, therefore just add a new group to the end.
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         groupeditems.Add(New Group(Of RecordingViewModel)(rec.startDate.Date, New ObservableCollection(Of RecordingViewModel)))
                                                                                                                     End Sub)



                    End If
            End Select
        End If

        'The daygroup should now exist, re-catch the Index 
        dayGroup = groupeditems.Where(Function(x) x.Key = rec.startDate.Date).FirstOrDefault()
        dayGroupIndex = groupeditems.IndexOf(dayGroup)

        'INSERT THE RECORDING
        Select Case SortingOrder
            Case Ascending
                Dim insertRecording = (groupeditems(dayGroupIndex).Where(Function(x) x.startDate > rec.startDate).OrderBy(Function(x) x.startDate)).FirstOrDefault()
                If Not insertRecording Is Nothing Then
                    'We found a recording that starts later than the one we want to insert. Use this recording's index to insert our new recording
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                     groupeditems(dayGroupIndex).Insert(groupeditems(dayGroupIndex).IndexOf(insertRecording), rec)
                                                                                                                 End Sub)


                Else
                    'No recording was found with a time > than the recording we want to add. Therefore just add our recording to the end
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                     groupeditems(dayGroupIndex).Add(rec)
                                                                                                                 End Sub)



                End If
            Case Descending
                Dim insertRecording = (groupeditems(dayGroupIndex).Where(Function(x) x.startDate < rec.startDate).OrderByDescending(Function(x) x.startDate)).FirstOrDefault()
                If Not insertRecording Is Nothing Then
                    'We found a recording that starts earlier than the one we want to insert. Use this recording's index to insert our new recording
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                     groupeditems(dayGroupIndex).Insert(groupeditems(dayGroupIndex).IndexOf(insertRecording), rec)
                                                                                                                 End Sub)
                Else
                    'No recording was found with a time < than the recording we want to add. Therefore just add our recording to the end
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                     groupeditems(dayGroupIndex).Add(rec)
                                                                                                                 End Sub)
                End If
        End Select
        If fromComet Then
            Dim strRecording As String = vm.loader.GetString("Recording")
            Dim strDeleted As String = vm.loader.GetString("deleted")
            vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.msg = strRecording + " """ + rec.title + """ " + rec.status,
                                                                            .isGoing = False,
                                                                            .secondsToShow = 2,
                                                                            .isError = False})
        End If
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         If groupeditems.Count = 0 Then NoRecordingsAvailableVisibility = Visibility.Visible Else NoRecordingsAvailableVisibility = Visibility.Collapsed
                                                                                                     End Sub)

        'WriteToDebug("AddUpdateRecordings()", "stop")
    End Function

    Public Async Function GetFlatList() As Task(Of List(Of RecordingViewModel))
        If Not groupeditems Is Nothing Then
            Return (From g In groupeditems From r In g Select r).ToList()
        End If
    End Function

    Public Async Function Load() As Task
        'First check if the account has access. If not, retrigger checking authentication
        Dim hasProperAccess As Boolean = False
        If Me.RecordingType = upcomingRecordings Or Me.RecordingType = finishedRecordings Then
            If vm.hasDVRAccess = False Then
                Await vm.checkDVRAccess()
            End If
            hasProperAccess = vm.hasDVRAccess
        End If
        If Me.RecordingType = failedRecordings Then
            If vm.hasFailedDVRAccess = False Then
                Await vm.checkFailedDVRAccess()
            End If
            hasProperAccess = vm.hasFailedDVRAccess
        End If

        If hasProperAccess Then
            If Me.RecordingType = upcomingRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingUpcomingRecordings"), True, 0, True)
            If Me.RecordingType = finishedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
            If Me.RecordingType = failedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFailedRecordings"), True, 0, True)

            Dim updatedRecordings As New List(Of RecordingViewModel)
            Try
                If Me.RecordingType = upcomingRecordings Then updatedRecordings = (Await LoadUpcomingRecordings()).ToList
                If Me.RecordingType = finishedRecordings Then updatedRecordings = (Await LoadFinishedRecordings()).ToList
                If Me.RecordingType = failedRecordings Then updatedRecordings = (Await LoadFailedRecordings()).ToList
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 groupeditems = (From item In updatedRecordings
                                                                                                                                 Group By Day = item.startDate.Date Into Group
                                                                                                                                 Select New Group(Of RecordingViewModel)(Day, Group)).ToObservableCollection()
                                                                                                             End Sub)
                dataLoaded = True
            Catch ex As Exception
                dataLoaded = False
            End Try
        End If
    End Function

    Public Async Function Reload(produceStatusUpdates As Boolean) As Task
        Dim hasProperAccess As Boolean = False
        If Me.RecordingType = upcomingRecordings Or Me.RecordingType = finishedRecordings Then
            If vm.hasDVRAccess = False Then
                Await vm.checkDVRAccess()
            End If

            hasProperAccess = vm.hasDVRAccess
        End If
        If Me.RecordingType = failedRecordings Then
            If vm.hasFailedDVRAccess = False Then
                Await vm.checkFailedDVRAccess()
            End If
            hasProperAccess = vm.hasFailedDVRAccess
        End If

        If hasProperAccess Then
            Try
                If Me.RecordingType = upcomingRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingUpcomingRecordings"), True, 0, True)
                If Me.RecordingType = finishedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
                If Me.RecordingType = failedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFailedRecordings"), True, 0, True)

                Dim updatedRecordings As New List(Of RecordingViewModel)
                If Me.RecordingType = upcomingRecordings Then updatedRecordings = (Await LoadUpcomingRecordings()).ToList
                If Me.RecordingType = finishedRecordings Then updatedRecordings = (Await LoadFinishedRecordings()).ToList
                If Me.RecordingType = failedRecordings Then updatedRecordings = (Await LoadFailedRecordings()).ToList
                Dim currentRecordings = (From g In groupeditems From r In g Select r).ToList()
                Dim recordingsToRemove = currentRecordings.Where(Function(p) Not updatedRecordings.Any(Function(p2) p2.recording_id = p.recording_id)).ToList()
                Dim recordingsToAdd = updatedRecordings.Where(Function(p) Not currentRecordings.Any(Function(p2) p2.recording_id = p.recording_id)).ToList()
                Dim recordingsToUpdate = updatedRecordings.Where(Function(p) currentRecordings.Any(Function(p2) p2.recording_id = p.recording_id)).ToList()


                If Not recordingsToAdd Is Nothing Then
                    For Each rec In recordingsToAdd
                        Await Me.AddRecording(rec, produceStatusUpdates)
                    Next
                End If

                If Not recordingsToUpdate Is Nothing Then
                    For Each rec In recordingsToUpdate
                        Await Me.UpdateRecording(rec)
                    Next

                End If
                For Each rec In recordingsToRemove
                    Await Me.RemoveRecording(rec.recording_id, produceStatusUpdates)
                Next

                'WriteToDebug("RecordingListViewModel.Reload()", "stop")

            Catch ex As Exception
                dataLoaded = False
            End Try
            Await vm.StatusBar.Clean()
        End If
    End Function

    Public Async Function AbortSelectedRecordings() As Task
        Dim loader As New Windows.ApplicationModel.Resources.ResourceLoader()
        'Dim settings As New AppSettings
        Dim ContinueWithDeletion As Boolean = False
        Dim succesfulDeletions As Integer = 0
        'Dim app As App = CType(Application.Current, App)

        If Not Me Is Nothing Then
            If vm.appSettings.ConfirmDeletion Then
                Dim amountOfDeletions As Integer = (Await Me.GetFlatList).Where(Function(p) p.IsSelected = True).Count
                Dim amountOfItems As Integer = (Await Me.GetFlatList).Count()
                Dim strheader As String
                Dim strMessage As String
                If Me.RecordingType = upcomingRecordings Then
                    strheader = loader.GetString("RecordingAbortMultipleHeader")
                    strMessage = String.Format(loader.GetString("RecordingAbortMultipleContent"),
                                                         amountOfDeletions,
                                                         amountOfItems)
                Else
                    strheader = loader.GetString("RecordingDeleteHeader")
                    strMessage = String.Format(loader.GetString("RecordingDeleteContent"),
                                                         amountOfDeletions,
                                                         amountOfItems)
                End If
                Dim msgBox As New MessageDialog(strMessage, strheader)
                msgBox.Commands.Add(New UICommand(loader.GetString("Yes"), Sub()
                                                                               ContinueWithDeletion = True
                                                                           End Sub))
                msgBox.Commands.Add(New UICommand(loader.GetString("No"), Sub()
                                                                              ContinueWithDeletion = False
                                                                          End Sub))

                Await msgBox.ShowAsync()
            Else
                ContinueWithDeletion = True
            End If
            If ContinueWithDeletion Then
                Dim recordingsToDelete As New List(Of RecordingViewModel)

                For Each group In Me.groupeditems
                    For Each recording In group.Where(Function(y) y.IsSelected)
                        recordingsToDelete.Add(recording)
                    Next
                Next
                For Each r In recordingsToDelete
                    Dim retValue As RecordingReturnValue = Await Task.Run(Function() r.RecordingAbort())
                    If retValue.tvhResponse.success = 1 Then
                        succesfulDeletions += 1
                    Else
                        Dim strheader As String = loader.GetString("RecordingDeleteErrorHeader")
                        Dim strMessage As String = String.Format(loader.GetString("RecordingDeleteErrorContent"),
                                                                 r.title)
                        Dim msgBox As New MessageDialog(strMessage, strheader)
                        msgBox.Commands.Add(New UICommand(loader.GetString("OK")))
                        'recording.IsSelected = False
                        Await msgBox.ShowAsync()
                    End If
                Next



            End If
            If Not vm.appSettings.LongPollingEnabled Then
                Await Me.Reload(True)
            End If
            'Initiate a refresh of the list by retrieving the latest recordings from the TVH server
            WriteToDebug("TVHead_ViewModel.AbortSelectedRecordings()", String.Format("DeleteSelectedRecordings - {0} items deleted...", succesfulDeletions.ToString))
        End If
        Me.MultiSelectMode = ListViewSelectionMode.None
        'End If
    End Function

    'Protected Overrides Sub Finalize()
    '    MyBase.Finalize()
    'End Sub
End Class

