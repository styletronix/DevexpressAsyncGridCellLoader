Imports System.Threading

Public Class Form1
    Private WithEvents _asyncHelper As GridAsyncHelper
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        'Initialize the AsyncHelper with 4 threads and KeyField "ID" to enable caching of loaded values on sorting, filtering and grouping
        Me._asyncHelper = New GridAsyncHelper(Me.GridView1, ThreadsCount:=4, KeyField:=NameOf(MyDataItem.ID))
        Me._asyncHelper.loadingImage = Me.SvgImageCollection1.GetImage("loading") 'Optional: set a loading image to be displayed for images while data is being loaded
    End Sub

    Private Sub _asyncHelper_GetAsyncData(sender As Object, e As GridAsyncHelper.GetAsyncDataEventArgs) Handles _asyncHelper.GetAsyncData
        'Any Exception thrown in this event handler will be captured and discarded by the AsyncHelper.

        Dim myData = CType(e.row, MyDataItem)

        'Add "async_" prefix to the FieldName in the grid column to mark it as async loaded
        'The AsyncHelper will call this event for all columns with "async_" prefix in their FieldName
        'e.FieldName will not contain the "async_" prefix. It is removed before calling this event.
        'If you do not hande a specific FieldName in this event, the AsyncHelper will call the property getter of the actual datarow
        'from a background thread. This is useful for properties that are marked as Async in the grid but do not require special loading logic.
        'For example "async_MyValue" will call the MyValue property getter of the MyDataItem class in a background thread.
        'You should set unboundType to the type of the actual data because the gridview is threading it like a normal unbound column.

        Select Case e.FieldName
            Case "unboundAsyncValue1"
                'This is executed in a background thread
                System.Threading.Thread.Sleep(50) ' Simulate delay
                e.value = "Async Value 4 for ID " & myData.ID
                e.handled = True

            Case "unboundAsyncValue2"
                e.AwaitableTask = Async Function()
                                      'This is executed in a background thread
                                      'Advantage: You can use AwaitableTask to perform asynchronous operations
                                      'If you use AwaitableTask, make sure to set e.handled = True in the async function
                                      'The grid will wait for the task to complete before updating the cell value

                                      Await Task.Delay(100, e.CancellationToken) ' Simulate delay
                                      e.value = $"Async Value 5 retrieved at {Date.Now.ToString } with awaitableTask for ID {myData.ID}"
                                      e.handled = True
                                  End Function()

            Case "myImage"
                System.Threading.Thread.Sleep(100) ' Simulate delay
                e.value = StubGlyph.GenerateStubGlyph(myData.ID, New Drawing.Size(128, 128), Color.White, Color.Black, 30)
                e.handled = True
        End Select
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'Set DataSource to an empty Dataset or Gettype(...) first to clear the BindingSource
        'If you don't do this, the async loading of the previous data source may interfere with the new one
        'and causes mismatch between async loaded data and row if no KeyField is set in the AsyncHelper

        'Keeps previously loaded data in cache when DataSource is changed.
        'Only available if KeyField is set in the AsyncHelper
        Me._asyncHelper.KeepCacheOnDataSourceChange = True 'Default is false

        Me.BindingSource1.DataSource = GetType(MyDataItem)
        Me.BindingSource1.DataSource = GetData.GetDummyData(1, 1000)
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Me._asyncHelper.KeepCacheOnDataSourceChange = True
        Me.BindingSource1.DataSource = GetType(MyDataItem)
        Me.BindingSource1.DataSource = GetData.GetDummyData(1000, 2000)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'This is only for demonstration purposes
        Me.LabelControl1.Text = $"Queue Length: {_asyncHelper.QueueLength}"
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'This is only for demonstration purposes
        'Recreate the AsyncHelper with KeyField "ID" to enable caching of loaded values on sorting, filtering and grouping
        Dim old = Interlocked.Exchange(Me._asyncHelper, New GridAsyncHelper(Me.GridView1, ThreadsCount:=4, KeyField:=NameOf(MyDataItem.ID)))
        Me._asyncHelper.loadingImage = Me.SvgImageCollection1.GetImage("loading")
        old.Dispose()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'This is only for demonstration purposes
        'Recreate the AsyncHelper without KeyField. Data is reloaded on sorting, filtering and grouping
        Dim old = Interlocked.Exchange(Me._asyncHelper, New GridAsyncHelper(Me.GridView1, ThreadsCount:=4))
        Me._asyncHelper.loadingImage = Me.SvgImageCollection1.GetImage("loading")
        old.Dispose()
    End Sub

    Private Sub Form1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Me._asyncHelper.Dispose()
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me._asyncHelper.WorkerThreadsCount = 1
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Me._asyncHelper.WorkerThreadsCount = 4
    End Sub

    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs) Handles SimpleButton7.Click
        Me._asyncHelper.WorkerThreadsCount = 10
    End Sub

    Private Sub SimpleButton8_Click(sender As Object, e As EventArgs) Handles SimpleButton8.Click
        Me._asyncHelper.ClearCache("myImage")
        Me.GridControl1.RefreshDataSource()
    End Sub

    Private Sub SimpleButton9_Click(sender As Object, e As EventArgs) Handles SimpleButton9.Click
        Me._asyncHelper.ClearCache("unboundAsyncValue2")
        Me.GridControl1.RefreshDataSource()
    End Sub
End Class
