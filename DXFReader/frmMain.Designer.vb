<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.pbMain = New System.Windows.Forms.PictureBox()
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbMain
        '
        Me.pbMain.Cursor = System.Windows.Forms.Cursors.Cross
        Me.pbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbMain.Location = New System.Drawing.Point(0, 0)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(815, 561)
        Me.pbMain.TabIndex = 1
        Me.pbMain.TabStop = False
        '
        'frmMain
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(815, 561)
        Me.Controls.Add(Me.pbMain)
        Me.Name = "frmMain"
        Me.Text = "frmMain"
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbMain As System.Windows.Forms.PictureBox
End Class
