using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Orleans;
using PipelineService.Models;

namespace BusinessLogic.GrainInterfaces
{
   public interface IOrder : IGrainWithGuidKey
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task<Order> GetOrder();
        Task RemoveOrder();
        Task ProcessOrder(OrderProcessing orderProcessing);
    }
}
