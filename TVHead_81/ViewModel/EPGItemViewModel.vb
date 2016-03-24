Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports TVHead_81.ViewModels
Imports Windows.UI.Popups
Imports GalaSoft.MvvmLight.Command

Public Class EPGItemViewModel
    Inherits ViewModelBase
    Private _EPGItem As tvh40.EPGEvent

#Region "Properties"
    Private ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
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
            Return UnixToDateTime(_EPGItem.stop).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property endDateString As String
        Get
            Return UnixToDateTime(_EPGItem.stop).ToLocalTime.ToString("t")
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
    Public ReadOnly Property genre As List(Of Integer)
        Get
            Return _EPGItem.genre
        End Get
    End Property
    Public ReadOnly Property genreName As String
        Get
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
    Public ReadOnly Property RecordingIcon As String
        Get
            If RecordingStatus = 1 Or RecordingStatus = 3 Then
                Return "/images/player_record.png"
            Else
                Return "/images/player_record_off.png"
            End If
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
            If (endDate > Date.MinValue) And (startDate > Date.MinValue) And (Date.Now > startDate) Then
                If Date.Now > endDate Then
                    Return 1
                Else
                    Return Math.Round((Date.Now - startDate).TotalSeconds / (endDate - startDate).TotalSeconds, 2)
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
    Public ReadOnly Property start As Integer
        Get
            Return _EPGItem.start
        End Get
    End Property
    Public ReadOnly Property startDate As DateTime
        Get
            Return UnixToDateTime(_EPGItem.start).ToLocalTime
        End Get
    End Property
    Public ReadOnly Property startDateString As String
        Get
            Return UnixToDateTime(_EPGItem.start).ToLocalTime.ToString("t")
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

                                        Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
                                        If rectie.Width > 720 Then
                                            vm.selectedEPGItem = Me
                                        Else
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

                                        If Not vm.appSettings.LongPollingEnabled And vm.TVHeadSettings.hasDVRAccess Then
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
#End Region

#Region "Methods"
    Public Async Function StopEventRecording() As Task
        If vm.TVHeadSettings.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.TVHeadSettings.hasDVRAccess Then
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
        If vm.TVHeadSettings.hasDVRAccess = False Then
            Await vm.checkDVRAccess()
        End If
        If vm.TVHeadSettings.hasDVRAccess Then
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
        If Not vm Is Nothing Then
            _EPGItem = New tvh40.EPGEvent With {.title = vm.loader.GetString("NoInformationAvailable"), .description = "", .dvrUuid = "", .dvrState = "", .eventId = 0}
            Status = "Existing"
            ExpandedView = "Collapsed"
        End If
    End Sub

    ''' <summary>
    ''' 'Create a new EPGViewModel from a TVH 3.9 / 4.X JSON entry
    ''' </summary>
    ''' <param name="epg_item"></param>
    ''' <remarks></remarks>
    Public Sub New(epg_item As tvh40.EPGEvent)
        _EPGItem = epg_item
        ExpandedView = ""
        'IsRecorded = 1
        RecordButtonEnabled = False
        Status = "New"
    End Sub
#End Region

End Class

