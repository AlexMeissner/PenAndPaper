function attachShader(gl, program, shader) {
    gl.attachShader(program, shader);
    gl.ARRAY_BUFFER;
}
function bindBuffer(gl, target, buffer) {
    gl.bindBuffer(target, buffer);
}
function bindVertexArray(gl, vao) {
    gl.bindVertexArray(vao);
}
function bufferData(gl, target, srcData, usage) {
    gl.bufferData(target, new Float32Array(srcData), usage);
}
function clear(gl) {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
}
function clearColor(gl, red, green, blue, alpha) {
    gl.clearColor(red, green, blue, alpha);
}
function compileShader(gl, shader) {
    gl.compileShader(shader);
}
function createBuffer(gl) {
    return gl.createBuffer();
}
function createProgram(gl) {
    return gl.createProgram();
}
function createShader(gl, type) {
    return gl.createShader(type);
}
function createVertexArray(gl) {
    return gl.createVertexArray();
}
function deleteProgram(gl, program) {
    gl.deleteProgram(program);
}
function drawElements(gl, mode, count, type, offset) {
    gl.drawElements(mode, count, type, offset);
}
function enableVertexAttribArray(gl, index) {
    gl.enableVertexAttribArray(index);
}
function getContext(elementName) {
    var canvas = document.getElementById(elementName);
    if (canvas == null) {
        console.error("Could not find canvas");
        return null;
    }
    var gl = canvas.getContext("webgl2");
    if (gl == null) {
        console.error("Could not create WebGL context");
        return null;
    }
    return gl;
}
function getProgramParameter(gl, program, pname) {
    return gl.getProgramParameter(program, pname);
}
function getUniformLocation(gl, program, name) {
    return gl.getUniformLocation(program, name);
}
function linkProgram(gl, program) {
    gl.linkProgram(program);
}
// ToDo: Remove
// https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-8.0
function render(timeStamp) {
    DotNet.invokeMethodAsync("Website", "Render")
        .then(function () {
        window.requestAnimationFrame(render);
    })
        .catch(function (err) {
        console.error("Error invoking C# Render method:", err);
    });
}
function shaderSource(gl, shader, source) {
    gl.shaderSource(shader, source);
}
function startRenderLoop() {
    window.requestAnimationFrame(render);
}
function uniform1f(gl, location, x) {
    gl.uniform1f(location, x);
}
function useProgram(gl, program) {
    gl.useProgram(program);
}
function vertexAttribPointer(gl, index, size, type, normalized, stride, offset) {
    gl.vertexAttribPointer(index, size, type, normalized, stride, offset);
}
function viewport(gl, x, y, width, height) {
    gl.viewport(x, y, width, height);
}
//# sourceMappingURL=WebGl.js.map