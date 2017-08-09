''Imports System.Collections
''Imports System.Drawing
''Imports System.Drawing.Drawing2D


''#Region "Shape class - abstract"

''Public MustInherit Class mShape
''    Protected contourColor As Color
''    Protected fillColor As Color
''    Protected lineWidth As Integer

''    Public shapeIdentifier As Integer
''    Public rotation As Integer
''    Public highlighted As Boolean
''    Public layer As String

''    Public MustOverride Property AccessContourColor() As Color
''    Public MustOverride Property AccessFillColor() As Color
''    Public MustOverride Property AccessLineWidth() As Integer
''    Public MustOverride Property AccessRotation() As Integer
''    Public MustOverride Sub Draw(pen As Pen, g As Graphics)
''    Public MustOverride Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean

''End Class
''#End Region

''#Region "Line class"
''Public Class mLine
''    Inherits mShape
''    Protected startPoint As Point
''    Protected endPoint As Point
''    '
''    Public Sub New(startPt As Point, endPt As Point, color As Color, w As Integer)
''        startPoint = startPt
''        endPoint = endPt
''        contourColor = color
''        lineWidth = w
''        shapeIdentifier = 1
''        rotation = 0
''    End Sub


''    Public Sub New()
''    End Sub

''    Public Overrides Property AccessContourColor() As Color
''        Get
''            Return contourColor
''        End Get
''        Set(value As Color)
''            contourColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessFillColor() As Color
''        Get
''            Return fillColor
''        End Get
''        Set(value As Color)
''            fillColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessLineWidth() As Integer
''        Get
''            Return lineWidth
''        End Get
''        Set(value As Integer)
''            lineWidth = value
''        End Set
''    End Property

''    Public Overrides Property AccessRotation() As Integer
''        Get
''            Return rotation
''        End Get
''        Set(value As Integer)
''            rotation = value
''        End Set
''    End Property

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        g.DrawLine(pen, startPoint, endPoint)
''    End Sub

''    Public Overloads Sub Draw(pen As Pen, g As Graphics, scale As Double)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        g.DrawLine(pen, CSng(startPoint.X) * CSng(scale), CSng(startPoint.Y) * CSng(scale), CSng(endPoint.X) * CSng(scale), CSng(endPoint.Y) * CSng(scale))
''    End Sub

''    Public Overridable ReadOnly Property GetStartPoint() As Point
''        Get
''            Return startPoint
''        End Get
''    End Property

''    Public Overridable ReadOnly Property GetEndPoint() As Point
''        Get
''            Return endPoint
''        End Get
''    End Property

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        Dim areaPath As GraphicsPath
''        Dim areaPen As Pen
''        Dim areaRegion As Region

''        ' Create path which contains wide line
''        ' for easy mouse selection
''        areaPath = New GraphicsPath()
''        areaPen = New Pen(Color.Red, 7)

''        areaPath.AddLine(GetStartPoint.X, GetStartPoint.Y, GetEndPoint.X, GetEndPoint.Y)
''        ' startPoint and EndPoint are class members of type Point
''        areaPath.Widen(areaPen)

''        ' Create region from the path
''        areaRegion = New Region(areaPath)

''        If areaRegion.IsVisible(point) = True Then
''            'g.DrawLine(pen, GetStartPoint, GetEndPoint);

''            'g.DrawLine(pen, GetStartPoint.X, GetStartPoint.Y , GetEndPoint.X, GetEndPoint.Y);

''            areaPath.Dispose()
''            areaPen.Dispose()
''            areaRegion.Dispose()

''            Return True
''        End If

''        Return False
''    End Function

''    Public Overloads Function Highlight(pen As Pen, g As Graphics, point As Point, scale As Double) As Boolean
''        Dim areaPath As GraphicsPath
''        Dim areaPen As Pen
''        Dim areaRegion As Region

''        ' Create path which contains wide line
''        ' for easy mouse selection
''        areaPath = New GraphicsPath()
''        areaPen = New Pen(Color.Red, 7)

''        areaPath.AddLine(CSng(GetStartPoint.X) * CSng(scale), CSng(GetStartPoint.Y) * CSng(scale), CSng(GetEndPoint.X) * CSng(scale), CSng(GetEndPoint.Y) * CSng(scale))
''        ' startPoint and EndPoint are class members of type Point
''        areaPath.Widen(areaPen)

''        ' Create region from the path
''        Try
''            areaRegion = New Region(areaPath)
''        Catch
''            Return False
''        End Try

''        If areaRegion.IsVisible(point) = True Then
''            'g.DrawLine(pen, GetStartPoint, GetEndPoint);

''            'g.DrawLine(pen, (float)GetStartPoint.X* (float)scale, (float)GetStartPoint.Y * (float)scale, (float)GetEndPoint.X* (float)scale, (float)GetEndPoint.Y* (float)scale);

''            areaPath.Dispose()
''            areaPen.Dispose()
''            areaRegion.Dispose()

''            Return True
''        End If

''        areaPath.Dispose()
''        areaPen.Dispose()
''        areaRegion.Dispose()

''        Return False
''    End Function
''End Class
''#End Region

''#Region "Rectangle class"
''Public Class mRectangle
''    Inherits mLine
''    Public Sub New(start As Point, [end] As Point, color As Color, fill As Color, w As Integer, angle As Integer)
''        startPoint = start
''        endPoint = [end]
''        contourColor = color
''        fillColor = fill
''        lineWidth = w
''        shapeIdentifier = 2

''        rotation = angle
''    End Sub

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        If AccessRotation <> 0 Then
''            DrawRotatedRectangle(pen, g)
''            Return
''        End If

''        g.DrawLine(pen, GetStartPoint.X, GetStartPoint.Y, GetEndPoint.X, GetStartPoint.Y)
''        g.DrawLine(pen, GetEndPoint.X, GetStartPoint.Y, GetEndPoint.X, GetEndPoint.Y)
''        g.DrawLine(pen, GetEndPoint.X, GetEndPoint.Y, GetStartPoint.X, GetEndPoint.Y)
''        g.DrawLine(pen, GetStartPoint.X, GetEndPoint.Y, GetStartPoint.X, GetStartPoint.Y)

''        Return
''    End Sub

''    Private Sub DrawRotatedRectangle(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        Dim P1 As Point = GetStartPoint
''        Dim P2 As Point = GetEndPoint

''        Dim P3 As New Point(P2.X, P1.Y)
''        Dim P4 As New Point(P1.X, P2.Y)


''        Dim center As New Point(P1.X + (P3.X - P1.X) / 2, P1.Y + (P4.Y - P1.Y) / 2)

''        Dim angle As Integer = AccessRotation

''        If angle <> 0 Then

''            P1 = CalculateRotatedNewPoint(P1, center, angle)
''            'Top left
''            P3 = CalculateRotatedNewPoint(P3, center, angle)
''            'Bottom right
''            P2 = CalculateRotatedNewPoint(P2, center, angle)
''            'Top right
''            P4 = CalculateRotatedNewPoint(P4, center, angle)
''            'Bottom left

''            g.DrawLine(pen, P1, P3)
''            g.DrawLine(pen, P3, P2)
''            g.DrawLine(pen, P2, P4)
''            g.DrawLine(pen, P4, P1)


''            Return
''        End If

''    End Sub

''    Private Function CalculateRotatedNewPoint(P As Point, center As Point, angle As Integer) As Point
''        Dim angleRad As Double = angle * 1 / 57.2957

''        Dim tempPoint As New Point(P.X - center.X, P.Y - center.Y)

''        Dim radius As Double = Math.Sqrt((tempPoint.X * tempPoint.X) + (tempPoint.Y * tempPoint.Y))


''        Dim radiant1 As Double = Math.Acos(tempPoint.X / radius)

''        If tempPoint.X < 0 AndAlso tempPoint.Y < 0 Then
''            radiant1 = -radiant1
''        End If

''        If tempPoint.X > 0 AndAlso tempPoint.Y < 0 Then
''            radiant1 = -radiant1
''        End If

''        Dim radiant2 As Double = Math.Asin(tempPoint.Y / radius)

''        radiant1 = radiant1 + angleRad
''        radiant2 = radiant2 + angleRad

''        Dim temp As Double
''        temp = radius * Math.Cos(radiant1)
''        P.X = CInt(temp) + center.X



''        temp = radius * Math.Sin(radiant1)
''        P.Y = CInt(temp) + center.Y


''        Return P
''    End Function

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        Dim P1 As Point = GetStartPoint
''        Dim P2 As Point = GetEndPoint

''        Dim P3 As New Point(P2.X, P1.Y)
''        Dim P4 As New Point(P1.X, P2.Y)

''        If AccessRotation <> 0 Then
''            Dim bottom As New Point(0, 0)
''            Dim top As New Point(0, 0)
''            Dim left As New Point(0, 0)
''            Dim right As New Point(0, 0)

''            Dim center As New Point(P1.X + (P3.X - P1.X) / 2, P1.Y + (P4.Y - P1.Y) / 2)

''            P1 = CalculateRotatedNewPoint(P1, center, AccessRotation)
''            P2 = CalculateRotatedNewPoint(P2, center, AccessRotation)
''            P3 = CalculateRotatedNewPoint(P3, center, AccessRotation)
''            P4 = CalculateRotatedNewPoint(P4, center, AccessRotation)

''            Dim maxX As Integer = Math.Max(P1.X, P2.X)
''            maxX = Math.Max(maxX, P3.X)
''            maxX = Math.Max(maxX, P4.X)

''            If maxX = P1.X Then
''                right = P1
''            End If
''            If maxX = P2.X Then
''                right = P2
''            End If
''            If maxX = P3.X Then
''                right = P3
''            End If
''            If maxX = P4.X Then
''                right = P4
''            End If

''            Dim minX As Integer = Math.Min(P1.X, P2.X)
''            minX = Math.Min(minX, P3.X)
''            minX = Math.Min(minX, P4.X)


''            If minX = P1.X Then
''                left = P1
''            End If
''            If minX = P2.X Then
''                left = P2
''            End If
''            If minX = P3.X Then
''                left = P3
''            End If
''            If minX = P4.X Then
''                left = P4
''            End If


''            Dim maxY As Integer = Math.Max(P1.Y, P2.Y)
''            maxY = Math.Max(maxY, P3.Y)
''            maxY = Math.Max(maxY, P4.Y)


''            If maxY = P1.Y Then
''                bottom = P1
''            End If
''            If maxY = P2.Y Then
''                bottom = P2
''            End If
''            If maxY = P3.Y Then
''                bottom = P3
''            End If
''            If maxY = P4.Y Then
''                bottom = P4
''            End If


''            Dim minY As Integer = Math.Min(P1.Y, P2.Y)
''            minY = Math.Min(minY, P3.Y)
''            minY = Math.Min(minY, P4.Y)


''            If minY = P1.Y Then
''                top = P1
''            End If
''            If minY = P2.Y Then
''                top = P2
''            End If
''            If minY = P3.Y Then
''                top = P3
''            End If
''            If minY = P4.Y Then
''                top = P4
''            End If


''            Dim c1 As Double = checkPosition(left, top, point)
''            Dim c2 As Double = checkPosition(right, top, point)
''            Dim c3 As Double = checkPosition(right, bottom, point)
''            Dim c4 As Double = checkPosition(left, bottom, point)

''            If (c1 > 0 AndAlso c2 > 0 AndAlso c3 < 0 AndAlso c4 < 0) Then

''                pen.Color = Color.LightGreen

''                Draw(pen, g)

''                Return True
''            End If
''        Else
''            Dim maxX As Integer = Math.Max(P1.X, P2.X)
''            maxX = Math.Max(maxX, P3.X)
''            maxX = Math.Max(maxX, P4.X)

''            Dim minX As Integer = Math.Min(P1.X, P2.X)
''            minX = Math.Min(minX, P3.X)
''            minX = Math.Min(minX, P4.X)

''            Dim maxY As Integer = Math.Max(P1.Y, P2.Y)
''            maxY = Math.Max(maxY, P3.Y)
''            maxY = Math.Max(maxY, P4.Y)

''            Dim minY As Integer = Math.Min(P1.Y, P2.Y)
''            minY = Math.Min(minY, P3.Y)
''            minY = Math.Min(minY, P4.Y)


''            If point.X > minX AndAlso point.X < maxX AndAlso point.Y > minY AndAlso point.Y < maxY Then
''                pen.Color = Color.LightGreen
''                '	pen.Width = 1;

''                Draw(pen, g)

''                Return True
''            End If
''        End If

''        Return False
''    End Function

''    Private Function checkPosition(P1 As Point, P2 As Point, current As Point) As Double
''        Dim m As Double = CDbl(P2.Y - P1.Y) / (P2.X - P1.X)
''        Return ((current.Y - P1.Y) - (m * (current.X - P1.X)))
''    End Function

''End Class
''#End Region

''#Region "Circle Class"
''Public Class mCircle
''    Inherits mShape
''    Private centerPoint As Point
''    Private radius As Double

''    Public Sub New(center As Point, r As Double, color1 As Color, color2 As Color, w As Integer)
''        centerPoint = center
''        radius = r
''        contourColor = color1
''        fillColor = color2
''        lineWidth = w
''        shapeIdentifier = 3
''        rotation = 0
''    End Sub

''    Public Overrides Property AccessContourColor() As Color
''        Get
''            Return contourColor
''        End Get
''        Set(value As Color)
''            contourColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessFillColor() As Color
''        Get
''            Return fillColor
''        End Get
''        Set(value As Color)
''            fillColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessLineWidth() As Integer
''        Get
''            Return lineWidth
''        End Get
''        Set(value As Integer)
''            lineWidth = value
''        End Set
''    End Property

''    Public Overrides Property AccessRotation() As Integer
''        Get
''            Return rotation
''        End Get
''        Set(value As Integer)
''            rotation = value
''        End Set
''    End Property

''    Public Property AccessCenterPoint() As Point
''        Get
''            Return centerPoint
''        End Get
''        Set(value As Point)
''            centerPoint = value
''        End Set
''    End Property

''    Public ReadOnly Property AccessRadius() As Double
''        Get
''            Return radius
''        End Get
''    End Property

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        g.DrawEllipse(pen, centerPoint.X - CInt(radius), centerPoint.Y - CInt(radius), CInt(radius) * 2, CInt(radius) * 2)
''    End Sub

''    Public Overloads Sub Draw(pen As Pen, g As Graphics, scale As Double)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        g.DrawEllipse(pen, CSng(centerPoint.X) * CSng(scale) - CSng(radius) * CSng(scale), CSng(centerPoint.Y) * CSng(scale) - CSng(radius) * CSng(scale), CSng(radius) * 2 * CSng(scale), CSng(radius) * 2 * CSng(scale))
''    End Sub

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        Dim center As Point = AccessCenterPoint
''        Dim rad As Integer = CInt(AccessRadius)

''        Dim check1y As Integer = center.Y - rad
''        Dim check2y As Integer = center.Y + rad
''        Dim check3x As Integer = center.X + rad
''        Dim check4x As Integer = center.X - rad

''        Dim result As Double = (point.X - center.X) * (point.X - center.X) + (point.Y - center.Y) * (point.Y - center.Y) - radius * radius

''        If result < 0 Then
''            'pen.Color = Color.Red;


''            'g.DrawEllipse(pen, centerPoint.X - (int) radius, centerPoint.Y - (int)radius, (int)radius*2, (int)radius*2);

''            Return True
''        End If



''        Return False
''    End Function

''    Public Overloads Function Highlight(pen As Pen, g As Graphics, point As Point, scale As Double) As Boolean
''        Dim center As Point = AccessCenterPoint
''        Dim rad As Integer = CInt(AccessRadius)

''        Dim check1y As Integer = center.Y - rad
''        Dim check2y As Integer = center.Y + rad
''        Dim check3x As Integer = center.X + rad
''        Dim check4x As Integer = center.X - rad

''        Dim result As Double = (point.X - center.X * CSng(scale)) * (point.X - center.X * CSng(scale)) + (point.Y - center.Y * CSng(scale)) * (point.Y - center.Y * CSng(scale)) - radius * radius * CSng(scale) * CSng(scale)

''        If result < 0 Then
''            'pen.Color = Color.Red;


''            'g.DrawEllipse(pen, (float)centerPoint.X*(float)scale - (float) radius*(float)scale, (float)centerPoint.Y*(float)scale - (float)radius*(float)scale, (float)radius*2*(float)scale, (float)radius*2*(float)scale);

''            Return True
''        End If



''        Return False
''    End Function

''End Class
''#End Region

''#Region "Freehand Class - Not Completed Yet"
''Public Class FreehandTool
''    Inherits mShape
''    Private linePoint As ArrayList

''    Public Sub New(points As ArrayList, color As Color, w As Integer)

''        contourColor = color
''        lineWidth = w
''        shapeIdentifier = 4
''        rotation = 0

''        linePoint = points
''    End Sub

''    Public Overrides Property AccessContourColor() As Color
''        Get
''            Return contourColor
''        End Get
''        Set(value As Color)
''            contourColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessFillColor() As Color
''        Get
''            Return fillColor
''        End Get
''        Set(value As Color)
''            fillColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessLineWidth() As Integer
''        Get
''            Return lineWidth
''        End Get
''        Set(value As Integer)
''            lineWidth = value
''        End Set
''    End Property

''    Public Overrides Property AccessRotation() As Integer
''        Get
''            Return rotation
''        End Get
''        Set(value As Integer)
''            rotation = value
''        End Set
''    End Property

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)

''    End Sub

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        Return False
''    End Function


''End Class
''#End Region

''#Region "Polyline Class"

''Public Class mPolyline
''    Inherits mShape
''    Private listOfLines As ArrayList

''    Public Sub New(color As Color, w As Integer)
''        listOfLines = New ArrayList()

''        contourColor = color
''        lineWidth = w
''    End Sub

''    Public Overrides Property AccessContourColor() As Color
''        Get
''            Return contourColor
''        End Get
''        Set(value As Color)
''            contourColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessFillColor() As Color
''        Get
''            Return fillColor
''        End Get
''        Set(value As Color)
''            fillColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessLineWidth() As Integer
''        Get
''            Return lineWidth
''        End Get
''        Set(value As Integer)
''            lineWidth = value
''        End Set
''    End Property

''    Public Overrides Property AccessRotation() As Integer
''        Get
''            Return rotation
''        End Get
''        Set(value As Integer)
''            rotation = value
''        End Set
''    End Property

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        For Each obj As mLine In listOfLines
''            obj.Draw(pen, g)
''        Next

''    End Sub

''    Public Overloads Sub Draw(pen As Pen, g As Graphics, scale As Double)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        For Each obj As mLine In listOfLines
''            obj.Draw(pen, g, scale)
''        Next

''    End Sub

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        For Each obj As mLine In listOfLines
''            'obj.Draw(pen, g);

''            If obj.Highlight(pen, g, point) Then
''                'pen.Color = Color.Red;
''                'Draw(pen, g);
''                Return True
''            End If
''        Next



''        Return False
''    End Function

''    Public Overloads Function Highlight(pen As Pen, g As Graphics, point As Point, scale As Double) As Boolean
''        For Each obj As mLine In listOfLines
''            'obj.Draw(pen, g);

''            If obj.Highlight(pen, g, point, scale) Then
''                'pen.Color = Color.Red;
''                'Draw(pen, g, scale);
''                Return True
''            End If
''        Next



''        Return False
''    End Function

''    Public Sub AppendLine(theLine As mLine)
''        listOfLines.Add(theLine)
''    End Sub

''End Class

''#End Region

''#Region "Arc Class"

''Public Class mArc
''    Inherits mShape
''    Private centerPoint As Point
''    Private radius As Double

''    Private startAngle As Double
''    Private sweepAngle As Double

''    Public Sub New(center As Point, r As Double, startangle__1 As Double, sweepangle__2 As Double, color1 As Color, color2 As Color, _
''        w As Integer)
''        centerPoint = center
''        radius = r
''        startAngle = startangle__1
''        sweepAngle = sweepangle__2
''        contourColor = color1
''        fillColor = color2
''        lineWidth = w
''        shapeIdentifier = 3
''        rotation = 0
''    End Sub

''    Public ReadOnly Property AccessStartAngle() As Double
''        Get
''            Return startAngle
''        End Get
''    End Property

''    Public ReadOnly Property AccessSweepAngle() As Double
''        Get
''            Return sweepAngle
''        End Get
''    End Property

''    Public Overrides Property AccessContourColor() As Color
''        Get
''            Return contourColor
''        End Get
''        Set(value As Color)
''            contourColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessFillColor() As Color
''        Get
''            Return fillColor
''        End Get
''        Set(value As Color)
''            fillColor = value
''        End Set
''    End Property

''    Public Overrides Property AccessLineWidth() As Integer
''        Get
''            Return lineWidth
''        End Get
''        Set(value As Integer)
''            lineWidth = value
''        End Set
''    End Property

''    Public Overrides Property AccessRotation() As Integer
''        Get
''            Return rotation
''        End Get
''        Set(value As Integer)
''            rotation = value
''        End Set
''    End Property

''    Public Property AccessCenterPoint() As Point
''        Get
''            Return centerPoint
''        End Get
''        Set(value As Point)
''            centerPoint = value
''        End Set
''    End Property

''    Public ReadOnly Property AccessRadius() As Double
''        Get
''            Return radius
''        End Get
''    End Property

''    Public Overrides Sub Draw(pen As Pen, g As Graphics)
''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        g.DrawArc(pen, CSng(centerPoint.X) - CSng(radius), CSng(centerPoint.Y) - CSng(radius), CSng(radius) * 2, CSng(radius) * 2, -CSng(startAngle), _
''            -360 + CSng(startAngle) - CSng(sweepAngle))
''    End Sub

''    Public Overloads Sub Draw(pen As Pen, g As Graphics, scale As Double)
''        'g.DrawEllipse(pen, (float) centerPoint.X* (float)scale - (float) radius* (float)scale, (float)centerPoint.Y * (float)scale - (float)radius* (float)scale, (float)radius*2* (float)scale, (float)radius*2* (float)scale);

''        If highlighted Then
''            pen.Color = Color.Red
''            highlighted = False
''        End If

''        Dim tempAngle As Single = 0




''        If sweepAngle < startAngle Then

''            tempAngle = -360 + CSng(startAngle) - CSng(sweepAngle)
''        Else
''            'else if (startAngle > 180 && sweepAngle > 180)
''            '				{
''            '					tempAngle = startAngle - sweepAngle;
''            '				
''            '				
''            '				}

''            tempAngle = CSng(startAngle) - CSng(sweepAngle)
''        End If



''        g.DrawArc(pen, CSng(centerPoint.X) * CSng(scale) - CSng(radius) * CSng(scale), CSng(centerPoint.Y) * CSng(scale) - CSng(radius) * CSng(scale), CSng(radius) * 2 * CSng(scale), CSng(radius) * 2 * CSng(scale), -CSng(startAngle), _
''            tempAngle)
''    End Sub

''    Public Overrides Function Highlight(pen As Pen, g As Graphics, point As Point) As Boolean
''        Dim center As Point = AccessCenterPoint
''        Dim rad As Integer = CInt(AccessRadius)

''        Dim check1y As Integer = center.Y - rad
''        Dim check2y As Integer = center.Y + rad
''        Dim check3x As Integer = center.X + rad
''        Dim check4x As Integer = center.X - rad

''        Dim result As Double = (point.X - center.X) * (point.X - center.X) + (point.Y - center.Y) * (point.Y - center.Y) - radius * radius

''        If result < 0 Then
''            'pen.Color = Color.Yellow;


''            'g.DrawEllipse(pen, centerPoint.X - (int) radius, centerPoint.Y - (int)radius, (int)radius*2, (int)radius*2);

''            Return True
''        End If

''        Return False
''    End Function

''    Public Overloads Function Highlight(pen As Pen, g As Graphics, point As Point, scale As Double) As Boolean
''        Dim center As Point = AccessCenterPoint
''        Dim rad As Integer = CInt(AccessRadius)

''        Dim check1y As Integer = center.Y - rad
''        Dim check2y As Integer = center.Y + rad
''        Dim check3x As Integer = center.X + rad
''        Dim check4x As Integer = center.X - rad

''        Dim result As Double = (point.X - center.X * CSng(scale)) * (point.X - center.X * CSng(scale)) + (point.Y - center.Y * CSng(scale)) * (point.Y - center.Y * CSng(scale)) - radius * radius * CSng(scale) * CSng(scale)

''        If result < 0 Then
''            'pen.Color = Color.Yellow;


''            Dim tempAngle As Single = 0
''            If sweepAngle < startAngle Then
''                tempAngle = -360 + CSng(startAngle) - CSng(sweepAngle)
''            Else
''                tempAngle = CSng(startAngle) - CSng(sweepAngle)
''            End If


''            'g.DrawArc(pen, (float)centerPoint.X*(float)scale - (float) radius*(float)scale, (float)centerPoint.Y*(float)scale - (float)radius*(float)scale, (float)radius*2*(float)scale, (float)radius*2*(float)scale, -startAngle, tempAngle);

''            Return True
''        End If

''        Return False
''    End Function

''End Class

''#End Region
