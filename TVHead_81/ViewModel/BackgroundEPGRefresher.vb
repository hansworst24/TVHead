Imports System.Threading

Public Class BackgroundEPGRefresher
    Public ct As CancellationToken
    Public tokenSource As New CancellationTokenSource()
    Public EPGRefresher As Task


    Public Async Sub StartRefresh()
        WriteToDebug("TVHead_ViewModel.StartRefresh()", "")
        If EPGRefresher Is Nothing OrElse EPGRefresher.IsCompleted Then
            tokenSource = New CancellationTokenSource
            ct = tokenSource.Token
            EPGRefresher = Await Task.Factory.StartNew(Function() EPG_Updater(ct), ct)
        End If
    End Sub

    Public Sub StopRefresh()
        If ct.CanBeCanceled Then
            tokenSource.Cancel()
        End If
        WriteToDebug("TVHead_ViewModel.StopRefresh()", "")
    End Sub

    ''' <summary>
    ''' Refreshes the current EPG item for each channel within the Channels list. If a channel is selected, it will also refresh the EPG information for that channel
    ''' </summary>
    ''' <param name="ct"></param>
    ''' <returns></returns>
    Public Async Function EPG_Updater(ct As CancellationToken) As Task
        'Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
        'While Not ct.IsCancellationRequested
        '    Dim s As New Stopwatch
        '    s.Start()
        '    If vm.TVHeadSettings.LongPollingEnabled Then
        '        WriteToDebug("TVHead_ViewModel.epgupdate()", "executed")
        '        If Await vm.TVHeadSettings.hasEPGAccess Then
        '            For Each c In vm.Channels.items.Where(Function(x) x.epgitemsAvailable = True)
        '                'Await c.epgitems.UpdateCurrentEPGItem(Nothing, True)
        '            Next
        '            If Not vm.SelectedChannel Is Nothing Then
        '                Await vm.SelectedChannel.RefreshEPG(False)
        '            End If
        '        End If
        '    Else
        '        WriteToDebug("TVHead_ViewModel.epgupdate()", "long polling disabled")
        '    End If

        '    If s.ElapsedMilliseconds < 5000 Then
        '        Await Task.Delay(5000 - s.ElapsedMilliseconds)
        '    End If
        'End While
    End Function
End Class
