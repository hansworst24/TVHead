Imports Windows.UI.Popups
Imports GalaSoft.MvvmLight
Imports System.Threading
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports Newtonsoft.Json
Imports Windows.Web.Http
Imports GalaSoft.MvvmLight.Command

Namespace ViewModels



    Public Class ToastListViewModel
        Public Property Messages As ObservableCollection(Of ToastMessageViewModel)

        Public Sub New()
            Messages = New ObservableCollection(Of ToastMessageViewModel)

        End Sub

        Public Async Sub AddMessage(msg As ToastMessageViewModel)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             Messages.Insert(0, msg)
                                                                                                         End Sub)
            Await Task.Delay(New TimeSpan(0, 0, msg.secondsToShow))
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             msg.isGoing = True
                                                                                                         End Sub)

            Await Task.Delay(1000)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             Messages.Remove(msg)
                                                                                                         End Sub)
        End Sub

    End Class

    Public Class ToastMessageViewModel
        Inherits ViewModelBase
        Public Property msg As String
        Public Property isGoing As Boolean
            Get
                Return _isGoing
            End Get
            Set(value As Boolean)
                _isGoing = value
                RaisePropertyChanged("isGoing")
            End Set
        End Property
        Private Property _isGoing As Boolean

        Public Property isError As Boolean
            Get
                Return _isError
            End Get
            Set(value As Boolean)
                _isError = value
                RaisePropertyChanged("isError")
            End Set
        End Property
        Private Property _isError As Boolean

        Public Property secondsToShow As Integer
    End Class




    Public Class Language
        Public Property code As String
        Public Property val As String
    End Class

    Public Class LanguageList
        Public Property languages As New List(Of Language)
        Private ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, Application).DefaultViewModel
            End Get

        End Property

        Public Sub New()
            Dim myRegion = New Windows.Globalization.GeographicRegion


            languages.Add(New Language With {.code = "af", .val = "Afrikaans"})
            languages.Add(New Language With {.code = "ak", .val = "Akan"})
            languages.Add(New Language With {.code = "sq", .val = "Albanian"})
            languages.Add(New Language With {.code = "am", .val = "Amharic"})
            languages.Add(New Language With {.code = "ar", .val = "Arabic"})
            languages.Add(New Language With {.code = "an", .val = "Aragonese"})
            languages.Add(New Language With {.code = "hy", .val = "Armenian"})
            languages.Add(New Language With {.code = "as", .val = "Assamese"})
            languages.Add(New Language With {.code = "av", .val = "Avaric"})
            languages.Add(New Language With {.code = "ae", .val = "Avestan"})
            languages.Add(New Language With {.code = "ay", .val = "Aymara"})
            languages.Add(New Language With {.code = "az", .val = "Azerbaijani"})
            languages.Add(New Language With {.code = "bm", .val = "Bambara"})
            languages.Add(New Language With {.code = "ba", .val = "Bashkir"})
            languages.Add(New Language With {.code = "eu", .val = "Basque"})
            languages.Add(New Language With {.code = "be", .val = "Belarusian"})
            languages.Add(New Language With {.code = "bn", .val = "Bengali, Bangla"})
            languages.Add(New Language With {.code = "bh", .val = "Bihari"})
            languages.Add(New Language With {.code = "bi", .val = "Bislama"})
            languages.Add(New Language With {.code = "bs", .val = "Bosnian"})
            languages.Add(New Language With {.code = "br", .val = "Breton"})
            languages.Add(New Language With {.code = "bg", .val = "Bulgarian"})
            languages.Add(New Language With {.code = "my", .val = "Burmese"})
            languages.Add(New Language With {.code = "ca", .val = "Catalan"})
            languages.Add(New Language With {.code = "ch", .val = "Chamorro"})
            languages.Add(New Language With {.code = "ce", .val = "Chechen"})
            languages.Add(New Language With {.code = "ny", .val = "Chichewa, Chewa, Nyanja"})
            languages.Add(New Language With {.code = "zh", .val = "Chinese"})
            languages.Add(New Language With {.code = "cv", .val = "Chuvash"})
            languages.Add(New Language With {.code = "kw", .val = "Cornish"})
            languages.Add(New Language With {.code = "co", .val = "Corsican"})
            languages.Add(New Language With {.code = "cr", .val = "Cree"})
            languages.Add(New Language With {.code = "hr", .val = "Croatian"})
            languages.Add(New Language With {.code = "cs", .val = "Czech"})
            languages.Add(New Language With {.code = "da", .val = "Danish"})
            languages.Add(New Language With {.code = "dv", .val = "Divehi, Dhivehi, Maldivian"})
            languages.Add(New Language With {.code = "nl", .val = "Dutch"})
            languages.Add(New Language With {.code = "dz", .val = "Dzongkha"})
            languages.Add(New Language With {.code = "en", .val = "English"})
            languages.Add(New Language With {.code = "eo", .val = "Esperanto"})
            languages.Add(New Language With {.code = "et", .val = "Estonian"})
            languages.Add(New Language With {.code = "ee", .val = "Ewe"})
            languages.Add(New Language With {.code = "fo", .val = "Faroese"})
            languages.Add(New Language With {.code = "fj", .val = "Fijian"})
            languages.Add(New Language With {.code = "fi", .val = "Finnish"})
            languages.Add(New Language With {.code = "fr", .val = "French"})
            languages.Add(New Language With {.code = "ff", .val = "Fula, Fulah, Pulaar, Pular"})
            languages.Add(New Language With {.code = "gl", .val = "Galician"})
            languages.Add(New Language With {.code = "ka", .val = "Georgian"})
            languages.Add(New Language With {.code = "de", .val = "German"})
            languages.Add(New Language With {.code = "el", .val = "Greek (modern)"})
            languages.Add(New Language With {.code = "gn", .val = "Guaraní"})
            languages.Add(New Language With {.code = "gu", .val = "Gujarati"})
            languages.Add(New Language With {.code = "ht", .val = "Haitian, Haitian Creole"})
            languages.Add(New Language With {.code = "ha", .val = "Hausa"})
            languages.Add(New Language With {.code = "he", .val = "Hebrew (modern)"})
            languages.Add(New Language With {.code = "hz", .val = "Herero"})
            languages.Add(New Language With {.code = "hi", .val = "Hindi"})
            languages.Add(New Language With {.code = "ho", .val = "Hiri Motu"})
            languages.Add(New Language With {.code = "hu", .val = "Hungarian"})
            languages.Add(New Language With {.code = "ia", .val = "Interlingua"})
            languages.Add(New Language With {.code = "id", .val = "Indonesian"})
            languages.Add(New Language With {.code = "ie", .val = "Interlingue"})
            languages.Add(New Language With {.code = "ga", .val = "Irish"})
            languages.Add(New Language With {.code = "ig", .val = "Igbo"})
            languages.Add(New Language With {.code = "ik", .val = "Inupiaq"})
            languages.Add(New Language With {.code = "io", .val = "Ido"})
            languages.Add(New Language With {.code = "is", .val = "Icelandic"})
            languages.Add(New Language With {.code = "it", .val = "Italian"})
            languages.Add(New Language With {.code = "iu", .val = "Inuktitut"})
            languages.Add(New Language With {.code = "ja", .val = "Japanese"})
            languages.Add(New Language With {.code = "jv", .val = "Javanese"})
            languages.Add(New Language With {.code = "kl", .val = "Kalaallisut, Greenlandic"})
            languages.Add(New Language With {.code = "kn", .val = "Kannada"})
            languages.Add(New Language With {.code = "kr", .val = "Kanuri"})
            languages.Add(New Language With {.code = "ks", .val = "Kashmiri"})
            languages.Add(New Language With {.code = "kk", .val = "Kazakh"})
            languages.Add(New Language With {.code = "km", .val = "Khmer"})
            languages.Add(New Language With {.code = "ki", .val = "Kikuyu, Gikuyu"})
            languages.Add(New Language With {.code = "rw", .val = "Kinyarwanda"})
            languages.Add(New Language With {.code = "ky", .val = "Kyrgyz"})
            languages.Add(New Language With {.code = "kv", .val = "Komi"})
            languages.Add(New Language With {.code = "kg", .val = "Kongo"})
            languages.Add(New Language With {.code = "ko", .val = "Korean"})
            languages.Add(New Language With {.code = "ku", .val = "Kurdish"})
            languages.Add(New Language With {.code = "kj", .val = "Kwanyama, Kuanyama"})
            languages.Add(New Language With {.code = "la", .val = "Latin"})
            languages.Add(New Language With {.code = "lb", .val = "Luxembourgish, Letzeburgesch"})
            languages.Add(New Language With {.code = "lg", .val = "Ganda"})
            languages.Add(New Language With {.code = "li", .val = "Limburgish, Limburgan, Limburger"})
            languages.Add(New Language With {.code = "ln", .val = "Lingala"})
            languages.Add(New Language With {.code = "lo", .val = "Lao"})
            languages.Add(New Language With {.code = "lt", .val = "Lithuanian"})
            languages.Add(New Language With {.code = "lu", .val = "Luba-Katanga"})
            languages.Add(New Language With {.code = "lv", .val = "Latvian"})
            languages.Add(New Language With {.code = "gv", .val = "Manx"})
            languages.Add(New Language With {.code = "mk", .val = "Macedonian"})
            languages.Add(New Language With {.code = "mg", .val = "Malagasy"})
            languages.Add(New Language With {.code = "ms", .val = "Malay"})
            languages.Add(New Language With {.code = "ml", .val = "Malayalam"})
            languages.Add(New Language With {.code = "mt", .val = "Maltese"})
            languages.Add(New Language With {.code = "mi", .val = "Māori"})
            languages.Add(New Language With {.code = "mr", .val = "Marathi (Marāṭhī)"})
            languages.Add(New Language With {.code = "mh", .val = "Marshallese"})
            languages.Add(New Language With {.code = "mn", .val = "Mongolian"})
            languages.Add(New Language With {.code = "na", .val = "Nauru"})
            languages.Add(New Language With {.code = "nv", .val = "Navajo, Navaho"})
            languages.Add(New Language With {.code = "nd", .val = "Northern Ndebele"})
            languages.Add(New Language With {.code = "ne", .val = "Nepali"})
            languages.Add(New Language With {.code = "ng", .val = "Ndonga"})
            languages.Add(New Language With {.code = "nb", .val = "Norwegian Bokmål"})
            languages.Add(New Language With {.code = "nn", .val = "Norwegian Nynorsk"})
            languages.Add(New Language With {.code = "no", .val = "Norwegian"})
            languages.Add(New Language With {.code = "ii", .val = "Nuosu"})
            languages.Add(New Language With {.code = "nr", .val = "Southern Ndebele"})
            languages.Add(New Language With {.code = "oc", .val = "Occitan"})
            languages.Add(New Language With {.code = "oj", .val = "Ojibwe, Ojibwa"})
            languages.Add(New Language With {.code = "cu", .val = "Old Church Slavonic, Church Slavonic, Old Bulgarian"})
            languages.Add(New Language With {.code = "om", .val = "Oromo"})
            languages.Add(New Language With {.code = "or", .val = "Oriya"})
            languages.Add(New Language With {.code = "os", .val = "Ossetian, Ossetic"})
            languages.Add(New Language With {.code = "pa", .val = "Panjabi, Punjabi"})
            languages.Add(New Language With {.code = "pi", .val = "Pāli"})
            languages.Add(New Language With {.code = "fa", .val = "Persian (Farsi)"})
            languages.Add(New Language With {.code = "pl", .val = "Polish"})
            languages.Add(New Language With {.code = "ps", .val = "Pashto, Pushto"})
            languages.Add(New Language With {.code = "pt", .val = "Portuguese"})
            languages.Add(New Language With {.code = "qu", .val = "Quechua"})
            languages.Add(New Language With {.code = "rm", .val = "Romansh"})
            languages.Add(New Language With {.code = "rn", .val = "Kirundi"})
            languages.Add(New Language With {.code = "ro", .val = "Romanian"})
            languages.Add(New Language With {.code = "ru", .val = "Russian"})
            languages.Add(New Language With {.code = "sa", .val = "Sanskrit (Saṁskṛta)"})
            languages.Add(New Language With {.code = "sc", .val = "Sardinian"})
            languages.Add(New Language With {.code = "sd", .val = "Sindhi"})
            languages.Add(New Language With {.code = "se", .val = "Northern Sami"})
            languages.Add(New Language With {.code = "sm", .val = "Samoan"})
            languages.Add(New Language With {.code = "sg", .val = "Sango"})
            languages.Add(New Language With {.code = "sr", .val = "Serbian"})
            languages.Add(New Language With {.code = "gd", .val = "Scottish Gaelic, Gaelic"})
            languages.Add(New Language With {.code = "sn", .val = "Shona"})
            languages.Add(New Language With {.code = "si", .val = "Sinhala, Sinhalese"})
            languages.Add(New Language With {.code = "sk", .val = "Slovak"})
            languages.Add(New Language With {.code = "sl", .val = "Slovene"})
            languages.Add(New Language With {.code = "so", .val = "Somali"})
            languages.Add(New Language With {.code = "st", .val = "Southern Sotho"})
            languages.Add(New Language With {.code = "es", .val = "Spanish"})
            languages.Add(New Language With {.code = "su", .val = "Sundanese"})
            languages.Add(New Language With {.code = "sw", .val = "Swahili"})
            languages.Add(New Language With {.code = "ss", .val = "Swati"})
            languages.Add(New Language With {.code = "sv", .val = "Swedish"})
            languages.Add(New Language With {.code = "ta", .val = "Tamil"})
            languages.Add(New Language With {.code = "te", .val = "Telugu"})
            languages.Add(New Language With {.code = "tg", .val = "Tajik"})
            languages.Add(New Language With {.code = "th", .val = "Thai"})
            languages.Add(New Language With {.code = "ti", .val = "Tigrinya"})
            languages.Add(New Language With {.code = "bo", .val = "Tibetan Standard, Tibetan, Central"})
            languages.Add(New Language With {.code = "tk", .val = "Turkmen"})
            languages.Add(New Language With {.code = "tl", .val = "Tagalog"})
            languages.Add(New Language With {.code = "tn", .val = "Tswana"})
            languages.Add(New Language With {.code = "to", .val = "Tonga (Tonga Islands)"})
            languages.Add(New Language With {.code = "tr", .val = "Turkish"})
            languages.Add(New Language With {.code = "ts", .val = "Tsonga"})
            languages.Add(New Language With {.code = "tt", .val = "Tatar"})
            languages.Add(New Language With {.code = "tw", .val = "Twi"})
            languages.Add(New Language With {.code = "ty", .val = "Tahitian"})
            languages.Add(New Language With {.code = "ug", .val = "Uyghur"})
            languages.Add(New Language With {.code = "uk", .val = "Ukrainian"})
            languages.Add(New Language With {.code = "ur", .val = "Urdu"})
            languages.Add(New Language With {.code = "uz", .val = "Uzbek"})
            languages.Add(New Language With {.code = "ve", .val = "Venda"})
            languages.Add(New Language With {.code = "vi", .val = "Vietnamese"})
            languages.Add(New Language With {.code = "vo", .val = "Volapük"})
            languages.Add(New Language With {.code = "wa", .val = "Walloon"})
            languages.Add(New Language With {.code = "cy", .val = "Welsh"})
            languages.Add(New Language With {.code = "wo", .val = "Wolof"})
            languages.Add(New Language With {.code = "fy", .val = "Western Frisian"})
            languages.Add(New Language With {.code = "xh", .val = "Xhosa"})
            languages.Add(New Language With {.code = "yi", .val = "Yiddish"})
            languages.Add(New Language With {.code = "yo", .val = "Yoruba"})
            languages.Add(New Language With {.code = "za", .val = "Zhuang, Chuang"})
            languages.Add(New Language With {.code = "zu", .val = "Zulu"})
            languages.OrderBy(Function(x) x.val)
            languages.Insert(0, New Language With {.code = "", .val = "Use Phone Language"})

        End Sub
    End Class




    Public Class StatusUpdateViewModel
        Inherits ViewModelBase

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, Application).DefaultViewModel
            End Get

        End Property

        Public Overloads Property UpdateText As String
            Get
                Return _UpdateText
            End Get
            Set(value As String)
                _UpdateText = value
                RaisePropertyChanged("UpdateText")

            End Set
        End Property
        Private Property _UpdateText As String

        'Public Property NewMessage As String
        '    Get
        '        Return _NewMessage
        '    End Get
        '    Set(value As String)
        '        _NewMessage = value
        '        ' NotifyPropertyChanged()
        '    End Set
        'End Property
        'Private Property _NewMessage As String

        Public Property IsBusy As Boolean
            Get
                Return _IsBusy
            End Get
            Set(value As Boolean)
                _IsBusy = value
                RaisePropertyChanged("IsBusy")
            End Set
        End Property
        Private Property _IsBusy As Boolean

        'Public Property ConnectionColor As String
        '    Get
        '        If isConnected Then Return "Green" Else Return "Red"
        '    End Get
        '    Set(value As String)
        '        '_ConnectionColor = value
        '        RaisePropertyChanged("ConnectionColor")
        '    End Set
        'End Property
        'Private Property _ConnectionColor As String

        'Public Property isConnected As Boolean
        '    Get
        '        Return _isConnected
        '    End Get
        '    Set(value As Boolean)
        '        _isConnected = value
        '        RaisePropertyChanged("UpdateText")
        '        'RaisePropertyChanged("ConnectionColor")
        '        RaisePropertyChanged("isConnected")
        '    End Set
        'End Property
        'Private Property _isConnected As Boolean

        'Public Property ConnectedRotation As Integer
        '    Get
        '        Return _ConnectedRotation
        '    End Get
        '    Set(value As Integer)
        '        If _ConnectedRotation <= 170 Then _ConnectedRotation = _ConnectedRotation + 10 Else _ConnectedRotation = 0
        '        RaisePropertyChanged("ConnectedRotation")
        '    End Set
        'End Property
        'Private Property _ConnectedRotation As Integer





        Public Async Function Update(text As String, animated As Boolean, timoutInSeconds As Integer, Optional areWeBusy As Boolean = False, Optional areWeConnected As Boolean = True) As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             UpdateText = text
                                                                                                             IsBusy = areWeBusy
                                                                                                         End Sub)

        End Function

        Public Async Function Clean() As Task
            'Dim app As App = CType(Application.Current, Application)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             UpdateText = vm.appSettings.TVHVersionLong
                                                                                                             IsBusy = False
                                                                                                         End Sub)

        End Function

        Public Sub New()
            ' vm.isConnected = False
        End Sub
    End Class

    Public Class StreamListViewModel
        Inherits ViewModelBase

        Public Sub New()
            items = New ObservableCollection(Of StreamViewModel)
        End Sub
        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, Application).DefaultViewModel
            End Get

        End Property
        Private Property _items As ObservableCollection(Of StreamViewModel)
        Public Property items As ObservableCollection(Of StreamViewModel)
            Get
                Return _items
            End Get
            Set(value As ObservableCollection(Of StreamViewModel))
                _items = value
                RaisePropertyChanged("items")
            End Set
        End Property

        Public Async Function Reload() As Task
            If Await vm.TVHeadSettings.hasAdminAccess Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                 If Not items Is Nothing Then
                                                                                                                     items.Clear()
                                                                                                                     items = (Await LoadStreams()).ToObservableCollection()
                                                                                                                 End If

                                                                                                             End Sub)
            End If
        End Function

        Public Async Function Update(input_message As CometMessages.input_status) As Task
            Dim currentInput = items.Where(Function(y) y.identifier = input_message.uuid).FirstOrDefault()
            If Not currentInput Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 currentInput.Update(input_message)
                                                                                                             End Sub)

            Else
                Await Me.Reload()
            End If
        End Function

    End Class



    Public Class StreamViewModel
        Inherits ViewModelBase

        'Implements INotifyPropertyChanged
        'Public Function ShallowCopy() As AutoRecordingViewModel
        '    Return DirectCast(Me.MemberwiseClone(), AutoRecordingViewModel)
        'End Function


        'Public Event PropertyChanged As PropertyChangedEventHandler _
        '        Implements INotifyPropertyChanged.PropertyChanged
        'Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        'End Sub

        Public Property identifier As String
        Public Property bps As Integer
            Get
                Return _bps
            End Get
            Set(value As Integer)
                _bps = value
                RaisePropertyChanged("bps")
                RaisePropertyChanged("bandwidth_string")
            End Set
        End Property
        Private Property _bps As Integer

        Public ReadOnly Property bandwidth_string As String
            Get
                Return String.Format("{0} Mbps", Math.Round(bps / 1024 / 1024, 2))
            End Get
        End Property



        Public Property cc As Integer
            Get
                Return _cc
            End Get
            Set(value As Integer)
                _cc = value
                RaisePropertyChanged("cc")
                RaisePropertyChanged("errors_string")
            End Set
        End Property
        Private Property _cc As Integer

        Public Property te As Integer
            Get
                Return _te
            End Get
            Set(value As Integer)
                _te = value
                RaisePropertyChanged("te")
                RaisePropertyChanged("errors_string")
            End Set
        End Property
        Private Property _te As Integer
        Public Property weight As Integer
            Get
                Return _weight
            End Get
            Set(value As Integer)
                _weight = value
                RaisePropertyChanged("weight")
            End Set
        End Property
        Private Property _weight As Integer

        Public ReadOnly Property errors_string As String
            Get
                Return String.Format("{0} TE / {1} CE", te.ToString, cc.ToString)
            End Get

        End Property
        Public Property subs As Integer
        Public Property snr As Integer
            Get
                Return _snr
            End Get
            Set(value As Integer)
                _snr_string = value
                RaisePropertyChanged("snr")
                RaisePropertyChanged("snr_percentage")
            End Set
        End Property
        Private Property _snr As Integer

        Public Property snr_scale As Integer
        Public ReadOnly Property snr_string As String
            Get
                If snr_scale = 1 Then
                    Return (String.Format("{0}%", Math.Round(snr / 65535).ToString))
                Else
                    Return "NA"
                End If
            End Get
        End Property
        Public ReadOnly Property snr_percentage As Double
            Get
                If snr_scale = 1 Then
                    Return snr / 65535
                Else
                    Return 0
                End If
            End Get
        End Property
        Private Property _snr_string As String

        Public Property signal As Integer
            Get
                Return _signal
            End Get
            Set(value As Integer)
                _signal = value
                RaisePropertyChanged("signal")
                RaisePropertyChanged("signal_percentage")
            End Set
        End Property
        Private Property _signal As Integer

        Public Property signal_scale As Integer
        Public ReadOnly Property signal_string As String
            Get
                If signal_scale = 1 Then
                    Return (String.Format("{0}%", Math.Round(snr / 65535) * 100.ToString))
                Else
                    Return "NA"
                End If
            End Get
        End Property
        Public ReadOnly Property signal_percentage As Double
            Get
                If signal_scale = 1 Then
                    Return signal / 65535
                Else
                    Return 0
                End If
            End Get
        End Property


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


        Public Property currentMux As String
            Get
                Return _currentMux
            End Get
            Set(value As String)
                _currentMux = value
                RaisePropertyChanged("currentMux")
            End Set
        End Property
        Private Property _currentMux As String


        Public Sub New(a As tvh40.Adapter)
            name = a.input
            snr = a.snr
            snr_scale = a.snr_scale
            subs = a.subs
            cc = a.cc
            te = a.te
            bps = a.bps
            signal = a.signal
            signal_scale = a.signal_scale
            currentMux = a.stream
            identifier = a.uuid
            weight = a.weight
        End Sub

        Public Sub New(json As JsonObject)
            bps = json.GetNamedNumber("bps")
            identifier = json.GetNamedString("uuid")
            snr = json.GetNamedNumber("snr")
            snr_scale = json.GetNamedNumber("snr_scale")
            subs = json.GetNamedNumber("subs")
            name = json.GetNamedString("input")
            cc = json.GetNamedNumber("cc")
            te = json.GetNamedNumber("te")
            signal = json.GetNamedNumber("signal")
            signal_scale = json.GetNamedNumber("signal_scale")
            currentMux = json.GetNamedString("stream")
            weight = json.GetNamedNumber("weight")
        End Sub

        Public Sub Update(a As CometMessages.input_status)
            If Me.identifier <> a.uuid Then
                WriteToDebug("WARNING WARNING", "WARNING WARNING")
            End If
            Me.snr = a.snr
            Me.cc = a.cc
            Me.te = a.te
            Me.signal = a.signal
            Me.bps = a.bps
            Me.currentMux = a.stream
            Me.weight = a.weight
        End Sub
    End Class


    Public Class SubscriptionListViewModel
        Inherits ViewModelBase

        Public Sub New()
            items = New ObservableCollection(Of SubscriptionViewModel)
        End Sub
        'Public ReadOnly Property vm As TVHead_ViewModel
        '    Get
        '        Return CType(Application.Current, Application).DefaultViewModel
        '    End Get
        'End Property

        Private Property _items As ObservableCollection(Of SubscriptionViewModel)
        Public Property items As ObservableCollection(Of SubscriptionViewModel)
            Get
                Return _items
            End Get
            Set(value As ObservableCollection(Of SubscriptionViewModel))
                _items = value
                RaisePropertyChanged("items")
            End Set
        End Property

        Public Async Function Reload() As Task
            Dim vm As TVHead_ViewModel = CType(Application.Current, Application).DefaultViewModel
            If Await vm.TVHeadSettings.hasAdminAccess Then
                If Not items Is Nothing Then
                    Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                     items.Clear()
                                                                                                                     items = (Await LoadSubscriptions()).ToObservableCollection()
                                                                                                                 End Sub)
                End If
            End If
        End Function

        Public Async Function Update(subscription_message As CometMessages.subscription) As Task
            Dim x = items.Where(Function(y) y.id = subscription_message.id).FirstOrDefault()
            If Not x Is Nothing Then
                Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                 x.Update(subscription_message)
                                                                                                             End Sub)
            Else
                Await Me.Reload()
            End If
        End Function

    End Class



    Public Class SubscriptionViewModel
        Inherits ViewModelBase

        Private _subscription As tvh40.Subscription

        Public ReadOnly Property id As Integer
            Get
                Return _subscription.id
            End Get
        End Property
        Public Property start As Integer
            Get
                Return _subscription.start
            End Get
            Set(value As Integer)
                _subscription.start = value
            End Set
        End Property
        Public Property descramble As String
            Get
                Return _subscription.descramble
            End Get
            Set(value As String)
                _subscription.descramble = descramble
            End Set
        End Property
        Public Property errors As Integer
            Get
                Return _subscription.errors
            End Get
            Set(value As Integer)
                _subscription.errors = value
            End Set
        End Property
        Public Property state As String
            Get
                Return _subscription.state
            End Get
            Set(value As String)
                _subscription.state = value
                RaisePropertyChanged("state")
            End Set
        End Property
        Public ReadOnly Property hostname_usernameVisibility As String
            Get
                If _subscription.hostname = "" And _subscription.username = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property descrambleVisibility As String
            Get
                If _subscription.descramble = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property hostnameVisibility As String
            Get
                If _subscription.hostname = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property usernameVisibility As String
            Get
                If _subscription.username = "" Then Return "Collapsed" Else Return "Visible"
            End Get
        End Property
        Public ReadOnly Property hostname As String
            Get
                Return _subscription.hostname
            End Get
        End Property
        Public ReadOnly Property username As String
            Get
                Return _subscription.username
            End Get
        End Property
        Public Property title As String
            Get
                Return _subscription.title
            End Get
            Set(value As String)
                _subscription.title = value
                RaisePropertyChanged("title")
            End Set
        End Property
        Public Property channel As String
            Get
                Return _subscription.channel
            End Get
            Set(value As String)
                _subscription.channel = value
                RaisePropertyChanged("channel")
            End Set
        End Property
        Public ReadOnly Property service As String
            Get
                Return _subscription.service
            End Get

        End Property

        Public ReadOnly Property starttime As DateTime
            Get
                Return UnixToDateTime(_subscription.start)
            End Get
        End Property


        Public Property kbps_in As Integer
            Get
                Return _kbps_in
            End Get
            Set(value As Integer)
                _kbps_in = value
                RaisePropertyChanged("kbps_in")
                RaisePropertyChanged("kbps_in_string")
            End Set
        End Property
        Private Property _kbps_in As Integer

        Public Property kbps_out As Integer
            Get
                Return _kbps_out
            End Get
            Set(value As Integer)
                _kbps_out = value
                RaisePropertyChanged("kbps_out")
                RaisePropertyChanged("kbps_out_string")
            End Set
        End Property
        Private Property _kbps_out As Integer

        Public ReadOnly Property kbps_in_string As String
            Get
                Return String.Format("{0} kb/s", Math.Round(kbps_in / 100))
            End Get
        End Property

        Public ReadOnly Property kbps_out_string As String
            Get
                Return String.Format("{0} kb/s", Math.Round(kbps_out / 100))
            End Get
        End Property

        Public Sub New(s As tvh40.Subscription)
            _subscription = s
        End Sub

        Public Sub Update(x As CometMessages.subscription)
            If Me.id = x.id Then
                Me.start = x.start
                Me.state = x.state
                Me.title = x.title
                Me.errors = x.errors
                Me.kbps_in = x.in
                Me.kbps_out = x.out
                Me.channel = x.channel
                'Me.starttime = UnixToDateTime(start)
                Me.descramble = x.descramble
            End If
        End Sub
    End Class


    Public Class ServerInfoViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Property _sw_version As String
        Public Property sw_version As String
            Get
                Return _sw_version
            End Get
            Set(value As String)
                _sw_version = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _sw_versionlong As String
        Public Property sw_versionlong As String
            Get
                Return _sw_versionlong
            End Get
            Set(value As String)
                _sw_versionlong = value
                NotifyPropertyChanged()
            End Set
        End Property

        Private Property _api_version As Integer
        Public Property api_version As Integer
            Get
                Return _api_version
            End Get
            Set(value As Integer)
                _api_version = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _name As String
        Public Property name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Sub New(ServerInfo As tvh40.ServerInfo)
            name = ServerInfo.name
            sw_version = "3.9"
            api_version = ServerInfo.api_version
            sw_versionlong = ServerInfo.sw_version
        End Sub

        Public Sub New()

        End Sub
    End Class







    Public Class ServiceListViewModel
        Inherits ViewModelBase

        Private Property _items As List(Of ServiceViewModel)
        Public Property items As List(Of ServiceViewModel)
            Get
                Return _items
            End Get
            Set(value As List(Of ServiceViewModel))
                _items = value
                RaisePropertyChanged("items")
            End Set
        End Property

        Public Sub New()
            items = New List(Of ServiceViewModel)
        End Sub
    End Class


    Public Class ServiceViewModel


        Implements INotifyPropertyChanged

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, Application).DefaultViewModel
            End Get

        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
        Public Property auto As Integer
        Public Property caid As String
        Public Property created As String
        Public Property dvb_ignore_eit As Boolean
        Public Property dvb_servicetype As Integer

        Private Property _ExpandedView As String
        Public Property ExpandedView As String
            Get
                Return _ExpandedView
            End Get
            Set(value As String)
                _ExpandedView = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _IsLoadingSteams As Boolean
        Public Property IsLoadingSteams As Boolean
            Get
                Return _IsLoadingSteams
            End Get
            Set(value As Boolean)
                _IsLoadingSteams = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _servicedetails As List(Of ServiceDetailViewModel)
        Public Property servicedetails As List(Of ServiceDetailViewModel)
            Get
                Return _servicedetails
            End Get
            Set(value As List(Of ServiceDetailViewModel))
                _servicedetails = value
                NotifyPropertyChanged()
            End Set
        End Property

        Public Property enabled As Boolean
        Public Property encrypted As String
        Public Property force_caid As Integer
        Public Property last_seen As String
        Public Property lcn As Integer
        Public Property lcn_minor As Integer
        Public Property lcn2 As Integer
        Public Property multiplex As String
        Public Property network As String
        Public Property prefcapid As Integer
        Public Property prefcapid_lock As Integer
        Public Property provider As String
        Public Property sid As Integer
        Public Property svcname As String
        Public Property uuid As String

        Public Property ExpandCollapseCommand As RelayCommand
            Get
                Return New RelayCommand(Async Sub()
                                            'Dim app As App = CType(Application.Current, Application)
                                            WriteToDebug("ServiceViewModel.ExpanseCollapseCommand", "start")
                                            If Me.ExpandedView = "Expanded" Then
                                                Me.ExpandedView = "Collapsed"
                                            Else
                                                For Each service In vm.Services.items.Where(Function(x) x.ExpandedView = "Expanded")
                                                    service.ExpandedView = "Collapsed"
                                                Next
                                                Me.ExpandedView = "Expanded"
                                            End If
                                            'vm.StatusBar.Update("Loading streams...", True, 0, True, True)
                                            If Me.servicedetails Is Nothing Then
                                                Me.IsLoadingSteams = True
                                                Me.servicedetails = Await LoadServiceDetails(Me)
                                                Me.IsLoadingSteams = False
                                            End If
                                            'vm.StatusBar.Clean()
                                            WriteToDebug("ServiceViewModel.ExpanseCollapseCommand", "stop")
                                        End Sub)
            End Get
            Set(value As RelayCommand)
            End Set
        End Property




        Public Sub New(Service As tvh40.Service)
            uuid = Service.uuid
            enabled = Service.enabled
            multiplex = Service.multiplex
            lcn = Service.lcn
            sid = Service.sid
            encrypted = Service.encrypted
            svcname = Service.svcname
            network = Service.network
            created = UnixToDateTime(Service.created).ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern)
            ExpandedView = "Normal"
        End Sub

    End Class

    Public Class MuxListViewModel
        Inherits ViewModelBase

        Public Sub New()
            items = New ObservableCollection(Of MuxViewModel)
        End Sub

        Private Property _items As ObservableCollection(Of MuxViewModel)
        Public Property items As ObservableCollection(Of MuxViewModel)
            Get
                Return _items
            End Get
            Set(value As ObservableCollection(Of MuxViewModel))
                _items = value
                RaisePropertyChanged("items")
            End Set
        End Property
    End Class

    Public Class MuxViewModel
        Public Property constellation As String
        Public Property delsys As String
        Public Property enabled As Boolean
        Public Property epg As Integer
        Public Property fec As String
        Public Property frequency As Integer
        Public Property name As String
        Public Property network As String
        Public Property num_chn As Integer
        Public Property num_svc As Integer
        Public Property onid As Integer
        Public Property pmt_06_ac3 As Boolean
        Public Property scan_result As Integer
        Public Property scan_state As Integer
        Public Property symbolrate As Integer
        Public Property tsid As Integer
        Public Property uuid As String

        Public Sub New(Mux As tvh40.Mux)
            name = Mux.name
            uuid = Mux.uuid
            scan_result = Mux.scan_result
            scan_state = Mux.scan_state
            enabled = Mux.enabled
            constellation = Mux.constellation
            epg = Mux.epg
            tsid = Mux.tsid
            symbolrate = Mux.symbolrate
            num_chn = Mux.num_chn
            num_svc = Mux.num_svc
            frequency = Mux.frequency
        End Sub

    End Class

    Public Class ServiceDetailViewModel
        Public Property index As Integer
        Public Property pid As String
        Public Property type As String
        Public Property details As String



        Public Sub New(sd As tvh40.ServiceDetail)
            index = sd.index
            pid = sd.pid
            type = sd.type
            If sd.type = "CA" Then
                details = sd.details
            Else
                details = sd.language
            End If
            '            language = sd.language
            'details = sd.details
        End Sub
    End Class




    Public Class LogViewModel
        Inherits ViewModelBase

        Public Property entries As New ObservableCollection(Of String)

        Public Async Sub Add(message As String)
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                             'entries.Add(message)
                                                                                                             'If entries.Count = 50 Then entries.RemoveAt(0)
                                                                                                             entries.Insert(0, message)
                                                                                                             If entries.Count = 50 Then entries.RemoveAt(49)
                                                                                                         End Sub)
        End Sub

        Public Sub New()
        End Sub
    End Class

    Public Class DVRConfigListViewModel
        Public Property items As New List(Of DVRConfigViewModel)
        Public Property dataLoaded As Boolean

        Public Async Function Load() As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                             items.Clear()
                                                                                                             items = (Await LoadDVRConfigs()).ToList()
                                                                                                             dataLoaded = True
                                                                                                         End Sub)
        End Function
    End Class


    Public Class DVRConfigViewModel
        Public Property name As String
        Public Property identifier As String

        Public Sub New(dvrconfig As tvh40.DVRConfig)
            name = dvrconfig.val
            identifier = dvrconfig.key
        End Sub



        Public Sub New()

        End Sub
    End Class


    Public Class AddRecordingViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
                Implements INotifyPropertyChanged.PropertyChanged
        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public ReadOnly Property vm As TVHead_ViewModel
            Get
                Return CType(Application.Current, Application).DefaultViewModel
            End Get

        End Property

        Public Property channel As String
            Get
                Return _channel
            End Get
            Set(value As String)
                _channel = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _channel As String

        Public Property channelUuid As String
        Public Property chicon As String
            Get
                If _chicon = "" Then Return "/Images/tvheadend.png" Else Return _chicon
            End Get
            Set(value As String)
                If Not value Is Nothing And Not value = "/Images/tvheadend.png" Then
                    If value.ToUpper().IndexOf("HTTP:/") >= 0 Then _chicon = value Else _chicon = (New TVHead_Settings).GetFullURL() & "/" & value
                Else
                    _chicon = ""
                End If
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _chicon As String
        Public Property config_name As String
        Public Property title As String
        Public Property description As String
        Public Property dvrconfig_name As String
            Get
                Return _dvrconfig_name
            End Get
            Set(value As String)
                _dvrconfig_name = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _dvrconfig_name As String
        Public Property dvrconfig_uuid As String
        Public Property recording_id As String
        Public Property startDate As DateTime
        Public Property endDate As DateTime
        Public Property startTime As DateTime
        Public Property endTime As DateTime
        Public ReadOnly Property startDateTime As DateTime
            Get
                Return startDate.Date.Add(New TimeSpan(startTime.Hour, startTime.Minute, startTime.Second))
            End Get
        End Property

        Public ReadOnly Property endDateTime As DateTime
            Get
                Return endDate.Date.Add(New TimeSpan(endTime.Hour, endTime.Minute, endTime.Second))
            End Get
        End Property


        Public Property duration As Integer
        Public Property creator As String
        Public Property pri As String
        Public Property status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _status As String

        Public Property schedstate As String
            Get
                Return _schedstate
            End Get
            Set(value As String)
                _schedstate = value
                NotifyPropertyChanged()
            End Set
        End Property
        Private Property _schedstate As String
        Public Property DVRConfigVisibility As String




        Public Sub New()
            channel = "Click to select channel..."
            dvrconfig_uuid = ""
            If vm.TVHVersion = "3.4" Then
                DVRConfigVisibility = "Collapsed"
                dvrconfig_name = ""
            End If

            If vm.TVHVersion = "3.9" Then
                dvrconfig_name = "Select DVR Config..."
                DVRConfigVisibility = "Visible"
            End If

            title = "Enter title..."
            startDate = DateTime.Now
            endDate = DateTime.Now
            startTime = DateTime.Now
            endTime = DateTime.Now.AddHours(1)
        End Sub
    End Class



    Public Class ChannelTagListViewModel
        Inherits ViewModelBase
        Public Property items As New ObservableCollection(Of ChannelTagViewModel)
        Public Property dataLoaded As Boolean
        Public Property selectedChannelTag As ChannelTagViewModel
            Get
                Return _selectedChannelTag
            End Get
            Set(value As ChannelTagViewModel)
                _selectedChannelTag = value
                RaisePropertyChanged("selectedChannelTag")
            End Set
        End Property
        Private Property _selectedChannelTag As ChannelTagViewModel

        Public Async Function Load() As Task
            WriteToDebug("ChannelTagListViewModel.Load()", "executed")
            Dim vm As TVHead_ViewModel = (CType(Application.Current, Application)).DefaultViewModel
            Dim json_result As String
            Try
                json_result = Await (Await (New Downloader).DownloadJSON((New api40).apiGetChannelTags())).Content.ReadAsStringAsync
            Catch ex As Exception
                WriteToDebug("ChannelTagListViewModel.Load()", "stop-error")
                Return
            End Try
            If Not json_result = "" Then
                Dim dsChannelTagList = JsonConvert.DeserializeObject(Of tvh40.ChannelTagList)(json_result)
                Await RunOnUIThread(Sub()
                                        Me.items.Clear()
                                        For Each retrievedChannelTag In dsChannelTagList.entries.OrderBy(Function(x) x.name)
                                            items.Add(New ChannelTagViewModel(retrievedChannelTag))
                                        Next
                                    End Sub)
            End If

            'Set the Selected Channel Tag to the one stored in localsettings, if it exists
            Dim favTag = (From tag In items Select tag Where tag.uuid = vm.TVHeadSettings.FavouriteChannelTag).FirstOrDefault
            If Not favTag Is Nothing Then
                vm.selectedChannelTag = favTag
            Else
                If Not items.Count() = 0 Then
                    selectedChannelTag = items(0)
                End If
            End If
            dataLoaded = True
            WriteToDebug("Modules.LoadChannelTags()", "stop")
        End Function

        Public Sub New()
            'selectedChannelTag = New ChannelTagViewModel
        End Sub
    End Class


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



    Public Class ConnectionViewModel
        Inherits ViewModelBase
        Public Property id As Integer
        Public Property peer As String
        Public Property started As Long
        Public Property type As String
        Public Property user As String

        Public Sub New(c As tvh40.Connection)
            id = c.id
            peer = c.peer
            started = c.started
            type = c.type
            user = c.user

        End Sub
    End Class

    'ViewModel of a TVChannel



    'Public Class FinishedRecordingsViewModel
    '    Implements INotifyPropertyChanged

    '    Public Event PropertyChanged As PropertyChangedEventHandler _
    '    Implements INotifyPropertyChanged.PropertyChanged

    '    ' This method is called by the Set accessor of each property. 
    '    ' The CallerMemberName attribute that is applied to the optional propertyName 
    '    ' parameter causes the property name of the caller to be substituted as an argument. 
    '    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
    '        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    '    End Sub



    'End Class

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
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.GenreList)(json_allitems)
                Await RunOnUIThread(Sub()
                                        For Each f In deserialized.entries
                                            'Small hack to ensure we avoid having ContentType 0 in the list
                                            If f.key <> 0 Then
                                                allitems.Add(New ContentTypeViewModel(f))
                                            End If
                                        Next
                                    End Sub)
            End If
            If Not json_items = "" Then
                Dim deserialized = JsonConvert.DeserializeObject(Of tvh40.GenreList)(json_items)
                Await RunOnUIThread(Sub()
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

        Public Sub New(c As tvh40.Genre)
            uuid = c.key
            name = c.val
        End Sub
    End Class
End Namespace