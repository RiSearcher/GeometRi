Imports System.Math

Public Class Triangle

    Implements ICloneable

    Private _a As Point3d
    Private _b As Point3d
    Private _c As Point3d

    Public Sub New(A As Point3d, B As Point3d, C As Point3d)
        If Point3d.CollinearPoints(A, B, C) Then
            Throw New Exception("Collinear points")
        End If
        _a = A.Clone
        _b = B.Clone
        _c = C.Clone
    End Sub


    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Triangle = DirectCast(MemberwiseClone(), Triangle)
        newobj.A = newobj.A.Clone
        newobj.B = newobj.B.Clone
        newobj.C = newobj.C.Clone
        Return newobj
    End Function

#Region "Properties"
    ''' <summary>
    ''' First point of triangle
    ''' </summary>
    Public Property A As Point3d
        Get
            Return _a.Clone
        End Get
        Set(value As Point3d)
            If Point3d.CollinearPoints(value, _b, _c) Then
                Throw New Exception("Collinear points")
            End If
            _a = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Second point of triangle
    ''' </summary>
    Public Property B As Point3d
        Get
            Return _b.Clone
        End Get
        Set(value As Point3d)
            If Point3d.CollinearPoints(_a, value, _c) Then
                Throw New Exception("Collinear points")
            End If
            _b = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Third point of triangle
    ''' </summary>
    Public Property C As Point3d
        Get
            Return _c.Clone
        End Get
        Set(value As Point3d)
            If Point3d.CollinearPoints(_a, _b, value) Then
                Throw New Exception("Collinear points")
            End If
            _c = value.Clone
        End Set
    End Property

    ''' <summary>
    ''' Length of AB side
    ''' </summary>
    Public ReadOnly Property AB As Double
        Get
            Return _a.DistanceTo(_b)
        End Get
    End Property

    ''' <summary>
    ''' Length of AC side
    ''' </summary>
    Public ReadOnly Property AC As Double
        Get
            Return _a.DistanceTo(_c)
        End Get
    End Property

    ''' <summary>
    ''' Length of BC side
    ''' </summary>
    Public ReadOnly Property BC As Double
        Get
            Return _b.DistanceTo(_c)
        End Get
    End Property

    ''' <summary>
    ''' Perimeter of the triangle
    ''' </summary>
    Public ReadOnly Property Perimeter As Double
        Get
            Return AB + BC + AC
        End Get
    End Property

    ''' <summary>
    ''' Area of the triangle
    ''' </summary>
    Public ReadOnly Property Area As Double
        Get
            Dim v1 = New Vector3d(_a, _b)
            Dim v2 = New Vector3d(_a, _c)
            Return 0.5 * v1.Cross(v2).Norm
        End Get
    End Property

    ''' <summary>
    ''' Circumcircle of the triangle
    ''' </summary>
    Public ReadOnly Property Circumcircle As Circle3d
        Get
            Return New Circle3d(_a, _b, _c)
        End Get
    End Property

    ''' <summary>
    ''' Angle at the vertex A
    ''' </summary>
    Public ReadOnly Property Angle_A As Double
        Get
            Return New Vector3d(_a, _b).AngleTo(New Vector3d(_a, _c))
        End Get
    End Property

    ''' <summary>
    ''' Angle at the vertex B
    ''' </summary>
    Public ReadOnly Property Angle_B As Double
        Get
            Return New Vector3d(_b, _a).AngleTo(New Vector3d(_b, _c))
        End Get
    End Property

    ''' <summary>
    ''' Angle at the vertex C
    ''' </summary>
    Public ReadOnly Property Angle_C As Double
        Get
            Return New Vector3d(_c, _a).AngleTo(New Vector3d(_c, _b))
        End Get
    End Property

    ''' <summary>
    ''' Angle bisector at the vertex A
    ''' </summary>
    Public ReadOnly Property Bisector_A As Segment3d
        Get
            Dim p As Point3d = _b + (_c - _b) / (1 + AB / AC)
            Return New Segment3d(_a, p)
        End Get
    End Property

    ''' <summary>
    ''' Angle bisector at the vertex B
    ''' </summary>
    Public ReadOnly Property Bisector_B As Segment3d
        Get
            Dim p As Point3d = _a + (_c - _a) / (1 + AB / BC)
            Return New Segment3d(_b, p)
        End Get
    End Property

    ''' <summary>
    ''' Angle bisector at the vertex C
    ''' </summary>
    Public ReadOnly Property Bisector_C As Segment3d
        Get
            Dim p As Point3d = _a + (_b - _a) / (1 + AC / BC)
            Return New Segment3d(_c, p)
        End Get
    End Property

    ''' <summary>
    ''' Incenter of the triangle
    ''' </summary>
    Public ReadOnly Property Incenter As Point3d
        Get
            Return Bisector_A.ToLine.PerpendicularTo(Bisector_B.ToLine)
        End Get
    End Property

    ''' <summary>
    ''' Centroid of the triangle
    ''' </summary>
    Public ReadOnly Property Centroid As Point3d
        Get
            Return Median_A.ToLine.PerpendicularTo(Median_B.ToLine)
        End Get
    End Property

    ''' <summary>
    ''' Orthocenter of the triangle
    ''' </summary>
    Public ReadOnly Property Orthocenter As Point3d
        Get
            Return Altitude_A.ToLine.PerpendicularTo(Altitude_B.ToLine)
        End Get
    End Property

    ''' <summary>
    ''' Circumcenter of the triangle
    ''' </summary>
    Public ReadOnly Property Circumcenter As Point3d
        Get
            Return New Circle3d(_a, _b, _c).Center
        End Get
    End Property

    ''' <summary>
    ''' Incircle of the triangle
    ''' </summary>
    Public ReadOnly Property Incircle As Circle3d
        Get
            Dim p As Point3d = Bisector_A.ToLine.PerpendicularTo(Bisector_B.ToLine)
            Dim r As Double = 2 * Area / Perimeter
            Dim v As Vector3d = New Vector3d(_a, _b).Cross(New Vector3d(_a, _c))
            Return New Circle3d(p, r, v)
        End Get
    End Property

    ''' <summary>
    ''' Altitude at the vertex A
    ''' </summary>
    Public ReadOnly Property Altitude_A As Segment3d
        Get
            Dim p As Point3d = _a.ProjectionTo(New Segment3d(_b, _c).ToLine)
            Return New Segment3d(_a, p)
        End Get
    End Property

    ''' <summary>
    ''' Altitude at the vertex B
    ''' </summary>
    Public ReadOnly Property Altitude_B As Segment3d
        Get
            Dim p As Point3d = _b.ProjectionTo(New Segment3d(_a, _c).ToLine)
            Return New Segment3d(_b, p)
        End Get
    End Property

    ''' <summary>
    ''' Altitude at the vertex C
    ''' </summary>
    Public ReadOnly Property Altitude_C As Segment3d
        Get
            Dim p As Point3d = _c.ProjectionTo(New Segment3d(_a, _b).ToLine)
            Return New Segment3d(_c, p)
        End Get
    End Property

    ''' <summary>
    ''' Median at the vertex A
    ''' </summary>
    Public ReadOnly Property Median_A As Segment3d
        Get
            Return New Segment3d(_a, (_b + _c) / 2)
        End Get
    End Property

    ''' <summary>
    ''' Median at the vertex B
    ''' </summary>
    Public ReadOnly Property Median_B As Segment3d
        Get
            Return New Segment3d(_b, (_a + _c) / 2)
        End Get
    End Property

    ''' <summary>
    ''' Median at the vertex C
    ''' </summary>
    Public ReadOnly Property Median_C As Segment3d
        Get
            Return New Segment3d(_c, (_a + _b) / 2)
        End Get
    End Property
#End Region

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate triangle by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Triangle
        Return New Triangle(_a.Translate(v), _b.Translate(v), _c.Translate(v))
    End Function

    ''' <summary>
    ''' Rotate triangle by a given rotation matrix
    ''' </summary>
    Public Function Rotate(ByVal m As Matrix3d) As Triangle
        Return New Triangle(_a.Rotate(m), _b.Rotate(m), _c.Rotate(m))
    End Function

    ''' <summary>
    ''' Rotate triangle by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Function Rotate(m As Matrix3d, p As Point3d) As Triangle
        Return New Triangle(_a.Rotate(m, p), _b.Rotate(m, p), _c.Rotate(m, p))
    End Function

    ''' <summary>
    ''' Reflect triangle in given point
    ''' </summary>
    Public Function ReflectIn(p As Point3d) As Triangle
        Return New Triangle(_a.ReflectIn(p), _b.ReflectIn(p), _c.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect triangle in given line
    ''' </summary>
    Public Function ReflectIn(l As Line3d) As Triangle
        Return New Triangle(_a.ReflectIn(l), _b.ReflectIn(l), _c.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect triangle in given plane
    ''' </summary>
    Public Function ReflectIn(s As Plane3d) As Triangle
        Return New Triangle(_a.ReflectIn(s), _b.ReflectIn(s), _c.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim t As Triangle = CType(obj, Triangle)

        If (Me.A = t.A OrElse Me.A = t.B OrElse Me.A = t.C) AndAlso
           (Me.B = t.A OrElse Me.B = t.B OrElse Me.B = t.C) AndAlso
           (Me.C = t.A OrElse Me.C = t.B OrElse Me.C = t.C) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim str As New System.Text.StringBuilder
        Dim A, B, C As Point3d
        If coord IsNot Nothing Then
            A = _a.ConvertTo(coord)
            B = _b.ConvertTo(coord)
            C = _c.ConvertTo(coord)
        Else
            A = _a.ConvertToGlobal
            B = _b.ConvertToGlobal
            C = _c.ConvertToGlobal
        End If
        str.Append("Triangle:" + vbCrLf)
        str.Append(String.Format("A -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", A.X, A.Y, A.Z) + vbCrLf)
        str.Append(String.Format("B -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", B.X, B.Y, B.Z) + vbCrLf)
        str.Append(String.Format("C -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", C.X, C.Y, C.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------

    Public Shared Operator =(t1 As Triangle, t2 As Triangle) As Boolean
        Return t1.Equals(t2)
    End Operator
    Public Shared Operator <>(t1 As Triangle, t2 As Triangle) As Boolean
        Return Not t1.Equals(t2)
    End Operator
End Class
