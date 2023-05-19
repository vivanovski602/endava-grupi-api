using System.ComponentModel.DataAnnotations;


namespace endavaRestApi.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderPlaced { get; set; }
        public DateTime? OrderFulfilled { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
        public int UserId { get; set; }

        // Calculated property for the total amount
        public decimal TotalAmount => OrderDetails.Sum(od => od.Quantity * od.Product.Price);
    }
}

