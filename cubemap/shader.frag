#version 330 core

out vec4 FragColor;

in vec3 TexCoord;
uniform samplerCube skybox;

void main()
{
    vec3 result = texture(skybox, TexCoord).rgb;
    FragColor = vec4(result, 1.0);
}