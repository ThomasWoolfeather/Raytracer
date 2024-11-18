using OpenTK;
using System;

namespace RayTracer.Model.Primitives
{
    public class Triangle : Primitive
    {
        public Vector3 Coordinate1, Coordinate2, Coordinate3, Normal;

        // These vectors are lines from coordinate 1 to 2 and 3.
        private Vector3 U, V;

        private readonly float epsilon = 0.0001f;

        /// <summary>
        /// Create a triangle defined by three points.
        /// Note: The normal is defined by clockwise coordinates.
        /// </summary>
        /// <param name="coordinate1"></param>
        /// <param name="coordinate2"></param>
        /// <param name="coordinate3"></param>
        public Triangle(Vector3 coordinate1, Vector3 coordinate2, Vector3 coordinate3)
        {
            Coordinate1 = coordinate1;
            Coordinate2 = coordinate2;
            Coordinate3 = coordinate3;
            U = Coordinate2 - Coordinate1;
            V = Coordinate3 - Coordinate1;
            Normal = GetNormal(new Vector3());
        }

        /// <summary>
        /// Get the normal vector of an intersected point on the surface of the triangle.
        /// </summary>
        /// <param name="position">
        /// The Location on the surface where the normal vector should be calculated for.
        /// </param>
        /// <returns>The normal.</returns>
        public override Vector3 GetNormal(Vector3 position)
        {
            return (Vector3.Cross(U, V)).Normalized();
        }

        /// <summary>
        /// Check wether the ray intersects this triangle.
        /// </summary>
        /// <param name="ray">The ray to check.</param>
        /// <returns>An intersection if one is found.</returns>
        public override Intersection Intersect(Ray ray)
        {
            //A Möller Trumbore implementation of ray triangle intersection
            Vector3 r = Vector3.Cross(ray.Direction, V);

            //Get the determinant.
            float determinant = Vector3.Dot(U, r);

            Vector3 s;
            if (determinant > 0)
                s = ray.Origin - Coordinate1;
            else
                return null;

            float u = Vector3.Dot(s, r);
            if ((u < 0) || (determinant < u))
                return null;

            Vector3 q = Vector3.Cross(s, U);

            float v = Vector3.Dot(ray.Direction, q);
            if ((v < 0) || (determinant < (u + v)))
                return null;

            float distance = Vector3.Dot(V, q);
            //Invert the determinant to calculate the distance to the intersection point.
            float invertedDeterminant = 1 / determinant;

            distance = distance * invertedDeterminant;

            return new Intersection(distance, this);
        }

        /// <summary>
        /// Set the material of this triangle.
        /// </summary>
        /// <param name="material">the material.</param>
        internal override void SetMaterial(Material material)
        {
            if (material.GetType() == typeof(Materials.Checkerboard))
                throw new Exception("A triangle can not have a checkerboard as material.");
            this.material = material;
        }

        /// <summary>
        /// Get the texture at a position on the primitive surface.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The texture at that position.</returns>
        public override Vector3 GetTexture(Vector3 position)
        {
            Vector3 Color = Material.Diffuse(position);
            return Color;
        }
    }
}
