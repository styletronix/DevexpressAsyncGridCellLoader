Public Class GetData
    Public Shared Function GetDummyData(startIndex As Int64, endIndex As Int64) As List(Of MyDataItem)
        Dim ret As New List(Of MyDataItem)

        For x = startIndex To endIndex
            Dim item As New MyDataItem()
            item.ID = x
            item.Name = "Item " & x.ToString()

            ret.Add(item)
        Next

        Return ret
    End Function
End Class
