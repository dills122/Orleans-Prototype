using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipelineService.Models
{
    public class InputFile
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] FilesBytes { get; set; }
        public DocumentType documentType { get; set; }
    }
}
