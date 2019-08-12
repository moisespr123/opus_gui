<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GoogleDriveForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.GoBackButton = New System.Windows.Forms.Button()
        Me.GoToRootButton = New System.Windows.Forms.Button()
        Me.FoldersListBox = New System.Windows.Forms.ListBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.panel2 = New System.Windows.Forms.Panel()
        Me.EncodeSelected = New System.Windows.Forms.Button()
        Me.EncodeAll = New System.Windows.Forms.Button()
        Me.FilesListBox = New System.Windows.Forms.ListBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.FolderName = New System.Windows.Forms.Label()
        Me.tableLayoutPanel1.SuspendLayout
        Me.panel1.SuspendLayout
        Me.panel2.SuspendLayout
        Me.SuspendLayout
        '
        'tableLayoutPanel1
        '
        Me.tableLayoutPanel1.ColumnCount = 2
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.tableLayoutPanel1.Controls.Add(Me.panel1, 0, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.panel2, 1, 0)
        Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        Me.tableLayoutPanel1.RowCount = 1
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.tableLayoutPanel1.Size = New System.Drawing.Size(800, 454)
        Me.tableLayoutPanel1.TabIndex = 2
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.FolderName)
        Me.panel1.Controls.Add(Me.GoBackButton)
        Me.panel1.Controls.Add(Me.GoToRootButton)
        Me.panel1.Controls.Add(Me.FoldersListBox)
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel1.Location = New System.Drawing.Point(3, 3)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(394, 448)
        Me.panel1.TabIndex = 0
        '
        'GoBackButton
        '
        Me.GoBackButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.GoBackButton.Location = New System.Drawing.Point(209, 416)
        Me.GoBackButton.Name = "GoBackButton"
        Me.GoBackButton.Size = New System.Drawing.Size(182, 23)
        Me.GoBackButton.TabIndex = 2
        Me.GoBackButton.Text = "Go Back"
        Me.GoBackButton.UseVisualStyleBackColor = true
        '
        'GoToRootButton
        '
        Me.GoToRootButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.GoToRootButton.Location = New System.Drawing.Point(9, 416)
        Me.GoToRootButton.Name = "GoToRootButton"
        Me.GoToRootButton.Size = New System.Drawing.Size(194, 23)
        Me.GoToRootButton.TabIndex = 3
        Me.GoToRootButton.Text = "Go to Root"
        Me.GoToRootButton.UseVisualStyleBackColor = true
        '
        'FoldersListBox
        '
        Me.FoldersListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.FoldersListBox.FormattingEnabled = true
        Me.FoldersListBox.Location = New System.Drawing.Point(9, 22)
        Me.FoldersListBox.Name = "FoldersListBox"
        Me.FoldersListBox.Size = New System.Drawing.Size(379, 381)
        Me.FoldersListBox.TabIndex = 1
        '
        'label1
        '
        Me.label1.AutoSize = true
        Me.label1.Location = New System.Drawing.Point(9, 6)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(44, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Folders:"
        '
        'panel2
        '
        Me.panel2.Controls.Add(Me.EncodeSelected)
        Me.panel2.Controls.Add(Me.EncodeAll)
        Me.panel2.Controls.Add(Me.FilesListBox)
        Me.panel2.Controls.Add(Me.label2)
        Me.panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel2.Location = New System.Drawing.Point(403, 3)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(394, 448)
        Me.panel2.TabIndex = 1
        '
        'EncodeSelected
        '
        Me.EncodeSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.EncodeSelected.Enabled = false
        Me.EncodeSelected.Location = New System.Drawing.Point(6, 416)
        Me.EncodeSelected.Name = "EncodeSelected"
        Me.EncodeSelected.Size = New System.Drawing.Size(182, 23)
        Me.EncodeSelected.TabIndex = 6
        Me.EncodeSelected.Text = "Encode Selected File(s)"
        Me.EncodeSelected.UseVisualStyleBackColor = true
        '
        'EncodeAll
        '
        Me.EncodeAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.EncodeAll.Location = New System.Drawing.Point(202, 416)
        Me.EncodeAll.Name = "EncodeAll"
        Me.EncodeAll.Size = New System.Drawing.Size(182, 23)
        Me.EncodeAll.TabIndex = 4
        Me.EncodeAll.Text = "Encode All Files"
        Me.EncodeAll.UseVisualStyleBackColor = true
        '
        'FilesListBox
        '
        Me.FilesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.FilesListBox.FormattingEnabled = true
        Me.FilesListBox.Location = New System.Drawing.Point(6, 22)
        Me.FilesListBox.Name = "FilesListBox"
        Me.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.FilesListBox.Size = New System.Drawing.Size(378, 381)
        Me.FilesListBox.TabIndex = 5
        '
        'label2
        '
        Me.label2.AutoSize = true
        Me.label2.Location = New System.Drawing.Point(3, 6)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(31, 13)
        Me.label2.TabIndex = 4
        Me.label2.Text = "Files:"
        '
        'FolderName
        '
        Me.FolderName.AutoSize = true
        Me.FolderName.Location = New System.Drawing.Point(59, 6)
        Me.FolderName.Name = "FolderName"
        Me.FolderName.Size = New System.Drawing.Size(67, 13)
        Me.FolderName.TabIndex = 4
        Me.FolderName.Text = "Folder Name"
        '
        'GoogleDriveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 454)
        Me.Controls.Add(Me.tableLayoutPanel1)
        Me.Name = "GoogleDriveForm"
        Me.Text = "Google Drive Browser"
        Me.tableLayoutPanel1.ResumeLayout(false)
        Me.panel1.ResumeLayout(false)
        Me.panel1.PerformLayout
        Me.panel2.ResumeLayout(false)
        Me.panel2.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Private WithEvents tableLayoutPanel1 As TableLayoutPanel
    Private WithEvents panel1 As Panel
    Private WithEvents GoBackButton As Button
    Private WithEvents GoToRootButton As Button
    Private WithEvents FoldersListBox As ListBox
    Private WithEvents label1 As Label
    Private WithEvents panel2 As Panel
    Private WithEvents EncodeSelected As Button
    Private WithEvents EncodeAll As Button
    Private WithEvents FilesListBox As ListBox
    Private WithEvents label2 As Label
    Friend WithEvents FolderName As Label
End Class
