using System;
using OpenTK;

namespace RayTracer.Model.Materials
{
    public class Mirror : Material
    {
        /// <summary>
        /// Material is a material that has total reflection.
        /// </summary>
        public Mirror()
        {
            Color = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Calculates the specular color at a certain position.
        /// Note: In case of a material without specular it returns black to make sure no light gets added.
        /// </summary>
        /// <param name="position">The position of the point we want to know the color of.</param>
        /// <returns>The color.</returns>
        public override Vector3 Specular(Vector3 position)
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
        public override float Reflectiveness(Vector3 position)
        {
            return 1;
        }
    }
}
