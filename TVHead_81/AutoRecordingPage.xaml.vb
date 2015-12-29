Imports TVHead_81.Common

Public NotInheritable Class AutoRecordingPage
    Inherits Page
    Dim app As App = CType(Application.Current, App)

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary

    Public Sub New()
        InitializeComponent()
        lstDVRConfig.ItemsSource = app.DefaultViewModel.DVRConfigs.items
        lstChannelTag.ItemsSource = app.DefaultViewModel.ChannelTags.items
        lstGenre.ItemsSource = app.DefaultViewModel.Genres.items
        AddHandler HardwareButtons.BackPressed, AddressOf AutoRecordingBackPressed
    End Sub

    ''' <summary>
    ''' Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
    ''' </summary>
    Public ReadOnly Property NavigationHelper As NavigationHelper
        Get
            Return _navigationHelper
        End Get
    End Property

    ''' <summary>
    ''' Gets the view model for this <see cref="Page"/>.
    ''' This can be changed to a strongly typed view model.
    ''' </summary>
    Public ReadOnly Property DefaultViewModel As ObservableDictionary
        Get
            Return _defaultViewModel
        End Get
    End Property

    ''' <summary>
    ''' Populates the page with content passed during navigation. Any saved state is also
    ''' provided when recreating a page from a prior session.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>.
    ''' </param>
    ''' <param name="e">Event data that provides both the navigation parameter passed to
    ''' <see cref="Frame.Navigate"/> when this page was initially requested and
    ''' a dictionary of state preserved by this page during an earlier.
    ''' session. The state will be null the first time a page is visited.</param>
    Private Sub NavigationHelper_LoadState(sender As Object, e As LoadStateEventArgs) Handles _navigationHelper.LoadState
        ' TODO: Create an appropriate data model for your problem domain to replace the sample data.
        DefaultViewModel("selectedAutoRecording") = DirectCast(e.NavigationParameter, AutoRecordingViewModel)

    End Sub

    ''' <summary>
    ''' Preserves state associated with this page in case the application is suspended or the
    ''' page is discarded from the navigation cache.  Values must conform to the serialization
    ''' requirements of <see cref="SuspensionManager.SessionState"/>.
    ''' </summary>
    ''' <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
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
    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedTo(e)
        Me.DataContext = app.DefaultViewModel
    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
        RemoveHandler HardwareButtons.BackPressed, AddressOf AutoRecordingBackPressed
    End Sub

#End Region

    Private Sub Flyout_Opened(sender As Object, e As Object)
        BottomAppBar.Visibility = Visibility.Collapsed

    End Sub

    Private Sub Flyout_Closed(sender As Object, e As Object)
        BottomAppBar.Visibility = Visibility.Visible

    End Sub

    Private Sub tbChannelSearch_KeyUp(sender As Object, e As KeyRoutedEventArgs)
        Dim app As App = CType(Application.Current, App)
        If Not tbChannelSearch.Text = "" Then
            lstChannels.ItemsSource = From c In app.DefaultViewModel.AllChannels.items Where c.name.ToUpper.StartsWith(tbChannelSearch.Text.ToUpper())
        End If

    End Sub

    Private Async Sub AutoRecordingBackPressed(sender As Object, e As BackPressedEventArgs)
        Await app.DefaultViewModel.AutoRecordings.Reload()
        Dim content = Window.Current.Content
        Dim frame = CType(content, Frame)
        If frame.CanGoBack Then frame.GoBack()
        e.Handled = True
    End Sub

End Class
