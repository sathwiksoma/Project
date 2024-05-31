using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class Payment : IEquatable<Payment>

    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public float Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        public Payment()
        {
            PaymentId = 0;
        }

        public Payment(int paymentid, string paymentMode, float amount, string status, DateTime date, int orderId)
        {
            PaymentId = paymentid;
            PaymentMode = paymentMode;
            Amount = amount;
            Status = status;
            Date = date;
            OrderId = orderId;
        }

        public Payment(string paymentMode, float amount, string status, DateTime date, int orderId)
        {
            PaymentMode = paymentMode;
            Amount = amount;
            Status = status;
            Date = date;
            OrderId = orderId;
        }

        public bool Equals(Payment? other)
        {
            var payment = other ?? new Payment();
            return this.PaymentId.Equals(payment.PaymentId);
        }
    }
}
