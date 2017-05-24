Imports System.Math

Public Class Circle3d

    Implements ICloneable

    Private _point As Point3d
    Private _r As Double
    Private _normal As Vector3d

    Sub New(Center As Point3d, Radius As Double, Normal As Vector3d)
        _point = Center
        _r = Radius
        _normal = Normal
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
    Public Property Radius As Double
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

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim c As Circle3d = CType(obj, Circle3d)

        Return c.Center = Me.Center AndAlso
               Abs(c.Radius - Me.Radius) <= GeometRi3D.Tolerance AndAlso
               c.Normal = Me.Normal
    End Function

    Public Overloads Function ToString() As String
        Dim str As String = String.Format("Circle: ") + vbCrLf
        str += String.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _point.X, _point.Y, _point.Z) + vbCrLf
        str += String.Format("  Radius -> {0,10:g5}", _r) + vbCrLf
        str += String.Format("  Normal -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _normal.X, _normal.Y, _normal.Z)
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
