using System;
using OpenTK;

namespace RayTracer.Model.Materials
{
    public class Shiny : Material
    {
        /// <summary>
        /// Shiny is a material that has a high reflection and a specular color.
        /// </summary>
        public Shiny()
        {
            Roughness = 50;
        }

        /// <summary>
        /// Calculates the specular color at a certain position.
        /// Note: In case of a material without specular it returns black to make sure no light gets added.
        /// </summary>
        /// <param name="position">The position of the point we want to know the color of.</param>
        /// <returns>The color.</returns>
        public override Vector3 Specular(Vector3 position)
        {
            return Color * 0.5f;
        }

        /// <summary>
        /// Calculates the reflectiveness at a certain position.
        /// </summary>
        /// <param name="position">
        /// The position of the point we want to know the reflectiveness of.
        /// </param>
        /// <returns>The reflectiveness.</returns>
        public override float Reflectiveness(Vector3 position)
        {
            return 0.2f;
        }
    }
}
