#version 300 es
precision highp float;

in vec2 texCoord;

uniform float r;
uniform float g;
uniform float b;
uniform float alpha;

out vec4 fragColor;

void main()
{
    vec2 uv = texCoord / vec2(1.0, 1.0) * 2.0 - 1.0;

    float dist = length(uv);
    float alphaFactor = pow(1.0 - dist, 0.5);

    vec3 color = vec3(r, g, b);

    fragColor = vec4(color, alphaFactor * alpha);
}
