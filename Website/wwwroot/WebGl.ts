abstract class UniformBuffer {
    protected readonly gl: WebGL2RenderingContext;
    protected readonly buffer: WebGLBuffer;

    protected constructor(gl: WebGL2RenderingContext) {
        this.gl = gl;
        this.buffer = this.gl.createBuffer();
    }

    public abstract GetBindingPoint(): number;

    public abstract GetName(): string;
}

class Camera extends UniformBuffer {
    private x: GLfloat = 0.0;
    private y: GLfloat = 0.0;
    private readonly near: number = 0.1;
    private readonly far: number = 1.0;

    private zoomLevel: number = 0;
    private zoomFactor: GLfloat = 1.0;
    private readonly zoomSpeed: GLfloat = 0.1;

    private readonly BINDING_POINT_NUMBER: GLuint = 0;

    constructor(gl: WebGL2RenderingContext) {
        super(gl);
        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);
        const matrix = this.createViewProjectionMatrix(1.0, 1.0);
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, matrix.byteLength * 2, this.gl.DYNAMIC_DRAW);
    }

    public GetBindingPoint(): number {
        return this.BINDING_POINT_NUMBER;
    }

    public GetName(): string {
        return 'CameraBuffer';
    }

    public GetPosition(): number[] {
        return [this.x, this.y];
    }

    public GetZoomFactor(): number {
        return this.zoomFactor;
    }

    private createProjectionMatrix(width: GLfloat, height: GLfloat): Float32Array {
        const left: GLfloat = 0.0;
        const right: GLfloat = width * this.zoomFactor;
        const top: GLfloat = 0.0;
        const bottom: GLfloat = -height * this.zoomFactor;

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

    private createViewProjectionMatrix(width: GLfloat, height: GLfloat): Float32Array {
        const left: GLfloat = 0.0;
        const right: GLfloat = width * this.zoomFactor;
        const top: GLfloat = 0.0;
        const bottom: GLfloat = -height * this.zoomFactor;

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

    public destroy(): void {
        this.gl.deleteBuffer(this.buffer);
    }

    public move(x: number, y: number): void {
        this.x += x * this.zoomFactor;
        this.y -= y * this.zoomFactor;
    }

    public reset(): void {
        this.x = 0.0;
        this.y = 0.0;
        this.zoomLevel = 0;
        this.zoomFactor = 1.0;
    }

    public updateBuffer(width: GLfloat, height: GLfloat): void {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);
        const projectionMatrix = this.createProjectionMatrix(width, height);
        const viewProjectionMatrix = this.createViewProjectionMatrix(width, height);
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, projectionMatrix);
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, projectionMatrix.byteLength, viewProjectionMatrix);
    }

    public zoom(cursorX: number, cursorY: number, direction: number): void {
        const offsetX = cursorX * this.zoomFactor - this.x;
        const offsetY = cursorY * this.zoomFactor + this.y;

        this.zoomLevel += direction < 0.0 ? 1 : -1;
        this.zoomFactor = Math.exp(-this.zoomLevel * this.zoomSpeed);

        this.x = cursorX * this.zoomFactor - offsetX;
        this.y = -cursorY * this.zoomFactor + offsetY;
    }
}

class Grid extends UniformBuffer {
    public isActive: boolean = false;
    public color: Float32Array = new Float32Array(4);
    public size: GLfloat = 1.0;

    private readonly BINDING_POINT_NUMBER: GLuint = 1;

    constructor(gl: WebGL2RenderingContext) {
        super(gl);
        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);
        const gridData = this.createBufferData();
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, gridData.byteLength, this.gl.DYNAMIC_DRAW);
    }

    public GetBindingPoint(): number {
        return this.BINDING_POINT_NUMBER;
    }

    public GetName(): string {
        return 'GridBuffer';
    }

    public destroy(): void {
        this.gl.deleteBuffer(this.buffer);
    }

    public updateBuffer(): void {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);
        const gridData = this.createBufferData();
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, gridData);
    }

    private createBufferData(): Float32Array {
        const data = new Float32Array(6);
        data.set(this.color, 0);
        data[4] = this.size;
        data[5] = this.isActive ? 1.0 : 0.0;
        return data;
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

    public addUniformBuffer(buffer: UniformBuffer): void {
        const uniformBlockIndex = this.gl.getUniformBlockIndex(this.program, buffer.GetName());
        this.gl.uniformBlockBinding(this.program, uniformBlockIndex, buffer.GetBindingPoint());
    }

    public bind(): void {
        this.gl.useProgram(this.program);
    }

    public compile(vertexSource: string, fragmentSource: string): boolean {
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

        return true;
    }

    public destroy(): void {
        this.gl.deleteProgram(this.program);
    }

    public release(): void {
        this.gl.useProgram(null);
    }
}

class TexturedQuad {
    gl: WebGL2RenderingContext;
    vertexArray: WebGLVertexArrayObject;
    vertexBuffer: WebGLBuffer;
    uvBuffer: WebGLBuffer;
    indexBuffer: WebGLBuffer;
    shaderProgram: ShaderProgram | null;
    texture: WebGLTexture;
    floatUniforms: Map<string, GLfloat> = new Map();

    private width: number = 0;
    private height: number = 0;

    constructor(gl: WebGL2RenderingContext) {
        this.gl = gl;
        this.vertexArray = gl.createVertexArray();
        this.vertexBuffer = gl.createBuffer();
        this.uvBuffer = gl.createBuffer();
        this.indexBuffer = gl.createBuffer();
        this.texture = gl.createTexture();
    }

    destroy(): void {
        this.gl.deleteTexture(this.texture);
        this.gl.deleteVertexArray(this.vertexArray);
        this.gl.deleteBuffer(this.vertexBuffer);
        this.gl.deleteBuffer(this.uvBuffer);
        this.gl.deleteBuffer(this.indexBuffer);
    }

    public getWidth(): number {
        return this.width
    }

    public getHeight(): number {
        return this.height
    }

    public render(): void {
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

        this.floatUniforms.forEach((value: GLfloat, name: string) => {
            const location = this.gl.getUniformLocation(this.shaderProgram.program, name);
            if (location != null) {
                this.gl.uniform1f(location, value);
            }
        });

        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    }

    public setShaderProgram(program: ShaderProgram): void {
        this.shaderProgram = program;
    }

    public setTexture(imageBase64: string): void {
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

    public setUniform(name: string, value: GLfloat) {
        this.floatUniforms.set(name, value);
    }

    public getUniform(name: string): GLfloat {
        return this.floatUniforms.get(name);
    }

    public setVertices(srcData: number[]): void {
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

    public updateVertices(newData: number[]): void {
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

    private isPowerOf2(value: number): boolean {
        return (value & (value - 1)) === 0;
    }
}

class Token extends TexturedQuad {
    name: string;
    isMouseHover: number = 0.0;
    isLeftMouseButtonDown: number = 0.0;

    constructor(gl: WebGL2RenderingContext, name: string) {
        super(gl);
        this.name = name;
    }

    public render(): void {
        this.setUniform("isMouseOver", this.isMouseHover);
        this.setUniform("isLeftMouseButtonDown", this.isLeftMouseButtonDown);
        this.gl.enable(this.gl.BLEND);
        this.gl.blendFunc(this.gl.SRC_ALPHA, this.gl.ONE_MINUS_SRC_ALPHA);
        super.render();
        this.gl.disable(this.gl.BLEND);
    }

    public hover(mouseX: number, mouseY: number, grid: Grid): void {
        const x: number = this.getUniform("x");
        const y: number = this.getUniform("y");
        const width: number = this.getWidth() * grid.size;
        const height: number = this.getHeight() * grid.size;

        if (mouseX >= x && mouseX < x + width && mouseY >= y && mouseY < y + height) {
            this.isMouseHover = 1.0;
        } else {
            this.isMouseHover = 0.0;
        }
    }

    public setLeftMouseButtonDown(isMouseDown: boolean): void {
        this.isLeftMouseButtonDown = isMouseDown ? 1.0 : 0.0;
    }
}

class MouseIndicator extends TexturedQuad {
    constructor(gl: WebGL2RenderingContext) {
        super(gl);
        this.setUniform("alpha", 0.0);
    }

    public setPosition(x: number, y: number): void {
        this.setUniform("x", x);
        this.setUniform("y", y);
        this.setUniform("alpha", 1.0);
    }

    public render(): void {
        let alpha: number = this.getUniform("alpha");

        if (alpha > 0.0) {
            this.gl.enable(this.gl.BLEND);
            this.gl.blendFunc(this.gl.SRC_ALPHA, this.gl.ONE_MINUS_SRC_ALPHA);
            super.render();
            this.gl.disable(this.gl.BLEND);

            alpha = Math.max(0.0, alpha - 0.01);
            this.setUniform("alpha", alpha);
        }
    }
}

class RenderContext {
    canvas: HTMLCanvasElement | null;
    gl: WebGL2RenderingContext | null;
    map: TexturedQuad | null;
    grid: Grid | null;
    tokens: Token[] = [];
    mouseIndicators: MouseIndicator[] = [];
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

        this.grid = new Grid(this.gl);
        this.camera = new Camera(this.gl);

        this.gl.clearColor(0.1, 0.1, 0.1, 1.0);

        this.canvas.addEventListener("mousemove", (event: MouseEvent): void => this.onMouseChanged(event));
        this.canvas.addEventListener("mousedown", (event: MouseEvent): void => this.onMouseChanged(event));
        this.canvas.addEventListener("mouseup", (event: MouseEvent): void => this.onMouseChanged(event));
        this.canvas.addEventListener("wheel", (event: WheelEvent): void => this.onMouseWheel(event));
        this.canvas.oncontextmenu = () => false;

        this.render = this.render.bind(this);
        window.requestAnimationFrame(this.render);

        return true;
    }

    public addMouseIndicator(mouseIndicator: MouseIndicator): void {
        this.mouseIndicators.push(mouseIndicator);
    }

    public addToken(token: Token): void {
        this.tokens.push(token);
    }

    public clearTokens(): void {
        this.tokens.forEach(token => token.destroy());
        this.tokens = [];
    }

    public cleanup(): void {
        this.clearTokens();

        if (this.map != null) {
            this.map.destroy();
            this.map = null;
        }
    }

    public createShaderProgram(): ShaderProgram {
        return new ShaderProgram(this.gl, this.camera);
    }

    public createTexturedQuad(): TexturedQuad {
        return new TexturedQuad(this.gl);
    }

    public createToken(name: string): Token {
        return new Token(this.gl, name);
    }

    public createMouseIndicator(): MouseIndicator {
        return new MouseIndicator(this.gl);
    }

    public getCamera(): Camera {
        return this.camera;
    }

    public getGrid(): Grid {
        return this.grid;
    }

    public destroy(): void {
        this.cleanup();
        // ToDo: Clean up 'gl' context
    }

    private onMouseChanged(event: MouseEvent): void {
        this.tokens.forEach(token => {
                const position: number[] = this.transformPosition(event.clientX, event.clientY);
                token.hover(position[0], position[1], this.grid);

                const isLeftMouseButtonDown: boolean = event.buttons == 1;
                token.setLeftMouseButtonDown(isLeftMouseButtonDown);
            }
        );

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

    private onMouseWheel(event: WheelEvent): void {
        this.camera.zoom(event.clientX, event.clientY, event.deltaY);
    }

    private render(timeStamp: DOMHighResTimeStamp) {
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

    public setMap(map: TexturedQuad) {
        this.map = map;
        this.camera.reset();
    }

    public transformPosition(x: number, y: number): number[] {
        const camera = this.getCamera();
        const offset = camera.GetPosition();
        const zoom = camera.GetZoomFactor();

        return [x * zoom - offset[0], y * zoom + offset[1]];
    }

    public updateGrid(isActive: boolean, size: GLint, color: Float32Array) {
        this.grid.isActive = isActive;
        this.grid.size = size;
        this.grid.color = color;
    }
}

function createRenderContext(): RenderContext {
    return new RenderContext();
}
