Public Class DVRConfigListViewModel
    Public Property items As New List(Of DVRConfigViewModel)
    Public Property dataLoaded As Boolean

    Public Async Function Load() As Task
        RunOnUIThread(Async Sub()
                          items.Clear()
                          items = (Await LoadDVRConfigs()).ToList()
                          dataLoaded = True
                      End Sub)
    End Function
End Class


Public Class DVRConfigViewModel
    Public Property name As String
    Public Property identifier As String

    Public Sub New(dvrconfig As TVHDVRConfig)
        name = dvrconfig.val
        identifier = dvrconfig.key
    End Sub



    Public Sub New()

    End Sub
End Class