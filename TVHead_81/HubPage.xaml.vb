Imports TVHead_81.Common
Imports TVHead_81.ViewModels
Imports Windows.UI.Core



' The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

Public NotInheritable Class HubPage
    Inherits Page

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary
    Private ReadOnly _resourceLoader As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")

    Dim app As App = CType(Application.Current, App)
    Dim vm As TVHead_ViewModel = app.DefaultViewModel

    ''' <summary>
    ''' A page that displays a grouped collection of items.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
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
        AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf HubBackPressed
        'Loading of data happens here
        Me.DataContext = app.DefaultViewModel

        'Redirect to Settings page immediately when IP address or port are not set
        If vm.appSettings.ServerIPSetting = "" Or vm.appSettings.ServerPortSetting = "" Then
            Await vm.StatusBar.Update("IP address and/or port not set. Go to settings !", False, 0, False, False)
            Await Me.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub()
                                                                                            Me.Frame.Navigate(GetType(AppSettingsPage))
                                                                                        End Sub)
            Exit Sub
        End If

        If e.NavigationMode = NavigationMode.New Then
            app.DefaultViewModel.TVHIPAddress = vm.appSettings.ServerIPSetting
            app.DefaultViewModel.TVHPort = vm.appSettings.ServerPortSetting
            app.DefaultViewModel.TVHVersion = vm.appSettings.TVHVersion
            app.DefaultViewModel.TVHUser = vm.appSettings.UsernameSetting
            app.DefaultViewModel.TVHPass = vm.appSettings.PasswordSetting
            Await Task.Run(Function() vm.LoadDataAsync())
        End If

        If e.NavigationMode = NavigationMode.Back Then
            If (app.DefaultViewModel.TVHIPAddress <> vm.appSettings.ServerIPSetting) Or
                   (app.DefaultViewModel.TVHPort <> vm.appSettings.ServerPortSetting) Or
                   (app.DefaultViewModel.TVHUser <> vm.appSettings.UsernameSetting) Or
                   (app.DefaultViewModel.TVHPass <> vm.appSettings.PasswordSetting) Then
                'Set the apps variables to reflect the new app settings. Used to detect setting changes
                app.DefaultViewModel.TVHIPAddress = vm.appSettings.ServerIPSetting
                app.DefaultViewModel.TVHPort = vm.appSettings.ServerPortSetting
                app.DefaultViewModel.TVHUser = vm.appSettings.UsernameSetting
                app.DefaultViewModel.TVHPass = vm.appSettings.PasswordSetting
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
            End If
        End If
        If vm.appSettings.ServerIPSetting <> "" And vm.appSettings.ServerPortSetting <> "" And vm.appSettings.LongPollingEnabled Then vm.StartRefresh()

    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
        RemoveHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf HubBackPressed
    End Sub

#End Region

    Private Sub HubBackPressed(sender As Object, e As BackRequestedEventArgs)
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

        'If Pivot is on any EPG or Upcoming Recordings, or... go one pivotitem back
        If vm.PivotSelectedIndex > 0 Then
            vm.PivotSelectedIndex -= 1
            e.Handled = True
            Exit Sub
        End If
        'If Pivot is on Channels PivotItem, close the app
        If vm.PivotSelectedIndex = 0 Then
            Application.Current.Exit()
        End If
        WriteToDebug("HubPage.OnBackPressed()", "stop")
    End Sub
End Class
