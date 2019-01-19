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
        public FinAccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime LastTransaction { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
