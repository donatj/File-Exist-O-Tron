﻿Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1_Leave(sender, e)
        Dim skipped As New List(Of String)
        Dim notfound As New List(Of String)
        Dim issues As New List(Of String)
        If IO.Directory.Exists(TextBox1.Text) Then

            Dim fsentries As String() = IO.Directory.GetFiles(TextBox1.Text, "*.*", System.IO.SearchOption.TopDirectoryOnly)

            For Each line As String In FileBox.Lines
                line = line.Trim
                If line.Length > 0 Then

                    If line.Contains(".") And Not line.Contains("/") And Not line.Contains("\") Then
                        Dim local_line As String = line

                        Dim listed As Boolean = fsentries.Where(Function(a) a.EndsWith("\" & local_line, False, System.Globalization.CultureInfo.CurrentCulture)).Count > 0
                        Dim exists As Boolean = System.IO.File.Exists(TextBox1.Text & line)

                        If Not listed And Not exists Then
                            notfound.Add(line)
                            Console.WriteLine()
                        ElseIf Not listed Then
                            issues.Add(line)
                        End If

                    Else
                        skipped.Add(line)
                    End If
                End If
            Next

            TextErrors.Text = ""
            If notfound.Count > 0 Then
                TextErrors.Text &= "---Not Found---" & vbCrLf
                TextErrors.Text &= Join(notfound.ToArray, vbCrLf)
                TextErrors.Text &= vbCrLf & vbCrLf
            End If

            If skipped.Count > 0 Then
                TextErrors.Text &= "---Skipped---" & vbCrLf
                TextErrors.Text &= Join(skipped.ToArray, vbCrLf)
                TextErrors.Text &= vbCrLf & vbCrLf
            End If

            If issues.Count > 0 Then
                TextErrors.Text &= "---Capitilization Mismatch---" & vbCrLf
                TextErrors.Text &= Join(issues.ToArray, vbCrLf)
                TextErrors.Text &= vbCrLf & vbCrLf
            End If

            If TextErrors.Text.Trim.Length = 0 Then
                TextErrors.Text = "No Issues, Woohoo!!!"
            End If
        Else
            MsgBox("Path """ & TextBox1.Text & """ Not Found")
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.LastPath.Trim
        If TextBox1.Text = "" Or TextBox1.Text = "\" Then
            TextBox1.Text = My.Computer.FileSystem.SpecialDirectories.Desktop
        End If
        FileBox.Text = My.Settings.LastFiles
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If IO.Directory.Exists(TextBox1.Text) Then
            FolderBrowserDialog1.SelectedPath = TextBox1.Text
        End If
        FolderBrowserDialog1.ShowDialog()
        TextBox1.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub TextBox1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Leave
        If Not TextBox1.Text.EndsWith("\") Then
            TextBox1.Text &= "\"
        End If

        If TextBox1.Text = "\" Then
            TextBox1.Text = My.Computer.FileSystem.SpecialDirectories.Desktop
        End If

        My.Settings.LastPath = TextBox1.Text
    End Sub

    Public Sub ClosingApp() Handles Me.FormClosing
        My.Settings.LastPath = TextBox1.Text
        My.Settings.LastFiles = FileBox.Text
    End Sub

    Private Sub FileBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FileBox.KeyPress
        If e.KeyChar = Convert.ToChar(1) Then
            DirectCast(sender, TextBox).SelectAll()
            e.Handled = True
        End If

    End Sub

    Private Sub FileBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileBox.TextChanged
        Dim hold As String

        For Each line As String In FileBox.Lines

            line = line.Trim
            If line <> "" Then
                hold &= line & vbCrLf
            End If

        Next

        FileBox.Text = hold

    End Sub

    Private Sub BtnClearList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClearList.Click
        FileBox.Clear()
    End Sub
End Class
