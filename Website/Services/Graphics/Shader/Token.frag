#version 300 es
precision highp float;

uniform sampler2D sampler;
uniform float isMouseOver;
uniform float isLeftMouseButtonDown;

in vec2 texCoord;

out vec4 fragColor;

const float highlightIntensity = 0.1;
const float selectionScalingFactor = 0.9;
const vec2 center = vec2(0.5, 0.5);

void main()
{
    float selectionFactor = isMouseOver * isLeftMouseButtonDown;
    vec2 uv = mix(texCoord, center + (texCoord - center) * selectionScalingFactor, selectionFactor);

    float highlightColor = highlightIntensity * isMouseOver;
    vec4 highlight = vec4(highlightColor, highlightColor, highlightColor, 0.0);

    vec4 textureColor = texture(sampler, uv);

    fragColor = clamp(textureColor + highlight, 0.0, 1.0);
}