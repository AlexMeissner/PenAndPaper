using DataTransfer.Grid;

namespace DataTransfer.Map;

public record MapDto(string Name, byte[] Image, GridDto Grid);