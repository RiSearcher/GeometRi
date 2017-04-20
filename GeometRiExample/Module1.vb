Imports System.Math
Imports GeometRi

Module Module1



    Sub Main()

        ' Examples of basic operations with GeometRi

        ' Global coordinate system is created automatically and can be accessed as "Coord3d.GlobalCS"
        Console.WriteLine("Number of defined coordinate systems: {0}", Coord3d.Counts)
        Console.WriteLine()
        Console.WriteLine("Default coordinate system: ")
        Console.WriteLine(Coord3d.GlobalCS.ToString)


        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("!!! Find intersection of plane with line !!!")

        ' Define point and vector in global CS
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)

        ' Define line using point and vector
        Dim l1 As Line3d = New Line3d(p1, v1)

        ' Define plane using general equation in 3D space in the form "A*x+B*y+C*z+D=0"
        Dim s1 As Plane3d = New Plane3d(-2, 2, 3, -29)

        ' Find the intersection of line with plane.
        ' The results could be point, line or nothing, therefore get result as general object
        ' and determine it's type.
        Dim obj As Object = l1.IntersectionWith(s1)
        If obj IsNot Nothing Then
            If obj.GetType Is GetType(Line3d) Then
                Console.WriteLine("Intersection is line")
                Dim l2 As Line3d = CType(obj, Line3d)
                Console.WriteLine(l2.ToString)
            ElseIf obj.GetType Is GetType(Point3d) Then
                Console.WriteLine("Intersection is point")
                Dim p2 As Point3d = CType(obj, Point3d)
                Console.WriteLine(p2.ToString)
            End If
        End If

        ' Short variant
        ' Will cause "InvalidCastException" if the intersection is not a point
        Dim p3 As Point3d = CType(l1.IntersectionWith(s1), Point3d)
        Console.WriteLine(p3.ToString())


        Console.ReadLine()


    End Sub




End Module
