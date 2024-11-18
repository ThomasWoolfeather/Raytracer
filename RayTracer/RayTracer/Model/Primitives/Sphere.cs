using OpenTK;
using System;

namespace RayTracer.Model.Primitives
{
    public class Sphere : Primitive
    {
        public Vector3 Position;

        public float Radius;

        public Sphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// Check wether the ray intersects this sphere.
        /// </summary>
        /// <param name="ray">The ray to check.</param>
        /// <returns>An intersection if one is found.</returns>
        public override Intersection Intersect(Ray ray)
        {
            Vector3 distance = Position - ray.Origin;
            float direction = Vector3.Dot(distance, ray.Direction);
            float t = 0;
            if (direction >= 0)
            {
                float disc = Radius * Radius - (Vector3.Dot(distance, distance) - direction * direction);
                if (disc >= 0)
                    t = direction - (float)Math.Sqrt(disc);
            }
            if (t == 0) return null;
            return new Intersection(t, this);
        }

        /// <summary>
        /// Get the normal vector of an intersected point on the surface of the sphere.
        /// </summary>
        /// <param name="position">
        /// The Location on the surface where the normal vector should be calculated for.
        /// </param>
        /// <returns>The normal.</returns>
        public override Vector3 GetNormal(Vector3 position)
        {
            return (position - Position).Normalized();
        }

        /// <summary>
        /// Set the material of this sphere.
        /// </summary>
        /// <param name="material">the material.</param>
        internal override void SetMaterial(Material material)
        {
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
