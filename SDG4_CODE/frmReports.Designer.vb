<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReports
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.LocalReport = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.dgvMonthlyReport = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReportDataSource = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvMonthlyReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(674, 371)
        Me.btnGenerate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(166, 28)
        Me.btnGenerate.TabIndex = 1
        Me.btnGenerate.Text = "GENERATE"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(13, 368)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(166, 31)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'LocalReport
        '
        Me.LocalReport.Font = New System.Drawing.Font("Imprint MT Shadow", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LocalReport.ForeColor = System.Drawing.Color.White
        Me.LocalReport.Location = New System.Drawing.Point(156, 22)
        Me.LocalReport.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LocalReport.Name = "LocalReport"
        Me.LocalReport.Size = New System.Drawing.Size(560, 28)
        Me.LocalReport.TabIndex = 3
        Me.LocalReport.Text = "Monthly Resource Consumption Report"
        Me.LocalReport.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(187, 369)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(166, 30)
        Me.btnExport.TabIndex = 4
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'dgvMonthlyReport
        '
        Me.dgvMonthlyReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMonthlyReport.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.ReportDataSource, Me.Column3, Me.Column4})
        Me.dgvMonthlyReport.Location = New System.Drawing.Point(13, 70)
        Me.dgvMonthlyReport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvMonthlyReport.Name = "dgvMonthlyReport"
        Me.dgvMonthlyReport.RowHeadersVisible = False
        Me.dgvMonthlyReport.RowHeadersWidth = 51
        Me.dgvMonthlyReport.Size = New System.Drawing.Size(827, 280)
        Me.dgvMonthlyReport.TabIndex = 6
        '
        'Column1
        '
        Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column1.HeaderText = "Categories"
        Me.Column1.MinimumWidth = 6
        Me.Column1.Name = "Column1"
        '
        'ReportDataSource
        '
        Me.ReportDataSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ReportDataSource.DataPropertyName = "ReportDataSource"
        Me.ReportDataSource.HeaderText = "Total Resources"
        Me.ReportDataSource.MinimumWidth = 6
        Me.ReportDataSource.Name = "ReportDataSource"
        '
        'Column3
        '
        Me.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column3.HeaderText = "Added This Month"
        Me.Column3.MinimumWidth = 6
        Me.Column3.Name = "Column3"
        '
        'Column4
        '
        Me.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column4.HeaderText = "Most Accessed"
        Me.Column4.MinimumWidth = 6
        Me.Column4.Name = "Column4"
        '
        'frmReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(853, 416)
        Me.Controls.Add(Me.dgvMonthlyReport)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.LocalReport)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnGenerate)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmReports"
        Me.Text = "frmReports"
        CType(Me.dgvMonthlyReport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnGenerate As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents LocalReport As Label
    Friend WithEvents btnExport As Button
    Friend WithEvents dgvMonthlyReport As DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents ReportDataSource As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
End Class
