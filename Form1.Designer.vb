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
        Dim ChartArea5 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend5 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series9 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series10 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea6 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend6 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series11 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series12 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbACCID = New System.Windows.Forms.TextBox()
        Me.lLenderName = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Chart2 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbTWAR = New System.Windows.Forms.TextBox()
        Me.tbWAM = New System.Windows.Forms.TextBox()
        Me.tbAmountTot = New System.Windows.Forms.TextBox()
        Me.tbYieldAvg = New System.Windows.Forms.TextBox()
        Me.tbMonthsAvg = New System.Windows.Forms.TextBox()
        Me.tbChangeTotl = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Months = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "AccountID"
        '
        'tbACCID
        '
        Me.tbACCID.Location = New System.Drawing.Point(77, 10)
        Me.tbACCID.Name = "tbACCID"
        Me.tbACCID.Size = New System.Drawing.Size(100, 20)
        Me.tbACCID.TabIndex = 1
        '
        'lLenderName
        '
        Me.lLenderName.AutoSize = True
        Me.lLenderName.Location = New System.Drawing.Point(193, 13)
        Me.lLenderName.Name = "lLenderName"
        Me.lLenderName.Size = New System.Drawing.Size(71, 13)
        Me.lLenderName.TabIndex = 2
        Me.lLenderName.Text = "Lender Name"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(16, 71)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(915, 265)
        Me.DataGridView1.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(30, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Loan Book"
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(16, 371)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(435, 312)
        Me.DataGridView2.TabIndex = 5
        '
        'DataGridView3
        '
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(16, 731)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(376, 231)
        Me.DataGridView3.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(30, 355)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Amount Deployed"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(30, 715)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Maturity Profile"
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(848, 7)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(75, 23)
        Me.btnGo.TabIndex = 9
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'Chart1
        '
        ChartArea5.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea5)
        Legend5.Name = "Legend1"
        Legend5.Title = "Deployment History"
        Me.Chart1.Legends.Add(Legend5)
        Me.Chart1.Location = New System.Drawing.Point(523, 383)
        Me.Chart1.Name = "Chart1"
        Series9.ChartArea = "ChartArea1"
        Series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series9.Color = System.Drawing.Color.Blue
        Series9.Legend = "Legend1"
        Series9.Name = "Amount Lent"
        Series10.ChartArea = "ChartArea1"
        Series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series10.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Series10.Legend = "Legend1"
        Series10.Name = "Total Account Value"
        Me.Chart1.Series.Add(Series9)
        Me.Chart1.Series.Add(Series10)
        Me.Chart1.Size = New System.Drawing.Size(735, 319)
        Me.Chart1.TabIndex = 10
        Me.Chart1.Text = "Chart1"
        '
        'Chart2
        '
        ChartArea6.Name = "ChartArea1"
        Me.Chart2.ChartAreas.Add(ChartArea6)
        Legend6.Name = "Legend1"
        Legend6.Title = "Loan Maturity Profile"
        Me.Chart2.Legends.Add(Legend6)
        Me.Chart2.Location = New System.Drawing.Point(523, 705)
        Me.Chart2.Name = "Chart2"
        Series11.ChartArea = "ChartArea1"
        Series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series11.Color = System.Drawing.Color.Blue
        Series11.Legend = "Legend1"
        Series11.Name = "Total Lent"
        Series12.ChartArea = "ChartArea1"
        Series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series12.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Series12.Legend = "Legend1"
        Series12.Name = "End Date by Qtr"
        Me.Chart2.Series.Add(Series11)
        Me.Chart2.Series.Add(Series12)
        Me.Chart2.Size = New System.Drawing.Size(735, 300)
        Me.Chart2.TabIndex = 11
        Me.Chart2.Text = "Chart2"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 965)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(162, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Today's weighted average return"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 982)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(134, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Weighted average maturity"
        '
        'tbTWAR
        '
        Me.tbTWAR.Location = New System.Drawing.Point(196, 962)
        Me.tbTWAR.Name = "tbTWAR"
        Me.tbTWAR.Size = New System.Drawing.Size(78, 20)
        Me.tbTWAR.TabIndex = 14
        Me.tbTWAR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbWAM
        '
        Me.tbWAM.Location = New System.Drawing.Point(196, 982)
        Me.tbWAM.Name = "tbWAM"
        Me.tbWAM.Size = New System.Drawing.Size(78, 20)
        Me.tbWAM.TabIndex = 15
        Me.tbWAM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbAmountTot
        '
        Me.tbAmountTot.Location = New System.Drawing.Point(560, 336)
        Me.tbAmountTot.Name = "tbAmountTot"
        Me.tbAmountTot.Size = New System.Drawing.Size(98, 20)
        Me.tbAmountTot.TabIndex = 16
        Me.tbAmountTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbYieldAvg
        '
        Me.tbYieldAvg.Location = New System.Drawing.Point(655, 336)
        Me.tbYieldAvg.Name = "tbYieldAvg"
        Me.tbYieldAvg.Size = New System.Drawing.Size(60, 20)
        Me.tbYieldAvg.TabIndex = 17
        Me.tbYieldAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbMonthsAvg
        '
        Me.tbMonthsAvg.Location = New System.Drawing.Point(712, 336)
        Me.tbMonthsAvg.Name = "tbMonthsAvg"
        Me.tbMonthsAvg.Size = New System.Drawing.Size(79, 20)
        Me.tbMonthsAvg.TabIndex = 18
        Me.tbMonthsAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbChangeTotl
        '
        Me.tbChangeTotl.Location = New System.Drawing.Point(401, 682)
        Me.tbChangeTotl.Name = "tbChangeTotl"
        Me.tbChangeTotl.Size = New System.Drawing.Size(50, 20)
        Me.tbChangeTotl.TabIndex = 19
        Me.tbChangeTotl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(276, 965)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(15, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "%"
        '
        'Months
        '
        Me.Months.AutoSize = True
        Me.Months.Location = New System.Drawing.Point(279, 986)
        Me.Months.Name = "Months"
        Me.Months.Size = New System.Drawing.Size(42, 13)
        Me.Months.TabIndex = 21
        Me.Months.Text = "Months"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 1009)
        Me.Controls.Add(Me.Months)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbChangeTotl)
        Me.Controls.Add(Me.tbMonthsAvg)
        Me.Controls.Add(Me.tbYieldAvg)
        Me.Controls.Add(Me.tbAmountTot)
        Me.Controls.Add(Me.tbWAM)
        Me.Controls.Add(Me.tbTWAR)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Chart2)
        Me.Controls.Add(Me.Chart1)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.lLenderName)
        Me.Controls.Add(Me.tbACCID)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Lender Profile"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents tbACCID As TextBox
    Friend WithEvents lLenderName As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label3 As Label
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents DataGridView3 As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnGo As Button
    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents Chart2 As DataVisualization.Charting.Chart
    Friend WithEvents Label2 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents tbTWAR As TextBox
    Friend WithEvents tbWAM As TextBox
    Friend WithEvents tbAmountTot As TextBox
    Friend WithEvents tbYieldAvg As TextBox
    Friend WithEvents tbMonthsAvg As TextBox
    Friend WithEvents tbChangeTotl As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Months As Label
End Class
