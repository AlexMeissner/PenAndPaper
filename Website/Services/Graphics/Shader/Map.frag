#version 300 es
precision highp float;

uniform float width;
uniform float height;
uniform sampler2D sampler;

in vec2 screenPosition;
in vec2 texCoord;

out vec4 fragColor;

layout(std140) uniform GridBuffer {
    vec4 color;
    float size;
    bool isActive;
} grid;

#define PI 3.1415926535897932384626433832795

void main()
{
    /*
    fragColor = texture(sampler, texCoord);

    vec2 gridCoord = vec2(width, height) * texCoord;

    if (grid.isActive)
    {
        //float dx = min(mod(gridCoord.x, grid.size), grid.size - mod(gridCoord.x, grid.size));
        //float dy = min(mod(gridCoord.y, grid.size), grid.size - mod(gridCoord.y, grid.size));
        //
        //float lineWidth = 3.0;
        //float c = smoothstep(0.0, lineWidth, dx);
        ////float c = pow(cos(PI * dx / 2.0), 2.0);
        //fragColor = vec4(c, 0.0, 0.0, 1.0);

        // Calculate the distance from the nearest grid line in screen space
        float dx = mod(screenPosition.x*width, grid.size);
        float dy = mod(screenPosition.y*height, grid.size);

        // Apply grid color if within lineWidth / 2.0 from a grid line
        float lineWidth = 30.0;
        if (dx <= lineWidth / 2.0 || grid.size - dx <= lineWidth / 2.0 || dy <= lineWidth / 2.0 || grid.size - dy <= lineWidth / 2.0)
        {
            fragColor = grid.color;
        }
        else
        {
            fragColor = vec4(1, 1, 1, 1);
        }

        float c = screenPosition.x;
        fragColor = vec4(c, c, c, 1.0);
    }
    */

    /*
    // Convert world position to screen-space
    vec2 u_resolution = vec2(width, height);
    vec2 screenPos = (screenPosition * 0.5 + 0.5) * u_resolution;

    // Calculate line thickness in screen-space units
    float lineWidthInPixels = 0.002;// Set line thickness to 1 pixel
    float cellWidthInScreen = (grid.size / 100.0) * (u_resolution.x / (u_resolution.x - 1.0));

    // Use mod to determine grid line positions in world space, but maintain line thickness in screen-space
    float fx = mod(screenPos.x, cellWidthInScreen);
    float fy = mod(screenPos.y, cellWidthInScreen);

    // Determine if the fragment is near a grid line based on screen-space line width
    float line = step(fx, lineWidthInPixels) + step(fy, lineWidthInPixels);
    float alpha = clamp(line, 0.0, 1.0);

    // Set color of grid lines (white lines on black background)
    vec4 mapImage = texture(sampler, texCoord);

    fragColor = mix(mapImage, grid.color, alpha);

    fragColor = vec4(screenPos, 0.0, 1.0);
    */

    /*
    float lineWidthInPixels = 10.0;
    float cellWidthInScreen = 50.0;

    float fx = mod(screenPosition.x * width, cellWidthInScreen);
    //float fy = mod(screenPosition.y * height, cellWidthInScreen);

    //float line = step(fx, lineWidthInPixels) + step(fy, lineWidthInPixels);
    float line = step(fx, lineWidthInPixels);
    float alpha = clamp(line, 0.0, 1.0);

    fragColor = mix(vec4(1.0, 1.0, 1.0, 1.0), grid.color, alpha);
    //fragColor = vec4(abs(screenPosition.x), abs(screenPosition.y), 0.0, 1.0);
    */

    /*
    float fx = mod(screenPosition.x, grid.size);
    float world = min(fx, grid.size - fx);
    float screen = 0.5 * world / width;
    float d = clamp(screen, -1.0, 1.0);
    float c = 1.0 - abs(d);
    float alpha = clamp(fx, 0.0, 1.0);

    fragColor = mix(grid.color, vec4(1.0, 1.0, 1.0, 1.0), c);

    fragColor = vec4(world, world, world, 1.0);
    
    //if (screen == 0.0)
    //{
    //    fragColor = vec4(1.0, 0.0, 0.0, 1.0);
    //}
    //if (world <= 1.0)
    //{
    //    fragColor = vec4(0.0, 1.0, 0.0, 1.0);
    //}
    //if (fx <= 1.0)
    //{
    //    fragColor = vec4(0.0, 0.0, 1.0, 1.0);
    //}

    fragColor = vec4(screenPosition.x / width, abs(screenPosition.y) / height, 0.0, 1.0);
    */

    float pixel_width = 2.0 / width;
    float grid_size = grid.size * pixel_width;
    float fx = mod(screenPosition.x, grid_size);
    float world = min(fx, grid_size - fx);
    
    
    vec2 s = screenPosition + vec2(1.0, -1.0);



    fragColor = vec4(world, world, world, 1.0);
    //fragColor = vec4(s.x, s.y, 0.0, 1.0);
    //fragColor = vec4(abs(s.x), abs(s.y), 0.0, 1.0);
}
