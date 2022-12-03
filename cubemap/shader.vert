#version 460 core

layout (location = 0) in vec3 aPos;

uniform mat4 View;
uniform mat4 Projection;

out vec3 TexCoord;
void main()
{
    vec3 pos = aPos * vec3(500.0);
    mat4 view = mat4(mat3(View));
    vec4 clipPos = Projection * view * vec4(pos, 1.0);
    gl_Position = clipPos.xyzw;
    TexCoord = aPos;
}