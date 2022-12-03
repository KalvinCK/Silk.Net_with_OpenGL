using Silk.NET.OpenGL;

namespace MyGame
{
    public struct Cube : IDisposable
    {
        private static GL Gl { get => Program.Gl; }
        private static BufferObject<float> ?Vbo;
        private static VertexArrayObject ?Vao;
        public  static void RenderCube()
        {
            if (Vao == null)
            {
                float[] vertices = 
                {
                    -1.0f, -1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    0.0f, 0.0f, // bottom-left
                     1.0f,  1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    1.0f, 1.0f, // top-right
                     1.0f, -1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    1.0f, 0.0f, // bottom-right         
                     1.0f,  1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    1.0f, 1.0f, // top-right
                    -1.0f, -1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    0.0f, 0.0f, // bottom-left
                    -1.0f,  1.0f, -1.0f,     0.0f,  0.0f, -1.0f,    0.0f, 1.0f, // top-left
                    -1.0f, -1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    0.0f, 0.0f, // bottom-left
                     1.0f, -1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    1.0f, 0.0f, // bottom-right
                     1.0f,  1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    1.0f, 1.0f, // top-right
                     1.0f,  1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    1.0f, 1.0f, // top-right
                    -1.0f,  1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    0.0f, 1.0f, // top-left
                    -1.0f, -1.0f,  1.0f,     0.0f,  0.0f,  1.0f,    0.0f, 0.0f, // bottom-left
                    -1.0f,  1.0f,  1.0f,    -1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // top-right
                    -1.0f,  1.0f, -1.0f,    -1.0f,  0.0f,  0.0f,    1.0f, 1.0f, // top-left
                    -1.0f, -1.0f, -1.0f,    -1.0f,  0.0f,  0.0f,    0.0f, 1.0f, // bottom-left
                    -1.0f, -1.0f, -1.0f,    -1.0f,  0.0f,  0.0f,    0.0f, 1.0f, // bottom-left
                    -1.0f, -1.0f,  1.0f,    -1.0f,  0.0f,  0.0f,    0.0f, 0.0f, // bottom-right
                    -1.0f,  1.0f,  1.0f,    -1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // top-right
                     1.0f,  1.0f,  1.0f,     1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // top-left
                     1.0f, -1.0f, -1.0f,     1.0f,  0.0f,  0.0f,    0.0f, 1.0f, // bottom-right
                     1.0f,  1.0f, -1.0f,     1.0f,  0.0f,  0.0f,    1.0f, 1.0f, // top-right         
                     1.0f, -1.0f, -1.0f,     1.0f,  0.0f,  0.0f,    0.0f, 1.0f, // bottom-right
                     1.0f,  1.0f,  1.0f,     1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // top-left
                     1.0f, -1.0f,  1.0f,     1.0f,  0.0f,  0.0f,    0.0f, 0.0f, // bottom-left     
                    -1.0f, -1.0f, -1.0f,     0.0f, -1.0f,  0.0f,    0.0f, 1.0f, // top-right
                     1.0f, -1.0f, -1.0f,     0.0f, -1.0f,  0.0f,    1.0f, 1.0f, // top-left
                     1.0f, -1.0f,  1.0f,     0.0f, -1.0f,  0.0f,    1.0f, 0.0f, // bottom-left
                     1.0f, -1.0f,  1.0f,     0.0f, -1.0f,  0.0f,    1.0f, 0.0f, // bottom-left
                    -1.0f, -1.0f,  1.0f,     0.0f, -1.0f,  0.0f,    0.0f, 0.0f, // bottom-right
                    -1.0f, -1.0f, -1.0f,     0.0f, -1.0f,  0.0f,    0.0f, 1.0f, // top-right
                    -1.0f,  1.0f, -1.0f,     0.0f,  1.0f,  0.0f,    0.0f, 1.0f, // top-left
                     1.0f,  1.0f , 1.0f,     0.0f,  1.0f,  0.0f,    1.0f, 0.0f, // bottom-right
                     1.0f,  1.0f, -1.0f,     0.0f,  1.0f,  0.0f,    1.0f, 1.0f, // top-right     
                     1.0f,  1.0f,  1.0f,     0.0f,  1.0f,  0.0f,    1.0f, 0.0f, // bottom-right
                    -1.0f,  1.0f, -1.0f,     0.0f,  1.0f,  0.0f,    0.0f, 1.0f, // top-left
                    -1.0f,  1.0f,  1.0f,     0.0f,  1.0f,  0.0f,    0.0f, 0.0f  // bottom-left     
                };

                Vbo = new BufferObject<float>(vertices, BufferTargetARB.ArrayBuffer);
                Vao = new VertexArrayObject();

                Vao.BindBuffer(ref Vbo);
                Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 0);
                Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 8, 3);
                Vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, 8, 6);

            }
            // render Cube
            Vao.Bind();
            Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
        public void Dispose()
        {
            Vao!.Dispose();
            Vbo!.Dispose();
        }
    }
    public struct Quad : IDisposable
    {
        private static GL Gl { get => Program.Gl; }
        private static BufferObject<float> ?Vbo;
        private static BufferObject<uint> ?Ebo;
        private static VertexArrayObject ?Vao;
        public static unsafe void RenderQuad()
        {
            if (Vao == null)
            {
                float[] vertices = 
                {
                     0.5f,  0.5f, 0.0f,     1f, 0f,
                     0.5f, -0.5f, 0.0f,     1f, 1f,
                    -0.5f, -0.5f, 0.0f,     0f, 1f,
                    -0.5f,  0.5f, 0.0f,     0f, 0f
                };
                uint[] Indices =
                {
                    0, 1, 3,
                    1, 2, 3
                };

                Vao = new VertexArrayObject();
                Vbo = new BufferObject<float>(vertices, BufferTargetARB.ArrayBuffer);
                Ebo = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer);
                
                Vao.BindBuffer(ref Vbo);
                Vao.BindBuffer(ref Ebo);

                Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
                Vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, 5, 3);

            }
            // render Cube
            Vao.Bind();
            Gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, null);
        }
        public void Dispose()
        {
            Vao!.Dispose();
            Vbo!.Dispose();
        }
    }
}