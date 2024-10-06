attribute vec2 position;
attribute vec2 uv;

varying vec3 color;

void main()
{
    gl_Position = vec4(position, 0.0, 1.0);
    color = vec3(uv.xy, 0.0);
}
