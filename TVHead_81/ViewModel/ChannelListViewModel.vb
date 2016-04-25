Imports GalaSoft.MvvmLight

Public Class ChannelListViewModel
    Inherits ViewModelBase
    Public Property _allchannels As New List(Of ChannelViewModel)

    Public ReadOnly Property items As List(Of ChannelViewModel)
        Get
            Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
            Dim selectedChannelTag As ChannelTagViewModel = vm.ChannelTags.selectedChannelTag
            If Not selectedChannelTag Is Nothing Then
                Return _allchannels.OrderBy(Function(x) x.number).Where(Function(x) x.tags.Contains(selectedChannelTag.uuid)).ToList()
            Else
                Return Nothing
            End If
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
                    Dim selectedChannels As List(Of ChannelViewModel) = Me._allchannels.Where(Function(x) x.tags.Contains(vm.ChannelTags.selectedChannelTag.uuid)).OrderBy(Function(y) y.number).ToList()
                    For Each channel In selectedChannels
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
            RunOnUIThread(Sub()
                              items.Clear()
                          End Sub)

        End If
    End Function

    Public Async Function Load() As Task
        WriteToDebug("ChannelListViewModel.Load()", "executed")
        Dim vm As TVHead_ViewModel = TryCast(CType(Application.Current, Application).DefaultViewModel, TVHead_ViewModel)
        Await vm.Notify.Update(False, vm.loader.GetString("status_RefreshingChannels"), 1, False, 0)
        Dim selectedChannelTag As ChannelTagViewModel = vm.ChannelTags.selectedChannelTag
        If Me._allchannels.Count = 0 Then
            Me._allchannels = (Await LoadAllChannels()).ToList()
        End If
        RaisePropertyChanged("items")
        Await Me.RefreshCurrentEvents()
        Me.dataLoaded = True
    End Function

    ''' <summary>
    ''' Sets a channel within the ChannelListViewModel.items to selected. If the app is not running on a widescreen device, it will also reverse the IsExpanded property
    ''' so that a ChannelViewModel can be expanded or collapsed by tapping on it multiple times
    ''' </summary>
    ''' <param name="c"></param>
    Public Sub SelectChannel(c As ChannelViewModel)
        For Each channel In items
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If channel.uuid = c.uuid Then
                channel.IsSelected = True
                If Not vm.IsRunningOnWideScreen Then channel.IsExpanded = Not channel.IsExpanded
            Else
                channel.IsSelected = False
                channel.IsExpanded = False
                If vm.IsRunningOnWideScreen Then channel.epgitems.ClearAllButCurrent()
            End If
        Next
    End Sub

    Public Sub ClearSelection()
        For Each c In items
            c.IsExpanded = False
            c.IsSelected = False
        Next
    End Sub


    ''' <summary>
    ''' Used on touch devices. Triggers the loading of the EPG content for this channel. Method is only executed when the HoldingState = Started 
    ''' and we're not running on a widescreen device
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Async Sub Channel_Holding(sender As Object, e As HoldingRoutedEventArgs)
        WriteToDebug("ChannelListViewModel.Channel_Holding", "executed")
        Dim source As Object = e.OriginalSource
        Dim ChannelHolded As ChannelViewModel = TryCast(source.DataContext, ChannelViewModel)
        If Not ChannelHolded Is Nothing Then
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If e.HoldingState = Windows.UI.Input.HoldingState.Started AndAlso Not vm.IsRunningOnWideScreen Then
                SelectChannel(ChannelHolded)
                Await ChannelHolded.LoadEPG()
                vm.SelectedChannel = ChannelHolded
                vm.selectedEPGItem = ChannelHolded.epgitems.currentEPGItem
                vm.SelectedPivotIndex = 1
            End If
        End If
    End Sub


    ''' <summary>
    ''' When the channel is clicked or tapped, this will select the channel and, if on widescreen, load the channel's EPG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Async Sub Channel_Clicked(sender As Object, e As ItemClickEventArgs)
        WriteToDebug("ChannelListViewModel.ChannelClicked", "executed")
        Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        Dim clickedChannel As ChannelViewModel = TryCast(e.ClickedItem, ChannelViewModel)
        SelectChannel(clickedChannel)
        If vm.IsRunningOnWideScreen Then Await clickedChannel.LoadEPG()
        If vm.IsRunningOnWideScreen Then vm.SelectedChannel = clickedChannel
        vm.selectedEPGItem = clickedChannel.epgitems.currentEPGItem
    End Sub

End Class