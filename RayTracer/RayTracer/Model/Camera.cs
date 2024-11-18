using OpenTK;
using System;

namespace RayTracer.Model
{
    public class Camera
    {
        public Vector3 MinBounds, MaxBounds;

        //The position and direction of the camera.
        public Vector3 Position, Direction, Perpendicular;

        //The screen dimensions can be calculated using two coordinates when assuming the screen is rectangular.
        public Vector2 ScreenOrigin, ScreenDimensions;

        public int FOV = 90;

        private float eyeposition = 0;

        private float rotation = 0;

        private float zrotation = 0;

        /// <summary>
        /// Create a camera with default values.
        /// </summary>
        public Camera()
        {
            MinBounds = new Vector3(-10, -10, -0.8f);
            MaxBounds = new Vector3(10, 10, 10);
            Position = new Vector3(0, -3f, 0);
            Rotate(0);
            ScreenOrigin = new Vector2(-1, -1);
            ScreenDimensions = new Vector2(2f, 2f);
            CalculateEyeDistance();
        }

        /// <summary>
        /// Rotate the camera.
        /// </summary>
        /// <param name="alpha">The amount of rotation in radians.</param>
        public void Rotate(float alpha)
        {
            rotation += alpha;
            Vector2 rotate = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation)).Normalized();
            Direction = new Vector3(rotate.X, rotate.Y, zrotation);
            CalculatePerpendicular();
        }

        /// <summary>
        /// Rotate the camera along the z axis.
        /// </summary>
        /// <param name="alpha">The amount of rotation in radians.</param>
        public void RotateZ(float alpha)
        {
            zrotation += alpha;
            Rotate(0);
        }

        /// <summary>
        /// Moves the camera along the horizontal viewing access.
        /// Note: this takes rotation into account by using the direction.
        /// </summary>
        /// <param name="movement">The amount of movement.</param>
        public void MoveHorizontally(float movement)
        {
            Position += movement * Perpendicular;
            CheckBounds();
        }

        /// <summary>
        /// Moves the camera along the vertical viewing access.
        /// Note: this takes rotation into account by using the perpendicular.
        /// </summary>
        /// <param name="movement">The amount of movement.</param>
        public void MoveVertically(float movement)
        {
            Position += movement * Direction;
            CheckBounds();
        }

        /// <summary>
        /// Moves the camera up and down.
        /// </summary>
        /// <param name="movement">The amount of movement.</param>
        public void MoveHeight(float movement)
        {
            Position.Z += movement;
            CheckBounds();
        }

        /// <summary>
        /// Check if the camera is still within the defined bounds, if not; set it back.
        /// </summary>
        private void CheckBounds()
        {
            //Check the x coordinate.
            if (Position.X < MinBounds.X)
                Position.X = MinBounds.X;
            else if (Position.X > MaxBounds.X)
                Position.X = MaxBounds.X;

            //Check the y coordinate.
            if (Position.Y < MinBounds.Y)
                Position.Y = MinBounds.Y;
            else if (Position.Y > MaxBounds.Y)
                Position.Y = MaxBounds.Y;

            //Check the z coordinate.
            if (Position.Z < MinBounds.Z)
                Position.Z = MinBounds.Z;
            else if (Position.Z > MaxBounds.Z)
                Position.Z = MaxBounds.Z;
        }

        /// <summary>
        /// Calculates perpendicular.
        /// </summary>
        private void CalculatePerpendicular()
        {
            Perpendicular = new Vector3(-Direction.Y, Direction.X, Direction.Z).Normalized();
        }

        /// <summary>
        /// Get the position of the eye in the world.
        /// </summary>
        /// <returns>The position of the eye in the world.</returns>
        public Vector3 GetEyePosition()
        {
            return Position - Direction * eyeposition; //(float)Math.Sin(FOV);//Math.Abs((0.5f * ScreenDimensions.X) / (float)Math.Tan(FOV * 0.5f));
        }

        /// <summary>
        /// Calculates the eye distance from the screen.
        /// </summary>
        /// <returns>The distance of the eye.</returns>
        private void CalculateEyeDistance()
        {
            float tan = (float)Math.Tan(0.5f * Utils.Translator.ToRadians(FOV));
            eyeposition = (0.5f * ScreenDimensions.X) / tan;
        }

        /// <summary>
        /// Adjust the position of the eye.
        /// </summary>
        /// <param name="adjustment">The adjustment.</param>
        public void AdjustFOV(int adjustment)
        {
            FOV += adjustment;
            if (FOV < 70)
                FOV = 70;
            else if (FOV > 120)
                FOV = 120;
            CalculateEyeDistance();
        }
    }
}
