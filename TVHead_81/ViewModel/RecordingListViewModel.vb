Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json
Imports Windows.UI.Popups

Public Class RecordingListViewModel
    Inherits ViewModelBase

    Public Const upcomingRecordings = "upcomingRecordings"
    Public Const finishedRecordings = "finishedRecordings"
    Public Const failedRecordings = "failedRecordings"
    Public Const autoRecordings = "autoRecordings"
    Public Const Ascending = "Ascending"
    Public Const Descending = "Descending"
    'Public ReadOnly Property AbortSelectedRecordingButtonIsEnabled As Boolean
    '    Get
    '        If Not items Is Nothing AndAlso items.Where(Function(x) x.IsSelected).Count > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End Get
    'End Property
    'Public ReadOnly Property DeleteSelectedRecordingButtonIsEnabled As Boolean
    '    Get
    '        If Not items Is Nothing AndAlso items.Where(Function(x) x.IsSelected).Count > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End Get
    'End Property
    'Public ReadOnly Property StopSelectedRecordingButtonIsEnabled As Boolean
    '    Get
    '        If Not items Is Nothing AndAlso items.Where(Function(x) x.IsSelected).Count > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End Get
    'End Property

    Private Property _ItemSelected As Boolean
    Public Property ItemSelected As Boolean
        Get
            Return _ItemSelected
        End Get
        Set(value As Boolean)
            If _ItemSelected <> value Then
                _ItemSelected = value
                RaisePropertyChanged("ItemSelected")
            End If
        End Set
    End Property



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
        items = New ObservableCollection(Of RecordingViewModel)
        MultiSelectMode = ListViewSelectionMode.Single
    End Sub

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

    Public Async Sub AddNewRecording(sender As Object, e As RoutedEventArgs)
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim dc As New RecordingEditViewModel(New RecordingViewModel(New TVHRecording))
        'Dim dc As New AutoRecordingEditViewModel(New AutoRecordingViewModel(New TVHAutoRecording))
        Dim cDialog As New ContentDialog With {.IsPrimaryButtonEnabled = True,
                                               .IsSecondaryButtonEnabled = True,
                                               .Style = CType(Application.Current.Resources("TVHeadContentDialog"), Style),
                                               .DataContext = dc,
                                               .FullSizeDesired = True,
                                               .ContentTemplate = CType(Application.Current.Resources("ContentDialogAddRecording"), DataTemplate),
                                               .Title = vm.loader.GetString("RecordingConfiguration"),
                                               .PrimaryButtonText = vm.loader.GetString("Save"),
                                               .SecondaryButtonText = vm.loader.GetString("Cancel")}
        Dim r As ContentDialogResult = Await cDialog.ShowAsync()
        If r = ContentDialogResult.Primary Then
            Dim RecordingToSave As RecordingViewModel = CType(dc, RecordingViewModel)
            Dim response = Await RecordingToSave.Save()
            If response.success = 1 Then
                Await vm.Notify.Update(False, "Recording created !", 1, False, 2)
                Await Load()

            Else
                Await vm.Notify.Update(True, "Error creating Recording !", 2, False, 4)
            End If
            'Dim butje = CType(dc, AutoRecordingViewModel)
            'Dim response = Await butje.Save()
            'If response.success = 1 Then
            '    Await vm.Notify.Update(False, "Recording created !", 1, False, 2)
            '    Await Load()
            'Else
            '    Await vm.Notify.Update(True, "Error creating Recording !", 2, False, 4)
            'End If
        End If


    End Sub



    Public Async Function Load() As Task
        items.Clear()
        dataLoaded = False
        'First check if the account has access. If not, retrigger checking authentication
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
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
            Dim tvh40api = New api40

            Dim result As New List(Of RecordingViewModel)
            Dim json_result As String
            Try
                If Me.RecordingType = upcomingRecordings Then json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetUpcomingRecordings())).Content.ReadAsStringAsync
                If Me.RecordingType = finishedRecordings Then json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetFinishedRecordings())).Content.ReadAsStringAsync
                If Me.RecordingType = failedRecordings Then json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetFailedRecordings())).Content.ReadAsStringAsync
            Catch ex As Exception
                WriteToDebug("RecordingListViewModel.Load()", "error-stop")
                Throw New ArgumentException("Exception Occured")
            End Try
            If Not json_result = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of TVHRecordingList)(json_result)
                For Each f In deserialized.entries.OrderBy(Function(x) x.start)
                    Me.items.Add(New RecordingViewModel(f))
                Next
            End If
            Me.dataLoaded = True
            RaisePropertyChanged("groupeditems")
        End If
    End Function


    Public Async Function ShowConfirmDeletePrompt(sender As Object, e As RoutedEventArgs) As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If vm.TVHeadSettings.ConfirmDeletion Then
            Dim amountOfDeletions As Integer = Me.items.Where(Function(p) p.IsSelected = True).Count
            Dim amountOfItems As Integer = Me.items.Count()
            Dim strheader As String = vm.loader.GetString("RecordingDeleteMultipleHeader")
            Dim strMessage As String = String.Format(vm.loader.GetString("RecordingDeleteMultipleContent"),
                                                         amountOfDeletions,
                                                         amountOfItems)
            Dim msgBox As New MessageDialog(strMessage, strheader)
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), New UICommandInvokedHandler(AddressOf DeleteSelectedRecordings)))
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("No"), New UICommandInvokedHandler(AddressOf CancelRecordingListEditing)))
            Await msgBox.ShowAsync()
        Else
            Await StopSelectedRecordings()
        End If
    End Function

    Public Async Function ShowConfirmCancelPrompt(sender As Object, e As RoutedEventArgs) As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If vm.TVHeadSettings.ConfirmDeletion Then
            Dim amountOfCancellations As Integer = Me.items.Where(Function(p) p.IsSelected = True).Count
            Dim amountOfItems As Integer = Me.items.Count()
            Dim strheader As String = vm.loader.GetString("RecordingAbortMultipleHeader")
            Dim strMessage As String = String.Format(vm.loader.GetString("RecordingAbortMultipleContent"),
                                                         amountOfCancellations,
                                                         amountOfItems)

            Dim msgBox As New MessageDialog(strMessage, strheader)
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), New UICommandInvokedHandler(AddressOf AbortSelectedRecordings)))
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("No"), New UICommandInvokedHandler(AddressOf CancelRecordingListEditing)))
            Await msgBox.ShowAsync()
        Else
            Await StopSelectedRecordings()
        End If
    End Function

    Public Async Function ShowConfirmStopPrompt(sender As Object, e As RoutedEventArgs) As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If vm.TVHeadSettings.ConfirmDeletion Then

            Dim amountOfStops As Integer = Me.items.Where(Function(p) p.IsSelected = True).Count
            Dim amountOfItems As Integer = Me.items.Count()

            Dim strheader As String = vm.loader.GetString("RecordingStopMultipleHeader")
            Dim strMessage As String = String.Format(vm.loader.GetString("RecordingStopMultipleContent"),
                                                         amountOfStops,
                                                         amountOfItems)
            Dim msgBox As New MessageDialog(strMessage, strheader)
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), New UICommandInvokedHandler(AddressOf StopSelectedRecordings)))
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("No"), New UICommandInvokedHandler(AddressOf CancelRecordingListEditing)))
            Await msgBox.ShowAsync()
        Else
            Await StopSelectedRecordings()
        End If

    End Function

    'Sets the recordinglist back to its normal state
    Public Sub CancelRecordingListEditing()
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        ClearSelections()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()
    End Sub



    Public Async Function DeleteSelectedRecordings() As Task
        WriteToDebug("RecordingListViewMode.DeleteSelectedRecordings", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim errors, success As Integer
        For Each recording In items.Where(Function(x) x.IsSelected)
            Dim tvhResponse As tvhCommandResponse = Await recording.Delete()
            If tvhResponse.success Then success += 1 Else errors += 1
        Next
        If errors > 0 Then
            Await vm.Notify.Update(True, "Not all recordings could be deleted", 2, False, 4)
        Else
            Await vm.Notify.Update(False, "Selected recordings deleted", 1, False, 4)
        End If

        ClearSelections()
        Await Me.Load()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()

    End Function


    Public Async Function StopSelectedRecordings() As Task
        WriteToDebug("RecordingListViewMode.StopSelectedRecordings", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim errors, success As Integer
        For Each recording In items.Where(Function(x) x.IsSelected)
            Dim tvhResponse As tvhCommandResponse = Await recording.Cancel()
            If tvhResponse.success Then success += 1 Else errors += 1
        Next
        If errors > 0 Then
            Await vm.Notify.Update(True, "Not all recordings could be stopped", 2, False, 4)
        Else
            Await vm.Notify.Update(False, "Selected recordings stopped", 1, False, 4)
        End If
        'ClearSelections()
        Await Me.Load()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()

    End Function



    Public Async Function AbortSelectedRecordings() As Task
        WriteToDebug("RecordingListViewMode.AbortSelectedRecordings", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim errors, success As Integer
        For Each recording In items.Where(Function(x) x.IsSelected)
            Dim tvhResponse As tvhCommandResponse = Await recording.Abort()
            If tvhResponse.success Then success += 1 Else errors += 1
        Next
        If errors > 0 Then
            Await vm.Notify.Update(True, "Not all recordings could be aborted", 2, False, 4)
        Else
            Await vm.Notify.Update(False, "Selected recordings aborted", 1, False, 4)
        End If
        'ClearSelections()
        Await Me.Load()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()

    End Function

    'Clears any selected recordings
    Public Sub ClearSelections()
        If Not items Is Nothing Then
            For Each rec In items
                rec.IsExpanded = False
                rec.IsSelected = False
            Next
            ItemSelected = False
        End If
    End Sub

    ''' <summary>
    ''' Sets the IsSelected Property of the clicked Recording in the RecordingListView to True, or toggles it if the SelectionMode = multiple
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub Recording_Clicked(sender As Object, e As ItemClickEventArgs)
        WriteToDebug("RecordingListViewModel.Recording.Clicked", "executed")
        Dim clickedRecording As RecordingViewModel = TryCast(e.ClickedItem, RecordingViewModel)
        If Not clickedRecording Is Nothing Then
            If Not MultiSelectMode = ListViewSelectionMode.Multiple Then
                clickedRecording.IsSelected = True
                clickedRecording.IsExpanded = Not clickedRecording.IsExpanded
                'When we're not selecting multiple items, set the IsSelected for all other items to False
                For Each r In items.Where(Function(x) x.uuid <> clickedRecording.uuid)
                    r.IsSelected = False
                    r.IsExpanded = False
                Next
            Else
                clickedRecording.IsSelected = Not clickedRecording.IsSelected
            End If
            ItemSelected = items.Any(Function(x) x.IsSelected)
        End If
    End Sub
End Class

