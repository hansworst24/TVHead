Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command

Public Class RecordingViewModel
    Inherits ViewModelBase

    Private _recording As tvh40.Recording

    Public Property ExpandCollapseCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        '    WriteToDebug("RecordingViewModel.ExpanseCollapse", "start")
                                        '    Dim rlist As New ObservableCollection(Of Group(Of RecordingViewModel))
                                        '    Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
                                        '    Select Case vm.PivotSelectedIndex
                                        '        Case 2
                                        '            rlist = vm.UpcomingRecordings.groupeditems
                                        '        Case 3
                                        '            rlist = vm.FinishedRecordings.groupeditems
                                        '        Case 4
                                        '            rlist = vm.FailedRecordings.groupeditems
                                        '    End Select
                                        '    For Each group In rlist
                                        '        For Each epgitem In group
                                        '            If epgitem Is Me Then
                                        '                If Me.ExpandedView = "Collapsed" Then Me.ExpandedView = "Visible" Else Me.ExpandedView = "Collapsed"
                                        '            Else
                                        '                epgitem.ExpandedView = "Collapsed"
                                        '            End If
                                        '        Next
                                        '    Next
                                        '    WriteToDebug("RecordingViewModel.ExpanseCollapse", "Stop")
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedChannel As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'WriteToDebug("RecordingViewModel.UpdateSelectedChannel", "start")
                                        'If TypeOf x Is ChannelViewModel Then
                                        '    Dim selectedChannel As ChannelViewModel = CType(x, ChannelViewModel)
                                        '    Me.channelUuid = selectedChannel.channelUuid
                                        '    Me.channel = selectedChannel.name
                                        '    ChannelSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("RecordingViewModel.UpdateSelectedChannel", "Stop")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedDVRConfig As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'WriteToDebug("RecordingViewModel.UpdateSelectedDVRConfig", "start")
                                        'If TypeOf x Is DVRConfigViewModel Then
                                        '    Dim selectedDVRConfig As DVRConfigViewModel = CType(x, DVRConfigViewModel)
                                        '    Me.configName = selectedDVRConfig.name
                                        '    Me.configUuid = selectedDVRConfig.identifier
                                        '    DVRConfigSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("RecordingViewModel.UpdateSelectedDVRConfig", "Stop")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property SaveRecording As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("RecordingViewModel.SaveRecording()", "start")
                                        'Dim app As App = CType(Application.Current, Application)
                                        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel

                                        Dim r As tvhCommandResponse
                                        If Me.uuid = "" Then
                                            r = Await AddManualRecording(Me)
                                        Else
                                            r = Await UpdateManualRecording(Me)
                                        End If

                                        If r.success = 1 Then
                                            If Not vm.TVHeadSettings.LongPollingEnabled Then
                                                'vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                '                                .msg = vm.loader.GetString("RecordingStartSuccess")})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Saved")
                                        Else
                                            If Not vm.TVHeadSettings.LongPollingEnabled Then
                                                'vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                '                                .msg = vm.loader.GetString("RecordingStartErrorContent")})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Failed")
                                        End If
                                        If Not vm.TVHeadSettings.LongPollingEnabled Then
                                            ' Await vm.UpcomingRecordings.Reload(True)
                                        End If
                                        Dim content = Window.Current.Content
                                        Dim frame = CType(content, Frame)
                                        frame.GoBack()
                                        Window.Current.Activate()
                                        WriteToDebug("RecordingViewModel.SaveRecording", "Stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property CancelRecordingEditing As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("RecordingViewModel.CancelRecordingEditing", "start")
                                        Dim content = Window.Current.Content
                                        Dim frame = CType(content, Frame)
                                        frame.GoBack()
                                        WriteToDebug("RecordingViewModel.CancelRecordingEditing", "Stop")

                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Private Property _ChannelSelectionFlyOutIsOpen As Boolean
    Public Property ChannelSelectionFlyOutIsOpen As Boolean
        Get
            Return _ChannelSelectionFlyOutIsOpen
        End Get
        Set(value As Boolean)
            _ChannelSelectionFlyOutIsOpen = value
            RaisePropertyChanged("ChannelSelectionFlyOutIsOpen")
        End Set
    End Property

    Private Property _DVRConfigSelectionFlyOutIsOpen As Boolean
    Public Property DVRConfigSelectionFlyOutIsOpen As Boolean
        Get
            Return _DVRConfigSelectionFlyOutIsOpen
        End Get
        Set(value As Boolean)
            _DVRConfigSelectionFlyOutIsOpen = value
            RaisePropertyChanged("DVRConfigSelectionFlyOutIsOpen")
        End Set
    End Property

    Public ReadOnly Property channelName As String
        Get
            Return _recording.channelname
        End Get
    End Property
    Public ReadOnly Property channelUuid As String
        Get
            Return _recording.channel
        End Get
    End Property
    Public ReadOnly Property channel_icon As String
        Get
            Dim i = _recording.channel_icon
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Not i Is Nothing And Not i = "" Then
                If i.ToUpper.IndexOf("HTTP:/") >= 0 Or i.ToUpper.IndexOf("HTTPS:/") >= 0 Then
                    Return i
                ElseIf i.StartsWith("imagecache/") Then
                    Return vm.TVHeadSettings.GetFullURL() & "/" & i
                Else
                    Return "ms-appx:///Images/tvheadend.png"
                End If
            Else
                Return "ms-appx:///Images/tvheadend.png"
            End If
        End Get
    End Property
    Public ReadOnly Property configName As String
        Get
            Return _recording.config_name
        End Get
    End Property
    Public ReadOnly Property creator As String
        Get
            Return _recording.creator
        End Get
    End Property
    Public ReadOnly Property description As String
        Get
            Return _recording.disp_description
        End Get
    End Property
    Public ReadOnly Property duration As Integer
        Get
            Return _recording.duration
        End Get
    End Property
    Public ReadOnly Property eventid As String
        Get
            Return _recording.broadcast
        End Get
    End Property
    Public ReadOnly Property filename As String
        Get
            Return _recording.filename
        End Get
    End Property
    Public ReadOnly Property filesizeGB As String
        Get
            If _recording.filesize > 0 Then
                Return Math.Round(_recording.filesize / 1024 / 1024 / 1024, 2).ToString + " GB"
            Else : Return ""
            End If
        End Get
        'Set(value As String)
        '    _recording.filesize = value
        '    RaisePropertyChanged("filesizeGB")
        'End Set

    End Property
    Private Property _IsRecorded As Integer
    Public Property IsRecorded As Integer
        Get
            Return _IsRecorded
        End Get
        Set(value As Integer)
            _IsRecorded = value
            RaisePropertyChanged("IsRecorded")
        End Set
    End Property
    Private Property _IsSelected As Boolean
    Public Property IsSelected As Boolean
        Get
            Return _IsSelected
        End Get
        Set(value As Boolean)
            _IsSelected = value

            RaisePropertyChanged("IsSelected")
        End Set
    End Property
    Public ReadOnly Property percentcompleted As Double
        Get
            If (stopDate > Date.MinValue) And (startDate > Date.MinValue) And (Date.Now > startDate) Then
                If Date.Now > stopDate Then
                    Return 1
                Else
                    Return Math.Round((Date.Now - startDate).TotalSeconds / (stopDate - startDate).TotalSeconds, 2)
                End If
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property pri As String
        Get
            Return _recording.pri
        End Get
    End Property
    Public ReadOnly Property RecordingIcon As String
        Get
            Select Case sched_status
                Case "recording" : Return "/Images/player_record_small.png"
                Case "scheduled" : Return "/Images/player_record_small_scheduled.png"
                Case "completed" : Return "/Images/player_record_small_completed.png"
                    'everything else is assumed to be in error state
                Case Else : Return "/Images/player_record_small_error.png"
            End Select
        End Get
    End Property
    Public ReadOnly Property title As String
        Get
            Return _recording.disp_title
        End Get
    End Property
    Public ReadOnly Property sched_status As String
        Get
            Return _recording.sched_status
        End Get
    End Property
    Public ReadOnly Property startDate As DateTime
        Get
            Return UnixToDateTime(_recording.start).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property startDateString As String
        Get
            Return startDate.ToString("t")
        End Get
    End Property
    Public ReadOnly Property stopDate As DateTime
        Get
            Return UnixToDateTime(_recording.stop).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property stopDateString As String
        Get
            Return stopDate.ToString("t")
        End Get
    End Property
    Public ReadOnly Property uuid As String
        Get
            Return _recording.uuid
        End Get
    End Property
    Public ReadOnly Property url As String
        Get
            Return _recording.url
        End Get
    End Property

    Public Sub Update(updatedRecording As RecordingViewModel)
        _recording = updatedRecording._recording
        RunOnUIThread(Sub()
                          RaisePropertyChanged("sched_state")
                          RaisePropertyChanged("filesizeGB")
                          RaisePropertyChanged("percentcompleted")
                      End Sub)
    End Sub

    ''' <summary>
    ''' Sends a command to the TVH server to abort the recording.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Abort() As Task(Of RecordingReturnValue)
        'Initiates the deletion of a recording
        Dim retValue As RecordingReturnValue
        retValue = Await AbortRecording(Me.uuid)
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        If retValue.tvhResponse.success = 1 Then
            If Not vm.TVHeadSettings.LongPollingEnabled Then
                vm.UpcomingRecordings.RemoveRecording(Me.uuid, True)
                vm.FinishedRecordings.RemoveRecording(Me.uuid, True)
                vm.FailedRecordings.RemoveRecording(Me.uuid, True)
                If Not vm.SelectedChannel Is Nothing AndAlso Me.channelUuid = vm.SelectedChannel.uuid Then
                    vm.SelectedChannel.RefreshEPG(True)
                End If
                Dim c As ChannelViewModel = (From chan In vm.Channels.items Where chan.currentEPGItem.dvrUuid = Me.uuid).FirstOrDefault()
                If Not c Is Nothing Then
                    c.RefreshCurrentEPGItem(Nothing, True)
                End If
            End If
        Else
            WriteToDebug("TVHead_ViewModel.RecordingAbort()", "Recording Failed")
        End If
        Return retValue
    End Function

    Public Sub New(recordingModel As tvh40.Recording)
        ' Create a new Viewmodel based on a 3.9 TVH JSON entry
        _recording = recordingModel
    End Sub
End Class
