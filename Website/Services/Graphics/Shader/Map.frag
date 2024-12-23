#version 300 es
precision highp float;

uniform sampler2D sampler;

in vec2 screenPosition;
in vec2 texCoord;

out vec4 fragColor;

layout(std140) uniform GridBuffer {
    vec4 color;
    float size;
    bool isActive;
} grid;

const float lineThickness = 1.0;

void main()
{
    fragColor = texture(sampler, texCoord);

    if (!grid.isActive)
    {
        return;
    }

    vec2 gridPosition = vec2(screenPosition.x, -screenPosition.y);
    vec2 offset = mod(gridPosition, grid.size);
    vec2 minimalOffset = min(offset, grid.size - offset);

    float minDistance = min(minimalOffset.x, minimalOffset.y);
    float gridAlpha = smoothstep(lineThickness, 0.0, minDistance);

    fragColor = mix(fragColor, grid.color, gridAlpha);
}
