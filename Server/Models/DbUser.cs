using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class DbUser
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}