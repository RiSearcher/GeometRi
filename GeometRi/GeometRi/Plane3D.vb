Imports System.Math

Public Class Plane3d

    Implements ICloneable

    Private _point As Point3d
    Private _normal As Vector3d
    Private _coord As Coord3d

#Region "Constructors"
    ''' <summary>
    ''' Create default XY plane
    ''' </summary>
    Public Sub New()
        _point = New Point3d(0, 0, 0)
        _normal = New Vector3d(0, 0, 1)
    End Sub

    ''' <summary>
    ''' Create plane using general equation in 3D space: A*x+B*y+C*z+D=0.
    ''' </summary>
    ''' <param name="a">Parameter "A" in general plane equation.</param>
    ''' <param name="b">Parameter "B" in general plane equation.</param>
    ''' <param name="c">Parameter "C" in general plane equation.</param>
    ''' <param name="d">Parameter "D" in general plane equation.</param>
    ''' <param name="coord">Coordinate system in which plane equation is defined (default: Coord3d.GlobalCS).</param>
    Public Sub New(a As Double, b As Double, c As Double, d As Double, Optional coord As Coord3d = Nothing)
        If coord Is Nothing Then
            coord = Coord3d.GlobalCS
        End If
        If Abs(a) > Abs(b) AndAlso Abs(a) > Abs(c) Then
            _point = New Point3d(-d / a, 0, 0, coord)
        ElseIf Abs(b) > Abs(a) AndAlso Abs(b) > Abs(c) Then
            _point = New Point3d(0, -d / b, 0, coord)
        Else
            _point = New Point3d(0, 0, -d / c, coord)
        End If
        _normal = New Vector3d(a, b, c, coord)
    End Sub

    ''' <summary>
    ''' Create plane by three points.
    ''' </summary>
    ''' <param name="p1">First point.</param>
    ''' <param name="p2">Second point.</param>
    ''' <param name="p3">Third point.</param>
    Public Sub New(p1 As Point3d, p2 As Point3d, p3 As Point3d)
        Dim v1 As Vector3d = New Vector3d(p1, p2)
        Dim v2 As Vector3d = New Vector3d(p1, p3)
        _normal = v1.Cross(v2)
        _point = p1
    End Sub

    ''' <summary>
    ''' Create plane by point and two vectors lying in the plane.
    ''' </summary>
    Public Sub New(p1 As Point3d, v1 As Vector3d, v2 As Vector3d)
        _normal = v1.Cross(v2)
        _point = p1
    End Sub

    ''' <summary>
    ''' Create plane by point and normal vector.
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="v1"></param>
    Public Sub New(p1 As Point3d, v1 As Vector3d)
        _normal = v1
        _point = p1
    End Sub
#End Region


    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Plane3d = DirectCast(MemberwiseClone(), Plane3d)
        newobj.Point = newobj.Point.Clone
        newobj.Normal = newobj.Normal.Clone
        Return newobj
    End Function

    ''' <summary>
    ''' Point of the plane
    ''' </summary>
    ''' <returns></returns>
    Public Property Point As Point3d
        Get
            Return _point.Clone
        End Get
        Set(value As Point3d)
            _point = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Normal vector of the plane
    ''' </summary>
    ''' <returns></returns>
    Public Property Normal As Vector3d
        Get
            Return _normal.Clone
        End Get
        Set(value As Vector3d)
            _normal = value.Clone
        End Set
    End Property

    Public Sub SetCoord(coord As Coord3d)
        _coord = coord
    End Sub

    Public ReadOnly Property A As Double
        Get
            A = _normal.ConvertTo(_coord).X
        End Get
    End Property
    Public ReadOnly Property B As Double
        Get
            B = _normal.ConvertTo(_coord).Y
        End Get
    End Property
    Public ReadOnly Property C As Double
        Get
            C = _normal.ConvertTo(_coord).Z
        End Get
    End Property
    Public ReadOnly Property D As Double
        Get
            Dim p As Point3d = _point.ConvertTo(_coord)
            Dim v As Vector3d = _normal.ConvertTo(_coord)
            D = -v.X * p.X - v.Y * p.Y - v.Z * p.Z
        End Get
    End Property



    ''' <summary>
    ''' Get intersection of line with plane.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Line3d'.
    ''' </summary>
    Public Function IntersectionWith(l As Line3d) As Object
        Dim r1 As Vector3d = New Vector3d(l.Point)
        Dim s1 As Vector3d = l.Direction
        Dim n2 As Vector3d = Me.Normal
        If Abs(s1 * n2) < GeometRi3D.Tolerance Then
            ' Line and plane are parallel
            If l.Point.BelongsTo(Me) Then
                ' Line lies in the plane
                Return l
            Else
                Return Nothing
            End If
        Else
            r1 = r1 - ((r1 * n2) + Me.D) / (s1 * n2) * s1
            Return r1.ToPoint
        End If
    End Function

    ''' <summary>
    ''' Finds the common intersection of three planes.
    ''' Return object of type 'Nothing', 'Point3d', 'Line3d' or 'Plane3d'
    ''' </summary>
    Public Function IntersectionWith(s2 As Plane3d, s3 As Plane3d) As Object
        ' Set all planes to global CS
        Me.SetCoord(Coord3d.GlobalCS)
        s2.SetCoord(Coord3d.GlobalCS)
        s3.SetCoord(Coord3d.GlobalCS)
        Dim det As Double = New Matrix3d({A, B, C}, {s2.A, s2.B, s2.C}, {s3.A, s3.B, s3.C}).Det
        If Abs(det) < GeometRi3D.Tolerance Then
            If Me.Normal.IsParallelTo(s2.Normal) AndAlso Me.Normal.IsParallelTo(s3.Normal) Then
                ' Planes are coplanar
                If Me.Point.BelongsTo(s2) AndAlso Me.Point.BelongsTo(s3) Then
                    Return Me
                Else
                    Return Nothing
                End If
            End If
            If Me.Normal.IsNotParallelTo(s2.Normal) AndAlso Me.Normal.IsNotParallelTo(s3.Normal) Then
                ' Planes are not parallel
                ' Find the intersection (Me,s2) and (Me,s3) and check if it is the same line
                Dim l1 As Line3d = Me.IntersectionWith(s2)
                Dim l2 As Line3d = Me.IntersectionWith(s3)
                If l1 = l2 Then
                    Return l1
                Else
                    Return Nothing
                End If
            End If

            ' Two planes are parallel, third plane is not
            Return Nothing

        Else
            Dim x As Double = -New Matrix3d({D, B, C}, {s2.D, s2.B, s2.C}, {s3.D, s3.B, s3.C}).Det / det
            Dim y As Double = -New Matrix3d({A, D, C}, {s2.A, s2.D, s2.C}, {s3.A, s3.D, s3.C}).Det / det
            Dim z As Double = -New Matrix3d({A, B, D}, {s2.A, s2.B, s2.D}, {s3.A, s3.B, s3.D}).Det / det
            Return New Point3d(x, y, z)
        End If
    End Function

    ''' <summary>
    ''' Get intersection of two planes.
    ''' Returns object of type 'Nothing', 'Line3d' or 'Plane3d'.
    ''' </summary>
    Public Function IntersectionWith(s2 As Plane3d) As Object
        Dim v As Vector3d = Me.Normal.Cross(s2.Normal).ConvertToGlobal
        If v.Norm < GeometRi3D.Tolerance Then
            ' Planes are coplanar
            If Me.Point.BelongsTo(s2) Then
                Return Me
            Else
                Return Nothing
            End If

        Else
            ' Find the common point for two planes by intersecting with third plane
            ' (using the 'most orthogonal' plane)
            ' This part needs to be rewritten
            If Abs(v.X) > Abs(v.Y) AndAlso Abs(v.X) > Abs(v.Z) Then
                Dim p As Point3d = Me.IntersectionWith(s2, Coord3d.GlobalCS.YZ_plane)
                Return New Line3d(p, v)
            ElseIf Abs(v.Y) > Abs(v.X) AndAlso Abs(v.Y) > Abs(v.Z) Then
                Dim p As Point3d = Me.IntersectionWith(s2, Coord3d.GlobalCS.XZ_plane)
                Return New Line3d(p, v)
            Else
                Dim p As Point3d = Me.IntersectionWith(s2, Coord3d.GlobalCS.XY_plane)
                Return New Line3d(p, v)
            End If
        End If
    End Function

    ''' <summary>
    ''' Get intersection of plane with sphere.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Circle3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Sphere)
        Return s.IntersectionWith(Me)
    End Function


#Region "AngleTo"
    ''' <summary>
    ''' Angle between vector and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(v As Vector3d) As Double
        Return Abs(PI / 2 - Me.Normal.AngleTo(v))
    End Function
    ''' <summary>
    ''' Angle between vector and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(v As Vector3d) As Double
        Return Abs(90.0 - Me.Normal.AngleToDeg(v))
    End Function

    ''' <summary>
    ''' Angle between line and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(l As Line3d) As Double
        Return Abs(PI / 2 - Me.Normal.AngleTo(l.Direction))
    End Function
    ''' <summary>
    ''' Angle between line and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(l As Line3d) As Double
        Return Abs(90.0 - Me.Normal.AngleToDeg(l.Direction))
    End Function

    ''' <summary>
    ''' Angle between two planes in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(s As Plane3d) As Double
        Dim ang As Double = Me.Normal.AngleTo(s.Normal)
        If ang <= PI / 2 Then
            Return ang
        Else
            Return PI - ang
        End If
    End Function
    ''' <summary>
    ''' Angle between two planes in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(s As Plane3d) As Double
        Return AngleTo(s) * 180 / PI
    End Function
#End Region


#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate plane by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Plane3d
        Return New Plane3d(Me.Point.Translate(v), Me.Normal)
    End Function

    ''' <summary>
    ''' Rotate plane by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Plane3d
        Return New Plane3d(Me.Point.Rotate(m), Me.Normal.Rotate(m))
    End Function

    ''' <summary>
    ''' Rotate plane by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Plane3d
        Return New Plane3d(Me.Point.Rotate(m, p), Me.Normal.Rotate(m))
    End Function

    ''' <summary>
    ''' Reflect plane in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Plane3d
        Return New Plane3d(Me.Point.ReflectIn(p), Me.Normal.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect plane in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Plane3d
        Return New Plane3d(Me.Point.ReflectIn(l), Me.Normal.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect plane in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Plane3d
        Return New Plane3d(Me.Point.ReflectIn(s), Me.Normal.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim s As Plane3d = CType(obj, Plane3d)

        Return s.Point.BelongsTo(Me) AndAlso s.Normal.IsParallelTo(Me.Normal)
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim str As New System.Text.StringBuilder
        Dim P As Point3d = _point.ConvertToGlobal
        Dim normal As Vector3d = _normal.ConvertToGlobal
        If coord IsNot Nothing Then
            P = _point.ConvertTo(coord)
            normal = _normal.ConvertTo(coord)
        End If
        str.Append("Plane3d:" + vbCrLf)
        str.Append(String.Format("Point  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + vbCrLf)
        str.Append(String.Format("Normal -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", normal.X, normal.Y, normal.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------

    Public Shared Operator =(s1 As Plane3d, s2 As Plane3d) As Boolean
        Return s1.Equals(s2)
    End Operator
    Public Shared Operator <>(s1 As Plane3d, s2 As Plane3d) As Boolean
        Return Not s1.Equals(s2)
    End Operator

End Class
