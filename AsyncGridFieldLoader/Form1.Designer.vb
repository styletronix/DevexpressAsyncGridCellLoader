<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMyAsyncValue1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMyAsyncValue2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMyAsyncValue3 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn3 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemPictureEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SvgImageCollection1 = New DevExpress.Utils.SvgImageCollection(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton8 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton9 = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SvgImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridControl1
        '
        Me.GridControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GridControl1.DataSource = Me.BindingSource1
        Me.GridControl1.Location = New System.Drawing.Point(12, 92)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemPictureEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(1365, 602)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'BindingSource1
        '
        Me.BindingSource1.DataSource = GetType(AsyncGridFieldLoader.MyDataItem)
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colID, Me.colName, Me.colMyAsyncValue1, Me.colMyAsyncValue2, Me.colMyAsyncValue3, Me.GridColumn1, Me.GridColumn2, Me.GridColumn3})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsCustomization.AllowRowSizing = True
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.RowHeight = 20
        '
        'colID
        '
        Me.colID.FieldName = "ID"
        Me.colID.MinWidth = 30
        Me.colID.Name = "colID"
        Me.colID.Visible = True
        Me.colID.VisibleIndex = 0
        Me.colID.Width = 56
        '
        'colName
        '
        Me.colName.FieldName = "Name"
        Me.colName.MinWidth = 30
        Me.colName.Name = "colName"
        Me.colName.Visible = True
        Me.colName.VisibleIndex = 1
        Me.colName.Width = 136
        '
        'colMyAsyncValue1
        '
        Me.colMyAsyncValue1.FieldName = "async_MyAsyncValue1"
        Me.colMyAsyncValue1.MinWidth = 30
        Me.colMyAsyncValue1.Name = "colMyAsyncValue1"
        Me.colMyAsyncValue1.UnboundDataType = GetType(String)
        Me.colMyAsyncValue1.Visible = True
        Me.colMyAsyncValue1.VisibleIndex = 3
        Me.colMyAsyncValue1.Width = 151
        '
        'colMyAsyncValue2
        '
        Me.colMyAsyncValue2.FieldName = "async_MyAsyncValue2"
        Me.colMyAsyncValue2.MinWidth = 30
        Me.colMyAsyncValue2.Name = "colMyAsyncValue2"
        Me.colMyAsyncValue2.UnboundDataType = GetType(String)
        Me.colMyAsyncValue2.Visible = True
        Me.colMyAsyncValue2.VisibleIndex = 4
        Me.colMyAsyncValue2.Width = 169
        '
        'colMyAsyncValue3
        '
        Me.colMyAsyncValue3.FieldName = "async_MyAsyncValue3"
        Me.colMyAsyncValue3.MinWidth = 30
        Me.colMyAsyncValue3.Name = "colMyAsyncValue3"
        Me.colMyAsyncValue3.OptionsColumn.ReadOnly = True
        Me.colMyAsyncValue3.UnboundDataType = GetType(String)
        Me.colMyAsyncValue3.Visible = True
        Me.colMyAsyncValue3.VisibleIndex = 5
        Me.colMyAsyncValue3.Width = 135
        '
        'GridColumn1
        '
        Me.GridColumn1.Caption = "unbound async Value 1"
        Me.GridColumn1.FieldName = "async_unboundAsyncValue1"
        Me.GridColumn1.MinWidth = 30
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.UnboundDataType = GetType(String)
        Me.GridColumn1.Visible = True
        Me.GridColumn1.VisibleIndex = 6
        Me.GridColumn1.Width = 168
        '
        'GridColumn2
        '
        Me.GridColumn2.Caption = "unbound async Value 2"
        Me.GridColumn2.FieldName = "async_unboundAsyncValue2"
        Me.GridColumn2.MinWidth = 30
        Me.GridColumn2.Name = "GridColumn2"
        Me.GridColumn2.UnboundDataType = GetType(String)
        Me.GridColumn2.Visible = True
        Me.GridColumn2.VisibleIndex = 7
        Me.GridColumn2.Width = 170
        '
        'GridColumn3
        '
        Me.GridColumn3.Caption = "Image"
        Me.GridColumn3.ColumnEdit = Me.RepositoryItemPictureEdit1
        Me.GridColumn3.FieldName = "async_myImage"
        Me.GridColumn3.MinWidth = 30
        Me.GridColumn3.Name = "GridColumn3"
        Me.GridColumn3.UnboundDataType = GetType(Object)
        Me.GridColumn3.Visible = True
        Me.GridColumn3.VisibleIndex = 2
        Me.GridColumn3.Width = 119
        '
        'RepositoryItemPictureEdit1
        '
        Me.RepositoryItemPictureEdit1.Name = "RepositoryItemPictureEdit1"
        Me.RepositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(12, 12)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(187, 34)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "Get Data"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(205, 12)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(194, 34)
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "Get Data 2"
        '
        'SvgImageCollection1
        '
        Me.SvgImageCollection1.Add("loading", CType(resources.GetObject("SvgImageCollection1.loading"), DevExpress.Utils.Svg.SvgImage))
        Me.SvgImageCollection1.Add("picture", CType(resources.GetObject("SvgImageCollection1.picture"), DevExpress.Utils.Svg.SvgImage))
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 200
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(12, 52)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(98, 19)
        Me.LabelControl1.TabIndex = 3
        Me.LabelControl1.Text = "LabelControl1"
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Location = New System.Drawing.Point(405, 12)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(292, 34)
        Me.SimpleButton3.TabIndex = 4
        Me.SimpleButton3.Text = "Change async helper to use KeyField"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Location = New System.Drawing.Point(703, 12)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(292, 34)
        Me.SimpleButton4.TabIndex = 5
        Me.SimpleButton4.Text = "Change async helper without KeyField"
        '
        'SimpleButton5
        '
        Me.SimpleButton5.Location = New System.Drawing.Point(205, 52)
        Me.SimpleButton5.Name = "SimpleButton5"
        Me.SimpleButton5.Size = New System.Drawing.Size(194, 34)
        Me.SimpleButton5.TabIndex = 6
        Me.SimpleButton5.Text = "Set worker threads to 1"
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Location = New System.Drawing.Point(405, 52)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(194, 34)
        Me.SimpleButton6.TabIndex = 7
        Me.SimpleButton6.Text = "Set worker threads to 4"
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Location = New System.Drawing.Point(605, 52)
        Me.SimpleButton7.Name = "SimpleButton7"
        Me.SimpleButton7.Size = New System.Drawing.Size(194, 34)
        Me.SimpleButton7.TabIndex = 8
        Me.SimpleButton7.Text = "Set worker threads to 10"
        '
        'SimpleButton8
        '
        Me.SimpleButton8.Location = New System.Drawing.Point(805, 52)
        Me.SimpleButton8.Name = "SimpleButton8"
        Me.SimpleButton8.Size = New System.Drawing.Size(190, 34)
        Me.SimpleButton8.TabIndex = 9
        Me.SimpleButton8.Text = "Refresh myImage"
        '
        'SimpleButton9
        '
        Me.SimpleButton9.Location = New System.Drawing.Point(1001, 52)
        Me.SimpleButton9.Name = "SimpleButton9"
        Me.SimpleButton9.Size = New System.Drawing.Size(239, 34)
        Me.SimpleButton9.TabIndex = 10
        Me.SimpleButton9.Text = "Refresh unboundAsyncValue2"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1389, 706)
        Me.Controls.Add(Me.SimpleButton9)
        Me.Controls.Add(Me.SimpleButton8)
        Me.Controls.Add(Me.SimpleButton7)
        Me.Controls.Add(Me.SimpleButton6)
        Me.Controls.Add(Me.SimpleButton5)
        Me.Controls.Add(Me.SimpleButton4)
        Me.Controls.Add(Me.SimpleButton3)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.GridControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SvgImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BindingSource1 As BindingSource
    Friend WithEvents colID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMyAsyncValue1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMyAsyncValue2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMyAsyncValue3 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridColumn3 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemPictureEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents SvgImageCollection1 As DevExpress.Utils.SvgImageCollection
    Friend WithEvents Timer1 As Timer
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton7 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton8 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton9 As DevExpress.XtraEditors.SimpleButton
End Class
