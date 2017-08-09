'Imports System.IO
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Data
'Imports System.Drawing


'Public Class Canvas
'    Private multipleSelect As Boolean = False
'    Private clicked As Boolean = False

'    Private XMax As Double, XMin As Double
'    Private YMax As Double, YMin As Double

'    Private scaleX As Double = 1
'    Private scaleY As Double = 1
'    Private mainScale As Double = 1

'    Private aPoint As Point
'    Private sizeChanged As Boolean = False

'    Private startPoint As Point
'    Private endPoint As Point

'    Private Shared exPoint As Point

'    Private drawingList As ArrayList
'    Private objectIdentifier As ArrayList

'    Public onCanvas As Boolean = False
'    Private thePolyLine As Polyline = Nothing

'    Private polyLineStarting As Boolean = True
'    Private CanIDraw As Boolean = False

'    Private theSourceFile As FileInfo

'    Private highlightedRegion As New rectangle(0, 0, 0, 0)


'    Public Sub New()

'        startPoint = New Point(0, 0)
'        endPoint = New Point(0, 0)
'        exPoint = New Point(0, 0)

'        drawingList = New ArrayList()
'        objectIdentifier = New ArrayList()

'    End Sub

'    Public Sub Draw(g As Graphics)
'        Dim lePen As New Pen(Color.White, 3)

'        If YMin < 0 Then
'            g.TranslateTransform(0, -CInt(Math.Abs(YMin)))
'        End If
'        'transforms point-of-origin to the lower left corner of the canvas.
'        If XMin < 0 Then
'            g.TranslateTransform(CInt(Math.Abs(XMin)), 0)
'        End If

'        '	g.SmoothingMode = SmoothingMode.AntiAlias; 

'        For Each obj In objectIdentifier
'            'iterates through the objects
'            Select Case obj.shapeType
'                Case 2
'                    'line

'                Case 3
'                    'rectangle 

'                Case 4
'                    'circle

'                Case 5
'                    'polyline
'                Case 6
'                    'arc
'            End Select
'        Next


'        '	g.Dispose();		//not disposed because "g" is get from the paintbackground event..
'        lePen.Dispose()
'    End Sub

'    Private Function CalculateRadius() As Double
'        'this helper function is used to calculate the radius for the circle-drawing mode.
'        Dim circleRadius As Double = Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y))
'        Return circleRadius
'    End Function


'#Region "DXF Data Extraction and Interpretation"

'    Public Sub ReadFromFile(textFile As String)
'        'Reads a text file (in fact a DXF file) for importing an Autocad drawing.
'        'In the DXF File structure, data is stored in two-line groupings ( or bi-line, coupling line ...whatever you call it)
'        'in this grouping the first line defines the data, the second line contains the data value.
'        '..as a result there is always even number of lines in the DXF file..
'        Dim line1 As String, line2 As String
'        'these line1 and line2 is used for getting the a/m data groups...
'        line1 = "0"
'        'line1 and line2 are are initialized here...
'        line2 = "0"

'        Dim position As Long = 0

'        theSourceFile = New FileInfo(textFile)
'        'the sourceFile is set.
'        Dim reader As StreamReader = Nothing
'        'a reader is prepared...
'        Try
'            'the reader is set ...
'            reader = theSourceFile.OpenText()
'        Catch e As FileNotFoundException
'            MsgBox(e.FileName.ToString() + " cannot be found")
'        Catch
'            MsgBox("An error occured while opening the DXF file")
'            Return
'        End Try




'        Do
'            '''/////////////////////////////////////////////////////////////////
'            'This part interpretes the drawing objects found in the DXF file...
'            '''/////////////////////////////////////////////////////////////////

'            If line1 = "0" AndAlso line2 = "LINE" Then
'                LineModule(reader)

'            ElseIf line1 = "0" AndAlso line2 = "LWPOLYLINE" Then
'                PolylineModule(reader)

'            ElseIf line1 = "0" AndAlso line2 = "CIRCLE" Then
'                CircleModule(reader)

'            ElseIf line1 = "0" AndAlso line2 = "ARC" Then
'                ArcModule(reader)
'            End If

'            '''/////////////////////////////////////////////////////////////////
'            '''/////////////////////////////////////////////////////////////////


'            'the related method is called for iterating through the text file and assigning values to line1 and line2...
'            GetLineCouple(reader, line1, line2)
'        Loop While line2 <> "EOF"



'        reader.DiscardBufferedData()
'        'reader is cleared...
'        theSourceFile = Nothing


'        reader.Close()
'        '...and closed.
'    End Sub


'    Private Sub GetLineCouple(theReader As StreamReader, ByRef line1 As String, ByRef line2 As String)
'        'this method is used to iterate through the text file and assign values to line1 and line2
'        Dim ci As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
'        Dim decimalSeparator As String = ci.NumberFormat.CurrencyDecimalSeparator

'        line1 = ""
'        line2 = ""

'        If theReader Is Nothing Then
'            Return
'        End If

'        line1 = theReader.ReadLine()
'        If line1 IsNot Nothing Then
'            line1 = line1.Trim()

'            line1 = line1.Replace("."c, decimalSeparator(0))
'        End If
'        line2 = theReader.ReadLine()
'        If line2 IsNot Nothing Then
'            line2 = line2.Trim()
'            line2 = line2.Replace("."c, decimalSeparator(0))
'        End If
'    End Sub


'    Private Sub LineModule(reader As StreamReader)
'        'Interpretes line objects in the DXF file
'        Dim line1 As String, line2 As String
'        line1 = "0"
'        line2 = "0"

'        Dim x1 As Double = 0
'        Dim y1 As Double = 0
'        Dim x2 As Double = 0
'        Dim y2 As Double = 0

'        Do
'            GetLineCouple(reader, line1, line2)

'            If line1 = "10" Then
'                x1 = Convert.ToDouble(line2)

'                If x1 > XMax Then
'                    XMax = x1
'                End If

'                If x1 < XMin Then
'                    XMin = x1
'                End If
'            End If

'            If line1 = "20" Then
'                y1 = Convert.ToDouble(line2)
'                If y1 > YMax Then
'                    YMax = y1
'                End If

'                If y1 < YMin Then
'                    YMin = y1
'                End If
'            End If

'            If line1 = "11" Then
'                x2 = Convert.ToDouble(line2)

'                If x2 > XMax Then
'                    XMax = x2
'                End If

'                If x2 < XMin Then
'                    XMin = x2
'                End If
'            End If

'            If line1 = "21" Then
'                y2 = Convert.ToDouble(line2)

'                If y2 > YMax Then
'                    YMax = y2
'                End If

'                If y2 < YMin Then
'                    YMin = y2
'                End If


'            End If
'        Loop While line1 <> "21"
'    End Sub


'    Private Sub PolylineModule(reader As StreamReader)
'        'Interpretes polyline objects in the DXF file
'        Dim line1 As String, line2 As String
'        line1 = "0"
'        line2 = "0"

'        Dim x1 As Double = 0
'        Dim y1 As Double = 0
'        Dim x2 As Double = 0
'        Dim y2 As Double = 0


'        thePolyLine = New polyline()

'        Dim ix As Integer 

'        Dim counter As Integer = 0
'        Dim numberOfVertices As Integer = 1
'        Dim openOrClosed As Integer = 0
'        Dim pointList As New ArrayList()


'        Do
'            GetLineCouple(reader, line1, line2)

'            If line1 = "90" Then
'                numberOfVertices = Convert.ToInt32(line2)
'            End If

'            If line1 = "70" Then
'                openOrClosed = Convert.ToInt32(line2)
'            End If


'            If line1 = "10" Then
'                x1 = Convert.ToDouble(line2)
'                If x1 > XMax Then
'                    XMax = x1
'                End If

'                If x1 < XMin Then
'                    XMin = x1
'                End If
'            End If

'            If line1 = "20" Then
'                y1 = Convert.ToDouble(line2)

'                If y1 > YMax Then
'                    YMax = y1
'                End If

'                If y1 < YMin Then
'                    YMin = y1
'                End If

'                pointList.Add(New Point(CInt(x1), CInt(-y1)))
'                counter += 1

'            End If
'        Loop While counter < numberOfVertices
'    End Sub


'    Private Sub CircleModule(reader As StreamReader)
'        'Interpretes circle objects in the DXF file
'        Dim line1 As String, line2 As String
'        line1 = "0"
'        line2 = "0"

'        Dim x1 As Double = 0
'        Dim y1 As Double = 0

'        Dim radius As Double = 0

'        Do
'            GetLineCouple(reader, line1, line2)

'            If line1 = "10" Then

'                x1 = Convert.ToDouble(line2)
'            End If


'            If line1 = "20" Then

'                y1 = Convert.ToDouble(line2)
'            End If


'            If line1 = "40" Then
'                radius = Convert.ToDouble(line2)

'                If (x1 + radius) > XMax Then
'                    XMax = x1 + radius
'                End If

'                If (x1 - radius) < XMin Then
'                    XMin = x1 - radius
'                End If

'                If y1 + radius > YMax Then
'                    YMax = y1 + radius
'                End If

'                If (y1 - radius) < YMin Then
'                    YMin = y1 - radius

'                End If



'            End If
'        Loop While line1 <> "40"
'    End Sub


'    Private Sub ArcModule(reader As StreamReader)
'        'Interpretes arc objects in the DXF file
'        Dim line1 As String, line2 As String
'        line1 = "0"
'        line2 = "0"

'        Dim x1 As Double = 0
'        Dim y1 As Double = 0

'        Dim radius As Double = 0
'        Dim angle1 As Double = 0
'        Dim angle2 As Double = 0

'        Do
'            GetLineCouple(reader, line1, line2)

'            If line1 = "10" Then
'                x1 = Convert.ToDouble(line2)
'                If x1 > XMax Then
'                    XMax = x1
'                End If
'                If x1 < XMin Then
'                    XMin = x1

'                End If
'            End If


'            If line1 = "20" Then
'                y1 = Convert.ToDouble(line2)
'                If y1 > YMax Then
'                    YMax = y1
'                End If
'                If y1 < YMin Then
'                    YMin = y1
'                End If
'            End If


'            If line1 = "40" Then
'                radius = Convert.ToDouble(line2)

'                If (x1 + radius) > XMax Then
'                    XMax = x1 + radius
'                End If

'                If (x1 - radius) < XMin Then
'                    XMin = x1 - radius
'                End If

'                If y1 + radius > YMax Then
'                    YMax = y1 + radius
'                End If

'                If (y1 - radius) < YMin Then
'                    YMin = y1 - radius
'                End If
'            End If

'            If line1 = "50" Then
'                angle1 = Convert.ToDouble(line2)
'            End If

'            If line1 = "51" Then
'                angle2 = Convert.ToDouble(line2)


'            End If
'        Loop While line1 <> "51"
'    End Sub


'#End Region

'End Class

