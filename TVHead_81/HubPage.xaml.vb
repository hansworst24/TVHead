Imports TVHead_81.Common
Imports TVHead_81.ViewModels



' The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

Public NotInheritable Class HubPage
    Inherits Page

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary
    Private ReadOnly _resourceLoader As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")

    Public searchStatus As IAsyncAction
    'Public ct As CancellationToken
    'Public tokenSource As New CancellationTokenSource()

    Public timer As New DispatcherTimer

    Dim app As App = CType(Application.Current, App)
    Dim vm As TVHead_ViewModel = app.DefaultViewModel

    ''' <summary>
    ''' A page that displays a grouped collection of items.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        AddHandler HardwareButtons.BackPressed, AddressOf HubBackPressed
        DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait
        NavigationCacheMode = NavigationCacheMode.Enabled

    End Sub

    ''' <summary>
    ''' Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
    ''' </summary>
    Public ReadOnly Property NavigationHelper As NavigationHelper
        Get
            Return _navigationHelper
        End Get
    End Property


    Public ReadOnly Property DefaultViewModel As ObservableDictionary
        Get
            Return _defaultViewModel
        End Get
    End Property

    ''' <summary>
    ''' Populates the page with content passed during navigation.  Any saved state is also
    ''' provided when recreating a page from a prior session.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>.
    ''' </param>
    ''' <param name="e">Event data that provides both the navigation parameter passed to
    ''' <see cref="Frame.Navigate"/> when this page was initially requested and
    ''' a dictionary of state preserved by this page during an earlier
    ''' session. The state will be null the first time a page is visited.</param>
    Private Sub NavigationHelper_LoadState(sender As Object, e As LoadStateEventArgs) Handles _navigationHelper.LoadState
        ' TODO: Create an appropriate data model for your problem domain to replace the sample data
    End Sub

    ''' <summary>
    ''' Preserves state associated with this page in case the application is suspended or the
    ''' page is discarded from the navigation cache.  Values must conform to the serialization
    ''' requirements of <see cref="SuspensionManager.SessionState"/>.
    ''' </summary>
    ''' <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
    ''' <param name="e">Event data that provides an empty dictionary to be populated with
    ''' serializable state.</param>
    Private Sub NavigationHelper_SaveState(sender As Object, e As SaveStateEventArgs) Handles _navigationHelper.SaveState
        ' TODO: Save the unique state of the page here.
        'vm.StopRefresh()
    End Sub



#Region "NavigationHelper registration"

    ''' <summary>
    ''' The methods provided in this section are simply used to allow
    ''' NavigationHelper to respond to the page's navigation methods.
    ''' <para>
    ''' Page specific logic should be placed in event handlers for the
    ''' <see cref="NavigationHelper.LoadState"/>
    ''' and <see cref="NavigationHelper.SaveState"/>.
    ''' The navigation parameter is available in the LoadState method
    ''' in addition to page state preserved during an earlier session.
    ''' </para>
    ''' </summary>
    ''' <param name="e">Event data that describes how this page was reached.</param>
    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedTo(e)
        'Loading of data happens here
        Dim s As New AppSettings
        Me.DataContext = app.DefaultViewModel
        If e.NavigationMode = NavigationMode.New Then
            If s.ServerIPSetting = "" Or s.ServerPortSetting = "" Then
                Await vm.StatusBar.Update("IP address and/or port not set. Go to settings !", False, 0, False, False)
                ' parameter
            Else
                App.DefaultViewModel.TVHIPAddress = s.ServerIPSetting
                App.DefaultViewModel.TVHPort = s.ServerPortSetting
                app.DefaultViewModel.TVHVersion = s.TVHVersion
                app.DefaultViewModel.TVHUser = s.UsernameSetting
                app.DefaultViewModel.TVHPass = s.PasswordSetting

                Await Task.Run(Function() vm.LoadDataAsync())
            End If
        End If

        If e.NavigationMode = NavigationMode.Back Then
            AddHandler HardwareButtons.BackPressed, AddressOf HubBackPressed
            'Reset the tests we've done to ensure access to the TVH server


            If s.ServerIPSetting = "" Or s.ServerPortSetting = "" Then
                Await vm.StatusBar.Update("IP address and/or port not set. Go to settings !", False, 0, False, False)
            ElseIf (app.DefaultViewModel.TVHIPAddress <> s.ServerIPSetting) Or
                   (app.DefaultViewModel.TVHPort <> s.ServerPortSetting) Or
                   (app.DefaultViewModel.TVHUser <> s.UsernameSetting) Or
                   (app.DefaultViewModel.TVHPass <> s.PasswordSetting) Then
                vm.hasEPGAccess = False
                vm.hasDVRAccess = False
                vm.hasAdminAccess = False
                vm.isConnected = False
                vm.doCatchComents = False
                app.DefaultViewModel.TVHIPAddress = s.ServerIPSetting
                app.DefaultViewModel.TVHPort = s.ServerPortSetting
                app.DefaultViewModel.TVHUser = s.UsernameSetting
                app.DefaultViewModel.TVHPass = s.PasswordSetting
                'Retrigger the loading of data by setting the dataLoaded property of each list to False. LoadDataSync() will then reload the data
                vm.logmessages.entries.Clear()
                vm.DVRConfigs.dataLoaded = False
                vm.AllChannels.dataLoaded = False
                vm.Channels.dataLoaded = False
                vm.Genres.dataLoaded = False
                vm.AllGenres.dataLoaded = False
                vm.ChannelTags.dataLoaded = False
                vm.UpcomingRecordings.dataLoaded = False
                vm.FinishedRecordings.dataLoaded = False
                vm.FailedRecordings.dataLoaded = False
                vm.AutoRecordings.dataLoaded = False
                vm.SelectedChannel = Nothing
                Await Task.Run(Function() vm.LoadDataAsync())
                vm.CatchCometsBoxID = ""
            Else
            End If
        End If
        If s.ServerIPSetting <> "" And s.ServerPortSetting <> "" And s.LongPollingEnabled Then vm.StartRefresh()

    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
        RemoveHandler HardwareButtons.BackPressed, AddressOf HubBackPressed

    End Sub

#End Region

    Private Sub HubBackPressed(sender As Object, e As BackPressedEventArgs)
        ''Override certain BackKeyPresses
        WriteToDebug("HubPage.OnBackPressed()", "start")

        If vm.UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Multiple Then
            vm.UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None
            vm.UpcomingRecordings.SetExpanseCollapseEnabled(True)
            vm.SetApplicationBarButtons()
            e.Handled = True
            Exit Sub
        End If
        If vm.FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple Then
            vm.FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None
            vm.FinishedRecordings.SetExpanseCollapseEnabled(True)
            vm.SetApplicationBarButtons()
            e.Handled = True
            Exit Sub
        End If
        If vm.FailedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple Then
            vm.FailedRecordings.MultiSelectMode = ListViewSelectionMode.None
            vm.FailedRecordings.SetExpanseCollapseEnabled(True)
            vm.SetApplicationBarButtons()
            e.Handled = True
            Exit Sub
        End If
        If vm.AutoRecordings.MultiSelectMode = ListViewSelectionMode.Multiple Then
            vm.AutoRecordings.MultiSelectMode = ListViewSelectionMode.None
            vm.AutoRecordings.SetExpanseCollapseEnabled(True)
            vm.SetApplicationBarButtons()
            e.Handled = True
            Exit Sub
        End If

        If vm.PivotSelectedIndex > 0 Then
            vm.PivotSelectedIndex -= 1
            e.Handled = True
            Exit Sub
        End If

        If vm.PivotSelectedIndex = 0 Then
            Application.Current.Exit()
        End If
        WriteToDebug("HubPage.OnBackPressed()", "stop")


    End Sub

    Public result As IEnumerable(Of ChannelViewModel)


End Class
