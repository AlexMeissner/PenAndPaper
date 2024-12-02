#version 300 es
precision highp float;

in vec2 texCoord;

uniform float alpha;

out vec4 fragColor;

const float outerRadius = 1.0;
const float innerRadius = 0.5;

void main()
{
    vec2 uv = texCoord / vec2(1.0, 1.0) * 2.0 - 1.0;

    float dist = length(uv);

    float ringWidth = 0.5 * (outerRadius - innerRadius);
    float torusEdge = smoothstep(innerRadius, innerRadius + ringWidth, dist) *  (1.0 - smoothstep(outerRadius - ringWidth, outerRadius, dist));

    vec3 color = vec3(1.0, 0.0, 0.0);
    fragColor = vec4(color, torusEdge*alpha);
}
