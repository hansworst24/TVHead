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


    Public Property items As ObservableCollection(Of RecordingViewModel)
        Get
            Return _items
        End Get
        Set(value As ObservableCollection(Of RecordingViewModel))
            _items = value
            RaisePropertyChanged("items")
            RaisePropertyChanged("groupeditems")
        End Set
    End Property
    Private Property _items As ObservableCollection(Of RecordingViewModel)





    Public Sub New()
        SortingOrder = Descending
        ' groupeditems = New ObservableCollection(Of Group(Of RecordingViewModel))
        'vm = vm
    End Sub

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
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
    Public ReadOnly Property groupeditems As ObservableCollection(Of Group(Of RecordingViewModel))
        Get
            If SortingOrder = Descending Then
                Return (From rec In items.OrderByDescending(Function(x) x.startDate)
                        Group By Day = rec.startDate.Date Into Group
                        Select New Group(Of RecordingViewModel)(Day, Group)).OrderByDescending(Function(x) x.Key).ToObservableCollection()

            Else
                Return (From rec In items.OrderBy(Function(x) x.startDate)
                        Group By Day = rec.startDate.Date Into Group
                        Select New Group(Of RecordingViewModel)(Day, Group)).OrderBy(Function(x) x.Key).ToObservableCollection()

            End If
        End Get
    End Property

    Public Property RecordingsSelectionChanged As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        ''WINRT APPARENTLY DOESNT SUPPORT SETTER PROPERTY ISSELECTED, SO WE HAVE TO CAPTURE EACH SELECT/DESELECT HERE
                                        'WriteToDebug("RecordingListViewModel.RecordingSelectionChanged()", "start")
                                        ''Dim app As App = CType(Application.Current, Application)
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
        WriteToDebug("RecordingListViewModel.RemoveRecording()", "executed")
        Dim recToRemove As RecordingViewModel = (From rec In items Where rec.uuid = uuid Select rec).FirstOrDefault()
        If Not recToRemove Is Nothing Then
            RunOnUIThread(Sub()
                              items.Remove(recToRemove)
                              RaisePropertyChanged("groupeditems")
                          End Sub)
        End If
    End Function

    Public Async Function UpdateRecording(updatedRecording As RecordingViewModel) As Task
        WriteToDebug("RecordingListViewModel.UpdateRecording()", "executed")
        Dim oldRecording As RecordingViewModel
        Dim dayGroupIndex As Integer
        If updatedRecording Is Nothing Then
            WriteToDebug("", "")
        End If
        oldRecording = (From r In items Where r.uuid = updatedRecording.uuid Select r).FirstOrDefault()
        If Not oldRecording Is Nothing Then
            oldRecording.Update(updatedRecording)
        Else
            Await AddRecording(updatedRecording, True)

        End If
    End Function

    Public Async Function AddRecording(rec As RecordingViewModel, Optional fromComet As Boolean = False) As Task
        WriteToDebug("RecordingListViewModel.AddRecording()", "executed")
        RunOnUIThread(Sub()
                          items.Add(rec)
                          RaisePropertyChanged("groupeditems")
                      End Sub)


    End Function

    'Public Async Function GetFlatList() As Task(Of List(Of RecordingViewModel))
    '    If Not groupeditems Is Nothing Then
    '        Return (From g In groupeditems From r In g Select r).ToList()
    '    End If
    'End Function

    Public Async Function Load() As Task
        'First check if the account has access. If not, retrigger checking authentication
        Dim hasProperAccess As Boolean = False
        If Me.RecordingType = upcomingRecordings Or Me.RecordingType = finishedRecordings Then
            hasProperAccess = Await vm.TVHeadSettings.hasDVRAccess
        End If
        If Me.RecordingType = failedRecordings Then
            hasProperAccess = Await vm.TVHeadSettings.hasFailedDVRAccess
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
                RunOnUIThread(Sub()
                                  items = updatedRecordings.ToObservableCollection()
                                  'groupeditems = (From item In updatedRecordings
                                  '                Group By Day = item.startDate.Date Into Group
                                  '                Select New Group(Of RecordingViewModel)(Day, Group)).ToObservableCollection()
                              End Sub)
                dataLoaded = True
            Catch ex As Exception
                dataLoaded = False
            End Try
        End If
    End Function

    'Public Async Function Reload(produceStatusUpdates As Boolean) As Task
    '    Dim hasProperAccess As Boolean = False
    '    If Me.RecordingType = upcomingRecordings Or Me.RecordingType = finishedRecordings Then
    '        hasProperAccess = Await vm.TVHeadSettings.hasDVRAccess
    '    End If
    '    If Me.RecordingType = failedRecordings Then
    '        hasProperAccess = Await vm.TVHeadSettings.hasFailedDVRAccess
    '    End If

    '    If hasProperAccess Then
    '        Try
    '            If Me.RecordingType = upcomingRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingUpcomingRecordings"), True, 0, True)
    '            If Me.RecordingType = finishedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
    '            If Me.RecordingType = failedRecordings Then Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingFailedRecordings"), True, 0, True)

    '            Dim updatedRecordings As New List(Of RecordingViewModel)
    '            If Me.RecordingType = upcomingRecordings Then updatedRecordings = (Await LoadUpcomingRecordings()).ToList
    '            If Me.RecordingType = finishedRecordings Then updatedRecordings = (Await LoadFinishedRecordings()).ToList
    '            If Me.RecordingType = failedRecordings Then updatedRecordings = (Await LoadFailedRecordings()).ToList
    '            Dim currentRecordings = (From g In groupeditems From r In g Select r).ToList()
    '            Dim recordingsToRemove = currentRecordings.Where(Function(p) Not updatedRecordings.Any(Function(p2) p2.uuid = p.uuid)).ToList()
    '            Dim recordingsToAdd = updatedRecordings.Where(Function(p) Not currentRecordings.Any(Function(p2) p2.uuid = p.uuid)).ToList()
    '            Dim recordingsToUpdate = updatedRecordings.Where(Function(p) currentRecordings.Any(Function(p2) p2.uuid = p.uuid)).ToList()


    '            If Not recordingsToAdd Is Nothing Then
    '                For Each rec In recordingsToAdd
    '                    Await Me.AddRecording(rec, produceStatusUpdates)
    '                Next
    '            End If

    '            If Not recordingsToUpdate Is Nothing Then
    '                For Each rec In recordingsToUpdate
    '                    Await Me.UpdateRecording(rec)
    '                Next

    '            End If
    '            For Each rec In recordingsToRemove
    '                Await Me.RemoveRecording(rec.uuid, produceStatusUpdates)
    '            Next

    '            'WriteToDebug("RecordingListViewModel.Reload()", "stop")

    '        Catch ex As Exception
    '            dataLoaded = False
    '        End Try
    '        Await vm.StatusBar.Clean()
    '    End If
    'End Function

    Public Async Function AbortSelectedRecordings() As Task
        Dim loader As New Windows.ApplicationModel.Resources.ResourceLoader()
        Dim ContinueWithDeletion As Boolean = False
        Dim succesfulDeletions As Integer = 0

        If Not Me Is Nothing Then
            If vm.TVHeadSettings.ConfirmDeletion Then
                Dim amountOfDeletions As Integer = Me.items.Where(Function(p) p.IsSelected = True).Count
                Dim amountOfItems As Integer = Me.items.Count()
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
                    Dim retValue As RecordingReturnValue = Await Task.Run(Function() r.Abort())
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
            If Not vm.TVHeadSettings.LongPollingEnabled Then
                'Await Me.Reload(True)
            End If
            'Initiate a refresh of the list by retrieving the latest recordings from the TVH server
            WriteToDebug("TVHead_ViewModel.AbortSelectedRecordings()", String.Format("DeleteSelectedRecordings - {0} items deleted...", succesfulDeletions.ToString))
        End If
        Me.MultiSelectMode = ListViewSelectionMode.None
        'End If
    End Function
End Class

