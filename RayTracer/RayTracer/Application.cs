using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RayTracer.Service;
using RayTracer.Service.Debug;
using RayTracer.Shaders;
using System;
using System.Drawing;

namespace RayTracer
{
    public class Application : GameWindow
    {
        private static int screenID;

        private static Raytracer Raytracer;

        private static DebugWindow Debugger;

        private static DiagnosticTool Diagnose;

        private static AntiAliasing AA;

        private static bool terminated = false;

        private bool antialiasing;

        protected override void OnLoad(EventArgs e)
        {
            // called upon app init
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            ClientSize = new Size(1024, 512);
            Raytracer = new Raytracer();
            Raytracer.Screen = new Surface(Width, Height);
            Sprite.target = Raytracer.Screen;
            screenID = Raytracer.Screen.GenTexture();
            Raytracer.Init();
            Diagnose = new DiagnosticTool();
            Debugger = new DebugWindow(Raytracer, Diagnose);
            AA = new AntiAliasing();
            PrintKeyBindings();
        }

        protected override void OnUnload(EventArgs e)
        {
            // called upon app close
            GL.DeleteTextures(1, ref screenID);
            Environment.Exit(0); // bypass wait for key on CTRL-F5
        }

        protected override void OnResize(EventArgs e)
        {
            // called upon window resize
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // called once per frame; app logic
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Diagnose.Start();
            // called once per frame; render
            Raytracer.Screen.Clear(0);
            Debugger.Render();
            Raytracer.Render();
            if (antialiasing) AA.AA(ref Raytracer.Screen.pixels, Raytracer.Screen.width, Raytracer.Screen.height);
            if (terminated)
            {
                Exit();
                return;
            }
            // convert Game.screen to OpenGL texture
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, screenID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                           Raytracer.Screen.width, Raytracer.Screen.height, 0,
                           OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                           PixelType.UnsignedByte, Raytracer.Screen.pixels
                         );
            // clear window contents
            GL.Clear(ClearBufferMask.ColorBufferBit);
            // setup camera
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // draw screen filling quad
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
            GL.End();

            /*
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            AA.Draw(Raytracer.Screen.pixels);
            */

            SwapBuffers();
            Diagnose.End();
        }

        public static void Main(string[] args)
        {
            // entry point
            using (Application app = new Application())
            {
                app.KeyDown += app.window_KeyDown;
                app.Run(30.0, 0.0);
            }
        }

        private float movementspeed = 0.1f;

        private int degreespeed = 5;

        private float rotationspeed = 0.1f;

        private int rays = 5;

        private void window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            //MOVEMENT
            if (e.Key == Key.W) //FORWARD
                Raytracer.Camera.MoveVertically(movementspeed);
            if (e.Key == Key.A) //LEFT
                Raytracer.Camera.MoveHorizontally(movementspeed);
            if (e.Key == Key.D) //RIGHT
                Raytracer.Camera.MoveHorizontally(-movementspeed);
            if (e.Key == Key.S) //BACK
                Raytracer.Camera.MoveVertically(-movementspeed);
            if (e.Key == Key.Up) //UP
                Raytracer.Camera.MoveHeight(movementspeed);
            if (e.Key == Key.Down) //DOWN
                Raytracer.Camera.MoveHeight(-movementspeed);
            //ROTATION
            if (e.Key == Key.Right)
                Raytracer.Camera.Rotate(rotationspeed);
            if (e.Key == Key.Left)
                Raytracer.Camera.Rotate(-rotationspeed);
            //FOV
            if (e.Key == Key.Z)
                Raytracer.Camera.AdjustFOV(-degreespeed);
            if (e.Key == Key.X)
                Raytracer.Camera.AdjustFOV(degreespeed);
            //LIGHTS
            if (e.Key == Key.P)
                Raytracer.Scene.TogglePrimitives();
            if (e.Key == Key.L)
                Raytracer.Scene.ToggleLights();
            //ANTI-ALIASING
            if (e.Key == Key.M)
                antialiasing = !antialiasing;
            //DEBUG RAYS
            if (e.Key == Key.Plus || e.Key == Key.KeypadPlus)
                Debugger.AddRays(rays);
            if (e.Key == Key.Minus || e.Key == Key.KeypadMinus)
                Debugger.AddRays(-rays);
            //DEBUG INFO
            if (e.Key == Key.I)
                Diagnose.Active = !Diagnose.Active;
        }

        /// <summary>
        /// Prints all the relevant key bindings to the console window for easy understanding of the controls.
        /// </summary>
        private void PrintKeyBindings()
        {
            Console.WriteLine("----Keys Bindings----");
            Console.WriteLine("Use WASD for camera movement horizontally.");
            Console.WriteLine("Use the up/down keys for movement vertically.");
            Console.WriteLine("Use the left/right keys for rotation horizontally.");
            Console.WriteLine("Use Z and X to increase and decrease the eye distance (FOV).");
            Console.WriteLine("Use P to toggle primitive visibility.");
            Console.WriteLine("Use L to toggle the light between point and spot light.");
            Console.WriteLine("Use M to toggle Anti-Aliasing.");
            Console.WriteLine("Use + and - to increase or decrease the amount of debug rays by " + rays + ".");
            Console.WriteLine("Use I to show diagnostic info.");
        }
    }
}
