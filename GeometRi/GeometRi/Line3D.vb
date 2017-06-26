Imports System.Math

Public Class Line3d

    Implements ICloneable

    Private _point As Point3d
    Private _dir As Vector3d

#Region "Constructors"
    ''' <summary>
    ''' Create default line.
    ''' </summary>
    Public Sub New()
        _point = New Point3d()
        _dir = New Vector3d(1, 0, 0)
    End Sub

    ''' <summary>
    ''' Create line by point and dirction.
    ''' </summary>
    ''' <param name="p">Point on the line.</param>
    ''' <param name="v">Direction vector.</param>
    Public Sub New(ByVal p As Point3d, ByVal v As Vector3d)
        _point = p.Clone
        _dir = v.Clone
    End Sub

    ''' <summary>
    ''' Create line by two points.
    ''' </summary>
    ''' <param name="p1">First point.</param>
    ''' <param name="p2">Second point.</param>
    Public Sub New(ByVal p1 As Point3d, ByVal p2 As Point3d)
        _point = p1.Clone
        _dir = New Vector3d(p1, p2)
    End Sub
#End Region


    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Line3d = DirectCast(MemberwiseClone(), Line3d)
        newobj.Point = newobj.Point.Clone
        newobj.Direction = newobj.Direction.Clone
        Return newobj
    End Function

    ''' <summary>
    ''' Base point of the line
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
    ''' Direction vector of the line
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

#Region "DistanceTo"
    ''' <summary>
    ''' Shortest distance between line and point
    ''' </summary>
    Public Function DistanceTo(p As Point3d) As Double
        Return p.DistanceTo(Me)
    End Function

    ''' <summary>
    ''' Shortest distance between line and ray
    ''' </summary>
    Public Function DistanceTo(r As Ray3d) As Double
        Return r.DistanceTo(Me)
    End Function

    ''' <summary>
    ''' Shortest distance between line and segment
    ''' </summary>
    Public Function DistanceTo(s As Segment3d) As Double
        Return s.DistanceTo(Me)
    End Function

    ''' <summary>
    ''' Shortest distance between two lines
    ''' </summary>
    Public Overridable Function DistanceTo(l As Line3d) As Double
        Dim r1 As Vector3d = Me.Point.ToVector
        Dim r2 As Vector3d = l.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim s2 As Vector3d = l.Direction
        If s1.Cross(s2).Norm > GeometRi3D.Tolerance Then
            ' Crossing lines
            Return Abs((r2 - r1) * s1.Cross(s2)) / s1.Cross(s2).Norm
        Else
            ' Parallel lines
            Return (r2 - r1).Cross(s1).Norm / s1.Norm
        End If
    End Function
#End Region


    ''' <summary>
    ''' Point on the perpendicular to the second line
    ''' </summary>
    Public Overridable Function PerpendicularTo(l As Line3d) As Point3d
        Dim r1 As Vector3d = Me.Point.ToVector
        Dim r2 As Vector3d = l.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim s2 As Vector3d = l.Direction
        If s1.Cross(s2).Norm > GeometRi3D.Tolerance Then
            r1 = r2 + (r2 - r1) * s1.Cross(s1.Cross(s2)) / (s1 * s2.Cross(s1.Cross(s2))) * s2
            Return r1.ToPoint
        Else
            Throw New Exception("Lines are parallel")
        End If
    End Function

    ''' <summary>
    ''' Get intersection of line with plane.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Line3d'.
    ''' </summary>
    Public Overridable Function IntersectionWith(s As Plane3d) As Object
        Dim r1 As Vector3d = Me.Point.ToVector
        Dim s1 As Vector3d = Me.Direction
        Dim n2 As Vector3d = s.Normal
        If Abs(s1 * n2) < GeometRi3D.Tolerance Then
            ' Line and plane are parallel
            If Me.Point.BelongsTo(s) Then
                ' Line lies in the plane
                Return Me
            Else
                Return Nothing
            End If
        Else
            ' Intersection point
            s.SetCoord(r1.Coord)
            r1 = r1 - ((r1 * n2) + s.D) / (s1 * n2) * s1
            Return r1.ToPoint
        End If
    End Function

    ''' <summary>
    ''' Get intersection of line with sphere.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Segment3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Sphere) As Object
        Return s.IntersectionWith(Me)
    End Function

    ''' <summary>
    ''' Get the orthogonal projection of a line to the plane.
    ''' Return object of type 'Line3d' or 'Point3d'
    ''' </summary>
    Public Overridable Function ProjectionTo(s As Plane3d) As Object
        Dim n1 As Vector3d = s.Normal
        Dim n2 As Vector3d = Me.Direction.Cross(n1)
        If n2.Norm < GeometRi3D.Tolerance Then
            ' Line is perpendicular to the plane
            Return Me.Point.ProjectionTo(s)
        Else
            Return New Line3d(Me.Point.ProjectionTo(s), n1.Cross(n2))
        End If
    End Function

#Region "AngleTo"
    ''' <summary>
    ''' Smalest angle between two lines in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(l As Line3d) As Double
        Dim ang As Double = Me.Direction.AngleTo(l)
        If ang <= PI / 2 Then
            Return ang
        Else
            Return PI - ang
        End If
    End Function
    ''' <summary>
    ''' Smalest angle between two lines in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(l As Line3d) As Double
        Return AngleTo(l) * 180 / PI
    End Function

    ''' <summary>
    ''' Smallest angle between line and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(s As Plane3d) As Double
        Dim ang As Double = Asin(Me.Direction.Dot(s.Normal) / Me.Direction.Norm / s.Normal.Norm)
        Return Abs(ang)
    End Function
    ''' <summary>
    ''' Smallest angle line and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(s As Plane3d) As Double
        Return AngleTo(s) * 180 / PI
    End Function
#End Region


#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate line by a vector
    ''' </summary>
    Public Overridable Function Translate(v As Vector3d) As Line3d
        Dim l As Line3d = Me.Clone
        l.Point = l.Point.Translate(v)
        Return l
    End Function

    ''' <summary>
    ''' Rotate line by a given rotation matrix
    ''' </summary>
    Public Overridable Function Rotate(ByVal m As Matrix3d) As Line3d
        Dim l As Line3d = Me.Clone
        l.Point = l.Point.Rotate(m)
        l.Direction = l.Direction.Rotate(m)
        Return l
    End Function

    ''' <summary>
    ''' Rotate line by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Overridable Function Rotate(m As Matrix3d, p As Point3d) As Line3d
        Dim l As Line3d = Me.Clone
        l.Point = l.Point.Rotate(m, p)
        l.Direction = l.Direction.Rotate(m)
        Return l
    End Function

    ''' <summary>
    ''' Reflect line in given point
    ''' </summary>
    Public Overridable Function ReflectIn(p As Point3d) As Line3d
        Return New Line3d(Me.Point.ReflectIn(p), Me.Direction.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect line in given line
    ''' </summary>
    Public Overridable Function ReflectIn(l As Line3d) As Line3d
        Return New Line3d(Me.Point.ReflectIn(l), Me.Direction.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect line in given plane
    ''' </summary>
    Public Overridable Function ReflectIn(s As Plane3d) As Line3d
        Return New Line3d(Me.Point.ReflectIn(s), Me.Direction.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim l As Line3d = CType(obj, Line3d)
        Return Me.Point.BelongsTo(l) AndAlso Me.Direction.IsParallelTo(l.Direction)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return GeometRi3D.HashFunction(_point.GetHashCode, _dir.GetHashCode)
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim str As New System.Text.StringBuilder
        Dim P As Point3d = _point.ConvertToGlobal
        Dim dir As Vector3d = _dir.ConvertToGlobal
        If coord IsNot Nothing Then
            P = _point.ConvertTo(coord)
            dir = _dir.ConvertTo(coord)
        End If
        str.Append("Line:" + vbCrLf)
        str.Append(String.Format("Point  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + vbCrLf)
        str.Append(String.Format("Direction -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", dir.X, dir.Y, dir.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    Public Shared Operator =(l1 As Line3d, l2 As Line3d) As Boolean
        Return l1.Equals(l2)
    End Operator
    Public Shared Operator <>(l1 As Line3d, l2 As Line3d) As Boolean
        Return Not l1.Equals(l2)
    End Operator

End Class
