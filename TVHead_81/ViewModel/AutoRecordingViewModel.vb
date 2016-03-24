Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports TVHead_81.Common
Imports TVHead_81.ViewModels


Public Class AutoRecordingViewModel
    Inherits ViewModelBase

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
        End Get

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

    Public Property AutoRecordingClickCommand As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("AutoRecordingViewModel.AutoRecordingClickCommand()", "AutoRecordingClickCommand Executed")
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

    Private Property _ChannelTagSelectionFlyOutIsOpen As Boolean
    Public Property ChannelTagSelectionFlyOutIsOpen As Boolean
        Get
            Return _ChannelTagSelectionFlyOutIsOpen
        End Get
        Set(value As Boolean)
            _ChannelTagSelectionFlyOutIsOpen = value
            RaisePropertyChanged("ChannelTagSelectionFlyOutIsOpen")
        End Set
    End Property

    Private Property _GenreSelectionFlyOutIsOpen As Boolean
    Public Property GenreSelectionFlyOutIsOpen As Boolean
        Get
            Return _GenreSelectionFlyOutIsOpen
        End Get
        Set(value As Boolean)
            _GenreSelectionFlyOutIsOpen = value
            RaisePropertyChanged("GenreSelectionFlyOutIsOpen")
        End Set
    End Property

    Public Property UpdateSelectedChannel As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'If TypeOf x Is ChannelViewModel Then
                                        '    Dim selectedChannel As ChannelViewModel = CType(x, ChannelViewModel)
                                        '    Me.channel = selectedChannel.channelUuid
                                        '    Me.channelname = selectedChannel.name
                                        '    ChannelSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("AutoRecordingViewModel.UpdateSelectedChannel()", "UpdateSelectedChannel Executed")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedDVRConfig As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'If TypeOf x Is DVRConfigViewModel Then
                                        '    Dim selectedDVRConfig As DVRConfigViewModel = CType(x, DVRConfigViewModel)
                                        '    'Me.configName = selectedDVRConfig.name
                                        '    Me.configUuid = selectedDVRConfig.identifier
                                        '    Me.configName = selectedDVRConfig.name
                                        '    DVRConfigSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("AutoRecordingViewModel.UpdateSelectedDVRConfig()", "UpdateSelectedDVRConfig Executed")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedChannelTag As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'If TypeOf x Is ChannelTagViewModel Then
                                        '    Dim selectedChannelTag As ChannelTagViewModel = CType(x, ChannelTagViewModel)
                                        '    Me.tagName = selectedChannelTag.name
                                        '    Me.tagUuid = selectedChannelTag.uuid
                                        '    ChannelTagSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("AutoRecordingViewModel.UpdateSelectedChannelTag()", "UpdateSelectedChannelTag Executed")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property UpdateSelectedGenre As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'If TypeOf x Is ContentTypeViewModel Then
                                        '    Dim selectedGenre As ContentTypeViewModel = CType(x, ContentTypeViewModel)
                                        '    Me.contenttypeName = selectedGenre.name
                                        '    Me.contenttype = selectedGenre.uuid
                                        '    GenreSelectionFlyOutIsOpen = False
                                        '    WriteToDebug("AutoRecordingViewModel.UpdateSelectedGenre()", "UpdateSelectedGenre Executed")
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)
        End Set
    End Property


    Public Property SaveAutoRecording As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("AutoRecordingViewModel.SaveAutoRecording()", "start")
                                        'Dim app As App = CType(Application.Current, Application)
                                        'Trigger update of chicon property by setting it to a bogus value (NotifyPropertyChanged will kick in)
                                        Me.chicon = "Updated"
                                        Dim r As tvhCommandResponse
                                        If Me.id = "" Then
                                            r = Await AddAutoRecording(Me)
                                        Else
                                            r = Await UpdateAutoRecording(Me)
                                        End If

                                        If r.success = 1 Then
                                            If Not vm.appSettings.LongPollingEnabled Then
                                                vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                                                .msg = String.Format(vm.loader.GetString("AutoRecordingUpdated"), Me.title)})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Saved")
                                        Else
                                            If Not vm.appSettings.LongPollingEnabled Then
                                                vm.ToastMessages.AddMessage(New ToastMessageViewModel With {.secondsToShow = 3,
                                                                                .msg = String.Format(vm.loader.GetString("AutoRecordingUpdatedError"), Me.title)})

                                            End If
                                            WriteToDebug("TVHead_ViewModel.SaveRecording()", "Recording Failed")
                                        End If

                                        If Not vm.appSettings.LongPollingEnabled Then Await vm.AutoRecordings.Reload()

                                        'Move back to the HubPage after Saving
                                        'NavigationService.GoBack()

                                        Dim content = Window.Current.Content
                                        Dim frame = CType(content, Frame)
                                        If frame.CanGoBack Then frame.GoBack()
                                        'If Not frame Is Nothing Then
                                        '    frame.Navigate(GetType(HubPage))
                                        'End If
                                        'Window.Current.Activate()
                                        WriteToDebug("AutoRecordingViewModel.SaveAutoRecording()", "stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property CancelAutoRecordingEditing As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        WriteToDebug("AutoRecordingViewModel.CancelAutoRecordingEditing()", "start")
                                        'We cancel the editing, reload the autorecordings
                                        Await vm.AutoRecordings.Reload()
                                        Dim content = Window.Current.Content
                                        Dim frame = CType(content, Frame)
                                        If frame.CanGoBack Then frame.GoBack()
                                        WriteToDebug("AutoRecordingViewModel.CancelAutoRecordingEditing()", "stop")

                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property EditAutoRecording As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("AutoRecordingViewModel.EditAutoRecording()", "start")
                                        vm.selectedAutoRecording = Me
                                        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                                        If Not rootFrame.Navigate(GetType(AutoRecordingPage), Me) Then
                                            Dim resources As ResourceLoader = ResourceLoader.GetForCurrentView("Resources")
                                            Throw New Exception(resources.GetString("NavigationFailedExceptionMessage"))
                                        End If
                                        WriteToDebug("AutoRecordingViewModel.EditAutoRecording()", "stop")

                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property



    Public Property id As String
    Public Property _enabled As Boolean
    Public Property enabled As Boolean
        Get
            Return _enabled
        End Get
        Set(value As Boolean)
            _enabled = value
            RaisePropertyChanged("enabled")
            RaisePropertyChanged("Opacity")
        End Set
    End Property

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

    Private Property _Opacity As Double
    Public Property Opacity As Double
        Get
            If enabled Then
                Return 1
            Else
                Return 0.5
            End If
        End Get
        Set(value As Double)
            _Opacity = value
            RaisePropertyChanged("Opacity")
        End Set

    End Property

    Public Property contenttype As Integer
        Get
            Return _contenttype
        End Get
        Set(value As Integer)
            _contenttype = value
            RaisePropertyChanged("contenttype")
            If value = 0 Then GenreSelectionEnabled = False Else GenreSelectionEnabled = True
        End Set
    End Property
    Private Property _contenttype As Integer
    Public Property contenttypeName As String
        Get
            Return _contenttypeName
        End Get
        Set(value As String)
            _contenttypeName = value
            RaisePropertyChanged("contenttypeName")
        End Set
    End Property
    Private Property _contenttypeName As String

    Public Property channel As String
        Get
            Return _channel
        End Get
        Set(value As String)
            _channel = value
            RaisePropertyChanged("channel")
            If value = "" Then ChannelSelectionEnabled = False Else ChannelSelectionEnabled = True
        End Set
    End Property
    Private Property _channel As String

    Public Property channelname As String
        Get
            Return _channelname

        End Get
        Set(value As String)
            _channelname = value
            RaisePropertyChanged("channelname")
        End Set
    End Property
    Private Property _channelname As String
    Private Property _chicon As String
    Public Property chicon As String
        Get
            Return _chicon
        End Get
        Set(value As String)
            _chicon = value
            RaisePropertyChanged("chicon")
        End Set
    End Property

    Public Property tagUuid As String
        Get
            Return _tagUuid
        End Get
        Set(value As String)
            _tagUuid = value
            RaisePropertyChanged("tagUuid")
            If value = "" Then ChannelTagSelectionEnabled = False Else ChannelTagSelectionEnabled = True
        End Set
    End Property
    Private Property _tagUuid As String
    Public Property tagName As String
        Get
            Return _tagName
        End Get
        Set(value As String)
            _tagName = value
            RaisePropertyChanged("tagName")
        End Set
    End Property
    Private Property _tagName As String

    Public Property pri As Integer
    Private Property _name As String
    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            displayName = value
            RaisePropertyChanged("name")
        End Set
    End Property
    Public Property configUuid As String
        Get
            Return _configUuid
        End Get
        Set(value As String)
            _configUuid = value
            RaisePropertyChanged("configUuid")
        End Set
    End Property
    Private Property _configUuid As String

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

    Public Property comment As String
        Get
            Return _comment
        End Get
        Set(value As String)
            _comment = value
            RaisePropertyChanged("comment")
        End Set
    End Property
    Private Property _comment As String


    Public Property title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
            displayName = value
            RaisePropertyChanged("title")
        End Set
    End Property
    Private Property _title As String
    Private Property _displayName As String
    Public Property displayName As String
        Get
            If name = "" Then
                Return title
            Else
                Return name
            End If
        End Get
        Set(value As String)
            _displayName = value
            RaisePropertyChanged("displayName")
        End Set
    End Property

    Public Property startafter_isenabled As Boolean
        Get
            Return _startafter_isenabled
        End Get
        Set(value As Boolean)
            _startafter_isenabled = value
            RaisePropertyChanged("startafter_isenabled")
        End Set
    End Property
    Private Property _startafter_isenabled As Boolean

    Public Property startbefore_isenabled As Boolean
        Get
            Return _startbefore_isenabled
        End Get
        Set(value As Boolean)
            _startbefore_isenabled = value
            RaisePropertyChanged("startbefore_isenabled")
        End Set
    End Property
    Private Property _startbefore_isenabled As Boolean


    Public Property start As String
        Get
            Return _start
        End Get
        Set(value As String)
            If value = "Any" Then
                _start = "Any"
                RaisePropertyChanged("start")
            Else
                Dim aa As DateTime
                DateTime.TryParse(value, aa)
                _start = aa.ToString("HH:mm")
                RaisePropertyChanged("start")
            End If
        End Set
    End Property
    Private Property _start As String


    Private Property _startTime As TimeSpan
    Public Property startTime As TimeSpan
        Get
            Return _startTime
        End Get
        Set(value As TimeSpan)
            _startTime = value
            RaisePropertyChanged("startTime")
        End Set
    End Property

    Private Property _start_window_Time As TimeSpan
    Public Property start_window_Time As TimeSpan
        Get
            Return _start_window_Time
        End Get
        Set(value As TimeSpan)
            _start_window_Time = value
            RaisePropertyChanged("start_window_Time")
        End Set
    End Property

    Public Property start_window As String
        Get
            Return _start_window
        End Get
        Set(value As String)
            If value = "Any" Then
                _start_window = "Any"
                RaisePropertyChanged("start_window")
            Else
                Dim aa As DateTime
                DateTime.TryParse(value, aa)
                _start_window = aa.ToString("HH:mm")
                RaisePropertyChanged("start_window")
            End If
        End Set
    End Property
    Private Property _start_window As String

    Public ReadOnly Property approx_time_date As DateTime
        Get
            If _start = "Any" Then
                Return DateTime.ParseExact("00:00", "HH:mm", Nothing)
            Else
                Return DateTime.ParseExact(_start, "HH:mm", Nothing)
            End If
        End Get
    End Property

    Public Property weekdays As List(Of Integer)
        Get
            Return _weekdays
        End Get
        Set(value As List(Of Integer))
            _weekdays = value
            RaisePropertyChanged("recordOnMonday")
            RaisePropertyChanged("recordOnTuesday")
            RaisePropertyChanged("recordOnWednesday")
            RaisePropertyChanged("recordOnThursday")
            RaisePropertyChanged("recordOnFriday")
            RaisePropertyChanged("recordOnSaturday")
            RaisePropertyChanged("recordOnSunday")
            RaisePropertyChanged("recordingDays")
        End Set
    End Property
    Private Property _weekdays As List(Of Integer)

    Public ReadOnly Property Monday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(1)
            'Return New DateTime(2014, 5, 5).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Tuesday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(2)
            'Return New DateTime(2014, 5, 6).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Wednesday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(3)
            'Return New DateTime(2014, 5, 7).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Thursday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(4)
            'Return New DateTime(2014, 5, 8).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Friday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(5)
            'Return New DateTime(2014, 5, 9).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Saturday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(6)
            'Return New DateTime(2014, 5, 10).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property
    Public ReadOnly Property Sunday As String
        Get
            Return vm.myCultureInfoHelper.GetAbbreviatedDayNames()(0)
            'Return New DateTime(2014, 5, 11).ToString("ddd", System.Globalization.CultureInfo.CurrentCulture)
        End Get
    End Property

    Public Property recordOnMonday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(1) > -1, True, False)
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(1) > -1 Then
                _weekdays.Remove(1)
            End If
            If value = True And weekdays.IndexOf(1) = -1 Then
                _weekdays.Add(1)
            End If
            _recordOnMonday = value
            RaisePropertyChanged("recordOnMonday")
        End Set
    End Property
    Private Property _recordOnMonday As Boolean

    Public Property recordOnTuesday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(2) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(2) > -1 Then
                weekdays.Remove(2)
            End If
            If value = True And weekdays.IndexOf(2) = -1 Then
                weekdays.Add(2)
            End If
            _recordOnTuesday = value
            RaisePropertyChanged("recordOnTuesday")
        End Set
    End Property
    Private Property _recordOnTuesday As Boolean

    Public Property recordOnWednesday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(3) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(3) > -1 Then
                weekdays.Remove(3)
            End If
            If value = True And weekdays.IndexOf(3) = -1 Then
                weekdays.Add(3)
            End If
            _recordOnWednesday = value
            RaisePropertyChanged("recordOnWednesday")
        End Set
    End Property
    Private Property _recordOnWednesday As Boolean

    Public Property recordOnThursday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(4) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(4) > -1 Then
                weekdays.Remove(4)
            End If
            If value = True And weekdays.IndexOf(4) = -1 Then
                weekdays.Add(4)
            End If
            _recordOnThursday = value
            RaisePropertyChanged("recordOnThursday")
        End Set
    End Property
    Private Property _recordOnThursday As Boolean

    Public Property recordOnFriday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(5) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(5) > -1 Then
                weekdays.Remove(5)
            End If
            If value = True And weekdays.IndexOf(5) = -1 Then
                weekdays.Add(5)
            End If
            _recordOnFriday = value
            RaisePropertyChanged("recordOnFriday")
        End Set
    End Property
    Private Property _recordOnFriday As Boolean

    Public Property recordOnSaturday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(6) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(6) > -1 Then
                weekdays.Remove(6)
            End If
            If value = True And weekdays.IndexOf(6) = -1 Then
                weekdays.Add(6)
            End If
            _recordOnSaturday = value
            RaisePropertyChanged("recordOnSaturday")
        End Set
    End Property
    Private Property _recordOnSaturday As Boolean

    Public Property recordOnSunday As Boolean
        Get
            If Not weekdays Is Nothing Then
                Return If(weekdays.IndexOf(7) > -1, True, False)
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value = False And weekdays.IndexOf(7) > -1 Then
                weekdays.Remove(7)
            End If
            If value = True And weekdays.IndexOf(7) = -1 Then
                weekdays.Add(7)
            End If
            _recordOnSunday = value
            RaisePropertyChanged("recordOnSunday")
        End Set
    End Property
    Private Property _recordOnSunday As Boolean


    Private Property _recordingDays As String
    Public Property recordingDays As String
        Get
            Dim daysList As New List(Of String)
            If weekdays.IndexOf(1) > -1 Then daysList.Add(Monday)
            If weekdays.IndexOf(2) > -1 Then daysList.Add(Tuesday)
            If weekdays.IndexOf(3) > -1 Then daysList.Add(Wednesday)
            If weekdays.IndexOf(4) > -1 Then daysList.Add(Thursday)
            If weekdays.IndexOf(5) > -1 Then daysList.Add(Friday)
            If weekdays.IndexOf(6) > -1 Then daysList.Add(Saturday)
            If weekdays.IndexOf(7) > -1 Then daysList.Add(Sunday)
            Return String.Join("/", daysList)

        End Get
        Set(value As String)
            RaisePropertyChanged("recordingDays")
        End Set
    End Property


    Public Property AutoRecordingViewTitle As String


    Public Property Status As String
        Get
            Return _Status
        End Get
        Set(value As String)
            _Status = value
            RaisePropertyChanged("Status")
        End Set
    End Property
    Private Property _Status As String

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
    Public Property ChannelSelectionEnabled As Boolean
        Get
            Return _ChannelSelectionEnabled
        End Get
        Set(value As Boolean)
            _ChannelSelectionEnabled = value
            If value = True Then
                Dim x As ChannelViewModel = (From c In vm.AllChannels.items Where c.channelUuid = channel Select c).FirstOrDefault()
                If Not x Is Nothing Then
                    channelname = x.name
                Else
                    channelname = vm.loader.GetString("strAnyChannel")
                End If
            Else
                channelname = vm.loader.GetString("strAnyChannel")
            End If
            RaisePropertyChanged("ChannelSelectionEnabled")
        End Set
    End Property
    Private Property _ChannelSelectionEnabled As Boolean
    Public Property ChannelTagSelectionEnabled As Boolean
        Get
            Return _ChannelTagSelectionEnabled
        End Get
        Set(value As Boolean)
            _ChannelTagSelectionEnabled = value
            If value = True Then
                Dim t As ChannelTagViewModel = (From c In vm.ChannelTags.items Where c.uuid = tagUuid Select c).FirstOrDefault()
                If Not t Is Nothing Then
                    tagName = t.name
                Else
                    tagName = vm.loader.GetString("strAnyChannelTag")
                End If
            Else
                tagName = vm.loader.GetString("strAnyChannelTag")
            End If
            RaisePropertyChanged("ChannelTagSelectionEnabled")

        End Set
    End Property
    Private Property _ChannelTagSelectionEnabled As Boolean
    Public Property GenreSelectionEnabled As Boolean
        Get
            Return _GenreSelectionEnabled
        End Get
        Set(value As Boolean)
            _GenreSelectionEnabled = value
            If value = True Then
                Dim x As ContentTypeViewModel = (From c In vm.Genres.items Where c.uuid = contenttype Select c).FirstOrDefault()
                If Not x Is Nothing Then
                    contenttypeName = x.name
                Else
                    contenttypeName = vm.loader.GetString("strAnyGenre")
                End If
            Else
                contenttypeName = vm.loader.GetString("strAnyGenre")
            End If
            RaisePropertyChanged("GenreSelectionEnabled")
        End Set
    End Property
    Private Property _GenreSelectionEnabled As Boolean





    Public Sub New()
        Status = "New"
        start = Date.Now
        title = "myTitle"
        AutoRecordingViewTitle = vm.loader.GetString("AddAutoRecording")
        channelname = "Any Channel"
        tagName = "All Channels"
        tagUuid = ""
        channel = ""
        configName = ""
        configUuid = ""
        weekdays = New List(Of Integer)
    End Sub



    Public Sub New(recording As tvh40.AutoRecording)
        'Create a new AutoRecording Instance based on a tvh40 JSON return result. Fill in the missing information by querying the various lookup lists
        channel = recording.channel

        Dim c As ChannelViewModel
        c = (From channel In vm.AllChannels.items Where channel.channelUuid = Me.channel Select channel).FirstOrDefault()
        If Not c Is Nothing Then
            chicon = c.chicon
        Else
            chicon = "ms-appx:///Images/tvheadend.png"
        End If

        contenttype = recording.content_type
        comment = recording.comment
        tagUuid = recording.tag

        Status = "New"

        TimeSpan.TryParse(recording.start, startTime)
        TimeSpan.TryParse(recording.start_window, start_window_Time)

        start = If(recording.start = "Any", "00:00", recording.start)
        start_window = If(recording.start_window = "Any", "00:00", recording.start_window)
        startafter_isenabled = If(recording.start = "Any", False, True)
        startbefore_isenabled = If(recording.start_window = "Any", False, True)

        enabled = recording.enabled
        configUuid = recording.config_name ' Actually the UUID from the DVR Config
        'TODO : Collect the ConfigName based on the UUID
        Me.configName = (From d In vm.DVRConfigs.items Where d.identifier = configUuid Select d.name).FirstOrDefault
        If configUuid = "" Then DVRConfigSelectionEnabled = False Else DVRConfigSelectionEnabled = True
        id = recording.uuid
        pri = 0

        title = recording.title
        name = recording.name
        weekdays = recording.weekdays
        recordOnMonday = If(weekdays.IndexOf(1) > -1, True, False)
        recordOnTuesday = If(weekdays.IndexOf(2) > -1, True, False)
        recordOnWednesday = If(weekdays.IndexOf(3) > -1, True, False)
        recordOnThursday = If(weekdays.IndexOf(4) > -1, True, False)
        recordOnFriday = If(weekdays.IndexOf(5) > -1, True, False)
        recordOnSaturday = If(weekdays.IndexOf(6) > -1, True, False)
        recordOnSunday = If(weekdays.IndexOf(7) > -1, True, False)
        AutoRecordingViewTitle = vm.loader.GetString("EditAutoRecording")
        ExpanseCollapseEnabled = True
    End Sub

    Public Sub Update(Optional updatedAutoRecording As AutoRecordingViewModel = Nothing)
        If updatedAutoRecording Is Nothing Then
            'retrieve the recoridng from the server
        Else
            Me.enabled = updatedAutoRecording.enabled
            Me.title = updatedAutoRecording.title
            Me.name = updatedAutoRecording.name
            Me.channelname = updatedAutoRecording.channelname
            Me.chicon = updatedAutoRecording.chicon
            Me.weekdays = updatedAutoRecording.weekdays
        End If
    End Sub


    ''' <summary>
    ''' 'Stores all values into backup fields in order to be able to restore the autorecording to original values when the user decides to cancel out of the editing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Backup()


    End Sub




    ''' <summary>
    ''' Prepares the AutoRecordingViewModel for Saving. It returns a copy of the object, in which small adjustments have been made to cope with the TVHeadend Server JSON API.
    ''' For example : TVH 3.4 sees an empty config_name string as 'the default', where TVH 3.9 has already assigned a uuid to the default config_name
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReadyForSaving()
        'Dim mySave As AutoRecordingViewModel = Me.MemberwiseClone()
        'Hack for 3.4 TVH Servers, which don't like to congest (default) as the default profile
        If Me.configName = "(default)" Or Not Me.DVRConfigSelectionEnabled Then Me.configName = ""

        If Not Me.startafter_isenabled Then
            Me.start = "Any"
        Else
            Me.start = String.Format("{0:0}:{1:00}", Me.startTime.Hours, Me.startTime.Minutes)
        End If

        If Not Me.startbefore_isenabled Then
            Me.start_window = "Any"
        Else
            Me.start_window = String.Format("{0:0}:{1:00}", Me.start_window_Time.Hours, Me.start_window_Time.Minutes)
        End If

        If Not Me.ChannelSelectionEnabled Then Me.channel = ""
        If Not Me.ChannelTagSelectionEnabled Then
            Me.tagUuid = "" 'Used by tvh40
            Me.tagName = "" 'Used by TVH34
        End If
        If Not Me.GenreSelectionEnabled Then Me.contenttype = Nothing
        If Not Me.DVRConfigSelectionEnabled Then Me.configUuid = ""
    End Sub


End Class