using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface IOrderEvents : IGrainWithGuidKey
    {
        Task<IEnumerable<Event>> GetOrdersEvents();
    }
}
