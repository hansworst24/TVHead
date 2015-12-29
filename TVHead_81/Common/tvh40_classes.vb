Imports Newtonsoft.Json
Imports TVHead_81.ViewModels

Namespace tvh40

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' CLASSES REQUIRED TO CAPTURE JSON RESULTS OF TVHEADEND SERVER 3.9.21xx
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class Adapter
        Public Property notificationClass As String 'Is only used when consuming /comet/poll long polling messages
        Public Property uuid As String
        Public Property input As String
        Public Property stream As String
        Public Property subs As Integer
        Public Property weight As Integer
        Public Property signal As Integer
        Public Property signal_scale As Integer
        Public Property ber As Long
        Public Property snr As Integer
        Public Property snr_scale As Integer
        Public Property unc As Integer
        Public Property bps As Integer
        Public Property te As Integer
        Public Property cc As Integer
        Public Property ec_bit As Integer
        Public Property tc_bit As Integer
        Public Property ec_block As Integer
        Public Property tc_block As Integer
    End Class

    Public Class AdapterList
        Public Property entries As List(Of Adapter)
    End Class

    Public Class AutoRecordingList
        Public Property entries As List(Of AutoRecording)
        Public Property total As Integer
    End Class

    Public Class AutoRecording
        Public Property brand As String
        Public Property channel As String
        Public Property comment As String
        Public Property config_name As String
        Public Property content_type As Integer
        Public Property creator As String
        Public Property enabled As Boolean
        Public Property maxduration As Integer
        Public Property name As String
        Public Property minduration As Integer
        Public Property pri As Integer
        Public Property retention As Integer
        Public Property season As String
        Public Property serieslink As String
        Public Property start As String
        Public Property start_window As String
        Public Property start_extra As Integer
        Public Property start_real As Long
        Public Property status As String
        Public Property stop_extra As Integer
        Public Property tag As String
        Public Property timerec As String
        Public Property title As String
        Public Property uuid As String
        Public Property weekdays As List(Of Integer)
    End Class

    Public Class Channel

        Public Property uuid As String
        Public Property enabled As Boolean
        Public Property name As String
        Public Property number As Integer
        Public Property icon As String
        Public Property icon_public_url As String
        Public Property epgauto As Boolean
        Public Property epggrab As List(Of String)
        Public Property dvr_pre_time As Integer
        Public Property dvr_pst_time As Integer
        Public Property services As String()
        Public Property tags As String()
        Public Property bouquet As String
    End Class

    Public Class ChannelList
        Public Property entries As Channel()
        Public Property total As Integer
    End Class

    Public Class ChannelTagList
        Public Property entries As New List(Of ChannelTag)
        Public Property total As Integer
    End Class

    Public Class ChannelTag
        Public Property comment As String
        Public Property enabled As Boolean
        Public Property icon As String
        Public Property icon_public_url As String
        Public Property internal As Boolean
        Public Property name As String
        Public Property [private] As Boolean
        Public Property titled_icon As Boolean
        Public Property uuid As String
    End Class

    Public Class CometPollResponse
        '{"boxid":"fffdd6a9c5cca67c954cd4993791b0b6fe856855","messages":[{"notificationClass":"accessUpdate","username":"","address":"192.168.168.2","dvr":1,"admin":1,"time":1447886277,"cookie_expires":7,"info_area":"login,storage,time","freediskspace":3112701001728,"totaldiskspace":12000220413952},{"notificationClass":"setServerIpPort","ip":"192.168.168.2","port":9981}]}pi@raspberrypi ~ $ curl -GET "http://192.168.168.2:9981/comet/poll?boxid=&immediate=0"

        Public Property boxid As String
        'Public Property messages As String()

    End Class

    Public Class CometInputStatus
        Public Property boxid As String
        Public Property messages As Adapter()
    End Class

    Public Class ConnectionList
        Public Property entries As List(Of Connection)

    End Class

    Public Class Connection
        Public Property id As Integer
        Public Property peer As String
        Public Property started As Long
        Public Property type As String
        Public Property user As String
    End Class

    Public Class DVRConfig
        Public Property key As String
        Public Property val As String
    End Class

    Public Class DVRConfigList
        Public Property entries As New List(Of DVRConfig)
    End Class

    Public Class EPGEvent
        Public Sub New()

        End Sub

        Public Property eventId As Long                  '137935
        Public Property episodeId As Long                '137936
        Public Property episodeUri As String                'crid://bds.tv/50527985#00100000000CC7EF"
        Public Property serieslinkId As Integer             '473686
        Public Property serieslinkUri As String             'crid://eventis.nl/00000000-0000-1000-0008-00000000B083"
        Public Property channelName As String               'TV Oranje
        Public Property channelUuid As String               '234ae68c0c685386fda3a4f622eb8910
        Public Property channelNumber As String             '361
        Public Property channelIcon As String               '/1_0_1_4D23_12_600_FFFF0000_0_0_0.png or imagecache/276
        Public Property start As Long                       '1416898800
        Public Property [stop] As Long                      '1416934800
        Public Property title As String                     'TV Oranje Nonstop
        Public Property subtitle As String                  'Dom programma
        Public Property description As String               'Nederlands muziekprogramma. Muziekprogramma met non-stop clips van eigen bodem. Stem op je favoriete clip via de website: www.tvoranje.nl.
        Public Property genre As List(Of Integer)            '[96]
        Public Property dvrUuid As String
        Public Property dvrState As String
        Public Property nextEventId As Integer              '139715


    End Class

    Public Class EPGEventList
        Public Property entries As List(Of EPGEvent)
        Public Property totalCount As Integer
    End Class

    Public Class Genre
        Public Property key As Integer                       'key=00a2cc05aa0a930aca7d371fa4c0b0f7
        Public Property val As String
    End Class

    Public Class GenreList
        Public Property entries As List(Of Genre)                      'key=16
    End Class

    Public Class Mux
        Public Property constellation As String
        Public Property delsys As String
        Public Property enabled As Boolean
        Public Property epg As Integer
        Public Property fec As String
        Public Property frequency As Integer
        Public Property name As String
        Public Property network As String
        Public Property num_chn As Integer
        Public Property num_svc As Integer
        Public Property onid As Integer
        Public Property pmt_06_ac3 As Boolean
        Public Property scan_result As Integer
        Public Property scan_state As Integer
        Public Property symbolrate As Integer
        Public Property tsid As Integer
        Public Property uuid As String
    End Class

    Public Class MuxList
        Public Property entries As List(Of Mux)
    End Class

    Public Class RootObject
        Public Property values As List(Of Array)
    End Class

    Public Class Recording
        Public Property autorec As String
        Public Property broadcast As Integer
        Public Property channel As String
        Public Property channel_icon As String
        Public Property channelname As String
        Public Property config_name As String
        Public Property container As Integer
        Public Property content_type As Integer
        Public Property creator As String
        Public Property description As RootObject
        Public Property disp_description As String
        Public Property disp_subtitle As String
        Public Property disp_title As String
        Public Property duplicate As Integer
        Public Property duration As Integer
        Public Property dvb_eid As Integer
        Public Property episode As Long
        Public Property errorcode As Integer
        Public Property errors As Integer
        Public Property filename As String
        Public Property filesize As Long
        Public Property noresched As Boolean
        Public Property pri As Integer
        Public Property retention As Integer
        Public Property sched_status As String
        Public Property start As Long
        Public Property start_extra As Integer
        Public Property start_real As Long
        Public Property status As String
        Public Property [stop] As Long
        Public Property stop_extra As Integer
        Public Property stop_real As Long
        Public Property timerec As String
        Public Property title As RootObject
        Public Property url As String
        Public Property uuid As String
    End Class

    Public Class RecordingList
        Public Property entries As List(Of Recording)
        Public Property total As Integer
    End Class

    Public Class ServerInfo
        Public Property sw_version As String
        Public Property name As String
        Public Property api_version As Integer
    End Class

    Public Class Service
        Public Property auto As Integer
        Public Property caid As String
        Public Property created As Integer
        Public Property dvb_ignore_eit As Boolean
        Public Property dvb_servicetype As Integer
        Public Property enabled As Boolean
        Public Property encrypted As Boolean
        Public Property force_caid As Integer
        Public Property last_seen As Integer
        Public Property lcn As Integer
        Public Property lcn_minor As Integer
        Public Property lcn2 As Integer
        Public Property multiplex As String
        Public Property network As String
        Public Property prefcapid As Integer
        Public Property prefcapid_lock As Integer
        Public Property provider As String
        Public Property sid As Integer
        Public Property svcname As String
        Public Property uuid As String
    End Class

    Public Class ServiceList
        Public Property entries As List(Of Service)
    End Class

    Public Class ServiceDetail
        Public Property pid As Integer
        Public Property type As String
        Public Property language As String
        Public Property index As Integer
        Public Property details As String
    End Class

    Public Class ServiceDetailList
        'servicedetails/_dev_dvb_adapter1_Sit2_DVB_T2_C396000000_00c3?_dc=1419938362449 
        Public Property fstreams As List(Of ServiceDetail)
    End Class

    Public Class Subscription
        '       api/status/subscriptions'
        ' {"entries": [{"id": 718,"start": 1419862190,"errors": 0,"state": "Running","title": "DVR: kees","channel": "NPO 3","service": "ZIGGO/404MHz/NPO 3"},{"id": 709,"start": 1419850839,"errors": 0,"state": "Running","title": "DVR: fff","channel": "RTL 5","service": "ZIGGO/778MHz/RTL 5"}],"totalCount": 2}root@KIPKLUIF:~#
        Public Property id As Integer
        Public Property start As Integer
        Public Property errors As Integer
        Public Property state As String
        Public Property title As String
        Public Property channel As String
        Public Property service As String
        Public Property hostname As String
        Public Property username As String
        Public Property descramble As String

    End Class

    Public Class SubscriptionList
        Public Property entries As List(Of Subscription)
    End Class


    Public Class api40
        Public settings As New AppSettings

        Public ReadOnly Property apiGetStreamStatus() As String
            Get
                Return settings.GetFullURL() + "/api/status/inputs"
            End Get
        End Property

        Public ReadOnly Property apiGetServerInfo() As String
            Get
                Return settings.GetFullURL() + String.Format("/api/serverinfo")
            End Get
        End Property

        Public ReadOnly Property apiGetBoxID() As String
            Get
                Return settings.GetFullURL() + String.Format("/comet/poll?boxid=&immediate=0")
            End Get
        End Property

        Public ReadOnly Property apiGetCometPoll(boxid As String) As String
            Get
                Return settings.GetFullURL() + String.Format("/comet/poll?boxid={0}&immediate=0", boxid)
            End Get
        End Property

        Public ReadOnly Property apiGetServiceDetails(svc As ServiceViewModel) As String
            '/api/service/streams?uuid=UUID'
            Get
                Return settings.GetFullURL() + String.Format("/api/service/streams?uuid={0}", svc.uuid)
            End Get
        End Property

        Public ReadOnly Property apiGetConnections() As String
            '/api/mpegts/service/grid?sort=channel&dir=ASC'
            Get
                Return settings.GetFullURL() + "/api/status/connections"
            End Get
        End Property

        Public ReadOnly Property apiGetServices() As String
            '/api/mpegts/service/grid?sort=channel&dir=ASC'
            Get
                Return settings.GetFullURL() + "/api/mpegts/service/grid?sort=channel&dir=ASC&limit=1000"
            End Get
        End Property

        Public ReadOnly Property apiGetMuxes() As String
            '/api/mpegts/service/grid?sort=channel&dir=ASC'
            Get
                Return settings.GetFullURL() + "/api/mpegts/mux/grid?sort=tsid&dir=ASC&limit=1000"
            End Get
        End Property

        Public ReadOnly Property apiGetSubscriptions() As String
            '       api/status/subscriptions'
            ' {"entries": [{"id": 718,"start": 1419862190,"errors": 0,"state": "Running","title": "DVR: kees","channel": "NPO 3","service": "ZIGGO/404MHz/NPO 3"},{"id": 709,"start": 1419850839,"errors": 0,"state": "Running","title": "DVR: fff","channel": "RTL 5","service": "ZIGGO/778MHz/RTL 5"}],"totalCount": 2}root@KIPKLUIF:~#

            Get
                Return settings.GetFullURL() + "/api/status/subscriptions"
            End Get
        End Property

        Public ReadOnly Property apiGetContentTypes(full As Boolean) As String
            'Returns the URL for Fetching a list of ContentTypes, or Genres
            'The full list, will contain duplicated, but which have a different ID
            ' The -non-full- list, will only contain the main genres and will be used for recordings
            Get
                If full Then
                    Return settings.GetFullURL() + "/api/epg/content_type/list?full=1"
                Else
                    Return settings.GetFullURL() + "/api/epg/content_type/list"
                End If
            End Get
        End Property

        Public ReadOnly Property apiGetUpcomingRecordings() As String
            Get
                Return settings.GetFullURL() + "/api/dvr/entry/grid_upcoming?sort=start_real&dir=ASC&limit=999999999"
            End Get
        End Property

        Public ReadOnly Property apiGetFailedRecordings() As String
            Get
                Return settings.GetFullURL() + "/api/dvr/entry/grid_failed?sort=start_real&dir=ASC&limit=999999999"
            End Get
        End Property

        Public ReadOnly Property apiGetFinishedRecordings() As String
            Get
                Return settings.GetFullURL() + "/api/dvr/entry/grid_finished?start=0&limit=999999999&sort=start_real&dir=DESC"
            End Get
        End Property

        Public ReadOnly Property apiGetAutoRecordings() As String
            Get
                Return settings.GetFullURL() + "/api/dvr/autorec/grid?sort=name&dir=ASC&limit=999999999"
            End Get
        End Property

        Public ReadOnly Property apiGetDVRConfigs() As String
            Get
                Return settings.GetFullURL() + "/api/idnode/load?enum=1&class=dvrconfig"
            End Get
        End Property

        Public ReadOnly Property apiGetChannelTags() As String
            Get
                Return settings.GetFullURL() + "/api/channeltag/grid"
            End Get
        End Property

        Public ReadOnly Property apiGetChannels() As String
            Get
                Return settings.GetFullURL() + "/api/channel/grid?sort=number&dir=ASC&limit=500000"
            End Get
        End Property

        Public ReadOnly Property apiRecordProgramByEvent(eventId As String, Optional dvrConfig As DVRConfigViewModel = Nothing) As String
            'http://192.168.168.54:9981/api/dvr/entry/create_by_event?event_id=22971&config_uuid=
            Get
                Dim dvruuid As String = ""
                If Not dvrConfig Is Nothing Then dvruuid = dvrConfig.identifier

                Return settings.GetFullURL() + "/api/dvr/entry/create_by_event?event_id=" + eventId + "&config_uuid=" + dvruuid
            End Get
        End Property

        Public ReadOnly Property apiStopRecording(recording_id As String) As String
            Get
                Return settings.GetFullURL() + "/api/dvr/entry/stop?uuid=" + recording_id.ToString
            End Get
        End Property

        Public ReadOnly Property apiAbortRecording(recording_id As String) As String
            Get
                Return settings.GetFullURL() + "/api/dvr/entry/cancel?uuid=" + recording_id.ToString
            End Get
        End Property

        Public ReadOnly Property apiDeleteRecording(uuid As String) As String
            Get
                Return settings.GetFullURL() + "/api/idnode/delete?uuid=" + uuid
            End Get
        End Property

        Public ReadOnly Property apiLoadIDNode(uuid As String) As String
            'curl --user cre8or:12345 --data 'uuid=["638d73433d50d1ddec2f913d2c8f6667"]&grid=1&list=enabled,disp_title' 'http://192.168.168.2:9981/api/idnode/load?'

            'since commit in October 2015
            'uuid=["4ecf945d0c7823d859b6e0962d840cc4","51b2e00086b6d300fd73f5d36995c859","d4994a7669fdca0854b67ebf5b2d75c5"]&grid=1&list=enabled,duplicate,disp_title,disp_subtitle,episode,pri,start_real,stop_real,duration,filesize,channel,owner,creator,config_name,sched_status,errors,data_errors,comment

            'Response
            '{"entries":[{"uuid":"4ecf945d0c7823d859b6e0962d840cc4","enabled":true,"duplicate":0,"disp_title":"Dr Phil","disp_subtitle":"","pri":2,"start_real":1449216270,"stop_real":1449219600,"duration":3300,"filesize":990274396,"channel":"02b5af655ca949da17afac3922c5a9d5","creator":"192.168.168.62","config_name":"343f967e5efe3e8a165b86290b6a8b9d","sched_status":"recording","errors":0,"data_errors":0},{"uuid":"51b2e00086b6d300fd73f5d36995c859","enabled":true,"duplicate":0,"disp_title":"KRO Kindertijd","disp_subtitle":"","pri":2,"start_real":1449216870,"stop_real":1449218400,"duration":1500,"filesize":106060764,"channel":"01f6fd6fee85f98d8a19d4dbf020cb23","creator":"192.168.168.62","config_name":"343f967e5efe3e8a165b86290b6a8b9d","sched_status":"recording","errors":0,"data_errors":0},{"uuid":"d4994a7669fdca0854b67ebf5b2d75c5","enabled":true,"duplicate":0,"disp_title":"Nederland in Beweging","disp_subtitle":"","pri":2,"start_real":1449216870,"stop_real":1449217800,"duration":900,"filesize":106126752,"channel":"0fa0553848cc5d5645ec5bf2a03c97e7","creator":"192.168.168.62","config_name":"343f967e5efe3e8a165b86290b6a8b9d","sched_status":"recording","errors":0,"data_errors":0}]}
            Get
                'Return settings.GetFullURL() + "/api/idnode/load?uuid=" + uuid
                Return settings.GetFullURL() + String.Format("/api/idnode/load?uuid={0}", "[""" + uuid + """]&grid=1")
            End Get
        End Property

        Public ReadOnly Property apiDeleteAutoRecording(uuid As String) As String
            Get
                Return settings.GetFullURL() + "/api/idnode/delete?uuid=" + uuid
            End Get
        End Property

        Public ReadOnly Property apiAddManualRecording(rec As RecordingViewModel) As String
            Get
                'conf={"disp_title":"blaat","start":1418170200,"start_extra":0,"stop":1419381600,"stop_extra":0,"channel":"b8f6c9a16f836735bc546b9a51abed26","config_name":"5281d3bed2bb4cff1a70f9f64ab4d760","comment":""}
                Return settings.GetFullURL() + "/api/dvr/entry/create?conf=" + Uri.EscapeDataString(String.Format("{{""disp_title"":""{0}"",""start"":{1},""stop"":{2},""channel"":""{3}"",""config_name"":""{4}"",""comment"":""{5}""}}",
                                                             rec.title,
                                                             TimeToUnix(rec.startDate.Date.Add(New TimeSpan(rec.startTime.Hour, rec.startTime.Minute, rec.startTime.Second)).ToUniversalTime),
                                                             TimeToUnix(rec.stopDate.Date.Add(New TimeSpan(rec.stopTime.Hour, rec.stopTime.Minute, rec.stopTime.Second)).ToUniversalTime),
                                                             rec.channelUuid,
                                                             rec.configUuid,
                                                             "Created by TV Head"))

            End Get

        End Property

        Public ReadOnly Property apiUpdateManualRecording(r As RecordingViewModel) As String
            Get
                Return settings.GetFullURL() + String.Format("/api/idnode/save?node={{" &
                    """disp_title"":""{0}""" &
                    ",""start"":""{1}""" &
                    ",""start_extra"":""{2}""" &
                    ",""stop"":""{3}""" &
                    ",""stop_extra"":""{4}""" &
                    ",""channel"":{5}" &
                    ",""config_name"":{6}" &
                    ",""comment"":""{7}""" &
                    ",""uuid"":""{8}""}}",
                    r.title,
                    TimeToUnix(r.startDate.Date.Add(New TimeSpan(r.startTime.Hour, r.startTime.Minute, r.startTime.Second)).ToUniversalTime),
                    0,
                    TimeToUnix(r.stopDate.Date.Add(New TimeSpan(r.stopTime.Hour, r.stopTime.Minute, r.stopTime.Second)).ToUniversalTime),
                    0,
                    r.channelUuid,
                    r.configUuid,
                    "Edited by TV Head",
                    r.recording_id)

            End Get
        End Property

        Public ReadOnly Property apiUpdateAutoRecording(r As AutoRecordingViewModel) As String
            Get
                Return settings.GetFullURL() + String.Format("/api/idnode/save?node=[{{" &
                    """enabled"":{0}" &
                    ",""name"":""{1}""" &
                    ",""title"":""{2}""" &
                    ",""channel"":""{3}""" &
                    ",""tag"":""{4}""" &
                    ",""content_type"":{5}" &
                    ",""weekdays"":{6}" &
                    ",""start"":""{7}""" &
                    ",""start_window"":""{8}""" &
                    ",""config_name"":""{9}""" &
                    ",""uuid"":""{10}""" &
                    ",""comment"":""{11}""}}]",
                    r.enabled.ToString.ToLower,
                    System.Net.WebUtility.UrlEncode(r.name),
                    System.Net.WebUtility.UrlEncode(r.title),
                    r.channel,
                    r.tagUuid,
                    r.contenttype,
                    IntListToStringArray(r.weekdays),
                    r.start,
                    r.start_window,
                    r.configUuid,
                    r.id,
                    System.Net.WebUtility.UrlEncode("Edited by TV Head"))
            End Get
        End Property

        Public ReadOnly Property apiCreateAutoRecording(r As AutoRecordingViewModel) As String
            Get
                Return settings.GetFullURL() + "/api/dvr/autorec/create?conf=" + Uri.EscapeDataString(String.Format("{{" &
                   """enabled"":{0}" &
                   ",""name"":""{1}""" &
                   ",""title"":""{2}""" &
                   ",""channel"":""{3}""" &
                   ",""tag"":""{4}""" &
                   ",""content_type"":{5}" &
                   ",""weekdays"":{6}" &
                   ",""start"":""{7}""" &
                   ",""start_window"":""{8}""" &
                   ",""config_name"":""{9}""" &
                   ",""comment"":""{10}""}}",
                   r.enabled.ToString.ToLower,
                   r.name,
                   r.title,
                   r.channel,
                   r.tagUuid,
                   r.contenttype,
                   IntListToStringArray(r.weekdays),
                   r.start,
                   r.start_window,
                   r.configUuid,
                   "Edited by TV Head"))
            End Get
        End Property

        Public ReadOnly Property apiCreateAutoRecordingBySeries(eventId As String, Optional dvrconfig As DVRConfigViewModel = Nothing) As String
            Get
                Dim config_uuid As String = ""
                If Not dvrconfig Is Nothing Then config_uuid = dvrconfig.identifier
                Return settings.GetFullURL() + String.Format("/api/dvr/autorec/create_by_series?event_id={0}&config_uuid={1}", eventId, config_uuid)
            End Get
        End Property

        Public ReadOnly Property apiSearchEPGEvents(searchValue As String, fulltext As Boolean)
            Get
                Return settings.GetFullURL() + String.Format("/api/epg/events/grid?start=0&limit=300&title={0}&fulltext={1}", searchValue, fulltext.ToString.ToLower())
            End Get
        End Property

        Public ReadOnly Property apiGetEPGEvent(eventids As List(Of Integer))
            'Seems only to be supported in TVH 4.x -ish
            Get
                If eventids.Count > 1 Then
                    'WriteToDebug("", "[" + String.Join(",", eventids) + "]")
                End If



                Return settings.GetFullURL() + String.Format("/api/epg/events/load?eventId={0}", "[" + String.Join(",", eventids) + "]")
            End Get
        End Property

        Public ReadOnly Property apiGetEPGEvents(channelid As String, all As Boolean, Optional maxItems As Integer = 300)
            Get
                If all Then
                    'Return all EPG Events
                    If channelid = "" Then
                        Return settings.GetFullURL() + "/api/epg/events/grid?start=0&sort=&dir=ASC&limit=" & 99999
                    Else
                        Return settings.GetFullURL() + "/api/epg/events/grid?start=0&sort=&dir=ASC&limit=" & settings.MaxEPGItemsPerChannel & "&channel=" & channelid
                        'Return settings.GetFullURL() + "/api/epg/events/grid?start=0&limit=" & 5 & "&channel=" & channelid
                    End If
                Else
                    'Only return the first EPG Event
                    If channelid = "" Then
                        Return settings.GetFullURL() + "/api/epg/events/grid?_dc=" & TimeToUnixMs(Date.Now) & "&sort=&dir=ASC&limit=" & maxItems
                    Else
                        Return settings.GetFullURL() + "/api/epg/events/grid?start=0&sort=&dir=ASC&limit=1&channel=" & channelid
                    End If

                End If

            End Get
        End Property

    End Class

End Namespace



