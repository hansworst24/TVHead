Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports TVHead_81.ViewModels
Imports GalaSoft.MvvmLight.Command

Public Class ChannelViewModel
    Inherits ViewModelBase

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
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
    Public Property number As Integer
    Public Property genreName As String
    Public Property enabled As Boolean
    Public Property channelUuid As String
    Public Property tags As String()
    Private Property _title As String
    Public Property dvr_pre_time As Integer
    Public Property dvr_pst_time As Integer
    Public Property services As List(Of String)
    Public Property ch_icon As String

    Public Property ExpandCollapse As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        WriteToDebug("ChannelViewModel.ExpanseCollapse", "stop")
                                        If Me.ExpandedView = "Collapsed" Then
                                            If Not vm.AllChannels Is Nothing Then
                                                For Each c In vm.Channels.items
                                                    If Not c.ExpandedView = "Collapsed" Then c.ExpandedView = "Collapsed"
                                                Next
                                            End If
                                            Me.ExpandedView = "Visible"
                                        Else
                                            Me.ExpandedView = "Collapsed"
                                        End If
                                        WriteToDebug("ChannelViewModel.ExpanseCollapse", "stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property
    Public Property LoadChannelEPGItems As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        If Not vm.hasEPGAccess Then
                                            Await vm.checkEPGAccess()
                                        End If
                                        If vm.hasEPGAccess Then
                                            WriteToDebug("ChannelViewModel.LoadChannelEPGItems", "start")
                                            Await vm.StatusBar.Update("Loading EPG entries...", True, 0, True)
                                            Await Me.LoadEPG()
                                            If Me.epgitems.groupeditems.Count > 0 Then
                                                Me.epgItemsLoaded = True
                                                vm.EPGInformationAvailable = True
                                            Else
                                                Me.epgItemsLoaded = True
                                                vm.ChannelSelected = True
                                                vm.EPGInformationAvailable = False

                                            End If

                                            Await vm.StatusBar.Clean()
                                            vm.SelectedChannel = Me
                                            vm.PivotSelectedIndex = 1
                                            vm.ChannelSelected = True
                                            WriteToDebug("ChannelViewModel.LoadChannelEPGItems", "stop")
                                        End If
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
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
    Private Property _currentEPGItem As EPGItemViewModel
    Public Property currentEPGItem As EPGItemViewModel
        Get
            Return _currentEPGItem
        End Get
        Set(value As EPGItemViewModel)
            If _currentEPGItem Is Nothing Then
                _currentEPGItem = value
                RaisePropertyChanged("currentEPGItem")
            Else
                _currentEPGItem = value
                RaisePropertyChanged("currentEPGItem")
            End If
        End Set
    End Property

    Private Property _loadEPGButtonEnabled As Boolean
    Public Property loadEPGButtonEnabled As Boolean
        Get
            Return _loadEPGButtonEnabled
        End Get
        Set(value As Boolean)
            _loadEPGButtonEnabled = value
            RaisePropertyChanged("loadEPGButtonEnabled")
        End Set
    End Property

    Private Property _chicon As String
    Public Property chicon As String
        Get
            Return _chicon
        End Get
        Set(value As String)
            If Not value Is Nothing And Not value = "" Then
                If value.ToUpper().IndexOf("HTTP:/") >= 0 Or value.ToUpper().IndexOf("HTTPS:/") >= 0 Then
                    _chicon = value
                ElseIf value = "ms-appx:///Images/tvheadend.png" Then
                    _chicon = "ms-appx:///Images/tvheadend.png"
                ElseIf value.StartsWith("imagecache/") Then
                    _chicon = (New AppSettings).GetFullURL() & "/" & value
                Else
                    _chicon = "ms-appx:///Images/tvheadend.png"
                End If
            Else
                _chicon = "ms-appx:///Images/tvheadend.png"
            End If
            RaisePropertyChanged("chicon")
        End Set
    End Property



    Public Property ExpandedView As String

        Get
            Return _ExpandedView
        End Get
        Set(value As String)
            If value <> _ExpandedView Then
                _ExpandedView = value
                RaisePropertyChanged("ExpandedView")
            End If
        End Set
    End Property
    Private Property _ExpandedView As String
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

    Public Property ChannelNumberVisibility As String
        Get
            If vm.appSettings.ShowChannelNumbers Then
                Return "Visible"
            Else
                Return "Collapsed"
            End If
        End Get
        Set(value As String)
            _ChannelNumberVisibility = value
            RaisePropertyChanged("ChannelNumberVisibility")

        End Set
    End Property
    Private Property _ChannelNumberVisibility As String

    Public Async Function LoadEPG() As Task
        If Not vm.hasEPGAccess Then
            Await vm.checkEPGAccess()
        End If
        If vm.hasEPGAccess Then
            Dim newEPGItems As List(Of EPGItemViewModel) = (Await LoadEPGEntry(Me, True)).ToList()
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             epgitems.groupeditems = (From epgevent In newEPGItems
                                                                                                                                      Group By Day = epgevent.startDate.Date Into Group
                                                                                                                                      Select New Group(Of EPGItemViewModel)(Day, Group)).ToObservableCollection()
                                                                                                         End Sub)
        End If

    End Function

    ''' <summary>
    ''' 'Re-downloads EPG information for the channel, updates any changes to DVR state and removes obsolete EPG entries
    ''' </summary>
    ''' <returns>Task</returns>
    Public Async Function RefreshEPG(fromServer As Boolean) As Task
        If Not vm.hasEPGAccess Then
            Await vm.checkEPGAccess()
        End If
        If vm.hasEPGAccess Then
            WriteToDebug("ChannelViewModel.RefreshEPG()", "start")
            vm.StatusBar.Update(vm.loader.GetString("status_RefreshingEPGEntries"), True, 0, True, True)
            Dim epgitemsToRemove, epgitemsToAdd, epgItemsToUpdate As List(Of EPGItemViewModel)
            Dim oldEPGItems As List(Of EPGItemViewModel) = (From g In epgitems.groupeditems From e In g Select e).ToList()
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
                    Await epgitems.RemoveEvent(e.eventId)
                Next
            End If
            If Not epgitemsToAdd Is Nothing Then
                For Each e In epgitemsToAdd
                    Await epgitems.AddEvent(e)
                Next
            End If
            If Not epgItemsToUpdate Is Nothing Then
                For Each e In epgItemsToUpdate
                    Await epgitems.UpdateEvent(e)
                Next
            End If
            WriteToDebug("ChannelViewModel.RefreshEPG()", "stop")
            vm.StatusBar.Clean()
        End If

    End Function

    Public Async Function RefreshCurrentEPGItem(Optional newitem As EPGItemViewModel = Nothing, Optional fromServer As Boolean = False) As Task
        If Not vm.hasEPGAccess Then
            Await vm.checkEPGAccess()
        End If
        If vm.hasEPGAccess Then


            If newitem Is Nothing Then
                If Not currentEPGItem Is Nothing Then
                    If currentEPGItem.percentcompleted = 1 Then
                        'Update
                        newitem = (Await LoadEPGEntry(Me, False)).FirstOrDefault
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                         If Not newitem Is Nothing Then
                                                                                                                             If newitem.eventId <> currentEPGItem.eventId Then currentEPGItem = newitem
                                                                                                                         Else
                                                                                                                             currentEPGItem = (New EPGItemViewModel)
                                                                                                                         End If

                                                                                                                     End Sub)
                    Else
                        'Update percentcompleted
                        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                         If fromServer Then
                                                                                                                             newitem = (Await LoadEPGEntry(Me, False)).FirstOrDefault
                                                                                                                             currentEPGItem.Update(newitem)
                                                                                                                         Else
                                                                                                                             currentEPGItem.Update()
                                                                                                                         End If

                                                                                                                     End Sub)
                    End If
                Else
                    newitem = (Await LoadEPGEntry(Me, False)).FirstOrDefault
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                     If Not newitem Is Nothing Then
                                                                                                                         currentEPGItem = newitem
                                                                                                                     Else
                                                                                                                         currentEPGItem = (New EPGItemViewModel)
                                                                                                                     End If
                                                                                                                 End Sub)

                End If
            Else
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 If currentEPGItem Is Nothing Then
                                                                                                                     If Not newitem Is Nothing Then
                                                                                                                         currentEPGItem = newitem
                                                                                                                     End If
                                                                                                                 Else
                                                                                                                     If Not newitem Is Nothing Then
                                                                                                                         If currentEPGItem.eventId = newitem.eventId Then
                                                                                                                             currentEPGItem.dvrState = newitem.dvrState
                                                                                                                             currentEPGItem.dvrUuid = newitem.dvrUuid
                                                                                                                             currentEPGItem.percentcompleted = newitem.percentcompleted
                                                                                                                         Else
                                                                                                                             currentEPGItem = newitem
                                                                                                                         End If
                                                                                                                     Else
                                                                                                                         currentEPGItem.Update()
                                                                                                                     End If
                                                                                                                 End If
                                                                                                             End Sub)

            End If
        End If
    End Function

    Public Sub New()
        epgitems = New EPGItemListViewModel
        ExpandedView = "Collapsed"
        epgItemsLoaded = False
        ChannelNumberVisibility = If((New AppSettings).ShowChannelNumbers, "Visible", "Collapsed")
    End Sub



    Public Sub New(channel As tvh40.Channel)
        epgitems = New EPGItemListViewModel
        name = channel.name
        channelUuid = channel.uuid
        number = channel.number
        enabled = channel.enabled
        ch_icon = channel.icon
        'Debug.WriteLine("Channel : " + channel.name + ":" + channel.icon_public_url)
        chicon = channel.icon_public_url
        dvr_pre_time = channel.dvr_pre_time
        dvr_pst_time = channel.dvr_pst_time
        tags = channel.tags
        ExpandedView = "Collapsed"
        epgItemsLoaded = False
        ChannelNumberVisibility = If((New AppSettings).ShowChannelNumbers, "Visible", "Collapsed")
        loadEPGButtonEnabled = True
    End Sub
End Class
