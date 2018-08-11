using DataModels.Models;
using PipelineService.Models;
using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipelineService.Processing
{
    public static class OrderProcessor
    {
        
        public static void ProcessFiles(OrderProcessing orderProcessing)
        {
            IRepository<Document, Guid> repository = new DocumentRepository();
            foreach(InputFile file in orderProcessing.files)
            {
                Document document = new Document
                {
                    DocumentId = new Guid(),
                    DocumentBaseSixFour = Convert.ToBase64String(file.FilesBytes),
                    documentType = file.documentType,
                    DocumentExtension = file.FileName.Split('.')[1],
                    DocumentName = file.FileName,
                    IsApproved = true,
                    OrderId = orderProcessing.order.OrderId,
                    Location = file.FileName
                };
                repository.Add(document);
            }
        }
        
        public static void ProcessOrder(OrderProcessing orderProcessing)
        {
            IRepository<Order, Guid> repository = new OrderRepository();
            repository.Add(orderProcessing.order);
        }
    }
}
