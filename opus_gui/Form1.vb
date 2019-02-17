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
        BitrateNumberBox.Enabled = False
        enableMultithreading.Enabled = False
        Dim StartTasks As New Threading.Thread(Sub() StartThreads())
        StartTasks.Start()
    End Sub
    Private Sub StartThreads()
        If Not String.IsNullOrEmpty(OutputTxt.Text) Then If Not My.Computer.FileSystem.DirectoryExists(OutputTxt.Text) Then My.Computer.FileSystem.CreateDirectory(OutputTxt.Text)
        Dim ItemsToProcess As List(Of String) = New List(Of String)
        Dim ItemsToDelete As List(Of String) = New List(Of String)
        Dim IgnoreFilesWithExtensions As String = String.Empty
        If My.Computer.FileSystem.FileExists("ignore.txt") Then IgnoreFilesWithExtensions = My.Computer.FileSystem.ReadAllText("ignore.txt")
        If IO.Directory.Exists(InputTxt.Text) Then
            For Each File In IO.Directory.GetFiles(InputTxt.Text)
                If (IO.Path.GetExtension(File) = ".wav" Or IO.Path.GetExtension(File) = ".flac" Or IO.Path.GetExtension(File) = ".opus" And EncOpusenc.Checked) Or EncFfmpeg.Checked Then
                    ItemsToProcess.Add(File)
                ElseIf IO.Path.GetExtension(File) = ".mp3" Or IO.Path.GetExtension(File) = ".m4a" And EncOpusenc.Checked Then
                    ffmpeg_preprocess(File, IO.Path.GetFileNameWithoutExtension(File))
                    ItemsToProcess.Add(IO.Path.GetFileNameWithoutExtension(File) + ".flac")
                    ItemsToDelete.Add(IO.Path.GetFileNameWithoutExtension(File) + ".flac")
                Else
                    If Not String.IsNullOrEmpty(OutputTxt.Text) Then
                        If Not My.Computer.FileSystem.FileExists(OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File)) Then
                            If Not IgnoreFilesWithExtensions.Contains(IO.Path.GetExtension(File)) Then My.Computer.FileSystem.CopyFile(File, OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File))
                        End If
                    End If
                End If
            Next
        Else
            ItemsToProcess.Add(InputTxt.Text)
        End If
        ProgressBar1.BeginInvoke(Sub()
                                     ProgressBar1.Maximum = ItemsToProcess.Count
                                     ProgressBar1.Value = 0
                                 End Sub
        )
        Dim tasks = New List(Of Action)
        Dim outputPath As String = ""
        If enableMultithreading.Checked Then
            For Counter As Integer = 0 To ItemsToProcess.Count - 1
                If Not String.IsNullOrEmpty(OutputTxt.Text) Then
                    outputPath = OutputTxt.Text + "\" + IO.Path.GetFileNameWithoutExtension(ItemsToProcess(Counter)) + ".opus"
                End If
                Dim args As Array = {ItemsToProcess(Counter), outputPath, My.Settings.Bitrate}
                If EncOpusenc.Checked Then
                    tasks.Add(Function() Run_opus(args, "opusenc"))
                Else
                    tasks.Add(Function() Run_opus(args, "ffmpeg"))
                End If
            Next
            Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = Environment.ProcessorCount}, tasks.ToArray())
        Else
            For Counter As Integer = 0 To ItemsToProcess.Count - 1
                If Not String.IsNullOrEmpty(OutputTxt.Text) Then
                    outputPath = OutputTxt.Text + "\" + IO.Path.GetFileNameWithoutExtension(ItemsToProcess(Counter)) + ".opus"
                End If
                Dim args As Array = {ItemsToProcess(Counter), outputPath, My.Settings.Bitrate}
                If EncOpusenc.Checked Then
                    tasks.Add(Function() Run_opus(args, "opusenc"))
                Else
                    tasks.Add(Function() Run_opus(args, "ffmpeg"))
                End If
            Next
        End If
        If ItemsToDelete.Count > 0 Then
            For Each item As String In ItemsToDelete
                My.Computer.FileSystem.DeleteFile(item)
            Next
        End If
        StartBtn.BeginInvoke(Sub()
                                 StartBtn.Enabled = True
                                 BitrateNumberBox.Enabled = True
                                 enableMultithreading.Enabled = True
                                 InputTxt.Enabled = True
                                 OutputTxt.Enabled = True
                                 InputBrowseBtn.Enabled = True
                                 OutputBrowseBtn.Enabled = True
                             End Sub)
        MsgBox("Finished")
    End Sub
    Private Function Run_opus(args As Array, encoder As String)
        Dim Input_File As String = args(0)
        Dim Output_File As String = args(1)
        Dim Bitrate As String = args(2)
        Dim opusProcessInfo As New ProcessStartInfo
        Dim opusProcess As Process
        opusProcessInfo.FileName = encoder + ".exe"
        If encoder = "opusenc" Then
            If Not String.IsNullOrEmpty(Output_File) Then
                opusProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """ """ + Output_File + """"
            Else
                opusProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """"
            End If
        Else
            If Not String.IsNullOrEmpty(Output_File) Then
                opusProcessInfo.Arguments = "-i """ + Input_File + """ -c:a libopus -b:a " & Bitrate & "K """ + Output_File + """"
            Else
                opusProcessInfo.Arguments = "-i """ + Input_File + """ -c:a libopus -b:a " & Bitrate & "K """ + Input_File + ".opus"""
            End If
        End If
        opusProcessInfo.CreateNoWindow = True
        opusProcessInfo.RedirectStandardOutput = False
        opusProcessInfo.UseShellExecute = False
        opusProcess = Process.Start(opusProcessInfo)
        opusProcess.WaitForExit()
        ProgressBar1.BeginInvoke(Sub() ProgressBar1.PerformStep())
        Return True
    End Function
    Private Function ffmpeg_preprocess(Input As String, Output As String)
        Dim ffmpegProcessInfo As New ProcessStartInfo
        Dim ffmpegProcess As Process
        ffmpegProcessInfo.FileName = "ffmpeg.exe"
        ffmpegProcessInfo.Arguments = "-i """ + Input + """ -c:a flac """ + Output + ".flac"" -y"
        ffmpegProcessInfo.CreateNoWindow = True
        ffmpegProcessInfo.RedirectStandardOutput = False
        ffmpegProcessInfo.UseShellExecute = False
        ffmpegProcess = Process.Start(ffmpegProcessInfo)
        ffmpegProcess.WaitForExit()
        Return True
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BitrateNumberBox.Value = My.Settings.Bitrate
        enableMultithreading.Checked = My.Settings.Multithreading
        EncFfmpeg.Checked = My.Settings.EncFfmpeg
        EncOpusenc.Checked = My.Settings.EncOpusenc
        If Not EncFfmpeg.Checked And Not EncOpusenc.Checked Then EncOpusenc.Checked = True
        IO.Directory.SetCurrentDirectory(IO.Path.GetDirectoryName(Process.GetCurrentProcess.MainModule.FileName))
        If OpusEncExists() Then
            GetOpusencVersion()
        Else
            MessageBox.Show("opusenc.exe was not found. Exiting...")
            Me.Close()
        End If
        Dim vars As String() = Environment.GetCommandLineArgs
        If vars.Count > 1 Then
            InputTxt.Text = vars(1)
        End If
    End Sub


    Private Sub GetOpusencVersion()
        Dim opusProcessInfo As New ProcessStartInfo
        Dim opusProcess As Process
        opusProcessInfo.FileName = "opusenc.exe"
        opusProcessInfo.Arguments = "-V"
        opusProcessInfo.CreateNoWindow = True
        opusProcessInfo.RedirectStandardOutput = True
        opusProcessInfo.UseShellExecute = False
        opusProcess = Process.Start(opusProcessInfo)
        opusProcess.WaitForExit()
        OpusVersionLabel.Text = "opusenc version: " + opusProcess.StandardOutput.ReadLine()
    End Sub

    Private Function OpusEncExists() As Boolean
        If My.Computer.FileSystem.FileExists("opusenc.exe") Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles BitrateNumberBox.ValueChanged
        My.Settings.Bitrate = BitrateNumberBox.Value
        My.Settings.Save()
    End Sub

    Private Sub EnableMultithreading_CheckedChanged(sender As Object, e As EventArgs) Handles enableMultithreading.CheckedChanged
        My.Settings.Multithreading = enableMultithreading.Checked
        My.Settings.Save()
    End Sub
    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        InputTxt.Text = CType(e.Data.GetData(DataFormats.FileDrop), String())(0)
    End Sub

    Private Sub EncOpusenc_CheckedChanged(sender As Object, e As EventArgs) Handles EncOpusenc.CheckedChanged
        My.Settings.EncOpusenc = EncOpusenc.Checked
    End Sub

    Private Sub EncFfmpeg_CheckedChanged(sender As Object, e As EventArgs) Handles EncFfmpeg.CheckedChanged
        My.Settings.EncFfmpeg = EncFfmpeg.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim InputBrowser As New OpenFileDialog With {
          .Title = "Browse for a music file:",
          .FileName = ""
      }
        Dim OkAction As MsgBoxResult = InputBrowser.ShowDialog
        If OkAction = MsgBoxResult.Ok Then
            InputTxt.Text = InputBrowser.FileName
        End If
    End Sub
End Class
