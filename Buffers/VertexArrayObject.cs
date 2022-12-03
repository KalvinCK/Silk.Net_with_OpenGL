using Silk.NET.OpenGL;

namespace MyGame
{
    // A abstração do objeto array de vértices.
    public class VertexArrayObject : IDisposable
    {
        private static GL Gl { get => Program.Gl; }
        private uint Handle;
        public VertexArrayObject()
        {
            Handle = Gl.GenVertexArray();
        }
        public void BindBuffer(ref BufferObject<float> vbo)
        {
            Bind();
            vbo.Bind();
        }
        public void BindBuffer(ref BufferObject<uint> ebo)
        {
            Bind();
            ebo.Bind();
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
        {
            // Configurando um ponteiro de atributo de vértice
            Gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint) sizeof(float), (void*) (offSet * sizeof(float)));
            Gl.EnableVertexAttribArray(index);
        }
        public void Bind()
        {
            Gl.BindVertexArray(Handle);
        }
        public void Dispose()
        {
            Gl.DeleteVertexArray(Handle);
        }
    }
}