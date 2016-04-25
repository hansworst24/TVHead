Imports System.Threading
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports Newtonsoft.Json
Imports TVHead_81.ViewModels
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports Windows.UI.Popups
Imports Windows.Web.Http
''' <summary>
''' This is the main ViewModel that spans the entire application.
''' </summary>
Public Class TVHead_ViewModel
    Inherits ViewModelBase

    'A Dummy Boolean value which is used as additional binding for ApplicationBarButtons which are x:Binded. 
    Public Property DummyBoolTrue As Boolean = True
    'Instance of TVHead_Settings which contains all configuration settings related to TV Head
    Public Property TVHeadSettings As New TVHead_Settings
    'Instance of NotificationViewModel which handles the creation of notifications towards the user
    Public Property Notify As New NotificationViewModel
    'Instance of a CometCatcher, which handles the periodic update of EPGItems as well as the longpolling mechanism to receive and process TVHeadend updates sent through comets
    Public Property CometCatcher As New CometCatcher
    'Instance of CometStatistics, which is mainly used for debugging purposes, to visualize how many comets were received through the CometCatcher
    Public Property CometStatistics As New CometStatsViewModel
    'Bool which triggers/tells if the side menu is visible or not
    Public Property MenuIsOpen As Boolean
        Get
            Return _MenuIsOpen
        End Get
        Set(value As Boolean)
            _MenuIsOpen = value
            RaisePropertyChanged("MenuIsOpen")
        End Set
    End Property
    Private Property _MenuIsOpen As Boolean
    'Reference to the selectedEPGItem
    Public Property selectedEPGItem As EPGItemViewModel
        Get
            Return _selectedEPGItem
        End Get
        Set(value As EPGItemViewModel)
            _selectedEPGItem = value
            RaisePropertyChanged("selectedEPGItem")
            RaisePropertyChanged("RecordSelectedEPGItemButtonEnabled")
        End Set
    End Property
    Private Property _selectedEPGItem As EPGItemViewModel

    'Public ReadOnly Property selectedRecordingList As RecordingListViewModel
    '    Get
    '        If SelectedPivotIndex = 2 Then Return UpcomingRecordings
    '        If SelectedPivotIndex = 3 Then Return FinishedRecordings
    '        If SelectedPivotIndex = 4 Then Return FailedRecordings
    '        If SelectedPivotIndex = 5 Then Return AutoRecordings
    '    End Get
    'End Property




    ''' <summary>
    ''' Follows the Index of the Pivot View used in PhoneView. Resets any selection or selectionmode to defaults
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedPivotIndex As Integer
        Get
            Return _SelectedPivotIndex
        End Get
        Set(value As Integer)
            _SelectedPivotIndex = value
            If Not SelectedChannel Is Nothing Then
                SelectedChannel.epgitems.ClearSelections()
            End If
            If Not selectedEPGItem Is Nothing Then selectedEPGItem = Nothing
            UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Single : UpcomingRecordings.ClearSelections()
            FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Single : FinishedRecordings.ClearSelections()
            FailedRecordings.MultiSelectMode = ListViewSelectionMode.Single : FailedRecordings.ClearSelections()
            AutoRecordings.MultiSelectMode = ListViewSelectionMode.Single : AutoRecordings.ClearSelections()
            RaisePropertyChanged("SelectedPivotIndex")
            RaisePropertyChanged("CommandBarButtonCollection")
        End Set
    End Property
    Private Property _SelectedPivotIndex As Integer

    Public ReadOnly Property CommandBarButtonCollection As DataTemplate
        Get
            Select Case SelectedPivotIndex
                Case 2
                    Select Case UpcomingRecordings.MultiSelectMode
                        Case ListViewSelectionMode.Single
                            Return CType(Application.Current.Resources("UpcomingRecordingListButtons"), DataTemplate)
                        Case ListViewSelectionMode.Multiple
                            Return CType(Application.Current.Resources("UpcomingRecordingListEditButtons"), DataTemplate)
                    End Select
                Case 3
                    Select Case FinishedRecordings.MultiSelectMode
                        Case ListViewSelectionMode.Single
                            Return CType(Application.Current.Resources("RecordingListButtons"), DataTemplate)
                        Case ListViewSelectionMode.Multiple
                            Return CType(Application.Current.Resources("FinishedRecordingListEditButtons"), DataTemplate)
                    End Select
                Case 4
                    Select Case FailedRecordings.MultiSelectMode
                        Case ListViewSelectionMode.Single
                            Return CType(Application.Current.Resources("RecordingListButtons"), DataTemplate)
                        Case ListViewSelectionMode.Multiple
                            Return CType(Application.Current.Resources("FailedRecordingListEditButtons"), DataTemplate)
                    End Select
                Case 5
                    Select Case AutoRecordings.MultiSelectMode
                        Case ListViewSelectionMode.Single
                            Return CType(Application.Current.Resources("AutoRecordingListButtons"), DataTemplate)
                        Case ListViewSelectionMode.Multiple
                            Return CType(Application.Current.Resources("AutoRecordingListEditButtons"), DataTemplate)
                    End Select
                Case Else
                    Return CType(Application.Current.Resources("CommandBarEPGItemSelectedButtons"), DataTemplate)
            End Select
        End Get

    End Property




    Public myCultureInfoHelper As CultureInfoHelper = New CultureInfoHelper


    Public ReadOnly Property RecordSelectedEPGItemButtonEnabled As Boolean
        Get
            If _selectedEPGItem Is Nothing Then Return False Else Return True
        End Get
    End Property


    Public Property SearchPage As New SearchPageViewModel

    Public loader As New Windows.ApplicationModel.Resources.ResourceLoader()

    Public timer As New DispatcherTimer

    Public doCatchComents As Boolean




#Region "Properties"

    'Private Property _selectedChannelTag As ChannelTagViewModel
    'Public Property selectedChannelTag As ChannelTagViewModel
    '    Get
    '        Return _selectedChannelTag
    '    End Get
    '    Set(value As ChannelTagViewModel)
    '        _selectedChannelTag = value
    '        RaisePropertyChanged("selectedChannelTag")
    '    End Set
    'End Property




    Public Property CometCatcherLastRun As DateTime

    Private Property _totalBytesReceived As Long
    Public Property totalBytesReceived As Long
        Get
            Return _totalBytesReceived
        End Get
        Set(value As Long)
            _totalBytesReceived = value
            If totalBytesReceivedUpdates > 5 Then
                totalBytesReceivedUpdates = 0
                RunOnUIThread(Sub()
                                  RaisePropertyChanged("totalBytesReceived")
                              End Sub)
            Else
                totalBytesReceivedUpdates += 1
            End If


        End Set
    End Property

    Public Property totalBytesReceivedUpdates As Integer
    Public Property TVHIPAddress As String
    Public Property TVHPort As String
    Public Property TVHUser As String
    Public Property TVHPass As String
    Public Property TVHVersion As String
    Public Property TVHVersionLong As String

    Public Const visible = "visible"
    Public Const collapsed = "collapsed"

    Public Property WaitingForDiskspaceUpdate As Boolean
        Get
            Return _WaitingForDiskspaceUpdate
        End Get
        Set(value As Boolean)
            _WaitingForDiskspaceUpdate = value
            RaisePropertyChanged("WaitingForDiskspaceUpdate")
        End Set
    End Property
    Private Property _WaitingForDiskspaceUpdate As Boolean


    Public Property ConnectedRotation As Integer
        Get
            Return _ConnectedRotation
        End Get
        Set(value As Integer)
            If _ConnectedRotation <= 170 Then _ConnectedRotation = _ConnectedRotation + 10 Else _ConnectedRotation = 0
            RaisePropertyChanged("ConnectedRotation")
        End Set
    End Property
    Private Property _ConnectedRotation As Integer

    'Public Property ToastMessages As New ToastListViewModel



    'Public Property appSettings As TVHead_Settings
    '    Get
    '        Return New TVHead_Settings
    '    End Get
    '    Set(value As TVHead_Settings)
    '        _appSettings = value
    '    End Set
    'End Property
    'Private Property _appSettings As TVHead_Settings


    Public Property logmessages As New LogViewModel


    Public Property FreeDiskSpace As String
        Get
            Return _FreeDiskSpace
        End Get
        Set(value As String)
            _FreeDiskSpace = value
            RaisePropertyChanged("FreeDiskSpace")
        End Set
    End Property
    Private Property _FreeDiskSpace As String

    Public Property TotalDiskSpace As String
        Get
            Return _TotalDiskSpace
        End Get
        Set(value As String)
            _TotalDiskSpace = value
            RaisePropertyChanged("TotalDiskSpace")
        End Set
    End Property
    Private Property _TotalDiskSpace As String


    Public Property FreeDiskSpacePercentage As Double
        Get
            Return _FreeDiskSpacePercentage
        End Get
        Set(value As Double)
            _FreeDiskSpacePercentage = value
            RaisePropertyChanged("FreeDiskSpacePercentage")
        End Set
    End Property
    Private Property _FreeDiskSpacePercentage As Double



    Public Property SelectedChannelIndex As Integer

    Public Property ChannelSelected As Boolean
        Get
            Return _ChannelSelected
        End Get
        Set(value As Boolean)
            _ChannelSelected = value
            RaisePropertyChanged("ChannelSelected")
        End Set
    End Property
    Private Property _ChannelSelected As Boolean

    Private Property _EPGInformationAvailable As Boolean
    Public Property EPGInformationAvailable As Boolean
        Get
            If ChannelSelected = False Then
                Return True
            Else
                Return _EPGInformationAvailable
            End If
        End Get
        Set(value As Boolean)
            _EPGInformationAvailable = value
            RaisePropertyChanged("EPGInformationAvailable")
        End Set
    End Property

    Private Property _SelectedChannel As ChannelViewModel
    Public Property SelectedChannel As ChannelViewModel
        Get
            Return _SelectedChannel
        End Get
        Set(value As ChannelViewModel)
            _SelectedChannel = value
            RaisePropertyChanged("SelectedChannel")
        End Set
    End Property

    Private Property _SearchText As String
    Public Property SearchText As String
        Get
            Return _SearchText
        End Get
        Set(value As String)
            _SearchText = value
            RaisePropertyChanged("SearchText")
        End Set
    End Property

    Public Property Streams As New StreamListViewModel
    Public Property Subscriptions As New SubscriptionListViewModel
    Public Property Services As New ServiceListViewModel
    Public Property Muxes As New MuxListViewModel
    Public Property UpcomingRecordings As New RecordingListViewModel With {.SortingOrder = .Ascending, .RecordingType = .upcomingRecordings}
    Public Property FinishedRecordings As New RecordingListViewModel With {.SortingOrder = .Descending, .RecordingType = .finishedRecordings}
    Public Property FailedRecordings As New RecordingListViewModel With {.SortingOrder = .Descending, .RecordingType = .failedRecordings}
    Public Property allEPGEvents As New List(Of EPGItemViewModel) 'Temp/Debug list not to be used (big memory)
    Public Property AutoRecordings As New AutoRecordingListViewModel With {.RecordingType = .autoRecordings}
    Public Property StatusBar As New StatusUpdateViewModel
    Public Property AllChannels As New ChannelListViewModel
    Public Property Channels As New ChannelListViewModel
    Public Property ChannelTags As New ChannelTagListViewModel

    Public Property supportedLanguages As New LanguageList






    'Private Property _StatusPivotSelectedIndex As Integer
    'Public Property StatusPivotSelectedIndex As Integer
    '    Get
    '        Return _StatusPivotSelectedIndex
    '    End Get
    '    Set(value As Integer)
    '        _StatusPivotSelectedIndex = value
    '        RaisePropertyChanged()
    '    End Set
    'End Property

    'Private Property _PivotSelectedIndex As Integer
    'Public Property PivotSelectedIndex As Integer
    '    Get
    '        Return _PivotSelectedIndex
    '    End Get
    '    Set(value As Integer)
    '        _PivotSelectedIndex = value
    '        RaisePropertyChanged("PivotSelectedIndex")
    '    End Set
    'End Property

    Private Property _CommandBarVisibility As Visibility
    Public Property CommandBarVisibility As Visibility
        Get
            Return _CommandBarVisibility
        End Get
        Set(value As Visibility)
            _CommandBarVisibility = value
            RaisePropertyChanged()
        End Set
    End Property

    Private Property _ChannelTagFlyoutIsOpen As Boolean
    Public Property ChannelTagFlyoutIsOpen As Boolean
        Get
            Return _ChannelTagFlyoutIsOpen
        End Get
        Set(value As Boolean)
            _ChannelTagFlyoutIsOpen = value
            RaisePropertyChanged()
        End Set
    End Property




    Public Property favouriteChannelTag As ChannelTagViewModel

    Public Property ContentTypes As New ContentTypeListViewModel
    Public Property Genres As New ContentTypeListViewModel

    Public Property DVRConfigs As New DVRConfigListViewModel

    Public Property AppBar As New ApplicationBar


    Public Class ApplicationBar
        Inherits ViewModelBase

        Private Property _CommandBarVisibility As Visibility
        Public Property CommandBarVisibility As Visibility
            Get
                Return _CommandBarVisibility
            End Get
            Set(value As Visibility)
                _CommandBarVisibility = value
                RaisePropertyChanged("CommandBarVisibility")
            End Set
        End Property

        Public Property ButtonVisibility As New ButtonsVisibility
        Public Property ButtonEnabled As New ButtonsEnabled

    End Class

    Public Class ButtonsEnabled
        Inherits ViewModelBase

        Public Sub New()
            refreshButton = True
        End Sub

        Private Property _deleteButton As Boolean
        Public Property deleteButton As Boolean
            Get
                Return _deleteButton
            End Get
            Set(value As Boolean)
                _deleteButton = value
                RaisePropertyChanged("deleteButton")
            End Set
        End Property

        Private Property _refreshButton As Boolean
        Public Property refreshButton As Boolean
            Get
                Return _refreshButton
            End Get
            Set(value As Boolean)
                _refreshButton = value
                RaisePropertyChanged("refreshButton")
            End Set
        End Property
    End Class

    Public Class ButtonsVisibility
        Inherits ViewModelBase

        Private Property _refreshButton As Visibility
        Public Property refreshButton As Visibility
            Get
                Return _refreshButton
            End Get
            Set(value As Visibility)
                _refreshButton = value
                RaisePropertyChanged("refreshButton")
            End Set
        End Property

        Private Property _recordingsButton As Visibility
        Public Property recordingsButton As Visibility
            Get
                Return _recordingsButton
            End Get
            Set(value As Visibility)
                _recordingsButton = value
                RaisePropertyChanged("recordingsButton")
            End Set
        End Property
        Private Property _epgButton As Visibility
        Public Property epgButton As Visibility
            Get
                Return _epgButton
            End Get
            Set(value As Visibility)
                _epgButton = value
                RaisePropertyChanged("epgButton")
            End Set
        End Property
        Private Property _tagsButton As Visibility
        Public Property tagsButton As Visibility
            Get
                Return _tagsButton
            End Get
            Set(value As Visibility)
                _tagsButton = value
                RaisePropertyChanged("tagsButton")
            End Set
        End Property
        Private Property _manageButton As Visibility
        Public Property manageButton As Visibility
            Get
                Return _manageButton
            End Get
            Set(value As Visibility)
                _manageButton = value
                RaisePropertyChanged("manageButton")
            End Set
        End Property
        Private Property _searchButton As Visibility
        Public Property searchButton As Visibility
            Get
                Return _searchButton
            End Get
            Set(value As Visibility)
                _searchButton = value
                RaisePropertyChanged("searchButton")
            End Set
        End Property
        Private Property _addButton As Visibility
        Public Property addButton As Visibility
            Get
                Return _addButton
            End Get
            Set(value As Visibility)
                _addButton = value
                RaisePropertyChanged("addButton")
            End Set
        End Property
        Private Property _deleteButton As Visibility
        Public Property deleteButton As Visibility
            Get
                Return _deleteButton
            End Get
            Set(value As Visibility)
                _deleteButton = value
                RaisePropertyChanged("deleteButton")
            End Set
        End Property
        Private Property _saveButton As Visibility
        Public Property saveButton As Visibility
            Get
                Return _saveButton
            End Get
            Set(value As Visibility)
                _saveButton = value
                RaisePropertyChanged("saveButton")
            End Set
        End Property
        Private Property _cancelButton As Visibility
        Public Property cancelButton As Visibility
            Get
                Return _cancelButton
            End Get
            Set(value As Visibility)
                _cancelButton = value
                RaisePropertyChanged("cancelButton")
            End Set
        End Property

        Public Sub New()
            deleteButton = Visibility.Collapsed
        End Sub

    End Class



#Region "RelayCommands"

    Public ReadOnly Property ChannelSelectedCommand As RelayCommand(Of Object)
        Get
            Return New RelayCommand(Of Object)(Async Sub(x)
                                                   WriteToDebug("TVHead_ViewModel.ChannelSelectedCommand()", "executed")
                                                   Dim new_selected_channel As ChannelViewModel = TryCast(x, ChannelViewModel)

                                                   'If Not SelectedChannel Is Nothing AndAlso new_selected_channel.uuid = SelectedChannel.uuid Then
                                                   '    For Each g In SelectedChannel.groupeditems
                                                   '        For Each e In g
                                                   '            e.IsSelected = False
                                                   '        Next
                                                   '    Next
                                                   '    SelectedChannel.IsExpanded = Not SelectedChannel.IsExpanded
                                                   '    SelectedChannel.IsSelected = True
                                                   '    selectedEPGItem = SelectedChannel.currentEPGItem
                                                   '    Exit Sub
                                                   'End If




                                                   'If Not SelectedChannel Is Nothing Then SelectedChannel.ClearAllButCurrent()
                                                   'If Not new_selected_channel Is Nothing Then
                                                   '    Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
                                                   '    For Each channel In Channels.items
                                                   '        If channel Is new_selected_channel Then
                                                   '            channel.IsSelected = True
                                                   '            If rectie.Width < 720 Then
                                                   '                channel.IsExpanded = Not channel.IsExpanded
                                                   '            Else
                                                   '                channel.IsExpanded = False
                                                   '            End If
                                                   '        Else
                                                   '            channel.epgItemsLoaded = False
                                                   '            channel.IsSelected = False
                                                   '            channel.IsExpanded = False
                                                   '        End If
                                                   '    Next
                                                   If Not new_selected_channel Is Nothing Then
                                                       Await Notify.Update(False, loader.GetString("status_RefreshingEPGEntries"), 1, False, 0)
                                                       Await new_selected_channel.LoadEPG()
                                                       If Not SelectedChannel Is Nothing Then SelectedChannel.epgitems.ClearAllButCurrent()
                                                       SelectedChannel = new_selected_channel
                                                       selectedEPGItem = new_selected_channel.epgitems.currentEPGItem
                                                       Notify.Clear()
                                                   End If
                                                   'End If
                                               End Sub)
        End Get
    End Property




    Public Sub ToggleMultiSelect(sender As Object, e As RoutedEventArgs)
        WriteToDebug("TVHead_ViewModel.ToggleMultiSelect", "executed")
        Select Case SelectedPivotIndex
            Case 2 : UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Multiple : UpcomingRecordings.ClearSelections()
            Case 3 : FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple : FinishedRecordings.ClearSelections()
            Case 4 : FailedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple : FailedRecordings.ClearSelections()
            Case 5 : AutoRecordings.MultiSelectMode = ListViewSelectionMode.Multiple : AutoRecordings.ClearSelections()
            Case Else : Return
        End Select
        RaisePropertyChanged("CommandBarButtonCollection")
    End Sub






    Public Async Sub ShowStreamStatus(sender As Object, e As RoutedEventArgs)
        WriteToDebug("TVHead_ViewMode.ShowHideStreamStatus", "executed")
        Dim cDialog As New ContentDialog
        cDialog.Style = CType(Application.Current.Resources("TVHeadContentDialog"), Style)
        If TVHeadSettings.UseDarkTheme Then cDialog.RequestedTheme = ElementTheme.Dark Else cDialog.RequestedTheme = ElementTheme.Light
        cDialog.ContentTemplate = CType(Application.Current.Resources("TVHStatus"), DataTemplate)
        cDialog.DataContext = Me
        Await cDialog.ShowAsync()

    End Sub




    'Public Sub ChannelEPGListSelectionChangedCommand(sender As Object, e As ItemClickEventArgs)
    '    'WriteToDebug("TVHead_ViewModel.EPGListSelectionChangedCommand()", "executed")
    '    'Dim ClickedEPGItem As EPGItemViewModel = TryCast(e.ClickedItem, EPGItemViewModel)
    '    'If Not ClickedEPGItem Is Nothing Then
    '    '    If ClickedEPGItem.EPGItemDetailsVisibility = "Visible" Then ClickedEPGItem.EPGItemDetailsVisibility = "Collapsed" Else ClickedEPGItem.EPGItemDetailsVisibility = "Visible"
    '    '    ClickedEPGItem.IsSelected = True
    '    '    selectedEPGItem = ClickedEPGItem
    '    'End If
    '    'If Not SelectedChannel Is Nothing Then
    '    '    For Each g In SelectedChannel.groupeditems
    '    '        For Each i In g
    '    '            If i.eventId <> ClickedEPGItem.eventId Then
    '    '                i.IsSelected = False
    '    '                If i.EPGItemDetailsVisibility = "Visible" Then i.EPGItemDetailsVisibility = "Collapsed"
    '    '            End If
    '    '        Next
    '    '    Next
    '    'End If


    '    'WriteToDebug("TVHead_ViewModel.EPGListSelectionChangedCommand()", "end")
    'End Sub



    Public ReadOnly Property MenuButtonClickedCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("TVHead_ViewModel.MenuButtonClickedCommand()", "executed")
                                        MenuIsOpen = Not MenuIsOpen
                                    End Sub)
        End Get
    End Property

    Public ReadOnly Property PageLoadedCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("TVHead_ViewModel.PageLoadedCommand()", "executed")
                                        Await Me.LoadDataAsync()

                                    End Sub)
        End Get
    End Property





    Public ReadOnly Property ChannelTagListSelectionChanged As RelayCommand(Of Object)
        Get
            Return New RelayCommand(Of Object)(Async Sub(x)
                                                   WriteToDebug("TVHead_ViewModel.ChannelTagListSelectionChanged()", "start")
                                                   Dim sChannelTag As ChannelTagViewModel = TryCast(x, ChannelTagViewModel)
                                                   If Not sChannelTag Is Nothing Then
                                                       For Each c In Channels.items
                                                           c.epgitems.ClearAllButCurrent()
                                                       Next
                                                       ChannelTags.selectedChannelTag = sChannelTag
                                                       Await Task.Run(Function() Channels.Load())
                                                       SelectedChannel = Nothing
                                                       selectedEPGItem = Nothing
                                                       'Await Channels.RefreshCurrentEvents()
                                                   End If
                                                   WriteToDebug("TVHead_ViewModel.ChannelTagListSelectionChanged()", "end")
                                                   Notify.Clear()
                                               End Sub)
        End Get
    End Property


    Public Property AddCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'Navigate Away to the Add AutoRecording Page
                                        'If PivotSelectedIndex = 5 Then
                                        '    selectedAutoRecording = New AutoRecordingViewModel
                                        '    Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        '    'If Not rootFrame.Navigate(GetType(AutoRecordingPage), selectedAutoRecording) Then
                                        '    '    Dim resources As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")
                                        '    '    Throw New Exception(resources.GetString("NavigationFailedExceptionMessage"))
                                        '    'End If
                                        'End If

                                        ''Navigate Away to the Add Recording Page
                                        'If PivotSelectedIndex = 2 Then
                                        '    Dim recording As New RecordingViewModel
                                        '    Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        '    'If Not rootFrame.Navigate(GetType(RecordingPage), recording) Then
                                        '    '    Dim resources As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")
                                        '    '    Throw New Exception(resources.GetString("NavigationFailedExceptionMessage"))
                                        '    'End If
                                        'End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property SearchTextChangedCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        Await Task.Delay(100)
                                        WriteToDebug("TVHead_ViewModel.SearchTextChangedCommand()", String.Format("SearchKeyUpCommand Executed : Text = {0}", SearchText))
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property



    Public Property SearchCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("TVHead_ViewModel.SearchCommand()", "SearchCommand Executed")
                                        'Me.StopRefresh()
                                        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        'If Not rootFrame.Navigate(GetType(SearchPage), Me.SearchPage) Then
                                        '    Throw New Exception("Failed to create initial page")
                                        'End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property GoToRecordingsCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'PivotSelectedIndex = 2
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property GoToChannelsCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'PivotSelectedIndex = 0
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Property StatusCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("TVHead_ViewModel.StatusCommand", "start")
                                        'Me.StopRefresh()
                                        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        'If Not rootFrame.Navigate(GetType(StatusPage)) Then
                                        '    Throw New Exception("Failed to create initial page")
                                        'End If
                                        WriteToDebug("TVHead_ViewModel.StatusCommand", "stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property AboutCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'WriteToDebug("TVHead_ViewModel.AboutCommand", "start")
                                        ''Me.StopRefresh()
                                        'Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        'If Not rootFrame.Navigate(GetType(AboutPage)) Then
                                        '    Throw New Exception("Failed to create initial page")
                                        'End If
                                        'WriteToDebug("TVHead_ViewModel.AboutCommand", "stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Property SettingsCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'Me.StopRefresh()
                                        'Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        'If Not rootFrame.Navigate(GetType(AppSettingsPage)) Then
                                        '    Throw New Exception("Failed to create initial page")
                                        'End If
                                        'WriteToDebug("TVHead_ViewModel.SettingsCommand()", "SettingsCommand Executed")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property selectedRecordings As New List(Of RecordingViewModel)

    Public Property selectedAutoRecording As AutoRecordingViewModel




    '    Public Property DeleteSelectedRecordings As RelayCommand
    '        Get
    '            Return New RelayCommand(Async Sub()
    '                                        'Handle the deletion of Autorecording Entries
    '                                        If SelectedPivotIndex = 5 Then
    '                                            Dim myList As New AutoRecordingListViewModel
    '                                            myList = AutoRecordings
    '                                            Dim succesfulDeletions As Integer = 0
    '                                            Dim ContinueWithDeletion As Boolean = False
    '                                            If Not myList Is Nothing Then
    '                                                If TVHeadSettings.ConfirmDeletion Then
    '                                                    Dim strheader As String = loader.GetString("AutoRecordingDeleteHeader")
    '                                                    Dim strMessage As String = String.Format(loader.GetString("AutoRecordingDeleteContent"),
    '                                                                                             myList.items.Where(Function(y) y.IsSelected).Count.ToString,
    '                                                                                             myList.items.Count)
    '                                                    Dim msgBox As New MessageDialog(strMessage, strheader)
    '                                                    msgBox.Commands.Add(New UICommand(loader.GetString("OK"), Sub()
    '                                                                                                                  ContinueWithDeletion = True
    '                                                                                                              End Sub))
    '                                                    msgBox.Commands.Add(New UICommand(loader.GetString("Cancel"), Sub()
    '                                                                                                                      ContinueWithDeletion = False
    '                                                                                                                  End Sub))

    '                                                    Await msgBox.ShowAsync()
    '                                                Else
    '                                                    ContinueWithDeletion = True
    '                                                End If

    '                                            End If
    '                                            If ContinueWithDeletion Then
    '                                                For Each recording In myList.items.Where(Function(y) y.IsSelected)
    '#If DEBUG Then
    '                                                    Dim retValue As RecordingReturnValue = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 1}}
    '#Else
    '                                                        Dim retValue As RecordingReturnValue = Await DeleteAutoRecording(recording.id)
    '#End If
    '                                                    If retValue.tvhResponse.success = 1 Then
    '                                                        succesfulDeletions += 1
    '                                                    Else
    '                                                        Dim strheader As String = loader.GetString("AutoRecordingDeleteErrorHeader")
    '                                                        Dim strMessage As String = String.Format(loader.GetString("AutoRecordingDeleteErrorContent"),
    '                                                                                                 recording.title)
    '                                                        Dim msgBox As New MessageDialog(strMessage, strheader)
    '                                                        msgBox.Commands.Add(New UICommand(loader.GetString("OK")))
    '                                                        recording.IsSelected = False
    '                                                        Await msgBox.ShowAsync()

    '                                                    End If
    '                                                Next
    '                                                'Reload the Autorecordings, and Upcoming Recordings
    '                                                myList.items = (Await LoadAutoRecordings()).ToObservableCollection()
    '                                                WriteToDebug("TVHead_ViewModel.DeleteSelectedRecordings()", String.Format("DeleteSelectedRecordings - {0} items deleted...", succesfulDeletions.ToString))
    '                                            End If
    '                                            myList.MultiSelectMode = ListViewSelectionMode.None
    '                                            AutoRecordings.SetExpanseCollapseEnabled(True)
    '                                            SetApplicationBarButtons()
    '                                        End If

    '                                        'Handle the deletion of Upcoming, failed or finished recordings
    '                                        If SelectedPivotIndex = 2 Or SelectedPivotIndex = 3 Or SelectedPivotIndex = 4 Then
    '                                            Dim myRecList As New RecordingListViewModel
    '                                            'Dirty way to identify in which RecordingViewmodel we're working
    '                                            If SelectedPivotIndex = 2 Then Await UpcomingRecordings.AbortSelectedRecordings()
    '                                            If SelectedPivotIndex = 3 Then Await FinishedRecordings.AbortSelectedRecordings()
    '                                            If SelectedPivotIndex = 4 Then Await FailedRecordings.AbortSelectedRecordings()
    '                                            SetApplicationBarButtons()
    '                                        End If
    '                                    End Sub)

    '        End Get
    '        Set(value As RelayCommand)
    '        End Set
    '    End Property


    Public Property SearchResults As New ObservableCollection(Of ChannelViewModel)

    ''' <summary>
    ''' Gets called through the refresh button on the status.xaml pages application bar
    ''' </summary>
    ''' <returns></returns>
    Public Property RefreshStatusCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        RefreshStatus()
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Property RefreshCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        'If Me.AppBar.ButtonEnabled.refreshButton = True Then
                                        '    Me.AppBar.ButtonEnabled.refreshButton = False
                                        'End If
                                        'If Me.PivotSelectedIndex = 0 And Not Me.Channels Is Nothing Then Await Me.Channels.RefreshCurrentEvents()
                                        'If Me.PivotSelectedIndex = 1 And Not Me.SelectedChannel Is Nothing Then Await Task.Run(Function() Me.SelectedChannel.RefreshEPG(True))
                                        'If Me.PivotSelectedIndex = 2 Then Await Task.Run(Function() Me.UpcomingRecordings.Reload(False))
                                        'If Me.PivotSelectedIndex = 3 Then Await Task.Run(Function() Me.FinishedRecordings.Reload(False))
                                        'If Me.PivotSelectedIndex = 4 Then Await Task.Run(Function() Me.FailedRecordings.Reload(False))
                                        'If Me.PivotSelectedIndex = 5 Then Await Task.Run(Function() Me.AutoRecordings.Reload())
                                        'Me.AppBar.ButtonEnabled.refreshButton = True

                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property






    'Public Property MultiSelectCommand As RelayCommand
    '    Get
    '        Return New RelayCommand(Sub()
    '                                    WriteToDebug("TVHead_ViewModel.MultiSelectCommand", "start")
    '                                    Select Case Me.SelectedPivotIndex
    '                                        Case 2 'When the view is on the Upcoming Recordings Listview
    '                                            If UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None Then
    '                                                UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
    '                                                SetApplicationBarButtons("manage")
    '                                            Else
    '                                                UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None
    '                                                SetApplicationBarButtons()
    '                                            End If
    '                                        Case 3 'When the view is on the Finished Recordings Listview
    '                                            If FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None Then
    '                                                FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
    '                                                SetApplicationBarButtons("manage")
    '                                            Else
    '                                                FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None
    '                                                SetApplicationBarButtons()
    '                                            End If
    '                                        Case 4 'When the view is on the Failed Recordings Listview
    '                                            If FailedRecordings.MultiSelectMode = ListViewSelectionMode.None Then
    '                                                FailedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
    '                                                SetApplicationBarButtons("manage")
    '                                            Else
    '                                                FailedRecordings.MultiSelectMode = ListViewSelectionMode.None
    '                                                SetApplicationBarButtons()
    '                                            End If
    '                                        Case 5 'When the view is on the AutoRecordings Listview
    '                                            If AutoRecordings.MultiSelectMode = ListViewSelectionMode.None Then
    '                                                AutoRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
    '                                                AutoRecordings.SetExpanseCollapseEnabled(False)
    '                                                SetApplicationBarButtons("manage")
    '                                            Else
    '                                                AutoRecordings.MultiSelectMode = ListViewSelectionMode.None
    '                                                AutoRecordings.SetExpanseCollapseEnabled(True)
    '                                                SetApplicationBarButtons()
    '                                            End If
    '                                    End Select

    '                                    WriteToDebug("TVHead_ViewModel.MultiSelectCommand", "stop")
    '                                End Sub)

    '    End Get
    '    Set(value As RelayCommand)
    '    End Set
    'End Property




    ''' <summary>
    ''' Executes when the pivot index in the PhoneView main layout is changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub PhoneViewPivotSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'When we change from one pivotitem to another, we want any possible multiselectmode that is active on any recordings list to be set to none.
        UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Single
        FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Single
        FailedRecordings.MultiSelectMode = ListViewSelectionMode.Single
        AutoRecordings.MultiSelectMode = ListViewSelectionMode.Single
        'Then we want to set the CommandBar's button's actions according to what needs to be available for the selected Pivot Index (view)
        SetApplicationBarButtons()
    End Sub



    'Public Property PivotSelectionChanged As RelayCommand
    '    Get
    '        Return New RelayCommand(Async Sub()

    '                                    'Dim app As App = CType(Application.Current, Application)
    '                                    WriteToDebug("TVHead_ViewModel.PivotSelectionChanged()", "PivotSelectionChanged - Executed")

    '                                    'Remove MultiSelect for all recording lists

    '                                    UpcomingRecordings.SetExpanseCollapseEnabled(True)
    '                                    FinishedRecordings.SetExpanseCollapseEnabled(True)
    '                                    FailedRecordings.SetExpanseCollapseEnabled(True)
    '                                    AutoRecordings.SetExpanseCollapseEnabled(True)
    '                                    SetApplicationBarButtons()

    '                                    'If Me.PivotSelectedIndex = 2 And Me.UpcomingRecordings.dataLoaded Then Await Me.UpcomingRecordings.Reload()
    '                                    'If Me.PivotSelectedIndex = 3 And Me.FinishedRecordings.dataLoaded Then Await Me.FinishedRecordings.Reload()
    '                                    'If Me.PivotSelectedIndex = 4 And Me.FailedRecordings.dataLoaded Then Await Me.FailedRecordings.Reload()
    '                                End Sub)

    '    End Get
    '    Set(value As RelayCommand)
    '    End Set
    'End Property


#End Region

#End Region


#Region "Methods"

    Public Sub SetApplicationBarButtons(Optional mode As String = "")
        RaisePropertyChanged("CommandBarButtonCollection")

    End Sub




    'Public Async Function LoadCurrentEPGEventForChannels(easyOnBandwidth As Boolean) As Task
    '    WriteToDebug("ChannelViewModel.LoadCurrentEPGEventForChannels()", "start")

    '    'We have a choice of retrieving all current EPG Events from the TVH server each time when doing a refresh, or only pull the current EPG event for each channel for which 
    '    'the percentcompleted = 1.
    '    'The latter will cause less bandwidth usage, but will not retreive any updates for the existing EPG event, for example when it has been changed through another app)
    '    'Alternatively we can combine currently scheduled recordings with the local EPG info on the phone, to only update Recording status and percentcompleted

    '    If Not easyOnBandwidth Then
    '        'Initial load of the current EPG Events for all channels.

    '        Dim currentEPGItems = Await LoadEPGEntry(New ChannelViewModel, False, AllChannels.items.Count)
    '        If Not currentEPGItems Is Nothing Then
    '            For Each c In Channels.items
    '                Dim newItemForChannel = (From p In currentEPGItems Where p.channelUuid = c.uuid Select p).OrderBy(Function(x) x.startDate).FirstOrDefault
    '                If Not newItemForChannel Is Nothing Then
    '                    Await c.RefreshCurrentEPGItem(newItemForChannel)
    '                    'Else
    '                    '    If c.epgitems.currentEPGItem Is Nothing Then c.epgitems.currentEPGItem = New EPGItemViewModel()
    '                    '    'c.AddEmptyEPGDetails()
    '                End If

    '            Next
    '        End If
    '    Else
    '        'Dim recordings = Await LoadUpcomingRecordings()

    '        For Each c In Channels.items
    '            If Not c.currentEPGItem Is Nothing Then
    '                If c.currentEPGItem.percentcompleted = 1 Then
    '                    Await c.RefreshCurrentEPGItem()
    '                    'Don't hammer the server too much during initial load, add a pause for each request
    '                    'Await Task.Delay(100)
    '                Else
    '                    If c.currentEPGItem.eventId <> 0 Then
    '                        'c.currentEPGItem.percentcompleted = 1
    '                    End If

    '                End If
    '            Else
    '                Await c.RefreshCurrentEPGItem()
    '            End If


    '        Next
    '    End If
    '    WriteToDebug("ChannelViewModel.LoadCurrentEPGEventForChannels()", "stop")
    '    'Me.Channels = (From c In AllChannels Where c.tags.ToList.IndexOf(selectedChannelTag.uuid) > -1 Select c Order By c.number).ToObservableCollection()
    'End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES THE SERVER INFORMATION
    ''' </summary>
    ''' <returns>SERVERINFOVIEWMODEL</returns>
    ''' <remarks></remarks>
    Public Async Function GetServerInfo() As Task(Of ServerInfoViewModel)
        Dim strURL = (New api40).apiGetServerInfo()
        Dim response As New HttpResponseMessage
        Dim json_result As String
        Try
            response = Await (New Downloader).DownloadJSON(strURL)
            If response.IsSuccessStatusCode Then
                json_result = Await response.Content.ReadAsStringAsync
                Dim deserialized = JsonConvert.DeserializeObject(Of TVHServerInfo)(json_result)
                Return New ServerInfoViewModel(deserialized)
            Else
                Throw New ArgumentException(response.ReasonPhrase)
            End If
        Catch ex As Exception
            Throw New ArgumentException(response.ReasonPhrase)
        End Try
    End Function

    Public Function IsRunningOnWideScreen() As Boolean
        Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
        If rectie.Width > 720 Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Async Function IsConnected(Optional showmessages As Boolean = False) As Task(Of Boolean)
        If showmessages Then Await Notify.Update(False, loader.GetString("status_Connecting"), 1, 0, 0)
        Try
            Dim sInfo As ServerInfoViewModel = Await GetServerInfo()
            If Not sInfo Is Nothing Then
                TVHeadSettings.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
                TVHVersion = sInfo.sw_version
                If showmessages Then Await Notify.Update(False, loader.GetString("status_Connected"), 1, 0, 0)
                Return True
            Else

                If showmessages Then Await Notify.Update(True, loader.GetString("status_ConnectionError"), 1, 0, 0)
                Return False
            End If
        Catch ex As Exception
            If showmessages Then Notify.Update(True, loader.GetString("status_ConnectionError"), 2, 0, 0)
            Return False
        End Try
    End Function

    'Public Async Function checkEPGAccess(Optional report As Boolean = False) As Task
    '    Dim epgAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetEPGEvents("", False, 1))
    '    If epgAccessResponse.IsSuccessStatusCode Then
    '        TVHeadSettings.hasEPGAccess = True
    '    Else
    '        TVHeadSettings.hasEPGAccess = False
    '        If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 2, .msg = "Error accessing EPG : " + epgAccessResponse.ReasonPhrase})
    '    End If

    'End Function

    'Public Async Function checkAdminAccess(Optional report As Boolean = False) As Task
    '    Dim adminAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetSubscriptions())
    '    If adminAccessResponse.IsSuccessStatusCode Then
    '        TVHeadSettings.hasAdminAccess = True
    '    Else
    '        TVHeadSettings.hasAdminAccess = False
    '        If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 2, .msg = "Error accessing Admin : " + adminAccessResponse.ReasonPhrase})
    '    End If

    'End Function

    'Public Async Function checkFailedDVRAccess(Optional report As Boolean = False) As Task
    '    Dim dvrAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetFailedRecordings())
    '    If dvrAccessResponse.IsSuccessStatusCode Then
    '        TVHeadSettings.hasFailedDVRAccess = True
    '    Else
    '        TVHeadSettings.hasFailedDVRAccess = False
    '        If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 5, .msg = "Error accessing Failed DVR : " + dvrAccessResponse.ReasonPhrase})
    '    End If

    'End Function


    'Public Async Function checkDVRAccess(Optional report As Boolean = False) As Task
    '    Dim dvrAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetUpcomingRecordings())
    '    If dvrAccessResponse.IsSuccessStatusCode Then
    '        TVHeadSettings.hasDVRAccess = True
    '    Else
    '        TVHeadSettings.hasDVRAccess = False
    '        If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 5, .msg = "Error accessing DVR : " + dvrAccessResponse.ReasonPhrase})
    '    End If

    'End Function


    Public Async Function checkCapabilities() As Task
        If Await IsConnected() Then
            If Not Me.AllChannels Is Nothing AndAlso Me.AllChannels.items.Count > 0 Then
                Dim testChannel As ChannelViewModel = CType(Await LoadIDNode(AllChannels.items(0).uuid, GetType(ChannelViewModel)), ChannelViewModel)
                If Not testChannel Is Nothing Then
                    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test channel " + testChannel.name)
                Else
                    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test channel ")
                End If
            End If
            If Not Me.UpcomingRecordings Is Nothing AndAlso Me.UpcomingRecordings.groupeditems.Count > 0 Then
                'Dim testRecording As RecordingViewModel = CType(Await LoadIDNode(Me.UpcomingRecordings.groupeditems(0)(0).uuid, New RecordingViewModel), RecordingViewModel)
                'If Not testRecording Is Nothing Then
                '    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test recording " + testRecording.title)
                'Else
                '    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test recording ")
                'End If
            End If
            If Not Me.AutoRecordings Is Nothing AndAlso Me.AutoRecordings.items.Count > 0 Then
                Dim testAutoRecording As AutoRecordingViewModel = CType(Await LoadIDNode(Me.AutoRecordings.items(0).uuid, GetType(AutoRecordingViewModel)), AutoRecordingViewModel)
                If Not testAutoRecording Is Nothing Then
                    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test auto recording " + testAutoRecording.title)
                Else
                    WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test auto recording ")
                End If
            End If
        End If
    End Function

    'Public Async Function checkAccess(Optional report As Boolean = False) As Task
    '    'Checks the capabilities and authorization of the TVH server and used account
    '    'Await checkEPGAccess(report)
    '    'Await checkDVRAccess(report)
    '    'Await checkFailedDVRAccess(report)
    '    'Await checkAdminAccess(report)

    'End Function

    Public Async Function RefreshDataAsync() As Task

    End Function


    Public Async Function LoadDataAsync() As Task
        'This method ensures all initial data is loaded into the ViewModel.
        WriteToDebug("TVHead_ViewModel.LoadDataAsync()", "executed")
        'First check if TVHead has valid server details to connect
        If Not TVHeadSettings.ContainsValidServerDetails Then
            Await Notify.Update(True, "Invalid Host/Port settings", 2, True, 2)
            Exit Function
        End If

        If Not Await IsConnected(True) Then Exit Function
        If Not Await TVHeadSettings.hasEPGAccess(True) Then Exit Function

        If Me.ChannelTags.dataLoaded = False Then Await Me.ChannelTags.Load()
        If Me.DVRConfigs.dataLoaded = False Then Await Me.DVRConfigs.Load()
        If Me.ContentTypes.dataLoaded = False Then Await Me.ContentTypes.Load()
        If Me.Channels.dataLoaded = False Then Await Me.Channels.Load()
        'Await Task.Delay(3000)
        'For Each c In Channels.items
        '    Await c.LoadEPG()
        'Next
        'Await Task.Delay(3000)
        'EPGRefresher.StartRefresh()


        If Await TVHeadSettings.hasAdminAccess Then
            Streams.items = (Await LoadStreams()).ToObservableCollection()
            Subscriptions.items = (Await LoadSubscriptions()).ToObservableCollection()
            Services.items = Await LoadServices()
            Muxes.items = (Await LoadMuxes()).ToObservableCollection()
            ' Else
            'vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 4, .msg = "You don't have Admin access to the TVH server with this account. Therefore you won't see Subscriptions / Stream updates."})
        End If

        'If Not Me.Channels.dataLoaded Then Await Me.Channels.LoadAll()
        'REFRESH EPG INFORMATION FOR THE SELECTED CHANNEL. REFRESH EPG INFORMATION IF THE CHANNELS HAVE ALREADY BEEN LOADED AND THE PIVOT IS ON THE CHANNELS PAGE
        'If Me.PivotSelectedIndex = 0 And Not Me.Channels Is Nothing And Me.Channels.dataLoaded = True Then
        '    Await StatusBar.Update(loader.GetString("status_RefreshingChannels"), True, 0, True)
        '    Await Channels.RefreshCurrentEvents()
        '    'For Each c In Channels.items
        '    '    Await c.RefreshCurrentEPGItem()
        '    '    'Await Task.Delay(1000)
        '    'Next

        'End If
        'Me.Channels.dataLoaded = True

        ''REFRESH EPG INFORMATION FOR THE SELECTED CHANNEL. REFRESH EPG INFORMATION IF THE CHANNELS HAVE ALREADY BEEN LOADED AND THE PIVOT IS ON THE CHANNELS PAGE
        'If Me.PivotSelectedIndex = 1 And Not Me.SelectedChannel Is Nothing Then
        '    Await StatusBar.Update(loader.GetString("status_RefreshingEPGEntries"), True, 0, True)
        '    Await Task.Run(Function() Me.SelectedChannel.RefreshEPG(False))
        'End If

        'TODO VALIDATE FOR DVR ACCESS
        If 1 = 1 Then
            'LOAD UPCOMING RECORDINGS, OR REFRESH IF THE PIVOT ON THE UPCOMINGS RECORDINGS PAGE
            If UpcomingRecordings.dataLoaded = False Then Await Me.UpcomingRecordings.Load()

        End If
        'LOAD FINISHED RECORDINGS, OR REFRESH IF THE PIVOT ON THE FINISHED RECORDINGS PAGE
        If FinishedRecordings.dataLoaded = False Then Await Me.FinishedRecordings.Load()
        'Await StatusBar.Update(loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
        'Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()

        'End Sub)


        ''LOAD FAILED RECORDINGS, OR REFRESH IF THE PIVOT ON THE FAILED RECORDINGS PAGE
        '''TODO : RE-ENABLE DVR STUFF
        If FailedRecordings.dataLoaded = False Then Await Me.FailedRecordings.Load()

        '''LOAD AUTO RECORDINGS, OR REFRESH IF THE PIVOT ON THE AUTO RECORDINGS PAGE
        'If AutoRecordings.dataLoaded = False Or Me.PivotSelectedIndex = 5 Then
        '    Await StatusBar.Update(loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
        Await Me.AutoRecordings.Load()
        'End If

        'End If

        Await Task.Delay(1000)
        CometCatcher.StartRefresh()
        'Await checkCapabilities()
        'Await StatusBar.Clean()
        Notify.Clear()
    End Function



    Public Async Function StartRecording(item As Object) As Task(Of RecordingReturnValue)
        Dim response As New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 2}}
        Dim createAutoRecording As Boolean = False
        Dim createSeriesRecording As Boolean = False
        Dim createSingleRecording As Boolean = False

        Dim epgitem As New EPGItemViewModel
        If TypeOf (item) Is EPGItemViewModel Then
            epgitem = item
        Else
            epgitem = CType(item, ChannelViewModel).epgitems.currentEPGItem
        End If


        'Show a MessageBox, asking for the user to confirm whether a single or an auto-recording / Series should be created

        If TVHeadSettings.ProposeAutoRecording Then
            Dim strMessage As String = ""
            Dim strheader As String = ""

            If epgitem.serieslinkId <> 0 Then
                'The EPG Item is identified as a series, propose to create a Series Recording
                strMessage = loader.GetString("RecordingProposeSeriesRecordingContent")
                strheader = loader.GetString("RecordingProposeSeriesRecordingHeader")
                Dim msgBox As New MessageDialog(strMessage, strheader)
                msgBox.Commands.Add(New UICommand(loader.GetString("Once"), Sub()
                                                                                createSingleRecording = True
                                                                            End Sub))
                msgBox.Commands.Add(New UICommand(loader.GetString("SeriesRecording"), Sub()
                                                                                           createSeriesRecording = True
                                                                                       End Sub))
                Await msgBox.ShowAsync()

            Else
                'Propose to create a auto-recording

                strMessage = loader.GetString("RecordingProposeAutoRecordingContent")
                strheader = loader.GetString("RecordingProposeAutoRecordingHeader")
                Dim msgBox As New MessageDialog(strMessage, strheader)
                msgBox.Commands.Add(New UICommand(loader.GetString("Once"), Sub()
                                                                                createSingleRecording = True
                                                                            End Sub))
                msgBox.Commands.Add(New UICommand(loader.GetString("AutoRecording"), Sub()
                                                                                         createAutoRecording = True
                                                                                     End Sub))
                Await msgBox.ShowAsync()
            End If


        End If
        If createAutoRecording Then
            WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, auto recording selected")
            response = Await RecordProgramBySeries(epgitem)
        End If
        If createSeriesRecording Then
            WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, Series recording selected")
            response = Await RecordProgramBySeries(epgitem)
        End If
        If createSingleRecording Then
            WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, single recording selected")
            response = Await RecordProgram(epgitem)
        End If
        Return response
    End Function

#End Region

#Region "Constructor"
    Public Sub New()
        SelectedPivotIndex = 0

    End Sub
#End Region

    'Public Async Sub StartRefresh()
    '    WriteToDebug("TVHead_ViewModel.StartRefresh()", "")
    '    If CometCatcher Is Nothing OrElse CometCatcher.IsCompleted Then
    '        tokenSource = New CancellationTokenSource
    '        ct = tokenSource.Token
    '        CometCatcher = Await Task.Factory.StartNew(Function() CatchComets(ct), ct)
    '    End If
    'End Sub

    'Public Sub StopRefresh()
    '    If ct.CanBeCanceled Then
    '        tokenSource.Cancel()
    '        'tokenSource.Dispose()
    '    End If

    '    WriteToDebug("TVHead_ViewModel.StopRefresh()", "")
    'End Sub

    Public Async Sub Cleanup()
        If Not Me.AllChannels Is Nothing Then Me.AllChannels.items.Clear()
        If Not Me.Channels Is Nothing Then Me.Channels.items.Clear()
        Me.SelectedChannel = Nothing
        'Me.ChannelEPGItems = Nothing
        Me.UpcomingRecordings = Nothing
        Me.FinishedRecordings = Nothing
        Me.FailedRecordings = Nothing
        Me.AutoRecordings = Nothing
    End Sub

    Public Async Sub RefreshStatus()
        'If Await IsConnected() Then
        '    If Not Me.TVHeadSettings.hasAdminAccess Then
        '        Await checkAdminAccess(True)
        '    End If
        '    If Me.TVHeadSettings.hasAdminAccess Then
        '        Await StatusBar.Update("Refreshing streams...", True, 0, True)
        '        Me.Streams.items = (Await LoadStreams()).ToObservableCollection()
        '        Await StatusBar.Update("Refreshing subscriptions...", True, 0, True)
        '        Me.Subscriptions.items = (Await LoadSubscriptions()).ToObservableCollection()
        '        Await StatusBar.Clean()

        '    End If
        'End If
    End Sub



    Public Async Sub RefreshMe()

        ' Await Me.LoadDataAsync()

        'WriteToDebug("TVHead_ViewModel.RefreshMe()", "start")
        'If Not app.isConnected Then
        '    Try
        '        StatusBar.Update("Connecting...", True, 0, True, False)

        '        Dim sInfo As ServerInfoViewModel = Await GetServerInfo()
        '        If Not sInfo Is Nothing Then
        '            app.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
        '            app.TVHVersion = sInfo.sw_version
        '            app.isConnected = True
        '        Else
        '            app.isConnected = False
        '        End If
        '    Catch ex As Exception
        '        app.isConnected = False
        '    End Try
        'End If

        'If app.isConnected Then
        '    If Me.AppBar.ButtonEnabled.refreshButton = True Then

        '        Me.AppBar.ButtonEnabled.refreshButton = False
        '        Select Case Me.PivotSelectedIndex
        '            Case 0
        '                'Dim strheader As String = loader.GetString("RefreshingChannels")
        '                StatusBar.Update(loader.GetString("status_RefreshingChannels"), True, 0, True)
        '                If AllChannels.items.Count = 0 Then
        '                    Await LoadAllChannels()

        '                End If
        '                If Me.Channels Is Nothing OrElse Me.Channels.Count = 0 Then
        '                    Await SetChannelTags()
        '                    If Not ChannelTags.items.Count = 0 Then
        '                        Me.Channels = AllChannels.items.Where(Function(x) x.tags.ToList.IndexOf(selectedChannelTag.uuid) > -1).ToList()
        '                    Else
        '                        Me.Channels = AllChannels.items
        '                    End If
        '                Else
        '                    'Await LoadAllChannels()
        '                    Await LoadCurrentEPGEventForChannels(True)

        '                End If
        '            Case 1
        '                StatusBar.Update(loader.GetString("status_RefreshingEPGEntries"), True, 0, True)
        '                If Not Me.SelectedChannel Is Nothing Then
        '                    Await Me.SelectedChannel.RefreshEPG()
        '                End If
        '            Case 2
        '                StatusBar.Update(loader.GetString("status_RefreshingUpcomingRecordings"), True, 0, True)
        '                Await UpcomingRecordings.RefreshRecordings((Await LoadUpcomingRecordings()).ToList())
        '            Case 3
        '                StatusBar.Update(loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
        '                Await FinishedRecordings.RefreshRecordings((Await LoadFinishedRecordings()).ToList())
        '            Case 4
        '                StatusBar.Update(loader.GetString("status_RefreshingFailedRecordings"), True, 0, True)
        '                Await FailedRecordings.RefreshRecordings((Await LoadFailedRecordings()).ToList())
        '            Case 5
        '                StatusBar.Update(loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
        '                Await AutoRecordings.RefreshAutoRecordings(True)
        '                'Await LoadAutoRecordings()


        '        End Select
        '        StatusBar.Clean()
        '        Me.AppBar.ButtonEnabled.refreshButton = True
        '        Debug.WriteLine("RefreshMe() ended...")
        '    End If
        'End If

        'WriteToDebug("TVHead_ViewModel.RefreshMe()", "stop")
    End Sub


End Class