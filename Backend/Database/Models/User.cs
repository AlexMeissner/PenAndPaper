using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Models;

public class User
{
    public int Id { get; init; }
    [MaxLength(64)] public required string Email { get; init; }
    [MaxLength(32)] public required string Username { get; set; }
}