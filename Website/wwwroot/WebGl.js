class UniformBuffer {
    constructor(gl) {
        this.gl = gl;
        this.buffer = this.gl.createBuffer();
    }
}
class Camera extends UniformBuffer {
    constructor(gl) {
        super(gl);
        this.x = 0.0;
        this.y = 0.0;
        this.near = 0.1;
        this.far = 1.0;
        this.zoomLevel = 0;
        this.zoomFactor = 1.0;
        this.zoomSpeed = 0.1;
        this.BINDING_POINT_NUMBER = 0;
        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);
        const matrix = this.createViewProjectionMatrix(1.0, 1.0);
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, matrix.byteLength * 2, this.gl.DYNAMIC_DRAW);
    }
    GetBindingPoint() {
        return this.BINDING_POINT_NUMBER;
    }
    GetName() {
        return 'CameraBuffer';
    }
    createProjectionMatrix(width, height) {
        const left = 0.0;
        const right = width * this.zoomFactor;
        const top = 0.0;
        const bottom = -height * this.zoomFactor;
        const orthogonalMatrix = new Float32Array(16);
        orthogonalMatrix[0] = 2.0 / (right - left);
        orthogonalMatrix[1] = 0.0;
        orthogonalMatrix[2] = 0.0;
        orthogonalMatrix[3] = 0.0;
        orthogonalMatrix[4] = 0.0;
        orthogonalMatrix[5] = 2.0 / (top - bottom);
        orthogonalMatrix[6] = 0.0;
        orthogonalMatrix[7] = 0.0;
        orthogonalMatrix[8] = 0.0;
        orthogonalMatrix[9] = 0.0;
        orthogonalMatrix[10] = -2.0 / (this.far - this.near);
        orthogonalMatrix[11] = 0.0;
        orthogonalMatrix[12] = -(right + left) / (right - left);
        orthogonalMatrix[13] = -(top + bottom) / (top - bottom);
        orthogonalMatrix[14] = -(this.far + this.near) / (this.far - this.near);
        orthogonalMatrix[15] = 1.0;
        return orthogonalMatrix;
    }
    createViewProjectionMatrix(width, height) {
        const left = 0.0;
        const right = width * this.zoomFactor;
        const top = 0.0;
        const bottom = -height * this.zoomFactor;
        const orthogonalMatrix = new Float32Array(16);
        orthogonalMatrix[0] = 2.0 / (right - left);
        orthogonalMatrix[1] = 0.0;
        orthogonalMatrix[2] = 0.0;
        orthogonalMatrix[3] = 0.0;
        orthogonalMatrix[4] = 0.0;
        orthogonalMatrix[5] = 2.0 / (top - bottom);
        orthogonalMatrix[6] = 0.0;
        orthogonalMatrix[7] = 0.0;
        orthogonalMatrix[8] = 0.0;
        orthogonalMatrix[9] = 0.0;
        orthogonalMatrix[10] = -2.0 / (this.far - this.near);
        orthogonalMatrix[11] = 0.0;
        orthogonalMatrix[12] = (2 * this.x - right - left) / (right - left);
        orthogonalMatrix[13] = (2 * this.y - top - bottom) / (top - bottom);
        orthogonalMatrix[14] = -(this.far + this.near) / (this.far - this.near);
        orthogonalMatrix[15] = 1.0;
        return orthogonalMatrix;
    }
    destroy() {
        this.gl.deleteBuffer(this.buffer);
    }
    move(x, y) {
        this.x += x * this.zoomFactor;
        this.y -= y * this.zoomFactor;
    }
    reset() {
        this.x = 0.0;
        this.y = 0.0;
        this.zoomLevel = 0;
        this.zoomFactor = 1.0;
    }
    updateBuffer(width, height) {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);
        const projectionMatrix = this.createProjectionMatrix(width, height);
        const viewProjectionMatrix = this.createViewProjectionMatrix(width, height);
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, projectionMatrix);
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, projectionMatrix.byteLength, viewProjectionMatrix);
    }
    zoom(cursorX, cursorY, direction) {
        const offsetX = cursorX * this.zoomFactor - this.x;
        const offsetY = cursorY * this.zoomFactor + this.y;
        this.zoomLevel += direction < 0.0 ? 1 : -1;
        this.zoomFactor = 1.0 - this.zoomLevel * this.zoomSpeed;
        this.x = cursorX * this.zoomFactor - offsetX;
        this.y = -cursorY * this.zoomFactor + offsetY;
    }
}
class Grid extends UniformBuffer {
    constructor(gl) {
        super(gl);
        this.isActive = false;
        this.color = new Float32Array(4);
        this.size = 1.0;
        this.BINDING_POINT_NUMBER = 1;
        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);
        const gridData = this.createBufferData();
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, gridData.byteLength, this.gl.DYNAMIC_DRAW);
    }
    GetBindingPoint() {
        return this.BINDING_POINT_NUMBER;
    }
    GetName() {
        return 'GridBuffer';
    }
    destroy() {
        this.gl.deleteBuffer(this.buffer);
    }
    updateBuffer() {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);
        const gridData = this.createBufferData();
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, gridData);
    }
    createBufferData() {
        const data = new Float32Array(6);
        data.set(this.color, 0);
        data[4] = this.size;
        data[5] = this.isActive ? 1.0 : 0.0;
        return data;
    }
}
class ShaderProgram {
    constructor(gl, camera) {
        this.gl = gl;
        this.program = gl.createProgram();
        this.camera = camera;
    }
    addUniformBuffer(buffer) {
        const uniformBlockIndex = this.gl.getUniformBlockIndex(this.program, buffer.GetName());
        this.gl.uniformBlockBinding(this.program, uniformBlockIndex, buffer.GetBindingPoint());
    }
    bind() {
        this.gl.useProgram(this.program);
    }
    compile(vertexSource, fragmentSource) {
        const vertexShader = this.gl.createShader(this.gl.VERTEX_SHADER);
        this.gl.shaderSource(vertexShader, vertexSource);
        this.gl.compileShader(vertexShader);
        const vertexCompileStatus = this.gl.getShaderParameter(vertexShader, this.gl.COMPILE_STATUS);
        if (vertexCompileStatus == false) {
            const compileLog = this.gl.getShaderInfoLog(vertexShader);
            console.error(compileLog);
            return false;
        }
        const fragmentShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.gl.shaderSource(fragmentShader, fragmentSource);
        this.gl.compileShader(fragmentShader);
        const fragmentCompileStatus = this.gl.getShaderParameter(fragmentShader, this.gl.COMPILE_STATUS);
        if (fragmentCompileStatus == false) {
            const compileLog = this.gl.getShaderInfoLog(fragmentShader);
            console.error(compileLog);
            this.gl.deleteShader(vertexShader);
            return false;
        }
        this.gl.attachShader(this.program, vertexShader);
        this.gl.attachShader(this.program, fragmentShader);
        this.gl.linkProgram(this.program);
        const linkStatus = this.gl.getProgramParameter(this.program, this.gl.LINK_STATUS);
        if (linkStatus == false) {
            const linkLog = this.gl.getProgramInfoLog(this.program);
            console.error(linkLog);
            this.gl.deleteShader(vertexShader);
            this.gl.deleteShader(fragmentShader);
            return false;
        }
        this.gl.deleteShader(vertexShader);
        this.gl.deleteShader(fragmentShader);
        return true;
    }
    destroy() {
        this.gl.deleteProgram(this.program);
    }
    release() {
        this.gl.useProgram(null);
    }
}
class TexturedQuad {
    constructor(gl) {
        this.floatUniforms = new Map();
        this.width = 0;
        this.height = 0;
        this.gl = gl;
        this.vertexArray = gl.createVertexArray();
        this.vertexBuffer = gl.createBuffer();
        this.uvBuffer = gl.createBuffer();
        this.indexBuffer = gl.createBuffer();
        this.texture = gl.createTexture();
    }
    destroy() {
        this.gl.deleteTexture(this.texture);
        this.gl.deleteVertexArray(this.vertexArray);
        this.gl.deleteBuffer(this.vertexBuffer);
        this.gl.deleteBuffer(this.uvBuffer);
        this.gl.deleteBuffer(this.indexBuffer);
    }
    getWidth() {
        return this.width;
    }
    getHeight() {
        return this.height;
    }
    render() {
        if (this.shaderProgram == null) {
            console.error("No shader bound.");
            return;
        }
        this.gl.bindVertexArray(this.vertexArray);
        this.shaderProgram.bind();
        const samplerLocation = this.gl.getUniformLocation(this.shaderProgram.program, "sampler");
        if (samplerLocation != null) {
            this.gl.activeTexture(this.gl.TEXTURE0);
            this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
            this.gl.uniform1i(samplerLocation, 0);
        }
        this.floatUniforms.forEach((value, name) => {
            const location = this.gl.getUniformLocation(this.shaderProgram.program, name);
            if (location != null) {
                this.gl.uniform1f(location, value);
            }
        });
        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    }
    setShaderProgram(program) {
        this.shaderProgram = program;
    }
    setTexture(imageBase64) {
        const image = new Image();
        // the image is not available synchronously when setting the source
        // wait for it to be finished before working with it
        image.onload = () => {
            this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
            this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, true);
            this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);
            if (this.isPowerOf2(image.width) && this.isPowerOf2(image.height)) {
                this.gl.generateMipmap(this.gl.TEXTURE_2D);
            }
            else {
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.CLAMP_TO_EDGE);
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.CLAMP_TO_EDGE);
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.LINEAR);
            }
            this.gl.bindTexture(this.gl.TEXTURE_2D, null);
        };
        image.src = imageBase64;
    }
    setUniform(name, value) {
        this.floatUniforms.set(name, value);
    }
    getUniform(name) {
        return this.floatUniforms.get(name);
    }
    setVertices(srcData) {
        console.assert(srcData.length == 8);
        this.width = Math.abs(srcData[2]);
        this.height = Math.abs(srcData[5]);
        this.setUniform("width", this.width);
        this.setUniform("height", this.height);
        this.gl.bindVertexArray(this.vertexArray);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vertexBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(srcData), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(0, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(0);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        const uvs = [0.0, 1.0, 1.0, 1.0, 0.0, 0.0, 1.0, 0.0];
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.uvBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(uvs), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(1, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(1);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        const indices = [0, 1, 2, 2, 1, 3];
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.indexBuffer);
        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indices), this.gl.STATIC_DRAW);
    }
    updateVertices(newData) {
        console.assert(newData.length == 8);
        this.width = Math.abs(newData[2]);
        this.height = Math.abs(newData[5]);
        this.setUniform("width", this.width);
        this.setUniform("height", this.height);
        this.gl.bindVertexArray(this.vertexArray);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vertexBuffer);
        this.gl.bufferSubData(this.gl.ARRAY_BUFFER, 0, new Float32Array(newData));
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
    }
    isPowerOf2(value) {
        return (value & (value - 1)) === 0;
    }
}
class Token extends TexturedQuad {
    constructor(gl, name) {
        super(gl);
        this.name = name;
    }
}
class MouseIndicator extends TexturedQuad {
    constructor(gl) {
        super(gl);
        this.setUniform("alpha", 0.0);
    }
    setPosition(x, y) {
        this.setUniform("x", x);
        this.setUniform("y", y);
        this.setUniform("alpha", 1.0);
    }
    render() {
        this.gl.enable(this.gl.BLEND);
        this.gl.blendFunc(this.gl.SRC_ALPHA, this.gl.ONE_MINUS_SRC_ALPHA);
        super.render();
        this.gl.disable(this.gl.BLEND);
        const alpha = Math.max(0.0, this.getUniform("alpha") - 0.01);
        console.info(alpha);
        this.setUniform("alpha", alpha);
    }
}
class RenderContext {
    constructor() {
        this.tokens = [];
        this.mouseIndicators = [];
    }
    initialize(identifier) {
        this.canvas = document.getElementById(identifier);
        if (this.canvas == null) {
            console.error("Could not find canvas");
            return false;
        }
        this.gl = this.canvas.getContext("webgl2");
        if (this.gl == null) {
            console.error("Could not create WebGL context");
            return false;
        }
        this.grid = new Grid(this.gl);
        this.camera = new Camera(this.gl);
        this.gl.clearColor(1.0, 0.5, 0.5, 1.0);
        this.canvas.addEventListener("mousemove", (event) => this.onMouseMove(event));
        this.canvas.addEventListener("wheel", (event) => this.onMouseWheel(event));
        this.canvas.oncontextmenu = () => false;
        this.render = this.render.bind(this);
        window.requestAnimationFrame(this.render);
        return true;
    }
    addMouseIndicator(mouseIndicator) {
        this.mouseIndicators.push(mouseIndicator);
    }
    addToken(token) {
        this.tokens.push(token);
    }
    clearTokens() {
        this.tokens.forEach(token => token.destroy());
        this.tokens = [];
    }
    cleanup() {
        this.clearTokens();
        if (this.map != null) {
            this.map.destroy();
            this.map = null;
        }
    }
    createShaderProgram() {
        return new ShaderProgram(this.gl, this.camera);
    }
    createTexturedQuad() {
        return new TexturedQuad(this.gl);
    }
    createToken(name) {
        return new Token(this.gl, name);
    }
    createMouseIndicator() {
        return new MouseIndicator(this.gl);
    }
    getCamera() {
        return this.camera;
    }
    getGrid() {
        return this.grid;
    }
    destroy() {
        this.cleanup();
        // ToDo: Clean up 'gl' context
    }
    onMouseMove(event) {
        switch (event.buttons) {
            case 0: // no mouse button
                break;
            case 1: // left mouse button
                break;
            case 2: // right mouse button
                this.camera.move(event.movementX, event.movementY);
                break;
            case 3: // left and right mouse buttons
                break;
            default:
                break;
        }
    }
    onMouseWheel(event) {
        this.camera.zoom(event.clientX, event.clientY, event.deltaY);
    }
    render(timeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);
        if (this.camera != null) {
            this.camera.updateBuffer(this.canvas.width, this.canvas.height);
        }
        if (this.grid != null) {
            this.grid.updateBuffer();
        }
        if (this.map != null) {
            this.map.render();
        }
        this.tokens.forEach(token => {
            token.render();
        });
        this.mouseIndicators.forEach(mouseIndicator => {
            mouseIndicator.render();
        });
        window.requestAnimationFrame(this.render);
    }
    setMap(map) {
        this.map = map;
        this.camera.reset();
    }
    updateGrid(isActive, size, color) {
        this.grid.isActive = isActive;
        this.grid.size = size;
        this.grid.color = color;
    }
}
function createRenderContext() {
    return new RenderContext();
}
