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

function getProgramParameter(gl: WebGL2RenderingContext, program: WebGLProgram, pname: number) {
    return gl.getProgramParameter(program, pname);
}

function getUniformLocation(gl: WebGL2RenderingContext, program: WebGLProgram, name: string) {
    return gl.getUniformLocation(program, name);
}

function linkProgram(gl: WebGL2RenderingContext, program: WebGLProgram) {
    gl.linkProgram(program);
}

function shaderSource(gl: WebGL2RenderingContext, shader: WebGLShader, source: string) {
    gl.shaderSource(shader, source);
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

class Camera {
    x: GLfloat = 0.0;
    y: GLfloat = 0.0;
}

class ShaderProgram {
    program: WebGLProgram;

    constructor(gl: WebGL2RenderingContext) {
        this.program = gl.createProgram();
    }

    destroy(gl: WebGL2RenderingContext): void {
        gl.deleteProgram(this.program);
    }
}

class Quad {
    buffer: WebGLBuffer;

    constructor(gl: WebGL2RenderingContext) {
        this.buffer = gl.createBuffer();
    }

    destroy(gl: WebGL2RenderingContext): void {
        gl.deleteBuffer(this.buffer);
    }
}

class TexturedQuad extends Quad {
    texture: WebGLTexture;

    constructor(gl: WebGL2RenderingContext) {
        super(gl);
        this.texture = gl.createTexture();
    }

    destroy(gl: WebGL2RenderingContext): void {
        gl.deleteTexture(this.texture);
        super.destroy(gl);
    }
}

//class Map extends TexturedQuad {
//
//}

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
    grid: Grid | null;
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

    destroy(): void {
        this.tokens.forEach(token => token.destroy(this.gl));
        this.tokens = [];

        if (this.grid != null) {
            this.grid.destroy(this.gl);
            this.grid = null;
        }
    }

    render(timeStamp: DOMHighResTimeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);

        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

        window.requestAnimationFrame(this.render);
    }
}

function createRenderContext(): RenderContext {
    return new RenderContext();
}
