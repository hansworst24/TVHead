Imports System.Threading
Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json
Imports Windows.Web.Http

Public Class CometCatcher
    Inherits ViewModelBase
    Public CatchCometsBoxID As String

    Public ct As CancellationToken
    Public tokenSource As New CancellationTokenSource()
    Public CometCatcher As Task
    Public CometCatcherLastRun As DateTime
    Public Property CometStatistics As New CometStatsViewModel

    Public Property CometsReceived As Integer
        Get
            Return _CometsReceived
        End Get
        Set(value As Integer)
            _CometsReceived = value
            RaisePropertyChanged("CometRotation")
        End Set
    End Property
    Private Property _CometsReceived As Integer
    Public ReadOnly Property CometRotation As Integer
        Get
            If CometsReceived < 36 Then
                Return CometsReceived * 10
            Else
                CometsReceived = 0
                Return 0
            End If
        End Get
    End Property

    Public Async Sub StartRefresh()
        WriteToDebug("TVHead_ViewModel.StartRefresh()", "")
        If CometCatcher Is Nothing OrElse CometCatcher.IsCompleted Then
            tokenSource = New CancellationTokenSource
            ct = tokenSource.Token
            CometCatcher = Await Task.Factory.StartNew(Function() CatchComets(ct), ct)
        End If
    End Sub

    Public Sub StopRefresh()
        If ct.CanBeCanceled Then
            tokenSource.Cancel()
        End If

        WriteToDebug("TVHead_ViewModel.StopRefresh()", "")
    End Sub

    Public Async Function CatchComets(ct As CancellationToken) As Task
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        While Not ct.IsCancellationRequested
            Dim s As New Stopwatch
            s.Start()

            If Not vm.TVHeadSettings.LongPollingEnabled Then
                Await Task.Delay(5000)
            Else
                If CatchCometsBoxID = "" Then
                    WriteToDebug("TVHead_ViewModel.CatchComets()", "Catching new boxid...")
                    CatchCometsBoxID = Await GetBoxID()
                    WriteToDebug("TVHead_ViewModel.CatchComets()", "boxid = " & CatchCometsBoxID)
                End If
                If Not CatchCometsBoxID = "" Then
                    'If 1 = 2 Then
                    Using response As HttpResponseMessage = Await (New Downloader).DownloadComet(CatchCometsBoxID)
                            If Not response.IsSuccessStatusCode Then
                                WriteToDebug("TVHead_ViewModel.CatchComets()", "HTTP request error")
                                Await Task.Delay(2000)
                                CatchCometsBoxID = ""
                            Else
                                Dim body As String = Await response.Content.ReadAsStringAsync()
                                Dim comet = JsonConvert.DeserializeObject(Of CometMessages.CometMessage)(body)
                                For Each message In comet.messages
                                    RunOnUIThread(Sub()
                                                      CometsReceived += 1
                                                  End Sub)

                                    ' tvh40 : subscriptions,input_status,dvrentry,logmessage,servicemapper,service_raw,service,caclient,connections
                                    ' TVH41 : subscriptions,input_status,dvrentry,logmessage,servicemapper,service_raw,epg,mpegts_mux,mpegts_network,diskspaceUpdate
                                    ' TVH34 : dvrdb
                                    'WriteToDebug(message.ToString, "")
                                    Select Case message("notificationClass")

                                        Case "input_status"
                                            Dim input_message As CometMessages.input_status = JsonConvert.DeserializeObject(Of CometMessages.input_status)(message.ToString())
                                            If input_message.reload = 1 Then
                                                Await vm.Streams.Reload()
                                            Else
                                                Await vm.Streams.Update(input_message)
                                            End If

                                        Case "subscriptions"
                                            Dim subscription_message As CometMessages.subscription = JsonConvert.DeserializeObject(Of CometMessages.subscription)(message.ToString())
                                            If subscription_message.reload = 1 Then
                                                Await vm.Subscriptions.Reload()
                                            Else
                                                Await vm.Subscriptions.Update(subscription_message)
                                            End If
                                        Case "dvrdb" 'Only used in legacy 3.4-ish 
                                            Dim dvr_message As CometMessages.dvrdb = JsonConvert.DeserializeObject(Of CometMessages.dvrdb)(message.ToString())
                                            If dvr_message.reload = 1 Then
                                                'TVH server tells us to reload the recordings
                                            End If

                                        Case "dvrentry"
                                            Dim dvrEntry_message As CometMessages.dvrentry = JsonConvert.DeserializeObject(Of CometMessages.dvrentry)(message.ToString())
                                            If Not dvrEntry_message.create Is Nothing Then
                                                For Each m In dvrEntry_message.create
                                                'currentCometStats.AddComet("dvrcreate")
                                                '    Dim updatedRecording As RecordingViewModel
                                                '    'updatedRecording = TryCast(Await LoadIDNode(m, New RecordingViewModel()), RecordingViewModel)
                                                '    updatedRecording = (From rec In (Await LoadUpcomingRecordings()) Where rec.uuid = m Select rec).FirstOrDefault()
                                                '    If Not updatedRecording Is Nothing Then
                                                '        'we received an update for a upcoming / running recording. handle it
                                                '        'Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                '        '                                                                                                 Await Me.UpcomingRecordings.AddRecording(updatedRecording, True)
                                                '        '                                                                                             End Sub)

                                                '    End If
                                            Next
                                            End If
                                            If Not dvrEntry_message.change Is Nothing Then
                                                For Each m In dvrEntry_message.change
                                                    CometStatistics.AddComet("dvrchange")
                                                '    'We received the uuid for a recording that was changed. Request the updated recording from the server by quering
                                                '    'for the ID Node
                                                '    If TVHeadSettings.CapableOfLoadingRecordingIDNode Then
                                                Dim updatedRecording As RecordingViewModel
                                                '        ''TODO CHECK FOR ACCESS
                                                updatedRecording = TryCast(Await LoadIDNode(m, GetType(RecordingViewModel)), RecordingViewModel)

                                                If Not updatedRecording Is Nothing Then
                                                    If updatedRecording.sched_status = "scheduled" Or updatedRecording.sched_status = "recording" Then
                                                        Await vm.UpcomingRecordings.UpdateRecording(updatedRecording)
                                                    End If
                                                    If updatedRecording.sched_status = "completed" Then
                                                        Await vm.UpcomingRecordings.RemoveRecording(updatedRecording.uuid, False)
                                                        Await vm.FinishedRecordings.UpdateRecording(updatedRecording)
                                                    End If
                                                    If updatedRecording.sched_status = "completedError" Then
                                                        Await vm.UpcomingRecordings.RemoveRecording(updatedRecording.uuid, False)
                                                        Await vm.FailedRecordings.UpdateRecording(updatedRecording)
                                                    End If
                                                End If
                                                '            If updatedRecording.schedstate = "scheduled" Or updatedRecording.schedstate = "recording" Then
                                                '                Await Me.UpcomingRecordings.UpdateRecording(updatedRecording)
                                                '            End If
                                                '            If updatedRecording.schedstate = "completed" Then
                                                '                Await Me.UpcomingRecordings.RemoveRecording(updatedRecording.uuid, False)
                                                '                Await Me.FinishedRecordings.UpdateRecording(updatedRecording)
                                                '            End If
                                                '            If updatedRecording.schedstate = "completedError" Then
                                                '                Await Me.UpcomingRecordings.RemoveRecording(updatedRecording.uuid, False)
                                                '                Await Me.FailedRecordings.UpdateRecording(updatedRecording)
                                                '            End If
                                                '        Else
                                                '            'updatedRecording is Nothing, probably we don't have access somewhere
                                                '            If TVHeadSettings.hasDVRAccess Then Await Me.UpcomingRecordings.Reload(True)
                                                '            If TVHeadSettings.hasDVRAccess Then Await Me.FinishedRecordings.Reload(True)
                                                '            If TVHeadSettings.hasFailedDVRAccess Then Await Me.FailedRecordings.Reload(True)
                                                '        End If
                                                '    Else
                                                '        If TVHeadSettings.hasDVRAccess Then Await Me.UpcomingRecordings.Reload(True)
                                                '        If TVHeadSettings.hasDVRAccess Then Await Me.FinishedRecordings.Reload(True)
                                                '        If TVHeadSettings.hasFailedDVRAccess Then Await Me.FailedRecordings.Reload(True)
                                                '    End If
                                            Next
                                            End If
                                            If Not dvrEntry_message.delete Is Nothing Then

                                                For Each m In dvrEntry_message.delete
                                                    CometStatistics.AddComet("dvrdelete")
                                                '    'Without knowing in which list the recording is located, initiate deletion of the recording by targeting all lists
                                                If Await vm.TVHeadSettings.hasDVRAccess Then Await vm.UpcomingRecordings.RemoveRecording(m, True)
                                                If Await vm.TVHeadSettings.hasDVRAccess Then Await vm.FinishedRecordings.RemoveRecording(m, True)
                                                If Await vm.TVHeadSettings.hasFailedDVRAccess Then Await vm.FailedRecordings.RemoveRecording(m, True)

                                            Next
                                        End If

                                        Case "dvrautorec"
                                            Dim autorec_message As CometMessages.dvrautorec = JsonConvert.DeserializeObject(Of CometMessages.dvrautorec)(message.ToString())
                                            If Not autorec_message.change Is Nothing Then
                                                For Each m In autorec_message.change
                                                    CometStatistics.AddComet("dvrautorecchange")
                                                    '        If TVHeadSettings.CapableOfLoadingAutoRecordingIDNode And TVHeadSettings.hasDVRAccess Then
                                                    '            Dim updatedAutoRecording As AutoRecordingViewModel
                                                    '            updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    '            If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.UpdateAutoRecording(updatedAutoRecording, True)
                                                    '        End If
                                                Next

                                            End If
                                            If Not autorec_message.create Is Nothing Then
                                                For Each m In autorec_message.create
                                                    CometStatistics.AddComet("dvrautoreccreate")
                                                    '        If TVHeadSettings.CapableOfLoadingAutoRecordingIDNode And TVHeadSettings.hasDVRAccess Then
                                                    '            Dim updatedAutoRecording As AutoRecordingViewModel
                                                    '            updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    '            If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.AddAutoRecording(updatedAutoRecording, True)
                                                    '        End If
                                                Next
                                            End If

                                            If Not autorec_message.delete Is Nothing Then

                                                For Each m In autorec_message.delete
                                                    CometStatistics.AddComet("dvrautorecdelete")
                                                    '        Await Me.AutoRecordings.DeleteAutoRecording(m, True)
                                                Next
                                            End If

                                            If Not autorec_message.update Is Nothing Then
                                                For Each m In autorec_message.update
                                                    CometStatistics.AddComet("dvrautorecupdate")
                                                    '        If TVHeadSettings.CapableOfLoadingAutoRecordingIDNode And TVHeadSettings.hasDVRAccess Then
                                                    '            Dim updatedAutoRecording As AutoRecordingViewModel
                                                    '            updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    '            If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.UpdateAutoRecording(updatedAutoRecording, True)
                                                    '        End If
                                                Next
                                            End If


                                        Case "epg"

                                            Dim epg_message As CometMessages.epg = JsonConvert.DeserializeObject(Of CometMessages.epg)(message.ToString())
                                            'Provide the EPG message to SelectedChannel.GroupedEPGItems in order to process any changes
                                            If Not epg_message.delete Is Nothing Then
                                                For Each m In epg_message.delete
                                                    CometStatistics.AddComet("epgdelete")
                                                    'Remove the EPGItem from the SelectedChannel if an EPGItem with the EventID given is found in the SelectedChannel
                                                    If Not vm.SelectedChannel Is Nothing AndAlso Not vm.SelectedChannel.epgitems.GetEvent(m) Is Nothing Then
                                                        Dim oldEvent = vm.SelectedChannel.epgitems.GetEvent(m)
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Deleting programme {0} : {1}", oldEvent.title, m.ToString()))
                                                        vm.SelectedChannel.epgitems.RemoveEvent(m)
                                                    End If
                                                Next
                                            End If
                                            If Not epg_message.update Is Nothing Then
                                                'Running Programmes get updated regularly (percent completed ?)
                                                For Each m In epg_message.update
                                                    CometStatistics.AddComet("epgupdate")
                                                    Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(m)).FirstOrDefault()
                                                    If newEPGEvent Is Nothing Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                    End If

                                                    'Update the EPG Event if it is within the SelectedChannel's EPG List
                                                    If Not newEPGEvent Is Nothing AndAlso Not vm.SelectedChannel Is Nothing AndAlso newEPGEvent.channelUuid = vm.SelectedChannel.uuid Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                        vm.SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                    End If
                                                    'Update the EPG Event if it is the currentEPGItem for a Channel
                                                    Dim channel As ChannelViewModel = (From c In vm.Channels.items Where c.uuid = newEPGEvent.channelUuid Select c).FirstOrDefault()
                                                If Not channel Is Nothing AndAlso channel.epgitems.currentEPGItem.eventId = newEPGEvent.eventId Then
                                                    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                    Await channel.RefreshCurrentEPGItem(newEPGEvent)
                                                End If
                                            Next
                                            End If

                                            If Not epg_message.create Is Nothing Then
                                                For Each m In epg_message.create
                                                    CometStatistics.AddComet("epgcreate")
                                                    Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(m)).FirstOrDefault()
                                                    If newEPGEvent Is Nothing Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Creating programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                    End If
                                                    If Not newEPGEvent Is Nothing AndAlso Not vm.SelectedChannel Is Nothing AndAlso newEPGEvent.channelUuid = vm.SelectedChannel.uuid Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Creating programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                        vm.SelectedChannel.epgitems.AddEvent(newEPGEvent)
                                                    End If
                                                Next
                                            End If

                                            If Not epg_message.change Is Nothing Then
                                                For Each m In epg_message.change
                                                    CometStatistics.AddComet("epgchange")
                                                    Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(m)).FirstOrDefault()
                                                    If newEPGEvent Is Nothing Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Changing programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                    End If
                                                    If Not newEPGEvent Is Nothing AndAlso Not vm.SelectedChannel Is Nothing AndAlso newEPGEvent.channelUuid = vm.SelectedChannel.uuid Then
                                                        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Changing programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                        vm.SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                    End If
                                                Next
                                            End If
                                            If Not epg_message.dvr_update Is Nothing Then
                                                For Each m In epg_message.dvr_update
                                                    CometStatistics.AddComet("epgdvrupdate")
                                                    ''Get the latest info for the EPGEvent
                                                    'Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()

                                                    'If Not newEPGEvent Is Nothing AndAlso Not SelectedChannel Is Nothing AndAlso Not SelectedChannel.epgitems Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.uuid Then
                                                    '    Await SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                    'End If

                                                    'Dim channel As ChannelViewModel = (From c In Channels.items Where c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                    'If Not channel Is Nothing Then
                                                    '    Await channel.RefreshCurrentEPGItem(newEPGEvent)
                                                    'End If

                                                    'If Not SearchPage Is Nothing AndAlso Not SearchPage.GroupedSearchResults Is Nothing Then
                                                    '    For Each g In SearchPage.GroupedSearchResults
                                                    '        Dim searchChannel As ChannelViewModel = (From c In g Where c.uuid = newEPGEvent.channelUuid AndAlso c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                    '        If Not searchChannel Is Nothing Then
                                                    '            Await searchChannel.RefreshCurrentEPGItem(newEPGEvent)
                                                    '        End If
                                                    '    Next

                                                    'End If

                                                    'If Not newEPGEvent Is Nothing Then
                                                    '    'update the matching recording
                                                    '    Dim updatedRecording As RecordingViewModel
                                                    '    If CapableOfLoadingIDNode Then
                                                    '        updatedRecording = TryCast(Await LoadIDNode(newEPGEvent.dvrUuid, New RecordingViewModel()), RecordingViewModel)
                                                    '        If Not updatedRecording Is Nothing Then
                                                    '            If updatedRecording.schedstate = "completed" Then
                                                    '                Await Me.FinishedRecordings.AddRecording(updatedRecording, True)
                                                    '                Await Me.UpcomingRecordings.RemoveRecording(newEPGEvent.dvrUuid, False)
                                                    '            End If
                                                    '            If updatedRecording.schedstate = "completedError" Then
                                                    '                Await Me.FailedRecordings.AddRecording(updatedRecording, True)
                                                    '                Await Me.UpcomingRecordings.RemoveRecording(newEPGEvent.dvrUuid, False)
                                                    '            End If
                                                    '            If updatedRecording.schedstate = "scheduled" Or updatedRecording.schedstate = "recording" Then
                                                    '                Await Me.UpcomingRecordings.UpdateRecording(updatedRecording)
                                                    '            End If
                                                    '        Else
                                                    '            Await Me.UpcomingRecordings.RemoveRecording(newEPGEvent.dvrUuid, True)
                                                    '            Await Me.FinishedRecordings.RemoveRecording(newEPGEvent.dvrUuid, True)
                                                    '            Await Me.FailedRecordings.RemoveRecording(newEPGEvent.dvrUuid, True)
                                                    '            'Recording is gone...
                                                    '        End If
                                                    '    End If
                                                    'End If
                                                Next
                                            End If

                                            If Not epg_message.dvr_delete Is Nothing Then
                                                For Each m In epg_message.dvr_delete
                                                    CometStatistics.AddComet("epgdvrdelete")
                                                    '    Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()
                                                    '    'Update any entry for the EPGEvent in the Selected Channel
                                                    '    If Not newEPGEvent Is Nothing Then
                                                    '        'Update SelectedChannel's status
                                                    '        If Not SelectedChannel Is Nothing AndAlso Not SelectedChannel.epgitems Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.uuid Then
                                                    '            Await SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                    '        End If
                                                    '        'Update any channel with this EPG Event ID and tell it to refresh
                                                    '        Dim channel As ChannelViewModel = (From c In Channels.items Where c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                    '        If Not channel Is Nothing Then
                                                    '            Await channel.RefreshCurrentEPGItem(newEPGEvent)
                                                    '        End If

                                                    '        'Clean up any results in the Search Page
                                                    '        If Not SearchPage Is Nothing AndAlso Not SearchPage.GroupedSearchResults Is Nothing Then
                                                    '            For Each g In SearchPage.GroupedSearchResults
                                                    '                Dim searchChannel As ChannelViewModel = (From c In g Where c.uuid = newEPGEvent.channelUuid AndAlso c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                    '                If Not searchChannel Is Nothing Then
                                                    '                    Await searchChannel.RefreshCurrentEPGItem(newEPGEvent)
                                                    '                End If
                                                    '            Next
                                                    '        End If

                                                    '    Else
                                                    '        'EPG Item doesn't exist on the server anymore
                                                    '    End If
                                                Next
                                            End If

                                        Case "logmessage"
                                            Dim log_message As CometMessages.logUpdate = JsonConvert.DeserializeObject(Of CometMessages.logUpdate)(message.ToString())
                                        'logmessages.Add(log_message.logtxt)
                                        Case "service"

                                        Case "mpegts_mux"

                                        Case "mpegts_network"

                                        Case "accessUpdate"
                                            'We think such a message is ment for re-authentication or so, fixed  by asking new boxid from the TVH server
                                            CatchCometsBoxID = Await GetBoxID()
                                        'Dim diskspace_message As CometMessages.diskspaceUpdate = JsonConvert.DeserializeObject(Of CometMessages.diskspaceUpdate)(message.ToString())
                                        'Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                        '                                                                                                 DiskSpaceStats.Update(diskspace_message)
                                        '                                                                                             End Sub)

                                        Case "ServerIpPort"
                                            'Not sure what this is used for

                                        Case "diskspaceUpdate"
                                        'Dim diskspace_message As CometMessages.diskspaceUpdate = JsonConvert.DeserializeObject(Of CometMessages.diskspaceUpdate)(message.ToString())
                                        ''Dim d As DiskspaceUpdateViewModel = New DiskspaceUpdateViewModel(diskspace_message)
                                        'Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                        '                                                                                                 DiskSpaceStats.Update(diskspace_message)
                                        '                                                                                             End Sub)


                                        'NOT IMPLEMENTED YET
                                        'WriteToDebug("StatusPage.OnNavigatedTo()", message("notificationClass") & " : xxx  :")
                                        Case "servicemapper"
                                            'NOT IMPLEMENTED YET
                                            'WriteToDebug("StatusPage.OnNavigatedTo()", message("notificationClass") & " : xxx  :")
                                        Case "service_raw"
                                            'NOT IMPLEMENTED YET
                                            'WriteToDebug("StatusPage.OnNavigatedTo()", message("notificationClass") & " : xxx  :")
                                        Case Else
                                            'WriteToDebug("StatusPage.OnNavigatedTo()", message("notificationClass") & " : Something else  :")
                                            ' CatchCometsBoxID = ""
                                    End Select
                                Next
                                'End While
                            End If
                        End Using
                        'Catch ex As Exception
                        '    If Not ex.Message Is Nothing Then
                        '        WriteToDebug("TVHead_ViewModel.CatchComets()", "Some error occured, resting for a second : Explanation " + ex.Message.ToString())
                        '    Else
                        '        WriteToDebug("TVHead_ViewModel.CatchComets()", "Some error occured, resting for a second : No explanation given...")
                        '    End If
                        'End Try
                    Else
                        WriteToDebug("TVHead_ViewModel.CatchComets()", "Didn't get a boxid, sleeping for 5 seconds...")
                    'app.isConnected = False
                    Await Task.Delay(5000)
                End If
                'Perform some other updates while we're polling but only every 10 seconds or more
                If CometCatcherLastRun.AddSeconds(5) <= Date.Now Then
                    'Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingEPGEntries"), 0, False, 0)
                    ' First perform standard updates to currentEPGItem of each channel
                    If Await vm.TVHeadSettings.hasEPGAccess Then
                        For Each c In vm.Channels.items.Where(Function(x) x.epgitemsAvailable = True)
                            Await c.UpdateCurrentEPGItem(Nothing, True)
                        Next
                        If Not vm.SelectedChannel Is Nothing Then
                            ' Await vm.SelectedChannel.RefreshEPG(False)
                        End If
                    End If
                    CometCatcherLastRun = Date.Now()
                    'vm.Notify.Clear()
                End If

            End If
            s.Stop()
            ' WriteToDebug("CometCatcher.CatchComets()", String.Format("Refresh Cycle took {0}ms", s.ElapsedMilliseconds.ToString))
            'If s.ElapsedMilliseconds < 1000 Then
            '    Await Task.Delay(1000 - s.ElapsedMilliseconds)
            'End If
        End While
    End Function
End Class
