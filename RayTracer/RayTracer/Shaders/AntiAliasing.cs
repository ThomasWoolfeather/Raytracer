using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace RayTracer.Shaders
{
    public class AntiAliasing
    {
        public int programID, vsID, fsID, vbo_pos, vbo_color, attrPos, attrCol;

        private int[] positions;

        private readonly float[] colors;

        private readonly int width;

        private readonly int height;

        public AntiAliasing()
        {
        }

        /// <summary>
        /// Anti Aliasing by comparing adjacent pixels.
        /// </summary>
        /// <param name="pixels">The pixel array of the 2d texture.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void AA(ref int[] pixels, int width, int height)
        {
            int[] pix = pixels;
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width - 1; column++)
                {
                    //If we are over half the width we break the loop because we've reached debug territory
                    if (column > width * 0.5f) break;
                    //Calculate the position of the pixel in the array.
                    int pos = column + row * width;
                    //Get the color in floats so we can do calculations.
                    Vector3 color = Utils.Translator.RevertColor(pixels[pos]);
                    //Get left neighbor.
                    if (column % width != 0)
                        color = (color + Utils.Translator.RevertColor(pixels[pos - 1])) * 0.5f;
                    //Get right neighbor.
                    if (column % width != width - 1)
                        color = (color + Utils.Translator.RevertColor(pixels[pos + 1])) * 0.5f;
                    //Get upper neighbor.
                    if (row != 0)
                        color = (color + Utils.Translator.RevertColor(pixels[column + (row - 1) * width])) * 0.5f;
                    //Get lower neighbor.
                    if (row != height - 1)
                        color = (color + Utils.Translator.RevertColor(pixels[column + (row + 1) * width])) * 0.5f;
                    //Write the new value in a temporary array so it will not mess with pixels yet to come.
                    pix[pos] = Utils.Translator.CreateColor(color);
                }
            }
            //Swap the temporary pixel array for the actually pixel array and that's it.
            pixels = pix;
        }

        // The code below this point was an attempt of implementing Anti Aliasing on the GPU using
        // OpenGL shaders. It did however not succeed and that's why it's left uncommentated.

        public AntiAliasing(int width, int height)
        {
            this.width = width;
            this.height = height;
            positions = new int[width * height * 2];
            colors = new float[width * height * 3];

            BindShaders();
        }

        private void FillArrays(int[] pixels)
        {
            for (int row = 0; row < width; row++)
            {
                for (int column = 0; column < height; column++)
                {
                    int pos = ((row * width) + column) * 2;
                    positions[pos] = row;
                    positions[pos + 1] = column;

                    Vector3 color = Utils.Translator.RevertColor(pixels[(row * width) + column]);

                    int col = ((row * width) + column) * 3;
                    colors[pos] = pixels[pos];
                    colors[pos + 1] = pixels[pos];
                    colors[pos + 3] = pixels[pos];
                }
            }
        }

        public void Draw(int[] pixels)
        {
            FillArrays(pixels);
            GL.UseProgram(programID);

            GL.EnableVertexAttribArray(attrPos);
            GL.EnableVertexAttribArray(attrCol);
            GL.DrawArrays(BeginMode.Points, 0, pixels.Length);
        }

        /// <summary>
        /// Load and bind the vertex shader and the fragment shader.
        /// </summary>
        public void BindShaders()
        {
            //Get a program id and use this to attach the shaders.
            programID = GL.CreateProgram();
            //Load the vertex shader.
            LoadShader("../../Shaders/vs.glsl",
             ShaderType.VertexShader, programID, out vsID);
            //Load the fragment shader.
            LoadShader("../../Shaders/fs.glsl",
             ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            //Get the identifiers that allow access to the input variables of the shaders.
            attrPos = GL.GetAttribLocation(programID, "Position");
            attrCol = GL.GetAttribLocation(programID, "vColor");

            //Link the position data by generating and binding a buffer.
            vbo_pos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
            GL.BufferData<int>(BufferTarget.ArrayBuffer,
                (IntPtr)(positions.Length * 2),
                positions, BufferUsageHint.StreamDraw
             );
            GL.VertexAttribPointer(attrPos, 2,
                VertexAttribPointerType.Int,
                false, 0, 0
             );

            //Link the color data by generating and binding a buffer.
            vbo_color = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<float>(BufferTarget.ArrayBuffer,
                (IntPtr)(positions.Length * 3),
                colors, BufferUsageHint.StreamDraw
             );
            GL.VertexAttribPointer(attrCol, 2,
                VertexAttribPointerType.Float,
                false, 0, 0
             );
        }

        /// <summary>
        /// Load a shader.
        /// </summary>
        /// <param name="name">The path of the shader file.</param>
        /// <param name="type">The type of shader.</param>
        /// <param name="program">The GL program that uses the shader.</param>
        /// <param name="ID">The shader ID that gets set when it is created.</param>
        private void LoadShader(string name, ShaderType type, int program, out int ID)
        {
            //Create the shader.
            ID = GL.CreateShader(type);
            //Load the shader text.
            using (StreamReader sr = new StreamReader(name))
                GL.ShaderSource(ID, sr.ReadToEnd());
            //Compile the shader.
            GL.CompileShader(ID);
            //Attatch the shader to program.
            GL.AttachShader(program, ID);
            Console.WriteLine(GL.GetShaderInfoLog(ID));
        }
    }
}
