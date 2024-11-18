using System;
using OpenTK;

namespace RayTracer.Model.Primitives
{
    public class Quad : Primitive
    {
        private Triangle triangle1, triangle2;

        /// <summary>
        /// Create a quad defined by four points.
        /// Note: The normal is defined by clockwise coordinates.
        /// </summary>
        /// <param name="coordinate1">The first coordinate.</param>
        /// <param name="coordinate2">The first coordinate.</param>
        /// <param name="coordinate3">The first coordinate.</param>
        /// <param name="coordinate4">The first coordinate.</param>
        public Quad(Vector3 coordinate1, Vector3 coordinate2, Vector3 coordinate3, Vector3 coordinate4)
        {
            //Imagine a quad ABCD consisting of 2 triangles;
            // A --- B     where    A = coordinate 1
            // |   / |              B = coordinate 2
            // | /   |              C = coordinate 3
            // C --- D              D = coordinate 4

            triangle1 = new Triangle(coordinate1, coordinate2, coordinate3);
            triangle2 = new Triangle(coordinate2, coordinate4, coordinate3);
        }

        /// <summary>
        /// Get the normal vector of an intersected point on the surface of the quad.
        /// </summary>
        /// <param name="position">
        /// The Location on the surface where the normal vector should be calculated for.
        /// </param>
        /// <returns>The normal.</returns>
        public override Vector3 GetNormal(Vector3 position)
        {
            return (triangle1.Normal + triangle2.Normal) * 0.5f;
        }

        /// <summary>
        /// Get the texture at a position on the Quad surface.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The texture at that position.</returns>
        public override Vector3 GetTexture(Vector3 position)
        {
            Vector3 Color = Material.Diffuse(position);
            return Color;
        }

        /// <summary>
        /// Check wether the ray intersects this quad.
        /// </summary>
        /// <param name="ray">The ray to check.</param>
        /// <returns>An intersection if one is found.</returns>
        public override Intersection Intersect(Ray ray)
        {
            Intersection i = triangle1.Intersect(ray);
            if (i == null)
                i = triangle2.Intersect(ray);
            return i;
        }

        /// <summary>
        /// Set the material of this primitive.
        /// Note: This is required to prevent a certain material + primitive matchup.
        /// </summary>
        /// <param name="material">the material.</param>
        internal override void SetMaterial(Material material)
        {
            triangle1.Material = material;
            triangle2.Material = material;
        }
    }
}
