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
        StartBtn.Enabled = False
        InputTxt.Enabled = False
        OutputTxt.Enabled = False
        InputBrowseBtn.Enabled = False
        OutputBrowseBtn.Enabled = False
        BitrateTextBox.Enabled = False
        Dim StartTasks As New Threading.Thread(Sub() StartThreads())
        StartTasks.Start()
    End Sub
    Private Sub StartThreads()
        If Not My.Computer.FileSystem.DirectoryExists(OutputTxt.Text) Then My.Computer.FileSystem.CreateDirectory(OutputTxt.Text)
        Dim ItemsToProcess As List(Of String) = New List(Of String)
        For Each File In IO.Directory.GetFiles(InputTxt.Text)
            If IO.Path.GetExtension(File) = ".wav" Or IO.Path.GetExtension(File) = ".flac" Or IO.Path.GetExtension(File) = ".opus" Then
                ItemsToProcess.Add(File)
            Else
                If Not My.Computer.FileSystem.FileExists(OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File)) Then
                    My.Computer.FileSystem.CopyFile(File, OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File))
                End If
            End If
        Next
        ProgressBar1.BeginInvoke(Sub()
                                     ProgressBar1.Maximum = ItemsToProcess.Count
                                     ProgressBar1.Value = 0
                                 End Sub
        )
        Dim tasks = New Task(ItemsToProcess.Count - 1) {}
        For Counter As Integer = 0 To ItemsToProcess.Count - 1
            Dim args As Array = {ItemsToProcess(Counter), OutputTxt.Text + "\" + IO.Path.GetFileNameWithoutExtension(ItemsToProcess(Counter)) + ".opus", My.Settings.Bitrate}
            tasks(Counter) = Task.Factory.StartNew(Function() Run_opus(args))

        Next
        Task.WaitAll(tasks)
        StartBtn.BeginInvoke(Sub()
                                 StartBtn.Enabled = True
                                 BitrateTextBox.Enabled = True
                                 InputTxt.Enabled = True
                                 OutputTxt.Enabled = True
                                 InputBrowseBtn.Enabled = True
                                 OutputBrowseBtn.Enabled = True
                             End Sub)
        MsgBox("Finished")
    End Sub
    Private Function Run_opus(args As Array)
        Dim Input_File As String = args(0)
        Dim Output_File As String = args(1)
        Dim Bitrate As String = args(2)
        Dim flacProcessInfo As New ProcessStartInfo
        Dim flacProcess As Process
        flacProcessInfo.FileName = "opusenc.exe"
        flacProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """ """ + Output_File + """"
        flacProcessInfo.CreateNoWindow = True
        flacProcessInfo.RedirectStandardOutput = True
        flacProcessInfo.UseShellExecute = False
        flacProcess = Process.Start(flacProcessInfo)
        flacProcess.WaitForExit()
        Return True
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BitrateTextBox.Text = My.Settings.Bitrate
    End Sub

    Private Sub BitrateTextBox_TextChanged(sender As Object, e As EventArgs) Handles BitrateTextBox.TextChanged
        My.Settings.Bitrate = BitrateTextBox.Text
        My.Settings.Save()
    End Sub
End Class
