using System.ComponentModel.DataAnnotations;

namespace webapp_cloudrun.Models
{
    public class UserLogin
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
