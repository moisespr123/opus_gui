<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.BitrateTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(327, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Step 1: Browse for Input folder with opusenc-compatible music Files:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(268, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Step 2: Browse for output folder for encoded Opus files:"
        '
        'InputTxt
        '
        Me.InputTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputTxt.Location = New System.Drawing.Point(15, 26)
        Me.InputTxt.Name = "InputTxt"
        Me.InputTxt.Size = New System.Drawing.Size(613, 20)
        Me.InputTxt.TabIndex = 2
        '
        'OutputTxt
        '
        Me.OutputTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputTxt.Location = New System.Drawing.Point(15, 68)
        Me.OutputTxt.Name = "OutputTxt"
        Me.OutputTxt.Size = New System.Drawing.Size(613, 20)
        Me.OutputTxt.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 101)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Bitrate:"
        '
        'InputBrowseBtn
        '
        Me.InputBrowseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputBrowseBtn.Location = New System.Drawing.Point(634, 24)
        Me.InputBrowseBtn.Name = "InputBrowseBtn"
        Me.InputBrowseBtn.Size = New System.Drawing.Size(75, 23)
        Me.InputBrowseBtn.TabIndex = 5
        Me.InputBrowseBtn.Text = "Browse"
        Me.InputBrowseBtn.UseVisualStyleBackColor = True
        '
        'OutputBrowseBtn
        '
        Me.OutputBrowseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputBrowseBtn.Location = New System.Drawing.Point(634, 68)
        Me.OutputBrowseBtn.Name = "OutputBrowseBtn"
        Me.OutputBrowseBtn.Size = New System.Drawing.Size(75, 23)
        Me.OutputBrowseBtn.TabIndex = 6
        Me.OutputBrowseBtn.Text = "Browse"
        Me.OutputBrowseBtn.UseVisualStyleBackColor = True
        '
        'StartBtn
        '
        Me.StartBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StartBtn.Location = New System.Drawing.Point(505, 101)
        Me.StartBtn.Name = "StartBtn"
        Me.StartBtn.Size = New System.Drawing.Size(204, 37)
        Me.StartBtn.TabIndex = 8
        Me.StartBtn.Text = "Start"
        Me.StartBtn.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Progress:"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(15, 165)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(694, 23)
        Me.ProgressBar1.Step = 1
        Me.ProgressBar1.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "GUI by Moises Cardona"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(681, 201)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "v1.0"
        '
        'BitrateTextBox
        '
        Me.BitrateTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BitrateTextBox.Location = New System.Drawing.Point(15, 118)
        Me.BitrateTextBox.Name = "BitrateTextBox"
        Me.BitrateTextBox.Size = New System.Drawing.Size(484, 20)
        Me.BitrateTextBox.TabIndex = 13
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(721, 224)
        Me.Controls.Add(Me.BitrateTextBox)
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
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    Friend WithEvents BitrateTextBox As TextBox
End Class
