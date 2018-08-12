using DataModels.Exceptions;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using RepositoryLayer.Repository;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryLayer.RepositoryExtensions;

namespace BusinessLogic.Grains
{
    public class UserGrain : Grain, IUser
    {
        private User state { get; set; }
        private List<Guid> orderIds { get; set; }

        private IRepository<User,string> _repository;

        public override Task OnActivateAsync()
        {
            _repository = new UserRepository();
            //Initializes on grain activation
            state = new User();
            orderIds = new List<Guid>();

            state.Username = this.GetPrimaryKeyString();
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
        /// <summary>
        /// Creates the users in storage
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateUser(User user)
        {
            state = new User
            {
                Username = this.GetPrimaryKeyString(),
                accountType = user.accountType,
                FName = user.FName,
                LName = user.LName,
                CreatedDate = DateTime.Now
            };
            WriteState();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <returns></returns>
        public Task DeleteUser()
        {
            DeleteState();
            return Task.CompletedTask;
        }
        /// <summary>
        /// Gets the current users data
        /// </summary>
        /// <returns></returns>
        public Task<User> GetUser()
        {
            return Task.FromResult(state);
        }
        /// <summary>
        /// Gets all of the order ids 
        /// </summary>
        /// <returns></returns>
        public Task<List<Guid>> GetOrders()
        {
           return Task.FromResult(orderIds);
        }

        public Task UpdateUser(User user)
        {
            //Reason for second try/catch is to bubble the error to controller
            try
            {
                state = new User
                {
                    Username = this.GetPrimaryKeyString(),
                    accountType = user.accountType,
                    FName = user.FName,
                    LName = user.LName,
                    RowVersion = user.RowVersion,
                    CreatedDate = user.CreatedDate
                };
                UpdateState();
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns></returns>
        public void WriteState()
        {
            _repository.Add(state);
        }
        /// <summary>
        /// Reads State of user grain
        /// also gets all OrderId's associated with it
        /// </summary>
        /// <returns></returns>
        public void ReadState()
        {
            var temp = _repository.Get(this.GetPrimaryKeyString());
            if(temp != null)
            {
                state = temp;
            }
            IOrderRepository<Order, Guid> orderRepo = new OrderRepository();
            var orderIds = orderRepo.GetOrderIds(state.Username);
            if(orderIds != null)
            {
                this.orderIds = (List<Guid>)orderIds;
            }
        }

        /// <summary>
        /// Database wins Example of Optimistic Concurrency Pattern
        /// </summary>
        /// <returns></returns>
        public void UpdateState()
        {
            try
            {
                _repository.Update(state);
            } 
            catch(UpdateException ex)
            {
                throw ex;
            }
        }

        public void DeleteState()
        {
            _repository.Delete(state);
        }
    }
}
