using System;
using OpenTK;

namespace RayTracer.Model.Primitives
{
    public class Plane : Primitive
    {
        public Vector3 Normal;

        public float Offset;

        /// <summary>
        /// Create a plane defined by a normal and a origin.
        /// Note: Origin is the offset of the plane in the direction of the normal.
        /// </summary>
        /// <param name="normal">The normal of the plane.</param>
        /// <param name="origin">The offset of the plane.</param>
        public Plane(Vector3 normal, float origin)
        {
            Normal = normal;
            Offset = origin;
        }

        /// <summary>
        /// Check wether the ray intersects this plane.
        /// </summary>
        /// <param name="ray">The ray to check.</param>
        /// <returns>An intersection if one is found.</returns>
        public override Intersection Intersect(Ray ray)
        {
            //Get the dotproduct of the normal and direction.
            float dot = Vector3.Dot(Normal, ray.Direction);
            if (dot >= 0) return null; //no intersection
            return new Intersection((Vector3.Dot(Normal, ray.Origin) + Offset) / (-dot), this);
        }

        /// <summary>
        /// Get the normal vector of an intersected point on the surface of the plane.
        /// </summary>
        /// <param name="position">
        /// The Location on the surface where the normal vector should be calculated for.
        /// </param>
        /// <returns>The normal.</returns>
        public override Vector3 GetNormal(Vector3 position)
        {
            return Normal;
        }

        /// <summary>
        /// Set the material of this plane.
        /// </summary>
        /// <param name="material">the material.</param>
        internal override void SetMaterial(Material material)
        {
            this.material = material;
        }

        /// <summary>
        /// Get the texture at a position on the plane surface.
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
