Imports GalaSoft.MvvmLight
Imports TVHead_81.ViewModels
Imports Windows.UI.Popups
Imports Windows.UI


Public Class EPGItemViewModel
    Inherits ViewModelBase
    Private _EPGItem As TVHEPGEvent

#Region "Properties"

    Public ReadOnly Property Channel As ChannelViewModel
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Not vm Is Nothing AndAlso Not vm.Channels._allchannels.Count = 0 Then
                Return vm.Channels._allchannels.Where(Function(x) x.uuid = Me.channelUuid).FirstOrDefault()
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property channelName As String
        Get
            Return _EPGItem.channelName
        End Get
    End Property
    Public ReadOnly Property channelNumber As Integer
        Get
            Return _EPGItem.channelNumber
        End Get
    End Property
    Public ReadOnly Property channelUuid As String
        Get
            Return _EPGItem.channelUuid
        End Get
    End Property
    Private Property continueWithDeletion As Boolean
    Public ReadOnly Property description As String
        Get
            If Not _EPGItem.description Is Nothing Then Return _EPGItem.description.Replace(ChrW(10), "") Else Return ""
        End Get
    End Property
    Public Property dvrState As String
        Get
            Return _EPGItem.dvrState
        End Get
        Set(value As String)
            If (value <> _EPGItem.dvrState) Then
                _EPGItem.dvrState = value
                RaisePropertyChanged("dvrState")
                RaisePropertyChanged("RecordingStatus")
                RaisePropertyChanged("RecordingIcon")
                RaisePropertyChanged("RecordingIconColor")
                RaisePropertyChanged("RecordingStatusIndicatorVisibility")
            End If
        End Set
    End Property
    Public Property dvrUuid As String
        Get
            Return _EPGItem.dvrUuid
        End Get
        Set(value As String)
            If _EPGItem.dvrUuid <> value Then
                _EPGItem.dvrUuid = value
                RaisePropertyChanged()
            End If
        End Set
    End Property
    Public ReadOnly Property endDate As DateTime
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime Else Return UnixToLocalDateTime(_EPGItem.stop).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property endDateString As String
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime.ToString("t") Else Return UnixToLocalDateTime(_EPGItem.stop).ToLocalTime.ToString("t")
        End Get
    End Property
    Public ReadOnly Property episodeId As Long
        Get
            Return _EPGItem.episodeId
        End Get
    End Property
    Public ReadOnly Property episodeUri As String
        Get
            Return _EPGItem.episodeUri
        End Get
    End Property
    Public ReadOnly Property eventId As Long
        Get
            Return _EPGItem.eventId
        End Get
    End Property
    Public ReadOnly Property genre As List(Of Integer)
        Get
            Return _EPGItem.genre
        End Get
    End Property
    Public ReadOnly Property genreName As String
        Get
            Dim vm = CType(Application.Current, Application).DefaultViewModel
            If Not genre Is Nothing AndAlso Not genre.Count = 0 Then
                Return (From g In vm.ContentTypes.allitems Where g.uuid = genre(0) Select g.name).FirstOrDefault
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property info As String
        Get
            If _EPGItem.subtitle <> "" Then Return _EPGItem.subtitle Else Return _EPGItem.description
        End Get
    End Property
    Public ReadOnly Property RecordingIconColor As SolidColorBrush
        Get
            If RecordingStatus = 1 Or RecordingStatus = 3 Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return CType(Application.Current.Resources("SystemControlForegroundBaseHighBrush"), SolidColorBrush)
            End If
        End Get
    End Property
    Public ReadOnly Property EPGItemBackground As SolidColorBrush
        Get
            If IsSelected Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return New SolidColorBrush(Colors.Transparent)
            End If

        End Get
    End Property
    Public ReadOnly Property EPGItemDetailsVisibility As String
        Get
            Return If(IsExpanded, "Visible", "Collapsed")
        End Get
    End Property
    Public ReadOnly Property RecordingStatusIndicatorVisibility As String
        Get
            Select Case _EPGItem.dvrState
                Case "recording", "scheduled", "recordingError" : Return "Visible"
                Case Else : Return "Collapsed"
            End Select
        End Get
    End Property
    Public ReadOnly Property RecordingIcon As String
        Get
            Select Case _EPGItem.dvrState
                Case "recording" : Return "/Images/player_record_small.png"
                Case "scheduled" : Return "/Images/player_record_small_scheduled.png"
                Case "recordingError" : Return "/Images/player_record_small_error.png"
                Case Else : Return "/Images/player_record_small.png"
            End Select
        End Get
    End Property
    Public ReadOnly Property RecordingStatus As Integer
        Get
            Select Case _EPGItem.dvrState
                Case "recording" : Return 1
                Case "scheduled" : Return 3
                Case "recordingError" : Return 2
                Case Else : Return 0
            End Select
        End Get
    End Property
    Private Property _IsRecorded As Integer
    Public ReadOnly Property percentcompleted As Double
        Get
            If _EPGItem.eventId <> 0 Then
                If Date.Now > startDate Then
                    If Date.Now > endDate Then
                        Return 1
                    Else
                        Return Math.Round((Date.Now - startDate).TotalSeconds / (endDate - startDate).TotalSeconds, 2)
                    End If
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        End Get
    End Property
    Public Property RecordButtonEnabled As Boolean
        Get
            Return _RecordButtonEnabled
        End Get
        Set(value As Boolean)
            _RecordButtonEnabled = value
            RaisePropertyChanged()
        End Set

    End Property
    Private Property _RecordButtonEnabled As Boolean
    Public Property selectedDVRConfig As DVRConfigViewModel
    Public ReadOnly Property serieslinkId As Integer
        Get
            Return _EPGItem.serieslinkId
        End Get
    End Property
    Public ReadOnly Property serieslinkUri As String
        Get
            Return _EPGItem.serieslinkUri
        End Get
    End Property
    Public Property Status As String
        'Defines 3 possible states in which the ViewModel can be in, which triggers transitions between states
        ' New : Item is a new item (TODO : Insert transition effect with opactiy 0-->1, and visibilty 0-->1
        ' Existing : Item is already present. Sets the visibility to 1
        ' Remove : Item is (about to get) removed. 
        Get
            Return _Status
        End Get
        Set(value As String)
            If value <> _Status Then
                _Status = value
                RaisePropertyChanged("Status")
            End If
        End Set
    End Property
    Private Property _Status As String
    Public Property IsExpanded As Boolean
        Get
            Return _IsExpanded
        End Get
        Set(value As Boolean)
            If value <> _IsExpanded Then
                _IsExpanded = value
                RaisePropertyChanged("IsExpanded")
                RaisePropertyChanged("EPGItemDetailsVisibility")
            End If
        End Set
    End Property
    Private Property _IsExpanded As Boolean
    Public Property IsSelected As Boolean
        Get
            Return _IsSelected
        End Get
        Set(value As Boolean)
            If _IsSelected <> value Then
                _IsSelected = value
                RaisePropertyChanged("IsSelected")
                RaisePropertyChanged("EPGItemBackground")
            End If
        End Set
    End Property
    Private Property _IsSelected As Boolean
    Public ReadOnly Property start As Integer
        Get
            Return _EPGItem.start
        End Get
    End Property
    Public ReadOnly Property startDate As DateTime
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime Else Return UnixToLocalDateTime(_EPGItem.start).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property startDateString As String
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime.ToString("t") Else Return UnixToLocalDateTime(_EPGItem.start).ToLocalTime.ToString("t")
        End Get
    End Property
    Public ReadOnly Property [stop] As Integer
        Get
            Return _EPGItem.stop
        End Get
    End Property
    Public ReadOnly Property subtitle As String
        Get
            Return _EPGItem.subtitle
        End Get
    End Property
    Public ReadOnly Property title As String
        Get
            Return _EPGItem.title
        End Get
        'Private Set(value As String)
        '    _EPGItem.title = value
        '    RaisePropertyChanged("title")
        'End Set
    End Property
#End Region

#Region "Methods"
    ''' <summary>
    ''' Handles the start or stop of an EPGItem's recording. When the EPGItem is already being recorded, or scheduled for recording, then cancel the recording
    ''' When the EPGItem isn't yet being recorded, plan the recording.
    ''' Show a confirmation popup when deleting/creating a recording if the user has chosen this in the Settings menu
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Record() As Task
        WriteToDebug("EPGItemViewModel.Record()", "Executed")
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If RecordingStatus = 1 Or RecordingStatus = 2 Or RecordingStatus = 3 Then
            If vm.TVHeadSettings.ConfirmDeletion Then
                Await ShowConfirmDeletionPrompt()
                Exit Function
            Else
                Await StopEventRecording()
                Exit Function
            End If
        Else
            If vm.TVHeadSettings.ProposeAutoRecording Then
                Await ShowSingleAutoRecordingPrompt()
                Exit Function
            Else
                Await StartEventRecording()
                Exit Function
            End If
        End If
    End Function

    ''' <summary>
    ''' Stops the recording of this EPGItem, based on the dvr uuid attached.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function StopEventRecording() As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasDVRAccess And dvrUuid <> "" Then
            WriteToDebug("EPGItemViewModel.StopEventRecording()", "Executed")
            Dim RecordingCancelled As New tvhCommandResponse
            RecordingCancelled = (Await CancelRecording(dvrUuid)).tvhResponse

            Select Case RecordingCancelled.success
                Case 0
                    Await vm.Notify.Update(True, vm.loader.GetString("RecordingAbortErrorContent"), 2, 0, 4)
                Case 1
                    Await vm.Notify.Update(False, vm.loader.GetString("RecordingAbortSuccess"), 1, 0, 2)
            End Select
            'Retrieve the EPGItem again from the TVH server, which now should contain updated information around the DVR status
            Dim updatedEPGItem As EPGItemViewModel = (Await LoadEPGEventByID(Me.eventId)).FirstOrDefault
            If Not updatedEPGItem Is Nothing Then Me.Update(updatedEPGItem)
        End If
    End Function

    ''' <summary>
    ''' Provides a MessageDialog which is shown to ask the user to confirm the deletion of the recording.
    ''' </summary>
    ''' <returns></returns>
    Private Async Function ShowConfirmDeletionPrompt() As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim strMessage As String = vm.loader.GetString("RecordingAbortContent")
        Dim strheader As String = vm.loader.GetString("RecordingAbortHeader")
        Dim msgBox As New MessageDialog(strMessage, strheader)
        msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), New UICommandInvokedHandler(AddressOf StopEventRecording)))
        msgBox.Commands.Add(New UICommand(vm.loader.GetString("No"))) 'No command is attached to the No button, which will cause the MessageDialog to close without any action
        Await msgBox.ShowAsync()
    End Function


    Private Async Function ShowSingleAutoRecordingPrompt() As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim cDialogDataContext As New StartRecordingContentDialogViewModel(Me)

        Dim cDialog As New ContentDialog With {.IsPrimaryButtonEnabled = True,
                                               .IsSecondaryButtonEnabled = True,
                                               .DataContext = cDialogDataContext,
                                               .ContentTemplate = CType(Application.Current.Resources("StartRecordingContentDialog"), DataTemplate),
                                               .Title = vm.loader.GetString("RecordingConfiguration"),
                                               .PrimaryButtonText = vm.loader.GetString("OK"),
                                               .SecondaryButtonText = vm.loader.GetString("Cancel")}
        Dim r As ContentDialogResult = Await cDialog.ShowAsync()
        If r = ContentDialogResult.Primary Then Await StartEventRecording(cDialogDataContext)

    End Function


    Private Async Function StartEventRecording(Optional recordingoptions As StartRecordingContentDialogViewModel = Nothing) As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim response As New RecordingReturnValue
        If recordingoptions Is Nothing Then
            'Record program with standard settings, one time
            response = Await Task.Run(Function() RecordProgram(Me))

        Else
            If recordingoptions.SingleRecording Then response = Await Task.Run(Function() RecordProgram(Me, recordingoptions.selectedDVRConfig))
            If recordingoptions.SeriesRecording Then response = Await Task.Run(Function() RecordProgramBySeries(Me, recordingoptions.selectedDVRConfig))
            If recordingoptions.AutoRecording Then response = Await Task.Run(Function() RecordProgramBySeries(Me, recordingoptions.selectedDVRConfig))
        End If

        Select Case response.tvhResponse.success
            Case "0"
                'Error during starting Recording 
                Await vm.Notify.Update(True, vm.loader.GetString("RecordingStartErrorContent"), 2, 0, 4)
            Case "1"
                'Recording command cancelled, do nothing
                Await vm.Notify.Update(False, vm.loader.GetString("RecordingStartSuccess"), 1, 0, 2)
        End Select

        'Retrieve the EPGItem again from the TVH server, which now should contain updated information around the DVR status
        Dim updatedEPGItem As EPGItemViewModel = (Await LoadEPGEventByID(Me.eventId)).FirstOrDefault
            If Not updatedEPGItem Is Nothing Then Me.Update(updatedEPGItem)

    End Function

    ''' <summary>
    ''' 'Updates the EPGItemViewModel with new properties retrieved from a refreshed equivalent EPGItem
    ''' </summary>
    ''' <param name="epgitem"></param>
    ''' <remarks></remarks>
    Public Sub Update(Optional epgitem As EPGItemViewModel = Nothing)
        RunOnUIThread(Sub()
                          If Not epgitem Is Nothing Then
                              dvrState = epgitem.dvrState
                              dvrUuid = epgitem.dvrUuid
                              RaisePropertyChanged("percentcompleted")
                          Else
                              RaisePropertyChanged("percentcompleted")
                          End If
                      End Sub)
    End Sub
    ''' <summary>
    ''' 'Marks the EPGItemViewModel for removal
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        Status = "Remove"
    End Sub
#End Region

#Region "Constructors"
    Public Sub New()
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If Not vm Is Nothing Then
            _EPGItem = New TVHEPGEvent With {.title = vm.loader.GetString("NoInformationAvailable"), .description = "", .dvrUuid = "", .dvrState = "", .eventId = 0}
        End If
        Status = "Existing"
    End Sub

    ''' <summary>
    ''' 'Create a new EPGViewModel from a TVH 3.9 / 4.X JSON entry
    ''' </summary>
    ''' <param name="epg_item"></param>
    ''' <remarks></remarks>
    Public Sub New(epg_item As TVHEPGEvent)
        _EPGItem = epg_item
        'IsRecorded = 1
        RecordButtonEnabled = False
        Status = "New"
    End Sub
#End Region

End Class

