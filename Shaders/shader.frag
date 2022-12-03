#version 460 core

out vec4 FragColor;

in vec2 TexCoord;
uniform sampler2D TheTexture;


void main()
{
    FragColor = texture(TheTexture, TexCoord);
}