using DataModels.Models;
using PipelineService.Models;
using RepositoryLayer.Abstraction;
using RepositoryLayer.Repository;
using System;

namespace PipelineService.Processing
{
    public static class OrderProcessor
    {
        
        public static bool ProcessFiles(OrderProcessing orderProcessing)
        {
            try
            {
                IRepository<Document, Guid> repository = new DocumentRepository();
                foreach (InputFile file in orderProcessing.files)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
        
        public static bool ProcessOrder(OrderProcessing orderProcessing)
        {
            try
            {
                //IRepository<User, string> UserRepo = new UserRepository();
                IRepository<Order, Guid> repository = new OrderRepository();
                repository.Add(orderProcessing.order);
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
