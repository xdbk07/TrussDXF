Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class frmMain
   
    Private XMax As Double, XMin As Double
    Private YMax As Double, YMin As Double


    Private mainScale As Double = 1
    Private cnt As Long

    Private gEnts As New List(Of mEnt)
    Private gLayers As New SortedList(Of String, Boolean)

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub _DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
            If Path.GetExtension(files(0)).Equals(".dxf", StringComparison.CurrentCultureIgnoreCase) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub

    Private Sub _DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        If Path.GetExtension(files(0)).Equals(".dxf", StringComparison.CurrentCultureIgnoreCase) Then
            OpenDXF(files(0))
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        Dim g = pbMain.CreateGraphics
        g.Clear(Color.Black)
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs)
        Dim ofd As New OpenFileDialog
        ofd.Filter = "(DXF file)|*.dxf"
        If ofd.ShowDialog <> Windows.Forms.DialogResult.OK Then Return
        '
        OpenDXF(ofd.FileName)
    End Sub

    Private Sub OpenDXF(textFile As String)
        ResetParameter()
        '
        'Reads a text file (in fact a DXF file) for importing an Autocad drawing.
        'In the DXF File structure, data is stored in two-line groupings ( or bi-line, coupling line ...whatever you call it)
        'in this grouping the first line defines the data, the second line contains the data value.
        '..as a result there is always even number of lines in the DXF file..
        Dim line1 As String, line2 As String
        line1 = "0"
        line2 = "0"
        Dim theSourceFile = New FileInfo(textFile)
        'the sourceFile is set.
        Dim reader As StreamReader = Nothing
        'a reader is prepared...
        Try
            reader = theSourceFile.OpenText()
        Catch ex As FileNotFoundException
            MessageBox.Show(ex.FileName.ToString() + " cannot be found")
        Catch
            MessageBox.Show("An error occured while opening the DXF file")
            Return
        End Try
        '
        Do
            If line1 = "0" AndAlso line2 = "LINE" Then
                LineModule(reader)
            ElseIf line1 = "0" AndAlso line2 = "LWPOLYLINE" Then
                PolylineModule(reader)
                ''ElseIf line1 = "0" AndAlso line2 = "CIRCLE" Then
                ''    CircleModule(reader)
                ''ElseIf line1 = "0" AndAlso line2 = "ARC" Then
                ''    ArcModule(reader)
            ElseIf line1 = "0" AndAlso line2 = "LAYER" Then
                LayerModule(reader)
            End If
            '
            GetLineCouple(reader, line1, line2)
        Loop While line2 <> "EOF"
        '
        reader.DiscardBufferedData()
        'reader is cleared...
        theSourceFile = Nothing
        '
        reader.Close()
        '...and closed.
        RecalculateScale()
        Refresh()
    End Sub

    Private Sub pbMain_SizeChanged(sender As Object, e As EventArgs) Handles pbMain.SizeChanged
        RecalculateScale()
        Refresh()
    End Sub

    Public Sub RecalculateScale()
        mainScale = Math.Min(pbMain.Size.Width / (XMax - XMin + 5), pbMain.Size.Height / (YMax - YMin))
    End Sub

    Private Sub ResetParameter()
        Dim g = pbMain.CreateGraphics
        g.Clear(Color.Black)
        g.Dispose()
        '
        gEnts.Clear()
        gLayers.Clear()
        cnt = 0
        '
        mainScale = 1
        XMax = -99999999
        XMin = 99999999
        YMax = -99999999
        YMin = 99999999
    End Sub

    Public Sub Draw_new(g As Graphics)
        If gEnts.Count = 0 Then Return
        '
        Dim mPen As New Pen(Color.White, 1)
        '
        g.TranslateTransform(pbMain.Location.X + 5, pbMain.Location.Y + pbMain.Size.Height - 20)
        '
        For i = 0 To gEnts.Count - 1
            GetClosedDistance(i, gEnts)
            '
            gEnts(i).Draw(mPen, g, mainScale, XMin, YMin)
        Next
        '
        mPen.Dispose()
    End Sub

    Private Sub GetLineCouple(theReader As StreamReader, ByRef line1 As String, ByRef line2 As String)
        'this method is used to iterate through the text file and assign values to line1 and line2
        Dim ci As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Dim decimalSeparator As String = ci.NumberFormat.CurrencyDecimalSeparator

        line1 = ""
        line2 = ""

        If theReader Is Nothing Then
            Return
        End If

        line1 = theReader.ReadLine()
        Cnt += 1
        If line1 IsNot Nothing Then
            line1 = line1.Trim()
            line1 = line1.Replace("."c, decimalSeparator(0))
        End If
        '
        line2 = theReader.ReadLine()
        Cnt += 1
        If line2 IsNot Nothing Then
            line2 = line2.Trim()
            line2 = line2.Replace("."c, decimalSeparator(0))
        End If
    End Sub

    Private Sub LayerModule(reader As StreamReader)
        'Interpretes line objects in the DXF file
        Dim line1 As String, line2 As String
        line1 = "0"
        line2 = "0"

        Dim x1 As Double = 0
        Dim y1 As Double = 0
        Dim x2 As Double = 0
        Dim y2 As Double = 0
        Dim Layer As String = ""
        Dim isOff As Boolean
        '
        Do
            GetLineCouple(reader, line1, line2)
            '
            If line1 = "2" Then
                'Layer
                Layer = line2
            ElseIf line1 = "62" Then
                isOff = (Convert.ToDouble(line2) < 0)
            End If
        Loop While line1 <> "62"
        '
        If Layer <> "" Then
            gLayers.Add(Layer, isOff)
        End If
    End Sub

    Private Sub LineModule(reader As StreamReader)
        'Interpretes line objects in the DXF file
        Dim line1 As String, line2 As String
        line1 = "0"
        line2 = "0"

        Dim x1 As Double = 0
        Dim y1 As Double = 0
        Dim x2 As Double = 0
        Dim y2 As Double = 0
        Dim layer As String = ""

        '
        Do
            GetLineCouple(reader, line1, line2)
            '
            If line1 = "8" Then
                layer = line2.ToUpper
                ''LAYER
                If line2.Equals("DIMENSIONS", StringComparison.CurrentCultureIgnoreCase) Or line2.Equals("PLATES", StringComparison.CurrentCultureIgnoreCase) Then
                    Exit Sub
                End If
            ElseIf line1 = "10" Then
                x1 = Convert.ToDouble(line2)
                XMax = Math.Max(x1, XMax)
                XMin = Math.Min(x1, XMin)
            ElseIf line1 = "20" Then
                y1 = Convert.ToDouble(line2)
                YMax = Math.Max(y1, YMax)
                YMin = Math.Min(y1, YMin)
            ElseIf line1 = "11" Then
                x2 = Convert.ToDouble(line2)
                XMax = Math.Max(x2, XMax)
                XMin = Math.Min(x2, XMin)
            ElseIf line1 = "21" Then
                y2 = Convert.ToDouble(line2)
                YMax = Math.Max(y2, YMax)
                YMin = Math.Min(y2, YMin)
            End If
        Loop While line1 <> "21"
        '
        Dim points As New List(Of PointF)({New PointF(x1, y1), New PointF(x2, y2)})

        If gLayers(layer) = False Then
            gEnts.Add(New mEnt(ObjType.Line, layer, points))
        End If
    End Sub

    Private Sub PolylineModule(reader As StreamReader)
        'Interpretes polyline objects in the DXF file
        Dim line1 As String, line2 As String
        line1 = "0"
        line2 = "0"

        Dim x1 As Double = 0
        Dim y1 As Double = 0
        Dim x2 As Double = 0
        Dim y2 As Double = 0

        Dim counter As Integer = 0
        Dim numberOfVertices As Integer = 1
        '
        Dim layer As String = ""
        Dim flag As Boolean = False
        Dim lst As New List(Of PointF)
        '
        Do
            GetLineCouple(reader, line1, line2)
            '
            If flag = False Then
                If line1 = "8" Then
                    line2 = line2.ToUpper()
                    layer = line2
                    ''LAYER
                    If line2 Like "DIMENSION*" Or line2 Like "PLATE*" Or line2 Like "$*" Or line2 Like "*SOLID" Then
                        flag = True
                    End If
                ElseIf line1 = "90" Then
                    numberOfVertices = Convert.ToInt32(line2)
                ElseIf line1 = "70" Then
                    'cEnt.isClosed = (Convert.ToInt32(line2) <> 0)
                ElseIf line1 = "10" Then
                    x1 = Convert.ToDouble(line2)
                    XMax = Math.Max(x1, XMax)
                    XMin = Math.Min(x1, XMin)
                ElseIf line1 = "20" Then
                    y1 = Convert.ToDouble(line2)
                    YMax = Math.Max(y1, YMax)
                    YMin = Math.Min(y1, YMin)
                    '
                    lst.Add(New PointF(x1, y1))
                    counter += 1
                End If
            Else
                If line1 = "20" Then
                    counter += 1
                End If
            End If
        Loop While counter < numberOfVertices
        '
        If numberOfVertices > 10 Then Exit Sub
        '
        If lst.Count > 0 AndAlso gLayers(layer) = False Then
            gEnts.Add(New mEnt(ObjType.Polyline, layer, lst))
        End If
    End Sub

    'Private Sub PolylineModule(reader As StreamReader)
    '    'Interpretes polyline objects in the DXF file
    '    Dim line1 As String, line2 As String
    '    line1 = "0"
    '    line2 = "0"

    '    Dim x1 As Double = 0
    '    Dim y1 As Double = 0
    '    Dim x2 As Double = 0
    '    Dim y2 As Double = 0

    '    Dim thePolyLine = New mPolyline(Color.White, 1)

    '    Dim counter As Integer = 0
    '    Dim numberOfVertices As Integer = 1
    '    Dim openOrClosed As Integer = 0
    '    Dim pointList As New ArrayList()

    '    Do
    '        GetLineCouple(reader, line1, line2)
    '        '
    '        If line1 = "8" Then
    '            ''LAYER
    '            'If line2.Equals("DIMENSIONS", StringComparison.CurrentCultureIgnoreCase) Or line2.Equals("PLATES", StringComparison.CurrentCultureIgnoreCase) Then
    '            '    numberOfVertices = 100
    '            'End If
    '        ElseIf line1 = "90" Then
    '            numberOfVertices = Convert.ToInt32(line2)
    '        ElseIf line1 = "70" Then
    '            openOrClosed = Convert.ToInt32(line2)
    '        ElseIf line1 = "10" Then
    '            x1 = Convert.ToDouble(line2)
    '            If x1 > XMax Then
    '                XMax = x1
    '            End If

    '            If x1 < XMin Then
    '                XMin = x1
    '            End If
    '        ElseIf line1 = "20" Then
    '            y1 = Convert.ToDouble(line2)

    '            If y1 > YMax Then
    '                YMax = y1
    '            End If

    '            If y1 < YMin Then
    '                YMin = y1
    '            End If

    '            pointList.Add(New Point(CInt(x1), CInt(-y1)))
    '            counter += 1
    '        End If
    '    Loop While counter < numberOfVertices

    '    '****************************************************************************************************//
    '    '***************This Part is related with the drawing editor...the data taken from the dxf file******//
    '    '***************is interpreted hereinafter***********************************************************//

    '    For i As Integer = 1 To numberOfVertices - 1
    '        thePolyLine.AppendLine(New mLine(DirectCast(pointList(i - 1), Point), DirectCast(pointList(i), Point), Color.White, 1))
    '    Next
    '    If openOrClosed = 1 Then
    '        thePolyLine.AppendLine(New mLine(DirectCast(pointList(numberOfVertices - 1), Point), DirectCast(pointList(0), Point), Color.White, 1))
    '    End If
    '    '
    '    If numberOfVertices < 8 Then
    '        Dim ix As Integer = drawingList.Add(thePolyLine)
    '        objectIdentifier.Add(New mObj(5, ix))
    '    End If
    'End Sub

    'Private Sub CircleModule(reader As StreamReader)
    '    'Interpretes circle objects in the DXF file
    '    Dim line1 As String, line2 As String
    '    line1 = "0"
    '    line2 = "0"

    '    Dim x1 As Double = 0
    '    Dim y1 As Double = 0

    '    Dim radius As Double = 0

    '    Do
    '        GetLineCouple(reader, line1, line2)

    '        If line1 = "10" Then

    '            x1 = Convert.ToDouble(line2)
    '        End If


    '        If line1 = "20" Then

    '            y1 = Convert.ToDouble(line2)
    '        End If


    '        If line1 = "40" Then
    '            radius = Convert.ToDouble(line2)

    '            If (x1 + radius) > XMax Then
    '                XMax = x1 + radius
    '            End If

    '            If (x1 - radius) < XMin Then
    '                XMin = x1 - radius
    '            End If

    '            If y1 + radius > YMax Then
    '                YMax = y1 + radius
    '            End If

    '            If (y1 - radius) < YMin Then
    '                YMin = y1 - radius

    '            End If



    '        End If
    '    Loop While line1 <> "40"

    '    '****************************************************************************************************//
    '    '***************This Part is related with the drawing editor...the data taken from the dxf file******//
    '    '***************is interpreted hereinafter***********************************************************//

    '    Dim ix As Integer = drawingList.Add(New mCircle(New Point(CInt(x1), CInt(-y1)), radius, Color.White, Color.Red, 1))
    '    objectIdentifier.Add(New mObj(4, ix))

    'End Sub

    'Private Sub ArcModule(reader As StreamReader)
    '    'Interpretes arc objects in the DXF file
    '    Dim line1 As String, line2 As String
    '    line1 = "0"
    '    line2 = "0"

    '    Dim x1 As Double = 0
    '    Dim y1 As Double = 0

    '    Dim radius As Double = 0
    '    Dim angle1 As Double = 0
    '    Dim angle2 As Double = 0

    '    Do
    '        GetLineCouple(reader, line1, line2)
    '        '
    '        If line1 = "10" Then
    '            x1 = Convert.ToDouble(line2)
    '            If x1 > XMax Then
    '                XMax = x1
    '            End If
    '            If x1 < XMin Then
    '                XMin = x1

    '            End If
    '        End If
    '        If line1 = "20" Then
    '            y1 = Convert.ToDouble(line2)
    '            If y1 > YMax Then
    '                YMax = y1
    '            End If
    '            If y1 < YMin Then
    '                YMin = y1
    '            End If
    '        End If
    '        If line1 = "40" Then
    '            radius = Convert.ToDouble(line2)

    '            If (x1 + radius) > XMax Then
    '                XMax = x1 + radius
    '            End If

    '            If (x1 - radius) < XMin Then
    '                XMin = x1 - radius
    '            End If

    '            If y1 + radius > YMax Then
    '                YMax = y1 + radius
    '            End If

    '            If (y1 - radius) < YMin Then
    '                YMin = y1 - radius
    '            End If
    '        End If
    '        If line1 = "50" Then
    '            angle1 = Convert.ToDouble(line2)
    '        End If
    '        If line1 = "51" Then
    '            angle2 = Convert.ToDouble(line2)
    '        End If
    '    Loop While line1 <> "51"
    '    '****************************************************************************************************//
    '    '***************This Part is related with the drawing editor...the data taken from the dxf file******//
    '    '***************is interpreted hereinafter***********************************************************//
    '    Dim ix As Integer = drawingList.Add(New mArc(New Point(CInt(x1), CInt(-y1)), radius, angle1, angle2, Color.White, Color.Red, 1))
    '    objectIdentifier.Add(New mObj(6, ix))
    'End Sub


    'Public Sub Draw_new(g As Graphics)
    '    Dim mPen As New Pen(Color.White, 3)

    '    g.TranslateTransform(pbMain.Location.X + 1, pbMain.Location.Y + pbMain.Size.Height - 1)

    '    If YMin < 0 Then
    '        g.TranslateTransform(0, -CInt(Math.Abs(YMin)))
    '    End If
    '    'transforms point-of-origin to the lower left corner of the canvas.
    '    If XMin < 0 Then
    '        g.TranslateTransform(CInt(Math.Abs(XMin)), 0)
    '    End If
    '    '	g.SmoothingMode = SmoothingMode.AntiAlias; 
    '    For Each obj As mObj In objectIdentifier
    '        'iterates through the objects
    '        Select Case obj.shapeType
    '            Case 2
    '                'line
    '                If True Then
    '                    Dim temp As mLine = DirectCast(drawingList(obj.indexNo), mLine)
    '                    mPen.Color = temp.AccessContourColor
    '                    mPen.Width = temp.AccessLineWidth
    '                    If mainScale = 0 Then
    '                        mainScale = 1
    '                    End If
    '                    temp.Draw(mPen, g, mainScale)
    '                    Exit Select
    '                End If
    '            Case 3
    '                'rectangle 
    '                If True Then
    '                    Dim temp As mRectangle = DirectCast(drawingList(obj.indexNo), mRectangle)
    '                    mPen.Color = temp.AccessContourColor
    '                    mPen.Width = temp.AccessLineWidth
    '                    temp.Draw(mPen, g)

    '                    Exit Select
    '                End If
    '            Case 4
    '                'circle
    '                If True Then
    '                    Dim temp As mCircle = DirectCast(drawingList(obj.indexNo), mCircle)
    '                    mPen.Color = temp.AccessContourColor
    '                    mPen.Width = temp.AccessLineWidth
    '                    If mainScale = 0 Then
    '                        mainScale = 1
    '                    End If
    '                    temp.Draw(mPen, g, mainScale)
    '                    Exit Select
    '                End If
    '            Case 5
    '                'polyline
    '                If True Then
    '                    Dim temp As mPolyline = DirectCast(drawingList(obj.indexNo), mPolyline)
    '                    mPen.Color = temp.AccessContourColor
    '                    mPen.Width = temp.AccessLineWidth
    '                    If mainScale = 0 Then
    '                        mainScale = 1
    '                    End If
    '                    temp.Draw(mPen, g, mainScale)
    '                    Exit Select
    '                End If
    '            Case 6
    '                'arc
    '                If True Then
    '                    Dim temp As mArc = DirectCast(drawingList(obj.indexNo), mArc)

    '                    mPen.Color = temp.AccessContourColor
    '                    mPen.Width = temp.AccessLineWidth

    '                    If mainScale = 0 Then
    '                        mainScale = 1
    '                    End If

    '                    temp.Draw(mPen, g, mainScale)

    '                    Exit Select
    '                End If
    '        End Select
    '    Next
    '    '	g.Dispose();		//not disposed because "g" is get from the paintbackground event..
    '    mPen.Dispose()
    'End Sub

    Private Sub pbMain_Paint(sender As Object, e As PaintEventArgs) Handles pbMain.Paint
        'all drawing is made here in OnPaintBackground...
        MyBase.OnPaintBackground(e)
        If Me.WindowState = FormWindowState.Minimized Then Return

        Dim g As Graphics = e.Graphics

        Dim rect As New Rectangle(pbMain.Location, pbMain.Size)
        Dim brush As New LinearGradientBrush(rect, Color.Black, Color.Black, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal)

        If Me.WindowState <> FormWindowState.Minimized Then
            e.Graphics.FillRectangle(brush, rect)
            'All drawing is made here...
            Draw_new(g)
        End If

        g = Nothing
        brush.Dispose()
    End Sub

End Class