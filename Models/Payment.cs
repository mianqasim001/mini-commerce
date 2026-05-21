using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("PAYMENT")]
    public class Payment
    {
        [Key][Column("payment_id")] public int PaymentId { get; set; }

        [Required][Column("order_id")] public int OrderId { get; set; }

        [Required][Column("payment_method")] public string PaymentMethod { get; set; } = string.Empty;

        [Required][Column("payment_status")] public string PaymentStatus { get; set; } = "Pending";

        [Column("transaction_id")] public string? TransactionId { get; set; } // Null until gateway responds
    }
}