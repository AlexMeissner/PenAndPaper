
function Initialize(renderContext: string) {

    console.info(document);

    const canvas = document.getElementById(renderContext) as HTMLCanvasElement;

    if (canvas == null) {
        console.error("Could not find canvas");
        return;
    }

    const gl = canvas.getContext("webgl2");

    if (gl == null) {
        console.error("Could not create WebGL context");
        return;
    }

    return gl;
}

function Render(gl: WebGL2RenderingContext) {
    const color: number = 30.0 / 255.0;
    gl.clearColor(color, color, color, 1);
    gl.clear(gl.COLOR_BUFFER_BIT);
}