namespace DataTransfer.Map;

public record GridUpdateDto(bool IsActive, int Size);

public record NameUpdateDto(string Text);

public record ScriptUpdateDto(string Text);

public record MapUpdateDto(NameUpdateDto? Name, ScriptUpdateDto? Script, GridUpdateDto? Grid);