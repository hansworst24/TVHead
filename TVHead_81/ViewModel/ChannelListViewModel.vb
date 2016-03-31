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
            Return CType(Application.Current, Application).DefaultViewModel
        End Get
    End Property

    Public Async Function RefreshCurrentEvents(Optional c As ChannelViewModel = Nothing) As Task
        WriteToDebug("ChannelListViewModel.RefreshCurrentEvents()", "executed")
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
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
        Dim selectedChannelTag As ChannelTagViewModel = vm.ChannelTags.selectedChannelTag
        Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingChannels"), 1, False, 0)
        If Me._allchannels.Count = 0 Then
            Me._allchannels = (Await LoadAllChannels()).ToList()
        End If
        Await Me.RefreshCurrentEvents()

        Dim newChannels = _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
        Await RunOnUIThread(Sub()
                                items.Clear()
                                For Each channel In newChannels
                                    items.Add(channel)
                                Next
                            End Sub)
        Me.dataLoaded = True
    End Function


End Class