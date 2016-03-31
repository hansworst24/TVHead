Imports TVHead_81.Common
Imports TVHead_81.ViewModels

' The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Class Application
    Inherits Windows.UI.Xaml.Application
    Private _transitions As TransitionCollection
    Public DefaultViewModel As New TVHead_ViewModel


    ''' <summary>
    ''' Initializes the singleton application object. This is the first line of authored code
    ''' executed, and as such is the logical equivalent of main() or WinMain().
    ''' </summary>
    Public Sub New()
        'Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
        '    Microsoft.ApplicationInsights.WindowsCollectors.Metadata Or
        '    Microsoft.ApplicationInsights.WindowsCollectors.Session)
        InitializeComponent()
        AddHandler Application.Current.Resuming, AddressOf App_Resuming
    End Sub
    ''' <summary>
    ''' Reloads or loads the data of the app when the app is resuming
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub App_Resuming(sender As Object, e As Object)
        WriteToDebug("App.App_Resuming()", "launched")

        'Check and set access flags to TVH server
        'Await Me.DefaultViewModel.checkAccess()

        'If Await Me.DefaultViewModel.TVHeadSettings.hasEPGAccess Then
        '    If Not Me.DefaultViewModel.SelectedChannel Is Nothing Then Await Me.DefaultViewModel.SelectedChannel.RefreshEPG(True)
        '    Await Me.DefaultViewModel.Channels.RefreshCurrentEvents()
        'End If

        'If Me.DefaultViewModel.TVHeadSettings.hasDVRAccess Then
        '    Await Me.DefaultViewModel.UpcomingRecordings.Reload(False)
        '    Await Me.DefaultViewModel.FinishedRecordings.Reload(False)
        '    Await Me.DefaultViewModel.FailedRecordings.Reload(False)
        'End If

        'If Me.DefaultViewModel.TVHeadSettings.hasAdminAccess Then
        '    Await Me.DefaultViewModel.Streams.Reload()
        '    Await Me.DefaultViewModel.Subscriptions.Reload()
        'End If

        'If Me.DefaultViewModel.appSettings.LongPollingEnabled Then
        '    Me.DefaultViewModel.CatchCometsBoxID = Await GetBoxID()
        '    Me.DefaultViewModel.doCatchComents = True
        'End If

    End Sub


    ''' <summary>
    ''' Invoked when the application is launched normally by the end user. Other entry points
    ''' will be used when the application is launched to open a specific file, to display
    ''' search results, and so forth.
    ''' </summary>
    ''' <param name="e">Details about the launch request and process.</param>
    Protected Overrides Async Sub OnLaunched(e As LaunchActivatedEventArgs)
#If DEBUG Then
        If System.Diagnostics.Debugger.IsAttached Then
            DebugSettings.EnableFrameRateCounter = True
        End If
#End If

        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' Do not repeat app initialization when the Window already has content,
        ' just ensure that the window is active
        If rootFrame Is Nothing Then
            ' Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = New Frame()

            ' TODO: change this value to a cache size that is appropriate for your application
            rootFrame.CacheSize = 1

            'Set the Theme
            If DefaultViewModel.TVHeadSettings.UseDarkTheme Then rootFrame.RequestedTheme = ElementTheme.Dark Else rootFrame.RequestedTheme = ElementTheme.Light

            ' Set the default language
            rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages(0)

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Load state from previously suspended application
                Try
                    Await SuspensionManager.RestoreAsync()
                Catch ex As SuspensionManagerException
                    ' Something went wrong restoring state.
                    ' Assume there is no state and continue.
                End Try
            End If

            ' Place the frame in the current Window
            Window.Current.Content = rootFrame
        End If

        If rootFrame.Content Is Nothing Then
            ' Removes the turnstile navigation for startup.
            If rootFrame.ContentTransitions IsNot Nothing Then
                _transitions = New TransitionCollection()
                For Each transition As Transition In rootFrame.ContentTransitions
                    _transitions.Add(transition)
                Next
            End If

            rootFrame.ContentTransitions = Nothing
            AddHandler rootFrame.Navigated, AddressOf RootFrame_FirstNavigated
            ApplicationView.GetForCurrentView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseVisible)
            ApplicationView.PreferredLaunchViewSize = New Size(800, 480)
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize
            If (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) Then
                Dim sBar As StatusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView()
                If Not sBar Is Nothing Then
                    sBar.HideAsync()
                End If
            End If

            ' When the navigation stack isn't restored navigate to the first page,
            ' configuring the new page by passing required information as a navigation
            ' parameter
            If Not rootFrame.Navigate(GetType(EPG_WideView), e.Arguments) Then
                Throw New Exception("Failed to create initial page")
            End If
        End If
        ' Ensure the current window is active
        Window.Current.Activate()

    End Sub

    ''' <summary>
    ''' Restores the content transitions after the app has launched.
    ''' </summary>
    Private Sub RootFrame_FirstNavigated(sender As Object, e As NavigationEventArgs)
        Dim newTransitions As TransitionCollection
        If _transitions Is Nothing Then
            newTransitions = New TransitionCollection()
            newTransitions.Add(New NavigationThemeTransition())
        Else
            newTransitions = _transitions
        End If

        Dim rootFrame As Frame = DirectCast(sender, Frame)
        rootFrame.ContentTransitions = newTransitions
        RemoveHandler rootFrame.Navigated, AddressOf RootFrame_FirstNavigated
    End Sub

    ''' <summary>
    ''' Invoked when application execution is being suspended. Application state is saved
    ''' without knowing whether the application will be terminated or resumed with the contents
    ''' of memory still intact.
    ''' </summary>
    Private Async Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        Await SuspensionManager.SaveAsync()
        deferral.Complete()
        Me.DefaultViewModel.doCatchComents = False
    End Sub

End Class