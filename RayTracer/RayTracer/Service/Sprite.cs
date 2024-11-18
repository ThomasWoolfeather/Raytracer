using System;
using OpenTK.Graphics.OpenGL;

namespace RayTracer.Service
{
    public class Sprite
    {
        private Surface bitmap;

        static public Surface target;

        private int textureID;

        // sprite constructor
        public Sprite(string fileName)
        {
            bitmap = new Surface(fileName);
            textureID = bitmap.GenTexture();
        }

        // draw a sprite with scaling
        public void Draw(float x, float y, float scale = 1.0f)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(PrimitiveType.Quads);
            float u1 = (x * 2 - 0.5f * scale * bitmap.width) / target.width - 1;
            float v1 = 1 - (y * 2 - 0.5f * scale * bitmap.height) / target.height;
            float u2 = ((x + 0.5f * scale * bitmap.width) * 2) / target.width - 1;
            float v2 = 1 - ((y + 0.5f * scale * bitmap.height) * 2) / target.height;
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(u1, v2);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(u2, v2);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(u2, v1);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(u1, v1);
            GL.End();
            GL.Disable(EnableCap.Blend);
        }
    }
}
