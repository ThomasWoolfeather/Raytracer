using OpenTK;

namespace RayTracer.Model
{
    public class Material
    {
        public Vector3 Color = new Vector3(0, 0, 0);

        public float Roughness = 150;

        /// <summary>
        /// Calculates the color at a certain position.
        /// Note: In case of a single color material it always returns color.
        /// </summary>
        /// <param name="position">The position of the point we want to know the color of.</param>
        /// <returns>The color.</returns>
        public virtual Vector3 Diffuse(Vector3 position)
        {
            return Color;
        }

        /// <summary>
        /// Calculates the specular color at a certain position.
        /// Note: In case of a material without specular it returns black to make sure no light gets added.
        /// </summary>
        /// <param name="position">The position of the point we want to know the color of.</param>
        /// <returns>The color.</returns>
        public virtual Vector3 Specular(Vector3 position)
        {
            return new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Calculates the reflectiveness at a certain position.
        /// </summary>
        /// <param name="position">
        /// The position of the point we want to know the reflectiveness of.
        /// </param>
        /// <returns>The reflectiveness.</returns>
        public virtual float Reflectiveness(Vector3 position)
        {
            return 0;
        }
    }
}
