Public Class Form1
    Private opusenc_version As String = String.Empty
    Private ffmpeg_version As String = String.Empty
    Public Running As Boolean = False

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
    Private Sub DisableElements()
        StartBtn.Enabled = False
        InputTxt.Enabled = False
        OutputTxt.Enabled = False
        InputBrowseBtn.Enabled = False
        InputFileBtn.Enabled = False
        OutputBrowseBtn.Enabled = False
        BitrateNumberBox.Enabled = False
        enableMultithreading.Enabled = False
        Running = True
    End Sub
    Private Sub StartBtn_Click(sender As Object, e As EventArgs) Handles StartBtn.Click
        If InputTxt.Text = String.Empty Then
            MessageBox.Show("There was no input file or folder specified. Cannot encode")
            Exit Sub
        End If
        DisableElements()
        Dim StartTasks As New Threading.Thread(Sub() StartThreads())
        StartTasks.Start()
    End Sub
    Public Sub StartGoogleDriveEncodes(GDriveItemsToProcess As List(Of String), GDriveItemIDs As List(Of String))
        DisableElements()
        Dim StartTasks As New Threading.Thread(Sub() StartThreads(True, GDriveItemsToProcess, GDriveItemIDs))
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
    Public Sub StartThreads(Optional GoogleDrive As Boolean = False, Optional GDriveItemsToProcess As List(Of String) = Nothing, Optional GDriveItemIDs As List(Of String) = Nothing)
        If Not String.IsNullOrEmpty(OutputTxt.Text) Then If Not IO.Directory.Exists(OutputTxt.Text) Then IO.Directory.CreateDirectory(OutputTxt.Text)
        Dim ItemsToProcess As List(Of String) = New List(Of String)
        Dim ItemsToDelete As List(Of String) = New List(Of String)
        Dim FileAlreadyExist As List(Of String) = New List(Of String)
        Dim ErrorList As List(Of String) = New List(Of String)
        Dim IgnoreFilesWithExtensions As String = String.Empty
        If IO.File.Exists("ignore.txt") Then IgnoreFilesWithExtensions = My.Computer.FileSystem.ReadAllText("ignore.txt")
        If IO.Directory.Exists(InputTxt.Text) Or GoogleDrive Then
            Dim Items As Object
            If Not GoogleDrive Then
                Items = IO.Directory.GetFiles(InputTxt.Text)
            Else
                Items = GDriveItemsToProcess
                ItemsToDelete = GDriveItemsToProcess
            End If
            For Each File In Items
                If (IO.Path.GetExtension(File) = ".wav" Or IO.Path.GetExtension(File) = ".flac" Or IO.Path.GetExtension(File) = ".opus" And EncOpusenc.Checked) Or EncFfmpeg1.Checked Or EncFFmpeg2.Checked Then
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
                        If Not IO.File.Exists(OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File)) Then
                            If Not IgnoreFilesWithExtensions.Contains(IO.Path.GetExtension(File)) Then My.Computer.FileSystem.CopyFile(File, OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File))
                        End If
                    End If
                End If
            Next
        Else
            ItemsToProcess.Add(InputTxt.Text)
        End If
        ProgressBar1.BeginInvoke(Sub()
                                     If Not GoogleDrive Then
                                         ProgressBar1.Maximum = ItemsToProcess.Count
                                     Else
                                         ProgressBar1.Maximum = ItemsToProcess.Count * 2
                                     End If
                                     ProgressBar1.Value = 0
                                 End Sub)
        If GoogleDrive Then
            For Counter As Integer = 0 To GDriveItemIDs.Count - 1
                Dim fileStream As New IO.FileStream(GDriveItemsToProcess(Counter), IO.FileMode.Create)
                GoogleDriveForm.drive.DownloadFile(GDriveItemIDs(Counter), fileStream)
                fileStream.Close()
                ProgressBar1.BeginInvoke(Sub() ProgressBar1.PerformStep())
            Next
        End If
        Dim tasks = New List(Of Action)
        If enableMultithreading.Checked Then
            For Counter As Integer = 0 To ItemsToProcess.Count - 1
                Dim args As Array = {ItemsToProcess(Counter), GetOutputPath(OutputTxt.Text, ItemsToProcess(Counter)), My.Settings.Bitrate}
                If Not IO.File.Exists(args(1)) Then
                    If EncOpusenc.Checked Then
                        tasks.Add(Function() Run_opus(args, "opusenc", "opusenc"))
                    ElseIf EncFfmpeg1.Checked Then
                        tasks.Add(Function() Run_opus(args, "ffmpeg1", "ffmpeg"))
                    Else
                        tasks.Add(Function() Run_opus(args, "ffmpeg2", "ffmpeg"))
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
                        Run_opus(args, "opusenc", "opusenc")
                    ElseIf EncFfmpeg1.Checked Then
                        Run_opus(args, "ffmpeg1", "ffmpeg")
                    ElseIf EncFFmpeg2.Checked Then
                        Run_opus(args, "ffmpeg2", "ffmpeg")
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
        Running = False
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
    Private Function Run_opus(args As Array, encoder As String, encoderExe As String)
        Dim Input_File As String = args(0)
        Dim Output_File As String = args(1)
        Dim Bitrate As String = args(2)
        Dim opusProcessInfo As New ProcessStartInfo
        Dim opusProcess As Process
        opusProcessInfo.FileName = encoderExe + ".exe"
        Select Case encoder
            Case "opusenc"
                opusProcessInfo.Arguments = "--music --bitrate " & Bitrate & " """ + Input_File + """ """ + Output_File + """"
            Case "ffmpeg1"
                opusProcessInfo.Arguments = "-i """ + Input_File + """ -c:a libopus -application audio -b:a " & Bitrate & "K """ + Output_File + """"
            Case "ffmpeg2"
                opusProcessInfo.Arguments = "-i """ + Input_File + """ -c:a opus -strict -2 -b:a " & Bitrate & "K """ + Output_File + """"
        End Select
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
        EncFfmpeg1.Checked = My.Settings.EncFfmpeg
        EncFFmpeg2.Checked = My.Settings.EncFfmpeg2
        EncOpusenc.Checked = My.Settings.EncOpusenc
        If Not EncFfmpeg1.Checked And Not EncFFmpeg2.Checked And Not EncOpusenc.Checked Then EncOpusenc.Checked = True
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
            ffmpegVersionLabel.Text = ffmpeg_version
        Catch ex As Exception
            ffmpegVersionLabel.Text = "ffmpegenc.exe was not found."
            EncFfmpeg1.Enabled = False
            EncFFmpeg2.Enabled = False
            EncOpusenc.Checked = True
        End Try
    End Sub

    Private Function OpusEncExists() As Boolean 'Currently not used
        If IO.File.Exists("opusenc.exe") Then
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
        My.Settings.Save()
    End Sub

    Private Sub EncFfmpeg_CheckedChanged(sender As Object, e As EventArgs) Handles EncFfmpeg1.CheckedChanged
        My.Settings.EncFfmpeg = EncFfmpeg1.Checked
        My.Settings.Save()
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

    Private Sub EncFFmpeg2_CheckedChanged(sender As Object, e As EventArgs) Handles EncFFmpeg2.CheckedChanged
        My.Settings.EncFfmpeg2 = EncFFmpeg2.Checked
        My.Settings.Save()
    End Sub

    Private Sub GoogleDriveButton_Click(sender As Object, e As EventArgs) Handles GoogleDriveButton.Click
        Dim gdriveForm As New GoogleDriveForm With {.Owner = Me}
        gdriveForm.Show()
    End Sub
End Class
