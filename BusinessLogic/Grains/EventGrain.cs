using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Orleans;
using RepositoryLayer.Abstraction;
using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class EventGrain : Grain, IEvent, IState
    {
        private Event state { get; set; }

        private IRepository<Event, Guid> _repository;

        public override Task OnActivateAsync()
        {
            _repository = new EventRepository();
            //Initializes on grain activation
            state = new Event();

            state.EventId = this.GetPrimaryKey();
            ReadState();

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            return base.OnDeactivateAsync();
        }

        public Task CreateEvent(Event newEvent)
        {
            if(newEvent.EventId == this.GetPrimaryKey())
            {
                state = newEvent;
            }
            return Task.CompletedTask;
        }

        public Task<Event> GetEvent()
        {
            return Task.FromResult(state);
        }

        public void DeleteState()
        {
            throw new NotImplementedException();
        }

        public void ReadState()
        {
            state = _repository.Get(this.GetPrimaryKey());
        }

        public void UpdateState()
        {
            throw new NotImplementedException();
        }

        public void WriteState()
        {
            throw new NotImplementedException();
        }
    }
}
