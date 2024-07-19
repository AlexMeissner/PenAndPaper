namespace Website.Services.Graphics;

// https://developer.mozilla.org/en-US/docs/Web/API/WebGLRenderingContext/getProgramParameter
public enum ProgramParameterName
{
    DeleteStatus = 0x8B80,
    LinkStatus = 0x8B82,
    ValidateStatus = 0x8B83,
    AttachedShaders = 0x8B85,
    ActiveUniforms = 0x8B86,
    TransformFeedbackBufferMode = 0x8C7F,
    TransformFeedbackVaryings = 0x8C83,
    ActiveUniformBlocks = 0x8A36
}
