
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports TVHead_81.Common
Imports TVHead_81.ViewModels

Public Class RecordContentDialogViewModel
    Inherits ViewModelBase

    Private _epgitem As EPGItemViewModel


    Public ReadOnly Property epgItemTitle As String
        Get
            Return _epgitem.title
        End Get
    End Property


    Public ReadOnly Property RecordingTypes As List(Of String)
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel

            Dim l As New List(Of String)
            l.Add(vm.loader.GetString("Once"))
            If _epgitem.serieslinkId <> 0 Then l.Add(vm.loader.GetString("SeriesRecording")) Else l.Add(vm.loader.GetString("AutoRecording"))
            Return l
        End Get
    End Property

    Public ReadOnly Property Title As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.loader.GetString("RecordingConfigurationHeader")
        End Get
    End Property
    Public Property ShowSeriesButton As Boolean

    Public ReadOnly Property RecordingQuestion As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If ShowSeriesButton Then
                Return vm.loader.GetString("RecordingProposeSeriesRecordingContent")
            Else
                Return vm.loader.GetString("RecordingProposeAutoRecordingContent")
            End If

        End Get
    End Property

    Public ReadOnly Property OKButtonText As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.loader.GetString("OK")
        End Get
    End Property
    Public ReadOnly Property CancelButtonText As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.loader.GetString("Cancel")
        End Get
    End Property
    Public ReadOnly Property dvrconfigs As List(Of DVRConfigViewModel)
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Return vm.DVRConfigs.items.ToList()
        End Get
    End Property
    Public Property selectedDVRConfig As DVRConfigViewModel

    Public Property SingleRecording As Boolean
    Public Property SeriesRecording As Boolean
    Public Property AutoRecording As Boolean

    Public Property selectedDVRConfigIndex As Integer


    Public ReadOnly Property selectRecordingType As RelayCommand(Of Object)
        Get
            Return New RelayCommand(Of Object)(Sub(x)
                                                   Dim cbox As ComboBox = TryCast(x, ComboBox)
                                                   If Not cbox Is Nothing Then
                                                       If cbox.SelectedIndex = 0 Then
                                                           SingleRecording = True
                                                           SeriesRecording = False
                                                           AutoRecording = False
                                                       Else
                                                           If _epgitem.serieslinkId <> 0 Then
                                                               SingleRecording = False
                                                               SeriesRecording = True
                                                               AutoRecording = False
                                                           Else
                                                               SingleRecording = False
                                                               SeriesRecording = False
                                                               AutoRecording = True
                                                           End If
                                                       End If
                                                   End If
                                               End Sub)
        End Get
    End Property

    Public Property selectDVRConfig As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        'Dim i As DVRConfigViewModel = TryCast(x, DVRConfigViewModel)
                                        'If Not i Is Nothing Then
                                        '    selectedDVRConfig = i
                                        'End If
                                    End Sub)
        End Get
        Set(value As RelayCommand)

        End Set

    End Property

    Public Sub New(epgitem As EPGItemViewModel)
        _epgitem = epgitem
        SingleRecording = True
    End Sub

End Class