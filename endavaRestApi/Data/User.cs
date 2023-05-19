using System.ComponentModel.DataAnnotations;

namespace endavaRestApi.Data
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public bool IsActive { get; internal set; }
        public ICollection<Order> Orders { get; set; } = null!;

    }

}
