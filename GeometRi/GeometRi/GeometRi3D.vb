Public MustInherit Class GeometRi3D

    Private Shared _tolerance As Double = 0.000000000001

    ''' <summary>
    ''' Tolerance used for comparison operations (default 1e-12)
    ''' </summary>
    Public Shared Property Tolerance As Double
        Get
            Return _tolerance
        End Get
        Set(value As Double)
            _tolerance = value
        End Set
    End Property

    ''' <summary>
    ''' Tolerance based equality check
    ''' </summary>
    Public Shared Function AlmostEqual(a As Double, b As Double) As Boolean
        If Math.Abs(a - b) <= _tolerance Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Tolerance based unequality check
    ''' </summary>
    Public Shared Function NotEqual(a As Double, b As Double) As Boolean
        If Math.Abs(a - b) > _tolerance Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Tolerance based comparison
    ''' </summary>
    Public Shared Function Greater(a As Double, b As Double) As Boolean
        If (a - b) > _tolerance Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Tolerance based comparison
    ''' </summary>
    Public Shared Function Smaller(a As Double, b As Double) As Boolean
        If (a - b) < -_tolerance Then
            Return True
        Else
            Return False
        End If
    End Function

    Friend Shared Function HashFunction(n1 As Integer, n2 As Integer) As Integer
        Return (n1 << 4) Xor (n1 >> 28) Xor n2
    End Function

    Friend Shared Function HashFunction(n1 As Integer, n2 As Integer, n3 As Integer) As Integer
        n1 = (n1 << 4) Xor (n1 >> 28) Xor n2
        Return (n1 << 4) Xor (n1 >> 28) Xor n3
    End Function

    Friend Shared Function HashFunction(n1 As Integer, n2 As Integer, n3 As Integer, n4 As Integer) As Integer
        n1 = (n1 << 4) Xor (n1 >> 28) Xor n2
        n1 = (n1 << 4) Xor (n1 >> 28) Xor n3
        Return (n1 << 4) Xor (n1 >> 28) Xor n4
    End Function


End Class
