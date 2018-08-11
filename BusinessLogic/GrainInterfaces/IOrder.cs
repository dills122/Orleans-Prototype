using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Orleans;

namespace BusinessLogic.GrainInterfaces
{
   public interface IOrder : IGrainWithIntegerKey, IState
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task<Order> GetOrder();
        Task ProcessOrder();
    }
}
