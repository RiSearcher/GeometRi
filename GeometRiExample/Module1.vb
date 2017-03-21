Imports System.Math
Imports GeometRi

Module Module1



    Sub Main()



        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("!!! Find intersection of plane with line !!!")
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)
        Dim s1 As Plane3d = New Plane3d(-2, 2, 3, -29)
        Dim obj As Object = l1.IntersectionWith(s1)
        If obj IsNot Nothing Then
            If obj.GetType Is GetType(Line3d) Then
                Console.WriteLine("Object is Line3d")
                Dim l2 As Line3d = CType(obj, Line3d)
                Console.WriteLine(l2.ToString)
            ElseIf obj.GetType Is GetType(Point3d) Then
                Console.WriteLine("Object is Point3d")
                Dim p2 As Point3d = CType(obj, Point3d)
                Console.WriteLine(p2.ToString)
            End If
        End If


        Console.ReadLine()


    End Sub




End Module
