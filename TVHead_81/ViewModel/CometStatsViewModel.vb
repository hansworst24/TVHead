
Imports GalaSoft.MvvmLight
Imports Newtonsoft.Json

Public Class CometStatsViewModel
    Inherits ViewModelBase
    Public Property _intDVRCreate As Integer
    Public Property intDVRCreate As Integer
        Get
            Return _intDVRCreate
        End Get
        Set(value As Integer)
            _intDVRCreate = value
            RaisePropertyChanged("intDVRCreate")
        End Set
    End Property
    Private Property _intDVRChange As Integer
    Public Property intDVRChange As Integer
        Get
            Return _intDVRChange
        End Get
        Set(value As Integer)
            _intDVRChange = value
            RaisePropertyChanged("intDVRChange")
        End Set
    End Property
    Private Property _intDVRUpdate As Integer
    Public Property intDVRUpdate As Integer
        Get
            Return _intDVRUpdate
        End Get
        Set(value As Integer)
            _intDVRUpdate = value
            RaisePropertyChanged("intDVRUpdate")
        End Set
    End Property
    Private Property _intDVRDelete As Integer
    Public Property intDVRDelete As Integer
        Get
            Return _intDVRDelete
        End Get
        Set(value As Integer)
            _intDVRDelete = value
            RaisePropertyChanged("intDVRDelete")
        End Set
    End Property

    Private Property _intEPGCreate As Integer
    Public Property intEPGCreate As Integer
        Get
            Return _intEPGCreate
        End Get
        Set(value As Integer)
            _intEPGCreate = value
            RaisePropertyChanged("intEPGCreate")
        End Set
    End Property
    Private Property _intEPGChange As Integer
    Public Property intEPGChange As Integer
        Get
            Return _intEPGChange
        End Get
        Set(value As Integer)
            _intEPGChange = value
            RaisePropertyChanged("intEPGChange")
        End Set
    End Property
    Private Property _intEPGUpdate As Integer
    Public Property intEPGUpdate As Integer
        Get
            Return _intEPGUpdate
        End Get
        Set(value As Integer)
            _intEPGUpdate = value
            RaisePropertyChanged("intEPGUpdate")
        End Set
    End Property
    Private Property _intEPGDelete As Integer
    Public Property intEPGDelete As Integer
        Get
            Return _intEPGDelete
        End Get
        Set(value As Integer)
            _intEPGDelete = value
            RaisePropertyChanged("intEPGDelete")
        End Set
    End Property
    Private Property _intEPGDVRUpdate As Integer
    Public Property intEPGDVRUpdate As Integer
        Get
            Return _intEPGDVRUpdate
        End Get
        Set(value As Integer)
            _intEPGDVRUpdate = value
            RaisePropertyChanged("intEPGDVRUpdate")
        End Set
    End Property
    Private Property _intEPGDVRDelete As Integer
    Public Property intEPGDVRDelete As Integer
        Get
            Return _intEPGDVRDelete
        End Get
        Set(value As Integer)
            _intEPGDVRDelete = value
            RaisePropertyChanged("intEPGDVRDelete")
        End Set
    End Property

    Private Property _intDVRAutoRecCreate As Integer
    Public Property intDVRAutoRecCreate As Integer
        Get
            Return _intDVRAutoRecCreate
        End Get
        Set(value As Integer)
            _intDVRAutoRecCreate = value
            RaisePropertyChanged("intDVRAutoRecCreate")
        End Set
    End Property

    Private Property _intDVRAutoRecChange As Integer
    Public Property intDVRAutoRecChange As Integer
        Get
            Return _intDVRAutoRecChange
        End Get
        Set(value As Integer)
            _intDVRAutoRecChange = value
            RaisePropertyChanged("intDVRAutoRecChange")
        End Set
    End Property

    Private Property _intDVRAutoRecUpdate As Integer
    Public Property intDVRAutoRecUpdate As Integer
        Get
            Return _intDVRAutoRecUpdate
        End Get
        Set(value As Integer)
            _intDVRAutoRecUpdate = value
            RaisePropertyChanged("intDVRAutoRecUpdate")
        End Set
    End Property

    Private Property _intDVRAutoRecDelete As Integer
    Public Property intDVRAutoRecDelete As Integer
        Get
            Return _intDVRAutoRecDelete
        End Get
        Set(value As Integer)
            _intDVRAutoRecDelete = value
            RaisePropertyChanged("intDVRAutoRecDelete")
        End Set
    End Property

    Public DiskSpace As New DiskSpaceUpdateViewModel

    ''' <summary>
    ''' Increases the counter of a received Comet item
    ''' </summary>
    ''' <param name="s"></param>
    Public Async Sub AddComet(s As String)
        RunOnUIThread(Sub()
                          Select Case s
                              Case "epgcreate" : intEPGCreate += 1
                              Case "epgchange" : intEPGChange += 1
                              Case "epgupdate" : intEPGUpdate += 1
                              Case "epgdelete" : intEPGDelete += 1
                              Case "epgdvrupdate" : intEPGDVRUpdate += 1
                              Case "epgdrvdelete" : intEPGDVRDelete += 1
                              Case "dvrcreate" : intDVRCreate += 1
                              Case "dvrchange" : intDVRChange += 1
                              Case "dvrupdate" : intDVRUpdate += 1
                              Case "dvrdelete" : intDVRDelete += 1
                              Case "dvrautorecchange" : intDVRAutoRecChange += 1
                              Case "dvrautoreccreate" : intDVRAutoRecCreate += 1
                              Case "dvrautorecdelete" : intDVRAutoRecDelete += 1
                              Case "dvrautorecupdate" : intDVRAutoRecUpdate += 1
                          End Select
                      End Sub)
    End Sub

    Public Sub New()
    End Sub
End Class
