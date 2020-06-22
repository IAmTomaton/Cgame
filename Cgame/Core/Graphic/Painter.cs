using Cgame.Core.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Cgame.Core.Graphic
{
    class Painter : IPainter
    {
        private readonly Shader shader;
        private readonly ITextureLibrary textureLibrary;
        private readonly GLControl gLControl;

        public Painter(Shader shader, ITextureLibrary textureLibrary, GLControl gLControl)
        {
            this.shader = shader;
            this.textureLibrary = textureLibrary;
            this.gLControl = gLControl;
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader.Use();
        }

        public void Draw(IEnumerable<Sprite> sprites, Camera camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, gLControl.Width, gLControl.Height);
            shader.Use();

            foreach (var sprite in sprites)
                DrawSprite(sprite, camera);

            gLControl.SwapBuffers();
        }

        private void DrawSprite(Sprite sprite, Camera camera)
        {
            textureLibrary.GetTexture(sprite.Texture).Use();
            textureLibrary.GetPrimitive(sprite.Texture).Use();

            var model = Matrix4.Identity;
            model *= Matrix4.CreateRotationZ(sprite.Angle);
            model *= Matrix4.CreateTranslation(sprite.Position);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, textureLibrary.IndicesLength,
                DrawElementsType.UnsignedInt, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
        }
    }
}
