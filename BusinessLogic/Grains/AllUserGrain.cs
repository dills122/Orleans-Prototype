using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Orleans;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class AllUserGrain : Grain, IAllUsers
    {
        private List<User> users { get; set; }

        public override Task OnActivateAsync()
        {
            //Initializes on grain activation
            users = new List<User>();

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
        public Task DeleteState()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            //Reads States to make sure all updates are added to grain
            ReadState();
            return Task.FromResult(users);
        }

        public Task ReadState()
        {
            using(var context = new OrleansContext())
            {
                var users = context.users.ToList();
                if(users != null)
                {
                    this.users = users;
                }
            }
            return Task.CompletedTask;
        }

        public Task UpdateState()
        {
            throw new NotImplementedException();
        }

        public Task WriteState()
        {
            throw new NotImplementedException();
        }

        void IState.WriteState()
        {
            throw new NotImplementedException();
        }

        void IState.ReadState()
        {
            throw new NotImplementedException();
        }

        void IState.UpdateState()
        {
            throw new NotImplementedException();
        }

        void IState.DeleteState()
        {
            throw new NotImplementedException();
        }
    }
}
