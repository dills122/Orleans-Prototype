using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.Models
{
    public enum OrderType
    {
        Appraisal,
        Loan,
        Title,
        Closing
    }
    public class Order
    {
        [Key]
        public long OrderId { get; set; }
        public string OrderDescription { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public OrderType orderType { get; set; }
        public string Username { get; set; }
        public virtual User user { get; set; }
    }
}
