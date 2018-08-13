using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface IEvent : IGrainWithGuidKey
    {
        Task<Event> GetEvent();
        Task CreateEvent(Event newEvent);

    }
}
