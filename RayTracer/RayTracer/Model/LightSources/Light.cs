using System;
using OpenTK;

namespace RayTracer.Model.LightSources
{
    public class Light
    {
        public bool Active = true;

        public Vector3 Position;

        public Vector3 Color;

        /// <summary>
        /// Get the color at a certain position.
        /// Note: Color also describes the intensity of the light.
        /// </summary>
        /// <param name="position">The position, can be anything since this is a point light.</param>
        /// <returns>The light color.</returns>
        public virtual Vector3 GetColor(Vector3 position)
        {
            return Color;
        }
    }
}
