using OpenTK;
using System;

namespace RayTracer.Model.Materials
{
    public class Checkerboard : Material
    {
        //The amount of squares that fit in one unit.
        public float SquareAmount;

        /// <summary>
        /// Checkerboard is a material that has alternating squares and (color).
        /// </summary>
        public Checkerboard()
        {
            SquareAmount = 3f;
            Roughness = 150;
        }

        /// <summary>
        /// Calculates the color at a certain position, using the characteristic of a checkerboard.
        /// </summary>
        /// <param name="position">The position of the point we want to know the color of.</param>
        /// <returns>The color.</returns>
        public override Vector3 Diffuse(Vector3 position)
        {
            //Check if the square is either black or (color).
            if ((Math.Floor(position.X * SquareAmount) + Math.Floor(position.Y * SquareAmount)) % 2 != 0)
                return Color;
            else //black
                return new Vector3(0f, 0f, 0f);
        }

        /// <summary>
        /// Calculates the reflectiveness at a certain position, using the characteristic of a checkerboard.
        /// </summary>
        /// <param name="position">
        /// The position of the point we want to know the reflectiveness of.
        /// </param>
        /// <returns>The reflectiveness.</returns>
        public override float Reflectiveness(Vector3 position)
        {
            //Check if the square is either black or (color).
            if ((Math.Floor(position.X * SquareAmount) + Math.Floor(position.Y * SquareAmount)) % 2 != 0)
                return 0.4f;
            else //black
                return 0.5f;
        }
    }
}
