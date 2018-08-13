using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface IDocument : IGrainWithGuidKey
    {
        Task<Document> GetDocument();
        Task UpdateDocument(Document document);
        Task CreateDocument(Document document);
        Task DeleteDocument(Document document);
        Task<Guid> GetAssociatedOrder();
    }
}
