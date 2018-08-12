using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Orleans;
using RepositoryLayer.Repository;
using RepositoryLayer.RepositoryExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class OrderEventsGrain : Grain, IOrderEvents
    {
        private IEnumerable<Event> state { get; set; }
        private Guid _orderID { get; set; }
        private IEventRepository<Event, Guid> _repository;

        public override Task OnActivateAsync()
        {
            _repository = new EventRepository();
            state = new List<Event>();
            _orderID = this.GetPrimaryKey();

            ReadState();

            return base.OnActivateAsync();
        }
        /// <summary>
        /// Updates the state on deactivation
        /// </summary>
        /// <returns></returns>
        public override Task OnDeactivateAsync()
        {
            return base.OnDeactivateAsync();
        }

        public void DeleteState()
        {
            //throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetOrdersEvents()
        {
            return Task.FromResult(state);
        }

        public void ReadState()
        {
            this.state = _repository.GetOrdersEvents(_orderID);
        }

        public void UpdateState()
        {
            //throw new NotImplementedException();
        }

        public void WriteState()
        {
            //throw new NotImplementedException();
        }
    }
}
