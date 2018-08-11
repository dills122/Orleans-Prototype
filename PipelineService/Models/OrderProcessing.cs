using DataModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PipelineService.Models
{
    public class OrderProcessing
    {
        public Order order { get; set; }
        public List<InputFile> files { get; set; }

        public OrderProcessing()
        {
            files = new List<InputFile>();
        }
        public OrderProcessing(Order order, List<InputFile> files)
        {
            this.order = order;
            this.files = files;
        }
    }
}
