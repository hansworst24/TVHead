
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports TVHead_81.Common
Imports TVHead_81.ViewModels

Public Class RecordContentDialogViewModel
    Inherits ViewModelBase
    Public ReadOnly Property Title As String
        Get
            Return vm.loader.GetString("RecordingConfigurationHeader")
        End Get
    End Property
    Public Property ShowSeriesButton As Boolean

    Public ReadOnly Property RecordingQuestion As String
        Get
            If ShowSeriesButton Then
                Return vm.loader.GetString("RecordingProposeSeriesRecordingContent")
            Else
                Return vm.loader.GetString("RecordingProposeAutoRecordingContent")
            End If

        End Get
    End Property

    Public ReadOnly Property SingleRecordingButtonText As String
        Get
            Return vm.loader.GetString("Once")
        End Get
    End Property
    Public Property SeriesRecordingButtonVisibility As String
        Get
            Return _SeriesRecordingButtonVisibility
        End Get
        Set(value As String)
            _SeriesRecordingButtonVisibility = value
            RaisePropertyChanged("SeriesRecordingButtonVisibility")
        End Set
    End Property
    Private Property _SeriesRecordingButtonVisibility As String
    Public ReadOnly Property SeriesRecordingButtonText As String
        Get
            Return vm.loader.GetString("SeriesRecording")
        End Get
    End Property
    Public Property AutoRecordingButtonVisibility As String
        Get
            Return _AutoRecordingButtonVisibility
        End Get
        Set(value As String)
            _AutoRecordingButtonVisibility = value
            RaisePropertyChanged("AutoRecordingButtonVisibility")
        End Set
    End Property
    Private Property _AutoRecordingButtonVisibility As String
    Public ReadOnly Property AutoRecordingButtonText As String
        Get
            Return vm.loader.GetString("AutoRecording")
        End Get

    End Property
    Public ReadOnly Property OKButtonText As String
        Get
            Return vm.loader.GetString("OK")
        End Get
    End Property
    Public ReadOnly Property CancelButtonText As String
        Get
            Return vm.loader.GetString("Cancel")
        End Get
    End Property
    Public Property dvrconfigs As List(Of DVRConfigViewModel)
    Public Property selectedDVRConfig As DVRConfigViewModel
    Public Property SingleRecording As Boolean
        Get
            Return _SingleRecording
        End Get
        Set(value As Boolean)
            _SingleRecording = value
            RaisePropertyChanged("SingleRecording")
        End Set
    End Property
    Private Property _SingleRecording As Boolean
    Public Property SeriesRecording As Boolean
        Get
            Return _SeriesRecording
        End Get
        Set(value As Boolean)
            _SeriesRecording = value
            RaisePropertyChanged("SeriesRecording")
        End Set
    End Property
    Private Property _SeriesRecording As Boolean
    Public Property AutoRecording As Boolean
        Get
            Return _AutoRecording
        End Get
        Set(value As Boolean)
            _AutoRecording = value
            RaisePropertyChanged("AutoRecording")
        End Set
    End Property
    Private Property _AutoRecording As Boolean

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
        End Get

    End Property

    Public Property selectedDVRConfigIndex As Integer

    Public Property selectSingleRecording As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        SingleRecording = True
                                        SeriesRecording = False
                                        AutoRecording = False
                                    End Sub)
        End Get
        Set(value As RelayCommand)

        End Set
    End Property

    Public Property selectSeriesRecording As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        SeriesRecording = True
                                        SingleRecording = False
                                        AutoRecording = False
                                    End Sub)
        End Get
        Set(value As RelayCommand)

        End Set
    End Property

    Public Property selectAutoRecording As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        AutoRecording = True
                                        SeriesRecording = False
                                        SingleRecording = False
                                    End Sub)
        End Get
        Set(value As RelayCommand)

        End Set
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

    Public Sub New()
    End Sub

    Public Sub Init()
        SingleRecording = True
        If ShowSeriesButton Then SeriesRecordingButtonVisibility = "Visible" Else SeriesRecordingButtonVisibility = "Collapsed"
        If ShowSeriesButton Then AutoRecordingButtonVisibility = "Collapsed" Else AutoRecordingButtonVisibility = "Visible"
        If Not dvrconfigs.Count = 0 Then selectedDVRConfig = dvrconfigs(0)
    End Sub
End Class