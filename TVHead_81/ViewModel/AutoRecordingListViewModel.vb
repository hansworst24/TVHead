Imports TVHead_81.Common
Imports TVHead_81.ViewModels
Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports GalaSoft.MvvmLight.Command

Public Class AutoRecordingListViewModel
    Inherits ViewModelBase

    Public Sub New()
        items = New ObservableCollection(Of AutoRecordingViewModel)
    End Sub

    Public Property dataLoaded As Boolean
    'Public app As App = CType(Application.Current, Application)
    'Public vm As TVHead_ViewModel = vm

    '  Public Property vm As TVHead_ViewModel

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
        End Get

    End Property

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

    Private Property _items As ObservableCollection(Of AutoRecordingViewModel)
    Public Property items As ObservableCollection(Of AutoRecordingViewModel)
        Get
            Return _items
        End Get
        Set(value As ObservableCollection(Of AutoRecordingViewModel))
            _items = value
            RaisePropertyChanged("items")
            If value.Count = 0 Then NoRecordingsAvailableVisibility = Visibility.Visible Else NoRecordingsAvailableVisibility = Visibility.Collapsed
        End Set
    End Property
    'Public Property isEmpty As Boolean
    Public Property selected As AutoRecordingViewModel
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

    Public Property RecordingsSelectionChanged As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        ''                                            Dim a = vm.
                                        ''WINRT APPARENTLY DOESNT SUPPORT SETTER PROPERTY ISSELECTED, SO WE HAVE TO CAPTURE EACH SELECT/DESELECT HERE
                                        'WriteToDebug("AutoRecordingListViewModel.RecordingSelectionChanged()", "start")
                                        ''If Not Me.items Is Nothing Then
                                        'For Each item In CType(x, SelectionChangedEventArgs).AddedItems
                                        '    If TypeOf (item) Is AutoRecordingViewModel Then
                                        '        Dim recording = CType(item, AutoRecordingViewModel)
                                        '        recording.IsSelected = True
                                        '    End If
                                        'Next
                                        'For Each item In CType(x, SelectionChangedEventArgs).RemovedItems
                                        '    If TypeOf (item) Is AutoRecordingViewModel Then
                                        '        Dim recording = CType(item, AutoRecordingViewModel)
                                        '        recording.IsSelected = False
                                        '    End If
                                        'Next

                                        'Dim amountSelected As Integer = 0
                                        'For Each item In items
                                        '    If item.IsSelected Then
                                        '        amountSelected += 1
                                        '        If amountSelected > 0 Then vm.AppBar.ButtonEnabled.deleteButton = True
                                        '    End If
                                        'Next
                                        'If amountSelected = 0 Then vm.AppBar.ButtonEnabled.deleteButton = False
                                        'WriteToDebug("AutoRecordingListViewModel.RecordingSelectionChanged()", String.Format("{0} recordings selected", amountSelected.ToString))
                                        'WriteToDebug("AutoRecordingListViewModel.RecordingSelectionChanged()", "Stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Async Function Load() As Task
        If vm.TVHeadSettings.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.TVHeadSettings.hasDVRAccess Then
            Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                             items = (Await LoadAutoRecordings()).ToObservableCollection()
                                                                                                         End Sub)
            Await vm.StatusBar.Clean()

        End If

    End Function

    Public Async Function Reload() As Task
        If vm.TVHeadSettings.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.TVHeadSettings.hasDVRAccess Then
            Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
            Dim newItems = Await LoadAutoRecordings()

            Dim itemsToRemove = items.Where(Function(p) Not newItems.Any(Function(p2) p2.id = p.id)).ToList()
            Dim itemsToAdd = newItems.Where(Function(p) Not items.Any(Function(p2) p2.id = p.id)).ToList()
            Dim itemsToUpdate = newItems.Where(Function(p) items.Any(Function(p2) p2.id = p.id)).ToList()


            If Not itemsToRemove Is Nothing Then
                For Each item In itemsToRemove
                    Await DeleteAutoRecording(item.id)
                Next
            End If

            If Not itemsToUpdate Is Nothing Then
                For Each item In itemsToUpdate
                    Await UpdateAutoRecording(item)
                Next
            End If

            If Not itemsToAdd Is Nothing Then
                For Each item In itemsToAdd
                    Await AddAutoRecording(item)
                Next
            End If
            Await vm.StatusBar.Clean()
        End If
    End Function

    Public Async Function AddAutoRecording(item As AutoRecordingViewModel, Optional fromComet As Boolean = False) As Task
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         If fromComet Then
                                                                                                             Dim message As String = String.Format(vm.loader.GetString("AutoRecordingAdded"), item.title)
                                                                                                             vm.ToastMessages.AddMessage(New ToastMessageViewModel With {
                                                                                                                                                     .msg = message,
                                                                                                                                                     .isGoing = False,
                                                                                                                                                     .secondsToShow = 2,
                                                                                                                                                     .isError = False})

                                                                                                         End If
                                                                                                         items.Add(item)
                                                                                                         If items.Count = 0 Then NoRecordingsAvailableVisibility = Visibility.Visible Else NoRecordingsAvailableVisibility = Visibility.Collapsed
                                                                                                     End Sub)

    End Function

    Public Async Function UpdateAutoRecording(item As AutoRecordingViewModel, Optional fromComet As Boolean = False) As Task
        Dim currentAutoRecording As AutoRecordingViewModel = (From i In items Where i.id = item.id).FirstOrDefault()
        If Not currentAutoRecording Is Nothing Then
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             If fromComet Then
                                                                                                                 Dim message As String = String.Format(vm.loader.GetString("AutoRecordingUpdated"), item.title)
                                                                                                                 vm.ToastMessages.AddMessage(New ToastMessageViewModel With {
                                                                                                                                                     .msg = message,
                                                                                                                                                     .isGoing = False,
                                                                                                                                                     .secondsToShow = 2,
                                                                                                                                                     .isError = False})

                                                                                                             End If
                                                                                                             currentAutoRecording.Update(item)
                                                                                                         End Sub)
        End If
    End Function

    Public Async Function DeleteAutoRecording(id As String, Optional fromComet As Boolean = False) As Task
        Dim itemIndex As Integer = items.IndexOf(items.Where(Function(p) p.id = id).FirstOrDefault())
        If Not itemIndex < 0 Then
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             If fromComet Then
                                                                                                                 Dim message As String = String.Format(vm.loader.GetString("AutoRecordingDeleted"), items(itemIndex).title)
                                                                                                                 vm.ToastMessages.AddMessage(New ToastMessageViewModel With {
                                                                                                                                                     .msg = message,
                                                                                                                                                     .isGoing = False,
                                                                                                                                                     .secondsToShow = 2,
                                                                                                                                                     .isError = False})

                                                                                                             End If
                                                                                                             items.RemoveAt(itemIndex)
                                                                                                             If items.Count = 0 Then NoRecordingsAvailableVisibility = Visibility.Visible Else NoRecordingsAvailableVisibility = Visibility.Collapsed
                                                                                                         End Sub)

        End If

    End Function



    Public Async Function RefreshAutoRecordings(reload As Boolean) As Task
        Dim reloadedItems As IEnumerable(Of AutoRecordingViewModel) = Await LoadAutoRecordings()
        If Not reloadedItems Is Nothing Then

            If items Is Nothing Then
                items = reloadedItems.ToObservableCollection()
            Else
                'Add items that are not in the current list
                For Each i In reloadedItems
                    Dim myItem = items.Where(Function(x) x.id = i.id).FirstOrDefault()
                    If myItem Is Nothing Then
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         items.Add(i)
                                                                                                                     End Sub)
                    End If
                Next
                'Remove items that are not in the current list
                For i As Integer = items.Count - 1 To 0 Step -1
                    Dim index As Integer = i
                    Dim myItem As AutoRecordingViewModel = (From r In reloadedItems Where r.id = items(index).id Select r).FirstOrDefault()
                    If myItem Is Nothing Then

                        'item doesnt exist in newly retrieved list of autorecordings, remove it
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()

                                                                                                                         items.RemoveAt(index)
                                                                                                                     End Sub)


                    End If

                Next
                'Update items that are in the list
                For Each i In reloadedItems
                    Dim myItem = items.Where(Function(x) x.id = i.id).FirstOrDefault()
                    If Not myItem Is Nothing Then
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()

                                                                                                                         myItem.Update(i)
                                                                                                                     End Sub)

                    End If
                Next
            End If
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()

                                                                                                             If items.Count = 0 Then
                                                                                                                 Me.NoRecordingsAvailableVisibility = Visibility.Visible
                                                                                                             Else
                                                                                                                 Me.NoRecordingsAvailableVisibility = Visibility.Collapsed
                                                                                                             End If
                                                                                                         End Sub)

        End If



    End Function

    Public Sub SetExpanseCollapseEnabled(b As Boolean)
        If Not Me.items Is Nothing Then
            For Each r In Me.items
                r.ExpanseCollapseEnabled = b
            Next
        End If
    End Sub
End Class

