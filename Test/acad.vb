Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry

Public Class acad

    Public Sub AddHatch()
        '' Get the current document and database
        Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
        Dim acCurDb As Database = acDoc.Database

        '' Start a transaction
        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()

            '' Open the Block table for read
            Dim acBlkTbl As BlockTable
            acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead)

            '' Open the Block table record Model space for write
            Dim acBlkTblRec As BlockTableRecord
            acBlkTblRec = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

            '' Create a circle object for the closed boundary to hatch
            Using acCirc As Circle = New Circle()
                acCirc.Center = New Point3d(3, 3, 0)
                acCirc.Radius = 1

                '' Add the new circle object to the block table record and the transaction
                acBlkTblRec.AppendEntity(acCirc)
                acTrans.AddNewlyCreatedDBObject(acCirc, True)

                '' Adds the circle to an object id array
                Dim acObjIdColl As ObjectIdCollection = New ObjectIdCollection()
                acObjIdColl.Add(acCirc.ObjectId)

                '' Create the hatch object and append it to the block table record
                Using acHatch As Hatch = New Hatch()
                    acBlkTblRec.AppendEntity(acHatch)
                    acTrans.AddNewlyCreatedDBObject(acHatch, True)
                    '' Set the properties of the hatch object
                    '' Associative must be set after the hatch object is appended to the 
                    '' block table record and before AppendLoop
                    acHatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31")
                    acHatch.Associative = True
                    acHatch.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl)
                    acHatch.EvaluateHatch(True)
                End Using
            End Using

            '' Save the new object to the database
            acTrans.Commit()
        End Using
    End Sub



End Class
