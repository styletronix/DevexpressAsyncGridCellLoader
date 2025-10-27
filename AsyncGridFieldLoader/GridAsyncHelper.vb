Imports System.Threading
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

''' <summary>
''' Copyright (C) 2020 - 2025 by Andreas W. Pross (Styletronix.net GmbH)
''' 
''' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal
''' in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
''' of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
''' 
''' The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
''' 
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
''' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
''' WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </summary>
Public Class GridAsyncHelper
    Implements IDisposable

    Public Sub New(GridView As DevExpress.XtraGrid.Views.Grid.GridView, Optional SynchronisationContext As System.Threading.SynchronizationContext = Nothing, Optional KeyField As String = Nothing, Optional ThreadsCount As Short = 1)
        If SynchronisationContext Is Nothing Then
            Me.SynchronisationContext = System.Threading.SynchronizationContext.Current
        Else
            Me.SynchronisationContext = SynchronisationContext
        End If

        Me.GridView = GridView
        Me.KeyField = KeyField

        AddHandler GridView.CustomUnboundColumnData, AddressOf CustomUnboundColumnData
        AddHandler GridView.DataManagerReset, AddressOf DataManagerReset
        AddHandler GridView.StartSorting, AddressOf StartSorting
        AddHandler GridView.StartGrouping, AddressOf StartGrouping
        AddHandler GridView.RowCountChanged, AddressOf RowCountChanged
        AddHandler GridView.TopRowChanged, AddressOf TopRowChanged
        AddHandler GridView.Layout, AddressOf OnLayout

        'Set initial worker threads
        WorkerThreadsCount = ThreadsCount
    End Sub

    Private SetWorkerThreadsSemaphore As New SemaphoreSlim(1, 1)

    ''' <summary>
    ''' Worker count can be changed at runtime to increase or decrease the number of parallel threads processing the queue.
    ''' </summary>
    Public Property WorkerThreadsCount As Short
        Get
            SetWorkerThreadsSemaphore.Wait()
            Return Me.AsyncTasks.Count
            SetWorkerThreadsSemaphore.Release()
        End Get
        Set(value As Short)
            SetWorkerThreadsSemaphore.Wait()
            Try
                If Me.AsyncTasks.Count < value Then
                    Dim toAdd = value - Me.AsyncTasks.Count
                    For x = 1 To toAdd
                        Dim t As New WorkerItem
                        t.CTX = New CancellationTokenSource
                        t.Worker = Tasks.Task.Factory.StartNew(
                        Sub()
                            Task(t.CTX.Token)
                        End Sub, t.CTX.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)

                        Me.AsyncTasks.Add(t)
                    Next
                End If

                If Me.AsyncTasks.Count > value Then
                    Dim toRemove = Me.AsyncTasks.Count - value
                    For x = 1 To toRemove
                        Dim t = Me.AsyncTasks.Last
                        t.CTX.Cancel()
                        t.CTX.Dispose()
                        Me.AsyncTasks.Remove(t)
                    Next
                End If
            Finally
                SetWorkerThreadsSemaphore.Release()
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the cache should be kept when the data source changes.
    ''' Keep Cache works only if KeyField is set.
    ''' </summary>
    ''' <value>
    ''' <see langword="true"/> if the cache should be kept on data source change; otherwise, <see langword="false"/>.
    ''' </value>
    ''' <returns></returns>
    Public Property KeepCacheOnDataSourceChange As Boolean = False

    ''' <summary>
    ''' Gets or sets a value indicating whether the processing should be done
    ''' synchronous or asynchronous.
    ''' </summary>
    ''' <remarks>
    ''' This is designed for debugging purposes. If set to <see langword="true"/>, the
    ''' <see cref="GetAsyncData"/> event is called synchronousely on the UI thread. And you can't use
    ''' awaitable Tasks in the event handler. Also GetAsyncDataEventArgs.isAsyncCall is set to
    ''' <see langword="false"/> in this case.
    ''' </remarks>
    ''' <value>
    ''' <see langword="true"/> if processing is done synchronousely; otherwise, <see
    ''' langword="false"/>.
    ''' </value>
    Public Property DisableAsyncMode As Boolean
    ''' <summary>
    ''' Displays this image in the grid cell while the actual image is being loaded asynchronously.
    ''' </summary>
    ''' <returns></returns>
    Public Property loadingImage As System.Drawing.Image
    ''' <summary>
    ''' Gets the number of pending tasks in the processing queue. If QueueLength is 0, there may still be active tasks
    ''' being processed in the background. Depending in the number of threads configured.
    ''' </summary>
    ''' <returns>Integer</returns>
    Public ReadOnly Property QueueLength As Integer
        Get
            Return TaskList.Count
        End Get
    End Property

    ''' <summary>
    ''' Occurs when data for a table cell is requested.
    ''' You can use the cancellation token to cancel long running operations. Optionally,
    ''' you can return an awaitable Task to support asynchronous operations. In that case,
    ''' the grid will wait for the task to complete before updating the cell value. 
    ''' The value itself has to be set in the GetAsyncDataEventArgs.
    ''' </summary>
    ''' <remarks>
    ''' <para>This event is called asynchronousely on a background thread.</para>
    ''' <para>If this Event is not handled, internal asynchronous loading is
    ''' done which may cause problems if the datasource is not thread safe</para>
    ''' </remarks>
    Public Event GetAsyncData(sender As Object, e As GetAsyncDataEventArgs)

    Private ReadOnly GridView As DevExpress.XtraGrid.Views.Grid.GridView
    Private ReadOnly KeyField As String
    Private ReadOnly TaskList As New Concurrent.BlockingCollection(Of AsyncJob)(New Concurrent.ConcurrentStack(Of AsyncJob))
    Private ReadOnly Cache As New Concurrent.ConcurrentDictionary(Of String, Object)
    Private CTS As New System.Threading.CancellationTokenSource
    Private ReadOnly SynchronisationContext As System.Threading.SynchronizationContext
    Private AsyncTasks As New List(Of WorkerItem)
    Private firstVisibleRowHandle As Int64
    Private lastVisibleRowHandle As Int64
    Private Sub UpdateVisibleRowHandles()
        Dim vi As GridViewInfo = Me.GridView.GetViewInfo
        Dim visibleRows = New List(Of Object)
        Dim rows = vi.RowsInfo.OfType(Of GridDataRowInfo)

        Dim normalRows = rows.Where(Function(a) a.IsSpecialRow = False).ToList()

        If normalRows.Count = 0 Then
            firstVisibleRowHandle = 0
            lastVisibleRowHandle = 0
        Else
            firstVisibleRowHandle = Me.GridView.GetVisibleRowHandle(normalRows.Min(Function(a) a.VisibleIndex))
            lastVisibleRowHandle = Me.GridView.GetVisibleRowHandle(normalRows.Max(Function(a) a.VisibleIndex))
        End If
    End Sub
#Region "Event Handlers"

    Private Sub TopRowChanged(sender As Object, e As EventArgs)
        UpdateVisibleRowHandles()
    End Sub
    Private Sub OnLayout(sender As Object, e As EventArgs)
        UpdateVisibleRowHandles()
    End Sub
    Private Sub RowCountChanged(sender As Object, e As EventArgs)
        UpdateVisibleRowHandles()

        If Not KeepCacheOnDataSourceChange OrElse String.IsNullOrWhiteSpace(Me.KeyField) Then
            Dim count = Me.GridView.RowCount
            If LastRowCount > count Or count = 0 Then
                Me.ClearCache()
            End If
            LastRowCount = count
        End If
    End Sub
    Private Sub StartGrouping(sender As Object, e As EventArgs)
        UpdateVisibleRowHandles()

        If String.IsNullOrWhiteSpace(Me.KeyField) Then
            Me.ClearCache()
        End If
    End Sub

    Private Sub StartSorting(sender As Object, e As EventArgs)
        UpdateVisibleRowHandles()

        If String.IsNullOrWhiteSpace(Me.KeyField) Then
            Me.ClearCache()
        End If
    End Sub
    Private Sub DataManagerReset(sender As Object, e As EventArgs)
        If Me.KeepCacheOnDataSourceChange AndAlso Not String.IsNullOrWhiteSpace(Me.KeyField) Then
            'Do not clear cache
            Return
        End If

        Me.LastRowCount = 0
        Me.ClearCache()
    End Sub
#End Region
    Private Class WorkerItem
        Public Property Worker As Task
        Public Property CTX As CancellationTokenSource
    End Class
    Private LastRowCount As Integer


    ''' <summary>
    ''' Holds a unique ID for the cache. If changed, the cache get's cleared automatically
    ''' </summary>
    Private CurrentJobID = Guid.NewGuid
    ''' <summary>
    '''  Clears the internal cache and processing queue.
    ''' </summary>
    Public Sub ClearCache()
        'Generate new JobID
        CurrentJobID = Guid.NewGuid

        'Cancel existing tasks
        CancelAndResetCts(CTS)

        Me.Cache.Clear()
    End Sub
    Public Sub ClearCache(column As String)
        For Each item In Me.Cache.ToList
            If String.Equals(column, ExtractFieldName(item.Key), StringComparison.InvariantCultureIgnoreCase) Then
                Me.Cache.TryRemove(item.Key, Nothing)
            End If
        Next
    End Sub

    Private Function ExtractFieldName(input As String) As String
        If String.IsNullOrEmpty(input) Then Return Nothing
        Dim parts = input.Split("_"c)
        If parts.Length < 3 Then Return Nothing
        Return parts(2)
    End Function

    Private Sub CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs)
        Try
            Dim JobID = Me.CurrentJobID
            Dim fieldName = e.Column.FieldName

            If fieldName.StartsWith("async_") Then
                fieldName = fieldName.Remove(0, 6)
                Dim view = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)
                Dim rowhandle = view.GetRowHandle(e.ListSourceRowIndex)
                Dim key = view.DetailLevel & "_" & e.ListSourceRowIndex & "_" & fieldName
                Dim KeyValue As String = Nothing

                If Not String.IsNullOrWhiteSpace(Me.KeyField) Then
                    KeyValue = GetPropertyValueRecursive(view.GetRow(rowhandle), Me.KeyField).ToString
                    key = view.DetailLevel & "_[" & KeyValue & "]_" & fieldName
                End If
                key = key & "_" & JobID.ToString 'Add JobID to key to avoid wrong data after cache clear

                If e.IsGetData Then
                    If Not Cache.TryGetValue(key, e.Value) Then
                        If DisableAsyncMode Then
                            If view.IsRowLoaded(rowhandle) Then
                                Dim row = view.GetRow(rowhandle)

                                Dim args As New GetAsyncDataEventArgs With {
                                        .key = key,
                                        .row = row,
                                        .rowHandle = rowhandle,
                                        .view = view,
                                        .FieldName = fieldName,
                                        .Column = e.Column,
                                        .isAsyncCall = False,
                                        .JobID = JobID,
                                        .SynchronisationContext = Me.SynchronisationContext}

                                RaiseEvent GetAsyncData(Me, args)

                                If args.handled Then
                                    Cache.TryAdd(key, args.value)
                                    e.Value = args.value
                                Else
                                    Dim pi As System.Reflection.PropertyInfo = row.GetType.GetProperty(fieldName)
                                    If pi Is Nothing Then
                                        Cache.TryAdd(key, Nothing)
                                        e.Value = Nothing
                                    Else
                                        Dim value = pi.GetValue(row, Nothing)
                                        Cache.TryAdd(key, value)
                                        e.Value = value
                                    End If
                                End If
                            End If

                        Else
                            If TaskList.TryAdd(New AsyncJob With {
                                             .ListSourceRowIndex = e.ListSourceRowIndex,
                                             .FieldName = fieldName,
                                             .GridView = view,
                                             .Column = e.Column,
                                             .Key = key,
                                             .KeyValue = KeyValue,
                                             .JobID = JobID}) Then

                                If e.Column.UnboundDataType = GetType(String) Then
                                    e.Value = "Loading..."

                                Else
                                    If e.Column.RealColumnEdit.GetType Is GetType(DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit) Then
                                        e.Value = Me.loadingImage

                                    ElseIf e.Column.RealColumnEdit.GetType Is GetType(DevExpress.XtraEditors.Repository.RepositoryItemImageEdit) Then
                                        e.Value = Me.loadingImage

                                    End If

                                End If
                            Else
                                If e.Column.UnboundType = DevExpress.Data.UnboundColumnType.String Then
                                    e.Value = "Cancelled..."
                                Else
                                    e.Value = Nothing
                                End If
                            End If
                        End If
                    End If
                End If

                If e.IsSetData Then
                    Dim old As Object = Nothing

                    If Cache.TryGetValue(key, old) Then
                        Dim row = view.GetRow(rowhandle)

                        SetPropertyValueRecursive(row, fieldName, e.Value)

                        Cache.TryUpdate(key, e.Value, old)
                    End If
                End If
            End If
        Catch ex As Exception
            Debugger.Log(0, "Error", $"Error: {ex.Message}{Environment.NewLine}")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ct">Stops further processing of Tasklist and ends this task. It does not cancel current job</param>
    Private Async Sub Task(ct As CancellationToken)
        Try
            Do While Not TaskList?.IsCompleted AndAlso Not ct.IsCancellationRequested
                Try
                    Dim item = TaskList.Take(ct)
                    'Clear Job if grid contains new data
                    If Not item.JobID = Me.CurrentJobID Then Continue Do

                    Dim key As String = item.Key

                    If Not Cache.ContainsKey(key) Then
                        Dim rowhandle As Integer
                        Dim loaded As Boolean
                        Dim row As Object = Nothing
                        If item.GridView.GridControl Is Nothing Then Continue Do

                        'Get Grid Data
                        item.GridView.GridControl.Invoke(
                        Sub()
                            rowhandle = item.GridView.GetRowHandle(item.ListSourceRowIndex)
                            loaded = item.GridView.IsRowLoaded(rowhandle)
                            row = item.GridView.GetRow(rowhandle)
                        End Sub)

                        'Skip if row is not visible
                        If rowhandle < firstVisibleRowHandle Or rowhandle > lastVisibleRowHandle Then Continue Do

                        If loaded Then
                            If row Is Nothing Then Continue Do
                            If Not String.IsNullOrWhiteSpace(item.KeyValue) Then
                                If Not String.Equals(item.KeyValue, GetPropertyValueRecursive(row, Me.KeyField).ToString) Then Continue Do
                            End If

                            Dim tk = CTS.Token
                            'Clear Job if grid contains new data
                            If tk.IsCancellationRequested OrElse Not item.JobID = Me.CurrentJobID Then Continue Do

                            Dim args As New GetAsyncDataEventArgs With {
                                .key = key,
                                .row = row,
                                .rowHandle = rowhandle,
                                .view = item.GridView,
                                .FieldName = item.FieldName,
                                .isAsyncCall = True,
                                .Column = item.Column,
                                .JobID = item.JobID,
                                .CancellationToken = tk,
                                .SynchronisationContext = Me.SynchronisationContext}

                            RaiseEvent GetAsyncData(Me, args)

                            'If cancellation is requested, skip further processing
                            If tk.IsCancellationRequested OrElse Not item.JobID = Me.CurrentJobID Then Continue Do

                            'If Async Task is provided, await it
                            If args.AwaitableTask IsNot Nothing Then
                                Await args.AwaitableTask
                            End If

                            'Clear Job if grid data changed during GetAsyncData
                            If tk.IsCancellationRequested OrElse Not item.JobID = Me.CurrentJobID Then Continue Do

                            If args.handled Then
                                'Add Item Value from Event Handler
                                Cache.TryAdd(key, args.value)
                            Else
                                'Get Item Value from Entity if GetAsyncData is unhandled
                                Dim value = GetPropertyValueRecursive(row, item.FieldName)
                                Cache.TryAdd(key, value)
                            End If

                            'Refresh Grid Cell
                            item.GridView.GridControl.Invoke(
                                Sub()
                                    'Do not refresh grid in case of job ID mismatch
                                    If item.JobID = Me.CurrentJobID Then
                                        item.GridView.RefreshRowCell(rowhandle, item.GridView.Columns.ColumnByFieldName("async_" & item.FieldName))
                                    End If
                                End Sub)
                        Else
                            'Row is not loaded yet, re-add task to queue after a short delay
                            Await Tasks.Task.Delay(100).ConfigureAwait(False)
                            TaskList.TryAdd(item)

                            'System.Threading.Thread.Sleep(100)
                        End If
                    End If
                Catch ex2 As InvalidOperationException
                    'TaskList wurde abgeschlossen
                Catch ex3 As OperationCanceledException
                    'Task wurde abgebrochen
                Catch ex As Exception
                    'Styletronix.Utils.Debugging.LogError(ex)
                End Try
            Loop
        Catch ex As Exception

        End Try
    End Sub
    Private Shared Function GetPropertyValueRecursive(obj As Object, Name As String) As Object
        Dim split = Name.Split(".")
        Dim result As Object = obj

        For Each s In split
            If result Is Nothing Then Return Nothing

            Dim pi As System.Reflection.PropertyInfo = result.GetType.GetProperty(s)
            If pi Is Nothing Then
                Return Nothing
            Else
                result = pi.GetValue(result, Nothing)
            End If
        Next

        Return result
    End Function
    Private Shared Sub SetPropertyValueRecursive(obj As Object, Name As String, newValue As Object)
        Dim split = Name.Split(".")
        Dim result As Object = obj
        Dim pi As System.Reflection.PropertyInfo = Nothing

        Dim ind = 0
        For Each s In split
            ind += 1
            If result Is Nothing Then
                Return
            End If

            pi = result.GetType.GetProperty(s)

            If ind <split.Length Then
                If pi Is Nothing Then
            Return
        Else
            result = pi.GetValue(result, Nothing)
        End If
        End If
        Next

        If pi IsNot Nothing Then
            pi.SetValue(result, newValue)
        End If
    End Sub

    Private Class AsyncJob
        Public Property ListSourceRowIndex As Int64
        Public Property FieldName As String
        Public Property GridView As DevExpress.XtraGrid.Views.Grid.GridView
        Public Property Column As DevExpress.XtraGrid.Columns.GridColumn
        Public Property Key As String
        Public Property KeyValue As String
        Public Property JobID As Guid
    End Class
    Public Class GetAsyncDataEventArgs
        Inherits EventArgs

        ''' <summary>
        ''' Gets the row which is currently processing.
        ''' </summary>
        Public Property row As Object
        ''' <summary>
        ''' Gets or Sets the internal key to identify the processed data.
        ''' It is unique for each cell in the grid and unique to each time the cache is cleared.
        ''' </summary>
        Public Property key As String
        ''' <summary>
        ''' Gets or sets a value indicating whether the data processing was handled in an
        ''' eventhandler.
        ''' </summary>
        ''' <value>
        ''' <see langword="true"/> if data has been processed and the default data handler
        ''' should not be used; otherwise, <see langword="false"/>.
        ''' </value>
        Public Property handled As Boolean
        ''' <summary>
        ''' Gets or sets the return value.
        ''' </summary>
        Public Property value As Object
        Public Property rowHandle As Int64
        ''' <summary>
        ''' Gets or sets the view for which the data is processed.
        ''' </summary>
        ''' <remarks>
        ''' <para>This reference is not thread safe. Use Invoke / BeginInvoke to synchronize
        ''' access.</para>
        ''' <code lang="VB"><![CDATA[view.GridControl.invoke(sub() ..... )]]></code>
        ''' </remarks>
        Public Property view As DevExpress.XtraGrid.Views.Grid.GridView
        ''' <summary>
        ''' Gets or sets the fieldname for which data is requested.
        ''' </summary>
        Public Property FieldName As String
        ''' <summary>
        ''' Gets or sets a value indicating whether this instance is called asynchronousely.
        ''' </summary>
        ''' <value>
        ''' <see langword="true"/> if this instance was called in an asynchronous way on a
        ''' thread other than theMain UI; otherwise, <see langword="false"/>.
        ''' </value>
        Public Property isAsyncCall As Boolean
        Public Property Column As DevExpress.XtraGrid.Columns.GridColumn
        ''' <summary>
        ''' The JobID is equal to the GridAsyncHelper.CurrentJobID at the time the job was created.
        ''' It helps to identify if the job is still valid (i.e. the grid data has not changed meanwhile).
        ''' </summary>
        ''' <returns>Guid</returns>
        Public Property JobID As Guid
        ''' <summary>
        ''' CancellationToken to support cancelling of long running operations.
        ''' </summary>
        ''' <returns></returns>
        Public Property CancellationToken As System.Threading.CancellationToken
        Public Property AwaitableTask As System.Threading.Tasks.Task
        ''' <summary>
        ''' SynchronizationContext of the GridView UI thread.
        ''' </summary>
        ''' <returns></returns>
        Public Property SynchronisationContext As System.Threading.SynchronizationContext
    End Class
#Region "IDisposable Support"
    Private disposedValue As Boolean ' Dient zur Erkennung redundanter Aufrufe.

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                RemoveHandler GridView.CustomUnboundColumnData, AddressOf CustomUnboundColumnData
                RemoveHandler GridView.DataManagerReset, AddressOf DataManagerReset
                RemoveHandler GridView.StartSorting, AddressOf StartSorting
                RemoveHandler GridView.StartGrouping, AddressOf StartGrouping
                RemoveHandler GridView.TopRowChanged, AddressOf TopRowChanged
                RemoveHandler GridView.Layout, AddressOf OnLayout

                Me.TaskList.CompleteAdding()
                Me.ClearCache()
                Me.TaskList.Dispose()

                For Each item In Me.AsyncTasks
                    Try
                        item.CTX.Cancel()
                        item.Worker.Dispose()
                    Catch ex As Exception

                    End Try
                Next
                Me.AsyncTasks.Clear()
            End If
        End If
        disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region

    Public Shared Function CancelAndResetCts(ByRef CTS As CancellationTokenSource) As CancellationTokenSource
        Dim oldCTS = Interlocked.Exchange(Of CancellationTokenSource)(CTS, New CancellationTokenSource)

        If oldCTS IsNot Nothing Then
            Try : oldCTS.Cancel()
            Finally : oldCTS.Dispose()
            End Try
        End If

        Return CTS
    End Function
End Class