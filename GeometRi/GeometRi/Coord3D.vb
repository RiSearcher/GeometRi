Imports System.Math

Public Class Coord3d
    Implements ICloneable

    Private _origin As Point3d
    Private _axes As Matrix3d
    Private _name As String
    Private Shared count As Integer = 0
    Public Shared ReadOnly GlobalCS As Coord3d = New Coord3d("Global_CS")


#Region "Constructors"
    ''' <summary>
    ''' Create default coordinate system.
    ''' </summary>
    ''' <param name="name">Name of the coordinate system.</param>
    Public Sub New(Optional name As String = "")
        _origin = New Point3d(0, 0, 0)
        _axes = Matrix3d.Identity
        If (name <> "") Then
            _name = name
        Else
            _name = "Coord " + count.ToString
        End If
        count += 1
    End Sub

    ''' <summary>
    ''' Create coordinate system by origin and transformation matrix.
    ''' </summary>
    ''' <param name="p">Origin of the coordinate system.</param>
    ''' <param name="m">Transformation matrix.</param>
    ''' <param name="name">Name of the coordinate system.</param>
    Public Sub New(ByVal p As Point3d, ByVal m As Matrix3d, Optional name As String = "")
        If Not m.IsOrthogonal Then
            Throw New ArgumentException("The matrix is not orthogonal")
        End If

        _origin = p.ConvertToGlobal
        _axes = m.Clone
        If (name <> "") Then
            _name = name
        Else
            _name = "Coord " + count.ToString
        End If
        count += 1
    End Sub

    ''' <summary>
    ''' Create coordinate system by point and two vectors.
    ''' </summary>
    ''' <param name="p">Origin of the coordinate system.</param>
    ''' <param name="v1">Vector oriented along the X axis.</param>
    ''' <param name="v2">Vector in the XY plane.</param>
    ''' <param name="name">Name of the coordinate system.</param>
    Public Sub New(p As Point3d, v1 As Vector3d, v2 As Vector3d, Optional name As String = "")
        If v1.IsParallelTo(v2) Then
            Throw New Exception("Vectors are parallel")
        End If

        v1 = v1.ConvertToGlobal.Normalized
        Dim v3 As Vector3d = v1.Cross(v2).Normalized
        v2 = v3.Cross(v1).Normalized

        _origin = p.ConvertToGlobal
        _axes = New Matrix3d(v1, v2, v3)
        If (name <> "") Then
            _name = name
        Else
            _name = "Coord " + count.ToString
        End If
        count += 1
    End Sub

    ''' <summary>
    ''' Create coordinate system by point and two vectors (as Double())
    ''' </summary>
    ''' <param name="p">Origin of the coordinate system.</param>
    ''' <param name="d1">Vector oriented along the X axis.</param>
    ''' <param name="d2">Vector in the XY plane.</param>
    ''' <param name="name">Name of the coordinate system.</param>
    Public Sub New(p As Point3d, d1() As Double, d2() As Double, Optional name As String = "")
        Dim v1 As New Vector3d(d1)
        Dim v2 As New Vector3d(d2)
        If v1.IsParallelTo(v2) Then
            Throw New Exception("Vectors are parallel")
        End If

        v1 = v1.Normalized
        Dim v3 As Vector3d = v1.Cross(v2).Normalized
        v2 = v3.Cross(v1).Normalized

        _origin = p.ConvertToGlobal
        _axes = New Matrix3d(v1, v2, v3)
        If (name <> "") Then
            _name = name
        Else
            _name = "Coord " + count.ToString
        End If
        count += 1
    End Sub
#End Region

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Coord3d = DirectCast(MemberwiseClone(), Coord3d)
        newobj.Origin = newobj.Origin.Clone
        newobj.Axes = newobj.Axes.Clone
        newobj._name = "Coord " + count.ToString
        count += 1
        Return newobj
    End Function


    ''' <summary>
    ''' Get or Set the origin of the coordinate system
    ''' </summary>
    ''' <returns></returns>
    Public Property Origin As Point3d
        Get
            Return New Point3d(_origin.X, _origin.Y, _origin.Z)
        End Get
        Set(value As Point3d)
            _origin = value.ConvertToGlobal
        End Set
    End Property
    ''' <summary>
    ''' Get or Set unit vectors of the axes, stored as row-matrix(3x3)
    ''' </summary>
    ''' <returns></returns>
    Public Property Axes As Matrix3d
        Get
            Return _axes.Clone
        End Get
        Set(value As Matrix3d)
            If value.IsOrthogonal Then
                _axes = value.Clone
            Else
                Throw New ArgumentException("The matrix is not orthogonal")
            End If
        End Set
    End Property

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    ''' <summary>
    ''' Get total number of defined coordinate systems
    ''' </summary>
    Public Shared ReadOnly Property Counts As Integer
        Get
            Return count
        End Get
    End Property


    ''' <summary>
    ''' Get X-axis
    ''' </summary>
    Public ReadOnly Property Xaxis As Vector3d
        Get
            Return _axes.Row1
        End Get
    End Property
    ''' <summary>
    ''' Get Y-axis
    ''' </summary>
    Public ReadOnly Property Yaxis As Vector3d
        Get
            Return _axes.Row2
        End Get
    End Property
    ''' <summary>
    ''' Get Z-axis
    ''' </summary>
    Public ReadOnly Property Zaxis As Vector3d
        Get
            Return _axes.Row3
        End Get
    End Property

    ''' <summary>
    ''' XY plane in the current coordinate system
    ''' </summary>
    Public ReadOnly Property XY_plane As Plane3d
        Get
            Return New Plane3d(0, 0, 1, 0, Me)
        End Get
    End Property

    ''' <summary>
    ''' XZ plane in the current coordinate system
    ''' </summary>
    Public ReadOnly Property XZ_plane As Plane3d
        Get
            Return New Plane3d(0, 1, 0, 0, Me)
        End Get
    End Property

    ''' <summary>
    ''' YZ plane in the current coordinate system
    ''' </summary>
    Public ReadOnly Property YZ_plane As Plane3d
        Get
            Return New Plane3d(1, 0, 0, 0, Me)
        End Get
    End Property

    ''' <summary>
    ''' Rotate coordinate system around rotation axis
    ''' </summary>
    ''' <param name="axis">Rotation axis</param>
    ''' <param name="angle">Rotation angle (radians, counterclockwise)</param>
    Public Sub Rotate(axis As Vector3d, angle As Double)
        _axes = _axes * Matrix3d.RotationMatrix(axis.ConvertToGlobal, angle).Transpose
    End Sub

    ''' <summary>
    ''' Rotate coordinate system around rotation axis
    ''' </summary>
    ''' <param name="axis">Rotation axis</param>
    ''' <param name="angle">Rotation angle (degrees, counterclockwise)</param>
    Public Sub RotateDeg(axis As Vector3d, angle As Double)
        _axes = _axes * Matrix3d.RotationMatrix(axis.ConvertToGlobal, angle * PI / 180).Transpose
    End Sub


    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim cs As Coord3d = CType(obj, Coord3d)
        Return Me._name = cs._name
    End Function

    Public Overrides Function ToString() As String
        Dim str As New System.Text.StringBuilder
        str.Append("Coord3d: " + _name + vbCrLf)
        str.Append(String.Format("Origin -> X: {0,10:g5}, Y: {1,10:g5}, Z: {2,10:g5}", _origin.X, _origin.Y, _origin.Z) + vbCrLf)
        str.Append(String.Format("Xaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Xaxis.X, Xaxis.Y, Xaxis.Z) + vbCrLf)
        str.Append(String.Format("Yaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Yaxis.X, Yaxis.Y, Yaxis.Z) + vbCrLf)
        str.Append(String.Format("Zaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Zaxis.X, Zaxis.Y, Zaxis.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    Public Shared Operator =(c1 As Coord3d, c2 As Coord3d) As Boolean
        If c1 IsNot Nothing Then
            Return c1.Equals(c2)
        ElseIf c1 Is Nothing AndAlso c2 Is Nothing Then
            Return True
        Else
            Return False
        End If
    End Operator
    Public Shared Operator <>(c1 As Coord3d, c2 As Coord3d) As Boolean
        If c1 IsNot Nothing Then
            Return Not c1.Equals(c2)
        ElseIf c1 Is Nothing AndAlso c2 Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Operator

End Class
