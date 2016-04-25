Imports GalaSoft.MvvmLight
Imports Windows.Web.Http
Imports Newtonsoft.Json
Imports Windows.UI

Public Class ChannelViewModel
    Inherits ViewModelBase
    Private _channel As TVHChannel
    Public ReadOnly Property ch_icon As String
        Get
            Return _channel.icon_public_url
        End Get
    End Property
    Public ReadOnly Property ChannelBackground As SolidColorBrush
        Get
            If IsSelected Then
                Return CType(Application.Current.Resources("SystemControlHighlightAccentBrush"), SolidColorBrush)
            Else
                Return New SolidColorBrush(Colors.Transparent)
            End If

        End Get
    End Property
    Public ReadOnly Property ChannelNumberVisibility As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return If(vm.TVHeadSettings.ShowChannelNumbers, "Visible", "Collapsed")
        End Get
    End Property
    Private Property _chicon As String
    Public ReadOnly Property chicon As String
        Get
            Dim i = _channel.icon_public_url
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
    'Public ReadOnly Property currentEPGItem As EPGItemViewModel
    '    Get
    '        If epgitems.items.Count > 0 Then Return epgitems.items(0) Else Return New EPGItemViewModel
    '        'Return _currentEPGItem
    '    End Get
    'End Property
    Public ReadOnly Property CurrentEPGItemDetailsVisibility As String
        Get
            If IsExpanded Then Return "Visible" Else Return "Collapsed"
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
    Public Property IsExpanded As Boolean
        Get
            Return _IsExpanded
        End Get
        Set(value As Boolean)
            If value <> _IsExpanded Then
                _IsExpanded = value
                RaisePropertyChanged("IsExpanded")
                RaisePropertyChanged("CurrentEPGItemDetailsVisibility")
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

    Public Async Function LoadEPG(Optional loadAll As Boolean = True, Optional maxitems As Integer = 0) As Task
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasEPGAccess Then
            Me.epgitems.items.Clear()
            WriteToDebug("ChannelViewModel.LoadEPG()", "Loading EPG Entry for channel :" & Me.name)
            Dim response As HttpResponseMessage
            response = Await (New Downloader).DownloadJSON((New api40).apiGetEPGEvents(Me.uuid, loadAll, maxitems))
            Dim json_result As String
            If response.IsSuccessStatusCode Then
                json_result = Await response.Content.ReadAsStringAsync
                Dim deserialized As TVHEPGEventList
                Try
                    deserialized = JsonConvert.DeserializeObject(Of TVHEPGEventList)(json_result)
                    Dim zut As New List(Of EPGItemViewModel)
                    If deserialized.entries.Count > 0 Then
                        For Each entry In deserialized.entries.OrderBy(Function(x) x.start)
                            zut.Add(New EPGItemViewModel(entry))
                        Next
                    Else
                        zut.Add(New EPGItemViewModel)
                    End If
                    Me.epgitems.items = zut.ToObservableCollection()
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
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        If Await vm.TVHeadSettings.hasEPGAccess Then
            WriteToDebug("ChannelViewModel.RefreshEPG()", "executed")
            Dim epgitemsToRemove, epgitemsToAdd, epgItemsToUpdate As List(Of EPGItemViewModel)
            Dim oldEPGItems As List(Of EPGItemViewModel) = epgitems.items.ToList()
            If fromServer Then
                Dim newEPGItems As List(Of EPGItemViewModel) = (Await LoadEPGEntry(Me, True)).ToList()
                epgitemsToRemove = oldEPGItems.Where(Function(p) Not newEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
                epgitemsToAdd = newEPGItems.Where(Function(p) Not oldEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
                epgItemsToUpdate = newEPGItems.Where(Function(p) oldEPGItems.Any(Function(p2) p2.eventId = p.eventId)).ToList()
            Else
                epgitemsToRemove = oldEPGItems.Where(Function(p) p.percentcompleted = 1).ToList()
                epgItemsToUpdate = oldEPGItems.Where(Function(p) p.percentcompleted < 1).ToList()
                epgitemsToAdd = Nothing
            End If

            If Not epgitemsToRemove Is Nothing Then
                For Each e In epgitemsToRemove
                    epgitems.RemoveEvent(e.eventId)
                Next
            End If
            If Not epgitemsToAdd Is Nothing Then
                For Each e In epgitemsToAdd
                    epgitems.AddEvent(e)
                Next
            End If
            If Not epgItemsToUpdate Is Nothing Then
                For Each e In epgItemsToUpdate
                    epgitems.UpdateEvent(e)
                Next
            End If
            'vm.StatusBar.Clean()
        End If

    End Function

    Public Async Function RefreshCurrentEPGItem(Optional newitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task

        If Not epgitems.groupeditems.Count = 0 Then
            For Each g In epgitems.groupeditems
                For Each i In g.Where(Function(x) x.percentcompleted = 1)
                    epgitems.RemoveEvent(i.eventId)
                Next
            Next

            For Each g In epgitems.groupeditems
                For Each i In g.Where(Function(x) x.percentcompleted > 0 And x.percentcompleted < 1)
                    i.Update()
                Next
            Next

            If epgitems.groupeditems.Count = 0 Then
                Await LoadEPG(False, 1)
            End If
        End If
    End Function

    Public Async Function UpdateCurrentEPGItem(Optional newepgitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task
        'If a new epgitem is provided, we only add it to the epgitems collection and point the currentEPGItem to that item by raising propertychanged
        If Not newepgitem Is Nothing Then
            epgitems.items.Clear()
            epgitems.AddEvent(newepgitem)
            Exit Function
        End If

        'WriteToDebug("ChannelViewModel.UpdateCurrentEPGItem()", "executed")
        If epgitems.currentEPGItem.percentcompleted > 0 And epgitems.currentEPGItem.percentcompleted < 1 Then
            epgitems.currentEPGItem.Update()
            Exit Function
        End If
        If epgitems.currentEPGItem.percentcompleted = 1 Then
            'Check if channel's epgitems contains more than 1 EPG entry. If so, only the finished currentEPGitem can be removed. If not, then ask the TVH server for an updated EPG item
            If epgitems.items.Count = 0 Then
                Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
                If Not retrievedItem Is Nothing Then
                    If retrievedItem.eventId = epgitems.currentEPGItem.eventId Then
                        'We retrieved the same EPG Event as the current one, we don't need to update it yet
                        Exit Function
                    End If
                    epgitems.AddEvent(retrievedItem)
                Else
                    'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
                    epgitemsAvailable = False
                End If
            End If
            'Finally remove the old EPG item
            epgitems.RemoveEvent(epgitems.currentEPGItem.eventId)
        End If
        If epgitems.currentEPGItem.percentcompleted = 0 And epgitems.currentEPGItem.eventId = 0 Then
            Dim retrievedItem As EPGItemViewModel = (Await LoadEPGEntry(Me, False, 1)).FirstOrDefault()
            If Not retrievedItem Is Nothing Then
                epgitems.AddEvent(retrievedItem)
            Else
                'TVH doesn't have any EPG information for this channel. Set a flag that will prevent the channel for asking it's currentEPGItem each time
                epgitemsAvailable = False
            End If
        End If
    End Function

    Public Sub New()
        _channel = New TVHChannel
        epgitems = New EPGItemListViewModel
        epgItemsLoaded = False
        epgitemsAvailable = True
        IsExpanded = False
        IsSelected = False
    End Sub

    Public Sub New(channel As TVHChannel)
        _channel = channel
        epgitems = New EPGItemListViewModel
        epgItemsLoaded = False
        epgitemsAvailable = True
        IsExpanded = False
        IsSelected = False
    End Sub
End Class
