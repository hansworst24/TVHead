Imports System.Text.RegularExpressions
Imports GalaSoft.MvvmLight
Imports Windows.Web.Http

Public Class TVHead_Settings
    Inherits ViewModelBase

    Dim settings As Windows.Storage.ApplicationDataContainer
    Const strLanguageKeyName As String = "strLanguage"
    Const strProposeAutoRecordingKeyName As String = "strProposeAutoRecording"
    Const blAskConfirmationWhenDeletingKeyName As String = "blAskConfirmationWhenDeleting"
    Const blLongPollingEnabledKeyName As String = "blLongPollingEnabled"
    Const strServerIPKeyName As String = "strServerIP"
    Const strServerPortKeyName As String = "strServerPort"
    Const strUsernameKeyName As String = "strUserName"
    Const strUserPasswordKeyName As String = "strUserPassword"
    Const strTVHVersionKeyName As String = "strTVHVersion"
    Const strTVHVersionLongKeyName As String = "strTVHVersionLong"
    Const strConnectionStatusKeyName As String = "blConnected"
    Const strFavouriteChannelTagKeyName As String = "strFavouriteChannelTag"
    Const blHideNumberlessChannelsKeyName As String = "blHideNumberlessChannels"
    Const blShowChannelNumbersKeyName As String = "ShowChannelNumbers"
    Const blHideDisabledServicesKeyName As String = "blHideDisabledServices"
    Const blHideNamelessServicesKeyName As String = "blHideNamelessServices"
    Const blEasyOnBandwidthKeyName As String = "blEasyOnBandwidth"
    Const strUseDarkThemeKeyName As String = "strUseDarkTheme"

    Const strLanguageDefault = ""
    Const strProposeAutoRecordingDefault As Boolean = True
    Const blAskConfirmationWhenDeletingDefault As Boolean = True
    Const blHideDisabledServicesDefault = True
    Const blLongPollingEnabledDefault = False
    Const strTVHVersionDefault As String = "3.9"
    Const strTVHVersionLongDefault As String = ""
    Const blShowChannelNumbersDefault As Boolean = True
    Const blEasyOnBandwidthDefault As Boolean = False
    Const blShowChannelCountPerChannelTagDefault As Boolean = False
    Const intEPGHoursPerViewDefault As Integer = 24
    Const intEPGHoursPerPageDefault As Integer = 1
    Const intChannelsPerEPGGridViewDefault As Integer = 10
    Const strFavouriteChannelTagDefault = ""
    Const blHideNumberlessChannelsDefault = True
    Const blHideNamelessServicesDefault = True
    Const strUseDarkThemeDefault As Boolean = False 'TODO SET



#If DEBUG Then
    'TEST SETTINGS
    Const strServerIPDefault = "192.168.168.4"
    Const strServerPortDefault = "9981"
    Const strUsernameDefault = "cre8or"
    Const strUserPasswordDefault = "plex4220"
    Const intMaxEPGEntries = 30000
#Else
    'PROD SETTINGS
    Const strServerIPDefault = ""
    Const strServerPortDefault = ""
    Const strUsernameDefault = ""
    Const strUserPasswordDefault = ""
    Const intMaxEPGEntries = 30000
#End If


    'Bool which is used to determine of the TVH version is capable of returning properly formatted IDNode Information when requested. Only from a commit since Oct 2015 this seems
    'to work properly for recordings. It's set initially to true to try to load initially, but when that fails it is set to false.
    Public Property CapableOfLoadingRecordingIDNode As Boolean = False
    Public Property CapableOfLoadingAutoRecordingIDNode As Boolean = False
    Public Property CapableOfLoadingChannelIDNode As Boolean = False

    'Bools which are used for storing if the user has access to the TVHeadends various components
    'Public Property hasEPGAccess As Boolean
    Public Property hasDVRAccess As Boolean
    Public Property hasFailedDVRAccess As Boolean
    'Public Property hasAdminAccess As Boolean

    Public Async Function hasAdminAccess(Optional showmessage As Boolean = False) As Task(Of Boolean)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim adminAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetSubscriptions())
        If adminAccessResponse.IsSuccessStatusCode Then
            Return True
        Else
            If showmessage Then Await vm.Notify.Update(True, "You're not an admin, some data cannot be loaded", 2, False, 0)
            Return False
        End If

    End Function




    Public Async Function hasEPGAccess(Optional showmessage As Boolean = False) As Task(Of Boolean)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim epgAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New api40).apiGetEPGEvents("", False, 1))
        If epgAccessResponse.IsSuccessStatusCode Then
            Return True
        Else
            If showmessage Then Await vm.Notify.Update(True, "Error accessing EPG", 2, False, 0)
            Return False
        End If
    End Function


    Const strConnectionStatusDefault = False

    Public Sub New()
        settings = Windows.Storage.ApplicationData.Current.LocalSettings
    End Sub

    Public Function ContainsValidServerDetails() As Boolean
        Dim ValidHostnameRegex As New Regex("^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$")
        Dim ValidIpAddressRegex As New Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")

        If ValidHostnameRegex.IsMatch(ServerIPSetting) Or ValidIpAddressRegex.IsMatch(ServerIPSetting) Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Function GetFullURL() As String
        Return "http://" + ServerIPSetting + ":" + ServerPortSetting
    End Function

    Public Function AddOrUpdateValue(Key As String, value As Object)
        Dim valueChanged As Boolean = False

        If value Is Nothing Then Return False
        If settings.Values.ContainsKey(Key) Then
            settings.Values(Key) = value
            valueChanged = True

        Else
            settings.Values.Add(Key, value)
            valueChanged = True
        End If
        Return valueChanged
    End Function

    Public Function GetValueOrDefault(Of T)(Key As String, defaultValue As T) As T
        Dim value As T
        ' If the key exists, retrieve the value.
        If Not settings.Values(Key) Is Nothing Then
            value = DirectCast(settings.Values(Key), T)
        Else
            ' Otherwise, use the default value.
            value = defaultValue
        End If
        Return value
    End Function

    Public Sub Save()
        'settings.Save()
    End Sub

    Public Property UseDarkTheme As Boolean
        'DEFINES IF THE APP USES A DARK THEME OR NOT
        Get
            Return GetValueOrDefault(Of Boolean)(strUseDarkThemeKeyName, strUseDarkThemeDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(strUseDarkThemeKeyName, value) Then
                Save()
            End If
        End Set
    End Property



    Public Property PreferredLanguage As String
        'DEFINES IF LONG POLLING SHOULD BE ENABLED
        Get
            Return GetValueOrDefault(Of String)(strLanguageKeyName, strLanguageDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strLanguageKeyName, value) Then
                Save()
            End If
        End Set
    End Property


    Public Property LongPollingEnabled As Boolean
        'DEFINES IF LONG POLLING SHOULD BE ENABLED
        Get
            Return GetValueOrDefault(Of Boolean)(blLongPollingEnabledKeyName, blLongPollingEnabledDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blLongPollingEnabledKeyName, value) Then
                Save()
            End If
            RaisePropertyChanged("LongPollingEnabled")
        End Set
    End Property

    Public Property ProposeAutoRecording As Boolean
        'DEFINES IF WHEN A RECORDING IS STARTED, A POPUP WILL APPEAR ASKING TO MAKE A SINGLE OR AUTORECORDING OF THE RECORDING
        Get
            Return GetValueOrDefault(Of Boolean)(strProposeAutoRecordingKeyName, strProposeAutoRecordingDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(strProposeAutoRecordingKeyName, value) Then
                Save()
            End If
        End Set
    End Property


    Public Property HideDisabledServices As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blHideDisabledServicesKeyName, blHideDisabledServicesDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blHideDisabledServicesKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property HideNamelessServices As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blHideNamelessServicesKeyName, blHideNamelessServicesDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blHideNamelessServicesKeyName, value) Then
                Save()
            End If
        End Set
    End Property



    Public Property ConfirmDeletion As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blAskConfirmationWhenDeletingKeyName, blAskConfirmationWhenDeletingDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blAskConfirmationWhenDeletingKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property TVHVersionLong As String
        Get
            Return GetValueOrDefault(Of String)(strTVHVersionLongKeyName, strTVHVersionLongDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strTVHVersionLongKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property TVHVersion As String
        Get
            Return GetValueOrDefault(Of String)(strTVHVersionKeyName, strTVHVersionDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strTVHVersionKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property EasyOnBandwidth As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blEasyOnBandwidthKeyName, blEasyOnBandwidthDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blEasyOnBandwidthKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property ShowChannelNumbers As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blShowChannelNumbersKeyName, blShowChannelNumbersDefault)
        End Get
        Set(value As Boolean)
            AddOrUpdateValue(blShowChannelNumbersKeyName, value)
            Save()
        End Set
    End Property


    Public Property FavouriteChannelTag As String
        Get
            Return GetValueOrDefault(Of String)(strFavouriteChannelTagKeyName, Nothing)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strFavouriteChannelTagKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property HideNumberlessChannels As Boolean
        Get
            Return GetValueOrDefault(Of Boolean)(blHideNumberlessChannelsKeyName, blHideNumberlessChannelsDefault)
        End Get
        Set(value As Boolean)
            If AddOrUpdateValue(blHideNumberlessChannelsKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property ServerPortSetting As String
        Get
            Return GetValueOrDefault(Of String)(strServerPortKeyName, strServerPortDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strServerPortKeyName, value) Then
                Save()
            End If
        End Set
    End Property


    Public Property ServerIPSetting As String
        Get
            Return GetValueOrDefault(Of String)(strServerIPKeyName, strServerIPDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strServerIPKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property PasswordSetting As String
        Get
            Return GetValueOrDefault(Of String)(strUserPasswordKeyName, strUserPasswordDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strUserPasswordKeyName, value) Then
                Save()
            End If
        End Set
    End Property

    Public Property UsernameSetting As String
        Get
            Return GetValueOrDefault(Of String)(strUsernameKeyName, strUsernameDefault)
        End Get
        Set(value As String)
            If AddOrUpdateValue(strUsernameKeyName, value) Then
                Save()
            End If
        End Set
    End Property
End Class
