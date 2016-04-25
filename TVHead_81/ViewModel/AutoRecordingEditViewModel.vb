Public Class AutoRecordingEditViewModel
    Inherits AutoRecordingViewModel

    Public ReadOnly Property dvrconfiglist As List(Of DVRConfigViewModel)
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.DVRConfigs.items.ToList()
        End Get
    End Property
    Public Property selecteddvrconfig As DVRConfigViewModel
        Get
            If Not _autorecording.config_name = "" Then
                Dim x = dvrconfiglist.Where(Function(y) y.identifier = _autorecording.config_name).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return dvrconfiglist(0)
            Else
                Return dvrconfiglist(0)
            End If
        End Get
        Set(value As DVRConfigViewModel)
            _autorecording.config_name = value.identifier
        End Set
    End Property

    Private Property _contenttypelist As List(Of ContentTypeViewModel)
    Public ReadOnly Property contenttypelist As List(Of ContentTypeViewModel)
        Get
            If _contenttypelist Is Nothing Then
                Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
                Dim list As New List(Of ContentTypeViewModel)
                list.Add(New ContentTypeViewModel(New TVHGenre With {.key = 0, .val = "--any--"}))
                list.AddRange(vm.ContentTypes.items)
                _contenttypelist = list
            End If
            Return _contenttypelist
        End Get
    End Property
    Public Property selectedcontenttype As ContentTypeViewModel
        Get
            If Not _autorecording.content_type = 0 Then
                Dim x = contenttypelist.Where(Function(y) y.uuid = _autorecording.content_type).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return contenttypelist(0)
            Else
                Return contenttypelist(0)
            End If

        End Get
        Set(value As ContentTypeViewModel)
            _autorecording.content_type = value.uuid
        End Set
    End Property

    Private Property _channellist As List(Of ChannelViewModel)
    Public ReadOnly Property channellist As List(Of ChannelViewModel)
        Get
            If _channellist Is Nothing Then
                _channellist = New List(Of ChannelViewModel)
                _channellist.Add(New ChannelViewModel(New TVHChannel With {.name = "--any--", .uuid = ""}))
                Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
                _channellist.AddRange(vm.Channels._allchannels)
            End If
            Return _channellist
        End Get
    End Property
    Public Property selectedchannel As ChannelViewModel
        Get
            If Not _autorecording.channel = "" Then
                Dim x As ChannelViewModel = channellist.Where(Function(y) y.uuid = _autorecording.channel).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return channellist(0)
            Else
                Return channellist(0)
            End If
        End Get
        Set(value As ChannelViewModel)
            _autorecording.channel = value.uuid
        End Set
    End Property

    Private Property _channeltaglist As List(Of ChannelTagViewModel)
    Public ReadOnly Property channeltaglist As List(Of ChannelTagViewModel)
        Get
            If _channeltaglist Is Nothing Then
                _channeltaglist = New List(Of ChannelTagViewModel)
                _channeltaglist.Add(New ChannelTagViewModel With {.name = "--any--", .uuid = ""})
                Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
                _channeltaglist.AddRange(vm.ChannelTags.items)
            End If
            Return _channeltaglist
        End Get
    End Property
    Public Property selectedchanneltag As ChannelTagViewModel
        Get
            If Not _autorecording.tag = "" Then
                Dim x As ChannelTagViewModel = channeltaglist.Where(Function(y) y.uuid = _autorecording.tag).FirstOrDefault()
                If Not x Is Nothing Then Return x Else Return channeltaglist(0)
            Else
                Return channeltaglist(0)
            End If
        End Get
        Set(value As ChannelTagViewModel)
            _autorecording.tag = value.uuid
        End Set
    End Property

    Public Property StartBeforeEnabled As Boolean
        Get
            Return If(_autorecording.start_window <> "Any", True, False)
        End Get
        Set(value As Boolean)
            _autorecording.start_window = If(value = False, "Any", (New DateTime(_start_window_Time.Ticks).ToString("HH:mm")))
            RaisePropertyChanged("StartBeforeEnabled")
        End Set
    End Property

    Public Property StartAfterEnabled As Boolean
        Get
            Return If(_autorecording.start <> "Any", True, False)
        End Get
        Set(value As Boolean)
            _autorecording.start = If(value = False, "Any", (New DateTime(startTime.Ticks).ToString("HH:mm")))
            RaisePropertyChanged("StartAfterEnabled")
        End Set
    End Property

    Private Property _startTime As TimeSpan
    Public Property startTime As TimeSpan
        Get
            If _autorecording.start <> "Any" Then
                Dim hours As Integer = Integer.Parse(_autorecording.start.Split(":")(0))
                Dim minutes As Integer = Integer.Parse(_autorecording.start.Split(":")(1))
                Return New TimeSpan(hours, minutes, 0)
            Else
                Return New TimeSpan(0, 0, 0)
            End If
        End Get
        Set(value As TimeSpan)
            Dim d As New DateTime(value.Ticks)
            _autorecording.start = d.ToString("HH:mm")
        End Set
    End Property

    Private Property _start_window_Time As TimeSpan
    Public Property start_window_Time As TimeSpan
        Get
            If _autorecording.start_window <> "Any" Then
                Dim hours As Integer = Integer.Parse(_autorecording.start_window.Split(":")(0))
                Dim minutes As Integer = Integer.Parse(_autorecording.start_window.Split(":")(1))
                Return New TimeSpan(hours, minutes, 0)
            Else
                Return New TimeSpan(0, 0, 0)
            End If
        End Get
        Set(value As TimeSpan)
            Dim d As New DateTime(value.Ticks)
            _autorecording.start_window = d.ToString("HH:mm")
        End Set
    End Property

    Public Sub New(autorec As AutoRecordingViewModel)
        _autorecording = autorec._autorecording
    End Sub
End Class
