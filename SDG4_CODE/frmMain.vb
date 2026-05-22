Imports System.Data.SqlClient

Public Class frmMain

    Private currentRole As String = ""

    ' ===================== FORM LOAD =====================
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadResources()

        dgvResourceTracker.DefaultCellStyle.ForeColor = Color.Black
        dgvResourceTracker.DefaultCellStyle.BackColor = Color.White
        dgvResourceTracker.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
    End Sub

    Public Sub SetRole(role As String)
        currentRole = role
        If role = "Admin" Then
            btnAdd.Enabled = True
            btnEdit.Enabled = True
            btnDelete.Enabled = True
            btnCategories.Enabled = True
        ElseIf role = "Standard User" Then
            btnAdd.Enabled = True
            btnEdit.Enabled = True
            btnDelete.Enabled = True
            btnCategories.Enabled = True
        End If
    End Sub

    Private Function IsAdmin() As Boolean
        If currentRole <> "Admin" Then
            MessageBox.Show("You must be admin to access this feature.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    ' ===================== LOAD DATA INTO GRID =====================
    Private Sub LoadResources(Optional filter As String = "")
        Try
            Using conn As SqlConnection = dbConnection.GetConnection()
                Dim query As String =
                "SELECT r.ResourceID AS ID, r.Title, r.Author, r.ResourceURL AS URL, c.CategoryName AS Category " &
                "FROM tblResources r " &
                "LEFT JOIN tblCategories c ON r.CategoryID = c.CategoryID"

                If filter <> "" Then
                    query &= " WHERE r.Title LIKE @filter OR r.Author LIKE @filter OR c.CategoryName LIKE @filter"
                End If

                Using cmd As New SqlCommand(query, conn)
                    cmd.CommandTimeout = 0
                    If filter <> "" Then
                        cmd.Parameters.AddWithValue("@filter", "%" & filter & "%")
                    End If
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        dgvResourceTracker.AutoGenerateColumns = True
                        dgvResourceTracker.DataSource = dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading resources: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== SEARCH =====================
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        LoadResources(TextBox1.Text.Trim())
    End Sub

    ' ===================== ADD =====================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not IsAdmin() Then Exit Sub

        Dim title As String = InputBox("Enter Title:", "Add Resource")
        If title = "" Then Exit Sub

        Dim author As String = InputBox("Enter Author:", "Add Resource")

        Dim url As String = InputBox("Enter URL:", "Add Resource")
        If url = "" Then
            MessageBox.Show("URL is required. Please enter a valid URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim categoryName As String = ""
        Try
            Using conn As SqlConnection = dbConnection.GetConnection()
                Dim catList As String = ""
                Dim catQuery As String = "SELECT CategoryName FROM tblCategories ORDER BY CategoryName"
                Using cmd As New SqlCommand(catQuery, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            catList &= reader("CategoryName").ToString() & vbCrLf
                        End While
                    End Using
                End Using

                categoryName = InputBox("Enter Category (available categories below):" & vbCrLf & catList, "Add Resource")
                If categoryName = "" Then Exit Sub

                Dim catID As Integer = 0
                Dim catIDQuery As String = "SELECT CategoryID FROM tblCategories WHERE CategoryName = @name"
                Using cmd2 As New SqlCommand(catIDQuery, conn)
                    cmd2.Parameters.AddWithValue("@name", categoryName)
                    Dim result = cmd2.ExecuteScalar()
                    If result Is Nothing Then
                        MessageBox.Show("Category not found. Please enter a valid category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                    catID = Convert.ToInt32(result)
                End Using

                Dim insertQuery As String =
                "INSERT INTO tblResources (Title, Author, ResourceURL, CategoryID, DateAdded) " &
                "VALUES (@title, @author, @url, @catID, GETDATE())"
                Using cmd3 As New SqlCommand(insertQuery, conn)
                    cmd3.CommandTimeout = 0
                    cmd3.Parameters.AddWithValue("@title", title)
                    cmd3.Parameters.AddWithValue("@author", If(author = "", DBNull.Value, author))
                    cmd3.Parameters.AddWithValue("@url", url)
                    cmd3.Parameters.AddWithValue("@catID", catID)
                    cmd3.ExecuteNonQuery()
                End Using

                MessageBox.Show("Resource added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadResources()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error adding resource: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== EDIT =====================
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Not IsAdmin() Then Exit Sub

        If dgvResourceTracker.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a resource to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Dim selectedRow = dgvResourceTracker.SelectedRows(0)
            Dim selectedID As Integer = Convert.ToInt32(selectedRow.Cells("ID").Value)

            Dim newTitle As String = InputBox("Edit Title:", "Edit Resource", selectedRow.Cells("Title").Value.ToString())
            If newTitle = "" Then Exit Sub

            Dim newAuthor As String = InputBox("Edit Author:", "Edit Resource", selectedRow.Cells("Author").Value.ToString())
            Dim newURL As String = InputBox("Edit URL:", "Edit Resource", selectedRow.Cells("URL").Value.ToString())

            Using conn As SqlConnection = dbConnection.GetConnection()
                Dim catList As String = ""
                Dim catQuery As String = "SELECT CategoryName FROM tblCategories ORDER BY CategoryName"
                Using cmd As New SqlCommand(catQuery, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            catList &= reader("CategoryName").ToString() & vbCrLf
                        End While
                    End Using
                End Using

                Dim newCategory As String = InputBox("Edit Category (available categories below):" & vbCrLf & catList,
                    "Edit Resource", selectedRow.Cells("Category").Value.ToString())
                If newCategory = "" Then Exit Sub

                Dim catID As Integer = 0
                Dim catIDQuery As String = "SELECT CategoryID FROM tblCategories WHERE CategoryName = @name"
                Using cmd2 As New SqlCommand(catIDQuery, conn)
                    cmd2.Parameters.AddWithValue("@name", newCategory)
                    Dim result = cmd2.ExecuteScalar()
                    If result Is Nothing Then
                        MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                    catID = Convert.ToInt32(result)
                End Using

                Dim updateQuery As String =
                    "UPDATE tblResources SET Title=@title, Author=@author, ResourceURL=@url, CategoryID=@catID " &
                    "WHERE ResourceID=@id"
                Using cmd3 As New SqlCommand(updateQuery, conn)
                    cmd3.Parameters.AddWithValue("@title", newTitle)
                    cmd3.Parameters.AddWithValue("@author", If(newAuthor = "", DBNull.Value, newAuthor))
                    cmd3.Parameters.AddWithValue("@url", newURL)
                    cmd3.Parameters.AddWithValue("@catID", catID)
                    cmd3.Parameters.AddWithValue("@id", selectedID)
                    cmd3.ExecuteNonQuery()
                End Using

                MessageBox.Show("Resource updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadResources()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error editing resource: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== DELETE =====================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If Not IsAdmin() Then Exit Sub

        If dgvResourceTracker.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a resource to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to delete this resource?",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            Try
                Dim selectedID As Integer = Convert.ToInt32(dgvResourceTracker.SelectedRows(0).Cells("ID").Value)
                Using conn As SqlConnection = dbConnection.GetConnection()
                    Dim cmd As New SqlCommand("DELETE FROM tblResources WHERE ResourceID = @id", conn)
                    cmd.Parameters.AddWithValue("@id", selectedID)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Resource deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadResources()
            Catch ex As Exception
                MessageBox.Show("Error deleting resource: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' ===================== REPORT =====================
    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        Dim frm As New frmReports()
        frm.ShowDialog()
    End Sub

    ' ===================== CATEGORIES =====================
    Private Sub btnCategories_Click(sender As Object, e As EventArgs) Handles btnCategories.Click
        If Not IsAdmin() Then Exit Sub

        Try
            Using conn As SqlConnection = dbConnection.GetConnection()
                Dim catList As String = "Current Categories:" & vbCrLf & vbCrLf
                Using cmd As New SqlCommand("SELECT CategoryName FROM tblCategories ORDER BY CategoryName", conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            catList &= "• " & reader("CategoryName").ToString() & vbCrLf
                        End While
                    End Using
                End Using

                Dim choice As DialogResult = MessageBox.Show(catList & vbCrLf & "Do you want to add a new category?",
                    "Categories", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                If choice = DialogResult.Yes Then
                    Dim newCat As String = InputBox("Enter new category name:", "Add Category")
                    If newCat <> "" Then
                        Using cmd2 As New SqlCommand("INSERT INTO tblCategories (CategoryName) VALUES (@name)", conn)
                            cmd2.Parameters.AddWithValue("@name", newCat)
                            cmd2.ExecuteNonQuery()
                        End Using
                        MessageBox.Show("Category added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===================== NAVIGATION BUTTONS =====================
    Private Sub btnResources_Click(sender As Object, e As EventArgs)
        If Not IsAdmin() Then Exit Sub
        LoadResources()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadResources(TextBox1.Text.Trim())
    End Sub

    Private Sub btnC1_Click(sender As Object, e As EventArgs)
        If Not IsAdmin() Then Exit Sub
        Dim frm As New frmReports()
        frm.ShowDialog()
        LoadResources()
    End Sub

    Private Sub btnR1_Click(sender As Object, e As EventArgs)
        Dim frm As New frmReports()
        frm.ShowDialog()
        LoadResources()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs)
        If Not IsAdmin() Then Exit Sub
        MessageBox.Show("Settings feature coming soon.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to logout?",
        "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.Yes Then
            Me.Hide()
            Dim frm As New frmLogin()
            frm.Show()
        End If
    End Sub

    ' ===================== CELL CLICK =====================
    Private Sub dgvResourceTracker_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResourceTracker.CellContentClick

    End Sub

    Private Sub btnResearch_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

End Class