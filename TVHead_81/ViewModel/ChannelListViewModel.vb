Imports GalaSoft.MvvmLight
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports TVHead_81.ViewModels

Public Class ChannelListViewModel
    Inherits ViewModelBase
    Private Property _allchannels As New List(Of ChannelViewModel)

    Public ReadOnly Property items As List(Of ChannelViewModel)
        Get
            Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
            Dim selectedChannelTag As ChannelTagViewModel = vm.ChannelTags.selectedChannelTag
            Return _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
        End Get
    End Property
    Public Property dataLoaded As Boolean

    Public Async Function RefreshCurrentEvents(Optional c As ChannelViewModel = Nothing) As Task
        WriteToDebug("ChannelListViewModel.RefreshCurrentEvents()", "executed")
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
        If Await vm.TVHeadSettings.hasEPGAccess Then
            If c Is Nothing Then
                'Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingChannels"), 1, False, 0)
                'Trigger an update for all channels. Do 1 query to TVH server to retrieve X number of most recent EPG entries, where X is the amount of total channels
                'This should ensure we get the most recent EPG entry for each channel, and avoid hammering the server with individual requests
                Dim updatedEvents As List(Of EPGItemViewModel) = (Await LoadEPGEntry(New ChannelViewModel, False, Me._allchannels.Count)).ToList()
                If Not updatedEvents Is Nothing Then
                    For Each channel In Me._allchannels
                        Dim currentEPGItem As EPGItemViewModel = updatedEvents.Where(Function(x) x.channelUuid = channel.uuid).FirstOrDefault()
                        If Not currentEPGItem Is Nothing Then
                            Await channel.UpdateCurrentEPGItem(currentEPGItem)
                        Else
                            channel.epgitemsAvailable = False
                            Await channel.UpdateCurrentEPGItem(New EPGItemViewModel)
                        End If
                    Next
                End If
            End If
        End If
        vm.Notify.Clear()
    End Function

    Public Async Function ClearChannels() As Task
        If Not items Is Nothing Then
            Await RunOnUIThread(Sub()
                                    items.Clear()
                                End Sub)

        End If
    End Function

    Public Async Function Load() As Task
        WriteToDebug("ChannelListViewModel.Load()", "executed")
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
        Dim selectedChannelTag As ChannelTagViewModel = vm.ChannelTags.selectedChannelTag
        Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingChannels"), 1, False, 0)
        If Me._allchannels.Count = 0 Then
            Me._allchannels = (Await LoadAllChannels()).ToList()
        End If
        Await Me.RefreshCurrentEvents()
        'Await RunOnUIThread(Sub()
        '                        RaisePropertyChanged("items")
        '                    End Sub)


        Dim newChannels = _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
        items.Clear()
        For Each channel In newChannels
            items.Add(channel)
        Next
        Await RunOnUIThread(Sub()

                                RaisePropertyChanged("items")
                            End Sub)
        Me.dataLoaded = True
    End Function



    Public Async Sub Channel_Clicked(sender As Object, e As ItemClickEventArgs)
        WriteToDebug("ChannelListViewModel.ChannelClicked", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel

        Dim clickedChannel As ChannelViewModel = TryCast(e.ClickedItem, ChannelViewModel)
        If Not clickedChannel Is Nothing Then
            For Each c In items
                If c.uuid = clickedChannel.uuid Then c.IsSelected = True Else c.IsSelected = False
            Next

            Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingEPGEntries"), 1, False, 0)
            Await clickedChannel.LoadEPG()
            If Not vm.SelectedChannel Is Nothing Then vm.SelectedChannel.epgitems.ClearAllButCurrent()
            vm.SelectedChannel = clickedChannel
            vm.selectedEPGItem = clickedChannel.currentEPGItem
            vm.Notify.Clear()
            vm.SelectedPivotIndex = 1
        End If
        ' Dim new_selected_channel As ChannelViewModel = TryCast(x, ChannelViewModel)

    End Sub

End Class