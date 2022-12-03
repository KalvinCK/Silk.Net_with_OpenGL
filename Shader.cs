using System.Numerics;
using Silk.NET.OpenGL;

namespace MyGame
{
    public class Shader : IDisposable
    {
        private uint Handle;
        private GL Gl { get => Program.Gl; }
        private Dictionary<string, int> uniformsLocation;
        public Shader(string vertexPath_orCode, string fragmentPath_orCode)
        {
            uint vert = 0; 
            uint frag = 0;

            if(vertexPath_orCode.Substring(0, 1) == "#" && vertexPath_orCode.Contains("version"))
            {
                vert = LoadShader(ShaderType.VertexShader, vertexPath_orCode);
                frag = LoadShader(ShaderType.FragmentShader, fragmentPath_orCode);
            }
            else
            {
                vert = LoadShader(ShaderType.VertexShader, File.ReadAllText(vertexPath_orCode));
                frag = LoadShader(ShaderType.FragmentShader, File.ReadAllText(fragmentPath_orCode));
            }


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

            uniformsLocation = new Dictionary<string, int>();
            ProcessAllUniforms();

        }
        /// <summary>
        /// fazer chamadas no opengl é um pouco pesado, por isso vamos armazenar todos os identificadores dos uniforms 
        /// em um dicionario assim tornando as mais performatico 
        /// </summary>
        private void ProcessAllUniforms()
        {
            Gl.GetProgram(Handle, GLEnum.ActiveUniforms, out var numversUniforms);

            for(uint i = 0; i < numversUniforms; i++)
            {
                var name = Gl.GetActiveUniform(Handle, i, out int size, out _);

                if(size == 1)
                {
                    var location = Gl.GetUniformLocation(Handle, name);
                    uniformsLocation.Add(name, location);
                }
                else
                {
                    for(int j = 0; j < size; j++)
                    {
                        var arrayName = name.Substring(0, name.Length - 2) + j + "]";
                        var location = Gl.GetUniformLocation(Handle, arrayName);
                        uniformsLocation.Add(arrayName, location);
                    }
                }
            }
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
        public void SetUniform(string name, bool value)
        {
            Gl.Uniform1(uniformsLocation[name], value ? 1 : 0);
        }
        public void SetUniform(string name, int value)
        {
            Gl.Uniform1(uniformsLocation[name], value);
        }
        public void SetUniform(string name, float value)
        {
            Gl.Uniform1(uniformsLocation[name], value);
        }
        public void SetUniform(string name, Vector2 value)
        {
            Gl.Uniform2(uniformsLocation[name], value.X, value.Y);
        }
        public void SetUniform(string name, Vector3 value)
        {
            Gl.Uniform3(uniformsLocation[name], value.X, value.Y, value.Z);
        }
        public void SetUniform(string name, Vector4 value)
        {
            Gl.Uniform4(uniformsLocation[name], ref value);
        }
        public void SetUniform(string name, System.Drawing.Color value)
        {
            Gl.Uniform4(uniformsLocation[name], new Vector4(value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f) );
        }
        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            Gl.UniformMatrix4(uniformsLocation[name], 1, false, (float*)&value);
        }
    }
}