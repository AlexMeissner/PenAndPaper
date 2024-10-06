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
function getProgramParameter(gl, program, pname) {
    return gl.getProgramParameter(program, pname);
}
function getUniformLocation(gl, program, name) {
    return gl.getUniformLocation(program, name);
}
function linkProgram(gl, program) {
    gl.linkProgram(program);
}
function shaderSource(gl, shader, source) {
    gl.shaderSource(shader, source);
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
var Camera = /** @class */ (function () {
    function Camera() {
        this.x = 0.0;
        this.y = 0.0;
    }
    return Camera;
}());
var ShaderProgram = /** @class */ (function () {
    function ShaderProgram(gl) {
        this.program = gl.createProgram();
    }
    ShaderProgram.prototype.destroy = function (gl) {
        gl.deleteProgram(this.program);
    };
    return ShaderProgram;
}());
var Quad = /** @class */ (function () {
    function Quad(gl) {
        this.buffer = gl.createBuffer();
    }
    Quad.prototype.destroy = function (gl) {
        gl.deleteBuffer(this.buffer);
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
    TexturedQuad.prototype.destroy = function (gl) {
        gl.deleteTexture(this.texture);
        _super.prototype.destroy.call(this, gl);
    };
    return TexturedQuad;
}(Quad));
//class Map extends TexturedQuad {
//
//}
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
    RenderContext.prototype.destroy = function () {
        var _this = this;
        this.tokens.forEach(function (token) { return token.destroy(_this.gl); });
        this.tokens = [];
        if (this.grid != null) {
            this.grid.destroy(this.gl);
            this.grid = null;
        }
    };
    RenderContext.prototype.render = function (timeStamp) {
        this.gl.viewport(0, 0, this.canvas.width, this.canvas.height);
        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);
        window.requestAnimationFrame(this.render);
    };
    return RenderContext;
}());
function createRenderContext() {
    return new RenderContext();
}
//# sourceMappingURL=WebGl.js.map