#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

out vec2 TexCoord;
void main()
{
    gl_Position = Projection * View * Model * vec4(aPos, 1.0);
    TexCoord = aTexCoords;
}