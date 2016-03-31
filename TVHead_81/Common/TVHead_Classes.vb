
Imports System.Threading
'Imports System.Net.Http
Imports TVHead_81.ViewModels
Imports Windows.Globalization
Imports Windows.Globalization.DateTimeFormatting
Imports System.Globalization
Imports Windows.Web.Http
Imports Windows.Web.Http.Filters

Namespace CometMessages

    Public Class dvrdb
        Public Property reload As Integer
    End Class

    Public Class dvrentry
        Public Property create As String()
        Public Property update As String()
        Public Property change As String()
        Public Property delete As String()
        Public Property notificationClass As String
        Public Property uuid As String
    End Class

    Public Class dvrautorec
        Public Property create As String()
        Public Property update As String()
        Public Property change As String()
        Public Property delete As String()
        Public Property notificationClass As String
        Public Property uuid As String

    End Class

    Public Class epg
        Public Property create As Integer()
        Public Property change As Integer()
        Public Property delete As Integer()
        Public Property dvr_update As Integer()
        Public Property dvr_delete As Integer()
        Public Property update As Integer()
        Public Property notificationClass As String
        Public Property uuid As String

        Public Sub New()
        End Sub
    End Class

    Public Class diskspaceUpdate
        Public Property freediskspace As Long
        Public Property notificationClass As String
        Public Property totaldiskspace As Long
        Public Property useddiskspace As Long
    End Class

    Public Class logUpdate
        Public Property logtxt As String
        Public Property notificationClass As String
    End Class

    Public Class input_status
        Public Property ber As Long
        Public Property bps As Integer
        Public Property cc As Integer
        Public Property ec_bit As Integer
        Public Property ec_block As Integer
        Public Property input As String
        Public Property notificationClass As String
        Public Property reload As Integer
        Public Property signal As Integer
        Public Property signal_scale As Integer
        Public Property snr As Integer
        Public Property snr_scale As Integer
        Public Property stream As String
        Public Property subs As Integer
        Public Property tc_bit As Integer
        Public Property tc_block As Integer
        Public Property te As Integer
        Public Property unc As Integer
        Public Property update As Integer
        Public Property uuid As String
        Public Property weight As Integer
    End Class

    Public Class subscription
        Public Property channel As String
        Public Property descramble As String
        Public Property errors As Integer
        Public Property id As Integer
        Public Property [in] As Integer
        Public Property notificationClass As String
        Public Property out As Integer
        Public Property reload As Integer
        Public Property service As String
        Public Property start As Long
        Public Property state As String
        Public Property title As String
        Public Property total_in As Long
        Public Property total_out As Long
    End Class

    Public Class CometMessage
        Public Property boxid As String
        Public Property messages As Object()

    End Class


    Public Class DownloadResponse
        Public Property result As String
        Public Property IsSuccess As Boolean
        Public Property StatusCode As HttpStatusCode
        Public Property StatusMessage As String

    End Class

End Namespace

Public Class FlyoutHelpers
    Inherits DependencyObject
    Public Shared ReadOnly IsOpenProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsOpen", GetType(Boolean), GetType(FlyoutHelpers), New PropertyMetadata(False, AddressOf OnIsOpenPropertyChanged))
    Public Shared ReadOnly ParentProperty As DependencyProperty = DependencyProperty.RegisterAttached("Parent", GetType(Button), GetType(FlyoutHelpers), New PropertyMetadata(Nothing, AddressOf OnParentPropertyChanged))
    Public Shared Sub SetIsOpen(d As DependencyObject, value As Boolean)
        d.SetValue(IsOpenProperty, value)
    End Sub

    Public Shared Function GetIsOpen(d As DependencyObject) As Boolean
        Return CBool(d.GetValue(IsOpenProperty))
    End Function
    Public Shared Sub SetParent(d As DependencyObject, value As Button)
        d.SetValue(ParentProperty, value)
    End Sub

    Public Shared Function GetParent(d As DependencyObject) As Button
        Return DirectCast(d.GetValue(ParentProperty), Button)
    End Function

    Private Shared Sub OnParentPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim flyout = CType(d, Flyout)
        If Not flyout Is Nothing Then
            AddHandler flyout.Opening, Sub()
                                           flyout.SetValue(IsOpenProperty, True)
                                       End Sub
            AddHandler flyout.Closed, Sub()
                                          flyout.SetValue(IsOpenProperty, False)
                                      End Sub
        End If
    End Sub

    Private Shared Sub OnIsOpenPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim flyout = CType(d, Flyout)
        Dim parent = CType(d.GetValue(ParentProperty), Button)
        If Not flyout Is Nothing And Not parent Is Nothing Then
            Dim newValue = CType(e.NewValue, Boolean)
            If newValue Then
                ' flyout.ShowAt(parent)
            Else
                flyout.Hide()
            End If

        End If
    End Sub




End Class

Public Class BindableFlyout
    Inherits DependencyObject
#Region "ItemsSource"

    Public Shared Function GetItemsSource(obj As DependencyObject) As IEnumerable
        Return TryCast(obj.GetValue(ItemsSourceProperty), IEnumerable)
    End Function
    Public Shared Sub SetItemsSource(obj As DependencyObject, value As IEnumerable)
        obj.SetValue(ItemsSourceProperty, value)
    End Sub
    Public Shared ReadOnly ItemsSourceProperty As DependencyProperty = DependencyProperty.RegisterAttached("ItemsSource", GetType(IEnumerable), GetType(BindableFlyout), New PropertyMetadata(Nothing, AddressOf ItemsSourceChanged))
    Private Shared Sub ItemsSourceChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Setup(TryCast(d, Windows.UI.Xaml.Controls.Flyout))
    End Sub

    Public Shared ReadOnly IsOpenProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsOpen", GetType(Boolean), GetType(BindableFlyout), New PropertyMetadata(False, AddressOf onIsOpenPropertyChanged))
    Public Shared ReadOnly ParentProperty As DependencyProperty = DependencyProperty.RegisterAttached("Parent", GetType(Button), GetType(BindableFlyout), New PropertyMetadata(Nothing, AddressOf onParentPropertyChanged))

    Public Shared Sub SetIsOpen(d As DependencyObject, value As Boolean)
        d.SetValue(IsOpenProperty, value)
    End Sub

    Public Shared Function GetIsOpen(d As DependencyObject) As Boolean
        Return d.GetValue(IsOpenProperty)
    End Function
    Public Shared Sub SetParent(d As DependencyObject, value As Button)
        d.SetValue(ParentProperty, value)
    End Sub
    Public Shared Function GetParent(d As DependencyObject) As Button
        Return d.GetValue(ParentProperty)
    End Function

    Private Shared Sub onParentPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim flyout = CType(d, Flyout)
        If Not flyout Is Nothing Then
            AddHandler flyout.Opening, Sub()
                                           flyout.SetValue(IsOpenProperty, True)
                                       End Sub
            AddHandler flyout.Closed, Sub()
                                          flyout.SetValue(IsOpenProperty, False)
                                      End Sub

        End If
    End Sub

    Private Shared Sub onIsOpenPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim flyout = CType(d, Flyout)
        Dim parent = CType(d.GetValue(ParentProperty), Button)

        If Not flyout Is Nothing And Not parent Is Nothing Then
            Dim newValue = CType(e.NewValue, Boolean)
            If newValue Then
                flyout.ShowAt(parent)
            Else
                flyout.Hide()
            End If
        End If
    End Sub



#End Region

#Region "ItemTemplate"

    Public Shared Function GetItemTemplate(obj As DependencyObject) As DataTemplate
        Return DirectCast(obj.GetValue(ItemTemplateProperty), DataTemplate)
    End Function
    Public Shared Sub SetItemTemplate(obj As DependencyObject, value As DataTemplate)
        obj.SetValue(ItemTemplateProperty, value)
    End Sub
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty = DependencyProperty.RegisterAttached("ItemTemplate", GetType(DataTemplate), GetType(BindableFlyout), New PropertyMetadata(Nothing, AddressOf ItemsTemplateChanged))
    Private Shared Sub ItemsTemplateChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Setup(TryCast(d, Windows.UI.Xaml.Controls.Flyout))
    End Sub

#End Region

    Private Shared Sub Setup(m As Windows.UI.Xaml.Controls.Flyout)
        If Windows.ApplicationModel.DesignMode.DesignModeEnabled Then
            Return
        End If
        Dim s = GetItemsSource(m)
        If s Is Nothing Then
            Return
        End If
        Dim t = GetItemTemplate(m)
        If t Is Nothing Then
            Return
        End If
        Dim c = New Windows.UI.Xaml.Controls.ItemsControl() With {.ItemsSource = s, .ItemTemplate = t}
        Dim n = Windows.UI.Core.CoreDispatcherPriority.Normal
        Dim h As Windows.UI.Core.DispatchedHandler = Function() InlineAssignHelper(m.Content, c)
        m.Dispatcher.RunAsync(n, h)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
End Class

Public Class DateTimeToTimeSpanConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Dim DateValue As DateTime = CType(value, DateTime)
        'Return DateValue.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern)
        Return DateValue.TimeOfDay
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Dim ts As TimeSpan = CType(value, TimeSpan)
        Dim dt As New Date
        Return dt.Add(ts)
    End Function
End Class

Public Class DateTimeToDateTimeOffsetConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Dim dt As DateTime = CType(value, DateTime)
        Dim dtOffset As DateTimeOffset = dt
        Return dtOffset
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
    Dim dtOffset As DateTimeOffset = CType(value, DateTimeOffset)
    Dim dt As DateTime = dtOffset.DateTime
    Return dt
End Function
End Class

'Public Class DateToTimeConverter
'    Implements IValueConverter

'    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

'        Dim DateValue As DateTime = CType(value, DateTime)
'        Return DateValue.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern)
'        'If Not DateValue = Date.MinValue Then

'        '    Dim rs As String = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion
'        '    Dim dtFormatter As New DateTimeFormatter("shorttime", New String() {rs})
'        '    Return dtFormatter.Format(DateValue)
'        'Else
'        '    Return ""
'        'End If


'        'Return DateValue.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern)
'    End Function

'    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
'        Dim ts As TimeSpan = CType(value, TimeSpan)
'        Dim dt As New Date
'        Return dt.Add(ts)
'    End Function
'End Class

'Public Class DateToTimeSpanConverter
'    Implements IValueConverter

'    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
'        Dim DateValue As DateTime = CType(value, DateTime)
'        'Return DateValue.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern)
'        Return DateValue.TimeOfDay
'    End Function

'    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
'        Dim ts As TimeSpan = CType(value, TimeSpan)
'        Dim dt As New Date
'        Return dt.Add(ts)
'    End Function
'End Class

Public Class BoolToVisibilityConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Dim bool As Boolean = CType(value, Boolean)
        If bool = False Then Return Visibility.Visible Else Return Visibility.Collapsed
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Dim vis As Visibility = CType(value, Visibility)
        If vis = Visibility.Collapsed Then Return True Else Return False
    End Function
End Class

Public Class StringFormatConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

        'No value provided
        If value Is Nothing Then
            Return Nothing
        End If
        ' No format provided.
        If parameter Is Nothing Then
            Return value
        End If

        Return [String].Format(DirectCast(parameter, [String]), value)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

        Return value
    End Function
End Class

Public Class CultureInfoHelper

    Private array1() As String = {"US"}
    Private cultureName = New DateTimeFormatter("longdate", array1).ResolvedLanguage
    Private myCultureInfo As CultureInfo = New CultureInfo(cultureName)
    Private GetLongDate As New DateTimeFormatter("longdate", New String() {myCultureInfo.TwoLetterISOLanguageName})
    Private GetShortDate As New DateTimeFormatter("shortdate", New String() {myCultureInfo.TwoLetterISOLanguageName})
    Private GetShortTime As New DateTimeFormatter("shorttime", New String() {myCultureInfo.TwoLetterISOLanguageName})

    Public Function GetAbbreviatedDayNames() As String()
        Return myCultureInfo.DateTimeFormat.AbbreviatedDayNames
    End Function

    Public Function GetCurrentCulture() As CultureInfo
        Return myCultureInfo
    End Function

    Public Function GetLongDateString(dt As DateTime) As String
        Return GetLongDate.Format(dt)
    End Function

    Public Function GetShortDateString(dt As DateTime) As String
        Return GetShortDate.Format(dt)
    End Function

    Public Function GetShortTimeString(dt As DateTime) As String
        Return GetShortTime.Format(dt)
    End Function

End Class

Public Class RecordingReturnValue
    Public Property tvhResponse As New tvhCommandResponse
    Public Property recording As New RecordingViewModel
    Public Property recording_id As String
End Class

Public Class Downloader

    Private vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel

    Public Async Function DownloadJSON(url As String) As Task(Of HttpResponseMessage)
        Using filter As New HttpBaseProtocolFilter
            If vm.TVHeadSettings.UsernameSetting <> "" And vm.TVHeadSettings.PasswordSetting <> "" Then
                filter.ServerCredential = New Windows.Security.Credentials.PasswordCredential With {.Password = vm.TVHeadSettings.PasswordSetting, .UserName = vm.TVHeadSettings.UsernameSetting}
            End If
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache
            filter.AllowUI = False
            filter.UseProxy = False
            Using wc As New HttpClient(filter)
                Dim cts As New CancellationTokenSource(5000)
                Try
                    'WriteToDebug("Downloader.DownloadJSON", url)
                    ''TODO FIX GETRSTRINGAYNSC
                    Dim response As HttpResponseMessage = Await wc.GetAsync(New Uri(url)).AsTask(cts.Token)
                    vm.totalBytesReceived += response.Content.ToString.Length
                    'WriteToDebug("Downloader.DownloadJSON", "Length :" & response.Content.ToString.Length.ToString & ",Size :" & Math.Round(response.Content.ToString.Length / 1024).ToString & "kb")
                    'If Await vm.IsConnected Then Await vm.StatusBar.Clean()
                    Return response
                Catch ex As TaskCanceledException
                    Return New HttpResponseMessage With {.ReasonPhrase = "Connection timed out", .StatusCode = HttpStatusCode.RequestTimeout}
                Catch ex As Exception
                    ' WriteToDebug("Downloader.DownloadJSON", ex.Message.ToString)
                    Return New HttpResponseMessage With {.ReasonPhrase = ex.Message, .StatusCode = HttpStatusCode.Unauthorized}
                End Try
            End Using
        End Using
    End Function

    Public Async Function DownloadComet(cometID As String) As Task(Of Windows.Web.Http.HttpResponseMessage)
        Using filter As New HttpBaseProtocolFilter
            filter.ServerCredential = New Windows.Security.Credentials.PasswordCredential With {.Password = vm.appSettings.PasswordSetting, .UserName = vm.appSettings.UsernameSetting}
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache
            filter.AllowUI = False
            filter.UseProxy = False
            Using wc As New HttpClient(filter)
                Dim url As String = String.Format("{0}/comet/poll?boxid={1}&immediate=0", vm.appSettings.GetFullURL(), cometID)
                Dim cts As New CancellationTokenSource(Timeout.Infinite)
                Dim request = New HttpRequestMessage(HttpMethod.[Get], New Uri(url))
                Dim response As New HttpResponseMessage
                Try
                    response = Await wc.SendRequestAsync(request, HttpCompletionOption.ResponseHeadersRead).AsTask(cts.Token)
                    'WriteToDebug("Downloader.DownloadComet", "Length :" & response.Content.ToString.Length.ToString & ",Size :" & Math.Round(response.Content.ToString.Length / 1024).ToString & "kb")
                    vm.totalBytesReceived += response.Content.ToString.Length
                Catch ex As TaskCanceledException
                    Return New HttpResponseMessage With {.ReasonPhrase = "Connection timed out", .StatusCode = HttpStatusCode.RequestTimeout}
                Catch ex As Exception
                    response.StatusCode = Net.HttpStatusCode.NotFound
                    'WriteToDebug("Downloader.DownloadComet", "error")
                End Try
                'WriteToDebug("Downloader.DownloadComet", "end")
                Return response
            End Using
        End Using
    End Function
End Class

Public Class Group(Of T)
    Inherits ObservableCollection(Of T)

    Public Sub New(name As Date, items As IEnumerable(Of T))
        Me.Key = name
        For Each item As T In items
            Me.Add(item)
        Next
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim that As Group(Of T) = TryCast(obj, Group(Of T))

        Return (that IsNot Nothing) AndAlso (Me.Key.Equals(that.Key))
    End Function

    Public Property Key As DateTime
        Get
            Return m_Key
        End Get
        Set(value As DateTime)
            m_Key = value
        End Set
    End Property
    Private m_Key As DateTime

    Public ReadOnly Property KeyString As String
        Get
            'Return vm.myCultureInfoHelper.GetLongDateString(Key)
            Return Key.ToString("D")
            'Return Key.ToString(vm.test.DateTimeFormat.LongDatePattern)
            'vm.test.DateTimeFormat.
            'Dim rs As String = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion
            'Dim dip = Windows.System.UserProfile.GlobalizationPreferences.Clocks
            'Dim stick = Windows.System.UserProfile.GlobalizationPreferences.Calendars
            'Dim dtFormatter As New DateTimeFormatter("longdate", New String() {rs})
            'Return dtFormatter.Format(Key)
        End Get

    End Property
    Private _KeyString As String


End Class

Public Class tvhCommandResponse
    Property success As Integer

    Public Sub New()
        'Default response is fail (0)
        success = 0
    End Sub
End Class



