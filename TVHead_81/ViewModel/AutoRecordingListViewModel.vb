Imports Newtonsoft.Json
Imports Windows.UI.Popups

Public Class AutoRecordingListViewModel
    Inherits RecordingListViewModel


    Public Sub New()
        aitems = New ObservableCollection(Of AutoRecordingViewModel)
        'MultiSelectMode = ListViewSelectionMode.Single
    End Sub

    Private Property _aitems As ObservableCollection(Of AutoRecordingViewModel)
    Public Property aitems As ObservableCollection(Of AutoRecordingViewModel)
        Get
            Return _aitems
        End Get
        Set(value As ObservableCollection(Of AutoRecordingViewModel))
            _aitems = value
            RaisePropertyChanged("items")
        End Set
    End Property

    Public Overloads Sub ClearSelections()
        If Not aitems Is Nothing Then
            For Each rec In aitems
                rec.IsExpanded = False
                rec.IsSelected = False
            Next
            ItemSelected = False
        End If
    End Sub

    Public Overloads Async Function Load() As Task
        WriteToDebug("AutoRecordingListViewModel.Load()", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasDVRAccess Then
            Me.aitems.Clear()
            Dim tvh40api As New api40
            Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)

            Dim result As New List(Of AutoRecordingViewModel)
            Dim json_result As String
            Try
                json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetAutoRecordings())).Content.ReadAsStringAsync
            Catch ex As Exception
                WriteToDebug("AutoRecordingListViewModel.Load()", ex.InnerException.ToString())
            End Try
            If Not json_result = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of TVHAutoRecordingList)(json_result)
                For Each retrievedAutoRecording In deserialized.entries.OrderBy(Function(x) x.title)
                    aitems.Add(New AutoRecordingViewModel(retrievedAutoRecording))
                Next
            End If
            Me.dataLoaded = True
            WriteToDebug("Modules.LoadAutoRecordings()", "stop")
        End If

    End Function

    Public Async Sub AddNewAutoRecording(sender As Object, e As RoutedEventArgs)
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim dc As New AutoRecordingEditViewModel(New AutoRecordingViewModel(New TVHAutoRecording))
        Dim cDialog As New ContentDialog With {.IsPrimaryButtonEnabled = True,
                                               .IsSecondaryButtonEnabled = True,
                                               .Style = CType(Application.Current.Resources("TVHeadContentDialog"), Style),
                                               .DataContext = dc,
                                               .FullSizeDesired = True,
                                               .ContentTemplate = CType(Application.Current.Resources("ContentDialogEditAutoRecording"), DataTemplate),
                                               .Title = vm.loader.GetString("RecordingConfiguration"),
                                               .PrimaryButtonText = vm.loader.GetString("Save"),
                                               .SecondaryButtonText = vm.loader.GetString("Cancel")}
        Dim r As ContentDialogResult = Await cDialog.ShowAsync()
        If r = ContentDialogResult.Primary Then
            Dim butje = CType(dc, AutoRecordingViewModel)
            Dim response = Await butje.Save()
            If response.success = 1 Then
                Await vm.Notify.Update(False, "Auto Recording created !", 1, False, 2)
                Await Load()
            Else
                Await vm.Notify.Update(True, "Error creating Auto Recording !", 2, False, 4)
            End If
        End If


    End Sub

    Public Overloads Async Function ShowConfirmDeleteAutoRecordingsPrompt(sender As Object, e As RoutedEventArgs) As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If vm.TVHeadSettings.ConfirmDeletion Then
            Dim amountOfDeletions As Integer = Me.aitems.Where(Function(p) p.IsSelected = True).Count
            Dim amountOfItems As Integer = Me.aitems.Count()
            Dim strheader As String = vm.loader.GetString("AutoRecordingDeleteHeader")
            Dim strMessage As String = String.Format(vm.loader.GetString("AutoRecordingDeleteContent"),
                                                         amountOfDeletions,
                                                         amountOfItems)
            Dim msgBox As New MessageDialog(strMessage, strheader)
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), New UICommandInvokedHandler(AddressOf DeleteSelectedAutoRecordings)))
            msgBox.Commands.Add(New UICommand(vm.loader.GetString("No"), New UICommandInvokedHandler(AddressOf CancelAutoRecordingListEditing)))
            Await msgBox.ShowAsync()
        Else
            Await DeleteSelectedAutoRecordings()
        End If
    End Function

    'Sets the recordinglist back to its normal state
    Public Sub CancelAutoRecordingListEditing()
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        ClearSelections()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()
    End Sub

    Public Async Function DeleteSelectedAutoRecordings() As Task
        WriteToDebug("RecordingListViewMode.DeleteSelectedAutoRecordings", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim errors, success As Integer
        For Each autorec In aitems.Where(Function(x) x.IsSelected)
            Dim tvhResponse As tvhCommandResponse = Await autorec.Delete()
            If tvhResponse.success Then success += 1 Else errors += 1
        Next
        If errors > 0 Then
            Await vm.Notify.Update(True, vm.loader.GetString("AutoRecordingDeleteFailure"), 2, False, 4)
        Else
            Await vm.Notify.Update(False, vm.loader.GetString("AutoRecordingDeleteSuccess"), 1, False, 4)
        End If
        ClearSelections()
        Await Me.Load()
        MultiSelectMode = ListViewSelectionMode.Single
        vm.SetApplicationBarButtons()
    End Function


    Public Async Sub EditAutoRecording(sender As Object, e As RoutedEventArgs)
        WriteToDebug("AutoRecordingListViewModel.EditAutoRecording()", "executed")
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim AutoRecordingToEdit As AutoRecordingViewModel = aitems.Where(Function(x) x.IsSelected).FirstOrDefault
        If Not AutoRecordingToEdit Is Nothing Then
            Dim dc As AutoRecordingEditViewModel = New AutoRecordingEditViewModel(AutoRecordingToEdit)
            Dim cDialog As New ContentDialog With {.IsPrimaryButtonEnabled = True,
                                               .IsSecondaryButtonEnabled = True,
                                               .Style = CType(Application.Current.Resources("TVHeadContentDialog"), Style),
                                               .DataContext = dc,
                                               .FullSizeDesired = True,
                                               .ContentTemplate = CType(Application.Current.Resources("ContentDialogEditAutoRecording"), DataTemplate),
                                               .Title = vm.loader.GetString("AutoRecordingConfiguration"),
                                               .PrimaryButtonText = vm.loader.GetString("Save"),
                                               .SecondaryButtonText = vm.loader.GetString("Cancel")}
            Dim r As ContentDialogResult = Await cDialog.ShowAsync()
            If r = ContentDialogResult.Primary Then
                Dim butje = CType(dc, AutoRecordingViewModel)
                Await butje.Save()
            End If

            Dim updatedAutoRecording As AutoRecordingViewModel = Await LoadIDNode(AutoRecordingToEdit.uuid, GetType(AutoRecordingViewModel))
            If Not updatedAutoRecording Is Nothing Then
                Dim index As Integer = aitems.IndexOf(AutoRecordingToEdit)
                aitems.RemoveAt(index)
                AddAutoRecording(updatedAutoRecording, index)
            End If
        End If



    End Sub

    Public Sub AddAutoRecording(autorec As AutoRecordingViewModel, Optional index As Integer = Nothing)
        If index = Nothing Or index > aitems.Count Then
            aitems.Add(autorec)
        Else
            aitems.Insert(index, autorec)
        End If

    End Sub

    Public Sub AutoRecording_Clicked(sender As Object, e As ItemClickEventArgs)
        WriteToDebug("AutoRecordingListViewModel.AutoRecording_Clicked()", "executed")
        Dim clickedAutoRecording As AutoRecordingViewModel = TryCast(e.ClickedItem, AutoRecordingViewModel)
        If Not clickedAutoRecording Is Nothing Then
            If Not MultiSelectMode = ListViewSelectionMode.Multiple Then
                clickedAutoRecording.IsSelected = True
                For Each r In aitems.Where(Function(x) x.uuid <> clickedAutoRecording.uuid)
                    r.IsSelected = False
                    'r.IsExpanded = False
                Next
            Else
                clickedAutoRecording.IsSelected = Not clickedAutoRecording.IsSelected
            End If
            ItemSelected = aitems.Any(Function(x) x.IsSelected)
        End If
    End Sub
End Class

