Imports GalaSoft.MvvmLight
Imports TVHead_81.ViewModels
Imports Windows.UI.Popups
Imports GalaSoft.MvvmLight.Command
Imports Windows.UI

Public Class EPGItemViewModel
    Inherits ViewModelBase
    Private _EPGItem As tvh40.EPGEvent

#Region "Properties"
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
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime Else Return UnixToDateTime(_EPGItem.stop).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property endDateString As String
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime.ToString("t") Else Return UnixToDateTime(_EPGItem.stop).ToLocalTime.ToString("t")
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
            _Status = value
            RaisePropertyChanged()
        End Set
    End Property
    Private Property _Status As String
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
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime Else Return UnixToDateTime(_EPGItem.start).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property startDateString As String
        Get
            If _EPGItem.eventId = 0 Then Return Date.Now.ToLocalTime.ToString("t") Else Return UnixToDateTime(_EPGItem.start).ToLocalTime.ToString("t")
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
    Public Property title As String
        Get
            Return _EPGItem.title
        End Get
        Set(value As String)
            _EPGItem.title = value
            RaisePropertyChanged("title")
        End Set
    End Property
#End Region

#Region "RelayCommands"
    Public Property ExpandCollapseCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("EPGItemViewModel.ExpanseCollapseCommand()", "start")
                                        Dim vm = CType(Application.Current, Application).DefaultViewModel
                                        Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
                                        If rectie.Width > 720 Then
                                            vm.selectedEPGItem = Me
                                        Else
                                            'For Each group In vm.SelectedChannel.groupeditems
                                            '    For Each epgitem In group
                                            '        If epgitem Is Me Then
                                            '            If (Me.ExpandedView = "Collapsed" Or Me.ExpandedView = "") Then
                                            '                Me.ExpandedView = "Expanded"
                                            '                Me.RecordButtonEnabled = True
                                            '            Else
                                            '                Me.ExpandedView = "Collapsed"
                                            '                Me.RecordButtonEnabled = False
                                            '            End If
                                            '        Else
                                            '            If epgitem.ExpandedView = "Expanded" Then
                                            '                epgitem.ExpandedView = "Collapsed"
                                            '                epgitem.RecordButtonEnabled = False
                                            '            End If
                                            '        End If
                                            '    Next
                                            'Next
                                        End If
                                        WriteToDebug("EPGItemViewModel.ExpanseCollapseCommand()", "stop")
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property
    Public Property RecordCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        'Me.createAutoRecording = False
                                        'Me.createSeriesRecording = False
                                        'Me.createSingleRecording = False
                                        Me.continueWithDeletion = False
                                        Dim vm = CType(Application.Current, Application).DefaultViewModel

                                        WriteToDebug("EPGItemViewModel.RecordCommand()", "Executed")
                                        If RecordingStatus = 1 Or RecordingStatus = 2 Or RecordingStatus = 3 Then
                                            If vm.appSettings.ConfirmDeletion Then
                                                Await ShowConfirmDeletionPrompt()
                                            Else
                                                continueWithDeletion = True
                                            End If
                                            'Abort the recording
                                            If continueWithDeletion Then StopEventRecording()
                                        Else
                                            StartEventRecording()
                                        End If

                                        If Not vm.appSettings.LongPollingEnabled And Await vm.TVHeadSettings.hasDVRAccess Then
                                            vm.UpcomingRecordings.Reload(True)
                                            vm.FinishedRecordings.Reload(True)
                                            vm.FailedRecordings.Reload(True)
                                            If Not vm.SelectedChannel Is Nothing AndAlso Me.channelUuid = vm.SelectedChannel.uuid Then
                                                vm.SelectedChannel.RefreshEPG(True)
                                            End If
                                            Dim c As ChannelViewModel = (From chan In vm.Channels.items Where chan.currentEPGItem.eventId = Me.eventId).FirstOrDefault()
                                            If Not c Is Nothing Then
                                                c.RefreshCurrentEPGItem(Nothing, True)
                                            End If
                                        End If



                                        'Collapse the view of the parent channel, in case the EPG Item is the current epgitem for a channel
                                        If Not vm.Channels Is Nothing Then
                                            Dim c = (From a In vm.Channels.items Where a.uuid = Me.channelUuid Select a).FirstOrDefault
                                            If Not c Is Nothing AndAlso c.IsExpanded Then
                                                c.IsExpanded = False
                                            End If

                                        End If

                                        'Collapse the view of the channel, in case the EPG Item is residing in a search query
                                        If Not vm.SearchPage Is Nothing AndAlso Not vm.SearchPage.GroupedSearchResults Is Nothing Then
                                            For Each g In vm.SearchPage.GroupedSearchResults
                                                Dim c = (From a In g Where a.uuid = Me.channelUuid AndAlso a.currentEPGItem.eventId = Me.eventId Select a).FirstOrDefault
                                                If Not c Is Nothing AndAlso c.IsExpanded Then
                                                    c.IsExpanded = False
                                                End If
                                            Next

                                        End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property
#End Region

#Region "Methods"
    Public Async Function StopEventRecording() As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasDVRAccess Then
            WriteToDebug("EPGItemViewModel.StopEventRecording()", "Executed")
            Dim RecordingCancelled As New tvhCommandResponse
            RecordingCancelled = (Await CancelRecording(dvrUuid)).tvhResponse

            Select Case RecordingCancelled.success
                Case 0
                    Dim strMessage As String = vm.loader.GetString("RecordingAbortErrorHeader")
                    Dim strheader As String = vm.loader.GetString("RecordingAbortErrorContent")
                    Dim msgBox As New MessageDialog(strMessage, strheader)
                    msgBox.Commands.Add(New UICommand(vm.loader.GetString("OK")))
                    Await msgBox.ShowAsync()
                Case 1

            End Select
            'Retrieve the EPGItem again from the TVH server, which now should contain updated information around the DVR status
            Dim updatedEPGItem As EPGItemViewModel = (Await LoadEPGEventByID(Me.eventId)).FirstOrDefault
            If Not updatedEPGItem Is Nothing Then Me.Update(updatedEPGItem)
        End If
    End Function

    Private Async Function ShowConfirmDeletionPrompt() As Task
        'Get the language specific text strings
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        Dim strMessage As String = vm.loader.GetString("RecordingAbortContent")
        Dim strheader As String = vm.loader.GetString("RecordingAbortHeader")
        Dim msgBox As New MessageDialog(strMessage, strheader)
        msgBox.Commands.Add(New UICommand(vm.loader.GetString("Yes"), Sub()
                                                                          continueWithDeletion = True
                                                                      End Sub))
        msgBox.Commands.Add(New UICommand(vm.loader.GetString("No")))
        Await msgBox.ShowAsync()
    End Function

    Private Async Function StartEventRecording() As Task
        Dim vm = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasDVRAccess Then
            Dim response As New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 2}}
            If vm.appSettings.ProposeAutoRecording Then
                'Provide Prompt to user to select single/auto recording and dvrconfig
                Dim a As New cDialogRecordEPGItem
                Dim recpars As New RecordContentDialogViewModel(Me)
                If Me.serieslinkId <> 0 Then recpars.ShowSeriesButton = True Else recpars.ShowSeriesButton = False
                a.DataContext = recpars
                Dim p As ContentDialogResult = Await a.ShowAsync()

                If p = ContentDialogResult.Primary Then
                    'User clicked OK button, to start recording
                    If recpars.SingleRecording Then response = Await Task.Run(Function() RecordProgram(Me, recpars.selectedDVRConfig))
                    If recpars.AutoRecording Then response = Await Task.Run(Function() RecordProgramBySeries(Me, recpars.selectedDVRConfig))
                    If recpars.SeriesRecording Then response = Await Task.Run(Function() RecordProgramBySeries(Me, recpars.selectedDVRConfig))

                Else
                    'User cancelled out of the ContentDialogBox by Pressing Cancel Button
                End If
            Else
                'Start recording as once, without providing any dvrconfig
                response = Await Task.Run(Function() RecordProgram(Me))
            End If
            Select Case response.tvhResponse.success
                Case "0"
                    'Error during starting Recording 
                    Dim strMessage As String = vm.loader.GetString("RecordingStartErrorContent")
                    Dim strheader As String = vm.loader.GetString("RecordingStartErrorHeader")
                    Dim msgBox As New MessageDialog(strMessage, strheader)
                    msgBox.Commands.Add(New UICommand(vm.loader.GetString("OK")))
                    Await msgBox.ShowAsync()
                Case "2"
                    'Recording command cancelled, do nothing
            End Select

            'Retrieve the EPGItem again from the TVH server, which now should contain updated information around the DVR status
            Dim updatedEPGItem As EPGItemViewModel = (Await LoadEPGEventByID(Me.eventId)).FirstOrDefault
            If Not updatedEPGItem Is Nothing Then Me.Update(updatedEPGItem)


        End If

    End Function

    ''' <summary>
    ''' 'Updates the EPGItemViewModel with new properties retrieved from a refreshed equivalent EPGItem
    ''' </summary>
    ''' <param name="epgitem"></param>
    ''' <remarks></remarks>
    Public Async Sub Update(Optional epgitem As EPGItemViewModel = Nothing)
        Await RunOnUIThread(Sub()
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
            _EPGItem = New tvh40.EPGEvent With {.title = vm.loader.GetString("NoInformationAvailable"), .description = "", .dvrUuid = "", .dvrState = "", .eventId = 0}
        End If
        Status = "Existing"
    End Sub

    ''' <summary>
    ''' 'Create a new EPGViewModel from a TVH 3.9 / 4.X JSON entry
    ''' </summary>
    ''' <param name="epg_item"></param>
    ''' <remarks></remarks>
    Public Sub New(epg_item As tvh40.EPGEvent)
        _EPGItem = epg_item
        'IsRecorded = 1
        RecordButtonEnabled = False
        Status = "New"
    End Sub
#End Region

End Class

