Imports System.Data.SqlClient

Public Class dbConnection

    Private Shared strConn As String =
        "Data Source=localhost;" &
        "Initial Catalog=SGD4_ResourceTracker;" &
        "Integrated Security=True;" &
        "TrustServerCertificate=True;"

    Public Shared Function GetConnection() As SqlConnection
        Dim conn As New SqlConnection(strConn)
        conn.Open()
        Return conn
    End Function

End Class