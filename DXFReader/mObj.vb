Imports System.Drawing

Public Class mLine
    Public PtStart As PointF
    Public PtEnd As PointF
    Public A As Double
    Public B As Double
    Public Xcp As Double 'Xcompare -centerline
    Public Ycp As Double 'Ycompare -centerline
    Public Len As Double
    Public isVer As Boolean
    Public Xmin As Double
    Public Ymin As Double
    Public Xmax As Double
    Public Ymax As Double
    '
    Sub New()
    End Sub

    Sub New(_sPoint As PointF, _ePoint As PointF)
        PtStart = _sPoint
        PtEnd = _ePoint
        If (_ePoint.X - _sPoint.X) = 0 Then
            A = 0
            isVer = True
        Else
            A = (_ePoint.Y - _sPoint.Y) / (_ePoint.X - _sPoint.X)
        End If

        B = _ePoint.Y - A * _ePoint.X
        Len = Math.Sqrt((_ePoint.Y - _sPoint.Y) ^ 2 + (_ePoint.X - _sPoint.X) ^ 2)
        Xcp = (_ePoint.X + _sPoint.X) / 2
        Ycp = (_ePoint.Y + _sPoint.Y) / 2
        Xmin = Math.Min(_ePoint.X, _sPoint.X)
        ymax = Math.Max(_ePoint.X, _sPoint.X)
        Ymin = Math.Min(_ePoint.Y, _sPoint.Y)
        Ymax = Math.Max(_ePoint.Y, _sPoint.Y)
    End Sub
End Class

Public Class mEnt
    Public mLayer As String
    Public mPoints As List(Of PointF)
    Public nType As ObjType
    Public specLine As mLine
    Public isClosed As Boolean
    Public Size As Double
    '
    Sub New()
    End Sub

    Sub New(_nType As ObjType, _mLayer As String, _mPoints As List(Of PointF))
        nType = _nType
        mLayer = _mLayer
        mPoints = _mPoints
        '
        If mPoints.Count = 2 Then
            specLine = New mLine(mPoints(0), mPoints(1))
        ElseIf mPoints.Count > 2 Then
            Dim maxNo As Integer
            Dim maxLen, minLen, tmpL As Double
            minLen = 9999
            '
            Dim stl As New List(Of PLData)
            For i = 0 To mPoints.Count - 2
                tmpL = (Math.Sqrt((mPoints(i + 1).Y - mPoints(i).Y) ^ 2 + (mPoints(i + 1).X - mPoints(i).X) ^ 2))
                stl.Add(New PLData(i, tmpL))
            Next
            '
            stl = stl.OrderByDescending(Function(x) x.Len).ToList()
            '
            Dim cPt As New PointF((mPoints(stl(1).No).X + mPoints(stl(1).No + 1).X) / 2, (mPoints(stl(1).No).Y + mPoints(stl(1).No + 1).Y) / 2)

            Dim sqA = GetDis2Pt(True, cPt, mPoints(stl(0).No))
            Dim sqB = GetDis2Pt(True, cPt, mPoints(stl(0).No + 1))
            Dim sqC = GetDis2Pt(True, mPoints(stl(0).No), mPoints(stl(0).No + 1))
            '
            Size = Math.Round(Math.Sqrt((sqA + sqB + sqC) ^ 2 - 2 * (sqA ^ 2 + sqB ^ 2 + sqC ^ 2)) / 2 / Math.Sqrt(sqC), 2)
            '
            specLine = New mLine(mPoints(maxNo), mPoints(maxNo + 1))
        End If
    End Sub

    Public Sub Draw(pen As Pen, g As Graphics, scale As Double, Xofset As Double, Yoffset As Double)
        If mPoints.Count < 2 Then

        ElseIf mPoints.Count = 2 Then
            g.DrawLine(pen, CSng(mPoints(0).X - Xofset) * CSng(scale), -CSng(mPoints(0).Y - Yoffset) * CSng(scale),
                       CSng(mPoints(1).X - Xofset) * CSng(scale), -CSng(mPoints(1).Y - Yoffset) * CSng(scale))
        Else
            For i = 0 To mPoints.Count - 2
                g.DrawLine(pen, CSng(mPoints(i).X - Xofset) * CSng(scale), -CSng(mPoints(i).Y - Yoffset) * CSng(scale),
                           CSng(mPoints(i + 1).X - Xofset) * CSng(scale), -CSng(mPoints(i + 1).Y - Yoffset) * CSng(scale))
            Next
        End If
    End Sub
End Class

Public Enum ObjType
    Other
    Line
    Polyline
    Rect
End Enum

Public Class PLData
    Public No As Integer
    Public Len As Double
    Sub New()
    End Sub
    Sub New(_No As Integer, _Len As Double)
        No = _No
        Len = _Len
    End Sub
End Class