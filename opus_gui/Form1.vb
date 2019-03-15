Public Class Form1
    Private opusenc_version As String = String.Empty
    Private ffmpeg_version As String = String.Empty

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
        InputFileBtn.Enabled = False
        OutputBrowseBtn.Enabled = False
        BitrateNumberBox.Enabled = False
        enableMultithreading.Enabled = False
        Dim StartTasks As New Threading.Thread(Sub() StartThreads())
        StartTasks.Start()
    End Sub

    Private Function GetOutputPath(OutputFolder As String, Item As String) As String
        Dim outputPath As String = String.Empty
        If Not String.IsNullOrEmpty(OutputFolder) Then
            outputPath = OutputTxt.Text + "\" + IO.Path.GetFileNameWithoutExtension(Item) + ".opus"
        Else
            outputPath = IO.Path.GetDirectoryName(Item) + "\" + IO.Path.GetFileNameWithoutExtension(Item) + ".opus"
        End If
        Return outputPath
    End Function
    Private Sub StartThreads()
        If Not String.IsNullOrEmpty(OutputTxt.Text) Then If Not My.Computer.FileSystem.DirectoryExists(OutputTxt.Text) Then My.Computer.FileSystem.CreateDirectory(OutputTxt.Text)
        Dim ItemsToProcess As List(Of String) = New List(Of String)
        Dim ItemsToDelete As List(Of String) = New List(Of String)
        Dim FileAlreadyExist As List(Of String) = New List(Of String)
        Dim ErrorList As List(Of String) = New List(Of String)
        Dim IgnoreFilesWithExtensions As String = String.Empty
        If My.Computer.FileSystem.FileExists("ignore.txt") Then IgnoreFilesWithExtensions = My.Computer.FileSystem.ReadAllText("ignore.txt")
        If IO.Directory.Exists(InputTxt.Text) Then
            For Each File In IO.Directory.GetFiles(InputTxt.Text)
                If (IO.Path.GetExtension(File) = ".wav" Or IO.Path.GetExtension(File) = ".flac" Or IO.Path.GetExtension(File) = ".opus" And EncOpusenc.Checked) Or EncFfmpeg.Checked Then
                    ItemsToProcess.Add(File)
                ElseIf IO.Path.GetExtension(File) = ".mp3" Or IO.Path.GetExtension(File) = ".m4a" And EncOpusenc.Checked Then
                    If Not ffmpeg_version = String.Empty Then
                        ffmpeg_preprocess(File, IO.Path.GetFileNameWithoutExtension(File))
                        ItemsToProcess.Add(IO.Path.GetFileNameWithoutExtension(File) + ".flac")
                        ItemsToDelete.Add(IO.Path.GetFileNameWithoutExtension(File) + ".flac")
                    Else
                        ErrorList.Add(File)
                    End If
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
                                 End Sub)
        Dim tasks = New List(Of Action)
        If enableMultithreading.Checked Then
            For Counter As Integer = 0 To ItemsToProcess.Count - 1
                Dim args As Array = {ItemsToProcess(Counter), GetOutputPath(OutputTxt.Text, ItemsToProcess(Counter)), My.Settings.Bitrate}
                If Not IO.File.Exists(args(1)) Then
                    If EncOpusenc.Checked Then
                        tasks.Add(Function() Run_opus(args, "opusenc"))
                    Else
                        tasks.Add(Function() Run_opus(args, "ffmpeg"))
                    End If
                Else
                    FileAlreadyExist.Add(args(1))
                End If
            Next
            Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = Environment.ProcessorCount}, tasks.ToArray())
        Else
            For Counter As Integer = 0 To ItemsToProcess.Count - 1
                Dim args As Array = {ItemsToProcess(Counter), GetOutputPath(OutputTxt.Text, ItemsToProcess(Counter)), My.Settings.Bitrate}
                If Not IO.File.Exists(args(1)) Then
                    If EncOpusenc.Checked Then
                        Run_opus(args, "opusenc")
                    Else
                        Run_opus(args, "ffmpeg")
                    End If
                Else
                    FileAlreadyExist.Add(args(1))
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
                                 InputFileBtn.Enabled = True
                                 OutputBrowseBtn.Enabled = True
                             End Sub)

        Dim MessageToShow As String = "Finished!"
        If FileAlreadyExist.Count > 0 Then
            MessageToShow += Environment.NewLine + Environment.NewLine + "The following file(s) could not be encoded because there's an output file with the same filename at the destination folder:" + Environment.NewLine
            For Each item As String In FileAlreadyExist
                MessageToShow += "- " + item + Environment.NewLine
            Next
        End If
        If ErrorList.Count > 0 Then
            MessageToShow += Environment.NewLine + Environment.NewLine + "The following file(s) could not be encoded. This could happen if you used opusenc but the files are not compatible and you don't have ffmpeg in your system:" + Environment.NewLine
            For Each item As String In FileAlreadyExist
                MessageToShow += "- " + item + Environment.NewLine
            Next
        End If
        MsgBox(MessageToShow)
    End Sub
    Private Function Run_opus(args As Array, encoder As String)
        Dim Input_File As String = args(0)
        Dim Output_File As String = args(1)
        Dim Bitrate As String = args(2)
        Dim opusProcessInfo As New ProcessStartInfo
        Dim opusProcess As Process
        opusProcessInfo.FileName = encoder + ".exe"
        If encoder = "opusenc" Then
            opusProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """ """ + Output_File + """"
        Else
            If Not String.IsNullOrEmpty(Output_File) Then
                opusProcessInfo.Arguments = "-i """ + Input_File + """ -c:a libopus -application audio -b:a " & Bitrate & "K """ + Output_File + """"
            End If
        End If
        opusProcessInfo.WorkingDirectory = IO.Path.GetDirectoryName(Input_File)
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
        GetOpusencVersion()
        GetFFmpegVersion()
        Dim vars As String() = Environment.GetCommandLineArgs
        If vars.Count > 1 Then
            InputTxt.Text = vars(1)
        End If
    End Sub


    Private Sub GetOpusencVersion()
        Try
            Dim opusProcessInfo As New ProcessStartInfo
            Dim opusProcess As Process
            opusProcessInfo.FileName = "opusenc.exe"
            opusProcessInfo.Arguments = "-V"
            opusProcessInfo.CreateNoWindow = True
            opusProcessInfo.RedirectStandardOutput = True
            opusProcessInfo.UseShellExecute = False
            opusProcess = Process.Start(opusProcessInfo)
            opusProcess.WaitForExit()
            opusenc_version = opusProcess.StandardOutput.ReadLine()
            OpusVersionLabel.Text = "opusenc version: " + opusenc_version
        Catch ex As Exception
            MessageBox.Show("opusenc.exe was not found. Exiting...")
            Process.Start("https://moisescardona.me/opusenc-builds/")
            Me.Close()
        End Try
    End Sub
    Private Sub GetFFmpegVersion()
        Try
            Dim ffmpegProcessInfo As New ProcessStartInfo
            Dim ffmpegProcess As Process
            ffmpegProcessInfo.FileName = "ffmpeg.exe"
            ffmpegProcessInfo.CreateNoWindow = True
            ffmpegProcessInfo.RedirectStandardError = True
            ffmpegProcessInfo.UseShellExecute = False
            ffmpegProcess = Process.Start(ffmpegProcessInfo)
            ffmpegProcess.WaitForExit()
            ffmpeg_version = ffmpegProcess.StandardError.ReadLine()
            ffmpegVersionLabel.Text = "ffmpeg version: " + ffmpeg_version
        Catch ex As Exception
            ffmpegVersionLabel.Text = "ffmpegenc.exe was not found."
            EncFfmpeg.Enabled = False
            EncOpusenc.Checked = True
        End Try
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
        If InputTxt.Enabled Then
            InputTxt.Text = CType(e.Data.GetData(DataFormats.FileDrop), String())(0)
        End If
    End Sub

    Private Sub EncOpusenc_CheckedChanged(sender As Object, e As EventArgs) Handles EncOpusenc.CheckedChanged
        My.Settings.EncOpusenc = EncOpusenc.Checked
    End Sub

    Private Sub EncFfmpeg_CheckedChanged(sender As Object, e As EventArgs) Handles EncFfmpeg.CheckedChanged
        My.Settings.EncFfmpeg = EncFfmpeg.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles InputFileBtn.Click
        Dim InputBrowser As New OpenFileDialog With {
          .Title = "Browse for a music file:",
          .FileName = ""
      }
        Dim OkAction As MsgBoxResult = InputBrowser.ShowDialog
        If OkAction = MsgBoxResult.Ok Then
            InputTxt.Text = InputBrowser.FileName
        End If
    End Sub

    Private Sub FfmpegVersionLabel_Click(sender As Object, e As EventArgs) Handles ffmpegVersionLabel.Click
        If Not ffmpeg_version = String.Empty Then
            Clipboard.SetText(ffmpeg_version)
        End If
    End Sub

    Private Sub OpusVersionLabel_Click(sender As Object, e As EventArgs) Handles OpusVersionLabel.Click
        If Not opusenc_version = String.Empty Then
            Clipboard.SetText(opusenc_version)
        End If
    End Sub
End Class
