using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.Models
{
    public enum EventType
    {
        StartProcessing,
        ErrorProcessing,
        FinishedProcessing,
    }
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }
        [Required]
        public EventType eventType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [Required]
        public long OrderId { get; set; }
        public virtual Order order { get; set; }
    }
}
