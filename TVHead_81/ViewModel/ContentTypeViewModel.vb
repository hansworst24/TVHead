Imports Newtonsoft.Json

Public Class ContentTypeListViewModel
    Public Property items As New List(Of ContentTypeViewModel)
    Public Property allitems As New List(Of ContentTypeViewModel)
    Public Property dataLoaded As Boolean

    Public Async Function Load() As Task
        WriteToDebug("ContentTypeListViewModel.Load()", "executed")
        Dim response As New List(Of ContentTypeViewModel)
        Dim json_allitems As String
        Dim json_items As String
        Try
            json_allitems = Await (Await (New Downloader).DownloadJSON((New api40).apiGetContentTypes(True))).Content.ReadAsStringAsync
            json_items = Await (Await (New Downloader).DownloadJSON((New api40).apiGetContentTypes(False))).Content.ReadAsStringAsync
        Catch ex As Exception
            WriteToDebug("ContentTypeListViewModel.Load()", "stop-error")
            Return
        End Try
        If Not json_allitems = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of TVHGenreList)(json_allitems)
            RunOnUIThread(Sub()
                              For Each f In deserialized.entries
                                  'Small hack to ensure we avoid having ContentType 0 in the list
                                  If f.key <> 0 Then
                                      allitems.Add(New ContentTypeViewModel(f))
                                  End If
                              Next
                          End Sub)
        End If
        If Not json_items = "" Then
            Dim deserialized = JsonConvert.DeserializeObject(Of TVHGenreList)(json_items)
            RunOnUIThread(Sub()
                              For Each f In deserialized.entries
                                  'Small hack to ensure we avoid having ContentType 0 in the list
                                  If f.key <> 0 Then
                                      items.Add(New ContentTypeViewModel(f))
                                  End If
                              Next
                          End Sub)
        End If
        dataLoaded = True
    End Function
End Class

Public Class ContentTypeViewModel
    Public Property uuid As Integer
    Public Property name As String

    Public Sub New(c As TVHGenre)
        uuid = c.key
        name = c.val
    End Sub
End Class