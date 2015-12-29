Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports TVHead_81.Common
Imports TVHead_81.ViewModels
Imports Windows.UI.Popups

Public Class EPGItemViewModel
    Inherits ViewModelBase

    Private Property continueWithDeletion As Boolean


    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
        End Get

    End Property

    Public Property ExpandCollapseCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("EPGItemViewModel.ExpanseCollapseCommand()", "start")
                                        For Each group In vm.SelectedChannel.epgitems.groupeditems
                                            For Each epgitem In group
                                                If epgitem Is Me Then
                                                    If (Me.ExpandedView = "Collapsed" Or Me.ExpandedView = "") Then
                                                        Me.ExpandedView = "Expanded"
                                                        Me.RecordButtonEnabled = True
                                                    Else
                                                        Me.ExpandedView = "Collapsed"
                                                        Me.RecordButtonEnabled = False
                                                    End If
                                                Else
                                                    If epgitem.ExpandedView = "Expanded" Then
                                                        epgitem.ExpandedView = "Collapsed"
                                                        epgitem.RecordButtonEnabled = False
                                                    End If
                                                End If
                                            Next
                                        Next
                                        WriteToDebug("EPGItemViewModel.ExpanseCollapseCommand()", "stop")
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property howToRecord As Integer

    Public Property selectedDVRConfig As DVRConfigViewModel




    Public Property RecordCommand As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        'Me.createAutoRecording = False
                                        'Me.createSeriesRecording = False
                                        'Me.createSingleRecording = False
                                        Me.continueWithDeletion = False

                                        WriteToDebug("EPGItemViewModel.RecordCommand()", "Executed")
                                        If IsRecorded = 1 Or IsRecorded = 2 Or IsRecorded = 3 Then
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

                                        If Not vm.appSettings.LongPollingEnabled And vm.hasDVRAccess Then
                                            vm.UpcomingRecordings.Reload(True)
                                            vm.FinishedRecordings.Reload(True)
                                            vm.FailedRecordings.Reload(True)
                                            If Not vm.SelectedChannel Is Nothing AndAlso Me.channelUuid = vm.SelectedChannel.channelUuid Then
                                                vm.SelectedChannel.RefreshEPG(True)
                                            End If
                                            Dim c As ChannelViewModel = (From chan In vm.Channels.items Where chan.currentEPGItem.eventId = Me.eventId).FirstOrDefault()
                                            If Not c Is Nothing Then
                                                c.RefreshCurrentEPGItem(Nothing, True)
                                            End If
                                        End If



                                        'Collapse the view of the parent channel, in case the EPG Item is the current epgitem for a channel
                                        If Not vm.Channels Is Nothing Then
                                            Dim c = (From a In vm.Channels.items Where a.channelUuid = Me.channelUuid Select a).FirstOrDefault
                                            If Not c Is Nothing AndAlso c.ExpandedView = "Visible" Then
                                                c.ExpandedView = "Collapsed"
                                            End If

                                        End If

                                        'Collapse the view of the channel, in case the EPG Item is residing in a search query
                                        If Not vm.SearchPage Is Nothing AndAlso Not vm.SearchPage.GroupedSearchResults Is Nothing Then
                                            For Each g In vm.SearchPage.GroupedSearchResults
                                                Dim c = (From a In g Where a.channelUuid = Me.channelUuid AndAlso a.currentEPGItem.eventId = Me.eventId Select a).FirstOrDefault
                                                If Not c Is Nothing AndAlso c.ExpandedView = "Visible" Then
                                                    c.ExpandedView = "Collapsed"
                                                End If
                                            Next

                                        End If
                                        Me.ExpandedView = "Collapsed"
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Property channelName As String
    Public Property channelNumber As Integer
    Public Property channelUuid As String
    Public Property episodeId As Long
    Public Property episodeUri As String
    Public Property subtitle As String

    Private Property _ShowSubtitles As Integer
    Public ReadOnly Property SubtitleVisibility As String
        Get
            If subtitle <> "" Then Return "Visible" Else Return "Collapsed"
        End Get
    End Property

    Private Property _ShowDescription As Integer
    Public ReadOnly Property DescriptionVisibility As String
        Get
            If subtitle = "" Then Return "Visible" Else Return "Collapsed"
        End Get
    End Property



    Public Property serieslinkId As Integer
    Public Property serieslinkUri As String
    Public Property eventId As Long
    Public Property dvrUuid As String
        Get
            Return _dvrUuid
        End Get
        Set(value As String)
            If _dvrUuid <> value Then
                _dvrUuid = value
                RaisePropertyChanged()
            End If
        End Set
    End Property
    Private Property _dvrUuid As String

    Public Property dvrState As String
        Get
            Return _dvrState
        End Get
        Set(value As String)
            _dvrState = value
            Select Case value
                Case "recording"
                    IsRecorded = 1
                Case "scheduled"
                    IsRecorded = 3
                Case "recordingError"
                    IsRecorded = 2
                Case Else
                    IsRecorded = 0
            End Select
            RaisePropertyChanged()
        End Set
    End Property
    Private Property _dvrState As String


    Public Property genre As New List(Of Integer)
    Public ReadOnly Property genreName As String
        Get
            'Dim app As App = CType(Application.Current, App)
            If Not genre Is Nothing Then
                If Not genre.Count = 0 Then
                    Return (From g In vm.AllGenres.items Where g.uuid = genre(0) Select g.name).FirstOrDefault
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Get
    End Property
    Public Property title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property
    Private Property _title As String
    Private Property _description As String
    Public Property description As String
        Get
            Return _description
        End Get
        Set(value As String)
            If Not value Is Nothing Then
                _description = value.Replace(ChrW(10), "")
            Else
                _description = ""
            End If
        End Set
    End Property
    Public Property start As Integer
        Get
            Return _start
        End Get
        Set(value As Integer)
            startDate = UnixToDateTime(value).ToLocalTime

            'TODO : Fix times
            'Dim rs As String = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion
            'Dim dtFormatter As New DateTimeFormatter("shorttime", New String() {rs})
            'startDateString = dtFormatter.Format(startDate)

            _start = value
        End Set
    End Property
    Private Property _start As Integer


    Public Property [stop] As Integer
        Get
            Return _stop
        End Get
        Set(value As Integer)


            _stop = value
        End Set
    End Property
    Private Property _stop As Integer
    Public Property startDate As DateTime
    Public Property endDate As DateTime

    Public ReadOnly Property startDateString As String
        Get
            Return startDate.ToString("t")
        End Get
    End Property


    Public ReadOnly Property endDateString As String
        Get
            Return endDate.ToString("t")
        End Get
    End Property



    Public Property percentcompleted As Double
        Get
            If (endDate > Date.MinValue) And (startDate > Date.MinValue) And (Date.Now > startDate) Then
                If Date.Now > endDate Then
                    Return 1
                Else
                    Return Math.Round((Date.Now - startDate).TotalSeconds / (endDate - startDate).TotalSeconds, 2)
                End If
            Else
                Return 0
            End If
            Return _percentcompleted

        End Get
        Set(value As Double)
            _percentcompleted = value
            RaisePropertyChanged()

        End Set
    End Property
    Private Property _percentcompleted As Double

    'Public ReadOnly Property RecordingIconVisibility As String
    '    Get
    '        If IsRecorded Then Return "Visible" Else Return "Collapsed"
    '    End Get
    'End Property

    Private Property _RecordButtonEnabled As Boolean
    Public Property RecordButtonEnabled As Boolean
        Get
            Return _RecordButtonEnabled
        End Get
        Set(value As Boolean)
            _RecordButtonEnabled = value
            RaisePropertyChanged()
        End Set

    End Property

    Private Property _IsRecorded As Integer
    Public Property IsRecorded As Integer
        Get
            Return _IsRecorded
        End Get
        Set(value As Integer)
            _IsRecorded = value
            RaisePropertyChanged()
        End Set

    End Property

    Public Property ExpandedView As String
        Get
            Return _ExpandedView
        End Get
        Set(value As String)
            If value <> _ExpandedView Then
                _ExpandedView = value
                RaisePropertyChanged()
            End If
        End Set
    End Property
    Private Property _ExpandedView As String
    'Defines 3 possible states in which the ViewModel can be in, which triggers transitions between states
    ' New : Item is a new item (TODO : Insert transition effect with opactiy 0-->1, and visibilty 0-->1
    ' Existing : Item is already present. Sets the visibility to 1
    ' Remove : Item is (about to get) removed. 
    Public Property Status As String
        Get
            Return _Status
        End Get
        Set(value As String)
            _Status = value
            RaisePropertyChanged()
        End Set
    End Property
    Private Property _Status As String


    Public ReadOnly Property progressBarBackgroundBrush



    Public Async Function StopEventRecording() As Task
        If vm.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.hasDVRAccess Then
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
        End If
    End Function


    Private Async Function ShowConfirmDeletionPrompt() As Task
        'Get the language specific text strings
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
        If vm.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.hasDVRAccess Then
            Dim response As New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 2}}
            If vm.appSettings.ProposeAutoRecording Then
                'Provide Prompt to user to select single/auto recording and dvrconfig
                Dim a As New cDialogRecordEPGItem
                Dim recpars As New RecordContentDialogViewModel With {.dvrconfigs = vm.DVRConfigs.items, .selectedDVRConfigIndex = 0}
                If Me.serieslinkId <> 0 Then recpars.ShowSeriesButton = True Else recpars.ShowSeriesButton = False
                recpars.Init()
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
        End If

    End Function


    'Public Async Function StartEventRecording() As Task
    'Dim response As New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 2}}

    'If createAutoRecording Then
    '    WriteToDebug("EPGItemViewModel.StartEventRecording()", "StartRecording Executed, auto recording selected")
    '    response = Await Task.Run(Function() RecordProgramBySeries(Me))
    'End If
    'If createSeriesRecording Then
    '    WriteToDebug("EPGItemViewModel.StartEventRecording()", "StartRecording Executed, Series recording selected")
    '    response = Await Task.Run(Function() RecordProgramBySeries(Me))
    'End If
    'If createSingleRecording Then
    '    WriteToDebug("EPGItemViewModel.StartEventRecording()", "StartRecording Executed, single recording selected")
    '    response = Await Task.Run(Function() RecordProgram(Me))
    'End If

    'Select Case response.tvhResponse.success


    '    Case "0"
    '        'Error during starting Recording 
    '        Dim strMessage As String = vm.loader.GetString("RecordingStartErrorContent")
    '        Dim strheader As String = vm.loader.GetString("RecordingStartErrorHeader")
    '        Dim msgBox As New MessageDialog(strMessage, strheader)
    '        msgBox.Commands.Add(New UICommand(vm.loader.GetString("OK")))
    '        Await msgBox.ShowAsync()
    '    Case "2"
    '        'Recording command cancelled, do nothing
    'End Select

    'End Function


    Public Sub New()
        title = vm.loader.GetString("NoInformationAvailable")
        Status = "Existing"
        description = ""
        dvrUuid = ""
        dvrState = ""
        IsRecorded = 0
        eventId = 0
        ExpandedView = "Collapsed"
    End Sub

    ''' <summary>
    ''' 'Updates the EPGItemViewModel with new properties retrieved from a refreshed equivalent EPGItem
    ''' </summary>
    ''' <param name="epgitem"></param>
    ''' <remarks></remarks>
    Public Async Sub Update(Optional epgitem As EPGItemViewModel = Nothing)
        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         If Not epgitem Is Nothing Then
                                                                                                             dvrState = epgitem.dvrState
                                                                                                             dvrUuid = epgitem.dvrUuid
                                                                                                             percentcompleted = epgitem.percentcompleted
                                                                                                         Else
                                                                                                             percentcompleted = 1 'Retriggers the iRaisePropertyChanged
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



    ''' <summary>
    ''' 'Create a new EPGViewModel from a TVH 3.9 JSON entry
    ''' </summary>
    ''' <param name="epg_item"></param>
    ''' <remarks></remarks>
    Public Sub New(epg_item As tvh40.EPGEvent)
        'Dim rs As String = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion
        'Dim dtFormatter As New DateTimeFormatter("shorttime", New String() {rs})
        'Dim dtFormatter As New DateTimeFormatter("shorttime")

        title = epg_item.title
        description = epg_item.description
        dvrState = epg_item.dvrState
        dvrUuid = epg_item.dvrUuid
        eventId = epg_item.eventId
        episodeId = epg_item.episodeId
        episodeUri = epg_item.episodeUri
        serieslinkId = epg_item.serieslinkId
        serieslinkUri = epg_item.serieslinkUri
        subtitle = epg_item.subtitle
        genre = epg_item.genre

        start = epg_item.start
        startDate = UnixToDateTime(epg_item.start).ToLocalTime

        [stop] = epg_item.stop
        endDate = UnixToDateTime(epg_item.stop).ToLocalTime
        channelName = epg_item.channelName
        channelUuid = epg_item.channelUuid
        ExpandedView = ""
        RecordButtonEnabled = False


    End Sub

End Class

