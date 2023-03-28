using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace endavaRestApi.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        public string ProductCategory { get; set; }
    }
}
