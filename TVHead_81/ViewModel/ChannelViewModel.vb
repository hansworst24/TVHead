Imports GalaSoft.MvvmLight
Imports Windows.Web.Http
Imports Newtonsoft.Json
Imports Windows.UI

Public Class ChannelViewModel
    Inherits ViewModelBase
    Private _channel As tvh40.Channel

    Private ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
        End Get

    End Property
    Public ReadOnly Property ch_icon As String
        Get
            Return _channel.icon_public_url
        End Get
    End Property
    Public ReadOnly Property ChannelNumberVisibility As String
        Get
            Return If((New TVHead_Settings).ShowChannelNumbers, "Visible", "Collapsed")
        End Get
    End Property
    Private Property _chicon As String
    Public ReadOnly Property chicon As String
        Get
            Dim i = _channel.icon_public_url
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
    Public ReadOnly Property currentEPGItem As EPGItemViewModel
        Get
            If epgitems.items.Count > 0 Then Return epgitems.items(0) Else Return New EPGItemViewModel
            'Return _currentEPGItem
        End Get
    End Property
    Public ReadOnly Property dvr_pre_time As Integer
        Get
            Return _channel.dvr_pre_time
        End Get
    End Property
    Public ReadOnly Property dvr_pst_time As Integer
        Get
            Return _channel.dvr_pst_time
        End Get
    End Property
    Public ReadOnly Property enabled As Boolean
        Get
            Return _channel.enabled
        End Get
    End Property
    Private Property _epgitems As EPGItemListViewModel
    Public Property epgitems As EPGItemListViewModel
        Get
            Return _epgitems
        End Get
        Set(value As EPGItemListViewModel)
            _epgitems = value
            RaisePropertyChanged("epgitems")
        End Set
    End Property

    Public ReadOnly Property epgitemcount As Integer
        Get
            Return epgitems.items.Count
        End Get
    End Property

    Public Property epgitemsAvailable As Boolean
    Private Property _epgItemsLoaded As Boolean
    Public Property epgItemsLoaded As Boolean
        Get
            Return _epgItemsLoaded
        End Get
        Set(value As Boolean)
            _epgItemsLoaded = value
            RaisePropertyChanged("epgItemsLoaded")
        End Set
    End Property
    'Public Property ExpandedView As String
    '    Get
    '        Return _ExpandedView
    '    End Get
    '    Set(value As String)
    '        _ExpandedView = value
    '        RaisePropertyChanged("ExpandedView")
    '    End Set
    'End Property
    'Private Property _ExpandedView As String
    'Public ReadOnly Property groupeditems As List(Of Group(Of EPGItemViewModel))
    '    Get
    '        If Not epgitems Is Nothing AndAlso epgitems.Count > 0 Then
    '            Return (From epgevent In epgitems
    '                    Group By Day = epgevent.startDate.Date Into Group
    '                    Select New Group(Of EPGItemViewModel)(Day, Group)).ToList
    '        End If

    '    End Get
    '    'Set(value As ObservableCollection(Of Group(Of EPGItemViewModel)))
    '    '    _groupeditems = value
    '    '    RaisePropertyChanged("groupeditems")
    '    '    RaisePropertyChanged("currentEPGItem")
    '    'End Set
    'End Property


    Public Property IsExpanded As Boolean
        Get
            Return _IsExpanded
        End Get
        Set(value As Boolean)
            If value <> _IsExpanded Then
                _IsExpanded = value
                RaisePropertyChanged("IsExpanded")
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
                RaisePropertyChanged("ChannelBackground")
            End If
        End Set
    End Property
    Private Property _IsSelected As Boolean

    Public ReadOnly Property ChannelBackground As SolidColorBrush
        Get
            If IsSelected Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return New SolidColorBrush(Colors.Transparent)
            End If

        End Get
    End Property

    Public ReadOnly Property name As String
        Get
            Return _channel.name
        End Get
    End Property
    Public ReadOnly Property number As Integer
        Get
            Return _channel.number
        End Get
    End Property
    Public ReadOnly Property services As List(Of String)
        Get
            Return _channel.services.ToList()
        End Get
    End Property
    Public ReadOnly Property tags As String()
        Get
            Return _channel.tags
        End Get
    End Property
    Public ReadOnly Property uuid As String
        Get
            Return _channel.uuid
        End Get
    End Property


    'Public Property ExpandCollapse As RelayCommand
    '    Get
    '        Return New RelayCommand(Sub()
    '                                    WriteToDebug("ChannelViewModel.ExpanseCollapse", "start")
    '                                    Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
    '                                    If rectie.Width > 720 Then
    '                                        vm.selectedEPGItem = Me.currentEPGItem
    '                                    Else
    '                                        If Me.ExpandedView = "Collapsed" Then
    '                                            If Not vm.AllChannels Is Nothing Then
    '                                                For Each c In vm.Channels.items
    '                                                    If Not c.ExpandedView = "Collapsed" Then c.ExpandedView = "Collapsed"
    '                                                Next
    '                                            End If
    '                                            Me.ExpandedView = "Visible"
    '                                        Else
    '                                            Me.ExpandedView = "Collapsed"
    '                                        End If
    '                                    End If
    '                                    WriteToDebug("ChannelViewModel.ExpanseCollapse", "stop")
    '                                End Sub)

    '    End Get
    '    Set(value As RelayCommand)
    '    End Set
    'End Property
    'Public Property LoadChannelEPGItems As RelayCommand
    '    Get
    '        Return New RelayCommand(Async Sub()
    '                                    If Await vm.TVHeadSettings.hasEPGAccess Then
    '                                        WriteToDebug("ChannelViewModel.LoadChannelEPGItems", "start")
    '                                        Await vm.StatusBar.Update("Loading EPG entries...", True, 0, True)
    '                                        Await Me.LoadEPG()
    '                                        ' If Me.epgitems.groupeditems.Count > 0 Then
    '                                        Me.epgItemsLoaded = True
    '                                        'vm.EPGInformationAvailable = True
    '                                        'Else
    '                                        '    Me.epgItemsLoaded = True
    '                                        '    vm.ChannelSelected = True
    '                                        '    vm.EPGInformationAvailable = False

    '                                        'End If

    '                                        Await vm.StatusBar.Clean()
    '                                        vm.SelectedChannel = Me
    '                                        vm.PivotSelectedIndex = 1
    '                                        vm.ChannelSelected = True
    '                                        WriteToDebug("ChannelViewModel.LoadChannelEPGItems", "stop")

    '                                        Dim rectie As Rect = ApplicationView.GetForCurrentView.VisibleBounds
    '                                        If rectie.Width > 720 Then vm.selectedEPGItem = Me.epgitems.currentEPGItem

    '                                    End If
    '                                End Sub)

    '    End Get
    '    Set(value As RelayCommand)
    '    End Set
    'End Property


    'Private Property _loadEPGButtonEnabled As Boolean
    'Public Property loadEPGButtonEnabled As Boolean
    '    Get
    '        Return _loadEPGButtonEnabled
    '    End Get
    '    Set(value As Boolean)
    '        _loadEPGButtonEnabled = value
    '        RaisePropertyChanged("loadEPGButtonEnabled")
    '    End Set
    'End Property






    'Public Property Status As String
    '    Get
    '        Return _Status
    '    End Get
    '    Set(value As String)

    '        _Status = value
    '        RaisePropertyChanged("Status")


    '    End Set
    'End Property
    'Private Property _Status As String
    'Public Async Function LoadEPGEntry(selectedChannel As ChannelViewModel, Optional loadAll As Boolean = True, Optional maxItems As Integer = 300) As Task(Of IEnumerable(Of EPGItemViewModel))
    '    Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
    '    WriteToDebug("Modules.LoadEPGEntry()", "Loading EPG Entry for channel :" & selectedChannel.name)
    '    Dim result As New List(Of EPGItemViewModel)
    '    Dim json_result As String
    '    Try
    '        json_result = Await (Await (New Downloader).DownloadJSON(tvh40api.apiGetEPGEvents(selectedChannel.uuid, loadAll, maxItems))).Content.ReadAsStringAsync
    '    Catch ex As Exception
    '        'WriteToDebug("Modules.LoadEPGEntry()", ex.InnerException.ToString)
    '        Return result
    '    End Try
    '    If Not json_result = "" Then
    '        Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
    '        For Each entry In deserialized.entries
    '            result.Add(New EPGItemViewModel(entry))
    '        Next
    '    End If
    '    WriteToDebug("Modules.LoadEPGEntry()", "Completed Loading EPG Entries. : " & result.Count.ToString & "item(s)")
    '    Return result.OrderBy(Function(x) x.start)
    'End Function


    'Public Async Function AddEvent(e As EPGItemViewModel) As Task
    '    Await RunOnUIThread(Sub()
    '                            epgitems.Add(e)
    '                        End Sub)
    'End Function

    'Public Async Function RemoveEvent(eventid As String) As Task
    '    'WriteToDebug("EPGItemListViewModel.RemoveEvent()", "executed")
    '    Dim eventToRemove As EPGItemViewModel = epgitems.Where(Function(x) x.eventId = eventid).FirstOrDefault
    '    If Not eventToRemove Is Nothing Then
    '        Await RunOnUIThread(Sub()
    '                                epgitems.Remove(eventToRemove)
    '                            End Sub)
    '    End If


    'End Function

    ''' <summary>
    ''' Searches for and then updates a given EPGItem within the EPGItemListViewModel
    ''' </summary>
    ''' <param name="e">Updated EPG Event</param>
    '''' <returns></returns>
    'Public Async Function UpdateEvent(e As EPGItemViewModel) As Task
    '    e.Update()
    'End Function


    'Public Function GetEvent(eventid As String) As EPGItemViewModel
    '    Dim tmpEvent As EPGItemViewModel = (From e In epgitems.items Where e.eventId = eventid Select e).FirstOrDefault()
    '    If Not tmpEvent Is Nothing Then
    '        Return tmpEvent
    '    End If
    '    Return Nothing
    'End Function

    'Public Sub ClearAllButCurrent()
    '    If epgitems.Count > 1 Then
    '        epgitems.RemoveRange(1, epgitems.Count - 1)
    '    End If
    '    RaisePropertyChanged("groupedEPGItems")
    '    RaisePropertyChanged("currentEPGItem")
    'End Sub




    Public Async Function LoadEPG(Optional loadAll As Boolean = True, Optional maxitems As Integer = 0) As Task
        If Await vm.TVHeadSettings.hasEPGAccess Then
            Me.epgitems.items.Clear()
            Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
            WriteToDebug("ChannelViewModel.LoadEPG()", "Loading EPG Entry for channel :" & Me.name)
            Dim response As HttpResponseMessage
            response = Await (New Downloader).DownloadJSON((New api40).apiGetEPGEvents(Me.uuid, loadAll, maxitems))
            Dim json_result As String
            If response.IsSuccessStatusCode Then
                json_result = Await response.Content.ReadAsStringAsync
                Dim deserialized As tvh40.EPGEventList
                Try
                    deserialized = JsonConvert.DeserializeObject(Of tvh40.EPGEventList)(json_result)
                    Dim zut As New List(Of EPGItemViewModel)
                    If deserialized.entries.Count > 0 Then
                        For Each entry In deserialized.entries.OrderBy(Function(x) x.start)
                            zut.Add(New EPGItemViewModel(entry))
                        Next
                    Else
                        zut.Add(New EPGItemViewModel)
                    End If
                    Me.epgitems.items = zut
                    deserialized = Nothing
                    epgItemsLoaded = True
                Catch ex As Exception
                    Throw New Exception
                End Try

            End If
            RaisePropertyChanged("currentEPGItem")
        End If

    End Function

    ''' <summary>
    ''' 'Re-downloads EPG information for the channel, updates any changes to DVR state and removes obsolete EPG entries
    ''' </summary>
    ''' <returns>Task</returns>
    Public Async Function RefreshEPG(fromServer As Boolean) As Task
        'If Await vm.TVHeadSettings.hasEPGAccess Then
        '    WriteToDebug("ChannelViewModel.RefreshEPG()", "executed")
        '    'vm.StatusBar.Update(vm.loader.GetString("status_RefreshingEPGEntries"), True, 0, True, True)
        '    Dim epgitemsToRemove, epgitemsToAdd, epgItemsToUpdate As List(Of EPGItemViewModel)
        '    Dim oldEPGItems As List(Of EPGItemViewModel) = (From g In groupeditems From e In g Select e).ToList()
        '    If fromServer Then
        '        Dim newEPGItems As List(Of EPGItemViewModel) = (Await LoadEPGEntry(Me, True)).ToList()
        '        epgitemsToRemove = oldEPGItems.Where(Function(p) Not newEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
        '        epgitemsToAdd = newEPGItems.Where(Function(p) Not oldEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
        '        epgItemsToUpdate = newEPGItems.Where(Function(p) oldEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
        '    Else
        '        epgitemsToRemove = oldEPGItems.Where(Function(p) p.percentcompleted = 1).ToList()
        '        epgItemsToUpdate = oldEPGItems.Where(Function(p) p.percentcompleted < 1).ToList()
        '        epgitemsToAdd = Nothing
        '    End If

        '    If Not epgitemsToRemove Is Nothing Then
        '        For Each e In epgitemsToRemove
        '            Await RemoveEvent(e.eventId)
        '        Next
        '    End If
        '    If Not epgitemsToAdd Is Nothing Then
        '        For Each e In epgitemsToAdd
        '            Await AddEvent(e)
        '        Next
        '    End If
        '    If Not epgItemsToUpdate Is Nothing Then
        '        For Each e In epgItemsToUpdate
        '            Await UpdateEvent(e)
        '        Next
        '    End If
        '    'vm.StatusBar.Clean()
        'End If

    End Function

    Public Async Function RefreshCurrentEPGItem(Optional newitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task

        'If Not epgitems.groupeditems.Count = 0 Then
        '    For Each g In epgitems.groupeditems
        '        For Each i In g.Where(Function(x) x.percentcompleted = 1)
        '            Await epgitems.RemoveEvent(i.eventId)
        '        Next
        '    Next

        '    For Each g In epgitems.groupeditems
        '        For Each i In g.Where(Function(x) x.percentcompleted > 0 And x.percentcompleted < 1)
        '            i.Update()
        '        Next
        '    Next

        '    If epgitems.groupeditems.Count = 0 Then
        '        Await LoadEPG(False, 1)
        '    End If
        'End If
    End Function



    ''' <summary>
    ''' Updates the current EPGItem for the channel. 
    ''' </summary>
    ''' <param name="newepgitem"></param>
    ''' <param name="fromServer"></param>
    ''' <returns></returns>
    'Public Async Function UpdateCurrentEPGItem(Optional newepgitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task
    '    'If a new epgitem is provided, we only add it to the epgitems collection and point the currentEPGItem to that item by raising propertychanged
    '    If Not newepgitem Is Nothing Then
    '        Await AddEvent(newepgitem)
    '        Exit Function
    '    End If

    '    'WriteToDebug("ChannelViewModel.UpdateCurrentEPGItem()", "executed")
    '    If currentEPGItem.percentcompleted > 0 And currentEPGItem.percentcompleted < 1 Then
    '        currentEPGItem.Update()
    '        Exit Function
    '    End If
    '    If currentEPGItem.percentcompleted = 1 Then
    '        'Check if channel's epgitems contains more than 1 EPG entry. If so, only the finished currentEPGitem can be removed. If not, then ask the TVH server for an updated EPG item
    '        If epgitems.Count = 1 Then
    '            Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
    '            If Not retrievedItem Is Nothing Then
    '                If retrievedItem.eventId = currentEPGItem.eventId Then
    '                    'We retrieved the same EPG Event as the current one, we don't need to update it yet
    '                    Exit Function
    '                End If
    '                Await AddEvent(retrievedItem)
    '                Await RunOnUIThread(Sub()
    '                                        RaisePropertyChanged("currentEPGItem")
    '                                        RaisePropertyChanged("groupeditems")
    '                                    End Sub)
    '            Else
    '                'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
    '                epgitemsAvailable = False
    '            End If
    '        End If
    '        'Finally remove the old EPG item
    '        Await RemoveEvent(currentEPGItem.eventId)
    '        Await RunOnUIThread(Sub()
    '                                RaisePropertyChanged("currentEPGItem")
    '                                RaisePropertyChanged("groupeditems")
    '                            End Sub)

    '    End If
    '    If currentEPGItem.percentcompleted = 0 And currentEPGItem.eventId = 0 Then
    '        Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
    '        If Not retrievedItem Is Nothing Then
    '            Await AddEvent(retrievedItem)
    '        Else
    '            'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
    '            epgitemsAvailable = False
    '        End If
    '    End If
    'End Function

    Public Async Function UpdateCurrentEPGItem(Optional newepgitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task
        'If a new epgitem is provided, we only add it to the epgitems collection and point the currentEPGItem to that item by raising propertychanged
        If Not newepgitem Is Nothing Then
            Await epgitems.AddEvent(newepgitem)
            Exit Function
        End If

        'WriteToDebug("ChannelViewModel.UpdateCurrentEPGItem()", "executed")
        If epgitems.currentEPGItem.percentcompleted > 0 And epgitems.currentEPGItem.percentcompleted < 1 Then
            epgitems.currentEPGItem.Update()
            Exit Function
        End If
        If epgitems.currentEPGItem.percentcompleted = 1 Then
            'Check if channel's epgitems contains more than 1 EPG entry. If so, only the finished currentEPGitem can be removed. If not, then ask the TVH server for an updated EPG item
            If epgitemcount = 0 Then
                Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
                If Not retrievedItem Is Nothing Then
                    If retrievedItem.eventId = epgitems.currentEPGItem.eventId Then
                        'We retrieved the same EPG Event as the current one, we don't need to update it yet
                        Exit Function
                    End If
                    Await epgitems.AddEvent(retrievedItem)
                Else
                    'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
                    epgitemsAvailable = False
                End If
            End If
            'Finally remove the old EPG item
            Await epgitems.RemoveEvent(epgitems.currentEPGItem.eventId)
        End If
        If epgitems.currentEPGItem.percentcompleted = 0 And epgitems.currentEPGItem.eventId = 0 Then
            Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
            If Not retrievedItem Is Nothing Then
                Await epgitems.AddEvent(retrievedItem)
            Else
                'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
                epgitemsAvailable = False
            End If
        End If
    End Function



    Public Sub New()
        _channel = New tvh40.Channel
        epgitems = New EPGItemListViewModel
        epgItemsLoaded = False
        epgitemsAvailable = True
    End Sub



    Public Sub New(channel As tvh40.Channel)
        _channel = channel
        epgitems = New EPGItemListViewModel
        epgItemsLoaded = False
        epgitemsAvailable = True
    End Sub
End Class
