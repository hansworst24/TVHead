Imports GalaSoft.MvvmLight
Imports TVHead_81.ViewModels

Public Class DiskSpaceUpdateViewModel
    Inherits ViewModelBase

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, App).DefaultViewModel
        End Get

    End Property

    Public Property WaitingForUpdate As Boolean
        Get
            Return _WaitingForUpdate
        End Get
        Set(value As Boolean)
            _WaitingForUpdate = value
            RaisePropertyChanged("WaitingForUpdate")
        End Set
    End Property
    Private Property _WaitingForUpdate As Boolean

    Public Property FreeDiskspace As Long
        Get
            Return _FreeDiskspace
        End Get
        Set(value As Long)
            _FreeDiskspace = value
            RaisePropertyChanged("FreeDiskspace")
            RaisePropertyChanged("FreeDiskspaceString")
            RaisePropertyChanged("FreeDiskspacePercentage")
        End Set
    End Property
    Private Property _FreeDiskspace As Long

    Public Property UsedDiskspace As Long
        Get
            Return _UsedDiskspace
        End Get
        Set(value As Long)
            _UsedDiskspace = value
            RaisePropertyChanged("UsedDiskspace")
            RaisePropertyChanged("UsedDiskspaceString")
            RaisePropertyChanged("UsedDiskspacePercentage")
        End Set
    End Property
    Private Property _UsedDiskspace As Long




    Public Property TotalDiskspace As Long
        Get
            Return _TotalDiskspace
        End Get
        Set(value As Long)
            _TotalDiskspace = value
            RaisePropertyChanged("TotalDiskspace")
            RaisePropertyChanged("TotalDiskspaceString")
        End Set
    End Property
    Private Property _TotalDiskspace As Long

    Public ReadOnly Property UsedDiskspaceString As String
        Get
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = UsedDiskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("UsedDiskSpace"), len, sizes(order))
            'Return String.Format("{0:0.##} {1}", len, sizes(order))
        End Get

    End Property


    Public ReadOnly Property FreeDiskspaceString As String
        Get
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = FreeDiskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("FreeDiskSpace"), len, sizes(order))
        End Get

    End Property

    Public ReadOnly Property TotalDiskspaceString As String
        Get
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = TotalDiskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("TotalDiskSpace"), len, sizes(order))
        End Get

    End Property

    Public ReadOnly Property UsedDiskspacePercentage As Double
        Get
            Return (Math.Round(UsedDiskspace / TotalDiskspace, 2))
        End Get
    End Property

    Public ReadOnly Property FreeDiskspacePercentage As Double
        Get
            Return (Math.Round((TotalDiskspace - FreeDiskspace) / TotalDiskspace, 2))
        End Get
    End Property


    Public Sub New(diskupdatemessage As CometMessages.diskspaceUpdate)
        TotalDiskspace = diskupdatemessage.totaldiskspace
        FreeDiskspace = diskupdatemessage.freediskspace
        UsedDiskspace = diskupdatemessage.useddiskspace
        WaitingForUpdate = False
    End Sub

    Public Sub New()
        WaitingForUpdate = True
    End Sub

    Public Sub Update(diskupdate As CometMessages.diskspaceUpdate)
        WriteToDebug("DiskSpaceUpdateViewModel.Update()", "Updated")
        TotalDiskspace = diskupdate.totaldiskspace
        FreeDiskspace = diskupdate.freediskspace
        UsedDiskspace = diskupdate.useddiskspace
        WaitingForUpdate = False
    End Sub
End Class
