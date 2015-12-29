Imports System.Threading.Tasks
Imports System.Threading
Imports TVHead_81.ViewModels

Partial Public Class ucRecording
    Inherits UserControl

    Public Event UpdateStatus As RoutedEventHandler
    Public Event RecordingDeleted As RoutedEventHandler
    Public Event SelectionStarted As RoutedEventHandler
    Public Event SelectionChanged As RoutedEventHandler
    Public Event ItemUnchecked As RoutedEventHandler
    Public Event ItemChecked As RoutedEventHandler

    'Public Event RecordingClick As RoutedEventHandler

    Public Sub New()
        InitializeComponent()

    End Sub

End Class
