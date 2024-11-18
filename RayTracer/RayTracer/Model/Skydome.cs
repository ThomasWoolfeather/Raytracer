using OpenTK;
using RayTracer.Utils;
using System;

namespace RayTracer.Model
{
    public class Skydome
    {
        private Service.Surface texture;

        private float oneoverpi;

        /// <summary>
        /// Create a skydome.
        /// </summary>
        public Skydome()
        {
            texture = new Service.Surface("../../assets/skydome.png");
            oneoverpi = (float)(1 / Math.PI);
        }

        /// <summary>
        /// Gets the color of the skydome at a certain point, calculated by a direction.
        /// Note: Direction is enough since we're assuming the skydome is an infinite distance away
        ///       from our camera.
        /// </summary>
        /// <param name="Direction">The direction to look at.</param>
        /// <returns>The color of the skydome at a certain direction.</returns>
        public Vector3 GetColor(Vector3 Direction)
        {
            float r = oneoverpi * (float)Math.Asin(Direction.Y) / (float)Math.Sqrt(Direction.X * Direction.X + Direction.Z * Direction.Z);

            float u = Math.Abs(Direction.X * r);

            float v = -Math.Abs(Direction.Z * r);

            int x = (int)(u * texture.width) + texture.width / 2;
            int y = (int)(v * texture.height) + texture.height / 2;
            int position = x + y * texture.width;
            if (position < 0 || position >= texture.pixels.Length)
                return new Vector3();
            return Translator.RevertColor(texture.pixels[position]);
        }
    }
}
