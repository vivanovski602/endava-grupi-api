using System.ComponentModel.DataAnnotations;

namespace endavaRestApi.Data
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
      
        public int OrderId { get; set; }
        //public decimal Amount { get; set; }
        public string Status { get; set; }
        //dodadeno
        public Order order { get; set; } = null!;
       
    }
}
