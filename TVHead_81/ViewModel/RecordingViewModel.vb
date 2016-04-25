Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json
Imports Windows.UI

Public Class RecordingViewModel
    Inherits ViewModelBase

    Protected Friend _recording As TVHRecording

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
    Public Property comment As String
        Get
            Return _recording.comment
        End Get
        Set(value As String)
            _recording.comment = value
        End Set
    End Property
    Public ReadOnly Property config_name As String
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
    Private _IsExpanded As Boolean
    Public Property IsExpanded As Boolean
        Get
            Return _IsExpanded
        End Get
        Set(ByVal value As Boolean)
            _IsExpanded = value
            RaisePropertyChanged("IsExpanded")
            RaisePropertyChanged("RecordingDetailsVisibility")
        End Set
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
            If value <> _IsSelected Then
                _IsSelected = value
                RaisePropertyChanged("IsSelected")
                RaisePropertyChanged("RecordingBackground")
            End If
        End Set
    End Property
    Public ReadOnly Property percentcompleted As Double
        Get
            If (_recording.start < TimeToUnix(Date.Now.ToUniversalTime)) Then
                If TimeToUnix(Date.Now.ToUniversalTime) > _recording.stop Then
                    Return 1
                Else
                    Return Math.Round((TimeToUnix(Date.Now.ToUniversalTime) - _recording.start) / (_recording.stop - _recording.start), 2)
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
    Public ReadOnly Property RecordingBackground As SolidColorBrush
        Get
            If IsSelected Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return New SolidColorBrush(Colors.Transparent)
            End If

        End Get
    End Property
    Public ReadOnly Property RecordingDetailsVisibility As String
        Get
            If IsExpanded Then Return "Visible" Else Return "Collapsed"
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
    Public ReadOnly Property sched_status As String
        Get
            Return _recording.sched_status
        End Get
    End Property
    Public ReadOnly Property start As Long
        Get
            Return _recording.start
        End Get
    End Property
    Public ReadOnly Property startDate As DateTime
        Get
            Return UnixToLocalDateTime(_recording.start).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property startTimeString As String
        Get
            Return UnixToLocalDateTime(_recording.start).ToString("t")
        End Get
    End Property
    Public ReadOnly Property status As String
        Get
            Return _recording.status
        End Get
    End Property
    Public ReadOnly Property [stop] As Long
        Get
            Return _recording.stop
        End Get
    End Property
    Public ReadOnly Property stopTimeString As String
        Get
            Return UnixToLocalDateTime(_recording.stop).ToString("t")
        End Get
    End Property
    Public Property title As String
        Get
            Return _recording.disp_title
        End Get
        Set(value As String)
            _recording.disp_title = value
        End Set
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

    ''' <summary>
    ''' Saves the Recording on the TVH Server.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Save() As Task(Of tvhCommandResponse)
        WriteToDebug("RecordingViewModel.Save()", "executed")
        Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
        Dim tvh40api As New api40
        Dim response As New tvhCommandResponse
        Try
            Dim json_result As String
            If Me.uuid = "" Then
                'If the recording we are trying to save doesn't have a uuid, we assume it's a blank (new) autorecording
                json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiAddManualRecording(Me))).Content.ReadAsStringAsync
            Else
                json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiUpdateManualRecording(Me))).Content.ReadAsStringAsync
            End If
            If Not (json_result Is Nothing) Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvhCommandResponse)(json_result)
                response.success = 1
            End If
        Catch ex As Exception
            WriteToDebug("RecordingViewModel.Save()", ex.InnerException.ToString)
            response.success = 0
        End Try
        Return response
    End Function

    ''' <summary>
    ''' Updates the RecordingViewModel with new Model data, and triggers the RaisePropertyChanged for the 
    ''' relevant properties to reflect the updated values on the screen
    ''' </summary>
    ''' <param name="updatedRecording"></param>
    Public Sub Update(updatedRecording As RecordingViewModel)
        _recording = updatedRecording._recording
        RunOnUIThread(Sub()
                          RaisePropertyChanged("sched_status")
                          RaisePropertyChanged("RecordingIcon")
                          RaisePropertyChanged("filesizeGB")
                          RaisePropertyChanged("percentcompleted")
                      End Sub)
    End Sub

    ''' <summary>
    ''' Sends a command to the TVH server to abort the recording.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Abort() As Task(Of tvhCommandResponse)
        'Initiates the aborting of a recording
        Dim tvh40api As New api40
        Dim retValue As New tvhCommandResponse
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiAbortRecording(uuid))).Content.ReadAsStringAsync
            retValue.success = 1
        Catch ex As Exception
            WriteToDebug("RecordingViewModel.Cancel()", ex.Message.ToString)
            retValue.success = 0
        End Try
        Return retValue
    End Function

    ''' <summary>
    ''' Sends a command to the TVH server to delete the recording.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Delete() As Task(Of tvhCommandResponse)
        'Initiates the deletion of a recording
        Dim tvh40api As New api40
        Dim retValue As New tvhCommandResponse
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiDeleteRecording(uuid))).Content.ReadAsStringAsync
            retValue.success = 1
        Catch ex As Exception
            WriteToDebug("RecordingViewModel.Delete()", ex.Message.ToString)
            retValue.success = 0
        End Try
        Return retValue
    End Function

    ''' <summary>
    ''' Sends a command to the TVH server to stop the recording.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Cancel() As Task(Of tvhCommandResponse)
        'Initiates the stopping of a recording
        Dim tvh40api As New api40
        Dim retValue As New tvhCommandResponse
        Try
            Dim json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiStopRecording(uuid))).Content.ReadAsStringAsync
            retValue.success = 1
        Catch ex As Exception
            WriteToDebug("RecordingViewModel.Cancel()", ex.Message.ToString)
            retValue.success = 0
        End Try
        Return retValue
    End Function

    Public Sub New(recordingModel As TVHRecording)
        ' Create a new Viewmodel based on a 3.9 TVH JSON entry
        _recording = recordingModel
    End Sub

    Public Sub New()

    End Sub
End Class
