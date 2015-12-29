Imports GalaSoft.MvvmLight
Imports TVHead_81.Common
Imports TVHead_81.ViewModels

Public Class RecordingViewModel
    Inherits ViewModelBase
    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
        End Get

    End Property

    Public Property ExpandCollapseCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("RecordingViewModel.ExpanseCollapse", "start")
                                        Dim rlist As New ObservableCollection(Of Group(Of RecordingViewModel))
                                        Select Case vm.PivotSelectedIndex
                                            Case 2
                                                rlist = vm.UpcomingRecordings.groupeditems
                                            Case 3
                                                rlist = vm.FinishedRecordings.groupeditems
                                            Case 4
                                                rlist = vm.FailedRecordings.groupeditems
                                        End Select
                                        For Each group In rlist
                                            For Each epgitem In group
                                                If epgitem Is Me Then
                                                    If Me.ExpandedView = "Collapsed" Then Me.ExpandedView = "Visible" Else Me.ExpandedView = "Collapsed"
                                                Else
                                                    epgitem.ExpandedView = "Collapsed"
                                                End If
                                            Next
                                        Next
                                        WriteToDebug("RecordingViewModel.ExpanseCollapse", "Stop")
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedChannel As RelayCommand
        Get
            Return New RelayCommand(Sub(x)
                                        WriteToDebug("RecordingViewModel.UpdateSelectedChannel", "start")
                                        If TypeOf x Is ChannelViewModel Then
                                            Dim selectedChannel As ChannelViewModel = CType(x, ChannelViewModel)
                                            Me.channelUuid = selectedChannel.channelUuid
                                            Me.channel = selectedChannel.name
                                            ChannelSelectionFlyOutIsOpen = False
                                            WriteToDebug("RecordingViewModel.UpdateSelectedChannel", "Stop")
                                        End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedDVRConfig As RelayCommand
        Get
            Return New RelayCommand(Sub(x)
                                        WriteToDebug("RecordingViewModel.UpdateSelectedDVRConfig", "start")
                                        If TypeOf x Is DVRConfigViewModel Then
                                            Dim selectedDVRConfig As DVRConfigViewModel = CType(x, DVRConfigViewModel)
                                            Me.configName = selectedDVRConfig.name
                                            Me.configUuid = selectedDVRConfig.identifier
                                            DVRConfigSelectionFlyOutIsOpen = False
                                            WriteToDebug("RecordingViewModel.UpdateSelectedDVRConfig", "Stop")
                                        End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property SaveRecording As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("RecordingViewModel.SaveRecording()", "start")
                                        'Dim app As App = CType(Application.Current, App)

                                        Dim r As tvhCommandResponse
                                        If Me.recording_id = "" Then
                                            r = Await AddManualRecording(Me)
                                        Else
                                            r = Await UpdateManualRecording(Me)
                                        End If

                                        If r.success = 1 Then
                                            If Not vm.appSettings.LongPollingEnabled Then
                                                vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                                                .msg = vm.loader.GetString("RecordingStartSuccess")})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Saved")
                                        Else
                                            If Not vm.appSettings.LongPollingEnabled Then
                                                vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                                                .msg = vm.loader.GetString("RecordingStartErrorContent")})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Failed")
                                        End If
                                        If Not vm.appSettings.LongPollingEnabled Then
                                            Await vm.UpcomingRecordings.Reload(True)
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

    Public Property filename As String
    Public Property broadcast As String

    Public Property channel As String
        Get
            Return _channel
        End Get
        Set(value As String)
            _channel = value
            RaisePropertyChanged("channel")
        End Set
    End Property
    Private Property _channel As String
    Public Property channelUuid As String

    Public Property chicon As String
        Get
            'Dim app As App = CType(Application.Current, App)
            Dim b = (From a In vm.AllChannels.items Where a.channelUuid = Me.channelUuid Select a).FirstOrDefault
            If Not b Is Nothing Then
                Return b.chicon
            End If
        End Get
        Set(value As String)
            _chicon = "blaat"

            RaisePropertyChanged("chicon")
        End Set
    End Property
    Private Property _chicon As String
    Public Property configName As String
        Get
            Return _configName
        End Get
        Set(value As String)
            _configName = value

            RaisePropertyChanged("configName")
        End Set
    End Property
    Private Property _configName As String

    Public Property configUuid As String
    Public Property title As String
    Public Property description As String
    Public Property recording_id As String

    Private Property _startDate As DateTime
    Public Property startDate As DateTime
        Get
            Return _startDate
        End Get
        Set(value As DateTime)
            _startDate = value
            RaisePropertyChanged("startDate")
        End Set
    End Property
    Private Property _startTime As DateTime

    Public ReadOnly Property startDateString As String
        Get
            Return startDate.ToString("t")
        End Get
    End Property
    Public ReadOnly Property stopDateString As String
        Get
            Return stopDate.ToString("t")
        End Get
    End Property


    Public Property startTime As DateTime
        Get
            Return _startTime
        End Get
        Set(value As DateTime)
            _startTime = value
            RaisePropertyChanged("startTime")
        End Set
    End Property
    Private Property _stopDate As DateTime
    Public Property stopDate As DateTime
        Get
            Return _stopDate
        End Get
        Set(value As DateTime)
            _stopDate = value
            RaisePropertyChanged("stopDate")
        End Set
    End Property
    Private Property _stopTime As DateTime
    Public Property stopTime As DateTime
        Get
            Return _stopTime
        End Get
        Set(value As DateTime)
            _stopTime = value
            RaisePropertyChanged("stopTime")
        End Set
    End Property

    Public Property duration As Integer
    Public Property creator As String
    Public Property pri As String

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
    Public Property status As String
        Get
            Return _status
        End Get
        Set(value As String)
            _status = value
            RaisePropertyChanged("status")
        End Set
    End Property
    Private Property _status As String

    Public Property schedstate As String
        Get
            Return _schedstate
        End Get
        Set(value As String)
            _schedstate = value
            Select Case value
                Case "recording"
                    IsRecorded = 1
                Case "scheduled"
                    IsRecorded = 3
                Case "recordingError"
                    IsRecorded = 2
                Case "completedError"
                    IsRecorded = 2

                Case Else
                    IsRecorded = 0
            End Select
            RaisePropertyChanged("schedstate")
        End Set
    End Property
    Private Property _schedstate As String
    Public Property percentcompleted As Double
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
        Set(value As Double)
            _percentcompleted = value
            RaisePropertyChanged("percentcompleted")

        End Set
    End Property
    Private Property _percentcompleted As Double

    Public Property filesize As Long
        Get
            Return _filesize
        End Get
        Set(value As Long)
            _filesize = value
            RaisePropertyChanged("filesize")
            RaisePropertyChanged("filesizeGB")

        End Set
    End Property
    Private Property _filesize As Long
    Public ReadOnly Property filesizeGB As String
        Get
            If filesize > 0 Then
                Return Math.Round(filesize / 1024 / 1024 / 1024, 2).ToString + " GB"
            Else : Return ""
            End If
        End Get
    End Property
    ' Used for FInished and Failed Recordings only
    Public Property url As String                       'Used for FInished and Failed Recordings only
    Public Property ExpandedView As String
        Get
            Return _ExpandedView
        End Get
        Set(value As String)
            _ExpandedView = value

            RaisePropertyChanged("ExpandedView")
        End Set
    End Property
    Private Property _ExpandedView As String
    Public Property ItemStatus As String
        Get
            Return _ItemStatus
        End Get
        Set(value As String)
            _ItemStatus = value

            RaisePropertyChanged("ItemStatus")
        End Set
    End Property
    Private Property _ItemStatus As String

    Public Property ExpanseCollapseEnabled As Boolean
        Get
            Return _ExpanseCollapseEnabled
        End Get
        Set(value As Boolean)
            _ExpanseCollapseEnabled = value

            RaisePropertyChanged("ExpanseCollapseEnabled")
        End Set
    End Property
    Private Property _ExpanseCollapseEnabled As Boolean

    Public Property DVRConfigSelectionEnabled As Boolean
        Get
            Return _DVRConfigSelectionEnabled
        End Get
        Set(value As Boolean)
            _DVRConfigSelectionEnabled = value

            RaisePropertyChanged("DVRConfigSelectionEnabled")
        End Set
    End Property
    Private Property _DVRConfigSelectionEnabled As Boolean

    Public Property EndDateVisibility As Visibility ' TVH3.4/3.5/3.6 do not used a End Date for a recording

    'Public ReadOnly Property progressBarBackgroundBrush As SolidColorBrush
    '    Get
    '        If (CType(Application.Current.Resources("PhoneBackgroundColor"), Color).Equals(Colors.Black)) Then
    '            Return New SolidColorBrush(Color.FromArgb(255, 33, 33, 33))
    '        Else
    '            Return New SolidColorBrush(Color.FromArgb(255, 230, 230, 230))
    '        End If

    '    End Get
    'End Property

    Public Async Function RecordingAbort() As Task(Of RecordingReturnValue)
        'Initiates the deletion of a recording
        Dim retValue As RecordingReturnValue
        retValue = Await AbortRecording(Me.recording_id)
        If retValue.tvhResponse.success = 1 Then
            If Not vm.appSettings.LongPollingEnabled Then
                vm.UpcomingRecordings.RemoveRecording(Me.recording_id, True)
                vm.FinishedRecordings.RemoveRecording(Me.recording_id, True)
                vm.FailedRecordings.RemoveRecording(Me.recording_id, True)
                If Not vm.SelectedChannel Is Nothing AndAlso Me.channelUuid = vm.SelectedChannel.channelUuid Then
                    vm.SelectedChannel.RefreshEPG(True)
                End If
                Dim c As ChannelViewModel = (From chan In vm.Channels.items Where chan.currentEPGItem.dvrUuid = Me.recording_id).FirstOrDefault()
                If Not c Is Nothing Then
                    c.RefreshCurrentEPGItem(Nothing, True)
                End If
            End If
        Else

            vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                                                .msg = vm.loader.GetString("RecordingAbortErrorContent")})

            WriteToDebug("TVHead_ViewModel.RecordingAbort()", "Recording Failed")
        End If


        '#End If

        Return retValue
    End Function

    Public Sub New()
        If vm.TVHVersion = "3.4" Then EndDateVisibility = Visibility.Collapsed Else EndDateVisibility = Visibility.Visible
        ExpandedView = "Collapsed"
        ItemStatus = "New"
        DVRConfigSelectionEnabled = False
        startDate = Date.Now
        startTime = Date.Now
        stopDate = Date.Now
        stopTime = Date.Now.AddHours(2)
    End Sub

    Public Sub New(recording As tvh40.Recording)
        ' Create a new Viewmodel based on a 3.9 TVH JSON entry
        ExpanseCollapseEnabled = True
        EndDateVisibility = Visibility.Visible
        broadcast = recording.broadcast
        channel = recording.channelname
        channelUuid = recording.channel
        chicon = recording.channel_icon
        configName = recording.config_name
        creator = recording.creator
        description = recording.disp_description
        duration = recording.duration
        startDate = UnixToDateTime(recording.start).ToLocalTime
        stopDate = UnixToDateTime(recording.stop).ToLocalTime
        filesize = recording.filesize
        pri = recording.pri
        recording_id = recording.uuid
        schedstate = recording.sched_status
        filename = recording.filename
        status = recording.status
        title = recording.disp_title
        url = recording.url
        ExpandedView = "Collapsed"
        ItemStatus = "New"
        DVRConfigSelectionEnabled = False
        IsSelected = False
    End Sub
End Class
