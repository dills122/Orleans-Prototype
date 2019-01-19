using System.ComponentModel.DataAnnotations;

namespace DataModels.Models
{
    public enum TransactionType
    {
        Debt,
        Payment,
        Interest
    }
    public class Transaction
    {
        [Key]
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public decimal StartBalance { get; set; }
        [Required]
        public decimal EndBalance { get; set; }
        [Required]
        public bool IsValidated { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }

        public Account Account { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
