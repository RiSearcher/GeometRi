## GeometRi
### Simple and lightweight computational geometry library for .Net

GeomtRi is a simple and lightweight 3D geometry library for .Net.
It is not one more 3D graphics library, its main job is manipulations with basic
geometrical primitives, such as point, line, plane, segment in 3D space:
translation and rotation operations, distance calculation, intersections,
orthogonal projections of one object into another, etc. The objects can be defined
in global or in one of the local coordinate systems and converted form one coordinate
system into another.

The library was build to be as simple and intuitive as posible. Users do not have to remember the reference coordinate
system of each object. The objects store the coordinate system they are defined in and all transformations
will be caried out implicitly when necessary.  

The main goal was simplisity and readability of the code, therefore speed and robustness was not a priority.
Global tolerance property is used for proximity checking, not an exact robust algorithms.

### Installation
Use NuGet to install library. Search for __GeometRi__ in NuGet package manager or type in the Package Manager Console:
```
Install-Package GeometRi
```

### Classes

* __Point3d__ and __Vector3d__ are two base classes, representing points and vectors in 3D space.
Objects of type Point3d or Vector3d can be defined in global or in local coordinate systems.

* __Line3d__, __Ray3d__, __Segment3d__ and __Plane3d__ are compound classes, which are defined in terms of points and vectors.

* __Coord3d__ and __Matrix3d__ are auxiliary classes.

* __GeometRi3d__ is an abstract class, which defines some common functionality, for example global tolerance property (GeometRi3d.Tolerance)
used in proximity operations by other classes.

#### Some common methods

* __Translate(Vector3d)__ - translate object by given vector.

* __Rotate(Matrix3d, [Point3d])__ - rotate object by given rotation matrix (optionally rotate around given point).

* __ReflectIn(obj)__ - reflect object in given point, line or plane.

* __DistanceTo(obj)__ - distance to other object (point, line or plane).

* __ProjectionTo(obj)__ - orthogonal projection of one object to another.

* __IntersectionWith(obj)__ - intersection of two objects.

* __AngleTo(obj)__ - angle between two objects (radians).

* __AngleToDeg(obj)__ - angle between two objects (degrees).

* __Clone()__ - creates copy of the object.

* __ConvertTo(coord)__ - convers point or vector to local cordinate system.

* __ConvertToGlobal()__ - convert point or vector to global coordinate system.

#### Point3d

One of the base classes, can be constructed by three double numbers (X, Y and Z) or from double array.
Each constructor has optional parameter 'coord' for local coordinate system in which point will be defined.
By default all points are defined in global coordinate system.

#### Vector3d

Second base class, representing vector in 3D space. Constructed by three components (X, Y and Z) or from double array
(with optional 'coord' parameter for local cordinate system). Additionally, can be constructed by point,
representing radius vector of that point, or by two points, representing vector from first point to another. In this cases
the vector will be defined in the same coordinate system as the first operand.

#### Line3d 

Represent infinite line  in 3D space and is defined by any point lying on the line and a direction vector.

#### Ray 3d

Represent ray in 3D space and is defined by starting point and direction vector.

#### Plane3d

Defined by arbutrary point on the plane and a normal vector. 
Optionally can be defined by coefficients in general equation of plane (Ax + By + Cz + D = 0), by three points
or by point and two vectors in the plane.

#### Coord3d

Class representing orthogonal cartesian 3D coordinate system. Defined by an origin point and transformation matrix
(three orthogonal unit vectors stored in row format). One global coordinate system (Coord3d.GlobalCS) is defined by default,
any number of local coordinate systems can be defined by users.