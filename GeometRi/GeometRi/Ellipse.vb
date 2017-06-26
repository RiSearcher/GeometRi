Imports System.Math

Public Class Ellipse

    Implements ICloneable

    Private _point As Point3d
    Private _v1 As Vector3d
    Private _v2 As Vector3d

    Public Sub New(Center As Point3d, semiaxis_a As Vector3d, semiaxis_b As Vector3d)
        If (Not semiaxis_a.IsOrthogonalTo(semiaxis_b)) Then
            Throw New Exception("Semiaxes are not orthogonal")
        End If
        _point = Center.Clone
        If semiaxis_a.Norm >= semiaxis_b.Norm Then
            _v1 = semiaxis_a.Clone
            _v2 = semiaxis_b.Clone
        Else
            _v1 = semiaxis_b.Clone
            _v2 = semiaxis_a.Clone
        End If

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New Ellipse(_point.Clone, _v1.Clone, _v2.Clone)
    End Function

#Region "Properties"
    Public ReadOnly Property Center As Point3d
        Get
            Return _point.Clone
        End Get
    End Property

    Public ReadOnly Property MajorSemiaxis As Vector3d
        Get
            Return _v1.Clone
        End Get
    End Property

    Public ReadOnly Property MinorSemiaxis As Vector3d
        Get
            Return _v2.Clone
        End Get
    End Property

    Public ReadOnly Property Normal As Vector3d
        Get
            Return _v1.Cross(_v2)
        End Get
    End Property

    ''' <summary>
    ''' Length of the major semiaxis
    ''' </summary>
    Public ReadOnly Property A As Double
        Get
            Return _v1.Norm
        End Get
    End Property

    ''' <summary>
    ''' Length of the minor semiaxis
    ''' </summary>
    Public ReadOnly Property B As Double
        Get
            Return _v2.Norm
        End Get
    End Property

    ''' <summary>
    ''' Distance from center to focus
    ''' </summary>
    Public ReadOnly Property F As Double
        Get
            Return Sqrt(_v1.Norm ^ 2 - _v2.Norm ^ 2)
        End Get
    End Property

    ''' <summary>
    ''' First focus
    ''' </summary>
    Public ReadOnly Property F1 As Point3d
        Get
            Return _point.Translate(F * _v1.Normalized)
        End Get
    End Property

    ''' <summary>
    ''' Second focus
    ''' </summary>
    Public ReadOnly Property F2 As Point3d
        Get
            Return _point.Translate(-F * _v1.Normalized)
        End Get
    End Property

    ''' <summary>
    ''' Eccentricity of the ellipse
    ''' </summary>
    Public ReadOnly Property e As Double
        Get
            Return Sqrt(1 - _v2.Norm ^ 2 / _v1.Norm ^ 2)
        End Get
    End Property

    Public ReadOnly Property Area As Double
        Get
            Return PI * A * B
        End Get
    End Property

    ''' <summary>
    ''' Approximate circumference of the ellipse
    ''' </summary>
    Public ReadOnly Property Perimeter As Double
        Get
            Dim a As Double = _v1.Norm
            Dim b As Double = _v2.Norm
            Dim h As Double = (a - b) ^ 2 / (a + b) ^ 2
            Return PI * (a + b) * (1 + 3 * h / (10 + Sqrt(4 - 3 * h)))
        End Get
    End Property
#End Region

    ''' <summary>
    ''' Returns point on ellipse for given parameter 't' (0 &lt;= t &lt; 2Pi)
    ''' </summary>
    Public Function ParametricForm(t As Double) As Point3d

        Return _point + _v1.ToPoint * Cos(t) + _v2.ToPoint * Sin(t)

    End Function

    ''' <summary>
    ''' Orthogonal projection of the ellipse to plane
    ''' </summary>
    Public Function ProjectionTo(s As Plane3d) As Ellipse

        Dim c As Point3d = _point.ProjectionTo(s)
        Dim q As Point3d = _point.Translate(_v1).ProjectionTo(s)
        Dim p As Point3d = _point.Translate(_v2).ProjectionTo(s)

        Dim f1 = New Vector3d(c, p)
        Dim f2 = New Vector3d(c, q)

        Dim t0 As Double = 0.5 * Atan2(2 * f1 * f2, f1 * f1 - f2 * f2)
        Dim v1 As Vector3d = f1 * Cos(t0) + f2 * Sin(t0)
        Dim v2 As Vector3d = f1 * Cos(t0 + PI / 2) + f2 * Sin(t0 + PI / 2)

        Return New Ellipse(c, v1, v2)
    End Function

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate ellipse by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Ellipse
        Return New Ellipse(Me.Center.Translate(v), _v1, _v2)
    End Function

    ''' <summary>
    ''' Rotate ellipse by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Ellipse
        Return New Ellipse(Me.Center.Rotate(m), _v1.Rotate(m), _v2.Rotate(m))
    End Function

    ''' <summary>
    ''' Rotate ellipse by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Ellipse
        Return New Ellipse(Me.Center.Rotate(m, p), _v1.Rotate(m), _v2.Rotate(m))
    End Function

    ''' <summary>
    ''' Reflect ellipse in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Ellipse
        Return New Ellipse(Me.Center.ReflectIn(p), _v1.ReflectIn(p), _v2.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect ellipse in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Ellipse
        Return New Ellipse(Me.Center.ReflectIn(l), _v1.ReflectIn(l), _v2.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect ellipse in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Ellipse
        Return New Ellipse(Me.Center.ReflectIn(s), _v1.ReflectIn(s), _v2.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim e As Ellipse = CType(obj, Ellipse)

        If GeometRi3D.AlmostEqual(Me.A, Me.B) Then
            ' Ellipse is circle
            If GeometRi3D.AlmostEqual(e.A, e.B) Then
                ' Second ellipse also circle
                Return Me.Center = e.Center AndAlso
                       GeometRi3D.AlmostEqual(Me.A, e.A) AndAlso
                       e.Normal.IsParallelTo(Me.Normal)
            Else
                Return False
            End If
        Else
            Return Me.Center = e.Center AndAlso
                   GeometRi3D.AlmostEqual(Me.A, e.A) AndAlso
                   GeometRi3D.AlmostEqual(Me.B, e.B) AndAlso
                   e.MajorSemiaxis.IsParallelTo(Me.MajorSemiaxis) AndAlso
                   e.MinorSemiaxis.IsParallelTo(Me.MinorSemiaxis)
        End If
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return GeometRi3D.HashFunction(_point.GetHashCode, _v1.GetHashCode, _v2.GetHashCode)
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String

        Dim P As Point3d = _point.ConvertToGlobal
        Dim v1 As Vector3d = _v1.ConvertToGlobal
        Dim v2 As Vector3d = _v2.ConvertToGlobal
        If coord IsNot Nothing Then
            P = P.ConvertTo(coord)
            v1 = v1.ConvertTo(coord)
            v2 = v2.ConvertTo(coord)
        End If

        Dim str As String = String.Format("Ellipse: ") + vbCrLf
        str += String.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + vbCrLf
        str += String.Format("  Semiaxis A -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v1.X, v1.Y, v1.Z) + vbCrLf
        str += String.Format("  Semiaxis B -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v2.X, v2.Y, v2.Z) + vbCrLf
        Return str
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------

    Public Shared Operator =(c1 As Ellipse, c2 As Ellipse) As Boolean
        Return c1.Equals(c2)
    End Operator
    Public Shared Operator <>(c1 As Ellipse, c2 As Ellipse) As Boolean
        Return Not c1.Equals(c2)
    End Operator

End Class
