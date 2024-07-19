declare var DotNet: any;

function attachShader(gl: WebGL2RenderingContext, program: WebGLProgram, shader: WebGLShader) {
    gl.attachShader(program, shader);
    gl.ARRAY_BUFFER;
}

function bindBuffer(gl: WebGL2RenderingContext, target: GLenum, buffer: WebGLBuffer) {
    gl.bindBuffer(target, buffer);
}

function bindVertexArray(gl: WebGL2RenderingContext, vao: WebGLVertexArrayObject) {
    gl.bindVertexArray(vao);
}

function bufferData(gl: WebGL2RenderingContext, target: GLenum, srcData: number[], usage: number) {
    gl.bufferData(target, new Float32Array(srcData), usage);
}

function clear(gl: WebGL2RenderingContext) {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
}

function clearColor(gl: WebGL2RenderingContext, red: number, green: number, blue: number, alpha: number) {
    gl.clearColor(red, green, blue, alpha);
}

function compileShader(gl: WebGL2RenderingContext, shader: WebGLShader) {
    gl.compileShader(shader);
}

function createBuffer(gl: WebGL2RenderingContext) {
    return gl.createBuffer();
}

function createProgram(gl: WebGL2RenderingContext) {
    return gl.createProgram();
}

function createShader(gl: WebGL2RenderingContext, type: number) {
    return gl.createShader(type);
}

function createVertexArray(gl: WebGL2RenderingContext) {
    return gl.createVertexArray();
}

function deleteProgram(gl: WebGL2RenderingContext, program: WebGLProgram) {
    gl.deleteProgram(program);
}

function drawElements(gl: WebGL2RenderingContext, mode: number, count: number, type: number, offset: number) {
    gl.drawElements(mode, count, type, offset);
}

function enableVertexAttribArray(gl: WebGL2RenderingContext, index: number) {
    gl.enableVertexAttribArray(index);
}

function getContext(elementName: string) {

    const canvas = document.getElementById(elementName) as HTMLCanvasElement;

    if (canvas == null) {
        console.error("Could not find canvas");
        return null;
    }

    const gl = canvas.getContext("webgl2");

    if (gl == null) {
        console.error("Could not create WebGL context");
        return null;
    }

    return gl;
}

function getProgramParameter(gl: WebGL2RenderingContext, program: WebGLProgram, pname: number) {
    return gl.getProgramParameter(program, pname);
}

function getUniformLocation(gl: WebGL2RenderingContext, program: WebGLProgram, name: string) {
    return gl.getUniformLocation(program, name);
}

function linkProgram(gl: WebGL2RenderingContext, program: WebGLProgram) {
    gl.linkProgram(program);
}

// ToDo: Remove
// https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-8.0
function render(timeStamp: DOMHighResTimeStamp) {
    DotNet.invokeMethodAsync("Website", "Render")
        .then(() => {
            window.requestAnimationFrame(render);
        })
        .catch(err => {
            console.error("Error invoking C# Render method:", err);
        });
}

function shaderSource(gl: WebGL2RenderingContext, shader: WebGLShader, source: string) {
    gl.shaderSource(shader, source);
}

function startRenderLoop() {
    window.requestAnimationFrame(render);
}

function uniform1f(gl: WebGL2RenderingContext, location: WebGLUniformLocation, x: number) {
    gl.uniform1f(location, x);
}

function useProgram(gl: WebGL2RenderingContext, program: WebGLProgram) {
    gl.useProgram(program);
}

function vertexAttribPointer(gl: WebGL2RenderingContext, index: number, size: number, type: number, normalized: boolean, stride: number, offset: number) {
    gl.vertexAttribPointer(index, size, type, normalized, stride, offset);
}

function viewport(gl: WebGL2RenderingContext, x: number, y: number, width: number, height: number) {
    gl.viewport(x, y, width, height);
}
