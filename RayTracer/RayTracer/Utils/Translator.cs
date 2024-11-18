using OpenTK;
using RayTracer.Service;
using System;

namespace RayTracer.Utils
{
    public static class Translator
    {
        private static float factor = (float)Math.PI / 180;

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degree">The degrees.</param>
        /// <returns>The radians.</returns>
        public static float ToRadians(float degree)
        {
            return degree * factor;
        }

        /// <summary>
        /// Translate a worldcoordinate X to a screen coordinate.
        /// </summary>
        /// <param name="x">The worldcoordinate X.</param>
        /// <param name="screen">The screen on which the x gets plotted.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TX(float x, Surface screen)
        {
            return TX(x, screen, 0.0f);
        }

        /// <summary>
        /// Translate a worldcoordinate X to a screen coordinate.
        /// </summary>
        /// <param name="x">The worldcoordinate X.</param>
        /// <param name="screen">The screen on which the X gets plotted.</param>
        /// <param name="offset">The offset of the worldcoordinate system.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TX(float x, Surface screen, float offset)
        {
            return TX(x, screen, offset, 10.0f);
        }

        /// <summary>
        /// Translate a worldcoordinate X to a screen coordinate.
        /// </summary>
        /// <param name="x">The worldcoordinate X.</param>
        /// <param name="screen">The screen on which the x gets plotted.</param>
        /// <param name="offset">The offset of the worldcoordinate system.</param>
        /// <param name="scale">The scale of the worldcoordinate system.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TX(float x, Surface screen, float offset, float scale)
        {
            x *= screen.height * (float)(screen.width / screen.height) / scale;
            x += screen.width / 2f + offset;
            return (int)x / 2;
        }

        /// <summary>
        /// Translate a worldcoordinate Y to a screen coordinate.
        /// </summary>
        /// <param name="y">The worldcoordinate Y.</param>
        /// <param name="screen">The screen on which the Y gets plotted.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TY(float y, Surface screen)
        {
            return TY(y, screen, 0);
        }

        /// <summary>
        /// Translate a worldcoordinate Y to a screen coordinate.
        /// </summary>
        /// <param name="y">The worldcoordinate Y.</param>
        /// <param name="screen">The screen on which the Y gets plotted.</param>
        /// <param name="offset">The offset of the worldcoordinate system.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TY(float y, Surface screen, float offset)
        {
            return TY(y, screen, offset, 10.0f);
        }

        /// <summary>
        /// Translate a worldcoordinate Y to a screen coordinate.
        /// </summary>
        /// <param name="y">The worldcoordinate Y.</param>
        /// <param name="screen">The screen on which the Y gets plotted.</param>
        /// <param name="offset">The offset of the worldcoordinate system.</param>
        /// <param name="scale">The scale of the worldcoordinate system.</param>
        /// <returns>The screen coordinate.</returns>
        public static int TY(float y, Surface screen, float offset, float scale)
        {
            y *= -1f;
            y *= screen.width * ((float)screen.height / (float)screen.width) / scale;
            y += screen.height / 2 + offset;
            return (int)y;
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="color">The color in floats.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(Vector3 color)
        {
            return CreateColor(color.X, color.Y, color.Z);
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="red">The float representation of red.</param>
        /// <param name="green">The float representation of green.</param>
        /// <param name="blue">The float representation of blue.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(float red, float green, float blue)
        {
            return CreateColor((int)(red * 255), (int)(green * 255), (int)(blue * 255));
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="red">The amount of red.</param>
        /// <param name="green">The amount of green.</param>
        /// <param name="blue">The amount of blue.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(int red, int green, int blue)
        {
            if (red > 255) red = 255;
            if (green > 255) green = 255;
            if (blue > 255) blue = 255;
            return (red << 16) + (green << 8) + blue;
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="color">The color in floats.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(Vector4 color)
        {
            return CreateColor(color.X, color.Y, color.Z, color.W);
        }

        /// <summary>
        /// Reverts an integer format color to a Vector of three floats.
        /// </summary>
        /// <param name="color">The color as integer.</param>
        /// <returns>The color as RGB vector.</returns>
        public static Vector3 RevertColor(int color)
        {
            int red = (color >> 16) & 255;
            int green = (color >> 8) & 255;
            int blue = color & 255;
            return new Vector3((float)red / 255, (float)green / 255, (float)blue / 255);
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="red">The float representation of red.</param>
        /// <param name="green">The float representation of green.</param>
        /// <param name="blue">The float representation of blue.</param>
        /// <param name="alpha">The float representation of alpha.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(float red, float green, float blue, float alpha)
        {
            return CreateColor((int)red * 255, (int)green * 255, (int)blue * 255, (int)alpha * 255);
        }

        /// <summary>
        /// Creates a color in integer format.
        /// </summary>
        /// <param name="red">The amount of red.</param>
        /// <param name="green">The amount of green.</param>
        /// <param name="blue">The amount of blue.</param>
        /// <param name="alpha">The amount of alpha.</param>
        /// <returns>The color as integer.</returns>
        public static int CreateColor(int red, int green, int blue, int alpha)
        {
            if (red > 255) red = 255;
            if (green > 255) green = 255;
            if (alpha > 255) alpha = 255;
            return (alpha << 24) + (red << 16) + (green << 8) + blue;
        }
    }
}
