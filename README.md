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
