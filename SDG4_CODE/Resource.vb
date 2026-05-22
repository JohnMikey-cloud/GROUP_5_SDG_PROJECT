Public Class Resource
    ' These properties represent the columns in your tblResources
    Public Property ResourceID As Integer
    Public Property Title As String
    Public Property Author As String
    Public Property URL As String
    Public Property CategoryID As Integer

    ' Logic Example (FR2): A function to check if the URL is valid
    Public Function IsValidResource() As Boolean
        If String.IsNullOrEmpty(Title) OrElse String.IsNullOrEmpty(URL) Then
            Return False
        End If
        ' Add Regex validation here to meet the Validation requirement 
        Return True
    End Function
End Class