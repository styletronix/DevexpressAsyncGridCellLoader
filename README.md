# AsyncGridFieldLoader

Asynchronous per-cell data loading for **DevExpress WinForms Grid** — featuring caching, cancellation, visible-row optimization, and dynamic worker scaling.  
Built for **.NET Framework 4.8**, **Visual Studio 2022**, and **DevExpress 24.x**.  
Everything is contained in a single VB class: `GridAsyncHelper`.

---

## Features

- Asynchronous field loading via `async_` column prefix  
- Automatic caching per cell  
- Persistent cache on sorting/grouping when a `KeyField` is provided  
- Dynamic worker count (`WorkerThreadsCount`)  
- Full cancellation support (`CancellationToken`)  
- Processes **only visible rows** for performance  
- Optional synchronous debug mode (`DisableAsyncMode`)  
- Optional loading image placeholder (`loadingImage`)  
- Clean resource management (`IDisposable`) and thread-safe design

---

## Requirements

- .NET Framework 4.8  
- DevExpress WinForms Grid (tested with v24.2)  
- Visual Basic (examples below). Works fine in C# as well.

---

## Installation

1. Add **`GridAsyncHelper.vb`** to your WinForms project.  
2. Ensure references to `DevExpress.XtraGrid` are available.

---

## Quick Start

```vb
' 1) Create and attach helper to your GridView before loading any data.
_asyncHelper = New GridAsyncHelper(MyGridView, KeyField:=NameOf(MyDataItem.ID), ThreadsCount:=1)
_asyncHelper.loadingImage = My.Resources.loader_32   ' optional
_asyncHelper.KeepCacheOnDataSourceChange = True      ' optional: keep cache on data source reset

' 2) Rename columns to use async loading
'    Change "MyFieldName" to "async_MyFieldName" and set UnboundType properly
colMyField.FieldName = "async_MyFieldName"
colMyField.UnboundType = DevExpress.Data.UnboundColumnType.String

' 3) (Optional) If your data source is NOT thread-safe or the field is unbound,
'    handle the GetAsyncData event
AddHandler _asyncHelper.GetAsyncData, AddressOf OnGetAsyncData
```

---

## Async Column Naming

Every asynchronously loaded column must start with `async_`.

**Examples:**
- `async_PreviewImage`
- `async_TotalPrice`
- `async_StatusText`

Also set `UnboundType` correctly (`String`, `Decimal`, `Object`, etc.).  
For image columns, use `RepositoryItemPictureEdit` or `RepositoryItemImageEdit` and define `loadingImage`.

---

## Persistent Cache via `KeyField`

If your data source has a stable unique key, specify it via the constructor parameter.  
This enables cache persistence even when sorting or grouping changes.

```vb
_asyncHelper = New GridAsyncHelper(MyGridView, KeyField:=NameOf(MyItem.ID))
```

### Advantages
- Keeps cached data consistent during resorting or regrouping  
- Avoids redundant reprocessing of values

---

## Dynamic Worker Count

Adjust the number of parallel background workers at runtime:

```vb
' Increase parallelism
_asyncHelper.WorkerThreadsCount = 4

' Reduce parallelism
_asyncHelper.WorkerThreadsCount = 1
```

---

## API Reference (Summary)

### Constructor
```vb
New GridAsyncHelper(view As GridView,
                    Optional SynchronisationContext As SynchronizationContext = Nothing,
                    Optional KeyField As String = Nothing,
                    Optional ThreadsCount As Short = 1)
```

---

### Key Properties

| Property | Description |
|-----------|--------------|
| `WorkerThreadsCount` | Number of concurrent worker threads. Can be changed dynamically. |
| `KeepCacheOnDataSourceChange` | Keeps cache on data source reset (requires `KeyField`). |
| `DisableAsyncMode` | Runs synchronously on UI thread (debug only). |
| `loadingImage` | Placeholder image displayed while loading. |
| `QueueLength` | Current number of queued jobs. |

---

### Key Methods

| Method | Description |
|---------|--------------|
| `ClearCache()` | Clears the cache and queue. |
| `ClearCache(column As String)` | Clears cached data for a single column. |
| `Dispose()` | Stops workers, removes handlers, and releases resources. |

---

### Event

#### `GetAsyncData(sender, e As GetAsyncDataEventArgs)`

Triggered whenever a grid cell value is requested asynchronously.

**Provides:**
- `row`, `FieldName`, `Column`, `JobID`  
- `CancellationToken`, `SynchronisationContext`  
- Optional `AwaitableTask` for async workflows

---

## Best Practices

- **Thread Safety:** Always handle `GetAsyncData` if your data source is not thread-safe.   
- **Cancellation:** Use `e.CancellationToken` in long operations to allow graceful cancellation.  
- **Job Validation:** The helper automatically invalidates jobs when grid data changes.  
- **UI Thread Access:** Always call `Invoke` or `BeginInvoke` before updating UI controls.
