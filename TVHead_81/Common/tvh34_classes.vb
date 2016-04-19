Imports Newtonsoft.Json
Imports TVHead_81.ViewModels


Namespace tvh34

    Public Class Adapter
        Public Property ber As Integer
        Public Property currentMux As String
        Public Property deliverySystem As String
        Public Property devicename As String
        Public Property freqMax As Integer
        Public Property freqMin As Integer
        Public Property freqStep As Integer
        Public Property hostconnection As String
        Public Property identifier As String
        Public Property initialMuxes As Integer
        Public Property muxes As Integer
        Public Property name As String
        Public Property path As String
        Public Property satConf As Integer
        Public Property services As Integer
        Public Property signal As Integer
        Public Property snr As Integer
        Public Property symrateMax As Integer
        Public Property symrateMin As Integer
        Public Property type As String
        Public Property unc As Integer
        Public Property uncavg As Integer
    End Class

    Public Class AdapterList
        Public Property entries As List(Of Adapter)
    End Class

    Public Class Service
        Public Property channel As Integer
        Public Property channelname As String
        Public Property dvb_eit_enable As Integer
        Public Property enabled As Integer
        Public Property encryption As String
        Public Property id As String
        Public Property mux As String
        Public Property network As String
        Public Property pcr As Integer
        Public Property pmt As Integer
        Public Property prefcapid As Integer
        Public Property provider As String
        Public Property sid As Integer
        Public Property svcname As String
        Public Property type As String
        Public Property typenum As Integer
        Public Property typestr As String
    End Class

    Public Class ServiceList
        Public Property entries As List(Of Service)
    End Class

    Public Class ServiceDetail
        Public Property pid As Integer
        Public Property type As String
        Public Property details As String
    End Class

    Public Class ServiceDetailList
        'servicedetails/_dev_dvb_adapter1_Sit2_DVB_T2_C396000000_00c3?_dc=1419938362449 
        Public Property streams As List(Of ServiceDetail)
    End Class

    Public Class Subscription
        '    {"entries": [{"id": 44,"start": 1419868265,"errors": 0,"state": "Running","hostname": "192.168.168.70","username": "cre8or","title": 
        '"XBMC Media Center","channel": "NPO 1 HD","service": "DVBSky T982C DVB-T2/C #1/546,000 kHz/NPO 1 HD"}]}
        Public Property id As Integer
        Public Property start As Integer
        Public Property errors As Integer
        Public Property state As String
        Public Property hostname As String
        Public Property username As String
        Public Property title As String
        Public Property channel As String
        Public Property service As String

    End Class

    Public Class SubscriptionList
        Public Property entries As List(Of Subscription)
    End Class

    Public Class Genre
        Public Property code As Integer                       'key=00a2cc05aa0a930aca7d371fa4c0b0f7
        Public Property name As String
    End Class

    Public Class GenreList
        Public Property entries As List(Of Genre)                      'key=16
    End Class

    Public Class AutoRecording
        Public Property id As Integer
        Public Property enabled As Boolean
        Public Property contenttype As Integer
        Public Property config_name As String
        Public Property channel As String
        Public Property tag As String
        Public Property pri As String
        Public Property title As String
        Public Property approx_time As Integer
        Public Property weekdays As String
    End Class

    Public Class AutoRecordingList
        Public Property entries As List(Of AutoRecording)
    End Class

    Public Class Channel
        Public Property name As String
        Public Property chid As Integer
        Public Property chicon As String
        Public Property ch_icon As String
            Get
                If _ch_icon = "" Then
                    Return "/Images/tvheadend.png"
                Else
                    Return _ch_icon
                End If
            End Get
            Set(value As String)
                If value.IndexOf("http") >= 0 Then
                    _ch_icon = value
                Else
                    _ch_icon = (New AppSettings).GetFullURL() & "/" & value
                End If

            End Set
        End Property
        Private Property _ch_icon As String
        Public Property tags As String
        Public Property epg_pre_start As Integer
        Public Property epg_post_end As Integer
        Public Property number As Integer
    End Class

    Public Class ChannelList
        Public Property entries As List(Of Channel)
    End Class

    Public Class Mux
        Public Property adapterId As String
        Public Property enabled As Integer
        Public Property fe_status As String
        Public Property freq As String
        Public Property id As String
        Public Property [mod] As String
        Public Property muxid As Integer
        Public Property network As String
        Public Property onid As Integer
        Public Property pol As String
        Public Property quality As Integer
    End Class

    Public Class MuxList
        Public Property entries As List(Of Mux)
    End Class

    Public Class NewAutoRec
        Public Property id As Integer
        Public Property enabled As Boolean
        Public Property contenttype As Integer
        Public Property channel As String
        Public Property tag As String
        Public Property pri As String
        Public Property title As String
        Public Property approx_time As Integer
        Public Property weekdays As List(Of Integer)
    End Class

    Public Class EPGEventList
        Public Property totalCount As Integer
        Public Property entries As List(Of EPGEvent)
    End Class

    Public Class EPGEvent
        Public Property channel As String
        Public Property channelid As Integer
        Public Property schedstate As String
        Public Property chicon As String
            Get
                Return _chicon
            End Get
            Set(value As String)
                If Not value = "" Then
                    If value.ToUpper().IndexOf("HTTP:/") >= 0 Then
                        _chicon = value
                    Else
                        _chicon = (New AppSettings).GetFullURL() & "/" & value
                    End If

                Else
                    _chicon = "/Images/tvheadend.png"
                End If
            End Set
        End Property
        Private Property _chicon As String
        Public Property title As String
        Public Property subtitle As String                  'Dom programma
        Public Property description As String
        Public Property id As Integer
        <JsonProperty(PropertyName:="start")> Public Property starttime As Integer
        <JsonProperty(PropertyName:="end")> Public Property endtime As Integer
        Public Property duration As Integer
        Public Property contenttype As Integer
        Public ReadOnly Property startDate As DateTime
            Get
                Return UnixToDateTime(starttime).ToLocalTime
            End Get
        End Property
        Public ReadOnly Property endDate As DateTime
            Get
                Return UnixToDateTime(endtime).ToLocalTime
            End Get
        End Property
        Public ReadOnly Property percentcompleted As Double
            Get
                If (starttime > 0) And (endtime > 0) And (TimeToUnix(Date.UtcNow) - starttime > 0) Then
                    Return (TimeToUnix(Date.UtcNow) - starttime) / (endtime - starttime)
                Else
                    Return 0
                End If
            End Get
        End Property
        Public Property recording_id As String
    End Class

    Public Class Recording
        Public Property channel As String
        Public Property chicon As String
        Public Property config_name As String
        Public Property title As String
        Public Property description As String
        <JsonProperty(PropertyName:="id")> Public Property recording_id As Integer
        <JsonProperty(PropertyName:="start")> Public Property starttime As Integer
        <JsonProperty(PropertyName:="end")> Public Property endtime As Integer
        Public ReadOnly Property startDate As DateTime
            Get
                Return UnixToDateTime(starttime).ToLocalTime
            End Get
        End Property
        Public ReadOnly Property endDate As DateTime
            Get
                Return UnixToDateTime(endtime).ToLocalTime
            End Get
        End Property
        Public Property duration As Integer
        Public Property creator As String
        Public Property pri As String
        Public Property status As String
        Public Property schedstate As String
        Public ReadOnly Property percentcompleted As Double
            Get
                If (starttime > 0) And (endtime > 0) And (TimeToUnix(Date.UtcNow) - starttime > 0) Then
                    If TimeToUnix(Date.UtcNow) > endtime Then
                        Return 1
                    Else
                        Return (TimeToUnix(Date.UtcNow) - starttime) / (endtime - starttime)
                    End If
                Else
                    Return 0
                End If
            End Get
        End Property
        Public Property filesize As Long    ' Used for Finished and Failed Recordings only
        Public Property url As String       ' Used for Finished and Failed Recordings only

    End Class

    Public Class RecordingList
        Public Property totalCount As Integer
        Public Property entries As List(Of Recording)
    End Class

    Public Class DVRConfig
        Public Property identifier As String
        Public Property name As String
    End Class

    Public Class DVRConfigList
        Public Property entries As List(Of DVRConfig)
    End Class

    Public Class ChannelTag
        Public Property comment As String
        Public Property enabled As Integer
        Public Property internal As Integer
        Public Property icon As String
        Public Property id As Integer
        Public Property name As String
        Public Property titledIcon As String
    End Class
    Public Class ChannelTagList
        Public Property entries As List(Of ChannelTag)
    End Class

    Public Class api34

        Public settings As New AppSettings

        Public ReadOnly Property apiGetContentTypeList() As String
            Get
                Return settings.GetFullURL() + "/ecglist"
            End Get
        End Property

        Public ReadOnly Property apiUpdateAutoRecording(x As AutoRecordingViewModel) As String
            ' /tablemgr?op=update&table=autorec&entries=[{"title":"cojones1","channel":"NPO 1 HD","tag":"HDTV",
            '"contenttype":48,"weekdays":"1,2,6,7","approx_time":"09:00",
            '"pri":"important","config_name":"frank","creator":"createdby","comment":"blaat","id":"14"}]

            Get
                Return settings.GetFullURL() + String.Format("/tablemgr?op=update&table=autorec&entries=[{{" & _
                                                             """enabled"":{0},""title"":""{1}""," & _
                                                             """channel"":""{2}"",""tag"":""{3}"",""contenttype"":{4}," & _
                                                             """weekdays"":""{5}"",""approx_time"":""{6}""," & _
                                                             """pri"":{7},""config_name"":""{8}""," & _
                                                             """creator"":""{9}"",""comment"":""{10}"",""id"":""{11}""}}]", _
                                                             x.enabled.ToString.ToLower, System.Net.WebUtility.UrlEncode(x.title), _
                                                             x.channelname, x.tagName, x.contenttype, _
                                                             IntListToString(x.weekdays), x.start, _
                                                             x.pri, x.configName, _
                                                             "Created by TV Head", "Your Comment Here", x.id)
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

        Public ReadOnly Property apiGetChannelTags() As String
            '/tablemgr?table=channeltags&op=get
            Get
                Return settings.GetFullURL() + "/tablemgr?table=channeltags&op=get"
            End Get
        End Property

        Public ReadOnly Property apiGetChannels() As String
            Get
                Return settings.GetFullURL() + "/channels?op=list"
            End Get
        End Property

        Public ReadOnly Property apiGetUpcomingRecordings() As String
            Get
                Return settings.GetFullURL() + "/dvrlist_upcoming?limit=2000"
            End Get
        End Property

        Public ReadOnly Property apiGetStreamStatus() As String
            '{"entries": [{"identifier": "_dev_dvb_adapter0_Sit2_DVB_T2_C","name": "DVBSky T982C DVB-T2/C #2","type": "dvb","services": 353,"muxes": 32,"initialMuxes": 0,"currentMux": "348,000 kHz","signal": 34,"snr": 0,"ber": 0,"unc": 0,"uncavg": 0,"path": "/dev/dvb/adapter0","hostconnection": "PCI","devicename": "Sit2 DVB-T2/C","deliverySystem": "DVB-C","satConf": 0,"freqMin": 48000,"freqMax": 870000,"freqStep": 62,"symrateMin": 870000,"symrateMax": 7500000},{"identifier": "_dev_dvb_adapter1_Sit2_DVB_T2_C","name": "DVBSky T982C DVB-T2/C #1","type": "dvb","services": 353,"muxes": 32,"initialMuxes": 0,"currentMux": "546,000 kHz","signal": 36,"snr": 0,"ber": 0,"unc": 0,"uncavg": 0,"path": "/dev/dvb/adapter1","hostconnection": "PCI","devicename": "Sit2 DVB-T2/C","deliverySystem": "DVB-C","satConf": 0,"freqMin": 48000,"freqMax": 870000,"freqStep": 62,"symrateMin": 870000,"symrateMax": 7500000}]}
            Get
                Return settings.GetFullURL() + "/tv/adapter"
            End Get
        End Property

        Public ReadOnly Property apiGetFailedRecordings() As String
            Get
                Return settings.GetFullURL() + "/dvrlist_failed?limit=2000"
            End Get
        End Property

        Public ReadOnly Property apiGetFinishedRecordings() As String
            Get
                Return settings.GetFullURL() + "/dvrlist_finished?limit=2000"
            End Get
        End Property

        Public ReadOnly Property apiGetAutoRecordings() As String
            Get
                Return settings.GetFullURL() + "/tablemgr?table=autorec&op=get"
            End Get
        End Property

        Public ReadOnly Property apiRecordProgramByEvent(eventId As String, dvrConfig As DVRConfigViewModel) As String
            Get
                Dim dvruuid As String = ""
                If Not dvrConfig Is Nothing Then dvruuid = dvrConfig.name
                Return settings.GetFullURL() + "/dvr?eventId=" + eventId.ToString + "&op=recordEvent&config_name=" + dvruuid
            End Get
        End Property

        Public ReadOnly Property apiCancelRecording(recording_id As String) As String
            Get
                Return settings.GetFullURL() + "/dvr?entryId=" + recording_id.ToString + "&op=cancelEntry"
            End Get
        End Property

        Public ReadOnly Property apiDeleteRecording(recording_id As String) As String
            Get
                Return settings.GetFullURL() + "/dvr?entryId=" + recording_id.ToString + "&op=deleteEntry"
            End Get
        End Property

        Public ReadOnly Property apiDeleteAutoRecording(recording_id As String) As String
            Get
                Return settings.GetFullURL() + "/tablemgr?op=delete&table=autorec&entries=[""" + recording_id.ToString + """]"
            End Get
        End Property

        Public ReadOnly Property apiGetDVRConfigs() As String
            Get
                Return settings.GetFullURL() + "/confignames?op=list"
            End Get
        End Property

        Public ReadOnly Property apiGetServices(stream As StreamViewModel) As String
            ' /dvb/services/<ADAPTER_IDENTIFIER>?op=get
            Get
                Return settings.GetFullURL() + String.Format("/dvb/services/{0}?op=get", stream.identifier)
            End Get
        End Property

        Public ReadOnly Property apiGetServiceDetails(service As ServiceViewModel) As String
            'servicedetails/_dev_dvb_adapter1_Sit2_DVB_T2_C396000000_00c3?_dc=1419938362449 
            Get
                Return settings.GetFullURL() + String.Format("/servicedetails/{0}?op=get", service.uuid)
            End Get
        End Property

        Public ReadOnly Property apiGetMuxes(stream As StreamViewModel) As String
            '/dvb/muxes/<ADAPTER_IDENTIFIER>?op=get
            Get
                Return settings.GetFullURL() + String.Format("/dvb/muxes/{0}?op=get", stream.identifier)
            End Get
        End Property

        Public ReadOnly Property apiGetSubscriptions() As String
            Get
                Return settings.GetFullURL() + "/subscriptions"
            End Get
        End Property

        Public ReadOnly Property apiGetEPGEvents(channelName As String, all As Boolean)
            Get
                If all Then
                    'Return all EPG Events
                    If channelName = "" Then
                        Return settings.GetFullURL() + "/epg?start=0&limit=40000"
                    Else
                        Return settings.GetFullURL() + "/epg?start=0&limit=" & settings.MaxEPGItemsPerChannel & "&channel=" & System.Net.WebUtility.UrlEncode(channelName)
                    End If
                Else
                    'Only return the first EPG Event
                    If channelName = "" Then
                        Return settings.GetFullURL() + "/epg?start=0&limit=500" ' Only retrieve the first 500 channel EPG entries if no channel was specified
                    Else
                        Return settings.GetFullURL() + "/epg?start=0&limit=1&channel=" & System.Net.WebUtility.UrlEncode(channelName)
                    End If

                End If
            End Get
        End Property

        Public ReadOnly Property apiSearchEPGEvents(searchString As String)
            Get
                Return settings.GetFullURL() + String.Format("/epg?start=0&limit=300&title={0}", searchString)
            End Get
        End Property

        Public ReadOnly Property apiCreateEmptyAutoRecording(newAutoRecording As AutoRecordingViewModel) As String
            Get
                Return settings.GetFullURL() + "/tablemgr?op=create&table=autorec"
            End Get
        End Property

        Public ReadOnly Property apiCreateAutoRecordingBySeries(eventId As String, Optional dvrConfig As DVRConfigViewModel = Nothing) As String
            'eventId=3558075&op=recordSeries&config_name=
            Get
                Dim config_uuid As String = ""
                If Not dvrConfig Is Nothing Then config_uuid = dvrConfig.name
                Return settings.GetFullURL() + String.Format("/dvr?eventId={0}&op=recordSeries&config_uuid={1}", eventId, config_uuid)
            End Get
        End Property

        Public ReadOnly Property apiAddManualRecording(rec As RecordingViewModel) As String
            Get
                ' /dvr/addentry
                'op=createEntry&channelid=28&date=12%2F12%2F2014&starttime=12%3A00&stoptime=13%3A30&pri=Normal&title=test&config_name=(default)
                Return settings.GetFullURL() + String.Format("/dvr/addentry?op=createEntry&channelid={0}&date={1}&starttime={2}&stoptime={3}&pri=Normal&config_name=&title={4}",
                                                             Uri.EscapeDataString(rec.channelUuid),
                                                             Uri.EscapeDataString(rec.startDate.ToString("MM/dd/yyy")),
                                                             Uri.EscapeDataString(rec.startTime.ToString("HH:mm")),
                                                             Uri.EscapeDataString(rec.stopTime.ToString("HH:mm")),
                                                             Uri.EscapeDataString(rec.title))
            End Get
        End Property

    End Class

End Namespace

