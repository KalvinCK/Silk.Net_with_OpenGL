using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;

using System.Numerics;

using System.Drawing;

namespace MyGame
{
    class Game : IDisposable
    {
        private GL Gl = Program.Gl!;
        private  Shader LightingShader;
        private Texture texture;
        private  Shader LampShader;
        private SkyBox skyBox;
        private Shader shaderskybox;
        public Game()
        {
            
            LightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            LampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            texture = new Texture("img/silk.png");

            var skyboxfiles = new SkyboxFiles();
            skyboxfiles.path = "img/skybox/";
            
            skyboxfiles.imagens.Add("space_right.png");
            skyboxfiles.imagens.Add("space_left.png");
            skyboxfiles.imagens.Add("space_top.png");
            skyboxfiles.imagens.Add("space_bottom.png");
            skyboxfiles.imagens.Add("space_front.png");
            skyboxfiles.imagens.Add("space_back.png");
            
            skyBox = new SkyBox(skyboxfiles);
            shaderskybox = new Shader("cubemap/shader.vert", "cubemap/shader.frag");
        }
        public void Render()
        {
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

            shaderskybox.Use();
            shaderskybox.SetUniform("View", Camera.ViewMatrix);
            shaderskybox.SetUniform("Projection", Camera.ProjectionMatrix);

            skyBox.Bind();
            shaderskybox.SetUniform("skybox", 0);
            Cube.RenderCube();

            LightingShader.Use();

            //Slightly rotate the cube to give it an angled face to look at
            LightingShader.SetUniform("Model", Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(25f)));
            LightingShader.SetUniform("View", Camera.ViewMatrix);
            LightingShader.SetUniform("Projection", Camera.ProjectionMatrix);
            LightingShader.SetUniform("color", Color.Tomato);

            Cube.RenderCube();

            LampShader.Use();
            var lampMatrix = Matrix4x4.Identity;
            lampMatrix *= Matrix4x4.CreateScale(0.2f);
            lampMatrix *= Matrix4x4.CreateTranslation(new Vector3(1.2f, 1.0f, 2.0f));

            LampShader.SetUniform("Model", lampMatrix);
            LampShader.SetUniform("View", Camera.ViewMatrix);
            LampShader.SetUniform("Projection", Camera.ProjectionMatrix);
            texture.Bind();
            LampShader.SetUniform("TheTexture", 0);
            
            Quad.RenderQuad();

        }
        public void Update()
        {

        }
        public void Resize()
        {

        }
        public void Dispose()
        {
        }
    }
}
// using Silk.NET.Input;
// using Silk.NET.OpenGL;
// using Silk.NET.Windowing;
// using Silk.NET.Maths;

// using System.Numerics;

// using System.Drawing;

// namespace MyGame
// {
//     class Setup : IDisposable
//     {
//         private GL Gl = Program.Gl!;
//         private  BufferObject<float> Vbo;
//         private  VertexArrayObject<float> VaoCube;
//         private  Shader LightingShader;
//         private  Shader LampShader;
        
//         public Setup()
//         {
//             float[] Vertices =
//             {
//                 //X    Y      Z
//                 -0.5f, -0.5f, -0.5f,
//                 0.5f, -0.5f, -0.5f,
//                 0.5f,  0.5f, -0.5f,
//                 0.5f,  0.5f, -0.5f,
//                 -0.5f,  0.5f, -0.5f,
//                 -0.5f, -0.5f, -0.5f,

//                 -0.5f, -0.5f,  0.5f,
//                 0.5f, -0.5f,  0.5f,
//                 0.5f,  0.5f,  0.5f,
//                 0.5f,  0.5f,  0.5f,
//                 -0.5f,  0.5f,  0.5f,
//                 -0.5f, -0.5f,  0.5f,

//                 -0.5f,  0.5f,  0.5f,
//                 -0.5f,  0.5f, -0.5f,
//                 -0.5f, -0.5f, -0.5f,
//                 -0.5f, -0.5f, -0.5f,
//                 -0.5f, -0.5f,  0.5f,
//                 -0.5f,  0.5f,  0.5f,

//                 0.5f,  0.5f,  0.5f,
//                 0.5f,  0.5f, -0.5f,
//                 0.5f, -0.5f, -0.5f,
//                 0.5f, -0.5f, -0.5f,
//                 0.5f, -0.5f,  0.5f,
//                 0.5f,  0.5f,  0.5f,

//                 -0.5f, -0.5f, -0.5f,
//                 0.5f, -0.5f, -0.5f,
//                 0.5f, -0.5f,  0.5f,
//                 0.5f, -0.5f,  0.5f,
//                 -0.5f, -0.5f,  0.5f,
//                 -0.5f, -0.5f, -0.5f,

//                 -0.5f,  0.5f, -0.5f,
//                 0.5f,  0.5f, -0.5f,
//                 0.5f,  0.5f,  0.5f,
//                 0.5f,  0.5f,  0.5f,
//                 -0.5f,  0.5f,  0.5f,
//                 -0.5f,  0.5f, -0.5f
//             };


//             Vbo = new BufferObject<float>(Vertices, BufferTargetARB.ArrayBuffer);
//             VaoCube = new VertexArrayObject<float>(Vbo);

//             VaoCube.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0);

//             LightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
//             LampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

//         }
//         public void Render()
//         {
//             Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

//             VaoCube.Bind();
//             LightingShader.Use();

//             //Slightly rotate the cube to give it an angled face to look at
//             LightingShader.SetUniform("Model", Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(25f)));
//             LightingShader.SetUniform("View", Camera.ViewMatrix);
//             LightingShader.SetUniform("Projection", Camera.ProjectionMatrix);
//             LightingShader.SetUniform("color", Color.Blue);

//             Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);

//             LampShader.Use();
//             var lampMatrix = Matrix4x4.Identity;
//             lampMatrix *= Matrix4x4.CreateScale(0.2f);
//             lampMatrix *= Matrix4x4.CreateTranslation(new Vector3(1.2f, 1.0f, 2.0f));

//             LampShader.SetUniform("Model", lampMatrix);
//             LampShader.SetUniform("View", Camera.ViewMatrix);
//             LampShader.SetUniform("Projection", Camera.ProjectionMatrix);
//             LampShader.SetUniform("color", Color.Tomato);

//             Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
//         }
//         public void Update()
//         {

//         }
//         public void Resize()
//         {

//         }
//         public void Dispose()
//         {
//             Vbo.Dispose();
//             VaoCube.Dispose();
//             LightingShader.Dispose();
//         }
//     }
// }