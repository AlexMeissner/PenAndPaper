function Initialize(renderContext) {
    console.info(document);
    var canvas = document.getElementById(renderContext);
    if (canvas == null) {
        console.error("Could not find canvas");
        return;
    }
    var gl = canvas.getContext("webgl2");
    if (gl == null) {
        console.error("Could not create WebGL context");
        return;
    }
    return gl;
}
function Render(gl) {
    var color = 30.0 / 255.0;
    gl.clearColor(color, color, color, 1);
    gl.clear(gl.COLOR_BUFFER_BIT);
}
//# sourceMappingURL=WebGl.js.map