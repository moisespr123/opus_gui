Public Class GoogleDriveForm
    Public Shared drive As GoogleDriveClass
    Private Sub GoogleDriveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        drive = New GoogleDriveClass("opus_gui")
        If drive.connected Then
            PopulateListBoxes(drive)
        Else
            MessageBox.Show("client_secret.json or credentials.json file not found. Please follow Step 1 in this guide: https://developers.google.com/drive/v3/web/quickstart/dotnet" & Environment.NewLine + Environment.NewLine & "This file should be located in the folder where this software is located.")
            Process.Start("https://developers.google.com/drive/v3/web/quickstart/dotnet")
            Close()
        End If
    End Sub
    Private Sub PopulateListBoxes(ByVal drive As GoogleDriveClass, ByVal Optional location As String = "root", ByVal Optional refreshing As Boolean = False)
        If location = "back" Then
            drive.GoBack()
        Else
            If Not refreshing Then
                drive.GetData(location)
            Else
                drive.GetData(location, False, True)
            End If
        End If
        FoldersListBox.Items.Clear()
        FilesListBox.Items.Clear()
        Try
            If drive.FolderList.Count > 0 Then
                For Each item As String In drive.FolderList
                    FoldersListBox.Items.Add(item)
                Next
            End If
            If drive.FileList.Count > 0 Then
                For Each item As String In drive.FileList
                    FilesListBox.Items.Add(item)
                Next
                EncodeAll.Enabled = True
            Else
                EncodeAll.Enabled = False
            End If
            If drive.previousFolder.Count < 1 Then
                GoBackButton.Enabled = False
            Else
                GoBackButton.Enabled = True
            End If
            Me.Text = "Google Drive Browser - " + drive.currentFolderName
            FolderName.Text = drive.currentFolderName
        Catch
            MessageBox.Show("Error loading Google Drive contents")
        End Try
    End Sub
    Private Sub GoToRootButton_Click(sender As Object, e As EventArgs) Handles GoToRootButton.Click
        PopulateListBoxes(drive)
    End Sub

    Private Sub GoBackButton_Click(sender As Object, e As EventArgs) Handles GoBackButton.Click
        PopulateListBoxes(drive, "back")
    End Sub

    Private Sub FilesListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilesListBox.SelectedIndexChanged
        If FilesListBox.SelectedIndex > -1 Then
            EncodeSelected.Enabled = True
        Else
            EncodeSelected.Enabled = False
        End If
    End Sub

    Private Sub EncodeSelected_Click(sender As Object, e As EventArgs) Handles EncodeSelected.Click
        Dim ItemNames As List(Of String) = New List(Of String)
        Dim ItemIDs As List(Of String) = New List(Of String)
        If FilesListBox.SelectedItems.Count > 0 Then
            For Each item In FilesListBox.SelectedItems
                ItemNames.Add(item)
                ItemIDs.Add(drive.FileListID(FilesListBox.Items.IndexOf(item)))
            Next
            StartEncodeTask(ItemNames, ItemIDs)
        Else
            MsgBox("You did not select an item to encode.")
        End If
    End Sub

    Private Sub EncodeAll_Click(sender As Object, e As EventArgs) Handles EncodeAll.Click
        Dim ItemNames As List(Of String) = New List(Of String)
        Dim ItemIDs As List(Of String) = New List(Of String)
        For Each item In FilesListBox.Items
            ItemNames.Add(item)
            ItemIDs.Add(drive.FileListID(FilesListBox.Items.IndexOf(item)))
        Next
        StartEncodeTask(ItemNames, ItemIDs)
    End Sub
    Private Sub StartEncodeTask(ItemNames As List(Of String), ItemIDs As List(Of String))
        If Not Form1.Running Then
            If Not Form1.OutputTxt.Text = String.Empty Then
                Form1.StartGoogleDriveEncodes(ItemNames, ItemIDs)
            Else
                MsgBox("The Output Path is empty. Please enter an Output Path before encoding files.")
            End If
        Else
            MessageBox.Show("An encoding task is currently running. Please wait for it to finish before encoding more files.")
        End If
    End Sub
    Private Sub FoldersListBox_DoubleClick(sender As Object, e As EventArgs) Handles FoldersListBox.DoubleClick
        If FoldersListBox.SelectedIndex > -1 Then PopulateListBoxes(drive, drive.FolderListID(FoldersListBox.SelectedIndex))
    End Sub

    Private Sub FoldersListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles FoldersListBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            If FoldersListBox.SelectedIndex > -1 Then
                PopulateListBoxes(drive, drive.FolderListID(FoldersListBox.SelectedIndex))
            End If
        ElseIf e.KeyCode = Keys.Back Then
            If (GoBackButton.Enabled) Then
                PopulateListBoxes(drive, "back")
            End If
        ElseIf (e.KeyCode = Keys.F5) Then
            PopulateListBoxes(drive, drive.currentFolder, True)
        End If
    End Sub

    Private Sub FolderName_Click(sender As Object, e As EventArgs) Handles FolderName.Click
        Clipboard.SetText(drive.currentFolderName)
    End Sub
End Class