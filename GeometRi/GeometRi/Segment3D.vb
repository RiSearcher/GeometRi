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
            Return New Line3d(_p1, _p2)
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

    ''' <summary>
    ''' Returns shortest distance between two segments
    ''' </summary>
    Public Function DistanceTo(s As Segment3d) As Double

        ' Algorithm by Dan Sunday
        ' http://geomalgorithms.com/a07-_distance.html

        Dim small As Double = 0.000000001

        Dim u As Vector3d = Me.ToVector
        Dim v As Vector3d = s.ToVector
        Dim w As Vector3d = New Vector3d(s.P1, Me.P1)

        Dim a As Double = u * u
        Dim b As Double = u * v
        Dim c As Double = v * v
        Dim d As Double = u * w
        Dim e As Double = v * w

        Dim DD As Double = a * c - b * b
        Dim sc, sN, sD, tc, tN, tD As Double
        sD = DD
        tD = DD

        If DD < small Then
            ' the lines are almost parallel, force using point Me.P1 to prevent possible division by 0.0 later
            sN = 0.0
            sD = 1.0
            tN = e
            tD = c
        Else
            ' get the closest points on the infinite lines
            sN = (b * e - c * d)
            tN = (a * e - b * d)
            If (sN < 0.0) Then
                ' sc < 0 => the s=0 edge Is visible
                sN = 0.0
                tN = e
                tD = c
            ElseIf (sN > sD) Then
                ' sc > 1  => the s=1 edge Is visible
                sN = sD
                tN = e + b
                tD = c
            End If
        End If

        If (tN < 0.0) Then
            ' tc < 0 => the t=0 edge Is visible
            tN = 0.0
            ' recompute sc for this edge
            If (-d < 0.0) Then
                sN = 0.0
            ElseIf (-d > a) Then
                sN = sD
            Else
                sN = -d
                sD = a
            End If
        ElseIf (tN > tD) Then
            ' tc > 1  => the t=1 edge Is visible
            tN = tD
            ' recompute sc for this edge
            If ((-d + b) < 0.0) Then
                sN = 0
            ElseIf ((-d + b) > a) Then
                sN = sD
            Else
                sN = (-d + b)
                sD = a
            End If
        End If

        ' finally do the division to get sc And tc
        sc = If(Abs(sN) < small, 0.0, sN / sD)
        tc = If(Abs(tN) < small, 0.0, tN / tD)

        ' get the difference of the two closest points
        Dim dP As Vector3d = w + (sc * u) - (tc * v)  ' =  S1(sc) - S2(tc)

        Return dP.Norm

    End Function

    ''' <summary>
    ''' Returns shortest distance from segment to ray
    ''' </summary>
    Public Function DistanceTo(r As Ray3d) As Double

        If Me.ToVector.IsParallelTo(r.Direction) Then Return Me.ToLine.DistanceTo(r.ToLine)

        If Me.ToLine.PerpendicularTo(r.ToLine).BelongsTo(r) AndAlso
                 r.ToLine.PerpendicularTo(Me.ToLine).BelongsTo(Me) Then
            Return Me.ToLine.DistanceTo(r.ToLine)
        End If

        Dim d1 As Double = Double.PositiveInfinity
        Dim d2 As Double = Double.PositiveInfinity
        Dim d3 As Double = Double.PositiveInfinity
        Dim flag As Boolean = False

        If r.Point.ProjectionTo(Me.ToLine).BelongsTo(Me) Then
            d1 = r.Point.DistanceTo(Me.ToLine)
            flag = True
        End If
        If Me.P1.ProjectionTo(r.ToLine).BelongsTo(r) Then
            d2 = Me.P1.DistanceTo(r.ToLine)
            flag = True
        End If
        If Me.P2.ProjectionTo(r.ToLine).BelongsTo(r) Then
            d3 = Me.P2.DistanceTo(r.ToLine)
            flag = True
        End If

        If flag Then Return Min(d1, Min(d2, d3))

        Return Min(Me.P1.DistanceTo(r.Point), Me.P2.DistanceTo(r.Point))

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
