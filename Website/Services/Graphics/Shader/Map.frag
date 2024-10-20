#version 300 es
precision mediump float;

uniform sampler2D sampler;

layout(std140) uniform CameraBuffer {
    float x;
    float y;
    float zoom;
} camera;

in vec2 texCoord;

out vec4 fragColor;

void main()
{
    fragColor = texture(sampler, texCoord) * vec4(camera.x, camera.y, camera.zoom, 1.0);
}
