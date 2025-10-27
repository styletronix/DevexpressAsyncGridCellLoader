Public Class MyDataItem
    Public Property ID As Integer
    Public Property Name As String

    Private _MyAsyncValue1 As String
    Public Property MyAsyncValue1 As String
        Get
            If _MyAsyncValue1 Is Nothing Then
                System.Threading.Thread.Sleep(100) ' Simulate delay
                _MyAsyncValue1 = "Async Value 1 for ID " & ID
            End If

            Return _MyAsyncValue1
        End Get
        Set(value As String)
            Me._myAsyncValue1 = value
        End Set
    End Property

    Private _MyAsyncValue2 As String
    Public Property MyAsyncValue2 As String
        Get
            If _MyAsyncValue2 Is Nothing Then
                System.Threading.Thread.Sleep(50) ' Simulate delay
                _MyAsyncValue2 = "Async Value 2 for ID " & ID
            End If

            Return _MyAsyncValue2
        End Get
        Set(value As String)
            Me._MyAsyncValue2 = value
        End Set
    End Property

    Public ReadOnly Property MyAsyncValue3 As String
        Get
            System.Threading.Thread.Sleep(50) ' Simulate delay
            Return "Async Value 3 is always uncached for ID " & ID
        End Get
    End Property
End Class
