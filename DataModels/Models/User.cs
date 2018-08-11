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
        [StringLength(25)]
        public string Username { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public AccountType accountType { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Order> orders { get; set; }
    }
}
