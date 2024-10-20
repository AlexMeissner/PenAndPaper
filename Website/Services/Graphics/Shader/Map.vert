#version 300 es
precision mediump float;

in vec2 position;
in vec2 uv;

out vec2 texCoord;

void main()
{
    gl_Position = vec4(position, 0.0, 1.0);
    texCoord = uv;
}
