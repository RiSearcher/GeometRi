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
#End Region

#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate ellipse by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Ellipse
        Return New Ellipse(Me.Center.Translate(v), _v1, _v2)
    End Function
#End Region


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

End Class
