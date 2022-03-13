Imports MySql.Data.MySqlClient

Public Class Form1

    Dim sqlConn As New MySqlConnection
    Dim sqlCommand As New MySqlCommand
    Dim sqlRead As MySqlDataReader
    Dim sqlDataTable As New DataTable

    Dim sqlQuerry As String
    Dim server As String = "localhost"
    Dim username As String = "root"
    Dim password As String = ""
    Dim database As String = "visual_basic"

    Dim id As Integer

    Private Sub updateTable() 'Load the result from the querry into DataGridView named as DataTable
        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn
        sqlCommand.CommandText = "SELECT * FROM visual_basic.books"
        sqlRead = sqlCommand.ExecuteReader
        sqlDataTable.Load(sqlRead)
        sqlRead.Close()
        sqlConn.Close()
        DataTable.DataSource = sqlDataTable
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateTable()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim bExit As DialogResult
        bExit = MessageBox.Show("Confirm you want to exit", "Book Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If bExit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub DataTable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataTable.CellClick
        Try
            id = DataTable.SelectedRows(0).Cells(0).Value.ToString
            TextTitle.Text = DataTable.SelectedRows(0).Cells(1).Value.ToString
            TextAuthor.Text = DataTable.SelectedRows(0).Cells(2).Value.ToString
            TextPrice.Text = DataTable.SelectedRows(0).Cells(3).Value.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database

        Try
            sqlConn.Open()
            sqlQuerry = "INSERT INTO visual_basic.books(title, author, price) VALUES ('" & TextTitle.Text & "', '" & TextAuthor.Text & "', '" & TextPrice.Text & "')"
            sqlCommand = New MySqlCommand(sqlQuerry, sqlConn)
            sqlRead = sqlCommand.ExecuteReader
            sqlConn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Book Management System", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            sqlConn.Dispose()
        End Try
        updateTable()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        TextTitle.Clear()
        TextAuthor.Clear()
        TextPrice.Clear()
        TextSearch.Clear()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn
        sqlCommand.CommandText = "UPDATE visual_basic.books SET title = '" & TextTitle.Text & "', author = '" & TextAuthor.Text & "', price = '" & TextPrice.Text & "' WHERE id = '" & id & "'"
        sqlCommand.CommandType = CommandType.Text
        sqlCommand.ExecuteNonQuery()
        sqlConn.Close()
        updateTable()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn
        sqlCommand.CommandText = "DELETE FROM books WHERE id = '" & id & "'"
        sqlCommand.CommandType = CommandType.Text
        sqlCommand.ExecuteNonQuery()
        sqlConn.Close()

        For Each row As DataGridViewRow In DataTable.SelectedRows
            DataTable.Rows.Remove(row)
        Next
        updateTable()
    End Sub

    Private Sub TextSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextSearch.KeyPress
        Try
            If Asc(e.KeyChar) = 13 Then
                Dim dv As DataView
                dv = sqlDataTable.DefaultView
                dv.RowFilter = String.Format("Title like '%{0}%' OR Author like '%{0}%'", TextSearch.Text)
                DataTable.DataSource = dv.ToTable()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class