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
    function Camera(gl) {
        this.x = 0.0;
        this.y = 0.0;
        this.zoom = 1.0;
        this.BINDING_POINT_NUMBER = 0;
        this.gl = gl;
        this.buffer = this.gl.createBuffer();
        this.gl.bindBufferBase(this.gl.UNIFORM_BUFFER, this.BINDING_POINT_NUMBER, this.buffer);
        var bufferSize = 3 * 4; // 3 x sizeof(GLfloat)
        this.gl.bufferData(this.gl.UNIFORM_BUFFER, bufferSize, this.gl.DYNAMIC_DRAW);
    }
    Camera.prototype.destroy = function () {
        this.gl.deleteBuffer(this.buffer);
    };
    Camera.prototype.updateBuffer = function () {
        this.gl.bindBuffer(this.gl.UNIFORM_BUFFER, this.buffer);
        var cameraData = new Float32Array([this.x, this.y, this.zoom]);
        this.gl.bufferSubData(this.gl.UNIFORM_BUFFER, 0, cameraData);
    };
    return Camera;
}());
var ShaderProgram = /** @class */ (function () {
    function ShaderProgram(gl, camera) {
        this.gl = gl;
        this.program = gl.createProgram();
        this.camera = camera;
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
        var cameraBufferIndex = this.gl.getUniformBlockIndex(this.program, 'CameraBuffer');
        this.gl.uniformBlockBinding(this.program, cameraBufferIndex, this.camera.BINDING_POINT_NUMBER);
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
        var uvs = [0.0, 1.0, 1.0, 1.0, 0.0, 0.0, 1.0, 0.0];
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.uvBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(uvs), this.gl.STATIC_DRAW);
        this.gl.vertexAttribPointer(1, 2, this.gl.FLOAT, false, 8, 0);
        this.gl.enableVertexAttribArray(1);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        var indices = [0, 1, 2, 2, 1, 3];
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
        if (this.shaderProgram == null) {
            console.error("No shader bound.");
            return;
        }
        this.gl.bindVertexArray(this.vertexArray);
        this.shaderProgram.bind();
        this.gl.activeTexture(this.gl.TEXTURE0);
        this.gl.bindTexture(this.gl.TEXTURE_2D, this.texture);
        var samplerLocation = this.gl.getUniformLocation(this.shaderProgram.program, "sampler");
        this.gl.uniform1i(samplerLocation, 0);
        this.gl.drawElements(this.gl.TRIANGLES, 6, this.gl.UNSIGNED_SHORT, 0);
    };
    TexturedQuad.prototype.setTexture = function (imageBase64) {
        var _this = this;
        var image = new Image();
        // the image is not available synchronously when setting the source
        // wait for it to be finished before working with it
        image.onload = function () {
            _this.gl.bindTexture(_this.gl.TEXTURE_2D, _this.texture);
            _this.gl.pixelStorei(_this.gl.UNPACK_FLIP_Y_WEBGL, true);
            _this.gl.texImage2D(_this.gl.TEXTURE_2D, 0, _this.gl.RGBA, _this.gl.RGBA, _this.gl.UNSIGNED_BYTE, image);
            if (_this.isPowerOf2(image.width) && _this.isPowerOf2(image.height)) {
                _this.gl.generateMipmap(_this.gl.TEXTURE_2D);
            }
            else {
                _this.gl.texParameteri(_this.gl.TEXTURE_2D, _this.gl.TEXTURE_WRAP_S, _this.gl.CLAMP_TO_EDGE);
                _this.gl.texParameteri(_this.gl.TEXTURE_2D, _this.gl.TEXTURE_WRAP_T, _this.gl.CLAMP_TO_EDGE);
                _this.gl.texParameteri(_this.gl.TEXTURE_2D, _this.gl.TEXTURE_MIN_FILTER, _this.gl.LINEAR);
            }
            _this.gl.bindTexture(_this.gl.TEXTURE_2D, null);
        };
        image.src = imageBase64;
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
        this.camera = new Camera(this.gl);
        this.canvas.addEventListener("mousemove", function (event) {
            // event.buttons
            // 0: no button
            // 1: left mouse button
            // 2: right mouse button
            // 3: left and right mouse buttons
        });
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
        return new ShaderProgram(this.gl, this.camera);
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
        if (this.camera != null) {
            this.camera.updateBuffer();
        }
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