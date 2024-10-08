var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
function drawElements(gl, mode, count, type, offset) {
    gl.drawElements(mode, count, type, offset);
}
function getUniformLocation(gl, program, name) {
    return gl.getUniformLocation(program, name);
}
function uniform1f(gl, location, x) {
    gl.uniform1f(location, x);
}
var Camera = /** @class */ (function () {
    function Camera() {
        this.x = 0.0;
        this.y = 0.0;
    }
    return Camera;
}());
var ShaderProgram = /** @class */ (function () {
    function ShaderProgram(gl) {
        this.gl = gl;
        this.program = gl.createProgram();
    }
    ShaderProgram.prototype.bind = function () {
        this.gl.useProgram(this.program);
    };
    ShaderProgram.prototype.compile = function (vertexSource, fragmentSource) {
        var vertexShader = this.gl.createShader(this.gl.VERTEX_SHADER);
        this.gl.shaderSource(vertexShader, vertexSource);
        this.gl.compileShader(vertexShader);
        var vertexCompileStatus = this.gl.getShaderParameter(vertexShader, this.gl.COMPILE_STATUS);
        if (vertexCompileStatus == false) {
            var compileLog = this.gl.getShaderInfoLog(vertexShader);
            console.error(compileLog);
            return false;
        }
        var fragmentShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.gl.shaderSource(fragmentShader, fragmentSource);
        this.gl.compileShader(fragmentShader);
        var fragmentCompileStatus = this.gl.getShaderParameter(fragmentShader, this.gl.COMPILE_STATUS);
        if (fragmentCompileStatus == false) {
            var compileLog = this.gl.getShaderInfoLog(fragmentShader);
            console.error(compileLog);
            this.gl.deleteShader(vertexShader);
            return false;
        }
        this.gl.attachShader(this.program, vertexShader);
        this.gl.attachShader(this.program, fragmentShader);
        this.gl.linkProgram(this.program);
        var linkStatus = this.gl.getProgramParameter(this.program, this.gl.LINK_STATUS);
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
    };
    ShaderProgram.prototype.destroy = function () {
        this.gl.deleteProgram(this.program);
    };
    ShaderProgram.prototype.release = function () {
        this.gl.useProgram(null);
    };
    return ShaderProgram;
}());
var Quad = /** @class */ (function () {
    function Quad(gl) {
        this.gl = gl;
        this.vertexArray = gl.createVertexArray();
        this.vertexBuffer = gl.createBuffer();
        this.uvBuffer = gl.createBuffer();
        this.indexBuffer = gl.createBuffer();
    }
    Quad.prototype.destroy = function () {
        this.gl.deleteVertexArray(this.vertexArray);
        this.gl.deleteBuffer(this.vertexBuffer);
        this.gl.deleteBuffer(this.uvBuffer);
        this.gl.deleteBuffer(this.indexBuffer);
    };
    Quad.prototype.setShaderProgram = function (program) {
        this.shaderProgram = program;
    };
    Quad.prototype.setVertices = function (srcData) {
        this.gl.bindVertexArray(this.vertexArray);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vertexBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(srcData), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(0, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(0);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        var uvs = [0.0, 0.0, 0.0, 1.0, 1.0, 0.0, 1.0, 1.0];
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.uvBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(uvs), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(1, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(1);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        var indices = [0, 1, 2, 0, 2, 3];
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.indexBuffer);
        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indices), this.gl.STATIC_DRAW);
    };
    return Quad;
}());
var TexturedQuad = /** @class */ (function (_super) {
    __extends(TexturedQuad, _super);
    function TexturedQuad(gl) {
        var _this = _super.call(this, gl) || this;
        _this.texture = gl.createTexture();
        return _this;
    }
    TexturedQuad.prototype.destroy = function () {
        this.gl.deleteTexture(this.texture);
        _super.prototype.destroy.call(this);
    };
    TexturedQuad.prototype.render = function () {
        this.gl.bindVertexArray(this.vertexArray);
        if (this.shaderProgram != null) {
            this.shaderProgram.bind();
        }
        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    };
    TexturedQuad.prototype.setTexture = function (imageBase64) {
        var image = new Image();
        image.src = imageBase64;
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
    };
    TexturedQuad.prototype.isPowerOf2 = function (value) {
        return (value & (value - 1)) === 0;
    };
    return TexturedQuad;
}(Quad));
var Grid = /** @class */ (function (_super) {
    __extends(Grid, _super);
    function Grid(gl, color, size) {
        var _this = _super.call(this, gl) || this;
        _this.color = color;
        _this.size = size;
        return _this;
    }
    return Grid;
}(Quad));
var Token = /** @class */ (function (_super) {
    __extends(Token, _super);
    function Token(gl, name) {
        var _this = _super.call(this, gl) || this;
        _this.name = name;
        return _this;
    }
    return Token;
}(TexturedQuad));
var RenderContext = /** @class */ (function () {
    function RenderContext() {
        this.tokens = [];
    }
    RenderContext.prototype.initialize = function (identifier) {
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
        this.gl.clearColor(1.0, 0.5, 0.5, 1.0);
        this.render = this.render.bind(this);
        window.requestAnimationFrame(this.render);
        return true;
    };
    RenderContext.prototype.addToken = function (token) {
        this.tokens.push(token);
    };
    RenderContext.prototype.cleanup = function () {
        this.tokens.forEach(function (token) { return token.destroy(); });
        this.tokens = [];
        if (this.grid != null) {
            this.grid.destroy();
            this.grid = null;
        }
        if (this.map != null) {
            this.map.destroy();
            this.map = null;
        }
    };
    RenderContext.prototype.createShaderProgram = function () {
        return new ShaderProgram(this.gl);
    };
    RenderContext.prototype.createTexturedQuad = function () {
        return new TexturedQuad(this.gl);
    };
    RenderContext.prototype.destroy = function () {
        this.cleanup();
        // ToDo: Clean up 'gl' context
    };
    RenderContext.prototype.render = function (timeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);
        if (this.map != null) {
            this.map.render();
        }
        window.requestAnimationFrame(this.render);
    };
    RenderContext.prototype.setMap = function (map) {
        this.map = map;
    };
    return RenderContext;
}());
function createRenderContext() {
    return new RenderContext();
}
//# sourceMappingURL=WebGl.js.map