Imports System.Text
Imports System.Math
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class GeometRiTest


    '===============================================================
    ' Point3d tests
    '===============================================================

    <TestMethod()> Public Sub PointAddDivideTest()
        Dim p1 As Point3d = New Point3d(1, 2, 3)
        Dim p2 As Point3d = New Point3d(3, 2, 1)
        Assert.IsTrue((p1 + p2) / 2 = New Point3d(2, 2, 2))
    End Sub

    <TestMethod()> Public Sub PointScaleTest()
        Dim p1 As Point3d = New Point3d(1, 2, 3)
        Dim p2 As Point3d = New Point3d(2, 4, 6)
        Assert.IsTrue(2 * p1 = p2)
    End Sub

    <TestMethod()> Public Sub PointDistanceToPointTest()
        Dim p1 As Point3d = New Point3d(0, 0, 0)
        Dim p2 As Point3d = New Point3d(0, 3, 4)
        Assert.IsTrue(Abs(p1.DistanceTo(p2) - 5) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PointDistanceToLineTest()
        Dim p1 As Point3d = New Point3d(-4, 3, 5)
        Dim p2 As Point3d = New Point3d(1, -5, -1)
        Dim v2 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p2, v2)
        Assert.IsTrue(Abs(p1.DistanceTo(l1) - 3) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PointDistanceToPlaneTest()
        Dim p1 As Point3d = New Point3d(-4, 3, 5)
        Dim s1 As Plane3d = New Plane3d(-1, 2, -2, 9)
        Assert.IsTrue(Abs(p1.DistanceTo(s1) - 3) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PointDistanceToRayTest()
        Dim p As Point3d = New Point3d(0, 0, 0)
        Dim r As Ray3d = New Ray3d(New Point3d(1, 1, 0), New Vector3d(1, 0, 0))
        Assert.IsTrue(Abs(p.DistanceTo(r) - Sqrt(2)) < GeometRi3D.Tolerance)

        p = New Point3d(2, 0, 0)
        Assert.IsTrue(Abs(p.DistanceTo(r) - 1) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PointDistanceToSegmentTest()
        Dim p As Point3d = New Point3d(0, 0, 0)
        Dim s As Segment3d = New Segment3d(New Point3d(1, 1, 0), New Point3d(3, 3, 0))
        Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance)

        p = New Point3d(1, 1, 0)
        Assert.IsTrue(Abs(p.DistanceTo(s) - 0) < GeometRi3D.Tolerance)
        p = New Point3d(4, 4, 0)
        Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance)
        p = New Point3d(1, 3, 0)
        Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PointProjectionToPlaneTest()
        Dim p1 As Point3d = New Point3d(-4, 3, 5)
        Dim s1 As Plane3d = New Plane3d(-1, 2, -2, 9)
        Assert.IsTrue(p1.ProjectionTo(s1) = New Point3d(-3, 1, 7))
    End Sub

    <TestMethod()> Public Sub PointProjectionToLineTest()
        Dim p1 As Point3d = New Point3d(-4, 3, 5)
        Dim p2 As Point3d = New Point3d(1, -5, -1)
        Dim v2 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p2, v2)
        Assert.IsTrue(p1.ProjectionTo(l1) = New Point3d(-3, 1, 7))
    End Sub

    <TestMethod()> Public Sub PointProjectionToSphereTest()
        Dim p1 As Point3d = New Point3d(1, 1, 1)
        Dim s As Sphere = New Sphere(p1, 2)
        Dim p2 As Point3d = New Point3d(5, 5, 5)

        Assert.IsTrue(p2.ProjectionTo(s) = New Point3d(1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3)))
    End Sub

    <TestMethod()> Public Sub PointBelongsToLineTest()
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)
        Dim p2 As Point3d = p1.Translate(3 * v1)
        Assert.IsTrue(p2.BelongsTo(l1))
    End Sub

    <TestMethod()> Public Sub PointBelongsToPlaneTest()
        Dim s1 As Plane3d = New Plane3d(-1, 2, -2, 9)
        Dim p1 As Point3d = s1.Point
        Assert.IsTrue(p1.BelongsTo(s1))
    End Sub

    <TestMethod()> Public Sub PointBelongsToRayTest()
        Dim p As Point3d = New Point3d(0, 0, 0)
        Dim r As Ray3d = New Ray3d(New Point3d(1, 1, 0), New Vector3d(1, 0, 0))
        Assert.IsFalse(p.BelongsTo(r))

        p = New Point3d(1, 1, 0)
        Assert.IsTrue(p.BelongsTo(r))

        p = New Point3d(3, 1, 0)
        Assert.IsTrue(p.BelongsTo(r))
    End Sub

    <TestMethod()> Public Sub PointBelongsToSegmentTest()
        Dim p As Point3d = New Point3d(0, 0, 0)
        Dim s As Segment3d = New Segment3d(New Point3d(1, 1, 0), New Point3d(3, 3, 0))
        Assert.IsFalse(p.BelongsTo(s))

        p = New Point3d(1, 1, 0)
        Assert.IsTrue(p.BelongsTo(s))
        p = New Point3d(2, 2, 0)
        Assert.IsTrue(p.BelongsTo(s))
        p = New Point3d(3, 3, 0)
        Assert.IsTrue(p.BelongsTo(s))
    End Sub

    '===============================================================
    ' Line3d tests
    '===============================================================

    <TestMethod()> Public Sub LineDistanceToCrossingLineTest()
        Dim p2 As Point3d = New Point3d(1, -5, -1)
        Dim v2 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p2, v2)
        p2 = New Point3d(-2, 1, 2)
        v2 = New Vector3d(-2, 2, 3)
        Dim l2 As Line3d = New Line3d(p2, v2)
        Dim zzz = l1.DistanceTo(l2)
        Assert.IsTrue(Abs(l1.DistanceTo(l2) - 3) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub LineDistanceToParallelLineTest()
        Dim p2 As Point3d = New Point3d(1, -5, -1)
        Dim v2 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p2, v2)
        p2 = New Point3d(-4, 3, 5)
        v2 = New Vector3d(4, -6, -8)
        Dim l2 As Line3d = New Line3d(p2, v2)
        Assert.IsTrue(Abs(l1.DistanceTo(l2) - 3) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub LinePerpendicularToLineTest()
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)

        Dim p2 As Point3d = New Point3d(-2, 1, 2)
        Dim v2 As Vector3d = New Vector3d(-2, 2, 3)
        Dim l2 As Line3d = New Line3d(p2, v2)

        Assert.IsTrue(l1.PerpendicularTo(l2) = New Point3d(-4, 3, 5))
    End Sub

    <TestMethod()> Public Sub LineIntersectionWithPlaneTest()
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)
        Dim s1 As Plane3d = New Plane3d(-2, 2, 3, -29)

        Assert.IsTrue(l1.IntersectionWith(s1) = New Point3d(-3, 1, 7))
    End Sub

    <TestMethod()> Public Sub LineIntersectionWithPlaneTest2()
        ' Parallel line
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)
        p1 = New Point3d(0, 2, 0)
        Dim p2 As Point3d = New Point3d(0, 0, 2)
        Dim l1 As Line3d = New Line3d(p1, New Vector3d(p1, p2))

        Assert.IsTrue(l1.IntersectionWith(s1) = Nothing)
    End Sub

    <TestMethod()> Public Sub LineIntersectionWithPlaneTest3()
        ' Line lies in the plane
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)
        p1 = New Point3d(0, 1, 0)
        Dim p2 As Point3d = New Point3d(0, 0, 1)
        Dim l1 As Line3d = New Line3d(p1, New Vector3d(p1, p2))

        Assert.IsTrue(l1.IntersectionWith(s1) = l1)
    End Sub

    <TestMethod()> Public Sub LineProjectionToPlaneTest()
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)
        Dim s1 As Plane3d = New Plane3d(-2, 2, 3, -29)

        Dim p2 As Point3d = l1.IntersectionWith(s1)
        Dim l2 As Line3d = l1.ProjectionTo(s1)

        Assert.IsTrue(p2.BelongsTo(l2))
    End Sub

    '===============================================================
    ' Ray3d tests
    '===============================================================

    <TestMethod()> Public Sub RayDistanceToCrossingLineTest()
        Dim l As Line3d = New Line3d(New Point3d(0, 0, 0), New Vector3d(1, 0, 0))
        Dim r As Ray3d = New Ray3d(New Point3d(2, -3, 3), New Vector3d(0, 1, 0))
        Assert.IsTrue(Abs(r.DistanceTo(l) - 3) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub RayDistanceToCrossingLineTest2()
        Dim l As Line3d = New Line3d(New Point3d(0, 0, 0), New Vector3d(1, 0, 0))
        Dim r As Ray3d = New Ray3d(New Point3d(2, 4, 3), New Vector3d(0, 1, 0))
        Assert.IsTrue(Abs(r.DistanceTo(l) - 5) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub RayIntersectionWithPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, 0)
        Dim r As Ray3d = New Ray3d(New Point3d(-1, -1, -1), New Vector3d(1, 1, 1))
        Assert.IsTrue(r.IntersectionWith(s) = New Point3d(0, 0, 0))
    End Sub

    <TestMethod()> Public Sub RayIntersectionWithPlaneTest2()
        Dim s As Plane3d = New Plane3d(0, 0, 1, 0)
        Dim r As Ray3d = New Ray3d(New Point3d(1, 1, 1), New Vector3d(1, 1, 1))
        Assert.IsTrue(r.IntersectionWith(s) Is Nothing)
    End Sub

    <TestMethod()> Public Sub RayProjectionToPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, 0)
        Dim r As Ray3d = New Ray3d(New Point3d(1, 1, 1), New Vector3d(1, 1, 1))
        Assert.IsTrue(r.ProjectionTo(s) = New Ray3d(New Point3d(1, 1, 0), New Vector3d(1, 1, 0)))
    End Sub

    <TestMethod()> Public Sub RayDistanceToRayTest()
        Dim r1 As Ray3d = New Ray3d(New Point3d(0, 0, 0), New Vector3d(1, 0, 0))
        Dim r2 As Ray3d = New Ray3d(New Point3d(5, 0, 5), New Vector3d(-1, 0, 0))
        Assert.IsTrue(Abs(r1.DistanceTo(r2) - 5) < GeometRi3D.Tolerance)

        r2 = New Ray3d(New Point3d(5, -5, 5), New Vector3d(0, 1, 0))
        Assert.IsTrue(Abs(r1.DistanceTo(r2) - 5) < GeometRi3D.Tolerance)

        r2 = New Ray3d(New Point3d(-5, 0, 5), New Vector3d(0, 1, 1))
        Assert.IsTrue(Abs(r1.DistanceTo(r2) - Sqrt(50)) < GeometRi3D.Tolerance)
    End Sub

    '===============================================================
    ' Segment3d tests
    '===============================================================

    <TestMethod()> Public Sub SegmentProjectionToLineTest()
        Dim l As Line3d = New Line3d(New Point3d(1, 1, 1), New Vector3d(0, 0, 1))
        Dim r As Segment3d = New Segment3d(New Point3d(-1, -3, -2), New Point3d(-5, 1, -3))
        Assert.IsTrue(r.ProjectionTo(l) = New Segment3d(New Point3d(1, 1, -2), New Point3d(1, 1, -3)))
    End Sub

    <TestMethod()> Public Sub SegmentProjectionToLineTest2()
        Dim l As Line3d = New Line3d(New Point3d(1, 1, 1), New Vector3d(0, 0, 1))
        Dim r As Segment3d = New Segment3d(New Point3d(-1, -3, -2), New Point3d(-5, 1, -2))
        Assert.IsTrue(r.ProjectionTo(l) = New Point3d(1, 1, -2))
    End Sub

    <TestMethod()> Public Sub SegmentProjectionToPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, -1)
        Dim r As Segment3d = New Segment3d(New Point3d(-1, -3, -2), New Point3d(-5, 1, -3))
        Assert.IsTrue(r.ProjectionTo(s) = New Segment3d(New Point3d(-1, -3, 1), New Point3d(-5, 1, 1)))
    End Sub

    <TestMethod()> Public Sub SegmentIntersectionWithPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, -1)
        Dim r As Segment3d = New Segment3d(New Point3d(-1, -3, -2), New Point3d(-5, 1, -3))
        Assert.IsTrue(r.IntersectionWith(s) Is Nothing)

        r = New Segment3d(New Point3d(-1, -3, 1), New Point3d(-5, 1, 6))
        Assert.IsTrue(CType(r.IntersectionWith(s), Point3d) = New Point3d(-1, -3, 1))

        r = New Segment3d(New Point3d(-1, -3, -5), New Point3d(-5, 1, 1))
        Assert.IsTrue(CType(r.IntersectionWith(s), Point3d) = New Point3d(-5, 1, 1))

        r = New Segment3d(New Point3d(-1, -3, 1), New Point3d(-5, 1, 1))
        Assert.IsTrue(CType(r.IntersectionWith(s), Segment3d) = r)
    End Sub

    <TestMethod()> Public Sub SegmentDistanceToPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, -1)
        Dim r As Segment3d = New Segment3d(New Point3d(-1, -3, -2), New Point3d(-5, 1, -3))
        Assert.IsTrue(Abs(r.DistanceTo(s) - 3) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-1, -3, 1), New Point3d(-5, 1, 6))
        Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-1, -3, -5), New Point3d(-5, 1, 1))
        Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-1, -3, 1), New Point3d(-5, 1, 1))
        Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub SegmentDistanceToLineTest()
        Dim l As Line3d = New Line3d(New Point3d(), New Vector3d(1, 0, 0))
        Dim r As Segment3d = New Segment3d(New Point3d(-2, -3, 1), New Point3d(-5, 6, 1))
        Assert.IsTrue(Abs(r.DistanceTo(l) - 1) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-2, 0, 1), New Point3d(-5, 6, 5))
        Assert.IsTrue(Abs(r.DistanceTo(l) - 1) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-5, -6, 0), New Point3d(-2, -2, 0))
        Assert.IsTrue(Abs(r.DistanceTo(l) - 2) < GeometRi3D.Tolerance)

        r = New Segment3d(New Point3d(-5, -6, 0), New Point3d(0, 0, 0))
        Assert.IsTrue(Abs(r.DistanceTo(l)) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub SegmentDistanceToSegmentTest()
        Dim s1 As Segment3d = New Segment3d(New Point3d(-5, 0, 0), New Point3d(5, 0, 0))
        Dim s2 As Segment3d = New Segment3d(New Point3d(-2, -3, 1), New Point3d(5, 6, 1))
        Assert.IsTrue(Abs(s1.DistanceTo(s2) - 1) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(6, -3, 1), New Point3d(6, 6, 1))
        Assert.IsTrue(Abs(s1.DistanceTo(s2) - Sqrt(2)) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(2, 4, 0), New Point3d(6, 8, 0))
        Assert.IsTrue(Abs(s1.DistanceTo(s2) - 4) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(2, 4, 5), New Point3d(4, 0, 2))
        Assert.IsTrue(Abs(s1.DistanceTo(s2) - 2) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(-7, -6, 0), New Point3d(-5, -2, 0))
        Assert.IsTrue(Abs(s1.DistanceTo(s2) - 2) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub SegmentDistanceToRayTest()
        Dim s1 As Segment3d = New Segment3d(New Point3d(-5, 0, 0), New Point3d(5, 0, 0))
        Dim r As Ray3d = s1.ToRay

        Dim s2 As Segment3d = New Segment3d(New Point3d(-2, -3, 1), New Point3d(5, 6, 1))
        Assert.IsTrue(Abs(s2.DistanceTo(r) - 1) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(6, -3, 1), New Point3d(6, 6, 1))
        Assert.IsTrue(Abs(s2.DistanceTo(r) - 1) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(2, 4, 0), New Point3d(6, 8, 0))
        Assert.IsTrue(Abs(s2.DistanceTo(r) - 4) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(2, 4, 5), New Point3d(4, 0, 2))
        Assert.IsTrue(Abs(s2.DistanceTo(r) - 2) < GeometRi3D.Tolerance)

        s2 = New Segment3d(New Point3d(-7, -6, 0), New Point3d(-5, -2, 0))
        Assert.IsTrue(Abs(s2.DistanceTo(r) - 2) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub SegmentAngleToPlaneTest()
        Dim s As Plane3d = New Plane3d(0, 0, 1, -1)
        Dim r1 As Segment3d = New Segment3d(New Point3d(0, 0, 3), New Point3d(1, 0, 4))
        Dim r2 As Segment3d = New Segment3d(New Point3d(0, 0, -3), New Point3d(1, 0, -4))
        Assert.IsTrue(Abs(r1.AngleToDeg(s) - 45) < GeometRi3D.Tolerance AndAlso Abs(r1.AngleTo(s) - r2.AngleTo(s)) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub SegmentEqualsTest()
        Dim p1 As Point3d = New Point3d(1, 4, 6)
        Dim p2 As Point3d = New Point3d(8, -4, 0)
        Dim r1 As Segment3d = New Segment3d(p1, p2)
        Dim r2 As Segment3d = New Segment3d(p2, p1)
        Assert.IsTrue(r1 = r2)
    End Sub

    '===============================================================
    ' Plane3d tests
    '===============================================================

    <TestMethod()> Public Sub PlaneIntersectionWithLineTest()
        ' Inclined line
        Dim p1 As Point3d = New Point3d(1, -5, -1)
        Dim v1 As Vector3d = New Vector3d(-2, 3, 4)
        Dim l1 As Line3d = New Line3d(p1, v1)
        Dim s1 As Plane3d = New Plane3d(-2, 2, 3, -29)

        Assert.IsTrue(s1.IntersectionWith(l1) = New Point3d(-3, 1, 7))
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithLineTest2()
        ' Parallel line
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)
        p1 = New Point3d(0, 2, 0)
        Dim p2 As Point3d = New Point3d(0, 0, 2)
        Dim l1 As Line3d = New Line3d(p1, New Vector3d(p1, p2))

        Assert.IsTrue(s1.IntersectionWith(l1) = Nothing)
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithLineTest3()
        ' Line lies in the plane
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)
        p1 = New Point3d(0, 1, 0)
        Dim p2 As Point3d = New Point3d(0, 0, 1)
        Dim l1 As Line3d = New Line3d(p1, New Vector3d(p1, p2))

        Assert.IsTrue(s1.IntersectionWith(l1) = l1)
    End Sub

    '===============================================================
    ' Intersection of three planes
    '===============================================================

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest1()
        ' Three coplanar planes
        ' Planes do not coinside
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 1), New Vector3d(1, 1, 1))
        Dim s2 As Plane3d = New Plane3d(New Point3d(2, 0, 1), New Vector3d(1, 1, 1))
        Dim s3 As Plane3d = New Plane3d(New Point3d(0, 3, 1), New Vector3d(-1, -1, -1))

        Assert.IsTrue(s1.IntersectionWith(s2, s3) = Nothing)
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest2()
        ' Three coplanar planes
        ' Two planes are coinside
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 1), New Vector3d(1, 1, 1))
        Dim s2 As Plane3d = New Plane3d(New Point3d(2, 0, 1), New Vector3d(1, 1, 1))
        Dim s3 As Plane3d = New Plane3d(New Point3d(1, 0, 0), New Vector3d(-1, -1, -1))

        Assert.IsTrue(s1.IntersectionWith(s2, s3) = Nothing)
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest3()
        ' Three coplanar planes
        ' Three planes are coinside
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 1), New Vector3d(1, 1, 1))
        Dim s2 As Plane3d = New Plane3d(New Point3d(0, 1, 0), New Vector3d(1, 1, 1))
        Dim s3 As Plane3d = New Plane3d(New Point3d(1, 0, 0), New Vector3d(-1, -1, -1))

        Assert.IsTrue(s1.IntersectionWith(s2, s3) = s1)
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest4()
        ' Three vertical planes (relative to the XY-plane) with common line
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 0), New Vector3d(1, 1, 0))
        Dim s2 As Plane3d = New Plane3d(New Point3d(0, 0, 2), New Vector3d(4, 2, 0))
        Dim s3 As Plane3d = New Plane3d(New Point3d(0, 0, 4), New Vector3d(-1, 1, 0))

        Dim obj As Object = s1.IntersectionWith(s2, s3)

        If obj IsNot Nothing AndAlso obj.GetType Is GetType(Line3d) Then
            Dim l1 As Line3d = CType(obj, Line3d)
            Assert.IsTrue(l1 = New Line3d(New Point3d(0, 0, 0), New Vector3d(0, 0, 1)))
        Else
            Assert.Fail()
        End If
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest5()
        ' Three vertical planes (relative to the XY-plane) with NO common line
        Dim s1 As Plane3d = New Plane3d(New Point3d(1, 0, 0), New Vector3d(1, 1, 0))
        Dim s2 As Plane3d = New Plane3d(New Point3d(0, 2, 2), New Vector3d(4, 2, 0))
        Dim s3 As Plane3d = New Plane3d(New Point3d(3, 3, 4), New Vector3d(-1, 1, 0))

        Dim obj As Object = s1.IntersectionWith(s2, s3)

        Assert.IsTrue(obj = Nothing)
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest6()
        ' Three intersecting planes with common point
        Dim s1 As Plane3d = New Plane3d(New Point3d(1, 0, 0), New Vector3d(0, 1, 5))
        Dim s2 As Plane3d = New Plane3d(New Point3d(0, 2, 0), New Vector3d(4, 0, 0))
        Dim s3 As Plane3d = New Plane3d(New Point3d(0, 0, 0), New Vector3d(-1, 1, 3))

        Dim obj As Object = s1.IntersectionWith(s2, s3)

        If obj IsNot Nothing AndAlso obj.GetType Is GetType(Point3d) Then
            Dim p1 As Point3d = CType(obj, Point3d)
            Assert.IsTrue(p1 = New Point3d(0, 0, 0))
        Else
            Assert.Fail()
        End If
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithTwoPlanesTest()
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 1), New Vector3d(0, 1, 1))
        Dim s2 As Plane3d = New Plane3d(-5, 2, 4, 1)
        Dim s3 As Plane3d = New Plane3d(2, -3, 2, 4)
        Assert.IsTrue(s1.IntersectionWith(s2, s3) = s1.IntersectionWith(s2.IntersectionWith(s3)))
    End Sub

    '===============================================================

    <TestMethod()> Public Sub PlaneEqualsToPlaneTest()
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)

        Dim p2 As Point3d = New Point3d(0, 0, 1)
        Dim v2 As Vector3d = New Vector3d(-1, -1, -1)
        Dim s2 As Plane3d = New Plane3d(p2, v2)

        Assert.IsTrue(s1 = s2)
    End Sub

    <TestMethod()> Public Sub PlaneAngleToPlaneTest()
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(0, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)

        Dim m As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(0, -1, 1), 10 * PI / 180)
        v1 = m * v1
        Dim s2 As Plane3d = New Plane3d(p1, v1)

        Assert.IsTrue(Abs(s1.AngleToDeg(s2) - 10) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub PlaneAngleToPlaneTest2()
        Dim p1 As Point3d = New Point3d(1, 0, 0)
        Dim v1 As Vector3d = New Vector3d(0, 1, 1)
        Dim s1 As Plane3d = New Plane3d(p1, v1)

        Dim m As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(0, -1, 1), 10 * PI / 180)
        v1 = m * v1
        Dim s2 As Plane3d = New Plane3d(p1, -v1)

        Assert.IsTrue(Abs(s1.AngleToDeg(s2) - 10) < GeometRi3D.Tolerance)
    End Sub

    '===============================================================
    ' Vector3d tests
    '===============================================================

    <TestMethod()> Public Sub VectorAngleTest()
        ' Angle < 90 
        Dim v1 As Vector3d = New Vector3d(1, 0, 0)
        Dim v2 As Vector3d = New Vector3d(1, 1, 0)
        Assert.IsTrue(Abs(v1.AngleToDeg(v2) - 45) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub VectorAngleTest2()
        ' Angle > 90 
        Dim v1 As Vector3d = New Vector3d(1, 0, 0)
        Dim v2 As Vector3d = New Vector3d(-1, 1, 0)
        Assert.IsTrue(Abs(v1.AngleToDeg(v2) - 135) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub VectorAngleToPlaneTest()
        Dim s As New Plane3d() ' XY plane

        Dim v As Vector3d = New Vector3d(1, 0, 0)
        Assert.IsTrue(Abs(v.AngleTo(s)) < GeometRi3D.Tolerance)
        Assert.IsTrue(Abs(s.AngleTo(v)) < GeometRi3D.Tolerance)

        v = New Vector3d(0, 0, 1)
        Assert.IsTrue(Abs(v.AngleTo(s) - PI / 2) < GeometRi3D.Tolerance)
        Assert.IsTrue(Abs(s.AngleTo(v) - PI / 2) < GeometRi3D.Tolerance)

        v = New Vector3d(0, 0, -1)
        Assert.IsTrue(Abs(v.AngleTo(s) - PI / 2) < GeometRi3D.Tolerance)
        Assert.IsTrue(Abs(s.AngleTo(v) - PI / 2) < GeometRi3D.Tolerance)

        v = New Vector3d(1, 0, 1)
        Assert.IsTrue(Abs(v.AngleTo(s) - PI / 4) < GeometRi3D.Tolerance)
        Assert.IsTrue(Abs(s.AngleTo(v) - PI / 4) < GeometRi3D.Tolerance)

        v = New Vector3d(1, 0, -1)
        Assert.IsTrue(Abs(v.AngleTo(s) - PI / 4) < GeometRi3D.Tolerance)
        Assert.IsTrue(Abs(s.AngleTo(v) - PI / 4) < GeometRi3D.Tolerance)

    End Sub

    '===============================================================
    ' Matrix3d tests
    '===============================================================

    <TestMethod()> Public Sub MatrixDeterminantTest()
        Dim m As Matrix3d = New Matrix3d({1, 2, 3}, {0, -4, 1}, {0, 3, -1})
        Assert.IsTrue(Abs(m.Det - 1) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub MatrixInverseTest()
        Dim m As Matrix3d = New Matrix3d({1, 2, 3}, {0, -4, 1}, {0, 3, -1})
        Assert.IsTrue(m.Inverse * m = Matrix3d.Identity)
    End Sub

    <TestMethod()> Public Sub RotationMatrixOrthogonalityTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(1, 2, 3), PI / 2)
        Assert.IsTrue(r.Transpose * r = Matrix3d.Identity)
    End Sub

    <TestMethod()> Public Sub MatrixMaxNormTest()
        Dim m As Matrix3d = New Matrix3d({1, 2, 3}, {0, -4, 1}, {0, 3, -1})
        Assert.IsTrue(Abs(m.MaxNorm - 4) < GeometRi3D.Tolerance)
    End Sub

    '===============================================================
    ' Other tests
    '===============================================================

    <TestMethod()> Public Sub ToleranceTest()
        Dim s1 As Plane3d = New Plane3d(New Point3d(0, 0, 1), New Vector3d(0, 1, 1))
        Dim s2 As Plane3d = New Plane3d(-5, 2, 4, 1)
        Dim s3 As Plane3d = New Plane3d(2, -3, 2, 4)
        GeometRi3D.Tolerance = 0
        Assert.IsFalse(s1.IntersectionWith(s2, s3) = s1.IntersectionWith(s2.IntersectionWith(s3)))
        GeometRi3D.Tolerance = 0.000000000001
    End Sub

    <TestMethod()> Public Sub CoordSystemTest()
        Dim c1 As Coord3d = New Coord3d(New Point3d(), {2, 0, 0}, {1, 1, 0})
        Assert.IsTrue(c1.Axes = Matrix3d.Identity)

        c1 = New Coord3d(New Point3d(), New Vector3d(2, 0, 0), New Vector3d(0, 0, 5))
        c1.RotateDeg(New Vector3d(1, 0, 0), -90)
        Assert.IsTrue(c1.Axes = Matrix3d.Identity)
    End Sub

    '===============================================================
    ' Coordinate transformation tests
    '===============================================================

    <TestMethod()> Public Sub PointConvertToTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Assert.IsTrue(New Point3d(1, 2, 3, coord1) = New Point3d(2, 3, 1, coord2))
    End Sub

    <TestMethod()> Public Sub PointConvertToTest2()
        Dim coord1 As Coord3d = New Coord3d(New Point3d(2, 3, 1), Matrix3d.RotationMatrix(New Vector3d(2, 1, 5), PI / 3))
        Dim coord2 As Coord3d = New Coord3d(New Point3d(1, -3, 4), Matrix3d.RotationMatrix(New Vector3d(3, 2, 1), PI / 2))

        Dim p1 As Point3d = New Point3d(1, 2, -2, coord1)
        Dim p2 As Point3d = p1.ConvertTo(coord2)

        Assert.IsTrue(p2 = p1)
    End Sub

    <TestMethod()> Public Sub PointConvertToGlobalTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim p1 As Point3d = New Point3d(1, 2, 3, coord1)
        Dim p2 As Point3d = New Point3d(2, 3, 1, coord2)
        p1 = p1.ConvertToGlobal
        p2 = p2.ConvertToGlobal

        Assert.IsTrue(p1 = p2)
    End Sub

    <TestMethod()> Public Sub VectorConvertToTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Assert.IsTrue(New Vector3d(1, 2, 3, coord1) = New Vector3d(2, 3, 1, coord2))
    End Sub

    <TestMethod()> Public Sub VectorConvertToTest2()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim v1 As Vector3d = New Vector3d(2, 3, 4)
        Dim v2 As Vector3d = v1.ConvertTo(coord1)
        Dim v3 As Vector3d = v1.ConvertTo(coord2)

        Assert.IsTrue(v2 = v3)
    End Sub

    <TestMethod()> Public Sub LineConvertToTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim p1 As Point3d = New Point3d(1, 2, 3, coord1)
        Dim v1 As Vector3d = New Vector3d(0, 0, 1)
        Dim l1 As Line3d = New Line3d(p1, v1)
        l1.Point = l1.Point.ConvertTo(coord2)
        Dim s1 As Plane3d = coord2.XZ_plane
        s1.Point = s1.Point.ConvertTo(coord1)

        Assert.IsTrue(l1.IntersectionWith(s1) = New Point3d(1, 2, 0))
    End Sub

    <TestMethod()> Public Sub PlaneConvertToTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim p1 As Point3d = New Point3d(1, 2, 3, coord1)
        Dim v1 As Vector3d = New Vector3d(0, 0, 1)
        Dim l1 As Line3d = New Line3d(p1, v1)
        l1.Point = l1.Point.ConvertTo(coord2)
        Dim s1 As Plane3d = coord2.XZ_plane
        s1.Point = s1.Point.ConvertTo(coord1)

        Assert.IsTrue(s1.IntersectionWith(l1) = New Point3d(1, 2, 0))
    End Sub

    <TestMethod()> Public Sub LineConvertProjectionToPlaneTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim p1 As Point3d = New Point3d(1, 2, 3, coord1)
        Dim v1 As Vector3d = New Vector3d(0, 0, 1)
        Dim l1 As Line3d = New Line3d(p1, v1)
        l1.Point = l1.Point.ConvertTo(coord2)
        Dim s1 As Plane3d = coord2.XZ_plane
        s1.Point = s1.Point.ConvertTo(coord1)

        Assert.IsTrue(l1.ProjectionTo(s1) = New Point3d(1, 2, 0))
    End Sub

    <TestMethod()> Public Sub LineConvertProjectionToPlaneTest2()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)

        Dim p1 As Point3d = New Point3d(1, 2, 3, coord1)
        Dim v1 As Vector3d = New Vector3d(0, 2, 1)
        Dim l1 As Line3d = New Line3d(p1, v1)
        l1.Point = l1.Point.ConvertTo(coord2)
        Dim s1 As Plane3d = coord2.XZ_plane
        s1.Point = s1.Point.ConvertTo(coord1)

        Assert.IsTrue(l1.ProjectionTo(s1) = New Line3d(New Point3d(1, 2, 0), New Vector3d(0, 1, 0)))
    End Sub

    <TestMethod()> Public Sub PlaneConvertIntersectionToPlaneTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(1, 1, 1), 90)
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)


        Dim s1 As Plane3d = New Plane3d(1, 1, 1, 0, Coord3d.GlobalCS)
        Dim s2 As Plane3d = New Plane3d(1, 3, 6, 0, coord1)
        Dim s3 As Plane3d = New Plane3d(100, -1000, 1, 0, coord2)


        Assert.IsTrue(s1.IntersectionWith(s2, s3) = New Point3d(0, 0, 0))
    End Sub

    <TestMethod()> Public Sub PlaneConvertToGlobalTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(1, 1, 1), 90)
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)
        coord1.Origin = New Point3d(1, 1, 1)
        coord2.Origin = New Point3d(10, 2, 5)

        Dim s1 As Plane3d = New Plane3d(1, 1, 1, 0, Coord3d.GlobalCS)
        s1.Point = s1.Point.ConvertTo(coord1)
        s1.Point = s1.Point.ConvertTo(coord2)
        s1.Point = s1.Point.ConvertToGlobal

        Assert.IsTrue(s1 = New Plane3d(1, 1, 1, 0, Coord3d.GlobalCS))
    End Sub

    <TestMethod()> Public Sub PlaneIntersectionWithPlanesTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(1, 2, 3), 90)
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)
        coord1.Origin = New Point3d(1, 1, 1)
        coord2.Origin = New Point3d(10, 2, 5)

        Dim s1 As Plane3d = New Plane3d(1, 1, 1, 0, Coord3d.GlobalCS)
        Dim s2 As Plane3d = New Plane3d(3, -2, 0, 9, Coord3d.GlobalCS)
        Dim s3 As Plane3d = New Plane3d(2, 5, 1, -2, Coord3d.GlobalCS)
        Dim p1 As Point3d = s1.IntersectionWith(s2, s3)
        s1.Point = s1.Point.ConvertTo(coord2)
        s2.Normal = s2.Normal.ConvertTo(coord1).ConvertTo(coord2)
        s3.Point = s3.Point.ConvertTo(coord2).ConvertTo(coord1)
        Dim p2 As Point3d = s2.IntersectionWith(s1, s3)

        Assert.IsTrue(p1 = p2)
    End Sub

    '===============================================================
    ' Object translate tests
    '===============================================================

    <TestMethod()> Public Sub TranslatePointTest1()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(0, 0, 1), 90)
        coord1.Origin = New Point3d(1, 1, 1)

        Dim p1 As Point3d = New Point3d(1, 2, 3, Coord3d.GlobalCS)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1, coord1)

        Assert.IsTrue(p1.Translate(v1) = New Point3d(0, 3, 4))
    End Sub

    <TestMethod()> Public Sub TranslatePointTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(1, 2, 3), 90)
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)
        coord1.Origin = New Point3d(1, 1, 1)
        coord2.Origin = New Point3d(10, 2, 5)

        Dim p1 As Point3d = New Point3d(1, 2, 3, Coord3d.GlobalCS)
        Dim p2 As Point3d = New Point3d(10, -2, 6, coord1)
        Dim p3 As Point3d = New Point3d(-3, 5, 1, coord2)
        Dim v1 As Vector3d = New Vector3d(p1, p2)
        Dim v2 As Vector3d = New Vector3d(p2, p3)
        Dim v3 As Vector3d = New Vector3d(p3, p1)

        Dim p As Point3d = New Point3d(5, 6, 7)

        Assert.IsTrue(p.Translate(v1).Translate(v2).Translate(v3) = p)
    End Sub

    <TestMethod()> Public Sub TranslatePlaneTest()
        Dim coord1 As Coord3d = Coord3d.GlobalCS.Clone
        Dim coord2 As Coord3d = Coord3d.GlobalCS.Clone
        coord1.RotateDeg(New Vector3d(1, 2, 3), 90)
        coord2.RotateDeg(New Vector3d(1, 1, 1), 120)
        coord1.Origin = New Point3d(1, 1, 1)
        coord2.Origin = New Point3d(10, 2, 5)

        Dim p1 As Point3d = New Point3d(1, 2, 3, Coord3d.GlobalCS)
        Dim p2 As Point3d = New Point3d(10, -2, 6, coord1)
        Dim p3 As Point3d = New Point3d(-3, 5, 1, coord2)
        Dim v1 As Vector3d = New Vector3d(p1, p2)
        Dim v2 As Vector3d = New Vector3d(p2, p3)
        Dim v3 As Vector3d = New Vector3d(p3, p1)

        Dim s As Plane3d = New Plane3d(1, 2, 3, 4)

        Assert.IsTrue(s.Translate(v1).Translate(v2).Translate(v3) = s)
    End Sub

    '===============================================================
    ' Object rotation tests
    '===============================================================

    <TestMethod()> Public Sub PointRotationTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(0, 0, 1), PI / 2)
        Dim p As Point3d = New Point3d(1, 2, 0)

        Assert.IsTrue(p.Rotate(r) = New Point3d(-2, 1, 0))
    End Sub

    <TestMethod()> Public Sub PointRotationAroundPointTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(0, 0, 1), PI / 2)
        Dim p As Point3d = New Point3d(3, 3, 0)
        Dim pc As Point3d = New Point3d(2, 3, 5)

        Assert.IsTrue(p.Rotate(r, pc) = New Point3d(2, 4, 0))
    End Sub

    <TestMethod()> Public Sub PointRotationAroundPointTest2()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(1, 1, 1), 2 * PI / 3)
        Dim p As Point3d = New Point3d(5, 0, 2)
        Dim pc As Point3d = New Point3d(1, 1, 3)

        Assert.IsTrue(p.Rotate(r, pc) = New Point3d(0, 5, 2))
    End Sub

    <TestMethod()> Public Sub VectorRotationTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(1, 1, 1), 2 * PI / 3)
        Dim v1 As Vector3d = New Vector3d(1, 1, 1)
        Dim v2 As Vector3d = New Vector3d(1, 0, 0)

        Assert.IsTrue(v1.Rotate(r) = v1 AndAlso v2.Rotate(r) = New Vector3d(0, 1, 0))
    End Sub

    <TestMethod()> Public Sub LineRotationTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(1, 1, 1), 2 * PI / 3)
        Dim coord As Coord3d = New Coord3d(New Point3d(3, 2, 1), r)
        Dim p As Point3d = New Point3d(5, 0, 2, coord)
        Dim l As Line3d = New Line3d(New Point3d(4, 1, 2), New Vector3d(1, 2, 6))

        Assert.IsTrue(l.Rotate(r, p).Rotate(r, p).Rotate(r, p) = l)
    End Sub

    <TestMethod()> Public Sub PlaneRotationTest()
        Dim r As Matrix3d = Matrix3d.RotationMatrix(New Vector3d(1, 1, 1), 2 * PI / 3)
        Dim coord As Coord3d = New Coord3d(New Point3d(3, 2, 1), r)
        Dim p As Point3d = New Point3d(5, 0, 2, coord)
        Dim s As Plane3d = New Plane3d(1, 2, 3, 4)

        Assert.IsTrue(s.Rotate(r, p).Rotate(r, p).Rotate(r, p) = s)
    End Sub

    '===============================================================
    ' Object reflection tests
    '===============================================================

    <TestMethod()> Public Sub PointReflectInPointTest()
        Dim p1 As Point3d = New Point3d(2, 3, 4)
        Dim p2 As Point3d = New Point3d(1, 1, 1)

        Assert.IsTrue(p1.ReflectIn(p2) = New Point3d(0, -1, -2))
    End Sub

    <TestMethod()> Public Sub PointReflectInLineTest()
        Dim p1 As Point3d = New Point3d(2, 3, 4)
        Dim p2 As Point3d = New Point3d(1, 1, 1)
        Dim l As Line3d = New Line3d(p2, New Vector3d(0, 0, 1))

        Assert.IsTrue(p1.ReflectIn(l) = New Point3d(0, -1, 4))
    End Sub

    <TestMethod()> Public Sub PointReflectInPlaneTest()
        Dim p1 As Point3d = New Point3d(3, 3, 3)
        Dim s As Plane3d = New Plane3d(1, 1, 1, -3)

        Assert.IsTrue(p1.ReflectIn(s) = New Point3d(-1, -1, -1))
    End Sub

    <TestMethod()> Public Sub LineReflectInPointTest()
        Dim p1 As Point3d = New Point3d(1, 1, 1)
        Dim l1 As Line3d = New Line3d(New Point3d(2, 3, 0), New Vector3d(0, 0, 1))
        Dim l2 As Line3d = New Line3d(New Point3d(0, -1, 2), New Vector3d(0, 0, 1))

        Assert.IsTrue(l1.ReflectIn(p1) = l2)
    End Sub

    <TestMethod()> Public Sub PlaneReflectInPlaneTest()
        Dim coord1 As Coord3d = New Coord3d(New Point3d(2, 3, 1), Matrix3d.RotationMatrix(New Vector3d(2, 1, 5), PI / 3))
        Dim coord2 As Coord3d = New Coord3d(New Point3d(1, -3, 4), Matrix3d.RotationMatrix(New Vector3d(3, 2, 1), PI / 2))

        Dim s1 As Plane3d = New Plane3d(1, 2, -2, -3, coord1)
        Dim s2 As Plane3d = New Plane3d(2, -1, 3, -1, coord2)
        Dim s3 As Plane3d = s1.ReflectIn(s2)
        s3.Point = s3.Point.ConvertTo(coord2)
        Dim s4 As Plane3d = s3.ReflectIn(s2)

        Dim bol As Boolean = s4.Equals(s1)

        Assert.IsTrue(s4 = s1)
    End Sub

    <TestMethod()> Public Sub LineReflectInLineTest()
        Dim coord1 As Coord3d = New Coord3d(New Point3d(2, 3, 1), Matrix3d.RotationMatrix(New Vector3d(2, 1, 5), PI / 3))
        Dim coord2 As Coord3d = New Coord3d(New Point3d(1, -3, 4), Matrix3d.RotationMatrix(New Vector3d(3, 2, 1), PI / 2))

        Dim l1 As Line3d = New Line3d(New Point3d(2, 3, 4, coord1), New Vector3d(2, 1, -3))
        Dim l2 As Line3d = New Line3d(New Point3d(2, 3, 4, coord2), New Vector3d(2, 1, -3))
        Dim lt As Line3d = l1.ReflectIn(l2)
        lt.Point = lt.Point.ConvertTo(coord2)

        Assert.IsTrue(lt.ReflectIn(l2) = l1)
    End Sub

    '===============================================================
    ' Sphere tests
    '===============================================================

    <TestMethod()> Public Sub SphereEqualTest()
        Dim s1 As Sphere = New Sphere(New Point3d(0, 0, 0), 5)
        Dim s2 As Sphere = New Sphere(New Point3d(0, 0, 0), 6)
        Assert.IsTrue(s1 <> s2)

        s1 = New Sphere(New Point3d(0, 0, 0), 5)
        s2 = New Sphere(New Point3d(1, 0, 0), 5)
        Assert.IsTrue(s1 <> s2)

        s1 = New Sphere(New Point3d(0, 0, 0), 5)
        s2 = New Sphere(New Point3d(0, 0, 0), 5)
        Assert.IsTrue(s1 = s2)
    End Sub

    <TestMethod()> Public Sub SphereIntersectionWithLineTest()
        Dim l As Line3d = New Line3d(New Point3d(5, 0, -6), New Vector3d(1, 0, 0))
        Dim s As Sphere = New Sphere(New Point3d(0, 0, 0), 5)

        Assert.IsTrue(s.IntersectionWith(l) = Nothing)

        l.Point = New Point3d(5, 0, -5)
        Assert.IsTrue(s.IntersectionWith(l) = New Point3d(0, 0, -5))

        l.Point = New Point3d(0, 0, 0)
        l.Direction = New Vector3d(1, 0, 0)
        Assert.IsTrue(s.IntersectionWith(l) = New Segment3d(New Point3d(-5, 0, 0), New Point3d(5, 0, 0)))

        l.Direction = New Vector3d(1, 3, 4)
        Assert.IsTrue(s.IntersectionWith(l).Length = 10)
    End Sub

    <TestMethod()> Public Sub SphereIntersectionWithPlaneTest()
        Dim s As New Sphere(New Point3d(1, -1, 3), 3)
        Dim p As New Plane3d(1, 4, 5, 6)
        Dim c As Circle3d = s.IntersectionWith(p)
        Assert.IsTrue(Abs(c.R - 1.13) < 0.005)
        Assert.IsTrue(c.Center.DistanceTo(New Point3d(0.57, -2.71, 0.86)) < 0.01)
    End Sub

    <TestMethod()> Public Sub SphereIntersectionWithSphereTest()
        Dim s1 As Sphere = New Sphere(New Point3d(-2, 2, 4), 5)
        Dim s2 As Sphere = New Sphere(New Point3d(3, 7, 3), 5)
        Dim c1 As Circle3d = s1.IntersectionWith(s2)
        Assert.IsTrue(Abs(c1.R - 3.5) < GeometRi3D.Tolerance)
        Assert.IsTrue(c1.Center = New Point3d(0.5, 4.5, 3.5))

        Dim c2 As Circle3d = s2.IntersectionWith(s1)
        Assert.IsTrue(c1 = c2)
    End Sub

    <TestMethod()> Public Sub SphereProjectionToPlaneTest()
        Dim s As Sphere = New Sphere(New Point3d(-2, -2, -2), 5)
        Dim p As Plane3d = New Plane3d(New Point3d(1, 1, 1), New Vector3d(1, 1, 1))
        Dim c As Circle3d = s.ProjectionTo(p)
        Dim res As Circle3d = New Circle3d(New Point3d(1, 1, 1), 5, New Vector3d(-1, -1, -1))
        Assert.AreEqual(c, res)
    End Sub

    <TestMethod()> Public Sub SphereProjectionToLineTest()
        Dim s As Sphere = New Sphere(New Point3d(-4, -3, -2), 5)
        Dim l As Line3d = New Line3d(New Point3d(0, 0, 0), New Vector3d(4, 3, 0))
        Dim c As Segment3d = s.ProjectionTo(l)
        Dim res As Segment3d = New Segment3d(New Point3d(0, 0, 0), New Point3d(-8, -6, 0))
        Assert.AreEqual(c, res)
    End Sub

    '===============================================================
    ' Circle3d tests
    '===============================================================

    <TestMethod()> Public Sub CircleBy3PointsTest()
        Dim p1 As Point3d = New Point3d(-3, 0, 4)
        Dim p2 As Point3d = New Point3d(4, 0, 5)
        Dim p3 As Point3d = New Point3d(1, 0, -4)

        Dim c As Circle3d = New Circle3d(p1, p2, p3)

        Assert.IsTrue(c.Center = New Point3d(1, 0, 1))
        Assert.IsTrue(Abs(c.R - 5) <= GeometRi3D.Tolerance)
    End Sub

    '===============================================================
    ' Triangle tests
    '===============================================================

    <TestMethod()> Public Sub TriangleEqualTest()
        Dim p1 As Point3d = New Point3d(-3, 0, 4)
        Dim p2 As Point3d = New Point3d(4, 0, 5)
        Dim p3 As Point3d = New Point3d(1, 0, -4)

        Dim t1 = New Triangle(p1, p2, p3)
        Dim t2 = New Triangle(p1, p3, p2)
        Dim t3 = New Triangle(p3, p2, p1)

        Assert.AreEqual(t1, t2)
        Assert.AreEqual(t1, t3)
        Assert.AreEqual(t3, t2)
    End Sub

    <TestMethod()> Public Sub TriangleAreaTest()
        Dim p1 As Point3d = New Point3d(0, 0, 0)
        Dim p2 As Point3d = New Point3d(1, 0, 0)
        Dim p3 As Point3d = New Point3d(0, 1, 0)

        Dim t = New Triangle(p1, p2, p3)
        Assert.IsTrue(Abs(t.Area - 0.5) < GeometRi3D.Tolerance)
    End Sub

    <TestMethod()> Public Sub TriangleBisectorTest()
        Dim p1 As Point3d = New Point3d(0, 0, 0)
        Dim p2 As Point3d = New Point3d(1, 0, 0)
        Dim p3 As Point3d = New Point3d(0, 1, 0)
        Dim t = New Triangle(p1, p2, p3)
        Assert.AreEqual(t.Bisector_A, New Segment3d(p1, New Point3d(0.5, 0.5, 0)))

        t = New Triangle(p2, p3, p1)
        Assert.AreEqual(t.Bisector_C, New Segment3d(p1, New Point3d(0.5, 0.5, 0)))

        t = New Triangle(p3, p1, p2)
        Assert.AreEqual(t.Bisector_B, New Segment3d(p1, New Point3d(0.5, 0.5, 0)))
    End Sub

End Class