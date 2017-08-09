Imports System.Drawing

Module Module1


    Public Function GetClosedDistance(ckLineNo As Integer, Lines As List(Of mEnt)) As Double
        Dim ret As Double = 0
        Dim dLeft, dRight, dTop, dBot As Double
        '
        Dim Xc = Lines(ckLineNo).specLine.Xcp
        Dim Yc = Lines(ckLineNo).specLine.Ycp

        For i = 0 To Lines.Count - 1
            If i = ckLineNo Then Continue For
            '
            If Lines(i).specLine.isVer Then
                'Vertical line
                If Lines(ckLineNo).specLine.isVer Then
                    Continue For
                ElseIf Lines(ckLineNo).specLine.A = 0 Then

                End If
            ElseIf Lines(i).specLine.A = 0 Then
                'Horizontal line

            Else
                '
                Dim dX, dY As Double
                '
                If Xc < Lines(i).specLine.Xmin AndAlso (Yc < Lines(i).specLine.Ymin Or Yc > Lines(i).specLine.Ymax) Then Continue For
                If Xc > Lines(i).specLine.Xmax AndAlso (Yc < Lines(i).specLine.Ymin Or Yc > Lines(i).specLine.Ymax) Then Continue For
                '
                If Xc < Lines(i).specLine.Xmin AndAlso Xc > Lines(i).specLine.Xmax Then
                    ''truc // OX
                    dX = (Yc - Lines(i).specLine.B) / Lines(i).specLine.A
                    dX = (Yc - Lines(i).specLine.B) / Lines(i).specLine.A - Xc
                    dY = 0
                ElseIf Yc < Lines(i).specLine.Ymin AndAlso Yc > Lines(i).specLine.Ymax Then
                    ''truc // OY
                    dX = 0
                    dY = (Lines(i).specLine.A * Xc + Lines(i).specLine.B)
                    dY = (Lines(i).specLine.A * Xc + Lines(i).specLine.B) - Yc
                Else
                    ''cat ca 2 truc
                    dX = (Yc - Lines(i).specLine.B) / Lines(i).specLine.A
                    dX = (Yc - Lines(i).specLine.B) / Lines(i).specLine.A - Xc
                    '
                    dY = Lines(i).specLine.A * Xc + Lines(i).specLine.B
                    dY = Lines(i).specLine.A * Xc + Lines(i).specLine.B - Yc
                End If
                '
                If dX > 0 Then
                    dRight = Math.Min(dRight, dX)
                Else
                    dLeft = Math.Min(dLeft, dX)
                End If
                If dY > 0 Then
                    dTop = Math.Min(dTop, dY)
                Else
                    dBot = Math.Min(dBot, dY)
                End If
            End If
        Next
        '
        If dTop = 0 Then
            Dim isTopChord = True
        End If
        '
        Return ret
    End Function

    Public Function isValidSize(Len As Double)
        Select Case Len
            Case 1.5 : Return True
            Case 2.5 : Return True
            Case 3.5 : Return True
            Case 5.5 : Return True
            Case 7.25 : Return True
            Case 9.25 : Return True
            Case 11.25 : Return True
        End Select
        '
        Return False
    End Function

    Public Function GetDis2Pt(isSquareLen As Boolean, P1 As PointF, P2 As PointF) As Double
        If isSquareLen Then
            Return (P2.X - P1.X) ^ 2 + (P2.Y - P1.Y) ^ 2
        Else
            Return Math.Sqrt((P2.X - P1.X) ^ 2 + (P2.Y - P1.Y) ^ 2)
        End If
    End Function
End Module
