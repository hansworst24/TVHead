Imports TVHead_81.Common

Imports TVHead_81.ViewModels



' The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

Public NotInheritable Class AboutPage
    Inherits Page

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary
    Private ReadOnly _resourceLoader As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")

    ' Set a reference to the Public objects in app.xaml.vb
    Dim app As App = CType(Application.Current, App)

    ''' <summary>
    ''' A page that displays a grouped collection of items.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        AddHandler HardwareButtons.BackPressed, AddressOf OnBackPressed

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
        ' app.DefaultViewModel.StopRefresh()
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
        Me.DataContext = app.DefaultViewModel

    End Sub




    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
    End Sub

#End Region

    Private Sub OnBackPressed(sender As Object, e As BackPressedEventArgs)

    End Sub



End Class
