using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.Models
{
    public enum AccountType
    {
        User,
        Admin,
        SuperAdmin
    }
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public AccountType accountType { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
