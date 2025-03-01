namespace DataTransfer.Map;

public record GridUpdateDto(bool IsActive, int Size);

public record NameUpdateDto(string Name);

public record ScriptUpdateDto(string Script);

public record MapUpdateDto(string? Name, string? Script, GridUpdateDto? Grid);