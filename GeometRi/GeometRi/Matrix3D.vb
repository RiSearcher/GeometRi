Imports System.Math

Public Class Matrix3d

    Implements ICloneable

    Public val(2, 2) As Double

    Public Sub New()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                Me.val(i, j) = 0.0
            Next
        Next
    End Sub
    Public Sub New(ByVal r1 As Vector3d, ByVal r2 As Vector3d, ByVal r3 As Vector3d)
        Row1 = r1
        Row2 = r2
        Row3 = r3
    End Sub
    Public Sub New(ByVal v1() As Double, ByVal v2() As Double, ByVal v3() As Double)
        Row1 = New Vector3d(v1)
        Row2 = New Vector3d(v2)
        Row3 = New Vector3d(v3)
    End Sub

    Public Shared Function Identity() As Matrix3d
        Dim I As Matrix3d = New Matrix3d()
        I(0, 0) = 1.0
        I(1, 1) = 1.0
        I(2, 2) = 1.0
        Return I
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newm As Matrix3d = DirectCast(MemberwiseClone(), Matrix3d)
        newm.val = newm.val.Clone
        Return newm
    End Function

    Default Public Property Item(i As Integer, j As Integer) As Double
        Get
            Return val(i, j)
        End Get
        Set(value As Double)
            val(i, j) = value
        End Set
    End Property

    Public Property Row1 As Vector3d
        Get
            Return New Vector3d(val(0, 0), val(0, 1), val(0, 2))
        End Get
        Set(value As Vector3d)
            val(0, 0) = value.X
            val(0, 1) = value.Y
            val(0, 2) = value.Z
        End Set
    End Property
    Public Property Row2 As Vector3d
        Get
            Return New Vector3d(val(1, 0), val(1, 1), val(1, 2))
        End Get
        Set(value As Vector3d)
            val(1, 0) = value.X
            val(1, 1) = value.Y
            val(1, 2) = value.Z
        End Set
    End Property
    Public Property Row3 As Vector3d
        Get
            Return New Vector3d(val(2, 0), val(2, 1), val(2, 2))
        End Get
        Set(value As Vector3d)
            val(2, 0) = value.X
            val(2, 1) = value.Y
            val(2, 2) = value.Z
        End Set
    End Property

    Public Property Column1 As Vector3d
        Get
            Return New Vector3d(val(0, 0), val(1, 0), val(2, 0))
        End Get
        Set(value As Vector3d)
            val(0, 0) = value.X
            val(1, 0) = value.Y
            val(2, 0) = value.Z
        End Set
    End Property
    Public Property Column2 As Vector3d
        Get
            Return New Vector3d(val(0, 1), val(1, 1), val(2, 1))
        End Get
        Set(value As Vector3d)
            val(0, 1) = value.X
            val(1, 1) = value.Y
            val(2, 1) = value.Z
        End Set
    End Property
    Public Property Column3 As Vector3d
        Get
            Return New Vector3d(val(0, 2), val(1, 2), val(2, 2))
        End Get
        Set(value As Vector3d)
            val(0, 2) = value.X
            val(1, 2) = value.Y
            val(2, 2) = value.Z
        End Set
    End Property

    ''' <summary>
    ''' Determinant of the matrix
    ''' </summary>
    Public ReadOnly Property Det As Double
        Get
            Det = val(0, 0) * (val(1, 1) * val(2, 2) - val(1, 2) * val(2, 1)) -
                      val(0, 1) * (val(1, 0) * val(2, 2) - val(1, 2) * val(2, 0)) +
                      val(0, 2) * (val(1, 0) * val(2, 1) - val(1, 1) * val(2, 0))
        End Get
    End Property

    Public ReadOnly Property MaxNorm As Double
        Get
            MaxNorm = Max(Max(Abs(val(0, 0)), Abs(val(0, 1))), Abs(val(0, 2)))
            MaxNorm = Max(MaxNorm, Max(Max(Abs(val(1, 0)), Abs(val(1, 1))), Abs(val(1, 2))))
            MaxNorm = Max(MaxNorm, Max(Max(Abs(val(2, 0)), Abs(val(2, 1))), Abs(val(2, 2))))
        End Get
    End Property

    Public ReadOnly Property IsZero As Boolean
        Get
            Return Me = New Matrix3d
        End Get
    End Property

    Public ReadOnly Property IsIdentity As Boolean
        Get
            Return Me = Matrix3d.Identity
        End Get
    End Property

    Public ReadOnly Property IsOrthogonal As Boolean
        Get
            Return Me.Transpose * Me = Matrix3d.Identity
        End Get
    End Property

    Public Function Add(ByVal a As Double) As Matrix3d
        Dim B As Matrix3d = New Matrix3d()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                B.val(i, j) = Me.val(i, j) + a
            Next
        Next
        Return B
    End Function
    Public Function Add(ByVal a As Matrix3d) As Matrix3d
        Dim B As Matrix3d = New Matrix3d()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                B.val(i, j) = Me.val(i, j) + a.val(i, j)
            Next
        Next
        Return B
    End Function
    Public Function Subtract(ByVal a As Matrix3d) As Matrix3d
        Dim B As Matrix3d = New Matrix3d()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                B.val(i, j) = Me.val(i, j) - a.val(i, j)
            Next
        Next
        Return B
    End Function

    Public Function Mult(ByVal a As Double) As Matrix3d
        Dim B As Matrix3d = New Matrix3d()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                B.val(i, j) = a * Me.val(i, j)
            Next
        Next
        Return B
    End Function
    Public Function Mult(ByVal a As Vector3d) As Vector3d
        Dim b As Vector3d = New Vector3d(0, 0, 0)
        b(0) = val(0, 0) * a(0) + val(0, 1) * a(1) + val(0, 2) * a(2)
        b(1) = val(1, 0) * a(0) + val(1, 1) * a(1) + val(1, 2) * a(2)
        b(2) = val(2, 0) * a(0) + val(2, 1) * a(1) + val(2, 2) * a(2)
        Return b
    End Function
    Public Function Mult(ByVal p As Point3d) As Point3d
        Dim b As Point3d = New Point3d(0, 0, 0, p.Coord)
        b.X = val(0, 0) * p.X + val(0, 1) * p.Y + val(0, 2) * p.Z
        b.Y = val(1, 0) * p.X + val(1, 1) * p.Y + val(1, 2) * p.Z
        b.Z = val(2, 0) * p.X + val(2, 1) * p.Y + val(2, 2) * p.Z
        Return b
    End Function
    Public Function Mult(ByVal a As Matrix3d) As Matrix3d
        Dim B As Matrix3d = New Matrix3d()
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                For k As Integer = 0 To 2
                    B.val(i, j) = B.val(i, j) + Me.val(i, k) * a.val(k, j)
                Next
            Next
        Next
        Return B
    End Function

    Public Function Inverse() As Matrix3d
        Dim B As Matrix3d = New Matrix3d()

        Dim k11 As Double = val(2, 2) * val(1, 1) - val(2, 1) * val(1, 2)
        Dim k12 As Double = val(2, 1) * val(0, 2) - val(2, 2) * val(0, 1)
        Dim k13 As Double = val(1, 2) * val(0, 1) - val(1, 1) * val(0, 2)
        Dim k21 As Double = val(2, 0) * val(1, 2) - val(2, 2) * val(1, 0)
        Dim k22 As Double = val(2, 2) * val(0, 0) - val(2, 0) * val(0, 2)
        Dim k23 As Double = val(1, 0) * val(0, 2) - val(1, 2) * val(0, 0)
        Dim k31 As Double = val(2, 1) * val(1, 0) - val(2, 0) * val(1, 1)
        Dim k32 As Double = val(2, 0) * val(0, 1) - val(2, 1) * val(0, 0)
        Dim k33 As Double = val(1, 1) * val(0, 0) - val(1, 0) * val(0, 1)

        Dim det As Double = val(0, 0) * k11 + val(1, 0) * k12 + val(2, 0) * k13

        If det <> 0.0 Then
            B.val(0, 0) = k11 / det
            B.val(0, 1) = k12 / det
            B.val(0, 2) = k13 / det

            B.val(1, 0) = k21 / det
            B.val(1, 1) = k22 / det
            B.val(1, 2) = k23 / det

            B.val(2, 0) = k31 / det
            B.val(2, 1) = k32 / det
            B.val(2, 2) = k33 / det
        Else
            Throw New Exception("Matrix is singular")
        End If

        Return B
    End Function

    Public Function Transpose() As Matrix3d
        Dim T As Matrix3d = Me.Clone
        T(0, 1) = Me(1, 0)
        T(0, 2) = Me(2, 0)
        T(1, 0) = Me(0, 1)
        T(1, 2) = Me(2, 1)
        T(2, 0) = Me(0, 2)
        T(2, 1) = Me(1, 2)
        Return T
    End Function

    Public Shared Function RotationMatrix(axis As Vector3d, alpha As Double) As Matrix3d
        Dim R As Matrix3d = New Matrix3d()
        Dim v As Vector3d = axis.Normalized
        Dim c As Double = Cos(alpha)
        Dim s As Double = Sin(alpha)

        R(0, 0) = c + v.X ^ 2 * (1 - c)
        R(0, 1) = v.X * v.Y * (1 - c) - v.Z * s
        R(0, 2) = v.X * v.Z * (1 - c) + v.Y * s

        R(1, 0) = v.Y * v.X * (1 - c) + v.Z * s
        R(1, 1) = c + v.Y ^ 2 * (1 - c)
        R(1, 2) = v.Y * v.Z * (1 - c) - v.X * s

        R(2, 0) = v.Z * v.X * (1 - c) - v.Y * s
        R(2, 1) = v.Z * v.Y * (1 - c) + v.X * s
        R(2, 2) = c + v.Z ^ 2 * (1 - c)

        Return R
    End Function

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim m As Matrix3d = CType(obj, Matrix3d)
        Return (Me - m).MaxNorm < GeometRi3D.Tolerance
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return GeometRi3D.HashFunction(Row1.GetHashCode, Row2.GetHashCode, Row3.GetHashCode)
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("Row1 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", val(0, 0), val(0, 1), val(0, 2)) +
                vbCrLf +
                   String.Format("Row2 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", val(1, 0), val(1, 1), val(1, 2)) +
                vbCrLf +
                   String.Format("Row3 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", val(2, 0), val(2, 1), val(2, 2))
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    ' "+"
    Public Shared Operator +(m As Matrix3d, a As Matrix3d) As Matrix3d
        Return m.Add(a)
    End Operator
    ' "-"
    Public Shared Operator -(m As Matrix3d) As Matrix3d
        Return m.Mult(-1.0)
    End Operator
    Public Shared Operator -(m As Matrix3d, a As Matrix3d) As Matrix3d
        Return m.Subtract(a)
    End Operator
    ' "*"
    Public Shared Operator *(m As Matrix3d, a As Double) As Matrix3d
        Return m.Mult(a)
    End Operator
    Public Shared Operator *(a As Double, m As Matrix3d) As Matrix3d
        Return m.Mult(a)
    End Operator
    Public Shared Operator *(m As Matrix3d, a As Vector3d) As Vector3d
        Return m.Mult(a)
    End Operator
    Public Shared Operator *(m As Matrix3d, p As Point3d) As Point3d
        Return m.Mult(p)
    End Operator
    Public Shared Operator *(m As Matrix3d, a As Matrix3d) As Matrix3d
        Return m.Mult(a)
    End Operator

    Public Shared Operator =(m1 As Matrix3d, m2 As Matrix3d) As Boolean
        Return m1.Equals(m2)
    End Operator
    Public Shared Operator <>(m1 As Matrix3d, m2 As Matrix3d) As Boolean
        Return Not m1.Equals(m2)
    End Operator

End Class
