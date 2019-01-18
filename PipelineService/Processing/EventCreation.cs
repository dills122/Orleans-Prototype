using DataModels.Models;
using RepositoryLayer.Abstraction;
using RepositoryLayer.Repository;
using System;

namespace PipelineService.Processing
{
    public static class EventCreation
    {
        public static void CreateEvent(Guid OrderId, EventType eventType)
        {
            IRepository<Event, Guid> repository = new EventRepository();
            Event EventVar = new Event
            {
                OrderId = OrderId,
                Created = DateTime.Now,
                EventId = Guid.NewGuid(),
                eventType = eventType,
            };
            repository.Add(EventVar);
        }
    }
}
