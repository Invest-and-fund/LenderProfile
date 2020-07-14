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

                Dim xBusinessName As String = Trim(dr1("firstname")) & " " & Trim(dr1("lastname")) & " - " & Trim(dr1("companyname")) & " - " & Trim(dr1("description"))

                lLenderName.Text = xBusinessName

                accfound = 1

            Next
        End If

        If accfound = 1 Then
            LoanBook(iaccid)
            AmountDeployed(iaccid)
            Maturity(iaccid)
        End If


    End Sub



    Public Sub LoanBook(iaccid As Integer)
        Dim MySQL, strConn, sHTML, bHTML, sUsers As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow

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


        Dim rtotal As Decimal = 0
        Dim rtotal2 As Integer



        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select loanid, business_name ,sum(amount) as loanamount, dd_lastdate as maturitydate, fixed_rate as yield, dd_date as dateenteredinto
            from
            (
            select distinct l.loanid, l.business_name , sum(b.num_units) as amount, l.dd_lastdate, l.fixed_rate, l.dd_date
            from loans l, accounts a, users u , orders o,  loan_holdings h, lh_balances b
            where u.userid = a.userid
            and u.usertype = 0
            and a.accountid = o.accountid
            and o.lh_id = h.loan_holdings_id
            and l.loanid = h.loanid
            and b.lh_id = h.loan_holdings_id
            and b.accountid = a.accountid
 
            and a.accountid = " & iaccid &
            " group by l.loanid, l.business_name, l.dd_lastdate, l.fixed_rate, l.dd_date

              union all

            select distinct l.loanid, l.business_name , sum(b.num_units) as amount, l.dd_lastdate, l.fixed_rate, l.dd_date
            from loans l, accounts a, users u , orders o,  loan_holdings h, lh_balances_suspense b
            where u.userid = a.userid
            and u.usertype = 0
            and a.accountid = o.accountid
            and o.lh_id = h.loan_holdings_id
            and l.loanid = h.loanid
            and b.lh_id = h.loan_holdings_id
            and b.accountid = a.accountid
  
            and a.accountid = " & iaccid &
            "  group by l.loanid, l.business_name, l.dd_lastdate, l.fixed_rate, l.dd_date
            ) results
            group by loanid, business_name, maturitydate, yield, dateenteredinto
            order by  business_name, loanid, maturitydate, yield, dateenteredinto"


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
            newExtract.LoanName = dr2("business_name")
            newExtract.LoanAmount = dr2("loanamount") / 100
            newExtract.MaturityDate = dr2("maturitydate")
            newExtract.Yield = dr2("yield")
            newExtract.AcquiredDate = dr2("dateenteredinto")
            newExtract.TotalExposure = 0
            newExtract.MonthsToGo = 0



            If newExtract.LoanAmount > 0 Then
                ExtractList.Add(newExtract)

                values_list.Add(dr2("loanamount") / 100)
                rtotal += dr2("loanamount") / 100
                v += 1
                x += 1
            End If

        Next

        If rtotal = 0 Then
            Exit Sub
        End If

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

        For v = 0 To 12

            getDeployedData(iaccid, v, DeployedList, PrevDeployed, DeployedList2)




        Next

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

        s.Name = "Deployed"
        s2.Name = "Balance"

        'Change to a line graph.

        s.ChartType = SeriesChartType.Line
        s2.ChartType = SeriesChartType.Line



        For d = 0 To d - 1
            s.Points.AddXY(DeployedList2(d).Month, DeployedList2(d).Amount)
            s2.Points.AddXY(DeployedList2(d).Month, DeployedList2(d).Balance)
        Next



        'Add the series to the Chart1 control.

        Chart1.Series.Add(s)
        Chart1.Series.Add(s2)




    End Sub




    Public Sub getDeployedData(iaccid As Integer, v As Integer, ByRef DeployedList As List(Of Deployed), ByRef PrevDeployed As Integer, ByRef DeployedList2 As List(Of Deployed2))

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
                     select accountid, max(lh_bals_id) as max_lh_bals_id
                     from lh_bals s
                      where lh_id > 0
                       and s.datecreated < @DeployedDate 
                   group by accountid, lh_id
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
                     select accountid, max(lh_bals_suspense_id) as max_lh_bals_sus_id
                     from lh_bals_suspense s
                      where lh_id > 0
                       and s.datecreated < @DeployedDate 
                   group by accountid, lh_id
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
        newDeployed.DeployedAmount = dr2("TheAmount")
        newDeployed.DeployedDate = DeployedDate

        newDeployed2.Amount = dr2("TheAmount")
        newDeployed2.Month = DeployedDate.Month & "/" & DeployedDate.Year

        If PrevDeployed = 0 Then


            newDeployed.Change = 0

        Else
            Dim change As Decimal = (newDeployed.DeployedAmount - PrevDeployed) / PrevDeployed
            newDeployed.Change = change
        End If
        PrevDeployed = newDeployed.DeployedAmount



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
            newDeployed.AccountBalance = dr2("TheAmount")
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

        'Dim DeployedList As New List(Of Deployed)
        Dim DeployedDate = New Date(Now.Year - 1, Now.Month, 1, 0, 0, 0)





        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select  sum (num_units) as TheAmount  from
            (
            select sum(num_units) as num_units, dd_lastdate, loanid
            from
            (
            select  distinct b.num_units, l.dd_lastdate, l.loanid
            from loans l, loan_holdings h, lh_balances b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            

            union

            select  distinct b.num_units, l.dd_lastdate, l.loanid
            from loans l, loan_holdings h, lh_balances_suspense b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
                  ) v
            group by  loanid, dd_lastdate
            order by  dd_lastdate, loanid
            )"


        dsMaturity = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)

        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsMaturity)
        MyConn.Close()



        Dim newLoanMaturity As New LoanMaturity


        Dim PrevMatured As Integer = 0
        dr2 = dsMaturity.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            PrevMatured = dr2("TheAmount")
        Else
            PrevMatured = 0
        End If







        Dim values_list As New List(Of Integer)

        Dim LoanMaturityList As New List(Of LoanMaturity)

        Dim MaturityDate As New DateTime
        MaturityDate = DateTime.Now


        Dim QuaterStartDate = New Date(Now.Year - 1, Now.Month, 1, 0, 0, 0)
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

        QuaterStartDate = QuaterStartDate.AddDays(-1)
        Dim QuaterEndDate = QuaterStartDate.AddMonths(3)

        For i = 0 To 8
            getMaturityData(iaccid, LoanMaturityList, PrevMatured, QuaterStartDate, QuaterEndDate)
        Next





        Dim d = LoanMaturityList.Count - 1

        DoLineChart2(d, LoanMaturityList)

        DataGridView3.DataSource = LoanMaturityList

        DataGridView3.Columns.Item(0).Width = 100
        DataGridView3.Columns.Item(1).Width = 115
        DataGridView3.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView3.Columns.Item(2).Width = 115
        DataGridView3.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub

    Public Sub getMaturityData(iaccid As Integer, ByRef LoanMaturityList As List(Of LoanMaturity), ByRef PrevMatured As Integer, ByRef QuaterStartDate As Date, ByRef QuaterEndDate As Date)

        Dim MySQL, strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim connection As String = "FBConnectionString"
        Dim dr1, dr2, dr3 As DataRow

        'Dim DeployedList As New List(Of Deployed)
        Dim DeployedDate = New Date(Now.Year - 1, Now.Month, 1, 0, 0, 0)





        'retrieve all loan holdings for the lender
        strConn = ConfigurationManager.ConnectionStrings(connection).ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        MySQL = "select  sum (num_units) as TheAmount  from
            (
            select sum(num_units) as num_units, dd_lastdate, loanid
            from
            (
            select  distinct b.num_units, l.dd_lastdate, l.loanid
            from loans l, loan_holdings h, lh_balances b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.dd_lastdate < @enddate
            and l.dd_lastdate > @startdate 

            union

            select  distinct b.num_units, l.dd_lastdate, l.loanid
            from loans l, loan_holdings h, lh_balances_suspense b
            where l.loanid = h.loanid
            and h.loan_holdings_id = b.lh_id
            and b.accountid = @accid
            and l.dd_lastdate < @enddate
            and l.dd_lastdate > @startdate       ) v
            group by  loanid, dd_lastdate
            order by  dd_lastdate, loanid
            )"


        dsMaturity = New DataSet
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.SelectCommand.Parameters.Add("@StartDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = QuaterStartDate
        Adaptor.SelectCommand.Parameters.Add("EndDate", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = QuaterEndDate
        Adaptor.SelectCommand.Parameters.Add("@accid", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = iaccid
        Adaptor.Fill(dsMaturity)
        MyConn.Close()



        Dim newLoanMaturity As New LoanMaturity



        dr2 = dsMaturity.Tables(0).Rows(0)
        If Not IsDBNull(dr2("TheAmount")) Then
            newLoanMaturity.Amount = dr2("TheAmount")
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

        s.Name = "Deployed"
        s2.Name = "Repayment"

        'Change to a line graph.

        s.ChartType = SeriesChartType.Line
        s2.ChartType = SeriesChartType.Column



        For d = 0 To d - 1
            s.Points.AddXY(LoanMaturityList(d).Quarter, LoanMaturityList(d).Balance)
            s2.Points.AddXY(LoanMaturityList(d).Quarter, LoanMaturityList(d).Amount)
        Next



        'Add the series to the Chart1 control.

        Chart2.Series.Add(s)
        Chart2.Series.Add(s2)




    End Sub

End Class
