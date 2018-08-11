using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.Models
{
    public enum DocumentType
    {
        ProofOfAddress,
        ProofOfEmployment,
        ProofOfAssests,
        Other
    }
    public class Document
    {
        [Key]
        public Guid DocumentId { get; set; }
        [Required]
        public string DocumentName { get; set; }
        [Required]
        public string DocumentExtension { get; set; }
        [Required]
        public DocumentType documentType { get; set; }
        public string Location { get; set; }
        public string DocumentBaseSixFour { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        public bool IsApproved { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order order { get; set; }
    }
}
