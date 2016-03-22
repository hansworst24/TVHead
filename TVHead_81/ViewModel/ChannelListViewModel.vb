Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports TVHead_81.ViewModels

Public Class ChannelListViewModel
    Private Property _allchannels As New List(Of ChannelViewModel)
    Public Property items As New ObservableCollection(Of ChannelViewModel)
    Public Property dataLoaded As Boolean

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
        End Get

    End Property

    Public Async Function RefreshCurrentEvents(Optional c As ChannelViewModel = Nothing) As Task
        If Not vm.hasEPGAccess Then
            Await vm.checkEPGAccess()
        End If
        If vm.hasEPGAccess Then
            If c Is Nothing Then
                vm.StatusBar.Update(vm.loader.GetString("status_RefreshingChannels"), True, 0, True, True)
                'Trigger an update for all channels. Do 1 query to TVH server to retrieve X number of most recent EPG entries, where X is the amount of total channels
                'This should ensure we get the most recent EPG entry for each channel, and avoid hammering the server with individual requests
                Dim updatedEvents As List(Of EPGItemViewModel) = (Await LoadEPGEntry(New ChannelViewModel, False, Me._allchannels.Count)).ToList()
                If Not updatedEvents Is Nothing Then
                    For Each channel In Me._allchannels
                        Dim updatedEPGEventForChannel = updatedEvents.Where(Function(x) x.channelUuid = channel.channelUuid).FirstOrDefault()
                        If Not updatedEPGEventForChannel Is Nothing Then
                            Await channel.RefreshCurrentEPGItem(updatedEPGEventForChannel)
                        Else
                            channel.currentEPGItem = New EPGItemViewModel
                        End If
                    Next
                End If
            Else
                'Tell the channel to do it's own update
                Await c.RefreshCurrentEPGItem()


            End If
        End If
        vm.StatusBar.Clean()
    End Function

    Public Async Function AddChannel(c As ChannelViewModel) As Task
        If Not items Is Nothing Then
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             items.Add(c)
                                                                                                         End Sub)
        End If
    End Function

    Public Async Function ClearChannels() As Task
        If Not items Is Nothing Then
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             items.Clear()
                                                                                                         End Sub)

        End If
    End Function

    Public Async Function LoadAll() As Task
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, App).DefaultViewModel, TVHead_ViewModel)
        Dim selectedChannelTag As ChannelTagViewModel = vm.selectedChannelTag
        Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingChannels"), True, 0, True)
        Me._allchannels = (Await LoadAllChannels()).ToList()
        Await RefreshCurrentEvents()
        Dim newChannels = _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
        For Each channel In newChannels
            Await AddChannel(channel)
        Next
        Me.dataLoaded = True
        Await vm.StatusBar.Clean()
    End Function

    ''' <summary>
    ''' Clear and reload the channel list based on the selected ChannelTag in the TVHead_ViewModel.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function ReloadChannelList() As Task
        Me.items.Clear()
        'Update the current EPG Event information for each channel, before adding the channels to the view
        Await RefreshCurrentEvents()
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, App).DefaultViewModel, TVHead_ViewModel)
        Dim selectedChannelTag As ChannelTagViewModel = vm.selectedChannelTag

        Dim newChannels = _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
        For Each channel In newChannels
            Await AddChannel(channel)
        Next
    End Function

    Public Async Function LoadFavouriteTagChannels() As Task
        Await vm.StatusBar.Update(vm.loader.GetString("status_RefreshingChannels"), True, 0, True)
        Await Me.ClearChannels()
        Dim currentEPGEntries As List(Of EPGItemViewModel) = (Await LoadEPGEntry(New ChannelViewModel, False, vm.AllChannels.items.Count)).ToList()
        Dim channelsToAdd As IEnumerable(Of ChannelViewModel)
        If vm.ChannelTags.items.Count = 0 Then
            channelsToAdd = vm.AllChannels.items
        Else
            channelsToAdd = vm.AllChannels.items.Where(Function(x) x.tags.ToList.IndexOf(vm.selectedChannelTag.uuid) > -1)
        End If
        For Each i In channelsToAdd
            Dim currentEventForChannel As EPGItemViewModel = (From current In currentEPGEntries Where current.channelUuid = i.channelUuid Select current).FirstOrDefault()
            If currentEventForChannel Is Nothing Then currentEventForChannel = New EPGItemViewModel()
            Await i.RefreshCurrentEPGItem(currentEventForChannel)
            Await Me.AddChannel(i)
        Next
        Await vm.StatusBar.Clean()
    End Function

End Class