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
    }
}
