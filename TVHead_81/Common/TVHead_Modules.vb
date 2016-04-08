Imports Newtonsoft.Json
'Imports System.Threading.Tasks
'Imports System.Threading 'Required for test thread.task
'Imports TVHead_81.tvh34
Imports TVHead_81.tvh40
Imports TVHead_81.ViewModels
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports Windows.Web.Http

Module TVHead_Modules

    'Private tvh34api As New api34
    Private tvh40api As New api40

    <Extension()>
    Public Function ToObservableCollection(Of T)(collection As IEnumerable(Of T)) As ObservableCollection(Of T)
        Dim observableCollection As New ObservableCollection(Of T)()
        For Each item As T In collection
            observableCollection.Add(item)
        Next

        Return observableCollection
    End Function


    Public Async Function TimeoutAfter(Of T)(task As Task(Of T), delay As Integer) As Task(Of T)
        Await task.WhenAny(task, task.Delay(delay))
        If Not task.IsCompleted Then Throw New TimeoutException("Timeout hit.")
        Return Await task
    End Function


    Public Async Function RunOnUIThread(p As DispatchedHandler) As Task
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, p)
    End Function

#Region "Connectivity Validation"



#End Region

    ''' <summary>
    ''' CONVERTOR THAT CONVERTS A UNIX EPOCH DATE INTEGER TO A DATETIMEVALUE
    ''' </summary>
    ''' <param name="intUnixTime"></param>
    ''' <returns>DATETIME</returns>
    ''' <remarks></remarks>
    Public Function UnixToDateTime(ByVal intUnixTime As Integer) As Date
        Dim unixDate As New Date(1970, 1, 1)
        Dim dtDateTime As Date
        dtDateTime = unixDate.AddSeconds(intUnixTime)
        Return dtDateTime.ToLocalTime
    End Function

    ''' <summary>
    ''' CONVERTOR THAT CONVERTS A UNIX EPOCH DATE INTEGER TO A LOCALIZED SHORTDATE
    ''' </summary>
    ''' <param name="strUnixTime"></param>
    ''' <returns>DATE</returns>
    ''' <remarks></remarks>
    Public Function UnixToDate(ByVal strUnixTime As String) As Date
        Dim unixDate As New Date(1970, 1, 1)
        Dim dtDateTime As Date
        dtDateTime = unixDate.AddSeconds(CType(strUnixTime, Integer))
        'If dtDateTime.IsDaylightSavingTime = True Then
        '	dtDateTime = dtDateTime.AddHours(1)
        'End If
        Return dtDateTime.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern)
    End Function
    ''' <summary>
    ''' CONVERTOR THAT CONVERTS A DATE TO A UNIX EPOCH INTEGER
    ''' </summary>
    ''' <param name="parDate"></param>
    ''' <returns>INTEGER</returns>
    ''' <remarks></remarks>
    Public Function TimeToUnix(ByVal parDate As Date) As Integer
        If parDate.IsDaylightSavingTime = True Then
            'parDate.AddHours(-1)
        End If
        Dim unixDate As New Date(1970, 1, 1)
        Dim intSecondsDifference = (parDate - unixDate).TotalSeconds
        Return intSecondsDifference
    End Function

    ''' <summary>
    ''' CONVERTOR THAT CONVERTS A DATE TO A UNIX EPOCH INTEGER
    ''' </summary>
    ''' <param name="parDate"></param>
    ''' <returns>INTEGER</returns>
    ''' <remarks></remarks>
    Public Function TimeToUnixMs(ByVal parDate As Date) As Long
        If parDate.IsDaylightSavingTime = True Then
            'parDate.AddHours(-1)
        End If
        Dim unixDate As New Date(1970, 1, 1)
        Dim intMilliSecondsDifference = (parDate - unixDate).TotalMilliseconds
        Return Math.Round(intMilliSecondsDifference)
    End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF UPCOMING RECORDINGS
    ''' </summary>
    ''' <returns>LIST OF TVHRECORDING</returns>
    ''' <remarks></remarks>
    Public Async Function LoadUpcomingRecordings() As Task(Of IEnumerable(Of RecordingViewModel))
        WriteToDebug("Modules.LoadUpcomingRecordings()", "start")
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim result As New List(Of RecordingViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetUpcomingRecordings())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadUpcomingRecordings()", "error-stop")
            Throw New ArgumentException("Exception Occured")
            'Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.RecordingList)(json_result)
            For Each f In deserialized.entries
                result.Add(New RecordingViewModel(f))
            Next
        End If
        vm.UpcomingRecordings.dataLoaded = True
        Return (result.OrderBy(Function(x) x.startDate))
        WriteToDebug("Modules.LoadUpcomingRecordings()", "stop")

    End Function
    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF FINISHED RECORDINGS
    ''' </summary>
    ''' <returns>LIST OF RecordingViewModel</returns>
    ''' <remarks></remarks>
    Public Async Function LoadFinishedRecordings() As Task(Of IEnumerable(Of RecordingViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        WriteToDebug("Modules.LoadFinishedRecordings()", "start")
        Dim result As New List(Of RecordingViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetFinishedRecordings())).Content.ReadAsStringAsync
        Catch ex As Exception
            Throw New ArgumentException("Exception Occured")
            'Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.RecordingList)(json_result)
            For Each f In deserialized.entries
                result.Add(New RecordingViewModel(f))
            Next
        End If
        vm.FinishedRecordings.dataLoaded = True
        WriteToDebug("Modules.LoadFinishedRecordings()", "stop")
        Return result
    End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF FINISHED RECORDINGS
    ''' </summary>
    ''' <returns>LIST OF TVHRECORDING</returns>
    ''' <remarks></remarks>
    Public Async Function LoadFailedRecordings() As Task(Of IEnumerable(Of RecordingViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        WriteToDebug("Modules.LoadFailedRecordings()", "start")
        Dim result As New List(Of RecordingViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetFailedRecordings())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadFailedRecordings()", "stop-error")
            Throw New ArgumentException("Exception Occured")
            'Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.RecordingList)(json_result)
            For Each f In deserialized.entries
                result.Add(New RecordingViewModel(f))
            Next
        End If
        vm.FailedRecordings.dataLoaded = True
        WriteToDebug("Modules.LoadFailedRecordings()", "stop")
        Return result.OrderByDescending(Function(x) x.startDate)
    End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF AUTO RECORDINGS
    ''' </summary>
    ''' <returns>LIST OF TVHRECORDING</returns>
    ''' <remarks></remarks>
    Public Async Function LoadAutoRecordings() As Task(Of IEnumerable(Of AutoRecordingViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        WriteToDebug("Modules.LoadAutoRecordings()", "start")
        Dim result As New List(Of AutoRecordingViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetAutoRecordings())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadAutoRecordings()", ex.InnerException.ToString())
            Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.AutoRecordingList)(json_result)
            For Each retrievedAutoRecording In deserialized.entries
                result.Add(New AutoRecordingViewModel(retrievedAutoRecording))
            Next
        End If
        vm.AutoRecordings.dataLoaded = True
        WriteToDebug("Modules.LoadAutoRecordings()", "stop")
        Return result.OrderBy(Function(x) x.title)
    End Function


    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF ACTIVE STREAMS
    ''' </summary>
    ''' <returns>LIST OF STREAMS</returns>
    ''' <remarks></remarks>
    Public Async Function LoadStreams() As Task(Of List(Of StreamViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim response As New List(Of StreamViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetStreamStatus())).Content.ReadAsStringAsync
        Catch ex As Exception
            Return response
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.AdapterList)(json_result)
            For Each a In deserialized.entries
                response.Add(New StreamViewModel(a))
            Next
        End If
        Return response
    End Function


    Public Async Function GetBoxID() As Task(Of String)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim response As String = ""
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetBoxID())).Content.ReadAsStringAsync
        Catch ex As Exception
            Return response
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.CometPollResponse)(json_result)
            response = deserialized.boxid
        End If
        Return response
    End Function


    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF MUXES
    ''' </summary>
    ''' <returns>LIST OF TVHSUBSCRIPTION</returns>
    ''' <remarks></remarks>
    Public Async Function LoadMuxes() As Task(Of List(Of MuxViewModel))
        Dim response As New List(Of MuxViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetMuxes())).Content.ReadAsStringAsync
        Catch ex As Exception
            Return response
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.MuxList)(json_result)
            For Each a In deserialized.entries
                response.Add(New MuxViewModel(a))
            Next
        End If
        Return response.OrderBy(Function(x) x.name).ToList()

    End Function


    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF SERVICES
    ''' </summary>
    ''' <returns>LIST OF TVHSUBSCRIPTION</returns>
    ''' <remarks></remarks>
    Public Async Function LoadServices() As Task(Of List(Of ServiceViewModel))
        Dim settings As New TVHead_Settings
        Dim response As New List(Of ServiceViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetServices())).Content.ReadAsStringAsync
        Catch ex As Exception
            Return response
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.ServiceList)(json_result)
            For Each a In deserialized.entries
                response.Add(New ServiceViewModel(a))
            Next
        End If
        If settings.HideNamelessServices Then response = response.Where(Function(q) q.svcname <> "").ToList()
        If settings.HideDisabledServices Then response = response.Where(Function(y) y.enabled).ToList()
        Return response.OrderBy(Function(x) x.svcname).ToList()
    End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF CONNECTIONS
    ''' </summary>
    ''' <returns>LIST OF TVHCONNECTIONS</returns>
    ''' <remarks></remarks>
    Public Async Function LoadConnections() As Task(Of List(Of ConnectionViewModel))
        Dim settings As New TVHead_Settings
        Dim response As New List(Of ConnectionViewModel)
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetConnections())).Content.ReadAsStringAsync
            If Not json_result = "" Then

                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.ConnectionList)(json_result)
                For Each a In deserialized.entries
                    response.Add(New ConnectionViewModel(a))
                Next
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.LoadConnections()", "Exception")
            Return response
        End Try
        Return response.OrderBy(Function(x) x.id).ToList()
    End Function


    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF SERVICES
    ''' </summary>
    ''' <returns>LIST OF TVHSUBSCRIPTION</returns>
    ''' <remarks></remarks>
    Public Async Function LoadServiceDetails(service As ServiceViewModel) As Task(Of List(Of ServiceDetailViewModel))
        Dim response As New List(Of ServiceDetailViewModel)
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetServiceDetails(service))).Content.ReadAsStringAsync
            If Not json_result = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.ServiceDetailList)(json_result)
                For Each a In deserialized.fstreams
                    response.Add(New ServiceDetailViewModel(a))
                Next
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.LoadServiceDetails()", "Exception")
        End Try
        Return response.OrderBy(Function(x) x.index).ToList()
    End Function

    ''' <summary>
    ''' FUNCTION THAT TALKS TO THE TVHEADEND SERVER AND RETRIEVES A LIST OF SUBSCRIPTIONS
    ''' </summary>
    ''' <returns>LIST OF TVHSUBSCRIPTION</returns>
    ''' <remarks></remarks>
    Public Async Function LoadSubscriptions() As Task(Of List(Of SubscriptionViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        Dim response As New List(Of SubscriptionViewModel)
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetSubscriptions())).Content.ReadAsStringAsync
            If Not json_result = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.SubscriptionList)(json_result)
                For Each a In deserialized.entries
                    response.Add(New SubscriptionViewModel(a))
                Next
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.LoadSubscriptions()", "Exception")
        End Try
        Return response
    End Function

    Public Async Function RecordProgramBySeries(epgEntry As EPGItemViewModel, Optional dvrConfig As DVRConfigViewModel = Nothing) As Task(Of RecordingReturnValue)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        'Dim tvhResponse As New tvhCommandResponse
        Dim result As New RecordingReturnValue
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiCreateAutoRecordingBySeries(epgEntry.eventId, dvrConfig))).Content.ReadAsStringAsync
            Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
            Dim rec_id As RecordingViewModel
            'RETREIVE ALL RECORDINGS AND PICK THE ONE THAT MATCHES THIS EVENT ID, IN ORDER TO ATTACH THE RECORING_ID BACK TO THE RETURN VALUE
            Dim recordings As IEnumerable(Of RecordingViewModel) = Await LoadUpcomingRecordings()
            rec_id = (From c In recordings Where c.channelUuid = epgEntry.channelUuid And c.startDate = epgEntry.startDate Select c).FirstOrDefault
            If Not rec_id Is Nothing Then
                result = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 1}, .recording = rec_id}
            Else
                result = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 0}, .recording = rec_id}
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.RecordProgramBySeries()", ex.InnerException.ToString)
            Return New RecordingReturnValue With {.recording_id = "", .tvhResponse = New tvhCommandResponse With {.success = 0}}
        End Try
        Return result
    End Function



    Public Async Function RecordProgram(epgEntry As EPGItemViewModel, Optional dvrConfig As DVRConfigViewModel = Nothing) As Task(Of RecordingReturnValue)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        'Dim tvhResponse As New tvhCommandResponse
        Dim result As New RecordingReturnValue
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiRecordProgramByEvent(epgEntry.eventId, dvrConfig))).Content.ReadAsStringAsync
            Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
            Dim rec_id As String
            'RETREIVE ALL RECORDINGS AND PICK THE ONE THAT MATCHES THIS EVENT ID, IN ORDER TO ATTACH THE RECORING_ID BACK TO THE RETURN VALUE
            'IF A RECORDING WAS STARTED BUT ENCOUNTERED AN ERROR, THE RECEIVED RESPONSE MIGHT BE TOO FAST AND THE RECORDING WILL SHOW UP AS RECORDING INSTEAD OF ERROR
            ' THIS WILL BE CORRECTED BY A REFRESH OF THE VIEW
            Dim recordings As IEnumerable(Of RecordingViewModel) = Await LoadUpcomingRecordings()
            Dim rec As New RecordingViewModel
            Dim matchingRecording = (From r In recordings Where r.broadcast = epgEntry.eventId Select r).FirstOrDefault
            If Not matchingRecording Is Nothing Then
                rec = matchingRecording
            End If

            If Not rec Is Nothing Then
                rec_id = rec.recording_id
                WriteToDebug("TVHead_Modules.RecordProgram()", deserialized.success.ToString)
                result = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 1}, .recording = rec}

            Else
                'We didn't retreive a matching recording from the server, assume there was an error
                WriteToDebug("TVHead_Modules.RecordProgram()", deserialized.success.ToString)
                result = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 0}, .recording = rec}
                rec_id = ""
            End If

        Catch ex As Exception
            WriteToDebug("TVHead_Modules.RecordProgram()", ex.InnerException.ToString)
            Return New RecordingReturnValue With {.recording_id = "", .tvhResponse = New tvhCommandResponse With {.success = 0}}
        End Try
        Return result
    End Function


    Public Async Function CancelRecording(uuid As String) As Task(Of RecordingReturnValue)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim result As New RecordingReturnValue
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiAbortRecording(uuid))).Content.ReadAsStringAsync
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 1}}
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.CancelRecording()", ex.InnerException.ToString)
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 0}}
        End Try
        Return result
    End Function


    Public Async Function AbortRecording(uuid As String) As Task(Of RecordingReturnValue)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim result As New RecordingReturnValue
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiAbortRecording(uuid))).Content.ReadAsStringAsync
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 1}}
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.AbortRecording()", ex.Message.ToString)
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 0}}
        End Try
        Return result
    End Function


    Public Async Function DeleteAutoRecording(uuid As String) As Task(Of RecordingReturnValue)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings

        Dim result As New RecordingReturnValue
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiDeleteAutoRecording(uuid))).Content.ReadAsStringAsync
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 1}}
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.DeleteAutoRecording()", ex.InnerException.ToString)
            result = New RecordingReturnValue With {.recording_id = uuid, .tvhResponse = New tvhCommandResponse With {.success = 0}}
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Create a new AutoRecording on the TVH server. In older TVH versions creating a Auto Recording is a 2-step process, where first an empty Auto Recording is created, after which it gets updated with the right properties
    ''' </summary>
    ''' <param name="newAutoRecording"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function AddAutoRecording(newAutoRecording As AutoRecordingViewModel) As Task(Of tvhCommandResponse)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        'Make the autorecording ready for saving, by making adjustments to how the values look. Depends on 3.4 / 3.9 differences
        newAutoRecording.ReadyForSaving()
        Dim response As New tvhCommandResponse
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiCreateAutoRecording(newAutoRecording))).Content.ReadAsStringAsync
            If Not (json_result Is Nothing) Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
                response.success = 1
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.AddAutoRecording()", ex.InnerException.ToString)
            response.success = 0
        End Try

        Return response
    End Function


    Public Async Function UpdateAutoRecording(updatedAutoRecording As AutoRecordingViewModel) As Task(Of tvhCommandResponse)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        updatedAutoRecording.ReadyForSaving()
        Dim response As New tvhCommandResponse
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiUpdateAutoRecording(updatedAutoRecording))).Content.ReadAsStringAsync
            If Not (json_result Is Nothing) Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
                response.success = 1
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.UpdateAutoRecording()", ex.InnerException.ToString)
            response.success = 0
        End Try
        Return response
    End Function

    Public Async Function AddManualRecording(manualRecording As RecordingViewModel) As Task(Of tvhCommandResponse)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim strManualRecording As String = "/dvr/addentry?"
        Dim settings As New TVHead_Settings
        Dim tvhResponse As New tvhCommandResponse
        Try
            Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiAddManualRecording(manualRecording))).Content.ReadAsStringAsync
            If Not (json_result Is Nothing) Then
                tvhResponse = New tvhCommandResponse With {.success = 1}
            End If
        Catch ex As Exception
            WriteToDebug("TVHead_Modules.AddManualRecording()", ex.InnerException.ToString)
            tvhResponse.success = 0
        End Try
        Return tvhResponse
    End Function


    Public Async Function UpdateManualRecording(manualRecording As RecordingViewModel) As Task(Of tvhCommandResponse)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        Dim strURL As String = ""
        Dim tvhResponse As New tvhCommandResponse
        If Not strURL = "" Then
            Try
                Dim json_result As String = Await (Await (New Downloader).DownloadJSON(tvh40api.apiUpdateManualRecording(manualRecording))).Content.ReadAsStringAsync
                If Not (json_result Is Nothing) Then
                    Dim blaat = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
                    tvhResponse = New tvhCommandResponse With {.success = 1}
                End If
            Catch ex As Exception
                WriteToDebug("TVHead_Modules.UpdateManualRecording()", ex.InnerException.ToString())
                tvhResponse.success = 0
            End Try
        End If

        Return tvhResponse
    End Function


    Public Async Function LoadAllChannels() As Task(Of IEnumerable(Of ChannelViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        Dim strUrl As String = ""
        Dim result As New List(Of ChannelViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetChannels())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadAllChannels()", ex.InnerException.ToString())
            Return Nothing
        End Try
        If Not json_result = "" Then
            Dim dsChannelList = JsonConvert.DeserializeObject(Of tvh40.ChannelList)(json_result)
            For Each c In dsChannelList.entries
                result.Add(New ChannelViewModel(c))
            Next

        End If

        Return result.OrderBy(Function(x) x.number)

    End Function

    ''' <summary>
    ''' Converts a string to a list of integers. String should be in the format of "1,2,3"
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function StringToIntList(str As String) As List(Of Integer)
        Dim l As New List(Of Integer)
        If Not str = "" Then
            Dim tmp() As String = str.Split(",")
            For Each c In tmp
                l.Add(CType(c, Integer))
            Next
        End If
        Return l
    End Function



    ''' <summary>
    ''' Converts a List of Ints to a String
    ''' </summary>
    ''' <param name="intList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IntListToString(intList As List(Of Integer)) As String
        Dim l As String = ""
        For Each intie In intList
            l += intie.ToString + ","
        Next
        'Remove last comma if any
        If Not l = "" Then
            l = l.Remove(l.LastIndexOf(","))
        End If
        Return l
    End Function

    Public Function IntListToStringArray(intList As List(Of Integer)) As String
        Dim l As String
        l = "["
        For Each intie In intList
            l += intie.ToString + ","
        Next
        'Remove last comma if any
        If Not l = "" And l.LastIndexOf(",") >= 0 Then
            l = l.Remove(l.LastIndexOf(","))
        End If
        l = l + "]"
        Return l
    End Function


    ''' <summary>
    ''' Converts a string to a list of strings. String should be in the format of "1,2,3"
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function StringToStringArray(str As String) As String()
        Dim tmp As String() = New String() {}

        If Not str = "" Then
            tmp = str.Split(",")
        End If
        Return tmp
    End Function

    ''' <summary>
    ''' Converts a string to a list of strings. String should be in the format of "1,2,3"
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function StringToStringList(str As String) As List(Of String)
        Dim l As New List(Of String)
        If Not str = "" Then
            Dim tmp() As String = str.Split(",")
            For Each c In tmp
                l.Add(CType(c, String))
            Next
        End If
        Return l
    End Function


    Public Async Function SearchEPGEntry(searchString As String, Optional UseFulltext As Boolean = False) As Task(Of IEnumerable(Of ChannelViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        Dim result As New List(Of ChannelViewModel)
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiSearchEPGEvents(searchString, UseFulltext))).Content.ReadAsStringAsync
            If Not json_result = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
                For Each entry In deserialized.entries
                    'If Not entry.genre Is Nothing Then
                    'entry.genreName = (From c In ContentTypes Where c.uuid = entry.genre.First Select c.name).FirstOrDefault
                    'End If
                    Dim c As ChannelViewModel = (From chan In vm.AllChannels.items Where chan.uuid = entry.channelUuid Select chan).FirstOrDefault()
                    If Not c Is Nothing Then
                        Dim foundChannel As New ChannelViewModel
                        'foundChannel.chicon = c.chicon
                        'foundChannel.uuid = c.uuid
                        'foundChannel.name = c.name
                        'foundChannel.number = c.number
                        'foundChannel.currentEPGItem = New EPGItemViewModel(entry)
                        'foundChannel.currentEPGItem.ExpandedView = "Expanded"
                        result.Add(foundChannel)
                    End If
                Next

            End If
        Catch ex As Exception
            WriteToDebug("Modules.SearchEPGEntry()", ex.InnerException.ToString())
        End Try
        WriteToDebug("Modules.SearchEPGEntry()", result.Count())
        Return result.OrderBy(Function(x) x.currentEPGItem.startDate)
    End Function



    Public Async Function LoadIDNode(uuid As String, type As Object) As Task(Of Object)
        WriteToDebug("Modules.LoadIDNode()", "Loading ID Node : " & uuid)
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        'WriteToDebug("Modules.LoadEPGEventByID()", "Loading EPG Entry for channel :" & selectedChannel.name)
        Dim settings As New TVHead_Settings
        Dim result As New List(Of RecordingViewModel)
        Dim json_result As String
        Try
            Dim response As HttpResponseMessage = Await (New Downloader).DownloadJSON(tvh40api.apiLoadIDNode(uuid))
            If response.IsSuccessStatusCode Then
                json_result = Await response.Content.ReadAsStringAsync
            Else
                'Error (access ?)
                Return Nothing
            End If
        Catch ex As Exception
            WriteToDebug("Modules.LoadEPGEntry()", ex.InnerException.ToString)
            Return Nothing
        End Try
        If Not json_result = "" Then
            If TypeOf (type) Is RecordingViewModel Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.RecordingList)(json_result)
                If Not deserialized.entries.Count = 0 Then
                    'Hack to test if the cast resulted in anything sensible
                    If Not deserialized.entries(0).start = 0 Then
                        vm.TVHeadSettings.CapableOfLoadingRecordingIDNode = True
                        Return New RecordingViewModel(deserialized.entries(0))
                    Else
                        vm.TVHeadSettings.CapableOfLoadingRecordingIDNode = False
                        Return Nothing
                    End If
                End If
            End If
            If TypeOf (type) Is AutoRecordingViewModel Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.AutoRecordingList)(json_result)
                If Not deserialized.entries.Count = 0 Then
                    'Hack to test if the cast resulted in anything sensible
                    If Not deserialized.entries(0).title = "" Then
                        vm.TVHeadSettings.CapableOfLoadingAutoRecordingIDNode = True
                        Return New AutoRecordingViewModel(deserialized.entries(0))
                    Else
                        vm.TVHeadSettings.CapableOfLoadingAutoRecordingIDNode = False
                        Return Nothing
                    End If
                End If
            End If

            If TypeOf (type) Is ChannelViewModel Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.ChannelList)(json_result)
                If Not deserialized.entries.Count = 0 Then
                    'Hack to test if the cast resulted in anything sensible
                    If Not deserialized.entries(0).name = "" Then
                        vm.TVHeadSettings.CapableOfLoadingChannelIDNode = True
                        Return New ChannelViewModel(deserialized.entries(0))
                    Else
                        vm.TVHeadSettings.CapableOfLoadingChannelIDNode = False
                        Return Nothing
                    End If
                End If
            End If
        End If
        WriteToDebug("Modules.LoadIDNode()", "Completed Loading ID Node : " & uuid)
        'Return result.OrderBy(Function(x) x.recording_id)
    End Function



    Public Async Function LoadEPGEventByID(eventid As Integer) As Task(Of IEnumerable(Of EPGItemViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        'WriteToDebug("Modules.LoadEPGEventByID()", "Loading EPG Entry for channel :" & selectedChannel.name)
        Dim settings As New TVHead_Settings
        Dim EventIDList As New List(Of Integer)
        EventIDList.Add(eventid)


        'If vm.TVHVersion = "3.4" Then strURL = tvh34api.apiGetEPGEvents(selectedChannel.name, loadAll)
        Dim result As New List(Of EPGItemViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetEPGEvent(EventIDList))).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadEPGEntry()", ex.InnerException.ToString)
            Return result
        End Try
        If Not json_result = "" Then

            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
            For Each entry In deserialized.entries
                result.Add(New EPGItemViewModel(entry))
            Next

        End If

        'WriteToDebug("Modules.LoadEPGEntry()", "Completed Loading EPG Entries. : " & result.Count.ToString & "item(s)")
        Return result.OrderBy(Function(x) x.channelNumber)
    End Function

    Public Async Function LoadEPGEventIDs(selectedChannel As ChannelViewModel, Optional loadAll As Boolean = True, Optional maxItems As Integer = 300) As Task(Of List(Of Integer))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        WriteToDebug("Modules.LoadEPGEventIDs()", "Loading EPG EPGEventIDs for channel :" & selectedChannel.name)
        Dim settings As New TVHead_Settings
        Dim result As New List(Of Integer)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetEPGEvents(selectedChannel.uuid, loadAll, maxItems))).Content.ReadAsStringAsync
        Catch ex As Exception
            'WriteToDebug("Modules.LoadEPGEntry()", ex.InnerException.ToString)
            Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
            For Each entry In deserialized.entries
                result.Add(entry.eventId)
            Next
        End If
        WriteToDebug("Modules.LoadEPGEntry()", "Completed Loading EPG Entries. : " & result.Count.ToString & "item(s)")
        Return result
    End Function



    Public Async Function LoadEPGEntry(selectedChannel As ChannelViewModel, Optional loadAll As Boolean = True, Optional maxItems As Integer = 300) As Task(Of IEnumerable(Of EPGItemViewModel))
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        WriteToDebug("Modules.LoadEPGEntry()", "Loading EPG Entry for channel :" & selectedChannel.name)
        Dim result As New List(Of EPGItemViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetEPGEvents(selectedChannel.uuid, loadAll, maxItems))).Content.ReadAsStringAsync
        Catch ex As Exception
            'WriteToDebug("Modules.LoadEPGEntry()", ex.InnerException.ToString)
            Return result
        End Try
        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
            For Each entry In deserialized.entries
                result.Add(New EPGItemViewModel(entry))
            Next
        End If
        WriteToDebug("Modules.LoadEPGEntry()", "Completed Loading EPG Entries. : " & result.Count.ToString & "item(s)")
        Return result.OrderBy(Function(x) x.start)
    End Function

    Public Async Function LoadDVRConfigs() As Task(Of List(Of DVRConfigViewModel))
        WriteToDebug("Modules.LoadDVRConfigs()", "start")
        Dim result As New List(Of DVRConfigViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetDVRConfigs())).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("Modules.LoadDVRConfig()", "stop-error")
            Return result
        End Try

        If Not json_result = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.DVRConfigList)(json_result)
            If Not deserialized.entries.Count = 0 Then
                For Each entry In deserialized.entries
                    'vm.DVRConfigs.items.Add(New DVRConfigViewModel(entry))
                    result.Add(New DVRConfigViewModel(entry))
                Next
            End If

        End If
        WriteToDebug("Modules.LoadDVRConfigs()", "stop")
        Return (result.OrderBy(Function(x) x.name)).ToList()
    End Function

    'Public Async Function LoadContentTypes(Optional all As Boolean = False) As Task
    '    WriteToDebug("Modules.LoadContentTypes()", "start")
    '    Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
    '    Dim settings As New TVHead_Settings
    '    Dim response As New List(Of ContentTypeViewModel)
    '    Dim json_result As String
    '    Try
    '        json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetContentTypes(all))).Content.ReadAsStringAsync
    '    Catch ex As Exception
    '        WriteToDebug("Modules.LoadContentTypes()", "stop-error")
    '        Return
    '    End Try
    '    If Not json_result = "" Then

    '        Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.GenreList)(json_result)
    '        For Each f In deserialized.entries
    '            'Small hack to ensure we avoid having ContentType 0 in the list
    '            If f.key <> 0 Then
    '                response.Add(New ContentTypeViewModel(f))
    '            End If

    '        Next
    '    End If
    '    For Each i In response.OrderBy(Function(x) x.uuid)
    '        If all = True Then
    '            vm.ContentTypes.items.Add(i)
    '        Else
    '            vm.Genres.items.Add(i)
    '        End If
    '    Next
    '    If all = True Then vm.ContentTypes.dataLoaded = True
    '    If all = False Then vm.Genres.dataLoaded = True
    '    WriteToDebug("Modules.LoadContentTypes()", "stop")
    'End Function


    Public Async Function LoadChannelTags() As Task
        WriteToDebug("Modules.LoadChannelTags()", "start")
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim settings As New TVHead_Settings
        Dim chTags As New List(Of ChannelTagViewModel)
        Dim json_result As String
        Try
            json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetChannelTags())).Content.ReadAsStringAsync
        Catch ex As Exception
            'Debug.WriteLine("LoadChannelTags() Exception : " + ex.InnerException.ToString)
            WriteToDebug("Modules.LoadChannelTags()", "stop-error")
            Return
        End Try
        If Not json_result = "" Then
            Dim dsChannelTagList = JsonConvert.DeserializeObject(Of tvh40.ChannelTagList)(json_result)
            For Each retrievedChannelTag In dsChannelTagList.entries.OrderBy(Function(x) x.name)
                chTags.Add(New ChannelTagViewModel(retrievedChannelTag))
            Next
        End If

        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         vm.ChannelTags.items.Clear()
                                                                                                         For Each c In chTags.OrderBy(Function(x) x.name).ToList()
                                                                                                             vm.ChannelTags.items.Add(c)
                                                                                                         Next
                                                                                                     End Sub)

        If vm.ChannelTags IsNot Nothing Then
            'Set the Selected Channel Tag to the one stored in localsettings, if it exists
            Dim a = (From tags In vm.ChannelTags.items Select tags Where tags.uuid = settings.FavouriteChannelTag).FirstOrDefault
            If Not a Is Nothing Then
                vm.selectedChannelTag = a
            Else
                If Not vm.ChannelTags.items.Count() = 0 Then
                    vm.selectedChannelTag = vm.ChannelTags.items(0)
                End If

            End If
        End If
        vm.ChannelTags.dataLoaded = True
        WriteToDebug("Modules.LoadChannelTags()", "stop")
    End Function


    Public Sub WriteToDebug(caller As String, content As String)

        Debug.WriteLine(String.Format("Thread : {0,3} : {1,15} : {2,60} : {3}", Environment.CurrentManagedThreadId, Date.Now.TimeOfDay.ToString, caller, content))
    End Sub

End Module


