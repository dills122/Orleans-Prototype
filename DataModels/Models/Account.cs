using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models
{
    public enum FinAccountType
    {
        Checking,
        Saving,
        Investing
    }

    public class Account
    {
        [Key]
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public FinAccountType AccountType { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public DateTime OpenDate { get; set; }
        [Required]
        public DateTime LastTransaction { get; set; }

        public User User { get; set; }
        [Required]
        public int UserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
