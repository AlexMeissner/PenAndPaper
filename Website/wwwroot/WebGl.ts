
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
}

class ShaderProgram {
    gl: WebGL2RenderingContext;
    program: WebGLProgram;

    constructor(gl: WebGL2RenderingContext) {
        this.gl = gl;
        this.program = gl.createProgram();
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
            var compileLog = this.gl.getShaderInfoLog(vertexShader);
            console.error(compileLog);
            return false;
        }

        const fragmentShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.gl.shaderSource(fragmentShader, fragmentSource);
        this.gl.compileShader(fragmentShader);
        const fragmentCompileStatus = this.gl.getShaderParameter(fragmentShader, this.gl.COMPILE_STATUS) as GLboolean;
        if (fragmentCompileStatus == false) {
            var compileLog = this.gl.getShaderInfoLog(fragmentShader);
            console.error(compileLog);
            this.gl.deleteShader(vertexShader);
            return false;
        }

        this.gl.attachShader(this.program, vertexShader);
        this.gl.attachShader(this.program, fragmentShader);
        this.gl.linkProgram(this.program);
        const linkStatus = this.gl.getProgramParameter(this.program, this.gl.LINK_STATUS);
        if (linkStatus == false) {
            var linkLog = this.gl.getProgramInfoLog(this.program);
            console.error(linkLog);
            this.gl.deleteShader(vertexShader);
            this.gl.deleteShader(fragmentShader);
            return false;
        }

        this.gl.deleteShader(vertexShader);
        this.gl.deleteShader(fragmentShader);

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

        const uvs = [0.0, 0.0, 0.0, 1.0, 1.0, 0.0, 1.0, 1.0];
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.uvBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(uvs), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(1, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(1);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);

        const indices = [0, 1, 2, 0, 2, 3];
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
        this.gl.bindVertexArray(this.vertexArray);

        if (this.shaderProgram != null) {
            this.shaderProgram.bind();
        }

        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    }

    setTexture(imageBase64: string): void {
        const image = new Image();

        // the image is not available synchronously when setting the source
        // wait for it to be finished before working with it
        image.onload = () => {
            this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
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
        return new ShaderProgram(this.gl);
    }

    createTexturedQuad(): TexturedQuad {
        return new TexturedQuad(this.gl);
    }

    destroy(): void {
        this.cleanup();
        // ToDo: Clean up 'gl' context
    }

    render(timeStamp: DOMHighResTimeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

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
