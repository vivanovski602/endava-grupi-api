using System.ComponentModel.DataAnnotations;


namespace endavaRestApi.Data
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerFName { get; set; }
        [Required]
        public string CustomerLName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;


    }
}
