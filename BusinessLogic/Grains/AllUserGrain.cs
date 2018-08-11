using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Orleans;
using RepositoryLayer;
using RepositoryLayer.Abstraction;
using RepositoryLayer.Repository;
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
        public IBulkRepository<User, string> _repository;

        public override Task OnActivateAsync()
        {
            _repository = new BulkUserRepository();
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

        public Task<List<User>> GetAllUsers()
        {
            //Reads States to make sure all updates are added to grain
            ReadState();
            return Task.FromResult(users);
        }

        public void WriteState()
        {
            throw new NotImplementedException();
        }

        public void ReadState()
        {
            users = _repository.GetAll().ToList();
        }

        public void UpdateState()
        {
            throw new NotImplementedException();
        }

        public void DeleteState()
        {
            throw new NotImplementedException();
        }
    }
}
