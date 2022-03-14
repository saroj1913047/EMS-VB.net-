Imports MySql.Data.MySqlClient

Public Class Form1

    'initialize some global variables
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

    Private Sub updateTable()

        'connecting to MySQL

        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn

        'fire querry in database

        sqlCommand.CommandText = "SELECT * FROM visual_basic.books"
        sqlRead = sqlCommand.ExecuteReader

        'load the data in sqlDataTable

        sqlDataTable.Load(sqlRead)
        sqlRead.Close()
        sqlConn.Close()

        'make sqlDataTable the data source for DataGridView

        DataTable.DataSource = sqlDataTable
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Updates table on first load

        updateTable()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        'ask for confirmation using messagebox

        Dim bExit As DialogResult
        bExit = MessageBox.Show("Confirm you want to exit", "Book Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If bExit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub DataTable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataTable.CellClick

        'load the row data into the form when a cell is clicked

        Try
            'declare value for id to use it for database querries

            id = DataTable.SelectedRows(0).Cells(0).Value.ToString

            TextTitle.Text = DataTable.SelectedRows(0).Cells(1).Value.ToString
            TextAuthor.Text = DataTable.SelectedRows(0).Cells(2).Value.ToString
            TextPrice.Text = DataTable.SelectedRows(0).Cells(3).Value.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'check if the input fields are empty

        If Not (String.IsNullOrEmpty(TextTitle.Text) Or String.IsNullOrEmpty(TextAuthor.Text) Or String.IsNullOrEmpty(TextPrice.Text)) Then

            'check if the given price contains alphabets or symbols

            If Integer.TryParse(TextPrice.Text, Nothing) Then

                'connecting to MySQL

                sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database

                Try
                    sqlConn.Open()

                    'fire querry in database

                    sqlQuerry = "INSERT INTO visual_basic.books(title, author, price) VALUES ('" & TextTitle.Text & "', '" & TextAuthor.Text & "', '" & TextPrice.Text & "')"
                    sqlCommand = New MySqlCommand(sqlQuerry, sqlConn)
                    sqlRead = sqlCommand.ExecuteReader
                    sqlConn.Close()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Book Management System", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Finally
                    sqlConn.Dispose()
                End Try

                'update the table

                updateTable()
            Else
                MessageBox.Show("Price should be in Numbers")
            End If
        Else
            MessageBox.Show("Fields cannot be empty")
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        'Clear all the textboxes

        TextTitle.Clear()
        TextAuthor.Clear()
        TextPrice.Clear()
        TextSearch.Clear()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        'connecting to MySQL

        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn

        'update the selected book in the database

        sqlCommand.CommandText = "UPDATE visual_basic.books SET title = '" & TextTitle.Text & "', author = '" & TextAuthor.Text & "', price = '" & TextPrice.Text & "' WHERE id = '" & id & "'"
        sqlCommand.CommandType = CommandType.Text
        sqlCommand.ExecuteNonQuery()
        sqlConn.Close()

        'Update the TABLE

        updateTable()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        'connecting to MySQL

        sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database
        sqlConn.Open()
        sqlCommand.Connection = sqlConn

        'delete the selected book from database

        sqlCommand.CommandText = "DELETE FROM books WHERE id = '" & id & "'"
        sqlCommand.CommandType = CommandType.Text
        sqlCommand.ExecuteNonQuery()
        sqlConn.Close()

        'remove the selected row from the DataTable

        For Each row As DataGridViewRow In DataTable.SelectedRows
            DataTable.Rows.Remove(row)
        Next

        'update the table

        updateTable()
    End Sub

    Private Sub TextSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextSearch.KeyPress
        Try

            'when enter is pressed search for the title or author which matches the given text in TextBox TextSearch

            If Asc(e.KeyChar) = 13 Then
                Dim dv As DataView
                dv = sqlDataTable.DefaultView
                dv.RowFilter = String.Format("Title like '%{0}%' OR Author like '%{0}%'", TextSearch.Text)

                'put the search result in the data table

                DataTable.DataSource = dv.ToTable()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class