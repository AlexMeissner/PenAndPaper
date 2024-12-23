#version 300 es
precision highp float;

in vec2 position;
in vec2 uv;

out vec2 screenPosition;
out vec2 texCoord;

layout(std140) uniform CameraBuffer {
    mat4 projection;
    mat4 viewProjection;
} camera;

void main()
{
    gl_Position = camera.viewProjection * vec4(position, -0.5, 1.0);
    screenPosition = position;
    texCoord = uv;
}
