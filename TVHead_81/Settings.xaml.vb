Imports TVHead_81.Common
Imports TVHead_81.ViewModels
Imports Windows.UI.Core

'Partial Public Class AppSettings


'    Dim settings As Windows.Storage.ApplicationDataContainer
'    Const strLanguageKeyName As String = "strLanguage"
'    Const strProposeAutoRecordingKeyName As String = "strProposeAutoRecording"
'    Const blAskConfirmationWhenDeletingKeyName As String = "blAskConfirmationWhenDeleting"
'    Const blLongPollingEnabledKeyName As String = "blLongPollingEnabled"
'    Const strServerIPKeyName As String = "strServerIP"
'    Const strServerPortKeyName As String = "strServerPort"
'    Const strUsernameKeyName As String = "strUserName"
'    Const strUserPasswordKeyName As String = "strUserPassword"
'    Const strTVHVersionKeyName As String = "strTVHVersion"
'    Const strTVHVersionLongKeyName As String = "strTVHVersionLong"
'    Const strConnectionStatusKeyName As String = "blConnected"
'    Const strFavouriteChannelTagKeyName As String = "strFavouriteChannelTag"
'    Const blHideNumberlessChannelsKeyName As String = "blHideNumberlessChannels"
'    Const blShowChannelNumbersKeyName As String = "ShowChannelNumbers"
'    Const blHideDisabledServicesKeyName As String = "blHideDisabledServices"
'    Const blHideNamelessServicesKeyName As String = "blHideNamelessServices"
'    Const blEasyOnBandwidthKeyName As String = "blEasyOnBandwidth"

'    Const strLanguageDefault = ""
'    Const strProposeAutoRecordingDefault As Boolean = True
'    Const blAskConfirmationWhenDeletingDefault As Boolean = True
'    Const blHideDisabledServicesDefault = True
'    Const blLongPollingEnabledDefault = False
'    Const strTVHVersionDefault As String = "3.9"
'    Const strTVHVersionLongDefault As String = ""
'    Const blShowChannelNumbersDefault As Boolean = True
'    Const blEasyOnBandwidthDefault As Boolean = False
'    Const blShowChannelCountPerChannelTagDefault As Boolean = False
'    Const intEPGHoursPerViewDefault As Integer = 24
'    Const intEPGHoursPerPageDefault As Integer = 1
'    Const intChannelsPerEPGGridViewDefault As Integer = 10
'    Const strFavouriteChannelTagDefault = ""
'    Const blHideNumberlessChannelsDefault = True
'    Const blHideNamelessServicesDefault = True



'#If DEBUG Then
'    'TEST SETTINGS
'    Const strServerIPDefault = ""
'    Const strServerPortDefault = ""
'    Const strUsernameDefault = ""
'    Const strUserPasswordDefault = ""
'    Const intMaxEPGEntries = 30000
'#Else
'    'PROD SETTINGS
'    Const strServerIPDefault = ""
'    Const strServerPortDefault = ""
'    Const strUsernameDefault = ""
'    Const strUserPasswordDefault = ""
'    Const intMaxEPGEntries = 30000
'#End If

'    Const strConnectionStatusDefault = False

'    Public Sub New()
'        settings = Windows.Storage.ApplicationData.Current.LocalSettings
'    End Sub



'    Public Function GetFullURL() As String
'        Return "http://" + ServerIPSetting + ":" + ServerPortSetting
'    End Function

'    Public Function AddOrUpdateValue(Key As String, value As Object)
'        Dim valueChanged As Boolean = False

'        If value Is Nothing Then Return False
'        If settings.Values.ContainsKey(Key) Then
'            settings.Values(Key) = value
'            valueChanged = True

'        Else
'            settings.Values.Add(Key, value)
'            valueChanged = True
'        End If
'        Return valueChanged
'    End Function

'    Public Function GetValueOrDefault(Of T)(Key As String, defaultValue As T) As T
'        Dim value As T
'        ' If the key exists, retrieve the value.
'        If Not settings.Values(Key) Is Nothing Then
'            value = DirectCast(settings.Values(Key), T)
'        Else
'            ' Otherwise, use the default value.
'            value = defaultValue
'        End If
'        Return value
'    End Function

'    Public Sub Save()
'        'settings.Save()
'    End Sub


'    Public Property PreferredLanguage As String
'        'DEFINES IF LONG POLLING SHOULD BE ENABLED
'        Get
'            Return GetValueOrDefault(Of String)(strLanguageKeyName, strLanguageDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strLanguageKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property


'    Public Property LongPollingEnabled As Boolean
'        'DEFINES IF LONG POLLING SHOULD BE ENABLED
'        Get
'            Return GetValueOrDefault(Of Boolean)(blLongPollingEnabledKeyName, blLongPollingEnabledDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blLongPollingEnabledKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property ProposeAutoRecording As Boolean
'        'DEFINES IF WHEN A RECORDING IS STARTED, A POPUP WILL APPEAR ASKING TO MAKE A SINGLE OR AUTORECORDING OF THE RECORDING
'        Get
'            Return GetValueOrDefault(Of Boolean)(strProposeAutoRecordingKeyName, strProposeAutoRecordingDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(strProposeAutoRecordingKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property


'    Public Property HideDisabledServices As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blHideDisabledServicesKeyName, blHideDisabledServicesDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blHideDisabledServicesKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property HideNamelessServices As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blHideNamelessServicesKeyName, blHideNamelessServicesDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blHideNamelessServicesKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property



'    Public Property ConfirmDeletion As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blAskConfirmationWhenDeletingKeyName, blAskConfirmationWhenDeletingDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blAskConfirmationWhenDeletingKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property TVHVersionLong As String
'        Get
'            Return GetValueOrDefault(Of String)(strTVHVersionLongKeyName, strTVHVersionLongDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strTVHVersionLongKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property TVHVersion As String
'        Get
'            Return GetValueOrDefault(Of String)(strTVHVersionKeyName, strTVHVersionDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strTVHVersionKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property EasyOnBandwidth As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blEasyOnBandwidthKeyName, blEasyOnBandwidthDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blEasyOnBandwidthKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property ShowChannelNumbers As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blShowChannelNumbersKeyName, blShowChannelNumbersDefault)
'        End Get
'        Set(value As Boolean)
'            AddOrUpdateValue(blShowChannelNumbersKeyName, value)
'            Save()
'        End Set
'    End Property


'    Public Property FavouriteChannelTag As String
'        Get
'            Return GetValueOrDefault(Of String)(strFavouriteChannelTagKeyName, Nothing)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strFavouriteChannelTagKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property HideNumberlessChannels As Boolean
'        Get
'            Return GetValueOrDefault(Of Boolean)(blHideNumberlessChannelsKeyName, blHideNumberlessChannelsDefault)
'        End Get
'        Set(value As Boolean)
'            If AddOrUpdateValue(blHideNumberlessChannelsKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property ServerPortSetting As String
'        Get
'            Return GetValueOrDefault(Of String)(strServerPortKeyName, strServerPortDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strServerPortKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property


'    Public Property ServerIPSetting As String
'        Get
'            Return GetValueOrDefault(Of String)(strServerIPKeyName, strServerIPDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strServerIPKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property PasswordSetting As String
'        Get
'            Return GetValueOrDefault(Of String)(strUserPasswordKeyName, strUserPasswordDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strUserPasswordKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property

'    Public Property UsernameSetting As String
'        Get
'            Return GetValueOrDefault(Of String)(strUsernameKeyName, strUsernameDefault)
'        End Get
'        Set(value As String)
'            If AddOrUpdateValue(strUsernameKeyName, value) Then
'                Save()
'            End If
'        End Set
'    End Property
'End Class

Partial Public Class AppSettingsPage
    Inherits Page

    Dim app As Application = CType(Application.Current, Application)
    Dim vm As TVHead_ViewModel = vm

    Private WithEvents _navigationHelper As New NavigationHelper(Me)
    Private ReadOnly _defaultViewModel As New ObservableDictionary

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
        'TODO: Save the unique state of the page here.

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
        vm.StopRefresh()
        Me.DataContext = vm
        'Dim myRegion = New Windows.Globalization.GeographicRegion
        vm.supportedLanguages.languages(0).val = vm.loader.GetString("usePhoneLanguage")
        BindChannelTags()
        Dim a = CType(cbLanguage.ItemsSource, LanguageList)
        cbLanguage.ItemsSource = vm.supportedLanguages.languages
        Dim aindex = vm.supportedLanguages.languages.IndexOf((From l In vm.supportedLanguages.languages Where l.code = vm.appSettings.PreferredLanguage).FirstOrDefault)
        cbLanguage.SelectedIndex = aindex
    End Sub


    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
        If Not cbChannelTags.SelectedItem Is Nothing Then
            vm.appSettings.FavouriteChannelTag = CType(cbChannelTags.SelectedItem, ChannelTagViewModel).uuid
        ElseIf cbChannelTags.Items.Count > 0 Then
            vm.appSettings.FavouriteChannelTag = CType(cbChannelTags.Items(0), ChannelTagViewModel).uuid
        End If
    End Sub

#End Region


    Public channelTags As List(Of ChannelTagViewModel)

    Public Sub New()
        InitializeComponent()
        AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf SettingsBackPressed
    End Sub

    Public Function BindChannelTags() As String

        Dim chTags As New List(Of ChannelTagViewModel)
        Try
            chTags = vm.ChannelTags.items.ToList()
            cbChannelTags.ItemsSource = chTags.OrderBy(Function(x) x.name)
            Dim intie As New ChannelTagViewModel
            If Not vm.appSettings.FavouriteChannelTag Is Nothing Then
                intie = (From c In chTags Where c.uuid = vm.appSettings.FavouriteChannelTag).FirstOrDefault
                If Not intie Is Nothing Then
                    cbChannelTags.SelectedIndex = chTags.IndexOf(intie)
                Else
                    If cbChannelTags.Items.Count > 0 Then cbChannelTags.SelectedIndex = 0
                End If
            Else
                If cbChannelTags.Items.Count > 0 Then cbChannelTags.SelectedIndex = 0
            End If
            tbChannelGroupExplanation.Visibility = Visibility.Visible
            cbChannelTags.Visibility = Visibility.Visible
            tbChannelGroup.Visibility = Visibility.Visible
        Catch ex As Exception
            WriteToDebug("AppSettingsPage.BindChannelTags()", ex.InnerException.ToString)
        End Try
        Return ""
    End Function

    Private Async Sub btnTestConnection_Click(sender As Object, e As RoutedEventArgs)
        Dim app As Application = CType(Application.Current, Application)
        pbTestResult.IsIndeterminate = True
        tbTestResult.Text = "Testing connection..."
        Dim sInfo As ServerInfoViewModel

        Try
            If Await vm.IsConnected Then
                sInfo = Await vm.GetServerInfo()
                vm.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
                vm.TVHVersion = sInfo.sw_version
                tbTestResult.Text = vm.TVHVersionLong
            End If
            Await vm.checkAccess(True)
        Catch ex As Exception
            WriteToDebug("Downloader.DownloadJSON", ex.Message.ToString)
            tbTestResult.Text = ex.Message
        End Try

        pbTestResult.IsIndeterminate = False

        'If successful connection has been made, load the channeltags and set the first as favourite
        If Await vm.IsConnected Then
            Await LoadChannelTags()
            cbChannelTags.ItemsSource = vm.ChannelTags.items.ToList()
            If Not vm.ChannelTags.items.Count = 0 Then
                cbChannelTags.SelectedIndex = 0
                cbChannelTags.IsEnabled = True
            Else
                cbChannelTags.IsEnabled = False
            End If
        End If
    End Sub

    Private Sub cbChannelTags_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not cbChannelTags.SelectedItem Is Nothing Then
            vm.appSettings.FavouriteChannelTag = CType(cbChannelTags.SelectedItem, ChannelTagViewModel).uuid
        End If
    End Sub

    Private Sub SettingsBackPressed(sender As Object, e As BackRequestedEventArgs)
        Dim content = Window.Current.Content
        Dim frame = CType(content, Frame)
        If frame.CanGoBack Then frame.GoBack()
        e.Handled = True
    End Sub

    Private Sub cbLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ''TODO FIX LANGUAGE SETTINGS
        If Not cbLanguage.SelectedIndex = -1 Then
            vm.appSettings.PreferredLanguage = CType(cbLanguage.SelectedItem, Language).code
            If Not vm.appSettings.PreferredLanguage = "" Then
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = vm.appSettings.PreferredLanguage
            Else
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = ""
            End If
        End If
    End Sub
End Class
