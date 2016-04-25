Public Class RecordingEditViewModel
    Inherits RecordingViewModel


    Public ReadOnly Property dvrconfiglist As List(Of DVRConfigViewModel)
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.DVRConfigs.items.ToList()
        End Get
    End Property
    Public Property selecteddvrconfig As DVRConfigViewModel
        Get
            If Not _recording.config_name = "" Then
                Dim x = dvrconfiglist.Where(Function(y) y.identifier = _recording.config_name).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return dvrconfiglist(0)
            Else
                Return dvrconfiglist(0)
            End If
        End Get
        Set(value As DVRConfigViewModel)
            _recording.config_name = value.identifier
        End Set
    End Property


    Private Property _channellist As List(Of ChannelViewModel)
    Public ReadOnly Property channellist As List(Of ChannelViewModel)
        Get
            If _channellist Is Nothing Then
                _channellist = New List(Of ChannelViewModel)
                Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
                _channellist.AddRange(vm.Channels._allchannels)
            End If
            Return _channellist
        End Get
    End Property
    Public Property selectedchannel As ChannelViewModel
        Get
            If Not _recording.channel = "" Then
                Dim x As ChannelViewModel = channellist.Where(Function(y) y.uuid = _recording.channel).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return channellist(0)
            Else
                _recording.channel = channellist(0).uuid
                Return channellist(0)
            End If
        End Get
        Set(value As ChannelViewModel)
            _recording.channel = value.uuid
        End Set
    End Property


    Public Property start_date As DateTimeOffset
        Get
            Dim d As DateTime = UnixToUniversalDateTime(_recording.start)
            Return d
        End Get
        Set(value As DateTimeOffset)
            Dim time As TimeSpan = startTime
            value = value.Date.Add(time)
            _recording.start = TimeToUnix(value.DateTime)
            WriteToDebug(TimeToUnix(value.DateTime), UnixToLocalDateTime(_recording.start).ToString)
        End Set
    End Property
    Public Property startTime As TimeSpan
        Get
            Dim startDate As DateTime = UnixToLocalDateTime(_recording.start)
            Return New TimeSpan(startDate.Hour, startDate.Minute, startDate.Second)
        End Get
        Set(value As TimeSpan)
            Dim dt As Date = start_date.DateTime.Date
            dt = dt.Add(value)
            _recording.start = TimeToUnix(dt.ToUniversalTime)
            WriteToDebug(TimeToUnix(dt), UnixToLocalDateTime(_recording.start).ToString)
        End Set
    End Property

    Public Property stop_date As DateTimeOffset
        Get
            Dim d As DateTime = UnixToUniversalDateTime(_recording.stop)
            Return d
        End Get
        Set(value As DateTimeOffset)
            Dim time As TimeSpan = stopTime
            value = value.Date.Add(time)
            _recording.stop = TimeToUnix(value.DateTime)
            WriteToDebug(TimeToUnix(value.DateTime), UnixToLocalDateTime(_recording.stop).ToString)
        End Set
    End Property

    Public Property stopTime As TimeSpan
        Get
            Dim stopDate As DateTime = UnixToLocalDateTime(_recording.stop)
            Return New TimeSpan(stopDate.Hour, stopDate.Minute, stopDate.Second)
        End Get
        Set(value As TimeSpan)
            Dim dt As Date = stop_date.DateTime.Date
            dt = dt.Add(value)
            _recording.stop = TimeToUnix(dt.ToUniversalTime)
            WriteToDebug(TimeToUnix(dt), UnixToLocalDateTime(_recording.stop).ToString)
        End Set
    End Property

    Public Sub New(recording As RecordingViewModel)
        _recording = recording._recording
        _recording.start = If(recording.start = Nothing, TimeToUnix(Date.Now.ToUniversalTime), recording.start)
        _recording.stop = If(recording.stop = Nothing, TimeToUnix(Date.Now.ToUniversalTime.AddHours(1)), recording.stop)
        WriteToDebug(TimeToUnix(Date.Now), "")
    End Sub
End Class
