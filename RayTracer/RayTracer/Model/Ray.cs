using System;
using OpenTK;

namespace RayTracer.Model
{
    public class Ray
    {
        public Vector3 Origin, Direction;

        public Intersection Intersection;

        /// <summary>
        /// Create a ray.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The direction of the ray.</param>
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction.Normalized();
        }
    }
}
