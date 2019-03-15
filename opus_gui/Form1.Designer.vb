<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.InputTxt = New System.Windows.Forms.TextBox()
        Me.OutputTxt = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.InputBrowseBtn = New System.Windows.Forms.Button()
        Me.OutputBrowseBtn = New System.Windows.Forms.Button()
        Me.StartBtn = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.OpusVersionLabel = New System.Windows.Forms.Label()
        Me.BitrateNumberBox = New System.Windows.Forms.NumericUpDown()
        Me.enableMultithreading = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.EncFfmpeg = New System.Windows.Forms.RadioButton()
        Me.EncOpusenc = New System.Windows.Forms.RadioButton()
        Me.InputFileBtn = New System.Windows.Forms.Button()
        Me.ffmpegVersionLabel = New System.Windows.Forms.Label()
        CType(Me.BitrateNumberBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(372, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Step 1: Browse for and input file or folder with opusenc-compatible music files:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(274, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Step 2: Browse for output folder for encoded Opus file(s):"
        '
        'InputTxt
        '
        Me.InputTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputTxt.Location = New System.Drawing.Point(12, 81)
        Me.InputTxt.Name = "InputTxt"
        Me.InputTxt.Size = New System.Drawing.Size(374, 20)
        Me.InputTxt.TabIndex = 2
        '
        'OutputTxt
        '
        Me.OutputTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputTxt.Location = New System.Drawing.Point(12, 123)
        Me.OutputTxt.Name = "OutputTxt"
        Me.OutputTxt.Size = New System.Drawing.Size(464, 20)
        Me.OutputTxt.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Bitrate:"
        '
        'InputBrowseBtn
        '
        Me.InputBrowseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputBrowseBtn.Location = New System.Drawing.Point(473, 78)
        Me.InputBrowseBtn.Name = "InputBrowseBtn"
        Me.InputBrowseBtn.Size = New System.Drawing.Size(84, 23)
        Me.InputBrowseBtn.TabIndex = 5
        Me.InputBrowseBtn.Text = "Browse Folder"
        Me.InputBrowseBtn.UseVisualStyleBackColor = True
        '
        'OutputBrowseBtn
        '
        Me.OutputBrowseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputBrowseBtn.Location = New System.Drawing.Point(482, 123)
        Me.OutputBrowseBtn.Name = "OutputBrowseBtn"
        Me.OutputBrowseBtn.Size = New System.Drawing.Size(75, 23)
        Me.OutputBrowseBtn.TabIndex = 6
        Me.OutputBrowseBtn.Text = "Browse"
        Me.OutputBrowseBtn.UseVisualStyleBackColor = True
        '
        'StartBtn
        '
        Me.StartBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StartBtn.Location = New System.Drawing.Point(356, 152)
        Me.StartBtn.Name = "StartBtn"
        Me.StartBtn.Size = New System.Drawing.Size(204, 37)
        Me.StartBtn.TabIndex = 8
        Me.StartBtn.Text = "Start"
        Me.StartBtn.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 199)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Progress:"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(15, 216)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(545, 23)
        Me.ProgressBar1.Step = 1
        Me.ProgressBar1.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 300)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "GUI by Moises Cardona"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(529, 300)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "v1.9"
        '
        'OpusVersionLabel
        '
        Me.OpusVersionLabel.AutoSize = True
        Me.OpusVersionLabel.Location = New System.Drawing.Point(12, 250)
        Me.OpusVersionLabel.Name = "OpusVersionLabel"
        Me.OpusVersionLabel.Size = New System.Drawing.Size(88, 13)
        Me.OpusVersionLabel.TabIndex = 14
        Me.OpusVersionLabel.Text = "opusenc version:"
        '
        'BitrateNumberBox
        '
        Me.BitrateNumberBox.Location = New System.Drawing.Point(15, 169)
        Me.BitrateNumberBox.Maximum = New Decimal(New Integer() {320, 0, 0, 0})
        Me.BitrateNumberBox.Name = "BitrateNumberBox"
        Me.BitrateNumberBox.Size = New System.Drawing.Size(61, 20)
        Me.BitrateNumberBox.TabIndex = 15
        '
        'enableMultithreading
        '
        Me.enableMultithreading.AutoSize = True
        Me.enableMultithreading.Location = New System.Drawing.Point(82, 172)
        Me.enableMultithreading.Name = "enableMultithreading"
        Me.enableMultithreading.Size = New System.Drawing.Size(121, 17)
        Me.enableMultithreading.TabIndex = 16
        Me.enableMultithreading.Text = "Use Multi-Threading"
        Me.enableMultithreading.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.EncFfmpeg)
        Me.GroupBox1.Controls.Add(Me.EncOpusenc)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(151, 48)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Encoder: "
        '
        'EncFfmpeg
        '
        Me.EncFfmpeg.AutoSize = True
        Me.EncFfmpeg.Location = New System.Drawing.Point(81, 19)
        Me.EncFfmpeg.Name = "EncFfmpeg"
        Me.EncFfmpeg.Size = New System.Drawing.Size(57, 17)
        Me.EncFfmpeg.TabIndex = 1
        Me.EncFfmpeg.TabStop = True
        Me.EncFfmpeg.Text = "ffmpeg"
        Me.EncFfmpeg.UseVisualStyleBackColor = True
        '
        'EncOpusenc
        '
        Me.EncOpusenc.AutoSize = True
        Me.EncOpusenc.Location = New System.Drawing.Point(6, 19)
        Me.EncOpusenc.Name = "EncOpusenc"
        Me.EncOpusenc.Size = New System.Drawing.Size(66, 17)
        Me.EncOpusenc.TabIndex = 0
        Me.EncOpusenc.TabStop = True
        Me.EncOpusenc.Text = "opusenc"
        Me.EncOpusenc.UseVisualStyleBackColor = True
        '
        'InputFileBtn
        '
        Me.InputFileBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputFileBtn.Location = New System.Drawing.Point(392, 79)
        Me.InputFileBtn.Name = "InputFileBtn"
        Me.InputFileBtn.Size = New System.Drawing.Size(75, 23)
        Me.InputFileBtn.TabIndex = 18
        Me.InputFileBtn.Text = "Browse File"
        Me.InputFileBtn.UseVisualStyleBackColor = True
        '
        'ffmpegVersionLabel
        '
        Me.ffmpegVersionLabel.AutoSize = True
        Me.ffmpegVersionLabel.Location = New System.Drawing.Point(12, 265)
        Me.ffmpegVersionLabel.Name = "ffmpegVersionLabel"
        Me.ffmpegVersionLabel.Size = New System.Drawing.Size(79, 13)
        Me.ffmpegVersionLabel.TabIndex = 19
        Me.ffmpegVersionLabel.Text = "ffmpeg version:"
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(572, 322)
        Me.Controls.Add(Me.ffmpegVersionLabel)
        Me.Controls.Add(Me.InputFileBtn)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.enableMultithreading)
        Me.Controls.Add(Me.BitrateNumberBox)
        Me.Controls.Add(Me.OpusVersionLabel)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.StartBtn)
        Me.Controls.Add(Me.OutputBrowseBtn)
        Me.Controls.Add(Me.InputBrowseBtn)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.OutputTxt)
        Me.Controls.Add(Me.InputTxt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Opus GUI"
        CType(Me.BitrateNumberBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents InputTxt As TextBox
    Friend WithEvents OutputTxt As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents InputBrowseBtn As Button
    Friend WithEvents OutputBrowseBtn As Button
    Friend WithEvents StartBtn As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents OpusVersionLabel As Label
    Friend WithEvents BitrateNumberBox As NumericUpDown
    Friend WithEvents enableMultithreading As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents EncFfmpeg As RadioButton
    Friend WithEvents EncOpusenc As RadioButton
    Friend WithEvents InputFileBtn As Button
    Friend WithEvents ffmpegVersionLabel As Label
End Class
