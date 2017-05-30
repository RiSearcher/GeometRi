Imports System.Math

Public Class Sphere

    Implements ICloneable

    Private _point As Point3d
    Private _r As Double

    Public Sub New(P As Point3d, R As Double)
        _point = P
        _r = R
    End Sub



    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Sphere = DirectCast(MemberwiseClone(), Sphere)
        newobj.Center = newobj.Center.Clone
        Return newobj
    End Function

#Region "Properties"
    ''' <summary>
    ''' Center of the sphere
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
    ''' X componente of the spheres center
    ''' </summary>
    Public Property X As Double
        Get
            Return _point.X
        End Get
        Set(value As Double)
            _point.X = value
        End Set
    End Property

    ''' <summary>
    ''' Y componente of the spheres center
    ''' </summary>
    Public Property Y As Double
        Get
            Return _point.Y
        End Get
        Set(value As Double)
            _point.Y = value
        End Set
    End Property

    ''' <summary>
    ''' Z componente of the spheres center
    ''' </summary>
    Public Property Z As Double
        Get
            Return _point.Z
        End Get
        Set(value As Double)
            _point.Z = value
        End Set
    End Property

    ''' <summary>
    ''' Radius of the sphere
    ''' </summary>
    Public Property R As Double
        Get
            Return _r
        End Get
        Set(value As Double)
            _r = value
        End Set
    End Property

    Public ReadOnly Property Area As Double
        Get
            Return 4.0 * PI * _r ^ 2
        End Get
    End Property

    Public ReadOnly Property Volume As Double
        Get
            Return 4.0 / 3.0 * PI * _r ^ 3
        End Get
    End Property
#End Region

#Region "DistaneTo"
    Public Function DistanceTo(p As Point3d) As Double
        Dim d As Double = p.DistanceTo(Me.Center)
        If d > Me.R Then
            Return d - Me.R
        Else
            Return 0
        End If
    End Function

    Public Function DistanceTo(l As Line3d) As Double
        Dim d As Double = l.DistanceTo(Me.Center)
        If d > Me.R Then
            Return d - Me.R
        Else
            Return 0
        End If
    End Function

    Public Function DistanceTo(r As Ray3d) As Double
        If Me.Center.ProjectionTo(r.ToLine).BelongsTo(r) Then
            Return Me.DistanceTo(r.ToLine)
        Else
            Return Me.DistanceTo(r.Point)
        End If
    End Function

    Public Function DistanceTo(s As Segment3d) As Double
        If Me.Center.ProjectionTo(s.ToLine).BelongsTo(s) Then
            Return Me.DistanceTo(s.ToLine)
        Else
            Return Min(Me.DistanceTo(s.P1), Me.DistanceTo(s.P2))
        End If
    End Function

    Public Function DistanceTo(s As Plane3d) As Double
        Dim d As Double = Me.Center.DistanceTo(s)
        If d > Me.R Then
            Return d - Me.R
        Else
            Return 0
        End If
    End Function
#End Region

#Region "Intersections"
    ''' <summary>
    ''' Get intersection of line with sphere.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Segment3d'.
    ''' </summary>
    Public Function IntersectionWith(l As Line3d) As Object

        Dim d As Double = l.Direction.Normalized * (l.Point.ToVector - Me.Center.ToVector)
        Dim det As Double = d ^ 2 - ((l.Point.ToVector - Me.Center.ToVector).Norm) ^ 2 + _r ^ 2

        If det < -GeometRi3D.Tolerance Then
            Return Nothing
        ElseIf det < GeometRi3D.Tolerance Then
            Return l.Point - d * l.Direction.Normalized.ToPoint
        Else
            Dim p1 As Point3d = l.Point + (-d + Sqrt(det)) * l.Direction.Normalized.ToPoint
            Dim p2 As Point3d = l.Point + (-d - Sqrt(det)) * l.Direction.Normalized.ToPoint
            Return New Segment3d(p1, p2)
        End If

    End Function

    ''' <summary>
    ''' Get intersection of plane with sphere.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Circle3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Plane3d) As Object

        s.SetCoord(Me.Center.Coord)
        Dim d1 As Double = s.A * Me.X + s.B * Me.Y + s.C * Me.Z + s.D
        Dim d2 As Double = s.A ^ 2 + s.B ^ 2 + s.C ^ 2
        Dim d As Double = Abs(d1) / Sqrt(d2)

        If d > Me.R + GeometRi3D.Tolerance Then
            Return Nothing
        Else
            Dim Xc As Double = Me.X - s.A * d1 / d2
            Dim Yc As Double = Me.Y - s.B * d1 / d2
            Dim Zc As Double = Me.Z - s.C * d1 / d2

            If Abs(d - Me.R) < GeometRi3D.Tolerance Then
                Return New Point3d(Xc, Yc, Zc, Me.Center.Coord)
            Else
                Dim R As Double = Sqrt(Me.R ^ 2 - d ^ 2)
                Return New Circle3d(New Point3d(Xc, Yc, Zc, Me.Center.Coord), R, s.Normal)
            End If
        End If
    End Function

    ''' <summary>
    ''' Get intersection of two spheres.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Circle3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Sphere) As Object

        Dim p As Point3d = s.Center.ConvertTo(Me.Center.Coord)
        Dim Dist As Double = Sqrt((Me.X - p.X) ^ 2 + (Me.Y - p.Y) ^ 2 + (Me.Z - p.Z) ^ 2)

        ' Separated spheres
        If Dist > Me.R + s.R + GeometRi3D.Tolerance Then Return Nothing

        ' One sphere inside the other
        If Dist < Abs(Me.R - s.R) - GeometRi3D.Tolerance Then Return Nothing

        ' Intersection plane
        Dim A As Double = 2 * (p.X - Me.X)
        Dim B As Double = 2 * (p.Y - Me.Y)
        Dim C As Double = 2 * (p.Z - Me.Z)
        Dim D As Double = Me.X ^ 2 - p.X ^ 2 + Me.Y ^ 2 - p.Y ^ 2 + Me.Z ^ 2 - p.Z ^ 2 - Me.R ^ 2 + s.R ^ 2

        ' Intersection center
        Dim t As Double = (Me.X * A + Me.Y * B + Me.Z * C + D) / (A * (Me.X - p.X) + B * (Me.Y - p.Y) + C * (Me.Z - p.Z))
        Dim x As Double = Me.X + t * (p.X - Me.X)
        Dim y As Double = Me.Y + t * (p.Y - Me.Y)
        Dim z As Double = Me.Z + t * (p.Z - Me.Z)

        ' Outer tangency
        If Abs(Me.R + s.R - D) < GeometRi3D.Tolerance Then Return New Point3d(x, y, z, Me.Center.Coord)

        ' Inner tangency
        If Abs(Abs(Me.R - s.R) - D) < GeometRi3D.Tolerance Then Return New Point3d(x, y, z, Me.Center.Coord)

        ' Intersection
        Dim alpha As Double = Acos((Me.R ^ 2 + Dist ^ 2 - s.R ^ 2) / (2 * Me.R * Dist))
        Dim R As Double = Me.R * Sin(alpha)
        Dim v As Vector3d = New Vector3d(Me.Center, s.Center)

        Return New Circle3d(New Point3d(x, y, z, Me.Center.Coord), R, v)

    End Function
#End Region


    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim s As Sphere = CType(obj, Sphere)

        Return s.Center = Me.Center AndAlso Abs(s.R - Me.R) <= GeometRi3D.Tolerance
    End Function

    Public Overloads Function ToString() As String
        Dim str As String = String.Format("Sphere: ") + vbCrLf
        str += String.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _point.X, _point.Y, _point.Z) + vbCrLf
        str += String.Format("  Radius -> {0,10:g5}", _r)
        Return str
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------

    Public Shared Operator =(s1 As Sphere, s2 As Sphere) As Boolean
        Return s1.Equals(s2)
    End Operator
    Public Shared Operator <>(s1 As Sphere, s2 As Sphere) As Boolean
        Return Not s1.Equals(s2)
    End Operator

End Class
