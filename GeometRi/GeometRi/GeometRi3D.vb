Public MustInherit Class GeometRi3D

    Private Shared _tolerance As Double = 0.000000000001
    Protected _coord As Coord3d
    'Protected Shared ReadOnly _def_coord As Coord3d = Coord3d.GlobalCS

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


    Public ReadOnly Property Coord As Coord3d
        Get
            Coord = _coord
        End Get
    End Property
    Public ReadOnly Property CoordName As String
        Get
            CoordName = _coord._name
        End Get
    End Property

End Class
