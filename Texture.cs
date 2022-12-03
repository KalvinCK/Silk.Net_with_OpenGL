using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MyGame
{
    public class Texture : IDisposable
    {
        private uint Handle;
        private GL Gl { get => Program.Gl; }

        public unsafe Texture(string path)
        {

            Handle = Gl.GenTexture();
            Bind();

            // Carregando uma imagem usando imagesharp.
            using (var img = Image.Load<Rgba32>(path))
            {
                // Reserve memória suficiente da gpu para toda a imagem
                Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint) img.Width, (uint) img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);

                img.ProcessPixelRows(accessor =>
                {
                    // ImageSharp 2 não armazena imagens em memória contígua por padrão, então devemos enviar a imagem linha por linha
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        fixed (void* data = accessor.GetRowSpan(y))
                        {
                            // carregando a imagen real
                            Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint) accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
            }

            SetParameters();

        }
        public unsafe Texture(Span<byte> data, uint width, uint height)
        {

            // Gerando o handle opengl;
            Handle = Gl.GenTexture();
            Bind();

            // Queremos a capacidade de criar uma textura usando dados gerados a partir de código também.
            fixed (void* d = &data[0])
            {
                // Configurando os dados de uma textura.
                Gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
                SetParameters();
            }
        }

        private void SetParameters()
        {
            // Configurando alguns parâmetros de textura para que a textura se comporte como esperado.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.Repeat);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.Repeat);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
            // Gerando mipmaps.
            Gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            // Quando vinculamos uma textura, podemos escolher em qual slot de textura podemos vinculá-la.
            Gl.ActiveTexture(textureUnit);
            Gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            Gl.DeleteTexture(Handle);
        }
    }
}