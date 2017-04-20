Imports System.Math

Public Class Vector3d
    Inherits GeometRi3D
    Implements ICloneable

    Public val(2) As Double

#Region "Constructors"
    Public Sub New(Optional coord As Coord3d = Nothing)
        Me.val(0) = 0.0
        Me.val(1) = 0.0
        Me.val(2) = 0.0
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub
    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, Optional coord As Coord3d = Nothing)
        Me.val(0) = X
        Me.val(1) = Y
        Me.val(2) = Z
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub
    Public Sub New(ByVal p As Point3d)
        Me.val(0) = p.X
        Me.val(1) = p.Y
        Me.val(2) = p.Z
        _coord = p.Coord
    End Sub
    Public Sub New(ByVal p1 As Point3d, ByVal p2 As Point3d)
        If p1.Coord <> p2.Coord Then p2 = p2.ConvertTo(p1.Coord)
        Me.val(0) = p2.X - p1.X
        Me.val(1) = p2.Y - p1.Y
        Me.val(2) = p2.Z - p1.Z
        _coord = p1.Coord
    End Sub
    Public Sub New(a() As Double, Optional coord As Coord3d = Nothing)
        If a.GetUpperBound(0) < 2 Then Throw New Exception("Vector3d: Array size mismatch")
        Me.val(0) = a(0)
        Me.val(1) = a(1)
        Me.val(2) = a(2)
        If coord IsNot Nothing Then
            _coord = coord
        Else
            _coord = Coord3d.GlobalCS
        End If
    End Sub
#End Region



    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newvec As Vector3d = DirectCast(MemberwiseClone(), Vector3d)
        newvec.val = newvec.val.Clone
        Return newvec
    End Function

    Default Public Property Item(i As Integer) As Double
        Get
            Return val(i)
        End Get
        Set(value As Double)
            val(i) = value
        End Set
    End Property

    Public Property X As Double
        Get
            Return val(0)
        End Get
        Set(value As Double)
            val(0) = value
        End Set
    End Property
    Public Property Y As Double
        Get
            Return val(1)
        End Get
        Set(value As Double)
            val(1) = value
        End Set
    End Property
    Public Property Z As Double
        Get
            Return val(2)
        End Get
        Set(value As Double)
            val(2) = value
        End Set
    End Property

    Public ReadOnly Property Norm As Double
        Get
            Return Sqrt(val(0) ^ 2 + val(1) ^ 2 + val(2) ^ 2)
        End Get
    End Property

    Public ReadOnly Property ToPoint As Point3d
        Get
            Return New Point3d(val(0), val(1), val(2), _coord)
        End Get
    End Property

    ''' <summary>
    ''' Normalize the current vector
    ''' </summary>
    Public Sub Normalize()
        Dim tmp As Double = 1.0 / Me.Norm
        val(0) = val(0) * tmp
        val(1) = val(1) * tmp
        val(2) = val(2) * tmp
    End Sub

    ''' <summary>
    ''' Returns normalized vector
    ''' </summary>
    ''' <returns></returns>
    Public Function Normalized() As Vector3d
        Dim tmp As Vector3d = Me.Clone
        Dim tmp_norm As Double = Me.Norm
        tmp.val(0) = val(0) / tmp_norm
        tmp.val(1) = val(1) / tmp_norm
        tmp.val(2) = val(2) / tmp_norm
        Return tmp
    End Function

    ''' <summary>
    ''' Check if two vectors are parallel
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Function IsParallelTo(v As Vector3d) As Boolean
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return Me.Cross(v).Norm < Tolerance
    End Function

    ''' <summary>
    ''' Check if two vectors are NOT parallel
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Function IsNotParallelTo(v As Vector3d) As Boolean
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return Me.Cross(v).Norm >= Tolerance
    End Function

    ''' <summary>
    ''' Check if two vectors are orthogonal
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Function IsOrthogonalTo(v As Vector3d) As Boolean
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return Abs(Me * v) < Tolerance
    End Function

    Public Function Add(ByVal a As Double) As Vector3d
        Dim tmp As Vector3d = Me.Clone
        tmp.val(0) += a
        tmp.val(1) += a
        tmp.val(2) += a
        Return tmp
    End Function
    Public Function Add(ByVal v As Vector3d) As Vector3d
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Dim tmp As Vector3d = Me.Clone
        tmp.val(0) += v.X
        tmp.val(1) += v.Y
        tmp.val(2) += v.Z
        Return tmp
    End Function
    Public Function Subtract(ByVal a As Double) As Vector3d
        Dim tmp As Vector3d = Me.Clone
        tmp.val(0) -= a
        tmp.val(1) -= a
        tmp.val(2) -= a
        Return tmp
    End Function
    Public Function Subtract(ByVal v As Vector3d) As Vector3d
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Dim tmp As Vector3d = Me.Clone
        tmp.val(0) -= v.X
        tmp.val(1) -= v.Y
        tmp.val(2) -= v.Z
        Return tmp
    End Function
    Public Function Mult(ByVal a As Double) As Vector3d
        Dim tmp As Vector3d = Me.Clone
        tmp.val(0) *= a
        tmp.val(1) *= a
        tmp.val(2) *= a
        Return tmp
    End Function

    Public Function Dot(ByVal v As Vector3d) As Double
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return Me.val(0) * v.val(0) + Me.val(1) * v.val(1) + Me.val(2) * v.val(2)
    End Function

    Public Function Cross(ByVal v As Vector3d) As Vector3d
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Dim tmp As Vector3d = New Vector3d(0, 0, 0, _coord)
        tmp.X = Me.Y * v.Z - Me.Z * v.Y
        tmp.Y = Me.Z * v.X - Me.X * v.Z
        tmp.Z = Me.X * v.Y - Me.Y * v.X
        Return tmp
    End Function

    Public Function ConvertTo(ByVal coord As Coord3d) As Vector3d
        ' Convert vector to local coordinate system
        Dim v1 As Vector3d = Me.Clone
        v1 = v1.ConvertToGlobal()
        If coord IsNot Nothing AndAlso coord IsNot Coord3d.GlobalCS Then
            v1 = coord.Axes * v1
            v1._coord = coord
        End If
        Return v1
    End Function
    Public Function ConvertToGlobal() As Vector3d
        ' Convert point from local coordinate system '_coord' to global system
        If _coord Is Nothing OrElse _coord Is Coord3d.GlobalCS Then
            Return Me
        Else
            Dim v As Vector3d = Me.Clone
            v = _coord.Axes.Inverse * v
            v._coord = Coord3d.GlobalCS
            Return v
        End If
    End Function

    ''' <summary>
    ''' Angle between two vectors in radians (0 &lt; angle &lt; Pi)
    ''' </summary>
    Public Function AngleTo(v As Vector3d) As Double
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return Acos(Me.Dot(v) / Me.Norm / v.Norm)
    End Function
    ''' <summary>
    ''' Angle between two vectors in degrees (0 &lt; angle &lt; 180)
    ''' </summary>
    Public Function AngleDegTo(v As Vector3d) As Double
        Return AngleTo(v) * 180 / PI
    End Function

    ''' <summary>
    ''' Angle between vector and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(s As Plane3d) As Double
        Dim ang As Double = Asin(Me.Dot(s.Normal) / Me.Norm / s.Normal.Norm)
        Return Abs(ang)
        'If ang <= PI / 2 Then
        '    Return ang
        'Else
        '    Return PI - ang
        'End If
    End Function
    ''' <summary>
    ''' Angle between vector and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleDegTo(s As Plane3d) As Double
        Return AngleTo(s) * 180 / PI
    End Function

    ''' <summary>
    ''' Returns projection of the current vector to the second vector
    ''' </summary>
    Public Function ProjectionTo(v As Vector3d) As Vector3d
        If (Me._coord <> v._coord) Then v = v.ConvertTo(Me._coord)
        Return (Me * v) / (v * v) * v
    End Function

#Region "RotateReflect"
    ''' <summary>
    ''' Rotate vector by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Vector3d
        Return m * Me
    End Function

    ''' <summary>
    ''' Reflect vector in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Vector3d
        Return -Me
    End Function

    ''' <summary>
    ''' Reflect vector in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Vector3d
        Dim p1 As Point3d = New Point3d(0, 0, 0, Me._coord)
        Dim p2 As Point3d = p1.Translate(Me)
        Return New Vector3d(p1.ReflectIn(l), p2.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect vector in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Vector3d
        Dim p1 As Point3d = New Point3d(0, 0, 0, Me._coord)
        Dim p2 As Point3d = p1.Translate(Me)
        Return New Vector3d(p1.ReflectIn(s), p2.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim v As Vector3d = CType(obj, Vector3d)
        If (Me._coord <> v.Coord) Then v = v.ConvertTo(_coord)
        Return Abs(Me.X - v.X) < Tolerance AndAlso Abs(Me.Y - v.Y) < Tolerance AndAlso Abs(Me.Y - v.Y) < Tolerance
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim v As Vector3d = Me.ConvertToGlobal
        If coord IsNot Nothing Then
            v = Me.ConvertTo(coord)
        End If
        Return String.Format("Vector3d -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v.X, v.Y, v.Z)
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    ' "+"
    Public Shared Operator +(v As Vector3d, a As Vector3d) As Vector3d
        Return v.Add(a)
    End Operator
    ' "-"
    Public Shared Operator -(v As Vector3d) As Vector3d
        Return v.Mult(-1.0)
    End Operator
    Public Shared Operator -(v As Vector3d, a As Vector3d) As Vector3d
        Return v.Subtract(a)
    End Operator
    ' "*"
    Public Shared Operator *(v As Vector3d, a As Double) As Vector3d
        Return v.Mult(a)
    End Operator
    Public Shared Operator *(a As Double, v As Vector3d) As Vector3d
        Return v.Mult(a)
    End Operator
    ''' <summary>
    ''' Dot product of two vectors
    ''' </summary>
    Public Shared Operator *(v As Vector3d, a As Vector3d) As Double
        Return v.Dot(a)
    End Operator

    Public Shared Operator =(v1 As Vector3d, v2 As Vector3d) As Boolean
        Return v1.Equals(v2)
    End Operator
    Public Shared Operator <>(v1 As Vector3d, v2 As Vector3d) As Boolean
        Return Not v1.Equals(v2)
    End Operator

End Class
