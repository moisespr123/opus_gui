Imports System.IO.Pipes

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
        CPUThreads.Enabled = False
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
            outputPath = IO.Path.ChangeExtension(Item, ".opus")
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
        Dim Item_Type As Integer = 0
        If IO.File.Exists("ignore.txt") Then IgnoreFilesWithExtensions = My.Computer.FileSystem.ReadAllText("ignore.txt")
        If IO.Directory.Exists(InputTxt.Text) Or GoogleDrive Then
            Dim Items As Object
            If Not GoogleDrive Then
                Item_Type = 0
                Items = IO.Directory.GetFiles(InputTxt.Text)
            Else
                Item_Type = 1
                Items = GDriveItemsToProcess
            End If
            For Each File In Items
                If Not String.IsNullOrEmpty(OutputTxt.Text) Then
                    If Not IO.File.Exists(OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File)) Then
                        If Not IgnoreFilesWithExtensions.Contains(IO.Path.GetExtension(File)) Then
                            ItemsToProcess.Add(File)
                        Else
                            If Item_Type = 0 Then
                                My.Computer.FileSystem.CopyFile(File, OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File))
                            Else
                                GoogleDriveForm.drive.DownloadFile(GDriveItemIDs(GDriveItemsToProcess.IndexOf(File)), New IO.FileStream(OutputTxt.Text + "\" + My.Computer.FileSystem.GetName(File), IO.FileMode.CreateNew))
                            End If
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
                Dim args As Array
                If GoogleDrive Then
                    args = {GDriveItemIDs(Counter), 1, GetOutputPath(OutputTxt.Text, ItemsToProcess(Counter)), IO.Path.GetExtension(ItemsToProcess(Counter)), My.Settings.Bitrate}
                Else
                    args = {ItemsToProcess(Counter), 0, GetOutputPath(OutputTxt.Text, ItemsToProcess(Counter)), IO.Path.GetExtension(ItemsToProcess(Counter)), My.Settings.Bitrate}
                End If
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
            Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = CPUThreads.Value}, tasks.ToArray())
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
                                 CPUThreads.Enabled = True
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
    Private Function Download_Files(id As String) As IO.MemoryStream
        Using memoryStream As New IO.MemoryStream
            GoogleDriveForm.drive.DownloadFile(id, memoryStream)
            Return memoryStream
        End Using
    End Function
    Private Function Run_opus(args As Array, encoder As String, encoderExe As String) As Boolean
        Dim Input_File As String = args(0)
        Dim Input_Type As Integer = args(1)
        Dim Output_File As String = args(2)
        Dim FileExtension As String = args(3)
        Dim Bitrate As String = args(4)
        Dim opusProcessInfo As New ProcessStartInfo
        Dim opusProcess As Process
        opusProcessInfo.FileName = encoderExe + ".exe"
        Dim Data As Byte()
        If Input_Type = 0 Then
            Data = IO.File.ReadAllBytes(Input_File)
        Else
            Data = Download_Files(Input_File).ToArray()
        End If
        Select Case encoder
            Case "opusenc"
                If Not (FileExtension = ".wav" Or FileExtension = ".flac" Or FileExtension = ".opus") Then
                    If Not ffmpeg_version = String.Empty Then
                        Data = ffmpeg_preprocess(Data, Input_File)
                    Else
                        Return False
                    End If
                End If
                opusProcessInfo.Arguments = "--music --bitrate " & Bitrate & " - """ + Output_File + """"
            Case "ffmpeg1"
                opusProcessInfo.Arguments = "-i - -c:a libopus -application audio -b:a " & Bitrate & "K -c:v copy """ + Output_File + """"
            Case "ffmpeg2"
                opusProcessInfo.Arguments = "-i - -c:a opus -strict -2 -b:a " & Bitrate & "K -c:v copy """ + Output_File + """"
        End Select
        opusProcessInfo.WorkingDirectory = IO.Path.GetDirectoryName(Input_File)
        opusProcessInfo.CreateNoWindow = True
        opusProcessInfo.RedirectStandardOutput = False
        opusProcessInfo.RedirectStandardInput = True
        opusProcessInfo.UseShellExecute = False
        opusProcess = Process.Start(opusProcessInfo)
        Dim ffmpegIn As IO.Stream = opusProcess.StandardInput.BaseStream
        ffmpegIn.Write(Data, 0, Data.Length)
        ffmpegIn.Flush()
        ffmpegIn.Close()
        opusProcess.WaitForExit()
        ProgressBar1.BeginInvoke(Sub() ProgressBar1.PerformStep())
        Return True
    End Function
    Private Function ffmpeg_preprocess(Input As Byte(), Filename As String) As Byte()
        Dim input_file As String = IO.Path.GetFileName(Filename)
        Dim output_file As String = IO.Path.GetFileNameWithoutExtension(Filename) + ".flac"
        Dim InputPipe As New NamedPipeServerStream(input_file, PipeDirection.Out, -1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 16384, 0)
        Dim OutputPipe As New NamedPipeServerStream(output_file, PipeDirection.In, -1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 0, 16384)
        Dim ffmpegProcessInfo As New ProcessStartInfo
        Dim ffmpegProcess As Process
        ffmpegProcessInfo.FileName = "ffmpeg.exe"
        ffmpegProcessInfo.Arguments = "-i ""\\.\pipe\" + input_file + """ -c:a flac -c:v copy ""\\.\pipe\" + output_file + """ -y"
        ffmpegProcessInfo.CreateNoWindow = True
        ffmpegProcessInfo.RedirectStandardInput = True
        ffmpegProcessInfo.RedirectStandardOutput = True
        ffmpegProcessInfo.UseShellExecute = False
        ffmpegProcess = Process.Start(ffmpegProcessInfo)
        WriteByteAsync(InputPipe, Input)
        Dim lastRead As Integer
        OutputPipe.WaitForConnection()
        Dim PipedOutput As Byte()
        Using ms As New IO.MemoryStream
            Dim buffer As Byte() = New Byte(16384) {}
            Do
                lastRead = OutputPipe.Read(buffer, 0, 16384)
                ms.Write(buffer, 0, lastRead)
            Loop While lastRead > 0
            PipedOutput = ms.ToArray()
            OutputPipe.Close()
        End Using
        ffmpegProcess.WaitForExit()
        Return PipedOutput
    End Function
    Private Async Sub WriteByteAsync(InputPipe As NamedPipeServerStream, Input As Byte())
        InputPipe.WaitForConnection()
        Dim ChunkSize As Integer = 16384
        For Bytes As Long = 0 To Input.Length Step 16384
            Try
                If Input.Length - Bytes < ChunkSize Then
                    ChunkSize = Input.Length - Bytes
                End If
                Await InputPipe.WriteAsync(Input, Bytes, ChunkSize)
            Catch
            End Try
        Next

        InputPipe.Flush()
        InputPipe.Dispose()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CPUThreads.Maximum = Environment.ProcessorCount
        If My.Settings.CPUThreads = 0 Then CPUThreads.Value = CPUThreads.Maximum Else CPUThreads.Value = My.Settings.CPUThreads
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

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Keys.F5 Then
            GetOpusencVersion()
            GetFFmpegVersion()
        End If
    End Sub

    Private Sub CPUThreads_ValueChanged(sender As Object, e As EventArgs) Handles CPUThreads.ValueChanged
        My.Settings.CPUThreads = CPUThreads.Value
        My.Settings.Save()
    End Sub
End Class
