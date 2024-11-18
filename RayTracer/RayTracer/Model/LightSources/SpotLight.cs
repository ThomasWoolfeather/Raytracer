using OpenTK;
using System;

namespace RayTracer.Model.LightSources
{
    public class SpotLight : Light
    {
        private Vector3 direction;

        private float angle;

        /// <summary>
        /// A spotlight has an angle and a direction.
        /// Note: An angle of 0 will default to a minimum value of radian 1.
        /// Note: If an angle appears to be bigger than desired, make sure the distance is correct
        ///       because the projection grows with distance.
        /// </summary>
        /// <param name="direction">The light direction.</param>
        /// <param name="angle">The light angle.</param>
        public SpotLight(Vector3 direction, float angle)
        {
            this.direction = direction;
            this.angle = Utils.Translator.ToRadians(angle);
        }

        /// <summary>
        /// Gets the color at a certain position by caculating if it falls within the angle.
        /// </summary>
        /// <param name="position">The position from which the shadowray is sent.</param>
        /// <returns></returns>
        public override Vector3 GetColor(Vector3 position)
        {
            //Calculate the distance between the point and the light.
            Vector3 distance = (position - Position).Normalized();
            //Calculate the angle.
            float alpha = (float)Math.Acos(Vector3.Dot(distance, direction));
            //Compare the angle.
            if (alpha > angle)
            {
                //Calculate and limit the light intensity.
                float intensity = 1 - (alpha / angle);
                if (intensity < 0)
                    intensity = 0;
                else if (intensity > 1)
                    intensity = 1;
                //Return the color with its intensity.
                return Color * intensity;
            }
            else
                //Not in the angle of the spotlight;
                return Color;
        }
    }
}
