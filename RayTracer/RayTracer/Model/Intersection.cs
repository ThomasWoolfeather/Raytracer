using System;

namespace RayTracer.Model
{
    public class Intersection
    {
        public float Distance;

        public Primitive Primitive;

        public Ray ReflectedRay;

        /// <summary>
        /// Create an intersection.
        /// </summary>
        /// <param name="distance">
        /// The distance between the origin of the ray and the point of intersection.
        /// </param>
        /// <param name="primitive">The primitive that is hit.</param>
        public Intersection(float distance, Primitive primitive)
        {
            Distance = distance;
            Primitive = primitive;
        }
    }
}
