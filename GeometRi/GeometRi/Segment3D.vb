Imports System.Math

Public Class Segment3d

    Implements ICloneable

    Private _p1 As Point3d
    Private _p2 As Point3d

    Sub New(p1 As Point3d, p2 As Point3d)
        _p1 = p1.Clone
        _p2 = p2.ConvertTo(p1.Coord)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newobj As Segment3d = DirectCast(MemberwiseClone(), Segment3d)
        newobj.P1 = newobj.P1.Clone
        newobj.P2 = newobj.P2.Clone
        Return newobj
    End Function

    Public Property P1 As Point3d
        Get
            Return _p1.Clone
        End Get
        Set(value As Point3d)
            _p1 = value.Clone
        End Set
    End Property

    Public Property P2 As Point3d
        Get
            Return _p2.Clone
        End Get
        Set(value As Point3d)
            _p2 = value.Clone
        End Set
    End Property

    Public ReadOnly Property Length As Double
        Get
            Return _p1.DistanceTo(_p2)
        End Get
    End Property

    Public ReadOnly Property ToVector As Vector3d
        Get
            Return New Vector3d(_p1, _p2)
        End Get
    End Property

    Public ReadOnly Property ToRay As Ray3d
        Get
            Return New Ray3d(_p1, New Vector3d(_p1, _p2))
        End Get
    End Property

    Public ReadOnly Property ToLine As Line3d
        Get
            Return New Line3d(_p1, New Vector3d(_p1, _p2))
        End Get
    End Property

#Region "DistanceTo"
    ''' <summary>
    ''' Returns shortest distance from segment to the point
    ''' </summary>
    Public Function DistanceTo(p As Point3d) As Double
        Return p.DistanceTo(Me)
    End Function

    ''' <summary>
    ''' Returns shortest distance from segment to the plane
    ''' </summary>
    Public Function DistanceTo(s As Plane3d) As Double

        Dim obj As Object = Me.IntersectionWith(s)

        If obj Is Nothing Then
            Return Min(Me.P1.DistanceTo(s), Me.P2.DistanceTo(s))
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Returns shortest distance from segment to the line
    ''' </summary>
    Public Function DistanceTo(l As Line3d) As Double
        If l.PerpendicularTo(Me.ToLine).BelongsTo(Me) Then
            Return l.DistanceTo(Me.ToLine)
        Else
            Return Min(Me.P1.DistanceTo(l), Me.P2.DistanceTo(l))
        End If
    End Function
#End Region

    ''' <summary>
    ''' Get intersection of segment with plane.
    ''' Returns object of type 'Nothing', 'Point3d' or 'Segment3d'.
    ''' </summary>
    Public Function IntersectionWith(s As Plane3d) As Object

        Dim obj As Object = Me.ToRay.IntersectionWith(s)

        If obj Is Nothing Then
            Return Nothing
        Else
            If obj.GetType Is GetType(Ray3d) Then
                Return Me
            Else
                Dim r As New Ray3d(Me.P2, New Vector3d(Me.P2, Me.P1))
                Dim obj2 As Object = r.IntersectionWith(s)
                If obj2 Is Nothing Then
                    Return Nothing
                Else
                    Return CType(obj2, Point3d)
                End If
            End If
        End If
    End Function

    ''' <summary>
    ''' Get the orthogonal projection of a segment to the line.
    ''' Return object of type 'Segment3d' or 'Point3d'
    ''' </summary>
    Public Function ProjectionTo(l As Line3d) As Object
        If Me.ToVector.IsOrthogonalTo(l.Direction) Then
            ' Segment is perpendicular to the line
            Return Me.P1.ProjectionTo(l)
        Else
            Return New Segment3d(Me.P1.ProjectionTo(l), Me.P2.ProjectionTo(l))
        End If
    End Function

    ''' <summary>
    ''' Get the orthogonal projection of a segment to the plane.
    ''' Return object of type 'Segment3d' or 'Point3d'
    ''' </summary>
    Public Function ProjectionTo(s As Plane3d) As Object
        If Me.ToVector.IsParallelTo(s.Normal) Then
            ' Segment is perpendicular to the plane
            Return Me.P1.ProjectionTo(s)
        Else
            Return New Segment3d(Me.P1.ProjectionTo(s), Me.P2.ProjectionTo(s))
        End If
    End Function

#Region "AngleTo"
    ''' <summary>
    ''' Angle between segment and plane in radians (0 &lt; angle &lt; Pi/2)
    ''' </summary>
    Public Function AngleTo(s As Plane3d) As Double
        Dim ang As Double = Asin(Me.ToVector.Dot(s.Normal) / Me.ToVector.Norm / s.Normal.Norm)
        Return Abs(ang)
    End Function
    ''' <summary>
    ''' Angle between segment and plane in degrees (0 &lt; angle &lt; 90)
    ''' </summary>
    Public Function AngleToDeg(s As Plane3d) As Double
        Return AngleTo(s) * 180 / PI
    End Function
#End Region


#Region "TranslateRotateReflect"
    ''' <summary>
    ''' Translate segment by a vector
    ''' </summary>
    Public Function Translate(v As Vector3d) As Segment3d
        Return New Segment3d(P1.Translate(v), P2.Translate(v))
    End Function

    ''' <summary>
    ''' Rotate segment by a given rotation matrix
    ''' </summary>
    Public Overridable Function Rotate(ByVal m As Matrix3d) As Segment3d
        Return New Segment3d(P1.Rotate(m), P2.Rotate(m))
    End Function

    ''' <summary>
    ''' Rotate segment by a given rotation matrix around point 'p' as a rotation center
    ''' </summary>
    Public Overridable Function Rotate(m As Matrix3d, p As Point3d) As Segment3d
        Return New Segment3d(P1.Rotate(m, p), P2.Rotate(m, p))
    End Function

    ''' <summary>
    ''' Reflect segment in given point
    ''' </summary>
    Public Overridable Function ReflectIn(p As Point3d) As Segment3d
        Return New Segment3d(P1.ReflectIn(p), P2.ReflectIn(p))
    End Function

    ''' <summary>
    ''' Reflect segment in given line
    ''' </summary>
    Public Overridable Function ReflectIn(l As Line3d) As Segment3d
        Return New Segment3d(P1.ReflectIn(l), P2.ReflectIn(l))
    End Function

    ''' <summary>
    ''' Reflect segment in given plane
    ''' </summary>
    Public Overridable Function ReflectIn(s As Plane3d) As Segment3d
        Return New Segment3d(P1.ReflectIn(s), P2.ReflectIn(s))
    End Function
#End Region

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim s As Segment3d = CType(obj, Segment3d)
        Return (Me.P1 = s.P1 AndAlso Me.P2 = s.P2) Or (Me.P1 = s.P2 AndAlso Me.P2 = s.P1)
    End Function

    Public Overloads Function ToString(Optional coord As Coord3d = Nothing) As String
        Dim str As New System.Text.StringBuilder
        Dim p1 As Point3d = _p1.ConvertToGlobal
        Dim p2 As Point3d = _p2.ConvertToGlobal
        If coord IsNot Nothing Then
            p1 = _p1.ConvertTo(coord)
            p2 = _p2.ConvertTo(coord)
        End If
        str.Append("Segment:" + vbCrLf)
        str.Append(String.Format("Point 1  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p1.X, p1.Y, p1.Z) + vbCrLf)
        str.Append(String.Format("Point 2 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p2.X, p2.Y, p2.Z))
        Return str.ToString
    End Function

    ' Operators overloads
    '-----------------------------------------------------------------
    Public Overloads Shared Operator =(l1 As Segment3d, l2 As Segment3d) As Boolean
        Return l1.Equals(l2)
    End Operator
    Public Overloads Shared Operator <>(l1 As Segment3d, l2 As Segment3d) As Boolean
        Return Not l1.Equals(l2)
    End Operator

End Class
