#version 300 es
precision mediump float;

in vec2 position;
in vec2 uv;

out vec2 texCoord;

layout(std140) uniform CameraBuffer {
    mat4 projection;
} camera;

void main()
{
    gl_Position = camera.projection * vec4(position, -0.5, 1.0);
    texCoord = uv;
}
