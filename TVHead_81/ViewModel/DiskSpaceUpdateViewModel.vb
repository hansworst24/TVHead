Imports GalaSoft.MvvmLight

Public Class DiskSpaceUpdateViewModel
    Inherits ViewModelBase

    Private _diskspaceupdate As CometMessages.diskspaceUpdate

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
            Return _diskspaceupdate.freediskspace
        End Get
        Set(value As Long)
            _diskspaceupdate.freediskspace = value
            RaisePropertyChanged("FreeDiskspace")
            RaisePropertyChanged("FreeDiskspaceString")
            RaisePropertyChanged("FreeDiskspacePercentage")
        End Set
    End Property
    Private Property _FreeDiskspace As Long
    Public Property UsedDiskspace As Long
        Get
            Return _diskspaceupdate.useddiskspace
        End Get
        Set(value As Long)
            _diskspaceupdate.useddiskspace = value
            RaisePropertyChanged("UsedDiskspace")
            RaisePropertyChanged("UsedDiskspaceString")
            RaisePropertyChanged("UsedDiskspacePercentage")
        End Set
    End Property
    Private Property _UsedDiskspace As Long
    Public Property TotalDiskspace As Long
        Get
            Return _diskspaceupdate.totaldiskspace
        End Get
        Set(value As Long)
            _diskspaceupdate.totaldiskspace = value
            RaisePropertyChanged("TotalDiskspace")
            RaisePropertyChanged("TotalDiskspaceString")
        End Set
    End Property
    Private Property _TotalDiskspace As Long
    Public ReadOnly Property UsedDiskspaceString As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = _diskspaceupdate.useddiskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("UsedDiskSpace"), len, sizes(order))
        End Get

    End Property
    Public ReadOnly Property FreeDiskspaceString As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = _diskspaceupdate.freediskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("FreeDiskSpace"), len, sizes(order))
        End Get

    End Property
    Public ReadOnly Property TotalDiskspaceString As String
        Get
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            Dim sizes As String() = {"B", "KiB", "MiB", "GiB", "TiB", "PiB"}
            Dim order As Integer = 0
            Dim len As Long = _diskspaceupdate.totaldiskspace
            While len > 1024 AndAlso order + 1 < sizes.Length
                order = order + 1
                len = len / 1024
            End While
            Return String.Format(vm.loader.GetString("TotalDiskSpace"), len, sizes(order))
        End Get

    End Property
    Public ReadOnly Property UsedDiskspacePercentage As Double
        Get
            Return (Math.Round(_diskspaceupdate.useddiskspace / _diskspaceupdate.totaldiskspace, 2))
        End Get
    End Property
    Public ReadOnly Property FreeDiskspacePercentage As Double
        Get
            Return (Math.Round((_diskspaceupdate.totaldiskspace - _diskspaceupdate.freediskspace) / _diskspaceupdate.totaldiskspace, 2))
        End Get
    End Property

    Public Sub New(diskupdatemessage As CometMessages.diskspaceUpdate)
        _diskspaceupdate = diskupdatemessage
    End Sub
    Public Sub New()
        WaitingForUpdate = True
    End Sub
    Public Sub Update(diskupdate As CometMessages.diskspaceUpdate)
        WriteToDebug("DiskSpaceUpdateViewModel.Update()", "Updated")
        _diskspaceupdate = diskupdate
        WaitingForUpdate = False
    End Sub
End Class
