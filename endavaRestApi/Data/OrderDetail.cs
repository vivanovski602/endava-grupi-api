using System.ComponentModel.DataAnnotations;


namespace endavaRestApi.Data
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
