function drawElements(gl: WebGL2RenderingContext, mode: number, count: number, type: number, offset: number) {
    gl.drawElements(mode, count, type, offset);
}

function getUniformLocation(gl: WebGL2RenderingContext, program: WebGLProgram, name: string) {
    return gl.getUniformLocation(program, name);
}

function uniform1f(gl: WebGL2RenderingContext, location: WebGLUniformLocation, x: number) {
    gl.uniform1f(location, x);
}

class Camera {
    x: GLfloat = 0.0;
    y: GLfloat = 0.0;
    zoom: GLfloat = 1.0;

    gl: WebGL2RenderingContext;
    buffer: WebGLBuffer;

    readonly BINDING_POINT_NUMBER: GLuint = 0;

    constructor(gl: WebGL2RenderingContext) {
        this.gl = gl;
        this.buffer = this.gl.createBuffer();

        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);

        const matrix = this.createOrthographicMatrix(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, matrix.byteLength, this.gl.DYNAMIC_DRAW);
    }

    createOrthographicMatrix(left: GLfloat, right: GLfloat, bottom: GLfloat, top: GLfloat, near: GLfloat, far: GLfloat): Float32Array {
        //const orthogonalMatrix = new Float32Array(16);
        //
        //orthogonalMatrix[0] = 2.0 / (right - left);
        //orthogonalMatrix[1] = 0.0;
        //orthogonalMatrix[2] = 0.0;
        //orthogonalMatrix[3] = 0.0;
        //orthogonalMatrix[4] = 0.0;
        //orthogonalMatrix[5] = 2.0 / (top - bottom);
        //orthogonalMatrix[6] = 0.0;
        //orthogonalMatrix[7] = 0.0;
        //orthogonalMatrix[8] = 0.0;
        //orthogonalMatrix[9] = 0.0;
        //orthogonalMatrix[10] = -2.0 / (far - near);
        //orthogonalMatrix[11] = 0.0;
        //orthogonalMatrix[12] = -(right + left) / (right - left);
        //orthogonalMatrix[13] = -(top + bottom) / (top - bottom);
        //orthogonalMatrix[14] = -(far + near) / (far - near);
        //orthogonalMatrix[15] = 1.0;
        //
        //return orthogonalMatrix;

        // Transposed?
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
        orthogonalMatrix[10] = -2.0 / (far - near);
        orthogonalMatrix[11] = 0.0;

        orthogonalMatrix[12] = (left + right) / (left - right);
        orthogonalMatrix[13] = (bottom + top) / (bottom - top);
        orthogonalMatrix[14] = (far + near) / (near - far);
        orthogonalMatrix[15] = 1.0;

        return orthogonalMatrix;
    }

    destroy(): void {
        this.gl.deleteBuffer(this.buffer);
    }

    updateBuffer(width: GLfloat, height: GLfloat): void {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);

        const left = this.x;
        const right = this.x + width;//1.0;//width;
        const bottom = this.y - height;//-1.0;// height;
        const top = this.y;// 0.0;
        //const left = this.x - width / 2.0;
        //const right = this.x + width / 2.0;
        //const bottom = this.y + height / 2.0;
        //const top = this.y - height / 2.0;
        const near = 0.1;
        const far = 1.0;
        const matrix = this.createOrthographicMatrix(left, right, bottom, top, near, far);

        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, matrix);
    }
}

class ShaderProgram {
    gl: WebGL2RenderingContext;
    program: WebGLProgram;
    camera: Camera;

    constructor(gl: WebGL2RenderingContext, camera: Camera) {
        this.gl = gl;
        this.program = gl.createProgram();
        this.camera = camera;
    }

    bind(): void {
        this.gl.useProgram(this.program);
    }

    compile(vertexSource: string, fragmentSource: string): boolean {
        const vertexShader = this.gl.createShader(this.gl.VERTEX_SHADER);
        this.gl.shaderSource(vertexShader, vertexSource);
        this.gl.compileShader(vertexShader);
        const vertexCompileStatus = this.gl.getShaderParameter(vertexShader, this.gl.COMPILE_STATUS) as GLboolean;
        if (vertexCompileStatus == false) {
            const compileLog = this.gl.getShaderInfoLog(vertexShader);
            console.error(compileLog);
            return false;
        }

        const fragmentShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.gl.shaderSource(fragmentShader, fragmentSource);
        this.gl.compileShader(fragmentShader);
        const fragmentCompileStatus = this.gl.getShaderParameter(fragmentShader, this.gl.COMPILE_STATUS) as GLboolean;
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

        const cameraBufferIndex = this.gl.getUniformBlockIndex(this.program, 'CameraBuffer');
        this.gl.uniformBlockBinding(this.program, cameraBufferIndex, this.camera.BINDING_POINT_NUMBER);

        return true;
    }

    destroy(): void {
        this.gl.deleteProgram(this.program);
    }

    release(): void {
        this.gl.useProgram(null);
    }
}

class Quad {
    gl: WebGL2RenderingContext;
    vertexArray: WebGLVertexArrayObject;
    vertexBuffer: WebGLBuffer;
    uvBuffer: WebGLBuffer;
    indexBuffer: WebGLBuffer;
    shaderProgram: ShaderProgram | null;

    constructor(gl: WebGL2RenderingContext) {
        this.gl = gl;
        this.vertexArray = gl.createVertexArray();
        this.vertexBuffer = gl.createBuffer();
        this.uvBuffer = gl.createBuffer();
        this.indexBuffer = gl.createBuffer();
    }

    destroy(): void {
        this.gl.deleteVertexArray(this.vertexArray);
        this.gl.deleteBuffer(this.vertexBuffer);
        this.gl.deleteBuffer(this.uvBuffer);
        this.gl.deleteBuffer(this.indexBuffer);
    }

    setShaderProgram(program: ShaderProgram): void {
        this.shaderProgram = program;
    }

    setVertices(srcData: number[]): void {
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
}

class TexturedQuad extends Quad {
    texture: WebGLTexture;

    constructor(gl: WebGL2RenderingContext) {
        super(gl);
        this.texture = gl.createTexture();
    }

    destroy(): void {
        this.gl.deleteTexture(this.texture);
        super.destroy();
    }

    render(): void {
        if (this.shaderProgram == null) {
            console.error("No shader bound.");
            return;
        }

        this.gl.bindVertexArray(this.vertexArray);
        this.shaderProgram.bind();

        this.gl.activeTexture(this.gl.TEXTURE0);
        this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
        const samplerLocation = this.gl.getUniformLocation(this.shaderProgram.program, "sampler");
        this.gl.uniform1i(samplerLocation, 0);

        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    }

    setTexture(imageBase64: string): void {
        const image = new Image();

        // the image is not available synchronously when setting the source
        // wait for it to be finished before working with it
        image.onload = () => {
            this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
            this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, true);
            this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);

            if (this.isPowerOf2(image.width) && this.isPowerOf2(image.height)) {
                this.gl.generateMipmap(this.gl.TEXTURE_2D);
            } else {
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.CLAMP_TO_EDGE);
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.CLAMP_TO_EDGE);
                this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.LINEAR);
            }

            this.gl.bindTexture(this.gl.TEXTURE_2D, null);
        }

        image.src = imageBase64;
    }

    isPowerOf2(value: number): boolean {
        return (value & (value - 1)) === 0;
    }
}

class Grid extends Quad {
    color: Float32Array;
    size: GLuint;

    constructor(gl: WebGL2RenderingContext, color: Float32Array, size: GLuint) {
        super(gl);
        this.color = color;
        this.size = size;
    }
}

class Token extends TexturedQuad {
    name: string;

    constructor(gl: WebGL2RenderingContext, name: string) {
        super(gl);
        this.name = name;
    }
}

class RenderContext {
    canvas: HTMLCanvasElement | null;
    gl: WebGL2RenderingContext | null;
    map: TexturedQuad | null;
    grid: Grid | null; // ToDo: Combine Grid with map? gridsize? gridcolor? but same size as map! maybe grid inherits texturedtoken as map?
    tokens: Token[] = [];
    camera: Camera | null;

    initialize(identifier: string): boolean {
        this.canvas = document.getElementById(identifier) as HTMLCanvasElement | null;

        if (this.canvas == null) {
            console.error("Could not find canvas");
            return false;
        }

        this.gl = this.canvas.getContext("webgl2");

        if (this.gl == null) {
            console.error("Could not create WebGL context");
            return false;
        }

        this.gl.clearColor(1.0, 0.5, 0.5, 1.0);

        this.camera = new Camera(this.gl);

        this.canvas.addEventListener("mousemove", (event: MouseEvent): void => this.onMouseMove(event));
        this.canvas.oncontextmenu = () => false;

        this.render = this.render.bind(this);
        window.requestAnimationFrame(this.render);

        return true;
    }

    addToken(token: Token): void {
        this.tokens.push(token);
    }

    cleanup(): void {
        this.tokens.forEach(token => token.destroy());
        this.tokens = [];

        if (this.grid != null) {
            this.grid.destroy();
            this.grid = null;
        }

        if (this.map != null) {
            this.map.destroy();
            this.map = null;
        }
    }

    createShaderProgram(): ShaderProgram {
        return new ShaderProgram(this.gl, this.camera);
    }

    createTexturedQuad(): TexturedQuad {
        return new TexturedQuad(this.gl);
    }

    destroy(): void {
        this.cleanup();
        // ToDo: Clean up 'gl' context
    }

    onMouseMove(event: MouseEvent): void {
        switch (event.buttons) {
            case 0: // no mouse button
                break;

            case 1: // left mouse button
                break;

            case 2: // right mouse button
                this.camera.x -= event.movementX;
                this.camera.y += event.movementY;
                break;

            case 3: // left and right mouse buttons
                break;

            default:
                break;
        }
    }

    render(timeStamp: DOMHighResTimeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

        if (this.camera != null) {
            this.camera.updateBuffer(this.canvas.width, this.canvas.height);
        }

        if (this.map != null) {
            this.map.render();
        }

        window.requestAnimationFrame(this.render);
    }

    setMap(map: TexturedQuad) {
        this.map = map;
    }
}

function createRenderContext(): RenderContext {
    return new RenderContext();
}
