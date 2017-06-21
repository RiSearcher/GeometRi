Imports System.Math

Public Class Circle3d

    Implements ICloneable

    Private _point As Point3d
    Private _r As Double
    Private _normal As Vector3d

    Public Sub New(Center As Point3d, Radius As Double, Normal As Vector3d)
        _point = Center.Clone
        _r = Radius
        _normal = Normal.Clone
    End Sub

    Public Sub New(p1 As Point3d, p2 As Point3d, p3 As Point3d)

        Dim v1 As Vector3d = New Vector3d(p1, p2)
        Dim v2 As Vector3d = New Vector3d(p1, p3)
        If v1.Cross(v2).Norm < GeometRi3D.Tolerance Then
            Throw New Exception("Collinear points")
        End If

        Dim CS As Coord3d = New Coord3d(p1, v1, v2)
        Dim a1 As Point3d = p1.ConvertTo(CS)
        Dim a2 As Point3d = p2.ConvertTo(CS)
        Dim a3 As Point3d = p3.ConvertTo(CS)

        Dim d1 As Double = a1.X ^ 2 + a1.Y ^ 2
        Dim d2 As Double = a2.X ^ 2 + a2.Y ^ 2
        Dim d3 As Double = a3.X ^ 2 + a3.Y ^ 2
        Dim f As Double = 2.0 * (a1.X * (a2.Y - a3.Y) - a1.Y * (a2.X - a3.X) + a2.X * a3.Y - a3.X * a2.Y)

        Dim X = (d1 * (a2.Y - a3.Y) + d2 * (a3.Y - a1.Y) + d3 * (a1.Y - a2.Y)) / f
        Dim Y = (d1 * (a3.X - a2.X) + d2 * (a1.X - a3.X) + d3 * (a2.X - a1.X)) / f
        _point = (New Point3d(X, Y, 0, CS)).ConvertTo(p1.Coord)
        _r = Sqrt((X - a1.X) ^ 2 + (Y - a1.Y) ^ 2)
        _normal = v1.Cross(v2)

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Circle3d = DirectCast(MemberwiseClone(), Circle3d)
        newobj.Center = newobj.Center.Clone
        newobj.Normal = newobj.Normal.Clone
        Return newobj
    End Function

    ''' <summary>
    ''' Center of the circle
    ''' </summary>
    Public Property Center As Point3d
        Get
            Return _point.Clone
        End Get
        Set(value As Point3d)
            _point = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Radius of the circle
    ''' </summary>
    Public Property R As Double
        Get
            Return _r
        End Get
        Set(value As Double)
            _r = value
        End Set
    End Property

    ''' <summary>
    ''' Normal of the circle
    ''' </summary>
    Public Property Normal As Vector3d
        Get
            Return _normal
        End Get
        Set(value As Vector3d)
            _normal = value
        End Set
    End Property

    Public ReadOnly Property Perimeter As Double
        Get
            Return 2 * PI * _r
        End Get
    End Property

    Public ReadOnly Property Area As Double
        Get
            Return PI * _r ^ 2
        End Get
    End Property

    Public ReadOnly Property ToEllipse As Ellipse
        Get
            Dim v1 As Vector3d = _r * _normal.OrthogonalVector.Normalized
            Dim v2 As Vector3d = _r * (_normal.Cross(v1)).Normalized
            Return New Ellipse(_point, v1, v2)
        End Get
    End Property

    ''' <summary>
    ''' Returns point on circle for given parameter 't' (0 &lt;= t &lt; 2Pi)
    ''' </summary>
    Public Function ParametricForm(t As Double) As Point3d

        ' Get two orthogonal coplanar vectors
        Dim v1 As Vector3d = _r * _normal.OrthogonalVector.Normalized
        Dim v2 As Vector3d = _r * (_normal.Cross(v1)).Normalized
        Return _point + v1.ToPoint * Cos(t) + v2.ToPoint * Sin(t)

    End Function

    ''' <summary>
    ''' Orthogonal projection of the circle to plane
    ''' </summary>
    Public Function ProjectionTo(s As Plane3d) As Ellipse
        Return Me.ToEllipse.ProjectionTo(s)
    End Function

    ''' <summary>
    ''' Intersection of circle with plane.
    ''' Returns object of type 'Nothing', 'Circle3d', 'Point3d' or 'Segment3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Plane3d) As Object

        If Me.Normal.IsParallelTo(s.Normal) Then

            If Me.Center.BelongsTo(s) Then
                ' coplanar objects
                Return Me.Clone
            Else
                ' parallel objects
                Return Nothing
            End If
        Else
            Dim l As Line3d = s.IntersectionWith(New Plane3d(Me.Center, Me.Normal))
            Dim local_coord As Coord3d = New Coord3d(Me.Center, l.Direction, Me.Normal.Cross(l.Direction))
            Dim p As Point3d = l.Point.ConvertTo(local_coord)

            If GeometRi3D.Greater(Abs(p.Y), Me.R) Then
                Return Nothing
            ElseIf GeometRi3D.AlmostEqual(p.Y, Me.R) Then
                Return New Point3d(0, Me.R, 0, local_coord)
            ElseIf GeometRi3D.AlmostEqual(p.Y, -Me.R) Then
                Return New Point3d(0, -Me.R, 0, local_coord)
            Else
                Dim d As Double = Sqrt(Me.R ^ 2 - p.Y ^ 2)
                Dim p1 = New Point3d(-d, p.Y, 0, local_coord)
                Dim p2 = New Point3d(d, p.Y, 0, local_coord)
                Return New Segment3d(p1, p2)
            End If
        End If

    End Function

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate circle by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Circle3d
        Return New Circle3d(Me.Center.Translate(v), Me.R, Me.Normal)
    End Function

    ''' <summary>
    ''' Rotate circle by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Circle3d
        Return New Circle3d(Me.Center.Rotate(m), Me.R, Me.Normal.Rotate(m))
    End Function

    ''' <summary>
    ''' Rotate circle by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Circle3d
        Return New Circle3d(Me.Center.Rotate(m, p), Me.R, Me.Normal.Rotate(m))
    End Function

    ''' <summary>
    ''' Reflect circle in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Circle3d
        Return New Circle3d(Me.Center.ReflectIn(p), Me.R, Me.Normal.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect circle in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Circle3d
        Return New Circle3d(Me.Center.ReflectIn(l), Me.R, Me.Normal.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect circle in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Circle3d
        Return New Circle3d(Me.Center.ReflectIn(s), Me.R, Me.Normal.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim c As Circle3d = CType(obj, Circle3d)

        Return c.Center = Me.Center AndAlso
               Abs(c.R - Me.R) <= GeometRi3D.Tolerance AndAlso
               c.Normal.IsParallelTo(Me.Normal)
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String

        Dim P As Point3d = _point.ConvertToGlobal
        Dim normal As Vector3d = _normal.ConvertToGlobal
        If coord IsNot Nothing Then
            P = P.ConvertTo(coord)
            normal = normal.ConvertTo(coord)
        End If

        Dim str As String = String.Format("Circle: ") + vbCrLf
        str += String.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + vbCrLf
        str += String.Format("  Radius -> {0,10:g5}", _r) + vbCrLf
        str += String.Format("  Normal -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", normal.X, normal.Y, normal.Z)
        Return str
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------

    Public Shared Operator =(c1 As Circle3d, c2 As Circle3d) As Boolean
        Return c1.Equals(c2)
    End Operator
    Public Shared Operator <>(c1 As Circle3d, c2 As Circle3d) As Boolean
        Return Not c1.Equals(c2)
    End Operator

End Class
