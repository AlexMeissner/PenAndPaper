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
    texCoord = uv;

    // mat4 projection = camera.viewProjection;
    // projection[3] = vec4(0, 0, 0, 1);
    // vec4 projectedPos = projection * vec4(position, -0.5, 1.0);
    // screenPosition = projectedPos.xy / projectedPos.w;
    // vec4 projectedPos = camera.viewProjection * vec4(position, -0.5, 1.0);
    // screenPosition = projectedPos.xy;

    //mat4 projection = camera.projection;
    ////projection[3] = vec4(0, 0, 0, 1);
    //projection[3].x = 0.0;
    //projection[3].y = 0.0;
    //vec4 test = projection * vec4(position, 0.0, 1.0);
    //screenPosition = test.xy / test.w;

    mat4 projection = camera.projection;
    //projection[3] = vec4(0, 0, 0, 1);
    screenPosition = (projection * vec4(position, 0.0, 1.0)).xy;

    //screenPosition = position;
}
