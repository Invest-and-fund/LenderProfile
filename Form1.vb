Imports System.Configuration
Imports System.Collections.Specialized
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms.DataVisualization.Charting

Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraCharts


Public Class Form1

    Public Class Extract
        Property LoanID As Integer
        Property LoanName As String
        Property MaturityDate As Date
        Property TotalExposure As Integer
        Property LoanAmount As Integer
        Property Yield As Decimal
        Property MonthsToGo As Decimal
        Property AcquiredDate As Date
        Property Index As String
    End Class

    Public Class Deployed
        Property DeployedDate As Date
        Property AccountBalance As Integer
        Property DeployedAmount As Integer
        Property Change As Decimal

    End Class

    Public Class Deployed2
        Property Amount As Integer
        Property Balance As Integer
        Property Month As String

    End Class

    Public Class LoanMaturity
        Property Amount As Integer
        Property Balance As Integer
        Property Quarter As String

    End Class

    Public dsLoans As DataSet
    Public dsLenders As DataSet
    Public dsHoldings As DataSet
    Public dsBalance As DataSet
    Public dsDeployed As DataSet
    Public dsMaturity As DataSet
    Public dsBals As DataSet
    Public dsLoansets As DataSet
    Private picChart As Object


    Private WithEvents timer1 As New Windows.Forms.Timer
    Private SpeedData As New List(Of Point)
    Private DirectionData As New List(Of Point)
    Private Rand As New Random(Now.Millisecond)
    Private Count As Single





    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim MySQL, strConn, sHTML, bHTML, sUsers As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow
        Dim iaccid As Integer
        Dim accfound As Integer = 0

        If IsNumeric(tbACCID.Text) Then
            iaccid = tbACCID.Text
        End If

        If iaccid > 0 Then
            'get the account details
            strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
            MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
            MyConn.Open()
            MySQL = "select u.userid, u.firstname, u.lastname, a.companyname, a.accounttype, t.description
                     from users u, accounts a, account_types t
                     where u.userid = a.userid
                       and a.accounttype = t.account_type_id
                       and a.accountid = " & iaccid
            dsLenders = New DataSet
            Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
            Adaptor.Fill(dsLenders)
            MyConn.Close()
            Dim iLenderCounter As Integer = dsLenders.Tables(0).Rows.Count
            For i = 0 To iLenderCounter - 1
                dr1 = dsLenders.Tables(0).Rows(i)
                If Not IsDBNull(dr1("companyname")) Then
                Else
                    dr1("companyname") = ""
                End If
                Dim xBusinessName As String = Trim(dr1("firstname")) & " " & Trim(dr1("lastname")) & " - " & Trim(dr1("companyname")) & " - " & Trim(dr1("description"))

                lLenderName.Text = xBusinessName

                accfound = 1

            Next
        End If

        If accfound = 1 Then
            LoanBook(iaccid)
            'Me.Refresh()
            AmountDeployed(iaccid)
            'Me.Refresh()
            Maturity(iaccid)
            'Me.Refresh()
        End If


    End Sub



    Public Sub LoanBook(iaccid As Integer)
        Dim MySQL, strConn, sHTML, bHTML, sUsers As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow
        Dim dToday = New DateTime(Now.Year - 1, Now.Month, Now.Day, 0, 0, 0)

        Dim values_list As New List(Of Integer)

        Dim colours() As Color = {Color.Blue, Color.Green,
            Color.Cyan, Color.Red, Color.Magenta, Color.Yellow,
            Color.White, Color.Gray, Color.LightBlue,
            Color.LightCyan, Color.LightGreen, Color.Pink,
            Color.Maroon, Color.LightYellow, Color.SkyBlue,
            Color.AliceBlue, Color.Black, Color.Beige,
            Color.BlueViolet, Color.Azure, Color.CadetBlue,
            Color.Chocolate, Color.CornflowerBlue,
            Color.Cornsilk, Color.Crimson, Color.Coral, Color.DarkBlue,
            Color.DarkCyan, Color.DarkGreen, Color.DarkOrange,
            Color.DarkRed, Color.DarkSalmon, Color.DeepPink,
            Color.DodgerBlue, Color.FloralWhite, Color.ForestGreen,
            Color.Fuchsia, Color.Gold, Color.Honeydew,
            Color.IndianRed, Color.Khaki, Color.LavenderBlush,
            Color.LemonChiffon, Color.LightPink, Color.Lime,
            Color.Magenta, Color.MediumAquamarine, Color.Maroon,
            Color.MediumOrchid, Color.MintCream, Color.MediumTurquoise,
            Color.MistyRose, Color.Moccasin, Color.Olive,
            Color.PaleGoldenrod, Color.PaleVioletRed, Color.Peru,
            Color.SeaShell, Color.Silver, Color.SpringGreen}



        Dim v As Integer = 0
        Dim w As Integer = 0
        Dim x As Integer = 0
        Dim iloansetid As Integer = 0
        Dim nloansetid As Integer = 0
        Dim iwrite As Integer = 0
        Dim iloanamount As Integer
        Dim dacquireddate As Date = DateTime.Now.AddYears(10)
        Dim dmaturesdate As Date = DateTime.Now.AddYears(10)
        Dim imonthstogototal As Decimal = 0
        Dim iyieldtotal As Decimal = 0
        Dim irecordscount As Integer = 0


        Dim rtotal As Integer = 0
        Dim rtotal2 As Integer
        Dim rProduct As Decimal = 0
        Dim rMProduct As Decimal = 0



        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select loanid,  businessname ,amount as loanamount, dd_lastdate as maturitydate, fixed_rate as yield,
                 dd_date as dateenteredinto, loansetid, loan_holdings_id
            from
            (
            select distinct l.loanid, l.business_name as businessname , b.num_units as amount, l.dd_lastdate, l.fixed_rate, l.dd_date, l.loansetid, h.loan_holdings_id
            from loans l, accounts a, users u , orders o,  loan_holdings h, lh_balances b
           where u.userid = a.userid
            and u.usertype = 0
            and a.accountid = o.accountid
            and o.lh_id = h.loan_holdings_id
            and l.loanid = h.loanid
            and b.lh_id = h.loan_holdings_id
            and b.accountid = a.accountid
            and l.loanstatus in (2, 7)
            and a.accountid = " & iaccid &
            "  group by l.loanid, l.business_name, l.dd_lastdate, l.fixed_rate, l.dd_date, l.loansetid , amount, h.loan_holdings_id
              union all
            select distinct l.loanid, l.business_name as businessname , b.num_units as amount, l.dd_lastdate, l.fixed_rate, l.dd_date, l.loansetid, h.loan_holdings_id
            from loans l, accounts a, users u , orders o,  loan_holdings h, lh_balances_suspense b
            where u.userid = a.userid
            and u.usertype = 0
            and a.accountid = o.accountid
            and o.lh_id = h.loan_holdings_id
            and l.loanid = h.loanid
            and b.lh_id = h.loan_holdings_id
            and b.accountid = a.accountid
            and l.loanstatus in (2, 7)
            and a.accountid = " & iaccid &
            "   group by l.loanid, l.business_name, l.dd_lastdate, l.fixed_rate, l.dd_date, l.loansetid   , amount, h.loan_holdings_id
            ) results
             order by
            loansetid, dateenteredinto, maturitydate, loanid,  loan_holdings_id,
            businessname,
            yield"


        dsLoans = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.Fill(dsLoans)
        MyConn.Close()
        Dim ExtractList As New List(Of Extract)
        Dim iLoanCounter As Integer = dsLoans.Tables(0).Rows.Count
        For i = 0 To iLoanCounter - 1
            Dim newExtract As New Extract
            dr2 = dsLoans.Tables(0).Rows(i)
            newExtract.LoanID = dr2("loanid")
            newExtract.LoanName = dr2("businessname")
            'newExtract.LoanAmount = dr2("loanamount") / 100
            'newExtract.MaturityDate = dr2("maturitydate")
            newExtract.Yield = Math.Round(dr2("yield") / 100, 2)
            'newExtract.AcquiredDate = dr2("dateenteredinto")
            newExtract.TotalExposure = 0
            newExtract.MonthsToGo = 0

            If Not IsDBNull(dr2("LoanSetID")) Then
                iLoansetid = dr2("LoanSetID")
            Else
                iLoansetid = 0
            End If

            MyConn.Open()
            MySQL = "select first 1 b.datecreated
            from lh_bals b, loans l, loan_holdings h
            where l.loanid = " & newExtract.LoanID &
           "  and b.accountid = " & iaccid &
           "  and b.lh_id = h.loan_holdings_id
              and h.loanid = l.loanid"
            dsBals = New DataSet
            Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
            Adaptor.Fill(dsBals)
            MyConn.Close()
            newExtract.AcquiredDate = dsBals.Tables(0).Rows(0)("datecreated")




            iwrite = 0 ' set to write record unless find otherwise 
            If i < iLoanCounter - 1 Then
                dr3 = dsLoans.Tables(0).Rows(i + 1)
                If Not IsDBNull(dr3("LoanSetID")) Then
                    nloansetid = dr3("LoanSetID")
                Else
                    nloansetid = 0
                End If
                If iloansetid = nLoansetid And nLoansetid <> 0 Then
                    'dont write this record -  just accumulate values
                    iwrite = 1

                End If
            End If


            iloanamount += dr2("loanamount")
            If dacquireddate > dr2("dateenteredinto") Then
                dacquireddate = dr2("dateenteredinto")
            End If
            If dmaturesdate > dr2("maturitydate") Then
                dmaturesdate = dr2("maturitydate")
            End If




            Dim TTF As New Decimal




            Dim dloanamount As Integer = iloanamount / 100

            If dloanamount > 0 And iwrite = 0 Then

                If iloansetid > 0 Then
                    MyConn.Open()
                    MySQL = "select business_name from loan_sets where loansetid = " & nloansetid
                    dsLoansets = New DataSet
                    Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
                    Adaptor.Fill(dsLoansets)
                    MyConn.Close()
                    newExtract.LoanName = dsLoansets.Tables(0).Rows(0)("business_name")
                End If

                newExtract.MaturityDate = dmaturesdate

                dmaturesdate = DateTime.Now.AddYears(10) 'set date into furure to enable picking up correct dates
                newExtract.AcquiredDate = dacquireddate
                dacquireddate = DateTime.Now.AddYears(10) 'set date into furure to enable picking up correct dates
                TTF = DateDiff(DateInterval.Day, Date.Now, newExtract.MaturityDate) / 30
                newExtract.MonthsToGo = Math.Round(TTF, 2)

                'If newExtract.MonthsToGo < 0 Then
                'Else
                newExtract.LoanAmount = iloanamount / 100
                rtotal += dloanamount
                values_list.Add(dr2("loanamount") / 100)


                rProduct += (iloanamount * (newExtract.Yield / 10000))
                rMProduct += (iloanamount * (newExtract.MonthsToGo / 10000))

                iloanamount = 0
                imonthstogototal += newExtract.MonthsToGo
                iyieldtotal += newExtract.Yield
                irecordscount += 1




                ExtractList.Add(newExtract)


                v += 1
                x += 1
                'End If

            End If

        Next

        If rtotal = 0 Then
            Exit Sub
        End If

        imonthstogototal = Math.Round(imonthstogototal / irecordscount, 2)
        tbMonthsAvg.Text = imonthstogototal
        iyieldtotal = Math.Round(iyieldtotal / irecordscount, 2)
        tbYieldAvg.Text = iyieldtotal


        tbAmountTot.Text = rtotal

        rProduct = rProduct / rtotal
        tbTWAR.Text = Math.Round((rProduct * 100), 2)
        rMProduct = rMProduct / rtotal
        tbWAM.Text = Math.Round((rMProduct * 100), 2)

        'now we know how big to make the arrays we can make them here
        Dim values(values_list.Count) As Integer
        Dim somecolours(values_list.Count) As Color
        Dim percs(values_list.Count) As Integer

        For v = 0 To values_list.Count - 1
            values(v) = values_list(v)
            somecolours(v) = colours(w)
            w += 1
            If w > 59 Then
                w = 0
            End If
        Next


        For v = 0 To values_list.Count - 1
            percs(v) = (100 / rtotal) * values(v)
            rtotal2 += percs(v)

        Next v

        'due to the vaguaries of percentages this could add up to 99 or 101 so sets the first one up or dwn by 1
        If rtotal2 <> 100 Then
            percs(0) += 100 - rtotal2
        End If

        Using PieGraphic = Me.CreateGraphics()
            'Set location of pie chart...
            Dim PieLocation As New Point(1000, 60)
            'Set size of pie chart...
            Dim PieSize As New Size(250, 250)
            'Call function which create a pie chart of given data...
            DrawPieChart(percs, somecolours, PieGraphic, PieLocation, PieSize)
        End Using



        DataGridView1.DataSource = ExtractList
        For v = 0 To x - 1


            DataGridView1.Rows(v).Cells(8).Style.BackColor = somecolours(v)

            ' DataGridView1.Rows(v).DefaultCellStyle.ForeColor = somecolours(v)


        Next
        DataGridView1.Columns.Item(0).Width = 55
        DataGridView1.Columns.Item(1).Width = 250
        DataGridView1.Columns.Item(2).Width = 100
        DataGridView1.Columns.Item(3).Width = 115
        DataGridView1.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns.Item(4).Width = 115
        DataGridView1.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns.Item(5).Width = 50
        DataGridView1.Columns.Item(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns.Item(6).Width = 80
        DataGridView1.Columns.Item(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns.Item(7).Width = 100
        DataGridView1.Columns.Item(8).Width = 15



    End Sub





    ' Draw a pie chart with the indicated values.
    Public Sub DrawPieChart(ByVal PiePercents() As Integer, ByVal PieColors() As Color,
                              ByVal PieGraphic As Graphics, ByVal PieLocation As Point,
                              ByVal PieSize As Size)

        'Check values total is 100 or not...
        Dim sum = 0
        For Each percent In PiePercents
            sum += percent
        Next

        ' If sum not 100 then throw Exception...
        If sum <> 100 Then
            Throw New ArgumentException("Percentages do not add up to 100.")
        End If

        ' Here it will check that total values & colors value are same or not...
        ' If not same then throw Exception...
        If PiePercents.Length <> PieColors.Length Then
            Throw New ArgumentException("There must be the same number of percents and colors.")
        End If



        Dim PiePercentTotal = 0
        For PiePercent = 0 To PiePercents.Length() - 1
            Using brush As New SolidBrush(PieColors(PiePercent))
                PieGraphic.FillPie(brush, New Rectangle(PieLocation, PieSize), CSng(PiePercentTotal * 360 / 100), CSng(PiePercents(PiePercent) * 360 / 100))
            End Using

            PiePercentTotal += PiePercents(PiePercent)
        Next
        Return
    End Sub

    Public Sub AmountDeployed(iaccid As Integer)



        Dim values_list As New List(Of Integer)






        Dim DeployedList As New List(Of Deployed)
        Dim DeployedList2 As New List(Of Deployed2)
        Dim DeployedDate = New DateTime(Now.Year - 1, Now.Month, 1, 0, 0, 0)
        Dim PrevDeployed As Integer = 0
        Dim FirstDeployed As Integer = 0
        Dim LastDeployed As Integer = 0
        Dim change As Decimal = 0

        For v = 0 To 12

            getDeployedData(iaccid, v, DeployedList, PrevDeployed, DeployedList2, FirstDeployed, LastDeployed)




        Next
        If FirstDeployed = 0 Then
            change = 0
        Else
            change = (LastDeployed - FirstDeployed) / FirstDeployed
            change = Math.Round(change, 2)
        End If
        tbChangeTotl.Text = change
        Dim d = DeployedList2.Count - 1

        DoLineChart(d, DeployedList2)

        DataGridView2.DataSource = DeployedList

        DataGridView2.Columns.Item(0).Width = 100
        DataGridView2.Columns.Item(1).Width = 115
        DataGridView2.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView2.Columns.Item(2).Width = 115
        DataGridView2.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView2.Columns.Item(3).Width = 60
        DataGridView2.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight




    End Sub

    Public Sub DoLineChart(ByVal d As Integer, DeployedList2 As List(Of Deployed2))

        Chart1.Series.Clear()
        Chart1.Titles.Clear()
        'Chart1.Titles.Add("Demo")
        'Create a new series and add data points to it.

        Dim s, s2 As New Series

        s.Name = "Amount Lent"
        s2.Name = "Total Account Value"

        'Change to a line graph.

        s.ChartType = SeriesChartType.Line
        s2.ChartType = SeriesChartType.Line

        Dim iHighest As Integer = 0

        For d = 0 To d - 1
            s.Points.AddXY(DeployedList2(d).Month, DeployedList2(d).Amount)
            s2.Points.AddXY(DeployedList2(d).Month, DeployedList2(d).Balance)
            If iHighest < DeployedList2(d).Balance Then
                iHighest = DeployedList2(d).Balance
            End If
            If iHighest < DeployedList2(d).Amount Then
                iHighest = DeployedList2(d).Amount
            End If
        Next

        If iHighest = 0 Then
            iHighest = 1000
        End If



        'Add the series to the Chart1 control.

        Chart1.Series.Add(s)
        Chart1.Series.Add(s2)
        Chart1.ResetAutoValues()
        Chart1.ChartAreas(0).AxisY.MaximumAutoSize = 100
        Chart1.ChartAreas(0).AxisX.MaximumAutoSize = 100
        'Chart1.ChartAreas(0).AxisY.Maximum = iHighest



    End Sub




    Public Sub getDeployedData(iaccid As Integer, v As Integer, ByRef DeployedList As List(Of Deployed), ByRef PrevDeployed As Integer, ByRef DeployedList2 As List(Of Deployed2),
                               ByRef FirstDeployed As Integer, ByRef LastDeployed As Integer)

        Dim MySQL, strConn, sHTML, bHTML, sUsers As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow

        'Dim DeployedList As New List(Of Deployed)
        Dim DeployedDate = New Date(Now.Year - 1, Now.Month, 1, 0, 0, 0)


        DeployedDate = DeployedDate.AddMonths(v)


        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select sum(f.balance) as theamount
                from (
                 Select            fb.num_units  as balance
                   From select_active_accounts a  
                  Left outer  join
                  ( select vt.accountid, lh_id, max_lh_bals_id, t.num_units
                 from 
                  ( 
                     select s.accountid, max(s.lh_bals_id) as max_lh_bals_id
                     from lh_bals s, loan_holdings h, loans l
                      where lh_id > 0
                      and s.lh_id = h.loan_holdings_id
                      and h.loanid = l.loanid
                      and l.loanstatus in (2, 7)
                       and s.datecreated < @DeployedDate 
                   group by s.accountid, s.lh_id
                    ) vt
                inner join lh_bals t on t.lh_bals_id = vt.max_lh_bals_id
                 where t.num_units > 0 ) 

                fb on fb.accountid = a.accountid
                 where fb.accountid = a.accountid
                   and a.accountid = @accid 
              union all
                 Select            fb.num_units  as balance
                   From select_active_accounts a  
                  Left outer  join 
            (select vt.accountid, lh_id, max_lh_bals_sus_id, t.num_units
                 from 
                  ( 
                     select s.accountid, max(s.lh_bals_suspense_id) as max_lh_bals_sus_id
                     from lh_bals_suspense s, loan_holdings h, loans l
                      where lh_id > 0
                      and s.lh_id = h.loan_holdings_id
                      and h.loanid = l.loanid
                      and l.loanstatus in (2, 7)
                       and s.datecreated < @DeployedDate 
                   group by s.accountid, s.lh_id
                    ) vt
                inner join lh_bals_suspense t on t.lh_bals_suspense_id = vt.max_lh_bals_sus_id
                 where t.num_units > 0 )   fb on fb.accountid = a.accountid
                 where fb.accountid = a.accountid
                   and a.accountid = @accid
                   ) f"


        dsDeployed = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.SelectCommand.Parameters.Add("@DeployedDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = DeployedDate
        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsDeployed)
        MyConn.Close()

        Dim newDeployed As New Deployed
        Dim newDeployed2 As New Deployed2
        dr2 = dsDeployed.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            newDeployed.DeployedAmount = dr2("TheAmount") / 100
        Else
            newDeployed.DeployedAmount = 0
        End If

        newDeployed.DeployedDate = DeployedDate

        If Not IsDBNull(dr2("TheAmount")) Then
            newDeployed2.Amount = dr2("TheAmount") / 100
        Else
            newDeployed2.Amount = 0
        End If

        newDeployed2.Month = DeployedDate.Month & "/" & DeployedDate.Year

        Dim change As Decimal
        If PrevDeployed = 0 Then


            change = 0

        Else
            change = (newDeployed.DeployedAmount - PrevDeployed) / PrevDeployed

        End If
        newDeployed.Change = Math.Round(change, 2)
        PrevDeployed = newDeployed.DeployedAmount
        If FirstDeployed = 0 Then
            FirstDeployed = newDeployed.DeployedAmount
        End If
        LastDeployed = newDeployed.DeployedAmount



        'now get the account balance for that date

        MyConn.Open()
        MySQL = "select sum(f.balance) as theamount
                from (
                 Select            fb.amount  as balance
                   From select_active_accounts a  
                  Left outer  join

                 (select vt.accountid,  max_fin_balid, t.amount
        from 
        ( 
        select accountid, max(fin_balid) as max_fin_balid
        from fin_bals s

        where s.datecreated < @DeployedDate
     group by accountid
        ) vt
        inner join fin_bals t on t.fin_balid = vt.max_fin_balid
        where t.amount > 0 ) fb on fb.accountid = a.accountid
                 where fb.accountid = a.accountid
                   and a.accountid = @accid 
               union all
                 Select            fb.amount as balance
                   From select_active_accounts a  
                  Left outer  join

            (select vt.accountid,  max_fin_bals_suspenseid, t.amount
        from 
        ( 
        select accountid, max(fin_bals_suspenseid) as max_fin_bals_suspenseid
        from fin_bals_suspense s

        where s.datecreated < @DeployedDate 
        group by accountid
        ) vt
        inner join fin_bals_suspense t on t.fin_bals_suspenseid = vt.max_fin_bals_suspenseid
        where t.amount > 0 )   fb on fb.accountid = a.accountid
                 where fb.accountid = a.accountid
                   and a.accountid = @accid  ) f"


        dsBalance = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.SelectCommand.Parameters.Add("@DeployedDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = DeployedDate
        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsBalance)
        MyConn.Close()

        dr2 = dsBalance.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            newDeployed.AccountBalance = dr2("TheAmount") / 100
        Else
            newDeployed.AccountBalance = 0
        End If

        newDeployed.AccountBalance += newDeployed.DeployedAmount
        newDeployed2.Balance = newDeployed.AccountBalance

        DeployedList.Add(newDeployed)
        DeployedList2.Add(newDeployed2)



    End Sub

    Public Sub Maturity(iaccid As Integer)
        'get loan holding position prior to iteration of future maturity
        Dim MySQL, strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow


        'retrieve all loan holdings for the lender - to current date
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select  sum (num_units) as TheAmount  from
            (
            select sum(num_units) as num_units, dd_lastdate, loanid, loan_holdings_id
            from
            (
            select  distinct b.num_units, l.dd_lastdate, l.loanid, h.loan_holdings_id
            from loans l, loan_holdings h, lh_balances b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.loanstatus in (2, 7)


            union

            select  distinct b.num_units, l.dd_lastdate, l.loanid, h.loan_holdings_id
            from loans l, loan_holdings h, lh_balances_suspense b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.loanstatus in (2, 7)
                  ) v
            group by  loanid, loan_holdings_id, dd_lastdate
            order by  dd_lastdate, loanid, loan_holdings_id
            )"


        dsMaturity = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)

        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsMaturity)
        MyConn.Close()


        Dim QuaterStartDate = New Date(Now.Year, Now.Month, 1, 0, 0, 0)
        Select Case QuaterStartDate.Month
            Case 1, 2, 3
                QuaterStartDate = New Date(QuaterStartDate.Year, 1, 1, 0, 0, 0)
            Case 4, 5, 6
                QuaterStartDate = New Date(QuaterStartDate.Year, 4, 1, 0, 0, 0)
            Case 7, 8, 9
                QuaterStartDate = New Date(QuaterStartDate.Year, 7, 1, 0, 0, 0)
            Case 10, 11, 12
                QuaterStartDate = New Date(QuaterStartDate.Year, 10, 1, 0, 0, 0)
        End Select

        Dim QuaterEndDate = QuaterStartDate.AddMonths(3)
        Dim QuateStartDate = QuaterStartDate.AddDays(-1)




        Dim newLoanMaturity As New LoanMaturity


        Dim PrevMatured As Integer = 0
        dr2 = dsMaturity.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            PrevMatured = dr2("TheAmount") / 100
        Else
            PrevMatured = 0
        End If







        Dim values_list As New List(Of Integer)

        Dim LoanMaturityList As New List(Of LoanMaturity)

        Dim MaturityDate As New DateTime
        MaturityDate = DateTime.Now




        For i = 0 To 8
            getMaturityData(iaccid, LoanMaturityList, PrevMatured, QuaterStartDate, QuaterEndDate, i)
        Next





        Dim d = LoanMaturityList.Count - 1

        DoLineChart2(d, LoanMaturityList)

        DataGridView3.DataSource = LoanMaturityList

        DataGridView3.Columns.Item(0).Width = 100
        DataGridView3.Columns.Item(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView3.Columns.Item(1).Width = 115
        DataGridView3.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView3.Columns.Item(2).Width = 115
        DataGridView3.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub

    Public Sub getMaturityData(iaccid As Integer, ByRef LoanMaturityList As List(Of LoanMaturity), ByRef PrevMatured As Integer, ByRef QuaterStartDate As Date, ByRef QuaterEndDate As Date, ByRef i As Integer)

        Dim MySQL, strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow
        Dim TempQuaterStartDate = New Date


        'for the first quarter we only need to get repayments from current date forward - so overwrite the from date
        'If i = 0 Then
        '    TempQuaterStartDate = New Date(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        'Else
        TempQuaterStartDate = QuaterStartDate
        'End If




        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select  sum (num_units) as TheAmount  from
            (
            select sum(num_units) as num_units, dd_lastdate, loanid
            from
            (
            select  distinct b.num_units, l.dd_lastdate, l.loanid, h.loan_holdings_id
            from loans l, loan_holdings h, lh_balances b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.dd_lastdate < @enddate
            and l.dd_lastdate > @startdate 

            union

            select  distinct b.num_units, l.dd_lastdate, l.loanid, h.loan_holdings_id
            from loans l, loan_holdings h, lh_balances_suspense b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.dd_lastdate < @enddate
            and l.dd_lastdate > @startdate       ) v
            group by  loanid, loan_holdings_id, dd_lastdate
            order by  dd_lastdate, loanid, loan_holdings_id
            )"


        dsMaturity = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.SelectCommand.Parameters.Add("@StartDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = TempQuaterStartDate
        Adaptor.SelectCommand.Parameters.Add("EndDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = QuaterEndDate
        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsMaturity)
        MyConn.Close()



        Dim newLoanMaturity As New LoanMaturity



        dr2 = dsMaturity.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            newLoanMaturity.Amount = dr2("TheAmount") / 100
        Else
            newLoanMaturity.Amount = 0
        End If

        newLoanMaturity.Balance = PrevMatured - newLoanMaturity.Amount
        newLoanMaturity.Quarter = QuaterEndDate.Month & "/" & QuaterEndDate.Year

        PrevMatured = newLoanMaturity.Balance

        LoanMaturityList.Add(newLoanMaturity)

        QuaterStartDate = QuaterEndDate.AddDays(-1)
        QuaterEndDate = QuaterStartDate.AddMonths(3)



    End Sub

    Public Sub DoLineChart2(ByVal d As Integer, LoanMaturityList As List(Of LoanMaturity))

        Chart2.Series.Clear()
        Chart2.Titles.Clear()
        'Chart2.Titles.Add("Demo")
        'Create a new series and add data points to it.

        Dim s, s2 As New Series

        s.Name = "Total Lent"
        s2.Name = "End Date by Qtr"

        'Change to a line graph.

        s.ChartType = SeriesChartType.Line
        s2.ChartType = SeriesChartType.Column

        Dim iHighest As Integer = 0

        For d = 0 To d - 1
            s.Points.AddXY(LoanMaturityList(d).Quarter, LoanMaturityList(d).Balance)
            s2.Points.AddXY(LoanMaturityList(d).Quarter, LoanMaturityList(d).Amount)
            If iHighest < LoanMaturityList(d).Balance Then
                iHighest = LoanMaturityList(d).Balance
            End If
            If iHighest < LoanMaturityList(d).Amount Then
                iHighest = LoanMaturityList(d).Amount
            End If
        Next

        If iHighest = 0 Then
            iHighest = 1000
        End If

        'Add the series to the Chart1 control.

        Chart2.Series.Add(s)
        Chart2.Series.Add(s2)
        Chart2.ResetAutoValues()
        Chart2.ChartAreas(0).AxisY.MaximumAutoSize = 100
        Chart2.ChartAreas(0).AxisX.MaximumAutoSize = 100

        'Chart2.ChartAreas(0).AxisY.Maximum = iHighest
        'Chart2.ChartAreas(0).AxisX.Minimum = Double.NaN





    End Sub

End Class
