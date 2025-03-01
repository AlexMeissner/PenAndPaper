using DataTransfer.Grid;

namespace DataTransfer.Map;

public record MapCreationDto(string Name, byte[] Image, GridDto Grid);