#version 300 es
precision highp float;

uniform sampler2D sampler;
uniform float isMouseOver;

in vec2 texCoord;

out vec4 fragColor;

const float highlightIntensity = 0.1;

void main()
{
    float highlightColor = highlightIntensity * isMouseOver;
    vec4 highlight = vec4(highlightColor, highlightColor, highlightColor, 0.0);

    fragColor = clamp(texture(sampler, texCoord) + highlight, 0.0, 1.0);
}
