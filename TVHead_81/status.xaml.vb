Imports TVHead_81.Common
Imports TVHead_81.ViewModels


' The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

Public NotInheritable Class StatusPage
    Inherits Page

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary
    Private ReadOnly _resourceLoader As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")

    ' Set a reference to the Public objects in app.xaml.vb
    Private vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel

    ''' <summary>
    ''' A page that displays a grouped collection of items.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        ' Hub is only supported in Portrait orientation
        DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait

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

        'End If
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
        ' vm.StopRefresh()
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
        DataContext = vm
        If Not vm.TVHeadSettings.hasAdminAccess Then
            Await vm.checkAdminAccess()
        End If
        If vm.TVHeadSettings.hasAdminAccess Then
            vm.Streams.items = (Await LoadStreams()).ToObservableCollection()
            vm.Subscriptions.items = (Await LoadSubscriptions()).ToObservableCollection()
            vm.Services.items = Await LoadServices()
            vm.Muxes.items = Await LoadMuxes()
        Else
            vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 4, .msg = "You don't have Admin access to the TVH server with this account. Therefore you won't see Subscriptions / Stream updates."})
        End If
        If vm.FreeDiskSpace = "" Then vm.WaitingForDiskspaceUpdate = True Else vm.WaitingForDiskspaceUpdate = False
        If Not vm.appSettings.LongPollingEnabled Then vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 4, .msg = "Auto Refresh is not enabled. Enable Auto Refresh to receive continuous updates for streams/subscriptions and log."})
    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
    End Sub

#End Region


End Class
