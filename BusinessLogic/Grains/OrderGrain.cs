using DataModels.Exceptions;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Repository;
using PipelineService.Interfaces;
using PipelineService.Pipelines;
using PipelineService.Allocation;
using PipelineService.Models;

namespace BusinessLogic.Grains
{
    public class OrderGrain : Grain, IOrder, IState
    {
        private Order state { get; set; }
        private IRepository<Order, Guid> _repository;
        private IPipelineAlloc<OrderProcessingPipeline> _pipelineAlloc;

        public override Task OnActivateAsync()
        {
            _pipelineAlloc = new PipelineAlloc<OrderProcessingPipeline>();
            _repository = new OrderRepository();
            //Initializes on grain activation
            state = new Order();

            state.OrderId = this.GetPrimaryKey();
            ReadState();

            return base.OnActivateAsync();
        }
        /// <summary>
        /// Updates the state on deactivation
        /// </summary>
        /// <returns></returns>
        public override Task OnDeactivateAsync()
        {
            UpdateState();
            return base.OnDeactivateAsync();
        }

        public Task CreateOrder(Order order)
        {
            WriteState();
            return Task.CompletedTask;
        }

        public Task UpdateOrder(Order order)
        {
            try
            {
                this.state = order;
                UpdateState();
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }

        public Task<Order> GetOrder()
        {
            return Task.FromResult(state);
        }

        public Task ProcessOrder(OrderProcessing orderProcessing)
        {
            var list = new List<OrderProcessing>();
            list.Add(orderProcessing);
            IPipeline<OrderProcessing> _pipeline = _pipelineAlloc.RetrievePipeline().Result;
            _pipeline.ProcessAndForget(list);

            return Task.CompletedTask;
        }

        public Task RemoveOrder()
        {
            DeleteState();
            return Task.CompletedTask;
        }

        public void ReadState()
        {
            var temp = _repository.Get(this.GetPrimaryKey());
            if (temp != null)
            {
                state = temp;
            }
        }
        public void DeleteState()
        {
            _repository.Delete(state);
        }

        public void UpdateState()
        {
            try
            {
                _repository.Update(state);
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
        }

        public void WriteState()
        {
            _repository.Add(state);
        }
    }
}
