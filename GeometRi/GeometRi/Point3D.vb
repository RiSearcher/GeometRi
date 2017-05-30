Imports System.Math

Public Class Point3d
    Inherits GeometRi3D
    Implements ICloneable

    Private _x, _y, _z As Double

#Region "Constructors"
    Public Sub New(Optional coord As Coord3d = Nothing)
        _x = 0.0
        _y = 0.0
        _z = 0.0
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub
    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double, Optional coord As Coord3d = Nothing)
        _x = x
        _y = y
        _z = z
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub

    Public Sub New(ByVal a As Double(), Optional coord As Coord3d = Nothing)
        If a.GetUpperBound(0) < 2 Then Throw New Exception("Point3d: Array size mismatch")
        _x = a(0)
        _y = a(1)
        _z = a(2)
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub
#End Region

    Public Function Clone() As Object Implements ICloneable.Clone
        Return DirectCast(MemberwiseClone(), Point3d)
    End Function

    Public Property X As Double
        Get
            Return _x
        End Get
        Set(value As Double)
            _x = value
        End Set
    End Property
    Public Property Y As Double
        Get
            Return _y
        End Get
        Set(value As Double)
            _y = value
        End Set
    End Property
    Public Property Z As Double
        Get
            Return _z
        End Get
        Set(value As Double)
            _z = value
        End Set
    End Property

    Public ReadOnly Property ToVector As Vector3d
        Get
            Return New Vector3d(Me)
        End Get
    End Property

    ''' <summary>
    ''' Convert point to local coordinate system
    ''' </summary>
    Public Function ConvertTo(ByVal coord As Coord3d) As Point3d
        Dim p As Point3d = Me.Clone

        p = p.ConvertToGlobal()
        If coord Is Nothing Then Return p
        If coord Is Coord3d.GlobalCS Then Return p



        Dim v As Vector3d
        ' If coord is cloned from GlobalCS, its Origin does not have a reference to coord.sys.
        If coord.Origin = New Point3d(0, 0, 0) Then
            v = New Vector3d(p)
        Else
            v = New Vector3d(coord.Origin, p)
        End If
        p.X = v.Dot(coord.Xaxis)
        p.Y = v.Dot(coord.Yaxis)
        p.Z = v.Dot(coord.Zaxis)
        p._coord = coord


        Return p
    End Function
    ''' <summary>
    ''' Convert point from current local coordinate system to global coordinate system
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertToGlobal() As Point3d
        If _coord Is Nothing OrElse _coord Is Coord3d.GlobalCS Then
            Return Me
        Else
            Dim v As Vector3d = New Vector3d(Me.X, Me.Y, Me.Z)
            v = _coord.Axes.Inverse * v

            Return v.ToPoint + _coord.Origin

        End If

    End Function

    Public Function Add(ByVal p As Point3d) As Point3d
        If (Me._coord <> p._coord) Then p = p.ConvertTo(Me._coord)
        Dim tmp As Point3d = Me.Clone
        tmp.X += p.X
        tmp.Y += p.Y
        tmp.Z += p.Z
        Return tmp
    End Function
    Public Function Subtract(ByVal p As Point3d) As Point3d
        If (Me._coord <> p._coord) Then p = p.ConvertTo(Me._coord)
        Dim tmp As Point3d = Me.Clone
        tmp.X -= p.X
        tmp.Y -= p.Y
        tmp.Z -= p.Z
        Return tmp
    End Function
    Public Function Scale(ByVal a As Double) As Point3d
        Dim tmp As Point3d = Me.Clone
        tmp.X *= a
        tmp.Y *= a
        tmp.Z *= a
        Return tmp
    End Function

#Region "DistaneTo"
    ''' <summary>
    ''' Returns distance between two points
    ''' </summary>
    Public Function DistanceTo(p As Point3d) As Double
        If (Me._coord <> p._coord) Then p = p.ConvertTo(Me._coord)
        Return New Vector3d(Me, p).Norm
    End Function

    ''' <summary>
    ''' Returns shortest distance to the line
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Function DistanceTo(l As Line3d) As Double
        Dim v As Vector3d = New Vector3d(Me, l.Point)
        Return v.Cross(l.Direction).Norm / l.Direction.Norm
    End Function

    ''' <summary>
    ''' Returns shortest distance from point to the plane
    ''' </summary>
    Public Function DistanceTo(s As Plane3d) As Double
        s.SetCoord(Me.Coord)
        Return Abs(X * s.A + Y * s.B + Z * s.C + s.D) / Sqrt(s.A ^ 2 + s.B ^ 2 + s.C ^ 2)
    End Function

    ''' <summary>
    ''' Returns shortest distance from point to the ray
    ''' </summary>
    Public Function DistanceTo(r As Ray3d) As Double
        If Me.ProjectionTo(r.ToLine).BelongsTo(r) Then
            Return Me.DistanceTo(r.ToLine)
        Else
            Return Me.DistanceTo(r.Point)
        End If
    End Function

    ''' <summary>
    ''' Returns shortest distance from point to the segment
    ''' </summary>
    Public Function DistanceTo(s As Segment3d) As Double
        If Me.ProjectionTo(s.ToLine).BelongsTo(s) Then
            Return Me.DistanceTo(s.ToLine)
        Else
            Return Min(Me.DistanceTo(s.P1), Me.DistanceTo(s.P2))
        End If
    End Function

#End Region


    ''' <summary>
    ''' Returns orthogonal projection of the point to the plane
    ''' </summary>
    Public Function ProjectionTo(s As Plane3d) As Point3d
        Dim r0 As Vector3d = New Vector3d(Me)
        s.SetCoord(Me.Coord)
        Dim n As Vector3d = New Vector3d(s.A, s.B, s.C, _coord)
        r0 = r0 - (r0 * n + s.D) / (n * n) * n
        Return r0.ToPoint
    End Function
    ''' <summary>
    ''' Returns orthogonal projection of the point to the line
    ''' </summary>
    Public Function ProjectionTo(l As Line3d) As Point3d
        Dim r0 As Vector3d = New Vector3d(Me)
        Dim r1 As Vector3d = l.Point.ToVector
        Dim s As Vector3d = l.Direction
        r0 = r1 - ((r1 - r0) * s) / (s * s) * s
        Return r0.ToPoint
    End Function

    ''' <summary>
    ''' Returns orthogonal projection of the point to the surface of the sphere
    ''' </summary>
    Public Function ProjectionTo(s As Sphere) As Point3d
        Dim v As Vector3d = New Vector3d(s.Center, Me)
        Return s.Center + s.R * v.Normalized.ToPoint
    End Function

    ''' <summary>
    ''' Check if point belongs to the line
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns>True, if the point belongs to the line</returns>
    Public Function BelongsTo(l As Line3d) As Boolean
        If Me = l.Point Then
            Return True
        Else
            Return l.Direction.IsParallelTo(New Vector3d(Me, l.Point))
        End If
    End Function

    ''' <summary>
    ''' Check if point belongs to the ray
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns>True, if the point belongs to the ray</returns>
    Public Function BelongsTo(l As Ray3d) As Boolean
        If Me = l.Point Then
            Return True
        Else
            Dim v As Vector3d = New Vector3d(l.Point, Me)
            If l.Direction.Normalized = v.Normalized Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' Check if point belongs to the segment
    ''' </summary>
    ''' <returns>True, if the point belongs to the segment</returns>
    Public Function BelongsTo(s As Segment3d) As Boolean
        Return Me.BelongsTo(s.ToRay) AndAlso Me.BelongsTo(New Ray3d(s.P2, New Vector3d(s.P2, s.P1)))
    End Function

    ''' <summary>
    ''' Check if point belongs to the plane
    ''' </summary>
    ''' <returns>True, if the point belongs to the plane</returns>
    Public Function BelongsTo(s As Plane3d) As Boolean
        s.SetCoord(Me.Coord)
        Return Abs(s.A * X + s.B * Y + s.C * Z + s.D) < Tolerance
    End Function

    ''' <summary>
    ''' Check if point belongs to the sphere
    ''' </summary>
    ''' <returns>True, if the point belongs to the sphere</returns>
    Public Function BelongsTo(s As Sphere) As Boolean
        Return Me.DistanceTo(s.Center) < s.R + Tolerance
    End Function

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate point by a vector
    ''' </summary>
    Public Function Translate(ByVal v As Vector3d) As Point3d
        If (Me._coord <> v.Coord) Then v = v.ConvertTo(Me._coord)
        Return Me + v.ToPoint
    End Function

    ''' <summary>
    ''' Rotate point by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Point3d
        Return m * Me
    End Function

    ''' <summary>
    ''' Rotate point by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Point3d
        If (Me._coord <> p.Coord) Then p = p.ConvertTo(Me._coord)
        Return m * (Me - p) + p
    End Function

    ''' <summary>
    ''' Reflect point in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Point3d
        If (Me._coord <> p.Coord) Then p = p.ConvertTo(Me._coord)
        Dim v As Vector3d = New Vector3d(Me, p)
        Return Me.Translate(2 * v)
    End Function

    ''' <summary>
    ''' Reflect point in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Point3d
        Dim v As Vector3d = New Vector3d(Me, Me.ProjectionTo(l))
        Return Me.Translate(2 * v)
    End Function

    ''' <summary>
    ''' Reflect point in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Point3d
        Dim v As Vector3d = New Vector3d(Me, Me.ProjectionTo(s))
        Return Me.Translate(2 * v)
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim p As Point3d = CType(obj, Point3d)
        If (Me._coord <> p.Coord) Then p = p.ConvertTo(_coord)
        Return Me.DistanceTo(p) < Tolerance
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim p As Point3d = Me.ConvertToGlobal
        If coord IsNot Nothing Then
            p = Me.ConvertTo(coord)
        End If
        Dim str As String = String.Format("Point3d -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p.X, p.Y, p.Z) + vbCrLf
        Return str
    End Function


    ' Operators overloads
    '-----------------------------------------------------------------
    Public Shared Operator +(v As Point3d, a As Point3d) As Point3d
        Return v.Add(a)
    End Operator
    Public Shared Operator -(v As Point3d, a As Point3d) As Point3d
        Return v.Subtract(a)
    End Operator
    Public Shared Operator -(v As Point3d) As Point3d
        Return v.Scale(-1.0)
    End Operator
    Public Shared Operator *(v As Point3d, a As Double) As Point3d
        Return v.Scale(a)
    End Operator
    Public Shared Operator *(a As Double, v As Point3d) As Point3d
        Return v.Scale(a)
    End Operator
    Public Shared Operator /(v As Point3d, a As Double) As Point3d
        Return v.Scale(1.0 / a)
    End Operator

    Public Shared Operator =(p1 As Point3d, p2 As Point3d) As Boolean
        Return p1.Equals(p2)
    End Operator
    Public Shared Operator <>(p1 As Point3d, p2 As Point3d) As Boolean
        Return Not p1.Equals(p2)
    End Operator

End Class
