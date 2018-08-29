Public Class Form1

    Private Sub InputBrowseBtn_Click(sender As Object, e As EventArgs) Handles InputBrowseBtn.Click
        Dim InputBrowser As New FolderBrowserDialog With {
            .ShowNewFolderButton = False
        }
        Dim OkAction As MsgBoxResult = InputBrowser.ShowDialog
        If OkAction = MsgBoxResult.Ok Then
            InputTxt.Text = InputBrowser.SelectedPath
        End If
    End Sub

    Private Sub OutputBrowseBtn_Click(sender As Object, e As EventArgs) Handles OutputBrowseBtn.Click
        Dim OutputBrowser As New FolderBrowserDialog With {
            .ShowNewFolderButton = True
        }
        Dim OkAction As MsgBoxResult = OutputBrowser.ShowDialog
        If OkAction = MsgBoxResult.Ok Then
            OutputTxt.Text = OutputBrowser.SelectedPath
        End If
    End Sub

    Private Sub StartBtn_Click(sender As Object, e As EventArgs) Handles StartBtn.Click
        If Not My.Computer.FileSystem.DirectoryExists(OutputTxt.Text) Then My.Computer.FileSystem.CreateDirectory(OutputTxt.Text)
        ProgressBar1.Maximum = My.Computer.FileSystem.GetFiles(InputTxt.Text).Count
        ProgressBar1.Value = 0
        For Each File In IO.Directory.GetFiles(InputTxt.Text)
            Dim FileWithoutExt As String = IO.Path.GetFileNameWithoutExtension(File)
            Run_opus(File, OutputTxt.Text + "\" + FileWithoutExt + ".opus", BitrateTextBox.Text)
            ProgressBar1.PerformStep()
            Me.Update()
        Next
        MsgBox("Finished")
    End Sub
    Private Sub Run_opus(Input_File As String, Output_File As String, Bitrate As String)
        Dim flacProcessInfo As New ProcessStartInfo
        Dim flacProcess As Process
        flacProcessInfo.FileName = "opusenc.exe"
        flacProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """ """ + Output_File + """"
        flacProcessInfo.CreateNoWindow = True
        flacProcessInfo.RedirectStandardOutput = True
        flacProcessInfo.UseShellExecute = False
        flacProcess = Process.Start(flacProcessInfo)
        flacProcess.WaitForExit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BitrateTextBox.Text = My.Settings.Bitrate
    End Sub

    Private Sub BitrateTextBox_TextChanged(sender As Object, e As EventArgs) Handles BitrateTextBox.TextChanged
        My.Settings.Bitrate = BitrateTextBox.Text
        My.Settings.Save()
    End Sub
End Class
