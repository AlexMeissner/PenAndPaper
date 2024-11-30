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

layout(std140) uniform GridBuffer {
    vec4 color;
    float size;
    bool isActive;
} grid;

void main()
{
    vec2 scaledPosition = position * vec2(grid.size, grid.size) + vec2(x, -y);
    gl_Position = camera.viewProjection * vec4(scaledPosition, -0.4, 1.0);
    texCoord = uv;
}
