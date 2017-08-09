Imports System.Drawing

Module Module1

    Sub Main()


        Dim sqA = GetDis2Pt(True, New PointF(5, 5), New PointF(29, 2))
        Dim sqB = GetDis2Pt(True, New PointF(25, 30), New PointF(29, 2))
        Dim sqC = GetDis2Pt(True, New PointF(25, 30), New PointF(5, 5))
        '
        Dim S = Math.Sqrt((sqA + sqB + sqC) ^ 2 - 2 * (sqA ^ 2 + sqB ^ 2 + sqC ^ 2)) / 4
        Dim dis = Math.Sqrt((sqA + sqB + sqC) ^ 2 - 2 * (sqA ^ 2 + sqB ^ 2 + sqC ^ 2)) / 2 / Math.Sqrt(sqA)

    End Sub

    Public Function GetDis2Pt(isSquareLen As Boolean, P1 As PointF, P2 As PointF) As Double
        If isSquareLen Then
            Return (P2.X - P1.X) ^ 2 + (P2.Y - P1.Y) ^ 2
        Else
            Return Math.Sqrt((P2.X - P1.X) ^ 2 + (P2.Y - P1.Y) ^ 2)
        End If
    End Function

End Module
