using System.Numerics;
using Silk.NET.OpenGL;

namespace MyGame
{
    public class Shader : IDisposable
    {
        private uint Handle;
        private GL Gl { get => Program.Gl; }

        public Shader(string vertexPath, string fragmentPath)
        {

            uint vert = 0; 
            uint frag = 0;

            vert = LoadShader(ShaderType.VertexShader, File.ReadAllText(vertexPath));
            frag = LoadShader(ShaderType.FragmentShader, File.ReadAllText(fragmentPath));


            Handle = Gl.CreateProgram();
            Gl.AttachShader(Handle, vert);
            Gl.AttachShader(Handle, frag);
            Gl.LinkProgram(Handle);
            Gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"O programa falhou ao vincular, erro: {Gl.GetProgramInfoLog(Handle)}");
            }
            Gl.DetachShader(Handle, vert);
            Gl.DetachShader(Handle, frag);

            Gl.DeleteShader(vert);
            Gl.DeleteShader(frag);
        }
        private uint LoadShader(ShaderType type, string shaderCode)
        {
            uint shader = Gl.CreateShader(type);
            Gl.ShaderSource(shader, shaderCode);
            
            Gl.CompileShader(shader);
            string infoLog = Gl.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Erro ao compilar o shader: {type}, \nFalhou com erro: {infoLog}");
            }

            return shader;
        }
        public void Use()
        {
            Gl.UseProgram(Handle);
        }

        public void Dispose()
        {
            Gl.DeleteProgram(Handle);
        }
        // uniforms
        private int GetUniform(string name)
        {
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1)
                throw new Exception($"O uniform {name} não foi encontado no Programa shader.");

            return location;
        }
        public void SetUniform(string name, bool value)
        {
            Gl.Uniform1(GetUniform(name), value ? 1 : 0);
        }
        public void SetUniform(string name, int value)
        {
            Gl.Uniform1(GetUniform(name), value);
        }
        public void SetUniform(string name, float value)
        {
            Gl.Uniform1(GetUniform(name), value);
        }
        public void SetUniform(string name, Vector2 value)
        {
            Gl.Uniform2(GetUniform(name), value.X, value.Y);
        }
        public void SetUniform(string name, Vector3 value)
        {
            Gl.Uniform3(GetUniform(name), value.X, value.Y, value.Z);
        }
        public void SetUniform(string name, Vector4 value)
        {
            Gl.Uniform4(GetUniform(name), ref value);
        }
        public void SetUniform(string name, System.Drawing.Color value)
        {
            Gl.Uniform4(GetUniform(name), new Vector4(value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f) );
        }
        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            Gl.UniformMatrix4(GetUniform(name), 1, false, (float*)&value);
        }
    }
}