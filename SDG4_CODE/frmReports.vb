Imports System.Data.SqlClient
Imports System.IO

Public Class frmReports

    ' ===================== GENERATE =====================
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            Using conn As SqlConnection = dbConnection.GetConnection()
                Dim query As String =
                    "SELECT c.CategoryName AS Categories, " &
                    "COUNT(r.ResourceID) AS [Total Resources], " &
                    "SUM(CASE WHEN MONTH(r.DateAdded) = MONTH(GETDATE()) AND YEAR(r.DateAdded) = YEAR(GETDATE()) THEN 1 ELSE 0 END) AS [Added This Month], " &
                    "MAX(r.Title) AS [Most Accessed] " &
                    "FROM tblCategories c " &
                    "LEFT JOIN tblResources r ON c.CategoryID = r.CategoryID " &
                    "GROUP BY c.CategoryName"

                Using cmd As New SqlCommand(query, conn)
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        dgvMonthlyReport.DataSource = dt
                        MessageBox.Show("Report generated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error generating report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== PRINT =====================
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            If dgvMonthlyReport.Rows.Count = 0 Then
                MessageBox.Show("No data to print. Please click GENERATE first.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim pd As New System.Drawing.Printing.PrintDocument()
            Dim printDialog As New PrintDialog()
            printDialog.Document = pd

            AddHandler pd.PrintPage, AddressOf PrintGridView

            If printDialog.ShowDialog() = DialogResult.OK Then
                pd.Print()
            End If
        Catch ex As Exception
            MessageBox.Show("Print error: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintGridView(sender As Object, e As System.Drawing.Printing.PrintPageEventArgs)
        Dim y As Integer = 50
        Dim x As Integer = 40
        Dim rowHeight As Integer = 25
        Dim font As New System.Drawing.Font("Arial", 9)
        Dim headerFont As New System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
        Dim titleFont As New System.Drawing.Font("Arial", 13, System.Drawing.FontStyle.Bold)

        ' Title
        e.Graphics.DrawString("Monthly Resource Consumption Report", titleFont,
            System.Drawing.Brushes.Black, x, y)
        y += 40

        ' Headers
        Dim colX As Integer = x
        For Each col As DataGridViewColumn In dgvMonthlyReport.Columns
            e.Graphics.DrawString(col.HeaderText, headerFont,
                System.Drawing.Brushes.Black, colX, y)
            colX += 160
        Next
        y += rowHeight

        ' Rows
        For Each row As DataGridViewRow In dgvMonthlyReport.Rows
            If row.IsNewRow Then Continue For
            colX = x
            For Each cell As DataGridViewCell In row.Cells
                e.Graphics.DrawString(If(cell.Value IsNot Nothing, cell.Value.ToString(), ""),
                    font, System.Drawing.Brushes.Black, colX, y)
                colX += 160
            Next
            y += rowHeight
        Next
    End Sub

    ' ===================== EXPORT TO PDF =====================
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            If dgvMonthlyReport.Rows.Count = 0 Then
                MessageBox.Show("No data to export. Please click GENERATE first.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim exportDirectory As String = AppDomain.CurrentDomain.BaseDirectory & "REPORTS\"
            If Not Directory.Exists(exportDirectory) Then
                Directory.CreateDirectory(exportDirectory)
            End If

            Dim filepath As String = exportDirectory & "Monthly_Report_" & Format(Now, "yyyyMMdd_HHmmss") & ".csv"

            Using sw As New StreamWriter(filepath)
                ' Write headers
                Dim headers As New List(Of String)
                For Each col As DataGridViewColumn In dgvMonthlyReport.Columns
                    headers.Add(col.HeaderText)
                Next
                sw.WriteLine(String.Join(",", headers))

                ' Write rows
                For Each row As DataGridViewRow In dgvMonthlyReport.Rows
                    If row.IsNewRow Then Continue For
                    Dim cells As New List(Of String)
                    For Each cell As DataGridViewCell In row.Cells
                        cells.Add(If(cell.Value IsNot Nothing, cell.Value.ToString(), ""))
                    Next
                    sw.WriteLine(String.Join(",", cells))
                Next
            End Using

            MessageBox.Show("Report exported successfully to: " & filepath, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Export error: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== CELL CLICK DRILLDOWN =====================
    Private Sub dgvMonthlyReport_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMonthlyReport.CellContentClick
        If e.RowIndex >= 0 Then
            Try
                Dim selectedCategory As String = dgvMonthlyReport.Rows(e.RowIndex).Cells(0).Value.ToString()

                Using conn As SqlConnection = dbConnection.GetConnection()
                    Dim query As String =
                        "SELECT r.Title, r.Author, r.ResourceURL " &
                        "FROM tblResources r " &
                        "INNER JOIN tblCategories c ON r.CategoryID = c.CategoryID " &
                        "WHERE c.CategoryName = @CategoryName"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CategoryName", selectedCategory)

                        Using da As New SqlDataAdapter(cmd)
                            Dim dtDetails As New DataTable()
                            da.Fill(dtDetails)

                            If dtDetails.Rows.Count > 0 Then
                                Dim msg As String = "Resources under " & selectedCategory & ":" & vbCrLf & vbCrLf
                                For Each row As DataRow In dtDetails.Rows
                                    msg &= "• Title: " & row("Title").ToString() & vbCrLf
                                    msg &= "  Author: " & If(row("Author") Is DBNull.Value, "Unknown", row("Author").ToString()) & vbCrLf
                                    msg &= "  Link: " & row("ResourceURL").ToString() & vbCrLf & vbCrLf
                                Next
                                MessageBox.Show(msg, selectedCategory & " Resources", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show("No resources found under " & selectedCategory, "Empty Category", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class