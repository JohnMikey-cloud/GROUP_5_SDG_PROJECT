Imports System.Data.SqlClient

Public Class frmLogin
    ' Global Session Tiers: Shared properties accessible by any form in the project
    Public Shared UserRole As String = "Standard User"
    Public Shared LoggedInUser As String = ""

    ' BUTTON: LOGIN (btnLogin)
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 1. Form Validation Layer
        If String.IsNullOrWhiteSpace(txtUsername.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please enter both your Username and Password to proceed.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cmbRole.SelectedIndex = -1 Then
            MessageBox.Show("Please select an authorization role from the dropdown menu.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' 2. Structural Error Handling & Authentication Layer
        Try
            Dim inputUser As String = txtUsername.Text.Trim().ToLower()
            Dim inputPass As String = txtPassword.Text
            Dim selectedRole As String = cmbRole.SelectedItem.ToString()

            ' Administrator Gateway Route

            ' Administrator Gateway Route
            If selectedRole = "Admin" Then
                If inputUser <> "admin" Then
                    MessageBox.Show("Wrong username!", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                ElseIf inputPass <> "admin123" Then
                    MessageBox.Show("Wrong password!", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    UserRole = "Admin"
                    LoggedInUser = txtUsername.Text.Trim()
                    MessageBox.Show("Administrator security clearance authorized. Welcome!",
                        "Authentication Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Hide()
                    Dim frm As New frmMain()
                    frm.SetRole("Admin")
                    frm.Show()
                End If

                ' Standard Student Gateway Route
            ElseIf selectedRole = "Standard User" Then
                If inputUser <> "student" Then
                    MessageBox.Show("Wrong username!", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                ElseIf inputPass <> "student123" Then
                    MessageBox.Show("Wrong password!", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    UserRole = "Standard User"
                    LoggedInUser = txtUsername.Text.Trim()
                    MessageBox.Show("Standard Reader authorization granted. Welcome, Student!",
                        "Authentication Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Hide()
                    Dim frm As New frmMain()
                    frm.SetRole("Standard User")
                    frm.Show()
                End If

            Else
                MessageBox.Show("Please select a valid role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("A physical breakdown occurred inside the security routing system: " & ex.Message,
                            "System Fault Detected", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class