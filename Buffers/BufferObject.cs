using Silk.NET.OpenGL;

namespace MyGame
{
    // Nossa abstração do objeto buffer.
    public class BufferObject<TDataType> : IDisposable
    where TDataType : unmanaged
    {
        // Nosso handle, buffertype e a instância GL que esta classe usará, são privados porque não têm razão para serem públicos.
        // Na maioria das vezes você deseja abstrair itens para tornar coisas como esta invisíveis.
        private uint Handle;
        private BufferTargetARB _bufferType;
        private GL Gl { get => Program.Gl; }

        public unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
        {
            // Configurando a instância gl e armazenando nosso tipo de buffer.
            _bufferType = bufferType;

            // Obtendo o handle, e então fazendo upload dos dados para o handle.
            Handle = Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                Gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            // Vinculando o objeto buffer, com o tipo de buffer correto.
            Gl.BindBuffer(_bufferType, Handle);
        }

        public void Dispose()
        {
            // Lembre-se de deletar nosso buffer.
            Gl.DeleteBuffer(Handle);
        }
    }
}