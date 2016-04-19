Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command

Public Class ChannelTagViewModel
    Inherits ViewModelBase

    Public ReadOnly Property vm As TVHead_ViewModel
        Get
            Return CType(Application.Current, Application).DefaultViewModel
        End Get

    End Property

    Public Property ChannelTagSelected As RelayCommand
        Get
            Return New RelayCommand(Async Sub()
                                        'WriteToDebug("ChannelTagViewModel.ChannelTagSelected", "start")
                                        'While vm.AppBar.ButtonEnabled.refreshButton = False
                                        '    WriteToDebug("ChanneltagViewModel.ChannelTagSelected()", "Waiting for refresh to finish...")
                                        '    Await Task.Delay(100)
                                        'End While
                                        'vm.ChannelSelected = False
                                        'vm.SelectedChannel = Nothing
                                        'vm.EPGInformationAvailable = False
                                        'vm.ChannelTagFlyoutIsOpen = False
                                        'vm.selectedChannelTag = x
                                        'vm.Channels.dataLoaded = False
                                        'Task.Run(Function() vm.LoadDataAsync())
                                        WriteToDebug("ChannelTagViewModel.ChannelTagSelected", "stop")
                                    End Sub)

        End Get
        Set(value As RelayCommand)
        End Set
    End Property

    Public Property comment As String
    Public Property enabled As Boolean
    Public Property icon As String
    Public Property icon_public_url As String
        Get
            If _icon_public_url = "" Then Return "ms-appx:///Images/tvheadend.png" Else Return _icon_public_url
        End Get
        Set(value As String)
            If Not value Is Nothing And Not value = "/Images/tvheadend.png" Then
                If value.ToUpper().IndexOf("HTTP:/") >= 0 Then _icon_public_url = value Else _icon_public_url = (New TVHead_Settings).GetFullURL() & "/" & value
            Else
                _icon_public_url = ""
            End If
        End Set
    End Property
    Private Property _icon_public_url As String
    Public Property internal As Boolean
    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            RaisePropertyChanged("name")
        End Set
    End Property
    Private Property _name As String
    Public Property numberOfChannels As String
        Get
            Return _numberOfChannels
        End Get
        Set(value As String)
            _numberOfChannels = value
        End Set
    End Property
    Private Property _numberOfChannels As String

    Public Property [private] As Boolean
    Public Property titled_icon As Boolean
    Public Property uuid As String
        Get
            Return _uuid
        End Get
        Set(value As String)
            _uuid = value
            RaisePropertyChanged("uuid")
        End Set
    End Property
    Private Property _uuid As String

    Public Sub New()

    End Sub



    Public Sub New(ChannelTag As tvh40.ChannelTag)
        comment = ChannelTag.comment
        enabled = ChannelTag.enabled
        internal = ChannelTag.internal
        icon = ChannelTag.icon
        name = ChannelTag.name
        uuid = ChannelTag.uuid
    End Sub
End Class