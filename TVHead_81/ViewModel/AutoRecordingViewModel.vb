Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json
Imports Windows.UI

Public Class AutoRecordingViewModel
    Inherits ViewModelBase

    Protected Friend _autorecording As TVHAutoRecording

    Private Property _IsSelected As Boolean
    Public Property IsSelected As Boolean
        Get
            Return _IsSelected
        End Get
        Set(value As Boolean)
            _IsSelected = value
            RaisePropertyChanged("IsSelected")
            RaisePropertyChanged("AutoRecordingBackground")
        End Set
    End Property

    Private _IsExpanded As String
    Public Property IsExpanded As String
        Get
            Return _IsExpanded
        End Get
        Set(ByVal value As String)
            _IsExpanded = value
            RaisePropertyChanged("IsExpanded")
        End Set
    End Property

    Public ReadOnly Property uuid As String
        Get
            Return _autorecording.uuid
        End Get
    End Property
    Public Property enabled As Boolean
        Get
            Return _autorecording.enabled
        End Get
        Set(value As Boolean)
            _autorecording.enabled = value
        End Set
    End Property

    Public Property channel As String
        Get
            Return _autorecording.channel
        End Get
        Set(value As String)
            _autorecording.channel = value
        End Set
    End Property
    Private Property _channel As String
    Public ReadOnly Property channel_icon As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Not vm.Channels Is Nothing AndAlso Not vm.Channels._allchannels Is Nothing AndAlso _autorecording.channel <> "" Then
                Dim c As ChannelViewModel = vm.Channels._allchannels.Where(Function(x) x.uuid = _autorecording.channel).FirstOrDefault()
                Return If(c Is Nothing, "ms-appx:///Images/tvheadend.png", c.chicon)
            Else
                Return "ms-appx:///Images/tvheadend.png"
            End If
        End Get

    End Property
    Public ReadOnly Property channelname As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Not vm.Channels Is Nothing AndAlso Not vm.Channels._allchannels Is Nothing AndAlso _autorecording.channel <> "" Then
                Dim c As ChannelViewModel = vm.Channels._allchannels.Where(Function(x) x.uuid = _autorecording.channel).FirstOrDefault()
                Return If(c Is Nothing, "", c.name)
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property tagName As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Not vm.ChannelTags Is Nothing AndAlso Not vm.ChannelTags.items Is Nothing Then
                Dim t As ChannelTagViewModel = vm.ChannelTags.items.Where(Function(x) x.uuid = _autorecording.tag).FirstOrDefault()
                Return If(t Is Nothing, "", t.name)
            Else
                Return "ms-appx:///Images/tvheadend.png"
            End If
        End Get
    End Property
    Public ReadOnly Property tag As String
        Get
            Return _autorecording.tag
        End Get
    End Property

    Public ReadOnly Property contenttype As Integer
        Get
            Return _autorecording.content_type
        End Get
    End Property
    Public Property title As String
        Get
            Return _autorecording.title
        End Get
        Set(value As String)
            _autorecording.title = value
        End Set
    End Property
    Public Property name As String
        Get
            Return _autorecording.name
        End Get
        Set(value As String)
            _autorecording.name = value
        End Set
    End Property
    Public Property comment As String
        Get
            Return _autorecording.comment
        End Get
        Set(value As String)
            _autorecording.comment = value
        End Set
    End Property
    Public ReadOnly Property config As String
        Get
            Return _autorecording.config_name
        End Get
    End Property

    Public Property start As String
        Get
            Return _autorecording.start
        End Get
        Set(value As String)
            _autorecording.start = value
        End Set
    End Property
    Public Property start_window As String
        Get
            Return _autorecording.start_window
        End Get
        Set(value As String)
            _autorecording.start_window = value
        End Set
    End Property
    Public Property weekdays As List(Of Integer)
        Get
            Return _autorecording.weekdays
        End Get
        Set(value As List(Of Integer))
            _autorecording.weekdays = value
        End Set
    End Property
    Private Property _RecordingOnDay As ObservableCollection(Of Boolean?)
    Public Property RecordingOnDay As ObservableCollection(Of Boolean?)
        Get
            If _RecordingOnDay Is Nothing Then
                _RecordingOnDay = New ObservableCollection(Of Boolean?)
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(1) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(2) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(3) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(4) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(5) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(6) > -1, True, False))
                _RecordingOnDay.Add(If(_autorecording.weekdays.IndexOf(7) > -1, True, False))
            End If
            Return _RecordingOnDay
        End Get
        Set(value As ObservableCollection(Of Boolean?))
            Dim dip As Boolean
            If dip = True Then
                WriteToDebug("", "")
            End If
        End Set
    End Property

    Public ReadOnly Property RecordingDays As List(Of String)
        Get
            Dim daysList As New List(Of String)
            Dim vm = CType(Application.Current, Application).DefaultViewModel
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(1))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(2))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(3))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(4))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(5))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(6))
            daysList.Add(vm.myCultureInfoHelper.GetAbbreviatedDayNames()(0))
            Return daysList
        End Get
    End Property

    'Public ReadOnly Property recordingDaysString As String
    '    Get
    '        Return String.Join("/", RecordingDays)
    '    End Get
    'End Property

    Public ReadOnly Property AutoRecordingBackground As SolidColorBrush
        Get
            If IsSelected Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return New SolidColorBrush(Colors.Transparent)
            End If

        End Get
    End Property

    Public Sub New()

    End Sub


    Public Sub New(recording As TVHAutoRecording)
        _autorecording = recording
    End Sub

    ''' <summary>
    ''' Sends a command to the TVH server to delete the AutoRecording with the given uuid
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Delete() As Task(Of tvhCommandResponse)
        WriteToDebug("AutoRecordingViewModel.Delete()", "executed")
        Dim tvh40api As New api40
        Dim retValue As New tvhCommandResponse
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiDeleteAutoRecording(uuid))).Content.ReadAsStringAsync
            retValue.success = 1
        Catch ex As Exception
            WriteToDebug("AutoRecordingViewModel.Delete()", ex.Message.ToString)
            retValue.success = 0
        End Try
        Return retValue
    End Function

    ''' <summary>
    ''' Saves the AutoRecording on the TVH Server.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Save() As Task(Of tvhCommandResponse)
        WriteToDebug("AutoRecordingViewModel.Save()", "executed")
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim tvh40api As New api40
        Dim response As New tvhCommandResponse
        'Hack to re-create the integer array based on the list of bools that was used when editing the autorecording
        weekdays.Clear()
        For i As Integer = 0 To _RecordingOnDay.Count - 1
            If _RecordingOnDay(i) = True Then weekdays.Add(i + 1)
        Next
        Try
            Dim json_result As String
            If Me.uuid = "" Then
                'If the autorecording we are trying to save doesn't have a uuid, we assume it's a blank (new) autorecording
                json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiCreateAutoRecording(Me))).Content.ReadAsStringAsync
            Else
                json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiUpdateAutoRecording(Me))).Content.ReadAsStringAsync
            End If
            If Not (json_result Is Nothing) Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
                response.success = 1
            End If
        Catch ex As Exception
            WriteToDebug("AutoRecordingViewModel.Save()", ex.InnerException.ToString)
            response.success = 0
        End Try
        Return response
    End Function

End Class