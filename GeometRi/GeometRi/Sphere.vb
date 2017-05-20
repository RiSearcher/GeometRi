Imports System.Math

Public Class Sphere

    Implements ICloneable

    Private _point As Point3d
    Private _r As Double

    Sub New(P As Point3d, R As Double)
        _point = P
        _r = R
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Sphere = DirectCast(MemberwiseClone(), Sphere)
        newobj.Center = newobj.Center.Clone
        Return newobj
    End Function

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

    Public Property Radius As Double
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

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim s As Sphere = CType(obj, Sphere)

        Return s.Center = Me.Center AndAlso Abs(s.Radius - Me.Radius) <= GeometRi3D.Tolerance
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
