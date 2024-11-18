using System;
using OpenTK;
using RayTracer.Model;
using RayTracer.Utils;

namespace RayTracer.Service.Debug
{
    public class DebugWindow
    {
        private Raytracer tracer;

        private DiagnosticTool diagnose;

        // The viewinglimit is to ensure the distance of a line never reaches infinity. See DrawRay
        // for further explanation.
        private readonly float viewinglimit = 1000;

        // The amount of rays that get cast from the debugwindow. Make sure this is never greater
        // then the screen width.
        private int rays = 30;

        /// <summary>
        /// Create the DebugWindow.
        /// </summary>
        public DebugWindow(Raytracer raytracer, DiagnosticTool diagnostictool)
        {
            tracer = raytracer;
            diagnose = diagnostictool;
        }

        /// <summary>
        /// Enables running time adjustments to the amount of rays.
        /// </summary>
        /// <param name="amount">the amount of rays to increas (use minus to decrease).</param>
        public void AddRays(int amount)
        {
            rays += amount;
            if (rays < 0)
                rays = 0;
            else if (rays > tracer.Screen.halfwidth)
                rays = tracer.Screen.halfwidth;
        }

        /// <summary>
        /// Render gets called once every frame.
        /// </summary>
        public void Render()
        {
            //Draw the screen.
            //Starting at:
            Vector3 ScreenBegin = tracer.PixelToCoordinate(0, tracer.Screen.halfheight);
            //Ending at:
            Vector3 ScreenEnd = tracer.PixelToCoordinate(tracer.Screen.halfwidth, tracer.Screen.halfheight);
            Vector3 Perpendicular = tracer.Camera.Perpendicular + tracer.Camera.Position;
            Vector3 Eye = tracer.Camera.GetEyePosition();
            //Draw the camera.
            tracer.Screen.Line(TX(ScreenBegin.X), TY(ScreenBegin.Y), TX(ScreenEnd.X), TY(ScreenEnd.Y), 0xffffff);
            //Draw the direction of the camera.
            tracer.Screen.Line(TX(tracer.Camera.Position.X), TY(tracer.Camera.Position.Y), TX(tracer.Camera.Direction.X * 0.2f + tracer.Camera.Position.X), TY(tracer.Camera.Direction.Y * 0.2f + tracer.Camera.Position.Y), 0xaa00aa);
            //Draw a line from eye to camera.
            tracer.Screen.Line(TX(tracer.Camera.Position.X), TY(tracer.Camera.Position.Y), TX(Eye.X), TY(Eye.Y), 0xaaffaa);
            tracer.Screen.Line(TX(tracer.Camera.Position.X), TY(tracer.Camera.Position.Y), TX(Perpendicular.X), TY(Perpendicular.Y), 0x00ffff);

            //Draw the debug lines.
            for (float i = 0; i < rays; i++)
            {
                //Create a ray using the function of the raytracer to ensure consistency.
                Ray ray = tracer.CreateRay((int)(i * ((float)tracer.Screen.halfwidth / ((float)rays - 1))), tracer.Screen.halfheight);
                //Call upon a recursive method to draw the ray. Pass a depth of 18 (diminishing with each ray).
                DrawRay(ray, tracer.MaxDepth);
            }

            //Draw the outline an debug text.
            //This is done last because it is drawn over the lines
            tracer.Screen.Print("Debug: " + rays + " lines", tracer.Screen.halfwidth + 3, 2, 0xffffff);
            tracer.Screen.Print("FOV: " + tracer.Camera.FOV, tracer.Screen.width - 120, 2, 0xffffff);
            //If diagnose info is activated show the info.
            if (diagnose.Active)
            {
                tracer.Screen.Print("FPS: " + diagnose.GetFPS(), tracer.Screen.halfwidth + 3, 22, 0xffffff);
                tracer.Screen.Print("Current frame (ms): " + diagnose.GetLatestTime(), tracer.Screen.halfwidth + 3, 42, 0xffffff);
                tracer.Screen.Print("Average (ms): " + diagnose.GetAverageTime(), tracer.Screen.halfwidth + 3, 62, 0xffffff);
                tracer.Screen.Print("Longest (ms): " + diagnose.GetLongestTime(), tracer.Screen.halfwidth + 3, 82, 0xffffff);
                tracer.Screen.Print("Shortest (ms): " + diagnose.GetShortestTime(), tracer.Screen.halfwidth + 3, 102, 0xffffff);
            }
            //Draw the outline of the debug window, which is just a white frame.
            tracer.Screen.Line(tracer.Screen.halfwidth, 0, tracer.Screen.width - 1, 0, 0xffffff);
            tracer.Screen.Line(tracer.Screen.halfwidth, 0, tracer.Screen.halfwidth, tracer.Screen.height - 1, 0xffffff);
            tracer.Screen.Line(tracer.Screen.width - 1, 0, tracer.Screen.width - 1, tracer.Screen.height - 1, 0xffffff);
            tracer.Screen.Line(tracer.Screen.halfwidth, tracer.Screen.height - 1, tracer.Screen.width - 1, tracer.Screen.height - 1, 0xffffff);
        }

        /// <summary>
        /// Draws a ray and its reflections.
        /// </summary>
        /// <param name="ray">The ray to be drawn.</param>
        /// <param name="depth">The current depth.</param>
        private void DrawRay(Ray ray, int depth)
        {
            //Get the color of a ray using the function of the raytracer; again to ensure consistency.
            Vector3 color = tracer.CastRay(ray);
            //Set a limit for the distance to the viewing limit to make sure we don't get a distance of infinity.
            //Note: A distance of infinity time the normalized direction vector would become a vector that goes to infinity in each axis
            //      resulting in a straightline towards an incorrect position.
            float distance = viewinglimit;
            //If there is an intersection we take that distance; unless it exceeds the limit in either positive or negative range.
            if (ray.Intersection != null && (ray.Intersection.Distance < distance && ray.Intersection.Distance > -distance))
                distance = ray.Intersection.Distance;
            //The end point of the debug line.
            Vector3 end = ray.Direction * distance + ray.Origin;
            //Plot the line using the color we got from the CastRay function.
            tracer.Screen.Line(TX(ray.Origin.X), TY(ray.Origin.Y), TX(end.X), TY(end.Y), Translator.CreateColor(color));
            //If there is an intersection and there is a reflected ray it gets drawn to using a recursive call.
            if (ray.Intersection != null && ray.Intersection.ReflectedRay != null)
                DrawRay(ray.Intersection.ReflectedRay, depth - 1);
        }

        /// <summary>
        /// An implementation of the TX function for lazy transformations.
        /// Note: uses the translator function, which has been reimplemented from the first assignment.
        /// </summary>
        /// <param name="x">The woorldcordinate x.</param>
        /// <returns>The screencoordinate x.</returns>
        private int TX(float x)
        {
            return tracer.Screen.halfwidth + Translator.TX(x, tracer.Screen);
        }

        /// <summary>
        /// An implementation of the TY function for lazy transformations.
        /// Note: uses the translator function, which has been reimplemented from the first assignment.
        /// </summary>
        /// <param name="x">The woorldcordinate y.</param>
        /// <returns>The screencoordinate y.</returns>
        private int TY(float y)
        {
            return Translator.TX(-y, tracer.Screen);
        }
    }
}
