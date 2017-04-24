Imports System.Math

Public Class Ray3d

    Implements ICloneable

    Protected _point As Point3d
    Protected _dir As Vector3d

    Public Sub New()
        _point = New Point3d()
        _dir = New Vector3d(1, 0, 0)
    End Sub
    Public Sub New(ByVal p As Point3d, ByVal v As Vector3d)
        _point = p.Clone
        _dir = v.ConvertTo(p.Coord).Normalized
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Line3d = DirectCast(MemberwiseClone(), Line3d)
        newobj.Point = newobj.Point.Clone
        newobj.Direction = newobj.Direction.Clone
        Return newobj
    End Function

    ''' <summary>
    ''' Base point of the ray
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
    ''' Direction vector of the ray
    ''' </summary>
    ''' <returns></returns>
    Public Property Direction As Vector3d
        Get
            Return _dir.Clone
        End Get
        Set(value As Vector3d)
            _dir = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Convert ray to line
    ''' </summary>
    Public ReadOnly Property ToLine As Line3d
        Get
            Return New Line3d(Me.Point, Me.Direction)
        End Get
    End Property


    ''' <summary>
    ''' Shortest distance to a line
    ''' </summary>
    Public Function DistanceTo(l As Line3d) As Double
        Dim r1 As Vector3d = Me.Point.ToVector
        Dim r2 As Vector3d = l.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim s2 As Vector3d = l.Direction
        If s1.Cross(s2).Norm > GeometRi3D.Tolerance Then
            ' Crossing lines
            Dim p As Point3d = l.PerpendicularTo(New Line3d(Me.Point, Me.Direction))
            If p.BelongsTo(Me) Then
                Return Abs((r2 - r1) * s1.Cross(s2)) / s1.Cross(s2).Norm
            Else
                Return Me.Point.DistanceTo(l)
            End If
        Else
            ' Parallel lines
            Return (r2 - r1).Cross(s1).Norm / s1.Norm
        End If
    End Function

    ''' <summary>
    ''' Point on the perpendicular to the line
    ''' </summary>
    Public Function PerpendicularTo(l As Line3d) As Point3d
        Dim r1 As Vector3d = Me.Point.ToVector
        Dim r2 As Vector3d = l.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim s2 As Vector3d = l.Direction
        If s1.Cross(s2).Norm > GeometRi3D.Tolerance Then
            Dim p As Point3d = l.PerpendicularTo(New Line3d(Me.Point, Me.Direction))
            If p.BelongsTo(Me) Then
                r1 = r2 + (r2 - r1) * s1.Cross(s1.Cross(s2)) / (s1 * s2.Cross(s1.Cross(s2))) * s2
                Return r1.ToPoint
            Else
                Return Me.Point.ProjectionTo(l)
            End If
        Else
            Throw New Exception("Lines are parallel")
        End If
    End Function

    ''' <summary>
    ''' Get intersection of ray with plane.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Ray3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Plane3d) As Object

        Dim r1 As Vector3d = Me.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim n2 As Vector3d = s.Normal
        If Abs(s1 * n2) < GeometRi3D.Tolerance Then
            ' Ray and plane are parallel
            If Me.Point.BelongsTo(s) Then
                ' Ray lies in the plane
                Return Me
            Else
                Return Nothing
            End If
        Else
            ' Intersection point
            s.SetCoord(r1.Coord)
            r1 = r1 - ((r1 * n2) + s.D) / (s1 * n2) * s1
            If r1.ToPoint.BelongsTo(Me) Then
                Return r1.ToPoint
            Else Return Nothing
            End If
        End If
    End Function

    ''' <summary>
    ''' Get the orthogonal projection of a ray to the plane.
    ''' Return object of type 'Ray3d' or 'Point3d'
    ''' </summary>
    Public Function ProjectionTo(s As Plane3d) As Object
        Dim n1 As Vector3d = s.Normal
        Dim n2 As Vector3d = Me.Direction.Cross(n1)
        If n2.Norm < GeometRi3D.Tolerance Then
            ' Ray is perpendicular to the plane
            Return Me.Point.ProjectionTo(s)
        Else
            Return New Ray3d(Me.Point.ProjectionTo(s), n1.Cross(n2))
        End If
    End Function

    ''' <summary>
    ''' Angle between ray and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(s As Plane3d) As Double
        Dim ang As Double = Asin(Me.Direction.Dot(s.Normal) / Me.Direction.Norm / s.Normal.Norm)
        Return Abs(ang)
    End Function
    ''' <summary>
    ''' Angle between ray and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(s As Plane3d) As Double
        Return AngleTo(s) * 180 / PI
    End Function

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate ray by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Ray3d
        Dim l As Ray3d = Me.Clone
        l.Point = l.Point.Translate(v)
        Return l
    End Function

    ''' <summary>
    ''' Rotate ray by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Ray3d
        Dim l As Ray3d = Me.Clone
        l.Point = l.Point.Rotate(m)
        l.Direction = l.Direction.Rotate(m)
        Return l
    End Function

    ''' <summary>
    ''' Rotate ray by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Ray3d
        Dim l As Ray3d = Me.Clone
        l.Point = l.Point.Rotate(m, p)
        l.Direction = l.Direction.Rotate(m)
        Return l
    End Function

    ''' <summary>
    ''' Reflect ray in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Ray3d
        Return New Ray3d(Me.Point.ReflectIn(p), Me.Direction.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect ray in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Ray3d
        Return New Ray3d(Me.Point.ReflectIn(l), Me.Direction.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect ray in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Ray3d
        Return New Ray3d(Me.Point.ReflectIn(s), Me.Direction.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim r As Ray3d = CType(obj, Ray3d)
        Return Me.Point = r.Point AndAlso Abs(Me.Direction.Normalized * r.Direction.Normalized - 1) < GeometRi3D.Tolerance
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim str As New System.Text.StringBuilder
        Dim P As Point3d = _point.ConvertToGlobal
        Dim dir As Vector3d = _dir.ConvertToGlobal
        If coord IsNot Nothing Then
            P = _point.ConvertTo(coord)
            dir = _dir.ConvertTo(coord)
        End If
        str.Append("Ray:" + vbCrLf)
        str.Append(String.Format("Point  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + vbCrLf)
        str.Append(String.Format("Direction -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", dir.X, dir.Y, dir.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    Public Overloads Shared Operator =(l1 As Ray3d, l2 As Ray3d) As Boolean
        Return l1.Equals(l2)
    End Operator
    Public Overloads Shared Operator <>(l1 As Ray3d, l2 As Ray3d) As Boolean
        Return Not l1.Equals(l2)
    End Operator


End Class
