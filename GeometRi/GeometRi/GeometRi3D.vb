Public MustInherit Class GeometRi3D

    Private Shared _tolerance As Double = 0.000000000001
    Protected _coord As Coord3d

    ''' <summary>
    ''' Tolerance used for comparison operations
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


    Public ReadOnly Property Coord As Coord3d
        Get
            Coord = _coord
        End Get
    End Property
    Public ReadOnly Property CoordName As String
        Get
            CoordName = _coord.Name
        End Get
    End Property

End Class
