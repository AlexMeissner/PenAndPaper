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

    if (grid.isActive)
    {
        vec2 gridPosition = vec2(screenPosition.x, -screenPosition.y);
        float dx = min(mod(gridPosition.x, grid.size), grid.size - mod(gridPosition.x, grid.size));
        float dy = min(mod(gridPosition.y, grid.size), grid.size - mod(gridPosition.y, grid.size));

        float minDistance = clamp(min(dx, dy), 0.0, lineThickness);
        float gridAlpha = 1.0 - (minDistance / lineThickness);

        fragColor = mix(fragColor, grid.color, gridAlpha);
    }
}
