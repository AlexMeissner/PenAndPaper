#version 300 es
precision highp float;

in vec2 position;
in vec2 uv;

out vec2 texCoord;

uniform float x;
uniform float y;

layout(std140) uniform CameraBuffer {
    mat4 projection;
    mat4 viewProjection;
} camera;

void main()
{
    vec2 pos = position + vec2(x, -y);
    gl_Position = camera.viewProjection * vec4(pos, -0.3, 1.0);
    texCoord = uv;
}
