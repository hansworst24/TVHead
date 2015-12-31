'Imports Windows.UI
Imports TVHead_81.Common
Imports Windows.UI.Popups
Imports GalaSoft.MvvmLight
'Imports System.Net.Http
Imports System.Threading
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports Newtonsoft.Json
Imports Windows.Web.Http
Imports Windows.Globalization.DateTimeFormatting
Imports System.Globalization

Namespace ViewModels




    Public Class TVHead_ViewModel
        Implements INotifyPropertyChanged

        Public myCultureInfoHelper As CultureInfoHelper = New CultureInfoHelper

        Public Property DiskSpaceStats As New DiskspaceUpdateViewModel

        Public Property hasEPGAccess As Boolean
        Public Property hasDVRAccess As Boolean
        Public Property hasFailedDVRAccess As Boolean
        Public Property hasAdminAccess As Boolean

        Public Property SearchPage As New SearchPageViewModel

        Public Event PropertyChanged As PropertyChangedEventHandler _
                Implements INotifyPropertyChanged.PropertyChanged
        Private Async Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
                                                                                                         End Sub)

        End Sub

        Public Class cometStats
            Inherits ViewModelBase
            Public Property _intDVRCreate As Integer
            Public Property intDVRCreate As Integer
                Get
                    Return _intDVRCreate
                End Get
                Set(value As Integer)
                    _intDVRCreate = value
                    RaisePropertyChanged("intDVRCreate")
                End Set
            End Property
            Private Property _intDVRChange As Integer
            Public Property intDVRChange As Integer
                Get
                    Return _intDVRChange
                End Get
                Set(value As Integer)
                    _intDVRChange = value
                    RaisePropertyChanged("intDVRChange")
                End Set
            End Property
            Private Property _intDVRUpdate As Integer
            Public Property intDVRUpdate As Integer
                Get
                    Return _intDVRUpdate
                End Get
                Set(value As Integer)
                    _intDVRUpdate = value
                    RaisePropertyChanged("intDVRUpdate")
                End Set
            End Property
            Private Property _intDVRDelete As Integer
            Public Property intDVRDelete As Integer
                Get
                    Return _intDVRDelete
                End Get
                Set(value As Integer)
                    _intDVRDelete = value
                    RaisePropertyChanged("intDVRDelete")
                End Set
            End Property

            Private Property _intEPGCreate As Integer
            Public Property intEPGCreate As Integer
                Get
                    Return _intEPGCreate
                End Get
                Set(value As Integer)
                    _intEPGCreate = value
                    RaisePropertyChanged("intEPGCreate")
                End Set
            End Property
            Private Property _intEPGChange As Integer
            Public Property intEPGChange As Integer
                Get
                    Return _intEPGChange
                End Get
                Set(value As Integer)
                    _intEPGChange = value
                    RaisePropertyChanged("intEPGChange")
                End Set
            End Property
            Private Property _intEPGUpdate As Integer
            Public Property intEPGUpdate As Integer
                Get
                    Return _intEPGUpdate
                End Get
                Set(value As Integer)
                    _intEPGUpdate = value
                    RaisePropertyChanged("intEPGUpdate")
                End Set
            End Property
            Private Property _intEPGDelete As Integer
            Public Property intEPGDelete As Integer
                Get
                    Return _intEPGDelete
                End Get
                Set(value As Integer)
                    _intEPGDelete = value
                    RaisePropertyChanged("intEPGDelete")
                End Set
            End Property
            Private Property _intEPGDVRUpdate As Integer
            Public Property intEPGDVRUpdate As Integer
                Get
                    Return _intEPGDVRUpdate
                End Get
                Set(value As Integer)
                    _intEPGDVRUpdate = value
                    RaisePropertyChanged("intEPGDVRUpdate")
                End Set
            End Property
            Private Property _intEPGDVRDelete As Integer
            Public Property intEPGDVRDelete As Integer
                Get
                    Return _intEPGDVRDelete
                End Get
                Set(value As Integer)
                    _intEPGDVRDelete = value
                    RaisePropertyChanged("intEPGDVRDelete")
                End Set
            End Property

            Private Property _intDVRAutoRecCreate As Integer
            Public Property intDVRAutoRecCreate As Integer
                Get
                    Return _intDVRAutoRecCreate
                End Get
                Set(value As Integer)
                    _intDVRAutoRecCreate = value
                    RaisePropertyChanged("intDVRAutoRecCreate")
                End Set
            End Property

            Private Property _intDVRAutoRecChange As Integer
            Public Property intDVRAutoRecChange As Integer
                Get
                    Return _intDVRAutoRecChange
                End Get
                Set(value As Integer)
                    _intDVRAutoRecChange = value
                    RaisePropertyChanged("intDVRAutoRecChange")
                End Set
            End Property

            Private Property _intDVRAutoRecUpdate As Integer
            Public Property intDVRAutoRecUpdate As Integer
                Get
                    Return _intDVRAutoRecUpdate
                End Get
                Set(value As Integer)
                    _intDVRAutoRecUpdate = value
                    RaisePropertyChanged("intDVRAutoRecUpdate")
                End Set
            End Property

            Private Property _intDVRAutoRecDelete As Integer
            Public Property intDVRAutoRecDelete As Integer
                Get
                    Return _intDVRAutoRecDelete
                End Get
                Set(value As Integer)
                    _intDVRAutoRecDelete = value
                    RaisePropertyChanged("intDVRAutoRecDelete")
                End Set
            End Property






            Public Async Sub AddComet(s As String)
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 'WriteToDebug("cometStats.AddComent", s)
                                                                                                                 Select Case s
                                                                                                                     Case "epgcreate"
                                                                                                                         intEPGCreate += 1
                                                                                                                     Case "epgchange"
                                                                                                                         intEPGChange += 1
                                                                                                                     Case "epgupdate"
                                                                                                                         intEPGUpdate += 1
                                                                                                                     Case "epgdelete"
                                                                                                                         intEPGDelete += 1
                                                                                                                     Case "epgdvrupdate"
                                                                                                                         intEPGDVRUpdate += 1
                                                                                                                     Case "epgdrvdelete"
                                                                                                                         intEPGDVRDelete += 1
                                                                                                                     Case "dvrcreate"
                                                                                                                         intDVRCreate += 1
                                                                                                                     Case "dvrchange"
                                                                                                                         intDVRChange += 1
                                                                                                                     Case "dvrupdate"
                                                                                                                         intDVRUpdate += 1
                                                                                                                     Case "dvrdelete"
                                                                                                                         intDVRDelete += 1
                                                                                                                     Case "dvrautorecchange"
                                                                                                                         intDVRAutoRecChange += 1
                                                                                                                     Case "dvrautoreccreate"
                                                                                                                         intDVRAutoRecCreate += 1
                                                                                                                     Case "dvrautorecdelete"
                                                                                                                         intDVRAutoRecDelete += 1
                                                                                                                     Case "dvrautorecupdate"
                                                                                                                         intDVRAutoRecUpdate += 1
                                                                                                                 End Select
                                                                                                             End Sub)

            End Sub

            Public Sub New()
            End Sub
        End Class

        'Reference to the app itself, in order to reference items outside the viewmodel (no idea if this is a valid way to do)
        'Public app As App = CType(Application.Current, App)

        ' Public appSettings As New AppSettings
        Public loader As New Windows.ApplicationModel.Resources.ResourceLoader()

        Public timer As New DispatcherTimer

        Public doCatchComents As Boolean

        Public CatchCometsBoxID As String

        Public ct As CancellationToken
        Public tokenSource As New CancellationTokenSource()

        Public CometCatcher As Task

#Region "Properties"

        'Bool which is used to determine of the TVH version is capable of returning properly formatted IDNode Information when requested. Only from a commit since Oct 2015 this seems
        'to work properly for recordings. It's set initially to true to try to load initially, but when that fails it is set to false.
        Public Property CapableOfLoadingRecordingIDNode As Boolean = False
        Public Property CapableOfLoadingAutoRecordingIDNode As Boolean = False
        Public Property CapableOfLoadingChannelIDNode As Boolean = False

        Public Property currentCometStats As New cometStats
        Public Property CometCatcherLastRun As DateTime

        Private Property _totalBytesReceived As Long
        Public Property totalBytesReceived As Long
            Get
                Return _totalBytesReceived
            End Get
            Set(value As Long)
                _totalBytesReceived = value
                If totalBytesReceivedUpdates > 5 Then
                    totalBytesReceivedUpdates = 0
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                               NotifyPropertyChanged()
                                                                                                           End Sub)
                Else
                    totalBytesReceivedUpdates += 1
                End If


            End Set
        End Property

        Public Property totalBytesReceivedUpdates As Integer



        Public Property TVHIPAddress As String
        Public Property TVHPort As String
        Public Property TVHUser As String
        Public Property TVHPass As String
        Public Property TVHVersion As String
        Public Property TVHVersionLong As String

        Public Const visible = "visible"
        Public Const collapsed = "collapsed"

        Public Property WaitingForDiskspaceUpdate As Boolean
            Get
                Return _WaitingForDiskspaceUpdate
            End Get
            Set(value As Boolean)
                _WaitingForDiskspaceUpdate = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _WaitingForDiskspaceUpdate As Boolean



        Public Property isConnected As Boolean
            Get
                Return _isConnected
            End Get
            Set(value As Boolean)
                _isConnected = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _isConnected As Boolean

        Public Property ConnectedRotation As Integer
            Get
                Return _ConnectedRotation
            End Get
            Set(value As Integer)
                If _ConnectedRotation <= 170 Then _ConnectedRotation = _ConnectedRotation + 10 Else _ConnectedRotation = 0
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _ConnectedRotation As Integer

        Public Property ToastMessages As New ToastListViewModel



        Public Property appSettings As AppSettings
            Get
                Return New AppSettings
            End Get
            Set(value As AppSettings)
                _appSettings = value
            End Set
        End Property
        Private Property _appSettings As AppSettings


        Public Property logmessages As New LogViewModel


        Public Property FreeDiskSpace As String
            Get
                Return _FreeDiskSpace
            End Get
            Set(value As String)
                _FreeDiskSpace = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _FreeDiskSpace As String

        Public Property TotalDiskSpace As String
            Get
                Return _TotalDiskSpace
            End Get
            Set(value As String)
                _TotalDiskSpace = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _TotalDiskSpace As String


        Public Property FreeDiskSpacePercentage As Double
            Get
                Return _FreeDiskSpacePercentage
            End Get
            Set(value As Double)
                _FreeDiskSpacePercentage = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _FreeDiskSpacePercentage As Double



        Public Property SelectedChannelIndex As Integer

        Public Property ChannelSelected As Boolean
            Get
                Return _ChannelSelected
            End Get
            Set(value As Boolean)
                _ChannelSelected = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _ChannelSelected As Boolean

        Private Property _EPGInformationAvailable As Boolean
        Public Property EPGInformationAvailable As Boolean
            Get
                If ChannelSelected = False Then
                    Return True
                Else
                    Return _EPGInformationAvailable
                End If
            End Get
            Set(value As Boolean)
                _EPGInformationAvailable = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _SelectedChannel As ChannelViewModel
        Public Property SelectedChannel As ChannelViewModel
            Get
                Return _SelectedChannel
            End Get
            Set(value As ChannelViewModel)
                _SelectedChannel = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _SearchText As String
        Public Property SearchText As String
            Get
                Return _SearchText
            End Get
            Set(value As String)
                _SearchText = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Property Streams As New StreamListViewModel
        Public Property Subscriptions As New SubscriptionListViewModel
        Public Property Services As New ServiceListViewModel
        Public Property Muxes As New MuxListViewModel
        Public Property UpcomingRecordings As New RecordingListViewModel With {.SortingOrder = .Ascending, .RecordingType = .upcomingRecordings}
        Public Property FinishedRecordings As New RecordingListViewModel With {.SortingOrder = .Descending, .RecordingType = .finishedRecordings}
        Public Property FailedRecordings As New RecordingListViewModel With {.SortingOrder = .Descending, .RecordingType = .failedRecordings}
        Public Property allEPGEvents As New List(Of EPGItemViewModel) 'Temp/Debug list not to be used (big memory)
        Public Property AutoRecordings As New AutoRecordingListViewModel
        Public Property StatusBar As New StatusUpdateViewModel
        Public Property AllChannels As New ChannelListViewModel
        Public Property Channels As New ChannelListViewModel
        Public Property ChannelTags As New ChannelTagListViewModel

        Public Property supportedLanguages As New LanguageList
        'Public Function FilterBySelectedChannelTag() As List(Of ChannelViewModel)
        '    Return AllChannels.items.Where(Function(y) y.tags.ToList.IndexOf(selectedChannelTag.uuid) > -1).ToList()
        'End Function

        Public Async Function CatchComets(ct As CancellationToken) As Task
            While Not ct.IsCancellationRequested
                '                CometCatcherLastRun = Date.Now
                Dim CometCatcherTaskID As String
                If Not CometCatcher Is Nothing Then CometCatcherTaskID = CometCatcher.Id.ToString
                'WriteToDebug("TVHead_ViewModel.CatchComets()", "Catching..." + CometCatcherTaskID)
                If CatchCometsBoxID = "" Then
                    WriteToDebug("TVHead_ViewModel.CatchComets()", "Catching new boxid...")
                    CatchCometsBoxID = Await GetBoxID()
                End If
                If Not CatchCometsBoxID = "" Then
                    Try
                        'isTimeOut = False
                        Dim response As HttpResponseMessage = Await (New Downloader).DownloadComet(CatchCometsBoxID)
                        If Not response.IsSuccessStatusCode Then
                            ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .msg = response.ReasonPhrase, .secondsToShow = 5})
                            WriteToDebug("TVHead_ViewModel.CatchComets()", "HTTP request error")
                            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                             isConnected = False
                                                                                                                         End Sub)
                            Await Task.Delay(2000)
                            doCatchComents = True
                            CatchCometsBoxID = ""
                        Else
                            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                             isConnected = True
                                                                                                                         End Sub)
                            Dim body As String = Await response.Content.ReadAsStringAsync()
                            'Dim reader = New StreamReader(body)
                            ' While Not reader.EndOfStream
                            Dim comet = JsonConvert.DeserializeObject(Of CometMessages.CometMessage)(body)
                            For Each message In comet.messages
                                ' tvh40 : subscriptions,input_status,dvrentry,logmessage,servicemapper,service_raw,service,caclient,connections
                                ' TVH41 : epg,dvrentry,subscriptions,input_status,logmessage,mpegts_mux,mpegts_network,diskspaceUpdate,servicemapper,service_raw
                                ' TVH34 : dvrdb
                                'WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Notification Class : {0}", message("notificationClass")))
                                Select Case message("notificationClass")
                                    Case "input_status"
                                        Dim input_message As CometMessages.input_status = JsonConvert.DeserializeObject(Of CometMessages.input_status)(message.ToString())
                                        If input_message.reload = 1 Then
                                            Await Streams.Reload()
                                        Else
                                            Await Streams.Update(input_message)
                                        End If

                                    Case "subscriptions"
                                        Dim subscription_message As CometMessages.subscription = JsonConvert.DeserializeObject(Of CometMessages.subscription)(message.ToString())
                                        If subscription_message.reload = 1 Then

                                            Await Subscriptions.Reload()
                                        Else
                                            Await Subscriptions.Update(subscription_message)
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
                                                currentCometStats.AddComet("dvrcreate")
                                                Dim updatedRecording As RecordingViewModel
                                                'updatedRecording = TryCast(Await LoadIDNode(m, New RecordingViewModel()), RecordingViewModel)
                                                updatedRecording = (From rec In (Await LoadUpcomingRecordings()) Where rec.recording_id = m Select rec).FirstOrDefault()
                                                If Not updatedRecording Is Nothing Then
                                                    'we received an update for a upcoming / running recording. handle it
                                                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                                                     Await Me.UpcomingRecordings.AddRecording(updatedRecording, True)
                                                                                                                                                 End Sub)

                                                End If
                                            Next
                                        End If
                                        If Not dvrEntry_message.change Is Nothing Then
                                            For Each m In dvrEntry_message.change
                                                currentCometStats.AddComet("dvrchange")
                                                'We received the uuid for a recording that was changed. Request the updated recording from the server by quering
                                                'for the ID Node
                                                If CapableOfLoadingRecordingIDNode Then
                                                    Dim updatedRecording As RecordingViewModel
                                                    ''TODO CHECK FOR ACCESS
                                                    updatedRecording = TryCast(Await LoadIDNode(m, New RecordingViewModel()), RecordingViewModel)

                                                    If Not updatedRecording Is Nothing Then
                                                        If updatedRecording.schedstate = "scheduled" Or updatedRecording.schedstate = "recording" Then
                                                            Await Me.UpcomingRecordings.UpdateRecording(updatedRecording)
                                                        End If
                                                        If updatedRecording.schedstate = "completed" Then
                                                            Await Me.UpcomingRecordings.RemoveRecording(updatedRecording.recording_id, False)
                                                            Await Me.FinishedRecordings.UpdateRecording(updatedRecording)
                                                        End If
                                                        If updatedRecording.schedstate = "completedError" Then
                                                            Await Me.UpcomingRecordings.RemoveRecording(updatedRecording.recording_id, False)
                                                            Await Me.FailedRecordings.UpdateRecording(updatedRecording)
                                                        End If
                                                    Else
                                                        'updatedRecording is Nothing, probably we don't have access somewhere
                                                        If hasDVRAccess Then Await Me.UpcomingRecordings.Reload(True)
                                                        If hasDVRAccess Then Await Me.FinishedRecordings.Reload(True)
                                                        If hasFailedDVRAccess Then Await Me.FailedRecordings.Reload(True)
                                                    End If
                                                Else
                                                    If hasDVRAccess Then Await Me.UpcomingRecordings.Reload(True)
                                                    If hasDVRAccess Then Await Me.FinishedRecordings.Reload(True)
                                                    If hasFailedDVRAccess Then Await Me.FailedRecordings.Reload(True)
                                                End If
                                            Next
                                        End If
                                        If Not dvrEntry_message.delete Is Nothing Then

                                            For Each m In dvrEntry_message.delete
                                                currentCometStats.AddComet("dvrdelete")
                                                'Without knowing in which list the recording is located, initiate deletion of the recording by targeting all lists
                                                If hasDVRAccess Then Await Me.UpcomingRecordings.RemoveRecording(m, True)
                                                If hasDVRAccess Then Await Me.FinishedRecordings.RemoveRecording(m, True)
                                                If hasFailedDVRAccess Then Await Me.FailedRecordings.RemoveRecording(m, True)

                                            Next
                                        End If

                                    Case "dvrautorec"
                                        Dim autorec_message As CometMessages.dvrautorec = JsonConvert.DeserializeObject(Of CometMessages.dvrautorec)(message.ToString())
                                        If Not autorec_message.change Is Nothing Then
                                            For Each m In autorec_message.change
                                                currentCometStats.AddComet("dvrautorecchange")
                                                If CapableOfLoadingAutoRecordingIDNode And hasDVRAccess Then
                                                    Dim updatedAutoRecording As AutoRecordingViewModel
                                                    updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.UpdateAutoRecording(updatedAutoRecording, True)
                                                End If
                                            Next

                                        End If
                                        If Not autorec_message.create Is Nothing Then
                                            For Each m In autorec_message.create
                                                currentCometStats.AddComet("dvrautoreccreate")
                                                If CapableOfLoadingAutoRecordingIDNode And hasDVRAccess Then
                                                    Dim updatedAutoRecording As AutoRecordingViewModel
                                                    updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.AddAutoRecording(updatedAutoRecording, True)
                                                End If
                                            Next
                                        End If

                                        If Not autorec_message.delete Is Nothing Then

                                            For Each m In autorec_message.delete
                                                currentCometStats.AddComet("dvrautorecdelete")
                                                Await Me.AutoRecordings.DeleteAutoRecording(m, True)
                                            Next
                                        End If

                                        If Not autorec_message.update Is Nothing Then
                                            For Each m In autorec_message.update
                                                currentCometStats.AddComet("dvrautorecupdate")
                                                If CapableOfLoadingAutoRecordingIDNode And hasDVRAccess Then
                                                    Dim updatedAutoRecording As AutoRecordingViewModel
                                                    updatedAutoRecording = TryCast(Await LoadIDNode(m, New AutoRecordingViewModel()), AutoRecordingViewModel)
                                                    If Not updatedAutoRecording Is Nothing Then Await Me.AutoRecordings.UpdateAutoRecording(updatedAutoRecording, True)
                                                End If
                                            Next
                                        End If


                                    Case "epg"
                                        Dim epg_message As CometMessages.epg = JsonConvert.DeserializeObject(Of CometMessages.epg)(message.ToString())
                                        'Provide the EPG message to SelectedChannel.GroupedEPGItems in order to process any changes
                                        If Not epg_message.delete Is Nothing Then
                                            For Each m In epg_message.delete
                                                currentCometStats.AddComet("epgdelete")
                                                'Await SelectedChannel.epgitems.RemoveEvent(m)
                                                'For Each g In SelectedChannel.GroupedEPGItems
                                                '    Dim oldEPGEvent As EPGItemViewModel = (From e In g Where e.eventId.Equals(m) Select e).FirstOrDefault()
                                                '    If Not oldEPGEvent Is Nothing Then
                                                '        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Deleting programme {0} : {1}", oldEPGEvent.title, m.ToString()))
                                                '        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                '                                                                                                         g.Remove(oldEPGEvent)
                                                '                                                                                                     End Sub)
                                                '    End If

                                                'Next
                                            Next
                                        End If
                                        If Not epg_message.update Is Nothing Then
                                            'Running Programmes get updated regularly (percent completed ?)
                                            For Each m In epg_message.update
                                                currentCometStats.AddComet("epgupdate")
                                                'Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()
                                                'If Not newEPGEvent Is Nothing Then
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                'Else
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                'End If
                                                'For Each g In SelectedChannel.GroupedEPGItems
                                                '    Dim oldEPGEvent As EPGItemViewModel = (From e In g Where e.eventId.Equals(m) Select e).FirstOrDefault()
                                                '    If Not oldEPGEvent Is Nothing Then
                                                '        WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Updating programme {0} : {1}", oldEPGEvent.title, m.ToString()))
                                                '        oldEPGEvent.Update()
                                                '    End If

                                                'Next
                                            Next
                                        End If

                                        If Not epg_message.create Is Nothing Then
                                            For Each m In epg_message.create
                                                currentCometStats.AddComet("epgcreate")
                                                'Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()
                                                'If Not newEPGEvent Is Nothing Then
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Creating programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                'Else
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Creating programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                'End If
                                                'If Not newEPGEvent Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.channelUuid Then
                                                '    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                '                                                                                                     Await SelectedChannel.AddEvent(newEPGEvent)
                                                '                                                                                                 End Sub)
                                                'End If
                                            Next
                                        End If

                                        If Not epg_message.change Is Nothing Then
                                            For Each m In epg_message.change
                                                currentCometStats.AddComet("epgchange")
                                                'Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()
                                                'If Not newEPGEvent Is Nothing Then
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Changing programme {0} : {1}", newEPGEvent.title, m.ToString()))
                                                'Else
                                                '    WriteToDebug("TVHead_ViewModel.CatchComets()", String.Format("Changing programme {0} : {1}", "EVENTID NOT ON SERVER", m.ToString()))
                                                'End If
                                                'If Not newEPGEvent Is Nothing AndAlso Not SelectedChannel Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.channelUuid Then
                                                '    Await SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                'End If
                                            Next
                                        End If
                                        If Not epg_message.dvr_update Is Nothing Then
                                            For Each m In epg_message.dvr_update
                                                currentCometStats.AddComet("epgdvrupdate")
                                                'Get the latest info for the EPGEvent
                                                Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()

                                                If Not newEPGEvent Is Nothing AndAlso Not SelectedChannel Is Nothing AndAlso Not SelectedChannel.epgitems Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.channelUuid Then
                                                    Await SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                End If

                                                Dim channel As ChannelViewModel = (From c In Channels.items Where c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                If Not channel Is Nothing Then
                                                    Await channel.RefreshCurrentEPGItem(newEPGEvent)
                                                End If

                                                If Not SearchPage Is Nothing AndAlso Not SearchPage.GroupedSearchResults Is Nothing Then
                                                    For Each g In SearchPage.GroupedSearchResults
                                                        Dim searchChannel As ChannelViewModel = (From c In g Where c.channelUuid = newEPGEvent.channelUuid AndAlso c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                        If Not searchChannel Is Nothing Then
                                                            Await searchChannel.RefreshCurrentEPGItem(newEPGEvent)
                                                        End If
                                                    Next

                                                End If

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
                                                currentCometStats.AddComet("epgdvrdelete")
                                                Dim newEPGEvent As EPGItemViewModel = (Await LoadEPGEventByID(New List(Of Integer)(New Integer() {m}))).FirstOrDefault()
                                                'Update any entry for the EPGEvent in the Selected Channel
                                                If Not newEPGEvent Is Nothing Then
                                                    'Update SelectedChannel's status
                                                    If Not SelectedChannel Is Nothing AndAlso Not SelectedChannel.epgitems Is Nothing AndAlso newEPGEvent.channelUuid = SelectedChannel.channelUuid Then
                                                        Await SelectedChannel.epgitems.UpdateEvent(newEPGEvent)
                                                    End If
                                                    'Update any channel with this EPG Event ID and tell it to refresh
                                                    Dim channel As ChannelViewModel = (From c In Channels.items Where c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                    If Not channel Is Nothing Then
                                                        Await channel.RefreshCurrentEPGItem(newEPGEvent)
                                                    End If

                                                    'Clean up any results in the Search Page
                                                    If Not SearchPage Is Nothing AndAlso Not SearchPage.GroupedSearchResults Is Nothing Then
                                                        For Each g In SearchPage.GroupedSearchResults
                                                            Dim searchChannel As ChannelViewModel = (From c In g Where c.channelUuid = newEPGEvent.channelUuid AndAlso c.currentEPGItem.eventId = newEPGEvent.eventId Select c).FirstOrDefault()
                                                            If Not searchChannel Is Nothing Then
                                                                Await searchChannel.RefreshCurrentEPGItem(newEPGEvent)
                                                            End If
                                                        Next
                                                    End If

                                                Else
                                                    'EPG Item doesn't exist on the server anymore
                                                End If
                                            Next
                                        End If

                                    Case "logmessage"
                                        Dim log_message As CometMessages.logUpdate = JsonConvert.DeserializeObject(Of CometMessages.logUpdate)(message.ToString())
                                        logmessages.Add(log_message.logtxt)
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
                                        Dim diskspace_message As CometMessages.diskspaceUpdate = JsonConvert.DeserializeObject(Of CometMessages.diskspaceUpdate)(message.ToString())
                                        'Dim d As DiskspaceUpdateViewModel = New DiskspaceUpdateViewModel(diskspace_message)
                                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                         DiskSpaceStats.Update(diskspace_message)
                                                                                                                                     End Sub)


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
                        'Perform some other updates while we're polling but only every 10 seconds or more
                        If CometCatcherLastRun.AddSeconds(10) <= Date.Now Then
                            If Not SelectedChannel Is Nothing Then
                                Await SelectedChannel.RefreshEPG(False)
                                'Await SelectedChannel.RemoveCompletedEvents()
                                'Await SelectedChannel.UpdateEventProgress()
                            End If
                            If Not Me.AppBar.ButtonEnabled.refreshButton = False And Not Channels Is Nothing And Not Channels.items.Count = 0 Then
                                For Each c In Channels.items
                                    Await c.RefreshCurrentEPGItem()
                                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                     c.Status = "Existing"
                                                                                                                                 End Sub)

                                Next
                            End If
                            CometCatcherLastRun = Date.Now()
                        End If
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         ConnectedRotation += 10
                                                                                                                     End Sub)
                    Catch ex As Exception
                        If Not ex.Message Is Nothing Then
                            WriteToDebug("TVHead_ViewModel.CatchComets()", "Some error occured, resting for a second : Explanation " + ex.Message.ToString())
                        Else
                            WriteToDebug("TVHead_ViewModel.CatchComets()", "Some error occured, resting for a second : No explanation given...")
                        End If
                    End Try
                Else
                    WriteToDebug("TVHead_ViewModel.CatchComets()", "Didn't get a boxid, sleeping for 5 seconds...")
                    'app.isConnected = False
                    Await Task.Delay(5000)
                End If
                'End While
                'WriteToDebug("TVHead_ViewModel.CatchComets", "doCatchComets = False, Sleeping...")
                'Await Task.Delay(5000)

            End While
            If ct.IsCancellationRequested Then
                ct.ThrowIfCancellationRequested()
            End If
        End Function





        Private Property _StatusPivotSelectedIndex As Integer
        Public Property StatusPivotSelectedIndex As Integer
            Get
                Return _StatusPivotSelectedIndex
            End Get
            Set(value As Integer)
                _StatusPivotSelectedIndex = value
                NotifyPropertyChanged()
            End Set
        End Property



        Private Property _PivotSelectedIndex As Integer
        Public Property PivotSelectedIndex As Integer
            Get
                Return _PivotSelectedIndex
            End Get
            Set(value As Integer)
                _PivotSelectedIndex = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _CommandBarVisibility As Visibility
        Public Property CommandBarVisibility As Visibility
            Get
                Return _CommandBarVisibility
            End Get
            Set(value As Visibility)
                _CommandBarVisibility = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _ChannelTagFlyoutIsOpen As Boolean
        Public Property ChannelTagFlyoutIsOpen As Boolean
            Get
                Return _ChannelTagFlyoutIsOpen
            End Get
            Set(value As Boolean)
                _ChannelTagFlyoutIsOpen = value
                NotifyPropertyChanged()
            End Set
        End Property


        'Private Property _GroupedEPGItems As ObservableCollection(Of Group(Of EPGItemViewModel))
        'Public Property GroupedEPGItems As ObservableCollection(Of Group(Of EPGItemViewModel))
        '    Get
        '        Return _GroupedEPGItems
        '    End Get
        '    Set(value As ObservableCollection(Of Group(Of EPGItemViewModel)))
        '        _GroupedEPGItems = value
        '        NotifyPropertyChanged()
        '    End Set
        'End Property



        Private Property _selectedChannelTag As ChannelTagViewModel
        Public Property selectedChannelTag As ChannelTagViewModel
            Get
                Return _selectedChannelTag
            End Get
            Set(value As ChannelTagViewModel)
                _selectedChannelTag = value
                'selectedChannelTagName = value.name
            End Set
        End Property
        Public Property favouriteChannelTag As ChannelTagViewModel

        'Public Property ChannelEPGItems As IEnumerable(Of EPGItemViewModel)

        Public Property AllGenres As New ContentTypeListViewModel
        Public Property Genres As New ContentTypeListViewModel

        'Public Property ContentTypes As IEnumerable(Of ContentTypeViewModel)
        Public Property DVRConfigs As New DVRConfigListViewModel


        Private Property _selectedChannelTagName As String
        Public Property selectedChannelTagName As String
            Get
                Return _selectedChannelTagName
            End Get
            Set(value As String)
                _selectedChannelTagName = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Property AppBar As New ApplicationBar


        Public Class ApplicationBar
            Inherits ViewModelBase

            'Implements INotifyPropertyChanged

            'Public Event PropertyChanged As PropertyChangedEventHandler _
            '        Implements INotifyPropertyChanged.PropertyChanged
            'Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            'End Sub

            Private Property _CommandBarVisibility As Visibility
            Public Property CommandBarVisibility As Visibility
                Get
                    Return _CommandBarVisibility
                End Get
                Set(value As Visibility)
                    _CommandBarVisibility = value
                    RaisePropertyChanged("CommandBarVisibility")
                End Set
            End Property

            Public Property ButtonVisibility As New ButtonsVisibility
            Public Property ButtonEnabled As New ButtonsEnabled

        End Class

        Public Class ButtonsEnabled
            Inherits ViewModelBase

            Public Sub New()
                refreshButton = True
            End Sub
            'Implements INotifyPropertyChanged

            'Public Event PropertyChanged As PropertyChangedEventHandler _
            '        Implements INotifyPropertyChanged.PropertyChanged
            'Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            'End Sub
            Private Property _deleteButton As Boolean
            Public Property deleteButton As Boolean
                Get
                    Return _deleteButton
                End Get
                Set(value As Boolean)
                    _deleteButton = value
                    RaisePropertyChanged("deleteButton")
                End Set
            End Property

            Private Property _refreshButton As Boolean
            Public Property refreshButton As Boolean
                Get
                    Return _refreshButton
                End Get
                Set(value As Boolean)
                    _refreshButton = value
                    RaisePropertyChanged("refreshButton")
                End Set
            End Property
        End Class

        Public Class ButtonsVisibility
            Inherits ViewModelBase
            'Implements INotifyPropertyChanged

            'Public Event PropertyChanged As PropertyChangedEventHandler _
            '        Implements INotifyPropertyChanged.PropertyChanged
            'Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            'End Sub
            Private Property _refreshButton As Visibility
            Public Property refreshButton As Visibility
                Get
                    Return _refreshButton
                End Get
                Set(value As Visibility)
                    _refreshButton = value
                    RaisePropertyChanged("refreshButton")
                End Set
            End Property

            Private Property _recordingsButton As Visibility
            Public Property recordingsButton As Visibility
                Get
                    Return _recordingsButton
                End Get
                Set(value As Visibility)
                    _recordingsButton = value
                    RaisePropertyChanged("recordingsButton")
                End Set
            End Property
            Private Property _epgButton As Visibility
            Public Property epgButton As Visibility
                Get
                    Return _epgButton
                End Get
                Set(value As Visibility)
                    _epgButton = value
                    RaisePropertyChanged("epgButton")
                End Set
            End Property
            Private Property _tagsButton As Visibility
            Public Property tagsButton As Visibility
                Get
                    Return _tagsButton
                End Get
                Set(value As Visibility)
                    _tagsButton = value
                    RaisePropertyChanged("tagsButton")
                End Set
            End Property
            Private Property _manageButton As Visibility
            Public Property manageButton As Visibility
                Get
                    Return _manageButton
                End Get
                Set(value As Visibility)
                    _manageButton = value
                    RaisePropertyChanged("manageButton")
                End Set
            End Property
            Private Property _searchButton As Visibility
            Public Property searchButton As Visibility
                Get
                    Return _searchButton
                End Get
                Set(value As Visibility)
                    _searchButton = value
                    RaisePropertyChanged("searchButton")
                End Set
            End Property
            Private Property _addButton As Visibility
            Public Property addButton As Visibility
                Get
                    Return _addButton
                End Get
                Set(value As Visibility)
                    _addButton = value
                    RaisePropertyChanged("addButton")
                End Set
            End Property
            Private Property _deleteButton As Visibility
            Public Property deleteButton As Visibility
                Get
                    Return _deleteButton
                End Get
                Set(value As Visibility)
                    _deleteButton = value
                    RaisePropertyChanged("deleteButton")
                End Set
            End Property
            Private Property _saveButton As Visibility
            Public Property saveButton As Visibility
                Get
                    Return _saveButton
                End Get
                Set(value As Visibility)
                    _saveButton = value
                    RaisePropertyChanged("saveButton")
                End Set
            End Property
            Private Property _cancelButton As Visibility
            Public Property cancelButton As Visibility
                Get
                    Return _cancelButton
                End Get
                Set(value As Visibility)
                    _cancelButton = value
                    RaisePropertyChanged("cancelButton")
                End Set
            End Property

            Public Sub New()
                deleteButton = Visibility.Collapsed
            End Sub

        End Class



#Region "RelayCommands"

        Public Property AddCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            'Navigate Away to the Add AutoRecording Page
                                            If PivotSelectedIndex = 5 Then
                                                selectedAutoRecording = New AutoRecordingViewModel
                                                Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                                If Not rootFrame.Navigate(GetType(AutoRecordingPage), selectedAutoRecording) Then
                                                    Dim resources As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")
                                                    Throw New Exception(resources.GetString("NavigationFailedExceptionMessage"))
                                                End If
                                            End If

                                            'Navigate Away to the Add Recording Page
                                            If PivotSelectedIndex = 2 Then
                                                Dim recording As New RecordingViewModel
                                                Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                                If Not rootFrame.Navigate(GetType(RecordingPage), recording) Then
                                                    Dim resources As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")
                                                    Throw New Exception(resources.GetString("NavigationFailedExceptionMessage"))
                                                End If
                                            End If
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property




        Public Property SearchTextChangedCommand As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            Await Task.Delay(100)
                                            WriteToDebug("TVHead_ViewModel.SearchTextChangedCommand()", String.Format("SearchKeyUpCommand Executed : Text = {0}", SearchText))
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property



        Public Property SearchCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            WriteToDebug("TVHead_ViewModel.SearchCommand()", "SearchCommand Executed")
                                            'Me.StopRefresh()
                                            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                            If Not rootFrame.Navigate(GetType(SearchPage), Me.SearchPage) Then
                                                Throw New Exception("Failed to create initial page")
                                            End If
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property GoToRecordingsCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            PivotSelectedIndex = 2
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property GoToChannelsCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            PivotSelectedIndex = 0
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property


        Public Property StatusCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            WriteToDebug("TVHead_ViewModel.StatusCommand", "start")
                                            'Me.StopRefresh()
                                            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                            If Not rootFrame.Navigate(GetType(StatusPage)) Then
                                                Throw New Exception("Failed to create initial page")
                                            End If
                                            WriteToDebug("TVHead_ViewModel.StatusCommand", "stop")
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property AboutCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            WriteToDebug("TVHead_ViewModel.AboutCommand", "start")
                                            'Me.StopRefresh()
                                            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                            If Not rootFrame.Navigate(GetType(AboutPage)) Then
                                                Throw New Exception("Failed to create initial page")
                                            End If
                                            WriteToDebug("TVHead_ViewModel.AboutCommand", "stop")
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property


        Public Property SettingsCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            'Me.StopRefresh()
                                            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                            If Not rootFrame.Navigate(GetType(AppSettingsPage)) Then
                                                Throw New Exception("Failed to create initial page")
                                            End If
                                            WriteToDebug("TVHead_ViewModel.SettingsCommand()", "SettingsCommand Executed")
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property selectedRecordings As New List(Of RecordingViewModel)

        Public Property selectedAutoRecording As AutoRecordingViewModel




        Public Property DeleteSelectedRecordings As RelayCommand
            Get
                Return New RelayCommand(Async Sub(x)
                                            'Handle the deletion of Autorecording Entries
                                            If PivotSelectedIndex = 5 Then
                                                Dim myList As New AutoRecordingListViewModel
                                                myList = AutoRecordings
                                                Dim succesfulDeletions As Integer = 0
                                                Dim ContinueWithDeletion As Boolean = False
                                                If Not myList Is Nothing Then
                                                    If appSettings.ConfirmDeletion Then
                                                        Dim strheader As String = loader.GetString("AutoRecordingDeleteHeader")
                                                        Dim strMessage As String = String.Format(loader.GetString("AutoRecordingDeleteContent"),
                                                                                                 myList.items.Where(Function(y) y.IsSelected).Count.ToString,
                                                                                                 myList.items.Count)
                                                        Dim msgBox As New MessageDialog(strMessage, strheader)
                                                        msgBox.Commands.Add(New UICommand(loader.GetString("OK"), Sub()
                                                                                                                      ContinueWithDeletion = True
                                                                                                                  End Sub))
                                                        msgBox.Commands.Add(New UICommand(loader.GetString("Cancel"), Sub()
                                                                                                                          ContinueWithDeletion = False
                                                                                                                      End Sub))

                                                        Await msgBox.ShowAsync()
                                                    Else
                                                        ContinueWithDeletion = True
                                                    End If

                                                End If
                                                If ContinueWithDeletion Then
                                                    For Each recording In myList.items.Where(Function(y) y.IsSelected)
#If DEBUG Then
                                                        Dim retValue As RecordingReturnValue = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 1}}
#Else
                                                        Dim retValue As RecordingReturnValue = Await DeleteAutoRecording(recording.id)
#End If
                                                        If retValue.tvhResponse.success = 1 Then
                                                            succesfulDeletions += 1
                                                        Else
                                                            Dim strheader As String = loader.GetString("AutoRecordingDeleteErrorHeader")
                                                            Dim strMessage As String = String.Format(loader.GetString("AutoRecordingDeleteErrorContent"),
                                                                                                     recording.title)
                                                            Dim msgBox As New MessageDialog(strMessage, strheader)
                                                            msgBox.Commands.Add(New UICommand(loader.GetString("OK")))
                                                            recording.IsSelected = False
                                                            Await msgBox.ShowAsync()

                                                        End If
                                                    Next
                                                    'Reload the Autorecordings, and Upcoming Recordings
                                                    myList.items = (Await LoadAutoRecordings()).ToObservableCollection()
                                                    WriteToDebug("TVHead_ViewModel.DeleteSelectedRecordings()", String.Format("DeleteSelectedRecordings - {0} items deleted...", succesfulDeletions.ToString))
                                                End If
                                                myList.MultiSelectMode = ListViewSelectionMode.None
                                                AutoRecordings.SetExpanseCollapseEnabled(True)
                                                SetApplicationBarButtons()
                                            End If

                                            'Handle the deletion of Upcoming, failed or finished recordings
                                            If PivotSelectedIndex = 2 Or PivotSelectedIndex = 3 Or PivotSelectedIndex = 4 Then
                                                Dim myRecList As New RecordingListViewModel
                                                'Dirty way to identify in which RecordingViewmodel we're working
                                                If PivotSelectedIndex = 2 Then Await UpcomingRecordings.AbortSelectedRecordings()
                                                If PivotSelectedIndex = 3 Then Await FinishedRecordings.AbortSelectedRecordings()
                                                If PivotSelectedIndex = 4 Then Await FailedRecordings.AbortSelectedRecordings()
                                                UpcomingRecordings.SetExpanseCollapseEnabled(True)
                                                FinishedRecordings.SetExpanseCollapseEnabled(True)
                                                FailedRecordings.SetExpanseCollapseEnabled(True)
                                                SetApplicationBarButtons()
                                            End If
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property


        Public Property SearchResults As New ObservableCollection(Of ChannelViewModel)

        ''' <summary>
        ''' Gets called through the refresh button on the status.xaml pages application bar
        ''' </summary>
        ''' <returns></returns>
        Public Property RefreshStatusCommand As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            RefreshStatus()
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property


        Public Property RefreshCommand As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            If Me.AppBar.ButtonEnabled.refreshButton = True Then
                                                Me.AppBar.ButtonEnabled.refreshButton = False
                                            End If
                                            If Me.PivotSelectedIndex = 0 And Not Me.Channels Is Nothing Then Await Me.Channels.RefreshCurrentEvents()
                                            If Me.PivotSelectedIndex = 1 And Not Me.SelectedChannel Is Nothing Then Await Task.Run(Function() Me.SelectedChannel.RefreshEPG(True))
                                            If Me.PivotSelectedIndex = 2 Then Await Task.Run(Function() Me.UpcomingRecordings.Reload(False))
                                            If Me.PivotSelectedIndex = 3 Then Await Task.Run(Function() Me.FinishedRecordings.Reload(False))
                                            If Me.PivotSelectedIndex = 4 Then Await Task.Run(Function() Me.FailedRecordings.Reload(False))
                                            If Me.PivotSelectedIndex = 5 Then Await Task.Run(Function() Me.AutoRecordings.Reload())
                                            Me.AppBar.ButtonEnabled.refreshButton = True

                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property






        Public Property MultiSelectCommand As RelayCommand
            Get
                Return New RelayCommand(Sub()
                                            WriteToDebug("TVHead_ViewModel.MultiSelectCommand", "start")
                                            Select Case Me.PivotSelectedIndex
                                                Case 2 'When the view is on the Upcoming Recordings Listview
                                                    If UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None Then
                                                        UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
                                                        UpcomingRecordings.SetExpanseCollapseEnabled(False)
                                                        SetApplicationBarButtons("manage")
                                                    Else
                                                        UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None
                                                        UpcomingRecordings.SetExpanseCollapseEnabled(True)
                                                        SetApplicationBarButtons()
                                                    End If
                                                Case 3 'When the view is on the Finished Recordings Listview
                                                    If FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None Then
                                                        FinishedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
                                                        FinishedRecordings.SetExpanseCollapseEnabled(False)
                                                        SetApplicationBarButtons("manage")
                                                    Else
                                                        FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None
                                                        FinishedRecordings.SetExpanseCollapseEnabled(True)
                                                        SetApplicationBarButtons()
                                                    End If
                                                Case 4 'When the view is on the Failed Recordings Listview
                                                    If FailedRecordings.MultiSelectMode = ListViewSelectionMode.None Then
                                                        FailedRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
                                                        FailedRecordings.SetExpanseCollapseEnabled(False)
                                                        SetApplicationBarButtons("manage")
                                                    Else
                                                        FailedRecordings.MultiSelectMode = ListViewSelectionMode.None
                                                        FailedRecordings.SetExpanseCollapseEnabled(True)
                                                        SetApplicationBarButtons()
                                                    End If
                                                Case 5 'When the view is on the AutoRecordings Listview
                                                    If AutoRecordings.MultiSelectMode = ListViewSelectionMode.None Then
                                                        AutoRecordings.MultiSelectMode = ListViewSelectionMode.Multiple
                                                        AutoRecordings.SetExpanseCollapseEnabled(False)
                                                        SetApplicationBarButtons("manage")
                                                    Else
                                                        AutoRecordings.MultiSelectMode = ListViewSelectionMode.None
                                                        AutoRecordings.SetExpanseCollapseEnabled(True)
                                                        SetApplicationBarButtons()
                                                    End If
                                            End Select

                                            WriteToDebug("TVHead_ViewModel.MultiSelectCommand", "stop")
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property PivotSelectionChanged As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            SetApplicationBarButtons()
                                            'Dim app As App = CType(Application.Current, App)
                                            WriteToDebug("TVHead_ViewModel.PivotSelectionChanged()", "PivotSelectionChanged - Executed")

                                            'Remove MultiSelect for all recording lists
                                            UpcomingRecordings.MultiSelectMode = ListViewSelectionMode.None
                                            FinishedRecordings.MultiSelectMode = ListViewSelectionMode.None
                                            FailedRecordings.MultiSelectMode = ListViewSelectionMode.None
                                            AutoRecordings.MultiSelectMode = ListViewSelectionMode.None
                                            UpcomingRecordings.SetExpanseCollapseEnabled(True)
                                            FinishedRecordings.SetExpanseCollapseEnabled(True)
                                            FailedRecordings.SetExpanseCollapseEnabled(True)
                                            AutoRecordings.SetExpanseCollapseEnabled(True)
                                            SetApplicationBarButtons()
                                            'If Me.PivotSelectedIndex = 2 And Me.UpcomingRecordings.dataLoaded Then Await Me.UpcomingRecordings.Reload()
                                            'If Me.PivotSelectedIndex = 3 And Me.FinishedRecordings.dataLoaded Then Await Me.FinishedRecordings.Reload()
                                            'If Me.PivotSelectedIndex = 4 And Me.FailedRecordings.dataLoaded Then Await Me.FailedRecordings.Reload()
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property


#End Region

#End Region


#Region "Methods"

        Public Sub SetApplicationBarButtons(Optional mode As String = "")
            If mode = "" Then
                Select Case Me.PivotSelectedIndex
                    Case 0
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Visible
                            .epgButton = Visibility.Collapsed
                            .tagsButton = Visibility.Visible
                            .manageButton = Visibility.Collapsed
                            .searchButton = Visibility.Visible
                            .addButton = Visibility.Collapsed
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                    Case 1
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Visible
                            .epgButton = Visibility.Collapsed
                            .tagsButton = Visibility.Collapsed
                            .manageButton = Visibility.Collapsed
                            .searchButton = Visibility.Visible
                            .addButton = Visibility.Collapsed
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                    Case 2
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Collapsed
                            .epgButton = Visibility.Visible
                            .tagsButton = Visibility.Collapsed
                            .manageButton = Visibility.Visible
                            .searchButton = Visibility.Collapsed
                            .addButton = Visibility.Visible
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                    Case 3
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Collapsed
                            .epgButton = Visibility.Visible
                            .tagsButton = Visibility.Collapsed
                            .manageButton = Visibility.Visible
                            .searchButton = Visibility.Collapsed
                            .addButton = Visibility.Collapsed
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                    Case 4
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Collapsed
                            .epgButton = Visibility.Visible
                            .tagsButton = Visibility.Collapsed
                            .manageButton = Visibility.Visible
                            .searchButton = Visibility.Collapsed
                            .addButton = Visibility.Collapsed
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                    Case 5
                        With Me.AppBar.ButtonVisibility
                            .refreshButton = Visibility.Visible
                            .recordingsButton = Visibility.Collapsed
                            .epgButton = Visibility.Visible
                            .tagsButton = Visibility.Collapsed
                            .manageButton = Visibility.Visible
                            .searchButton = Visibility.Collapsed
                            .addButton = Visibility.Visible
                            .deleteButton = Visibility.Collapsed
                            .saveButton = Visibility.Collapsed
                            .cancelButton = Visibility.Collapsed
                        End With
                        Me.AppBar.CommandBarVisibility = Visibility.Visible
                End Select
            End If
            Select Case mode
                Case "manage"
                    With Me.AppBar.ButtonVisibility
                        .deleteButton = Visibility.Visible
                        .refreshButton = Visibility.Collapsed
                        .recordingsButton = Visibility.Collapsed
                        .epgButton = Visibility.Collapsed
                        .tagsButton = Visibility.Collapsed
                        .manageButton = Visibility.Collapsed
                        .searchButton = Visibility.Collapsed
                        .addButton = Visibility.Collapsed
                        .saveButton = Visibility.Collapsed
                        .cancelButton = Visibility.Collapsed
                        '.deleteButton = Visibility.Visible

                    End With
                Case "add"
                    With Me.AppBar.ButtonVisibility
                        .recordingsButton = Visibility.Collapsed
                        .epgButton = Visibility.Collapsed
                        .tagsButton = Visibility.Collapsed
                        .manageButton = Visibility.Collapsed
                        .searchButton = Visibility.Collapsed
                        .addButton = Visibility.Collapsed
                        .deleteButton = Visibility.Collapsed
                        .saveButton = Visibility.Visible
                        .cancelButton = Visibility.Visible
                    End With
            End Select

        End Sub




        Public Async Function LoadCurrentEPGEventForChannels(easyOnBandwidth As Boolean) As Task
            WriteToDebug("ChannelViewModel.LoadCurrentEPGEventForChannels()", "start")

            'We have a choice of retrieving all current EPG Events from the TVH server each time when doing a refresh, or only pull the current EPG event for each channel for which 
            'the percentcompleted = 1.
            'The latter will cause less bandwidth usage, but will not retreive any updates for the existing EPG event, for example when it has been changed through another app)
            'Alternatively we can combine currently scheduled recordings with the local EPG info on the phone, to only update Recording status and percentcompleted

            If Not easyOnBandwidth Then
                'Initial load of the current EPG Events for all channels.

                Dim currentEPGItems = Await LoadEPGEntry(New ChannelViewModel, False, AllChannels.items.Count)
                If Not currentEPGItems Is Nothing Then
                    For Each c In Channels.items
                        Dim newItemForChannel = (From p In currentEPGItems Where p.channelUuid = c.channelUuid Select p).OrderBy(Function(x) x.startDate).FirstOrDefault
                        If Not newItemForChannel Is Nothing Then
                            Await c.RefreshCurrentEPGItem(newItemForChannel)
                        Else
                            If c.currentEPGItem Is Nothing Then c.currentEPGItem = New EPGItemViewModel()
                            'c.AddEmptyEPGDetails()
                        End If

                    Next
                End If
            Else
                'Dim recordings = Await LoadUpcomingRecordings()

                For Each c In Channels.items
                    If Not c.currentEPGItem Is Nothing Then
                        If c.currentEPGItem.percentcompleted = 1 Then
                            Await c.RefreshCurrentEPGItem()
                            'Don't hammer the server too much during initial load, add a pause for each request
                            'Await Task.Delay(100)
                        Else
                            If c.currentEPGItem.eventId <> 0 Then
                                c.currentEPGItem.percentcompleted = 1
                            End If

                        End If
                    Else
                        Await c.RefreshCurrentEPGItem()
                    End If


                Next
            End If
            For Each c In Channels.items.Where(Function(x) x.Status = "Updated")
                c.Status = "Existing"
            Next
            WriteToDebug("ChannelViewModel.LoadCurrentEPGEventForChannels()", "stop")
            'Me.Channels = (From c In AllChannels Where c.tags.ToList.IndexOf(selectedChannelTag.uuid) > -1 Select c Order By c.number).ToObservableCollection()
        End Function


        Public Async Function checkConnection() As Task(Of Boolean)
            Dim AmIConnected As Boolean
            Await StatusBar.Update(loader.GetString("status_Connecting"), True, 0, True, True)
            Try
                Dim sInfo As ServerInfoViewModel = Await GetServerInfo()
                If Not sInfo Is Nothing Then
                    appSettings.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
                    'app.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
                    TVHVersion = sInfo.sw_version
                    AmIConnected = True
                    Await StatusBar.Update(loader.GetString("status_Connected"), True, 0, True, True)
                Else
                    AmIConnected = False
                    Await StatusBar.Update(loader.GetString("status_ConnectionError"), True, 0, False, False)
                End If
            Catch ex As Exception
                AmIConnected = False
                ToastMessages.AddMessage(New ToastMessageViewModel With {.msg = ex.Message, .isError = True, .secondsToShow = 2})
            End Try
            Await StatusBar.Clean()
            Return AmIConnected

        End Function

        Public Async Function checkEPGAccess(Optional report As Boolean = False) As Task
            Dim epgAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New tvh40.api40).apiGetEPGEvents("", False, 1))
            If epgAccessResponse.IsSuccessStatusCode Then
                hasEPGAccess = True
            Else
                hasEPGAccess = False
                If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 2, .msg = "Error accessing EPG : " + epgAccessResponse.ReasonPhrase})
            End If

        End Function
        Public Async Function checkAdminAccess(Optional report As Boolean = False) As Task
            Dim adminAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New tvh40.api40).apiGetSubscriptions())
            If adminAccessResponse.IsSuccessStatusCode Then
                hasAdminAccess = True
            Else
                hasAdminAccess = False
                If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 2, .msg = "Error accessing Admin : " + adminAccessResponse.ReasonPhrase})
            End If

        End Function

        Public Async Function checkFailedDVRAccess(Optional report As Boolean = False) As Task
            Dim dvrAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New tvh40.api40).apiGetFailedRecordings())
            If dvrAccessResponse.IsSuccessStatusCode Then
                hasFailedDVRAccess = True
            Else
                hasFailedDVRAccess = False
                If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 5, .msg = "Error accessing Failed DVR : " + dvrAccessResponse.ReasonPhrase})
            End If

        End Function


        Public Async Function checkDVRAccess(Optional report As Boolean = False) As Task
            Dim dvrAccessResponse As HttpResponseMessage = Await (New Downloader).DownloadJSON((New tvh40.api40).apiGetUpcomingRecordings())
            If dvrAccessResponse.IsSuccessStatusCode Then
                hasDVRAccess = True
            Else
                hasDVRAccess = False
                If report Then ToastMessages.AddMessage(New ToastMessageViewModel With {.isError = True, .secondsToShow = 5, .msg = "Error accessing DVR : " + dvrAccessResponse.ReasonPhrase})
            End If

        End Function


        Public Async Function checkCapabilities() As Task
            If isConnected Then
                If Not Me.AllChannels Is Nothing AndAlso Me.AllChannels.items.Count > 0 Then
                    Dim testChannel As ChannelViewModel = CType(Await LoadIDNode(AllChannels.items(0).channelUuid, New ChannelViewModel), ChannelViewModel)
                    If Not testChannel Is Nothing Then
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test channel " + testChannel.name)
                    Else
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test channel ")
                    End If
                End If
                If Not Me.UpcomingRecordings Is Nothing AndAlso Me.UpcomingRecordings.groupeditems.Count > 0 Then
                    Dim testRecording As RecordingViewModel = CType(Await LoadIDNode(Me.UpcomingRecordings.groupeditems(0)(0).recording_id, New RecordingViewModel), RecordingViewModel)
                    If Not testRecording Is Nothing Then
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test recording " + testRecording.title)
                    Else
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test recording ")
                    End If
                End If
                If Not Me.AutoRecordings Is Nothing AndAlso Me.AutoRecordings.items.Count > 0 Then
                    Dim testAutoRecording As AutoRecordingViewModel = CType(Await LoadIDNode(Me.AutoRecordings.items(0).id, New AutoRecordingViewModel), AutoRecordingViewModel)
                    If Not testAutoRecording Is Nothing Then
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode passed for test auto recording " + testAutoRecording.title)
                    Else
                        WriteToDebug("TVHead_ViewModel.checkCapabilities()", "LoadIDNode FAILED for test auto recording ")
                    End If
                End If
            End If
        End Function

        Public Async Function checkAccess(Optional report As Boolean = False) As Task
            'Checks the capabilities and authorization of the TVH server and used account
            Await checkEPGAccess(report)
            Await checkDVRAccess(report)
            Await checkFailedDVRAccess(report)
            Await checkAdminAccess(report)

        End Function

        Public Async Function LoadDataAsync() As Task
            'This method ensures all initial data is loaded into the ViewModel.

            'Avoid running this task multiple times at once. Set use the refreshbutton in the appbar to control if the task is running or not.
            If Me.AppBar.ButtonEnabled.refreshButton = True Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 Me.AppBar.ButtonEnabled.refreshButton = False
                                                                                                             End Sub)
                WriteToDebug("TVHead_ViewModel.LoadDataAsync()", "start")

                isConnected = Await checkConnection()
                If isConnected Then Await checkAccess()

                If isConnected And hasEPGAccess Then
                    If Me.ChannelTags.dataLoaded = False Then Await LoadChannelTags()
                    If Me.DVRConfigs.dataLoaded = False Then Await Me.DVRConfigs.Load()
                    If Me.AllGenres.dataLoaded = False Then Await LoadContentTypes(True)
                    If Me.Genres.dataLoaded = False Then Await LoadContentTypes(False)
                    If Me.AllChannels.dataLoaded = False Then Await Me.AllChannels.LoadAll()
                    If Me.Channels.dataLoaded = False Then Await Me.Channels.LoadFavouriteTagChannels()

                    'REFRESH EPG INFORMATION FOR THE SELECTED CHANNEL. REFRESH EPG INFORMATION IF THE CHANNELS HAVE ALREADY BEEN LOADED AND THE PIVOT IS ON THE CHANNELS PAGE
                    If Me.PivotSelectedIndex = 0 And Not Me.Channels Is Nothing And Me.Channels.dataLoaded = True Then
                        Await StatusBar.Update(loader.GetString("status_RefreshingChannels"), True, 0, True)
                        Await Channels.RefreshCurrentEvents()
                        'For Each c In Channels.items
                        '    Await c.RefreshCurrentEPGItem()
                        '    'Await Task.Delay(1000)
                        'Next

                    End If
                    Me.Channels.dataLoaded = True

                    'REFRESH EPG INFORMATION FOR THE SELECTED CHANNEL. REFRESH EPG INFORMATION IF THE CHANNELS HAVE ALREADY BEEN LOADED AND THE PIVOT IS ON THE CHANNELS PAGE
                    If Me.PivotSelectedIndex = 1 And Not Me.SelectedChannel Is Nothing Then
                        Await StatusBar.Update(loader.GetString("status_RefreshingEPGEntries"), True, 0, True)
                        Await Task.Run(Function() Me.SelectedChannel.RefreshEPG(False))
                    End If


                    If hasDVRAccess Then
                        'LOAD UPCOMING RECORDINGS, OR REFRESH IF THE PIVOT ON THE UPCOMINGS RECORDINGS PAGE
                        If UpcomingRecordings.dataLoaded = False Or Me.PivotSelectedIndex = 2 Then
                            Await Me.UpcomingRecordings.Load()

                        End If
                        'LOAD FINISHED RECORDINGS, OR REFRESH IF THE PIVOT ON THE FINISHED RECORDINGS PAGE
                        If FinishedRecordings.dataLoaded = False Or Me.PivotSelectedIndex = 3 Then
                            'Await StatusBar.Update(loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
                            'Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                            Await Me.FinishedRecordings.Load()
                            'End Sub)

                        End If

                        'LOAD FAILED RECORDINGS, OR REFRESH IF THE PIVOT ON THE FAILED RECORDINGS PAGE
                        ''TODO : RE-ENABLE DVR STUFF
                        If FailedRecordings.dataLoaded = False Or Me.PivotSelectedIndex = 4 Then
                            Await Me.FailedRecordings.Load()
                        End If
                        ''LOAD AUTO RECORDINGS, OR REFRESH IF THE PIVOT ON THE AUTO RECORDINGS PAGE
                        If AutoRecordings.dataLoaded = False Or Me.PivotSelectedIndex = 5 Then
                            Await StatusBar.Update(loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
                            Await Me.AutoRecordings.Load()
                        End If

                    End If

                    Await checkCapabilities()
                    Await StatusBar.Clean()
                End If


                'Enable the refresh button again
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 Me.AppBar.ButtonEnabled.refreshButton = True
                                                                                                             End Sub)

            End If

        End Function



        Public Async Function StartRecording(item As Object) As Task(Of RecordingReturnValue)
            Dim response As New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 2}}
            Dim createAutoRecording As Boolean = False
            Dim createSeriesRecording As Boolean = False
            Dim createSingleRecording As Boolean = False

            Dim epgitem As New EPGItemViewModel
            If TypeOf (item) Is EPGItemViewModel Then
                epgitem = item
            Else
                epgitem = CType(item, ChannelViewModel).currentEPGItem
            End If


            'Show a MessageBox, asking for the user to confirm whether a single or an auto-recording / Series should be created

            If appSettings.ProposeAutoRecording Then
                Dim strMessage As String = ""
                Dim strheader As String = ""

                If epgitem.serieslinkId <> 0 Then
                    'The EPG Item is identified as a series, propose to create a Series Recording
                    strMessage = loader.GetString("RecordingProposeSeriesRecordingContent")
                    strheader = loader.GetString("RecordingProposeSeriesRecordingHeader")
                    Dim msgBox As New MessageDialog(strMessage, strheader)
                    msgBox.Commands.Add(New UICommand(loader.GetString("Once"), Sub()
                                                                                    createSingleRecording = True
                                                                                End Sub))
                    msgBox.Commands.Add(New UICommand(loader.GetString("SeriesRecording"), Sub()
                                                                                               createSeriesRecording = True
                                                                                           End Sub))
                    Await msgBox.ShowAsync()

                Else
                    'Propose to create a auto-recording

                    strMessage = loader.GetString("RecordingProposeAutoRecordingContent")
                    strheader = loader.GetString("RecordingProposeAutoRecordingHeader")
                    Dim msgBox As New MessageDialog(strMessage, strheader)
                    msgBox.Commands.Add(New UICommand(loader.GetString("Once"), Sub()
                                                                                    createSingleRecording = True
                                                                                End Sub))
                    msgBox.Commands.Add(New UICommand(loader.GetString("AutoRecording"), Sub()
                                                                                             createAutoRecording = True
                                                                                         End Sub))
                    Await msgBox.ShowAsync()
                End If


            End If
            If createAutoRecording Then
                WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, auto recording selected")
                response = Await RecordProgramBySeries(epgitem)
            End If
            If createSeriesRecording Then
                WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, Series recording selected")
                response = Await RecordProgramBySeries(epgitem)
            End If
            If createSingleRecording Then
                WriteToDebug("TVHead_ViewModel.StartRecording()", "StartRecording Executed, single recording selected")
                response = Await RecordProgram(epgitem)
            End If
            Return response
        End Function

#End Region

#Region "Constructor"
        Public Sub New()
            'Dim refreshInterval As Integer = appSettings.RefreshRate
            'WaitingForDiskspaceUpdate = True
            'If Not refreshInterval = 0 Then
            '    ChannelTagFlyoutIsOpen = False
            '    PivotSelectedIndex = 0
            '    AddHandler timer.Tick, AddressOf RefreshMe
            '    timer.Interval = New TimeSpan(0, 0, refreshInterval)
            '    ChannelSelected = False
            'timer.Start()
            'StatusBar.Clean()

            'End If

        End Sub
#End Region

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
                'tokenSource.Dispose()
            End If

            WriteToDebug("TVHead_ViewModel.StopRefresh()", "")
        End Sub

        Public Async Sub Cleanup()
            If Not Me.AllChannels Is Nothing Then Me.AllChannels.items.Clear()
            If Not Me.Channels Is Nothing Then Me.Channels.items.Clear()
            Me.SelectedChannel = Nothing
            'Me.ChannelEPGItems = Nothing
            Me.UpcomingRecordings = Nothing
            Me.FinishedRecordings = Nothing
            Me.FailedRecordings = Nothing
            Me.AutoRecordings = Nothing
        End Sub

        Public Async Sub RefreshStatus()
            If isConnected Then
                If Not Me.hasAdminAccess Then
                    Await checkAdminAccess(True)
                End If
                If Me.hasAdminAccess Then
                    Await StatusBar.Update("Refreshing streams...", True, 0, True)
                    Me.Streams.items = (Await LoadStreams()).ToObservableCollection()
                    Await StatusBar.Update("Refreshing subscriptions...", True, 0, True)
                    Me.Subscriptions.items = (Await LoadSubscriptions()).ToObservableCollection()
                    Await StatusBar.Clean()

                End If
            End If
        End Sub



        Public Async Sub RefreshMe()

            Await Me.LoadDataAsync()

            'WriteToDebug("TVHead_ViewModel.RefreshMe()", "start")
            'If Not app.isConnected Then
            '    Try
            '        StatusBar.Update("Connecting...", True, 0, True, False)

            '        Dim sInfo As ServerInfoViewModel = Await GetServerInfo()
            '        If Not sInfo Is Nothing Then
            '            app.TVHVersionLong = String.Format("{0} {1}", sInfo.name, sInfo.sw_versionlong)
            '            app.TVHVersion = sInfo.sw_version
            '            app.isConnected = True
            '        Else
            '            app.isConnected = False
            '        End If
            '    Catch ex As Exception
            '        app.isConnected = False
            '    End Try
            'End If

            'If app.isConnected Then
            '    If Me.AppBar.ButtonEnabled.refreshButton = True Then

            '        Me.AppBar.ButtonEnabled.refreshButton = False
            '        Select Case Me.PivotSelectedIndex
            '            Case 0
            '                'Dim strheader As String = loader.GetString("RefreshingChannels")
            '                StatusBar.Update(loader.GetString("status_RefreshingChannels"), True, 0, True)
            '                If AllChannels.items.Count = 0 Then
            '                    Await LoadAllChannels()

            '                End If
            '                If Me.Channels Is Nothing OrElse Me.Channels.Count = 0 Then
            '                    Await SetChannelTags()
            '                    If Not ChannelTags.items.Count = 0 Then
            '                        Me.Channels = AllChannels.items.Where(Function(x) x.tags.ToList.IndexOf(selectedChannelTag.uuid) > -1).ToList()
            '                    Else
            '                        Me.Channels = AllChannels.items
            '                    End If
            '                Else
            '                    'Await LoadAllChannels()
            '                    Await LoadCurrentEPGEventForChannels(True)

            '                End If
            '            Case 1
            '                StatusBar.Update(loader.GetString("status_RefreshingEPGEntries"), True, 0, True)
            '                If Not Me.SelectedChannel Is Nothing Then
            '                    Await Me.SelectedChannel.RefreshEPG()
            '                End If
            '            Case 2
            '                StatusBar.Update(loader.GetString("status_RefreshingUpcomingRecordings"), True, 0, True)
            '                Await UpcomingRecordings.RefreshRecordings((Await LoadUpcomingRecordings()).ToList())
            '            Case 3
            '                StatusBar.Update(loader.GetString("status_RefreshingFinishedRecordings"), True, 0, True)
            '                Await FinishedRecordings.RefreshRecordings((Await LoadFinishedRecordings()).ToList())
            '            Case 4
            '                StatusBar.Update(loader.GetString("status_RefreshingFailedRecordings"), True, 0, True)
            '                Await FailedRecordings.RefreshRecordings((Await LoadFailedRecordings()).ToList())
            '            Case 5
            '                StatusBar.Update(loader.GetString("status_RefreshingAutoRecordings"), True, 0, True)
            '                Await AutoRecordings.RefreshAutoRecordings(True)
            '                'Await LoadAutoRecordings()


            '        End Select
            '        StatusBar.Clean()
            '        Me.AppBar.ButtonEnabled.refreshButton = True
            '        Debug.WriteLine("RefreshMe() ended...")
            '    End If
            'End If

            'WriteToDebug("TVHead_ViewModel.RefreshMe()", "stop")
        End Sub


    End Class

    Public Class ToastListViewModel
        Public Property Messages As ObservableCollection(Of ToastMessageViewModel)

        Public Sub New()
            Messages = New ObservableCollection(Of ToastMessageViewModel)

        End Sub

        Public Async Sub AddMessage(msg As ToastMessageViewModel)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             Messages.Insert(0, msg)
                                                                                                         End Sub)
            Await Task.Delay(New TimeSpan(0, 0, msg.secondsToShow))
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             msg.isGoing = True
                                                                                                         End Sub)

            Await Task.Delay(1000)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             Messages.Remove(msg)
                                                                                                         End Sub)
        End Sub

    End Class

    Public Class ToastMessageViewModel
        Inherits ViewModelBase
        Public Property msg As String
        Public Property isGoing As Boolean
            Get
                Return _isGoing
            End Get
            Set(value As Boolean)
                _isGoing = value
                RaisePropertyChanged("isGoing")
            End Set
        End Property
        Private Property _isGoing As Boolean

        Public Property isError As Boolean
            Get
                Return _isError
            End Get
            Set(value As Boolean)
                _isError = value
                RaisePropertyChanged("isError")
            End Set
        End Property
        Private Property _isError As Boolean

        Public Property secondsToShow As Integer
    End Class




    Public Class Language
        Public Property code As String
        Public Property val As String
    End Class

    Public Class LanguageList
        Public Property languages As New List(Of Language)
        Private ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property

        Public Sub New()
            Dim myRegion = New Windows.Globalization.GeographicRegion


            languages.Add(New Language With {.code = "af", .val = "Afrikaans"})
            languages.Add(New Language With {.code = "ak", .val = "Akan"})
            languages.Add(New Language With {.code = "sq", .val = "Albanian"})
            languages.Add(New Language With {.code = "am", .val = "Amharic"})
            languages.Add(New Language With {.code = "ar", .val = "Arabic"})
            languages.Add(New Language With {.code = "an", .val = "Aragonese"})
            languages.Add(New Language With {.code = "hy", .val = "Armenian"})
            languages.Add(New Language With {.code = "as", .val = "Assamese"})
            languages.Add(New Language With {.code = "av", .val = "Avaric"})
            languages.Add(New Language With {.code = "ae", .val = "Avestan"})
            languages.Add(New Language With {.code = "ay", .val = "Aymara"})
            languages.Add(New Language With {.code = "az", .val = "Azerbaijani"})
            languages.Add(New Language With {.code = "bm", .val = "Bambara"})
            languages.Add(New Language With {.code = "ba", .val = "Bashkir"})
            languages.Add(New Language With {.code = "eu", .val = "Basque"})
            languages.Add(New Language With {.code = "be", .val = "Belarusian"})
            languages.Add(New Language With {.code = "bn", .val = "Bengali, Bangla"})
            languages.Add(New Language With {.code = "bh", .val = "Bihari"})
            languages.Add(New Language With {.code = "bi", .val = "Bislama"})
            languages.Add(New Language With {.code = "bs", .val = "Bosnian"})
            languages.Add(New Language With {.code = "br", .val = "Breton"})
            languages.Add(New Language With {.code = "bg", .val = "Bulgarian"})
            languages.Add(New Language With {.code = "my", .val = "Burmese"})
            languages.Add(New Language With {.code = "ca", .val = "Catalan"})
            languages.Add(New Language With {.code = "ch", .val = "Chamorro"})
            languages.Add(New Language With {.code = "ce", .val = "Chechen"})
            languages.Add(New Language With {.code = "ny", .val = "Chichewa, Chewa, Nyanja"})
            languages.Add(New Language With {.code = "zh", .val = "Chinese"})
            languages.Add(New Language With {.code = "cv", .val = "Chuvash"})
            languages.Add(New Language With {.code = "kw", .val = "Cornish"})
            languages.Add(New Language With {.code = "co", .val = "Corsican"})
            languages.Add(New Language With {.code = "cr", .val = "Cree"})
            languages.Add(New Language With {.code = "hr", .val = "Croatian"})
            languages.Add(New Language With {.code = "cs", .val = "Czech"})
            languages.Add(New Language With {.code = "da", .val = "Danish"})
            languages.Add(New Language With {.code = "dv", .val = "Divehi, Dhivehi, Maldivian"})
            languages.Add(New Language With {.code = "nl", .val = "Dutch"})
            languages.Add(New Language With {.code = "dz", .val = "Dzongkha"})
            languages.Add(New Language With {.code = "en", .val = "English"})
            languages.Add(New Language With {.code = "eo", .val = "Esperanto"})
            languages.Add(New Language With {.code = "et", .val = "Estonian"})
            languages.Add(New Language With {.code = "ee", .val = "Ewe"})
            languages.Add(New Language With {.code = "fo", .val = "Faroese"})
            languages.Add(New Language With {.code = "fj", .val = "Fijian"})
            languages.Add(New Language With {.code = "fi", .val = "Finnish"})
            languages.Add(New Language With {.code = "fr", .val = "French"})
            languages.Add(New Language With {.code = "ff", .val = "Fula, Fulah, Pulaar, Pular"})
            languages.Add(New Language With {.code = "gl", .val = "Galician"})
            languages.Add(New Language With {.code = "ka", .val = "Georgian"})
            languages.Add(New Language With {.code = "de", .val = "German"})
            languages.Add(New Language With {.code = "el", .val = "Greek (modern)"})
            languages.Add(New Language With {.code = "gn", .val = "Guaraní"})
            languages.Add(New Language With {.code = "gu", .val = "Gujarati"})
            languages.Add(New Language With {.code = "ht", .val = "Haitian, Haitian Creole"})
            languages.Add(New Language With {.code = "ha", .val = "Hausa"})
            languages.Add(New Language With {.code = "he", .val = "Hebrew (modern)"})
            languages.Add(New Language With {.code = "hz", .val = "Herero"})
            languages.Add(New Language With {.code = "hi", .val = "Hindi"})
            languages.Add(New Language With {.code = "ho", .val = "Hiri Motu"})
            languages.Add(New Language With {.code = "hu", .val = "Hungarian"})
            languages.Add(New Language With {.code = "ia", .val = "Interlingua"})
            languages.Add(New Language With {.code = "id", .val = "Indonesian"})
            languages.Add(New Language With {.code = "ie", .val = "Interlingue"})
            languages.Add(New Language With {.code = "ga", .val = "Irish"})
            languages.Add(New Language With {.code = "ig", .val = "Igbo"})
            languages.Add(New Language With {.code = "ik", .val = "Inupiaq"})
            languages.Add(New Language With {.code = "io", .val = "Ido"})
            languages.Add(New Language With {.code = "is", .val = "Icelandic"})
            languages.Add(New Language With {.code = "it", .val = "Italian"})
            languages.Add(New Language With {.code = "iu", .val = "Inuktitut"})
            languages.Add(New Language With {.code = "ja", .val = "Japanese"})
            languages.Add(New Language With {.code = "jv", .val = "Javanese"})
            languages.Add(New Language With {.code = "kl", .val = "Kalaallisut, Greenlandic"})
            languages.Add(New Language With {.code = "kn", .val = "Kannada"})
            languages.Add(New Language With {.code = "kr", .val = "Kanuri"})
            languages.Add(New Language With {.code = "ks", .val = "Kashmiri"})
            languages.Add(New Language With {.code = "kk", .val = "Kazakh"})
            languages.Add(New Language With {.code = "km", .val = "Khmer"})
            languages.Add(New Language With {.code = "ki", .val = "Kikuyu, Gikuyu"})
            languages.Add(New Language With {.code = "rw", .val = "Kinyarwanda"})
            languages.Add(New Language With {.code = "ky", .val = "Kyrgyz"})
            languages.Add(New Language With {.code = "kv", .val = "Komi"})
            languages.Add(New Language With {.code = "kg", .val = "Kongo"})
            languages.Add(New Language With {.code = "ko", .val = "Korean"})
            languages.Add(New Language With {.code = "ku", .val = "Kurdish"})
            languages.Add(New Language With {.code = "kj", .val = "Kwanyama, Kuanyama"})
            languages.Add(New Language With {.code = "la", .val = "Latin"})
            languages.Add(New Language With {.code = "lb", .val = "Luxembourgish, Letzeburgesch"})
            languages.Add(New Language With {.code = "lg", .val = "Ganda"})
            languages.Add(New Language With {.code = "li", .val = "Limburgish, Limburgan, Limburger"})
            languages.Add(New Language With {.code = "ln", .val = "Lingala"})
            languages.Add(New Language With {.code = "lo", .val = "Lao"})
            languages.Add(New Language With {.code = "lt", .val = "Lithuanian"})
            languages.Add(New Language With {.code = "lu", .val = "Luba-Katanga"})
            languages.Add(New Language With {.code = "lv", .val = "Latvian"})
            languages.Add(New Language With {.code = "gv", .val = "Manx"})
            languages.Add(New Language With {.code = "mk", .val = "Macedonian"})
            languages.Add(New Language With {.code = "mg", .val = "Malagasy"})
            languages.Add(New Language With {.code = "ms", .val = "Malay"})
            languages.Add(New Language With {.code = "ml", .val = "Malayalam"})
            languages.Add(New Language With {.code = "mt", .val = "Maltese"})
            languages.Add(New Language With {.code = "mi", .val = "Māori"})
            languages.Add(New Language With {.code = "mr", .val = "Marathi (Marāṭhī)"})
            languages.Add(New Language With {.code = "mh", .val = "Marshallese"})
            languages.Add(New Language With {.code = "mn", .val = "Mongolian"})
            languages.Add(New Language With {.code = "na", .val = "Nauru"})
            languages.Add(New Language With {.code = "nv", .val = "Navajo, Navaho"})
            languages.Add(New Language With {.code = "nd", .val = "Northern Ndebele"})
            languages.Add(New Language With {.code = "ne", .val = "Nepali"})
            languages.Add(New Language With {.code = "ng", .val = "Ndonga"})
            languages.Add(New Language With {.code = "nb", .val = "Norwegian Bokmål"})
            languages.Add(New Language With {.code = "nn", .val = "Norwegian Nynorsk"})
            languages.Add(New Language With {.code = "no", .val = "Norwegian"})
            languages.Add(New Language With {.code = "ii", .val = "Nuosu"})
            languages.Add(New Language With {.code = "nr", .val = "Southern Ndebele"})
            languages.Add(New Language With {.code = "oc", .val = "Occitan"})
            languages.Add(New Language With {.code = "oj", .val = "Ojibwe, Ojibwa"})
            languages.Add(New Language With {.code = "cu", .val = "Old Church Slavonic, Church Slavonic, Old Bulgarian"})
            languages.Add(New Language With {.code = "om", .val = "Oromo"})
            languages.Add(New Language With {.code = "or", .val = "Oriya"})
            languages.Add(New Language With {.code = "os", .val = "Ossetian, Ossetic"})
            languages.Add(New Language With {.code = "pa", .val = "Panjabi, Punjabi"})
            languages.Add(New Language With {.code = "pi", .val = "Pāli"})
            languages.Add(New Language With {.code = "fa", .val = "Persian (Farsi)"})
            languages.Add(New Language With {.code = "pl", .val = "Polish"})
            languages.Add(New Language With {.code = "ps", .val = "Pashto, Pushto"})
            languages.Add(New Language With {.code = "pt", .val = "Portuguese"})
            languages.Add(New Language With {.code = "qu", .val = "Quechua"})
            languages.Add(New Language With {.code = "rm", .val = "Romansh"})
            languages.Add(New Language With {.code = "rn", .val = "Kirundi"})
            languages.Add(New Language With {.code = "ro", .val = "Romanian"})
            languages.Add(New Language With {.code = "ru", .val = "Russian"})
            languages.Add(New Language With {.code = "sa", .val = "Sanskrit (Saṁskṛta)"})
            languages.Add(New Language With {.code = "sc", .val = "Sardinian"})
            languages.Add(New Language With {.code = "sd", .val = "Sindhi"})
            languages.Add(New Language With {.code = "se", .val = "Northern Sami"})
            languages.Add(New Language With {.code = "sm", .val = "Samoan"})
            languages.Add(New Language With {.code = "sg", .val = "Sango"})
            languages.Add(New Language With {.code = "sr", .val = "Serbian"})
            languages.Add(New Language With {.code = "gd", .val = "Scottish Gaelic, Gaelic"})
            languages.Add(New Language With {.code = "sn", .val = "Shona"})
            languages.Add(New Language With {.code = "si", .val = "Sinhala, Sinhalese"})
            languages.Add(New Language With {.code = "sk", .val = "Slovak"})
            languages.Add(New Language With {.code = "sl", .val = "Slovene"})
            languages.Add(New Language With {.code = "so", .val = "Somali"})
            languages.Add(New Language With {.code = "st", .val = "Southern Sotho"})
            languages.Add(New Language With {.code = "es", .val = "Spanish"})
            languages.Add(New Language With {.code = "su", .val = "Sundanese"})
            languages.Add(New Language With {.code = "sw", .val = "Swahili"})
            languages.Add(New Language With {.code = "ss", .val = "Swati"})
            languages.Add(New Language With {.code = "sv", .val = "Swedish"})
            languages.Add(New Language With {.code = "ta", .val = "Tamil"})
            languages.Add(New Language With {.code = "te", .val = "Telugu"})
            languages.Add(New Language With {.code = "tg", .val = "Tajik"})
            languages.Add(New Language With {.code = "th", .val = "Thai"})
            languages.Add(New Language With {.code = "ti", .val = "Tigrinya"})
            languages.Add(New Language With {.code = "bo", .val = "Tibetan Standard, Tibetan, Central"})
            languages.Add(New Language With {.code = "tk", .val = "Turkmen"})
            languages.Add(New Language With {.code = "tl", .val = "Tagalog"})
            languages.Add(New Language With {.code = "tn", .val = "Tswana"})
            languages.Add(New Language With {.code = "to", .val = "Tonga (Tonga Islands)"})
            languages.Add(New Language With {.code = "tr", .val = "Turkish"})
            languages.Add(New Language With {.code = "ts", .val = "Tsonga"})
            languages.Add(New Language With {.code = "tt", .val = "Tatar"})
            languages.Add(New Language With {.code = "tw", .val = "Twi"})
            languages.Add(New Language With {.code = "ty", .val = "Tahitian"})
            languages.Add(New Language With {.code = "ug", .val = "Uyghur"})
            languages.Add(New Language With {.code = "uk", .val = "Ukrainian"})
            languages.Add(New Language With {.code = "ur", .val = "Urdu"})
            languages.Add(New Language With {.code = "uz", .val = "Uzbek"})
            languages.Add(New Language With {.code = "ve", .val = "Venda"})
            languages.Add(New Language With {.code = "vi", .val = "Vietnamese"})
            languages.Add(New Language With {.code = "vo", .val = "Volapük"})
            languages.Add(New Language With {.code = "wa", .val = "Walloon"})
            languages.Add(New Language With {.code = "cy", .val = "Welsh"})
            languages.Add(New Language With {.code = "wo", .val = "Wolof"})
            languages.Add(New Language With {.code = "fy", .val = "Western Frisian"})
            languages.Add(New Language With {.code = "xh", .val = "Xhosa"})
            languages.Add(New Language With {.code = "yi", .val = "Yiddish"})
            languages.Add(New Language With {.code = "yo", .val = "Yoruba"})
            languages.Add(New Language With {.code = "za", .val = "Zhuang, Chuang"})
            languages.Add(New Language With {.code = "zu", .val = "Zulu"})
            languages.OrderBy(Function(x) x.val)
            languages.Insert(0, New Language With {.code = "", .val = "Use Phone Language"})

        End Sub
    End Class




    Public Class StatusUpdateViewModel
        Inherits ViewModelBase

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property

        Public Overloads Property UpdateText As String
            Get
                Return _UpdateText
            End Get
            Set(value As String)
                _UpdateText = value
                RaisePropertyChanged("UpdateText")

            End Set
        End Property
        Private Property _UpdateText As String

        'Public Property NewMessage As String
        '    Get
        '        Return _NewMessage
        '    End Get
        '    Set(value As String)
        '        _NewMessage = value
        '        ' NotifyPropertyChanged()
        '    End Set
        'End Property
        'Private Property _NewMessage As String

        Public Property IsBusy As Boolean
            Get
                Return _IsBusy
            End Get
            Set(value As Boolean)
                _IsBusy = value
                RaisePropertyChanged("IsBusy")
            End Set
        End Property
        Private Property _IsBusy As Boolean

        'Public Property ConnectionColor As String
        '    Get
        '        If isConnected Then Return "Green" Else Return "Red"
        '    End Get
        '    Set(value As String)
        '        '_ConnectionColor = value
        '        RaisePropertyChanged("ConnectionColor")
        '    End Set
        'End Property
        'Private Property _ConnectionColor As String

        'Public Property isConnected As Boolean
        '    Get
        '        Return _isConnected
        '    End Get
        '    Set(value As Boolean)
        '        _isConnected = value
        '        RaisePropertyChanged("UpdateText")
        '        'RaisePropertyChanged("ConnectionColor")
        '        RaisePropertyChanged("isConnected")
        '    End Set
        'End Property
        Private Property _isConnected As Boolean

        'Public Property ConnectedRotation As Integer
        '    Get
        '        Return _ConnectedRotation
        '    End Get
        '    Set(value As Integer)
        '        If _ConnectedRotation <= 170 Then _ConnectedRotation = _ConnectedRotation + 10 Else _ConnectedRotation = 0
        '        RaisePropertyChanged("ConnectedRotation")
        '    End Set
        'End Property
        'Private Property _ConnectedRotation As Integer





        Public Async Function Update(text As String, animated As Boolean, timoutInSeconds As Integer, Optional areWeBusy As Boolean = False, Optional areWeConnected As Boolean = True) As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             UpdateText = text
                                                                                                             vm.isConnected = areWeConnected
                                                                                                             IsBusy = areWeBusy
                                                                                                         End Sub)

        End Function

        Public Async Function Clean() As Task
            'Dim app As App = CType(Application.Current, App)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             UpdateText = vm.appSettings.TVHVersionLong
                                                                                                             IsBusy = False
                                                                                                         End Sub)

        End Function

        Public Sub New()
            ' vm.isConnected = False
        End Sub
    End Class

    Public Class StreamListViewModel
        Inherits ViewModelBase

        Public Sub New()
            items = New ObservableCollection(Of StreamViewModel)
        End Sub
        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property
        Private Property _items As ObservableCollection(Of StreamViewModel)
        Public Property items As ObservableCollection(Of StreamViewModel)
            Get
                Return _items
            End Get
            Set(value As ObservableCollection(Of StreamViewModel))
                _items = value
                RaisePropertyChanged("items")
            End Set
        End Property

        Public Async Function Reload() As Task
            If vm.hasAdminAccess Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                 If Not items Is Nothing Then
                                                                                                                     items.Clear()
                                                                                                                     items = (Await LoadStreams()).ToObservableCollection()
                                                                                                                 End If

                                                                                                             End Sub)
            End If
        End Function

        Public Async Function Update(input_message As CometMessages.input_status) As Task
            Dim currentInput = items.Where(Function(y) y.identifier = input_message.uuid).FirstOrDefault()
            If Not currentInput Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 currentInput.Update(input_message)
                                                                                                             End Sub)

            Else
                Await Me.Reload()
            End If
        End Function

    End Class



    Public Class StreamViewModel
        Inherits ViewModelBase

        'Implements INotifyPropertyChanged
        'Public Function ShallowCopy() As AutoRecordingViewModel
        '    Return DirectCast(Me.MemberwiseClone(), AutoRecordingViewModel)
        'End Function


        'Public Event PropertyChanged As PropertyChangedEventHandler _
        '        Implements INotifyPropertyChanged.PropertyChanged
        'Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        'End Sub

        Public Property identifier As String
        Public Property bps As Integer
            Get
                Return _bps
            End Get
            Set(value As Integer)
                _bps = value
                RaisePropertyChanged("bps")
                RaisePropertyChanged("bandwidth_string")
            End Set
        End Property
        Private Property _bps As Integer

        Public ReadOnly Property bandwidth_string As String
            Get
                Return String.Format("{0} Mbps", Math.Round(bps / 1024 / 1024, 2))
            End Get
        End Property



        Public Property cc As Integer
            Get
                Return _cc
            End Get
            Set(value As Integer)
                _cc = value
                RaisePropertyChanged("cc")
                RaisePropertyChanged("errors_string")
            End Set
        End Property
        Private Property _cc As Integer

        Public Property te As Integer
            Get
                Return _te
            End Get
            Set(value As Integer)
                _te = value
                RaisePropertyChanged("te")
                RaisePropertyChanged("errors_string")
            End Set
        End Property
        Private Property _te As Integer
        Public Property weight As Integer
            Get
                Return _weight
            End Get
            Set(value As Integer)
                _weight = value
                RaisePropertyChanged("weight")
            End Set
        End Property
        Private Property _weight As Integer

        Public ReadOnly Property errors_string As String
            Get
                Return String.Format("{0} TE / {1} CE", te.ToString, cc.ToString)
            End Get

        End Property
        Public Property subs As Integer
        Public Property snr As Integer
            Get
                Return _snr
            End Get
            Set(value As Integer)
                _snr_string = value
                RaisePropertyChanged("snr")
                RaisePropertyChanged("snr_percentage")
            End Set
        End Property
        Private Property _snr As Integer

        Public Property snr_scale As Integer
        Public ReadOnly Property snr_string As String
            Get
                If snr_scale = 1 Then
                    Return (String.Format("{0}%", Math.Round(snr / 65535).ToString))
                Else
                    Return "NA"
                End If
            End Get
        End Property
        Public ReadOnly Property snr_percentage As Double
            Get
                If snr_scale = 1 Then
                    Return snr / 65535
                Else
                    Return 0
                End If
            End Get
        End Property
        Private Property _snr_string As String

        Public Property signal As Integer
            Get
                Return _signal
            End Get
            Set(value As Integer)
                _signal = value
                RaisePropertyChanged("signal")
                RaisePropertyChanged("signal_percentage")
            End Set
        End Property
        Private Property _signal As Integer

        Public Property signal_scale As Integer
        Public ReadOnly Property signal_string As String
            Get
                If signal_scale = 1 Then
                    Return (String.Format("{0}%", Math.Round(snr / 65535) * 100.ToString))
                Else
                    Return "NA"
                End If
            End Get
        End Property
        Public ReadOnly Property signal_percentage As Double
            Get
                If signal_scale = 1 Then
                    Return signal / 65535
                Else
                    Return 0
                End If
            End Get
        End Property


        Public Property name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                RaisePropertyChanged("name")
            End Set
        End Property
        Private Property _name As String


        Public Property currentMux As String
            Get
                Return _currentMux
            End Get
            Set(value As String)
                _currentMux = value
                RaisePropertyChanged("currentMux")
            End Set
        End Property
        Private Property _currentMux As String


        Public Sub New(a As tvh40.Adapter)
            name = a.input
            snr = a.snr
            snr_scale = a.snr_scale
            subs = a.subs
            cc = a.cc
            te = a.te
            bps = a.bps
            signal = a.signal
            signal_scale = a.signal_scale
            currentMux = a.stream
            identifier = a.uuid
            weight = a.weight
        End Sub

        Public Sub New(json As JsonObject)
            bps = json.GetNamedNumber("bps")
            identifier = json.GetNamedString("uuid")
            snr = json.GetNamedNumber("snr")
            snr_scale = json.GetNamedNumber("snr_scale")
            subs = json.GetNamedNumber("subs")
            name = json.GetNamedString("input")
            cc = json.GetNamedNumber("cc")
            te = json.GetNamedNumber("te")
            signal = json.GetNamedNumber("signal")
            signal_scale = json.GetNamedNumber("signal_scale")
            currentMux = json.GetNamedString("stream")
            weight = json.GetNamedNumber("weight")
        End Sub

        Public Sub Update(a As CometMessages.input_status)
            If Me.identifier <> a.uuid Then
                WriteToDebug("WARNING WARNING", "WARNING WARNING")
            End If
            Me.snr = a.snr
            Me.cc = a.cc
            Me.te = a.te
            Me.signal = a.signal
            Me.bps = a.bps
            Me.currentMux = a.stream
            Me.weight = a.weight
        End Sub
    End Class


    Public Class SubscriptionListViewModel
        Implements INotifyPropertyChanged

        Public Sub New()
            items = New ObservableCollection(Of SubscriptionViewModel)

        End Sub
        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private Property _items As ObservableCollection(Of SubscriptionViewModel)
        Public Property items As ObservableCollection(Of SubscriptionViewModel)
            Get
                Return _items
            End Get
            Set(value As ObservableCollection(Of SubscriptionViewModel))
                _items = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Async Function Reload() As Task
            If vm.hasAdminAccess Then
                If Not items Is Nothing Then
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                     items.Clear()
                                                                                                                     items = (Await LoadSubscriptions()).ToObservableCollection()
                                                                                                                 End Sub)
                End If
            End If
        End Function

        Public Async Function Update(subscription_message As CometMessages.subscription) As Task
            Dim x = items.Where(Function(y) y.id = subscription_message.id).FirstOrDefault()
            If Not x Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 x.Update(subscription_message)
                                                                                                             End Sub)
            Else
                Await Me.Reload()
            End If
        End Function

    End Class



    Public Class SubscriptionViewModel
        Inherits ViewModelBase

        Public Property id As Integer
        Public Property start As Integer
        Public Property descramble As String
            Get
                Return _descramble
            End Get
            Set(value As String)
                _descramble = value
                RaisePropertyChanged("descramble")
                RaisePropertyChanged("descrambleVisibility")
            End Set
        End Property
        Private Property _descramble As String
        Private Property _errors As Integer
        Public Property errors As Integer
            Get
                Return _errors
            End Get
            Set(value As Integer)
                _errors = value
                RaisePropertyChanged("errors")
            End Set
        End Property

        Public Property state As String
            Get
                Return _state
            End Get
            Set(value As String)
                _state = value
                RaisePropertyChanged("state")
            End Set
        End Property
        Private Property _state As String
        Public ReadOnly Property hostname_usernameVisibility As String
            Get
                If hostname = "" And username = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property descrambleVisibility As String
            Get
                If descramble = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property

        Public ReadOnly Property hostnameVisibility As String
            Get
                If hostname = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property usernameVisibility As String
            Get
                If username = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property


        Public Property hostname As String
        Public Property username As String
        Public Property title As String
            Get
                Return _title
            End Get
            Set(value As String)
                _title = value
                RaisePropertyChanged("title")
            End Set
        End Property
        Private Property _title As String
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

        Public Property service As String
            Get
                Return _service
            End Get
            Set(value As String)
                _service = value
                RaisePropertyChanged("service")
            End Set
        End Property
        Private Property _service As String

        Public Property starttime As DateTime


        Public Property kbps_in As Integer
            Get
                Return _kbps_in
            End Get
            Set(value As Integer)
                _kbps_in = value
                RaisePropertyChanged("kbps_in")
                RaisePropertyChanged("kbps_in_string")
            End Set
        End Property
        Private Property _kbps_in As Integer

        Public Property kbps_out As Integer
            Get
                Return _kbps_out
            End Get
            Set(value As Integer)
                _kbps_out = value
                RaisePropertyChanged("kbps_out")
                RaisePropertyChanged("kbps_out_string")
            End Set
        End Property
        Private Property _kbps_out As Integer

        Public ReadOnly Property kbps_in_string As String
            Get
                Return String.Format("{0} kb/s", Math.Round(kbps_in / 100))
            End Get
        End Property

        Public ReadOnly Property kbps_out_string As String
            Get
                Return String.Format("{0} kb/s", Math.Round(kbps_out / 100))
            End Get
        End Property




        Public Sub New(s As tvh40.Subscription)
            id = s.id
            state = s.state
            hostname = s.hostname
            username = s.username
            title = s.title
            channel = s.channel
            service = s.service
            errors = s.errors
            start = s.start
            starttime = UnixToDateTime(s.start)
            descramble = s.descramble
        End Sub

        Public Sub New(json As JsonObject)
            Dim v As IJsonValue
            id = If(json.TryGetValue("id", v), json.GetNamedNumber("id"), 0)
            state = If(json.TryGetValue("state", v), json.GetNamedString("state"), "")
            channel = If(json.TryGetValue("channel", v), json.GetNamedString("channel"), "")
            hostname = If(json.TryGetValue("hostname", v), json.GetNamedString("hostname"), "")
            username = If(json.TryGetValue("username", v), json.GetNamedString("username"), "")
            title = If(json.TryGetValue("title", v), json.GetNamedString("title"), "")
            service = If(json.TryGetValue("service", v), json.GetNamedString("service"), "")
            errors = If(json.TryGetValue("errors", v), json.GetNamedNumber("errors"), 0)
            kbps_in = If(json.TryGetValue("in", v), json.GetNamedNumber("in"), 0)
            kbps_out = If(json.TryGetValue("out", v), json.GetNamedNumber("out"), 0)
            start = If(json.TryGetValue("start", v), json.GetNamedNumber("start"), 0)
            descramble = If(json.TryGetValue("descramble", v), json.GetNamedString("descramble"), "")

            'starttime = If(json.TryGetValue("start", v), UnixToDateTime(json.GetNamedNumber("start")), "")
        End Sub

        Public Sub Update(x As CometMessages.subscription)
            If Me.id = x.id Then
                Me.start = x.start
                Me.state = x.state
                Me.title = x.title
                Me.errors = x.errors
                Me.kbps_in = x.in
                Me.kbps_out = x.out
                Me.channel = x.channel
                Me.starttime = UnixToDateTime(start)
                Me.descramble = x.descramble
            End If
        End Sub



    End Class


    Public Class ServerInfoViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Property _sw_version As String
        Public Property sw_version As String
            Get
                Return _sw_version
            End Get
            Set(value As String)
                _sw_version = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _sw_versionlong As String
        Public Property sw_versionlong As String
            Get
                Return _sw_versionlong
            End Get
            Set(value As String)
                _sw_versionlong = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _api_version As Integer
        Public Property api_version As Integer
            Get
                Return _api_version
            End Get
            Set(value As Integer)
                _api_version = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _name As String
        Public Property name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Sub New(ServerInfo As tvh40.ServerInfo)
            name = ServerInfo.name
            sw_version = "3.9"
            api_version = ServerInfo.api_version
            sw_versionlong = ServerInfo.sw_version
        End Sub

        Public Sub New()

        End Sub
    End Class







    Public Class ServiceListViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
        Private Property _items As List(Of ServiceViewModel)
        Public Property items As List(Of ServiceViewModel)
            Get
                Return _items
            End Get
            Set(value As List(Of ServiceViewModel))
                _items = value
                NotifyPropertyChanged()
            End Set
        End Property
    End Class


    Public Class ServiceViewModel


        Implements INotifyPropertyChanged

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
        Public Property auto As Integer
        Public Property caid As String
        Public Property created As String
        Public Property dvb_ignore_eit As Boolean
        Public Property dvb_servicetype As Integer

        Private Property _ExpandedView As String
        Public Property ExpandedView As String
            Get
                Return _ExpandedView
            End Get
            Set(value As String)
                _ExpandedView = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _IsLoadingSteams As Boolean
        Public Property IsLoadingSteams As Boolean
            Get
                Return _IsLoadingSteams
            End Get
            Set(value As Boolean)
                _IsLoadingSteams = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _servicedetails As List(Of ServiceDetailViewModel)
        Public Property servicedetails As List(Of ServiceDetailViewModel)
            Get
                Return _servicedetails
            End Get
            Set(value As List(Of ServiceDetailViewModel))
                _servicedetails = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Property enabled As Boolean
        Public Property encrypted As String
        Public Property force_caid As Integer
        Public Property last_seen As String
        Public Property lcn As Integer
        Public Property lcn_minor As Integer
        Public Property lcn2 As Integer
        Public Property multiplex As String
        Public Property network As String
        Public Property prefcapid As Integer
        Public Property prefcapid_lock As Integer
        Public Property provider As String
        Public Property sid As Integer
        Public Property svcname As String
        Public Property uuid As String

        Public Property ExpandCollapseCommand As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            'Dim app As App = CType(Application.Current, App)
                                            WriteToDebug("ServiceViewModel.ExpanseCollapseCommand", "start")
                                            If Me.ExpandedView = "Expanded" Then
                                                Me.ExpandedView = "Collapsed"
                                            Else
                                                For Each service In vm.Services.items.Where(Function(x) x.ExpandedView = "Expanded")
                                                    service.ExpandedView = "Collapsed"
                                                Next
                                                Me.ExpandedView = "Expanded"
                                            End If
                                            'vm.StatusBar.Update("Loading streams...", True, 0, True, True)
                                            If Me.servicedetails Is Nothing Then
                                                Me.IsLoadingSteams = True
                                                Me.servicedetails = Await LoadServiceDetails(Me)
                                                Me.IsLoadingSteams = False
                                            End If
                                            'vm.StatusBar.Clean()
                                            WriteToDebug("ServiceViewModel.ExpanseCollapseCommand", "stop")
                                        End Sub)
            End Get
            Set(value As RelayCommand)
            End Set
        End Property




        Public Sub New(Service As tvh40.Service)
            uuid = Service.uuid
            enabled = Service.enabled
            multiplex = Service.multiplex
            lcn = Service.lcn
            sid = Service.sid
            encrypted = Service.encrypted
            svcname = Service.svcname
            network = Service.network
            created = UnixToDateTime(Service.created).ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern)
            ExpandedView = "Normal"
        End Sub

    End Class

    Public Class MuxListViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
        Private Property _items As List(Of MuxViewModel)
        Public Property items As List(Of MuxViewModel)
            Get
                Return _items
            End Get
            Set(value As List(Of MuxViewModel))
                _items = value
                NotifyPropertyChanged()
            End Set
        End Property
    End Class

    Public Class MuxViewModel
        Public Property constellation As String
        Public Property delsys As String
        Public Property enabled As Boolean
        Public Property epg As Integer
        Public Property fec As String
        Public Property frequency As Integer
        Public Property name As String
        Public Property network As String
        Public Property num_chn As Integer
        Public Property num_svc As Integer
        Public Property onid As Integer
        Public Property pmt_06_ac3 As Boolean
        Public Property scan_result As Integer
        Public Property scan_state As Integer
        Public Property symbolrate As Integer
        Public Property tsid As Integer
        Public Property uuid As String

        Public Sub New(Mux As tvh40.Mux)
            name = Mux.name
            uuid = Mux.uuid
            scan_result = Mux.scan_result
            scan_state = Mux.scan_state
            enabled = Mux.enabled
            constellation = Mux.constellation
            epg = Mux.epg
            tsid = Mux.tsid
            symbolrate = Mux.symbolrate
            num_chn = Mux.num_chn
            num_svc = Mux.num_svc
            frequency = Mux.frequency
        End Sub

    End Class

    Public Class ServiceDetailViewModel
        Public Property index As Integer
        Public Property pid As String
        Public Property type As String
        Public Property details As String



        Public Sub New(sd As tvh40.ServiceDetail)
            index = sd.index
            pid = sd.pid
            type = sd.type
            If sd.type = "CA" Then
                details = sd.details
            Else
                details = sd.language
            End If
            '            language = sd.language
            'details = sd.details
        End Sub
    End Class





    'Public Class ApplicationViewModel
    '    Implements INotifyPropertyChanged

    '    Public Event PropertyChanged As PropertyChangedEventHandler _
    '        Implements INotifyPropertyChanged.PropertyChanged

    '    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
    '        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    '    End Sub

    '    Public Property currentView As String
    '        'PROPERTY THAT REPRESENTS WHICH VIEW IS CURRENTLY SELECTED. POSSIBLE OPTIONS ARE
    '        ' EPG : THE EPG PIVOT IS SHOWN
    '        ' RECORDINGS : THE RECORDINGS PIVOT IS SHOWN
    '        ' SEARCH : THE SEARCH PIVOT IS SHOWN
    '        ' STATUS : THE STATUS PIVOT IS SHOWN
    '        Get
    '            Return _currentView
    '        End Get
    '        Set(value As String)
    '            _previousView = _currentView
    '            _currentView = value
    '            NotifyPropertyChanged()
    '        End Set
    '    End Property
    '    Private Property _currentView As String

    '    Public Property previousView As String
    '        'PROPERTY THAT REPRESENTS WHICH VIEW WAS PREVIOUSLY SELECTED. PROVIDES A "GO BACK FUNCTIONALITY FOR THOSE MOMENTS WHERE YOU NEED IT :)
    '        Get
    '            Return _previousView
    '        End Get
    '        Set(value As String)
    '            _previousView = value
    '            NotifyPropertyChanged()
    '        End Set
    '    End Property
    '    Private Property _previousView As String

    '    Public Sub New()
    '        currentView = "epg"
    '    End Sub
    'End Class


    '    Public Class RecordingViewModel
    '        Implements INotifyPropertyChanged

    '        Public Event PropertyChanged As PropertyChangedEventHandler _
    '                Implements INotifyPropertyChanged.PropertyChanged

    '        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
    '            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    '        End Sub

    '        Public settings As New AppSettings
    '        Public loader As New Windows.ApplicationModel.Resources.ResourceLoader()
    '        Public app As App = CType(Application.Current, App)

    '        Public Property ExpandCollapseCommand As RelayCommand
    '            Get
    '                Return New RelayCommand(Sub()
    '                                            Dim rlist As ObservableCollection(Of Group(Of RecordingViewModel))
    '                                            Select Case vm.PivotSelectedIndex
    '                                                Case 2
    '                                                    rlist = vm.UpcomingRecordings.groupeditems
    '                                                Case 3
    '                                                    rlist = vm.FinishedRecordings.groupeditems
    '                                                Case 4
    '                                                    rlist = vm.FailedRecordings.groupeditems
    '                                            End Select
    '                                            For Each group In rlist
    '                                                For Each epgitem In group
    '                                                    If epgitem Is Me Then
    '                                                        If Me.ExpandedView = "Collapsed" Then Me.ExpandedView = "Visible" Else Me.ExpandedView = "Collapsed"
    '                                                    Else
    '                                                        epgitem.ExpandedView = "Collapsed"
    '                                                    End If
    '                                                Next
    '                                            Next
    '                                            Debug.WriteLine("RecordingViewlModel - ExpandCollapse Executed")
    '                                        End Sub)
    '            End Get
    '            Set(value As RelayCommand)
    '            End Set
    '        End Property

    '        Public Property UpdateSelectedChannel As RelayCommand
    '            Get
    '                Return New RelayCommand(Sub(x)
    '                                            If TypeOf x Is ChannelViewModel Then
    '                                                Dim selectedChannel As ChannelViewModel = CType(x, ChannelViewModel)
    '                                                Me.channelUuid = selectedChannel.channelUuid
    '                                                Me.channel = selectedChannel.name
    '                                                ChannelSelectionFlyOutIsOpen = False
    '                                                Debug.WriteLine("RecordingViewModel - UpdateSelectedChannel Executed")
    '                                            End If
    '                                        End Sub)
    '            End Get
    '            Set(value As RelayCommand)
    '            End Set
    '        End Property

    '        Public Property UpdateSelectedDVRConfig As RelayCommand
    '            Get
    '                Return New RelayCommand(Sub(x)
    '                                            If TypeOf x Is DVRConfigViewModel Then
    '                                                Dim selectedDVRConfig As DVRConfigViewModel = CType(x, DVRConfigViewModel)
    '                                                Me.configName = selectedDVRConfig.name
    '                                                Me.configUuid = selectedDVRConfig.identifier
    '                                                DVRConfigSelectionFlyOutIsOpen = False
    '                                                Debug.WriteLine("RecordingViewModel - UpdateSelectedDVRConfig Executed")
    '                                            End If
    '                                        End Sub)
    '            End Get
    '            Set(value As RelayCommand)
    '            End Set
    '        End Property

    '        Public Property SaveRecording As RelayCommand
    '            Get
    '                Return New RelayCommand(Async Sub()
    '                                            Dim app As App = CType(Application.Current, App)
    '                                            ''Collect the channel Icon URI if a specific channel was selected
    '                                            'If Not vm.AllChannels.Count = 0 Then
    '                                            '    If Not Me.channel = "" Then
    '                                            '        Dim channel As ChannelViewModel = (From c In vm.AllChannels Where c.channelUuid = Me.channel Select c).FirstOrDefault
    '                                            '        If Not channel Is Nothing Then
    '                                            '            Me.chicon = channel.chicon
    '                                            '        End If
    '                                            '    End If
    '                                            'End If
    '                                            Debug.WriteLine("RecordingViewmodel - SaveRecording Executed")

    '                                            Dim r As tvhCommandResponse
    '                                            If Me.recording_id = "" Then
    '                                                r = Await AddManualRecording(Me)
    '                                            Else
    '                                                r = Await UpdateManualRecording(Me)
    '                                            End If

    '                                            If r.success = 1 Then
    '                                                vm.StatusBar.Update("Recording saved", True, 8, False, True)
    '                                                Debug.WriteLine("Recording Saved")
    '                                            Else
    '                                                Debug.WriteLine("Recording Failed")
    '                                                vm.StatusBar.Update("Error saving recording", True, 8, False, True)
    '                                            End If
    '                                            Await vm.UpcomingRecordings.RefreshUpcomingRecordings(True)
    '                                            'Dim currentAutorecording = (From c In vm.AutoRecordings.items Where c.id = Me.id Select c).FirstOrDefault
    '                                            'If Not currentAutorecording Is Nothing Then
    '                                            '    currentAutorecording.title = Me.title
    '                                            '    currentAutorecording.name = Me.name
    '                                            '    'currentAutorecording = Me
    '                                            'Else
    '                                            '    vm.AutoRecordings.items.Add(Me)
    '                                            'End If
    '                                            Dim content = Window.Current.Content
    '                                            Dim frame = CType(content, Frame)
    '                                            If Not frame Is Nothing Then
    '                                                frame.Navigate(GetType(HubPage))
    '                                            End If
    '                                            Window.Current.Activate()

    '                                        End Sub)

    '            End Get
    '            Set(value As RelayCommand)
    '            End Set
    '        End Property

    '        Public Property CancelRecordingEditing As RelayCommand
    '            Get
    '                Return New RelayCommand(Sub()
    '                                            Dim content = Window.Current.Content
    '                                            Dim frame = CType(content, Frame)
    '                                            If Not frame Is Nothing Then
    '                                                frame.Navigate(GetType(HubPage))
    '                                            End If
    '                                            Window.Current.Activate()

    '                                        End Sub)

    '            End Get
    '            Set(value As RelayCommand)
    '            End Set
    '        End Property

    '        Private Property _ChannelSelectionFlyOutIsOpen As Boolean
    '        Public Property ChannelSelectionFlyOutIsOpen As Boolean
    '            Get
    '                Return _ChannelSelectionFlyOutIsOpen
    '            End Get
    '            Set(value As Boolean)
    '                _ChannelSelectionFlyOutIsOpen = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property

    '        Private Property _DVRConfigSelectionFlyOutIsOpen As Boolean
    '        Public Property DVRConfigSelectionFlyOutIsOpen As Boolean
    '            Get
    '                Return _DVRConfigSelectionFlyOutIsOpen
    '            End Get
    '            Set(value As Boolean)
    '                _DVRConfigSelectionFlyOutIsOpen = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property


    '        Public Property broadcast As String

    '        Public Property channel As String
    '            Get
    '                Return _channel
    '            End Get
    '            Set(value As String)
    '                _channel = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _channel As String
    '        Public Property channelUuid As String

    '        Public Property chicon As String
    '            Get
    '                Dim app As App = CType(Application.Current, App)
    '                Dim b = (From a In vm.AllChannels Where a.channelUuid = Me.channelUuid Select a).FirstOrDefault
    '                If Not b Is Nothing Then
    '                    Return b.chicon
    '                End If
    '            End Get
    '            Set(value As String)
    '                _chicon = "blaat"
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _chicon As String
    '        Public Property configName As String
    '            Get
    '                Return _configName
    '            End Get
    '            Set(value As String)
    '                _configName = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _configName As String
    '        Public Property configUuid As String
    '        Public Property title As String
    '        Public Property description As String
    '        Public Property recording_id As String

    '        Private Property _startDate As DateTime
    '        Public Property startDate As DateTime
    '            Get
    '                Return _startDate
    '            End Get
    '            Set(value As DateTime)
    '                _startDate = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _startTime As DateTime
    '        Public Property startTime As DateTime
    '            Get
    '                Return _startTime
    '            End Get
    '            Set(value As DateTime)
    '                _startTime = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _stopDate As DateTime
    '        Public Property stopDate As DateTime
    '            Get
    '                Return _stopDate
    '            End Get
    '            Set(value As DateTime)
    '                _stopDate = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _stopTime As DateTime
    '        Public Property stopTime As DateTime
    '            Get
    '                Return _stopTime
    '            End Get
    '            Set(value As DateTime)
    '                _stopTime = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property

    '        Public Property duration As Integer
    '        Public Property creator As String
    '        Public Property pri As String
    '        Private Property _IsSelected As Boolean
    '        Public Property IsSelected As Boolean
    '            Get
    '                Return _IsSelected
    '            End Get
    '            Set(value As Boolean)
    '                _IsSelected = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Public Property status As String
    '            Get
    '                Return _status
    '            End Get
    '            Set(value As String)
    '                _status = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _status As String

    '        Public Property schedstate As String
    '            Get
    '                Return _schedstate
    '            End Get
    '            Set(value As String)
    '                _schedstate = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _schedstate As String
    '        Public Property percentcompleted As Double
    '            Get
    '                If (stopDate > Date.MinValue) And (startDate > Date.MinValue) And (Date.Now > startDate) Then
    '                    If Date.Now > stopDate Then
    '                        Return 1
    '                    Else
    '                        Return Math.Round((Date.Now - startDate).TotalSeconds / (stopDate - startDate).TotalSeconds, 2)
    '                    End If
    '                Else
    '                    Return 0
    '                End If
    '            End Get
    '            Set(value As Double)
    '                _percentcompleted = value
    '                NotifyPropertyChanged()

    '            End Set
    '        End Property
    '        Private Property _percentcompleted As Double

    '        Public Property filesize As Long                    ' Used for FInished and Failed Recordings only
    '        Public ReadOnly Property filesizeGB As String
    '            Get
    '                If filesize > 0 Then
    '                    Return Math.Round(filesize / 1024 / 1024 / 1024, 2).ToString + " GB"
    '                Else : Return ""
    '                End If
    '            End Get
    '        End Property
    '        ' Used for FInished and Failed Recordings only
    '        Public Property url As String                       'Used for FInished and Failed Recordings only
    '        Public Property ExpandedView As String
    '            Get
    '                Return _ExpandedView
    '            End Get
    '            Set(value As String)
    '                _ExpandedView = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _ExpandedView As String
    '        Public Property ItemStatus As String
    '            Get
    '                Return _ItemStatus
    '            End Get
    '            Set(value As String)
    '                _ItemStatus = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _ItemStatus As String

    '        Public Property DVRConfigSelectionEnabled As Boolean
    '            Get
    '                Return _DVRConfigSelectionEnabled
    '            End Get
    '            Set(value As Boolean)
    '                _DVRConfigSelectionEnabled = value
    '                NotifyPropertyChanged()
    '            End Set
    '        End Property
    '        Private Property _DVRConfigSelectionEnabled As Boolean

    '        Public Property EndDateVisibility As Visibility ' TVH3.4/3.5/3.6 do not used a End Date for a recording

    '        Public ReadOnly Property progressBarBackgroundBrush As SolidColorBrush
    '            Get
    '                If (CType(Application.Current.Resources("PhoneBackgroundColor"), Color).Equals(Colors.Black)) Then
    '                    Return New SolidColorBrush(Color.FromArgb(255, 33, 33, 33))
    '                Else
    '                    Return New SolidColorBrush(Color.FromArgb(255, 230, 230, 230))
    '                End If

    '            End Get
    '        End Property

    '        Public Async Function Delete() As Task(Of RecordingReturnValue)
    '            'Initiates the deletion of a recording
    '            ' First : Set the Status of the recording to "Remove" in order to trigger the animation"within the user control
    '            Me.ItemStatus = "Remove"

    '            'Next, initiate the deletion of the Recording towards TVH server. Don't do actual deletion if we run in DEBUG
    '            Dim retValue As RecordingReturnValue
    '#If DEBUG Then
    '            retValue = New RecordingReturnValue With {.tvhResponse = New tvhCommandResponse With {.success = 1}}
    '#Else
    '            retValue = Await DeleteAutoRecording(Me.recording_id)
    '#End If
    '            If retValue.tvhResponse.success = 0 Then
    '                'TVH server says the deletion was unsuccesful. Change the status of the item back to "New" in order to re-show it in the list
    '                Me.ItemStatus = "New"
    '            End If
    '            Return retValue
    '        End Function

    '        Public Sub New()
    '            Dim app As App = CType(Application.Current, App)
    '            Dim settings As New AppSettings
    '            If app.TVHVersion = "3.4" Then EndDateVisibility = Visibility.Collapsed Else EndDateVisibility = Visibility.Visible
    '            ExpandedView = "Collapsed"
    '            ItemStatus = "New"
    '            DVRConfigSelectionEnabled = False
    '            startDate = Date.Now
    '            startTime = Date.Now
    '            stopDate = Date.Now
    '            stopTime = Date.Now.AddHours(2)
    '        End Sub

    '        Public Sub New(recording As tvh34.Recording)
    '            ' Create a new Viewmodel based on a 3.4 / 3.5 TVH JSON entry
    '            EndDateVisibility = Visibility.Collapsed
    '            channel = recording.channel
    '            channelUuid = recording.channel
    '            chicon = recording.chicon
    '            configName = recording.config_name
    '            creator = recording.creator
    '            description = recording.description
    '            duration = recording.duration
    '            stopDate = UnixToDateTime(recording.endtime).ToLocalTime
    '            filesize = recording.filesize
    '            pri = recording.pri
    '            recording_id = recording.recording_id
    '            schedstate = recording.schedstate
    '            startDate = UnixToDateTime(recording.starttime).ToLocalTime
    '            status = recording.status
    '            title = recording.title
    '            url = recording.url
    '            ExpandedView = "Collapsed"
    '            ItemStatus = "New"
    '            DVRConfigSelectionEnabled = False
    '            IsSelected = False
    '            'SelectionVisibility = "Collapsed"
    '            'IsSelected = False
    '            'ButtonSelectionVisibility = "Visible"
    '        End Sub

    '        Public Sub New(recording As tvh40.Recording)
    '            ' Create a new Viewmodel based on a 3.9 TVH JSON entry
    '            EndDateVisibility = Visibility.Visible
    '            broadcast = recording.broadcast
    '            channel = recording.channelname
    '            channelUuid = recording.channel
    '            chicon = recording.channel_icon
    '            configName = recording.config_name
    '            creator = recording.creator
    '            description = recording.disp_description
    '            duration = recording.duration
    '            startDate = UnixToDateTime(recording.start).ToLocalTime
    '            stopDate = UnixToDateTime(recording.stop).ToLocalTime
    '            filesize = recording.filesize
    '            pri = recording.pri
    '            recording_id = recording.uuid
    '            schedstate = recording.sched_status
    '            status = recording.status
    '            title = recording.disp_title
    '            url = recording.url

    '            ExpandedView = "Collapsed"
    '            ItemStatus = "New"
    '            DVRConfigSelectionEnabled = False
    '            IsSelected = False
    '            'SelectionVisibility = "Collapsed"
    '            'IsSelected = False
    '            'ButtonSelectionVisibility = "Visible"
    '        End Sub
    '    End Class


    Public Class LogViewModel
        Inherits ViewModelBase

        Public Property entries As New ObservableCollection(Of String)

        Public Async Sub Add(message As String)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             'entries.Add(message)
                                                                                                             'If entries.Count = 50 Then entries.RemoveAt(0)
                                                                                                             entries.Insert(0, message)
                                                                                                             If entries.Count = 50 Then entries.RemoveAt(49)
                                                                                                         End Sub)
        End Sub

        Public Sub New()
        End Sub
    End Class








    'Public Class diskspaceUpdate
    '    Public Property freediskspace As Long
    '    Public Property notificationClass As String
    '    Public Property totaldiskspace As Long
    '    Public Property useddiskspace As Long
    'End Class

    Public Class DVRConfigListViewModel
        Public Property items As New List(Of DVRConfigViewModel)
        Public Property dataLoaded As Boolean

        Public Async Function Load() As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                             items.Clear()
                                                                                                             items = (Await LoadDVRConfigs()).ToList()
                                                                                                             dataLoaded = True
                                                                                                         End Sub)
        End Function
    End Class


    Public Class DVRConfigViewModel
        Public Property name As String
        Public Property identifier As String

        Public Sub New(dvrconfig As tvh40.DVRConfig)
            name = dvrconfig.val
            identifier = dvrconfig.key
        End Sub



        Public Sub New()

        End Sub
    End Class






    Public Class AddRecordingViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
                Implements INotifyPropertyChanged.PropertyChanged
        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property

        Public Property channel As String
            Get
                Return _channel
            End Get
            Set(value As String)
                _channel = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _channel As String

        Public Property channelUuid As String
        Public Property chicon As String
            Get
                If _chicon = "" Then Return "/Images/tvheadend.png" Else Return _chicon
            End Get
            Set(value As String)
                If Not value Is Nothing And Not value = "/Images/tvheadend.png" Then
                    If value.ToUpper().IndexOf("HTTP:/") >= 0 Then _chicon = value Else _chicon = (New AppSettings).GetFullURL() & "/" & value
                Else
                    _chicon = ""
                End If
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _chicon As String
        Public Property config_name As String
        Public Property title As String
        Public Property description As String
        Public Property dvrconfig_name As String
            Get
                Return _dvrconfig_name
            End Get
            Set(value As String)
                _dvrconfig_name = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _dvrconfig_name As String
        Public Property dvrconfig_uuid As String
        Public Property recording_id As String
        Public Property startDate As DateTime
        Public Property endDate As DateTime
        Public Property startTime As DateTime
        Public Property endTime As DateTime
        Public ReadOnly Property startDateTime As DateTime
            Get
                Return startDate.Date.Add(New TimeSpan(startTime.Hour, startTime.Minute, startTime.Second))
            End Get
        End Property

        Public ReadOnly Property endDateTime As DateTime
            Get
                Return endDate.Date.Add(New TimeSpan(endTime.Hour, endTime.Minute, endTime.Second))
            End Get
        End Property


        Public Property duration As Integer
        Public Property creator As String
        Public Property pri As String
        Public Property status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _status As String

        Public Property schedstate As String
            Get
                Return _schedstate
            End Get
            Set(value As String)
                _schedstate = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _schedstate As String
        Public Property DVRConfigVisibility As String




        Public Sub New()
            channel = "Click to select channel..."
            dvrconfig_uuid = ""
            If vm.TVHVersion = "3.4" Then
                DVRConfigVisibility = "Collapsed"
                dvrconfig_name = ""
            End If

            If vm.TVHVersion = "3.9" Then
                dvrconfig_name = "Select DVR Config..."
                DVRConfigVisibility = "Visible"
            End If

            title = "Enter title..."
            startDate = DateTime.Now
            endDate = DateTime.Now
            startTime = DateTime.Now
            endTime = DateTime.Now.AddHours(1)
        End Sub
    End Class








    Public Class ChannelTagListViewModel
        Public Property items As New ObservableCollection(Of ChannelTagViewModel)
        Public Property dataLoaded As Boolean
    End Class


    Public Class ChannelTagViewModel
        Inherits ViewModelBase

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, App).DefaultViewModel
            End Get

        End Property

        Public Property ChannelTagSelected As RelayCommand
            Get
                Return New RelayCommand(Async Sub(x)
                                            WriteToDebug("ChannelTagViewModel.ChannelTagSelected", "start")
                                            While vm.AppBar.ButtonEnabled.refreshButton = False
                                                WriteToDebug("ChanneltagViewModel.ChannelTagSelected()", "Waiting for refresh to finish...")
                                                Await Task.Delay(100)
                                            End While
                                            vm.ChannelSelected = False
                                            vm.SelectedChannel = Nothing
                                            vm.EPGInformationAvailable = False
                                            vm.ChannelTagFlyoutIsOpen = False
                                            vm.selectedChannelTag = x
                                            vm.Channels.dataLoaded = False
                                            Task.Run(Function() vm.LoadDataAsync())
                                            WriteToDebug("ChannelTagViewModel.ChannelTagSelected", "stop")
                                        End Sub)

            End Get
            Set(value As RelayCommand)
            End Set
        End Property

        Public Property comment As String
        Public Property enabled As Boolean
        Public Property icon As String
        Public Property icon_public_url As String
            Get
                If _icon_public_url = "" Then Return "ms-appx:///Images/tvheadend.png" Else Return _icon_public_url
            End Get
            Set(value As String)
                If Not value Is Nothing And Not value = "/Images/tvheadend.png" Then
                    If value.ToUpper().IndexOf("HTTP:/") >= 0 Then _icon_public_url = value Else _icon_public_url = (New AppSettings).GetFullURL() & "/" & value
                Else
                    _icon_public_url = ""
                End If
            End Set
        End Property
        Private Property _icon_public_url As String
        Public Property internal As Boolean
        Public Property name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                RaisePropertyChanged("name")
            End Set
        End Property
        Private Property _name As String
        Public Property numberOfChannels As String
            Get
                Return _numberOfChannels
            End Get
            Set(value As String)
                _numberOfChannels = value
            End Set
        End Property
        Private Property _numberOfChannels As String

        Public Property [private] As Boolean
        Public Property titled_icon As Boolean
        Public Property uuid As String
            Get
                Return _uuid
            End Get
            Set(value As String)
                _uuid = value
                RaisePropertyChanged("uuid")
            End Set
        End Property
        Private Property _uuid As String

        Public Sub New()

        End Sub



        Public Sub New(ChannelTag As tvh40.ChannelTag)
            comment = ChannelTag.comment
            enabled = ChannelTag.enabled
            internal = ChannelTag.internal
            icon = ChannelTag.icon
            name = ChannelTag.name
            uuid = ChannelTag.uuid
        End Sub
    End Class



    Public Class ConnectionViewModel
        Inherits ViewModelBase
        Public Property id As Integer
        Public Property peer As String
        Public Property started As Long
        Public Property type As String
        Public Property user As String

        Public Sub New(c As tvh40.Connection)
            id = c.id
            peer = c.peer
            started = c.started
            type = c.type
            user = c.user

        End Sub
    End Class

    'ViewModel of a TVChannel



    'Public Class FinishedRecordingsViewModel
    '    Implements INotifyPropertyChanged

    '    Public Event PropertyChanged As PropertyChangedEventHandler _
    '    Implements INotifyPropertyChanged.PropertyChanged

    '    ' This method is called by the Set accessor of each property. 
    '    ' The CallerMemberName attribute that is applied to the optional propertyName 
    '    ' parameter causes the property name of the caller to be substituted as an argument. 
    '    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
    '        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    '    End Sub



    'End Class

    Public Class ContentTypeListViewModel
        Public Property items As New List(Of ContentTypeViewModel)
        Public Property dataLoaded As Boolean
    End Class

    Public Class ContentTypeViewModel
        Public Property uuid As Integer
        Public Property name As String

        Public Sub New(c As tvh40.Genre)
            uuid = c.key
            name = c.val
        End Sub
    End Class
End Namespace