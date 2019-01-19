using BusinessLogic.GrainInterfaces;
using DataModels.Exceptions;
using DataModels.Models;
using Orleans;
using RepositoryLayer.Repository;
using RepositoryLayer.RepositoryExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class AccountGrain : Grain, IAccount, IState
    {
        private Account _state { get; set; }
        private IAccountRepository<Account, int> _repository { get; set; }

        public override Task OnActivateAsync()
        {
            _repository = new AccountRepository();
            _state = new Account();

            _state.AccountNumber = (int)this.GetPrimaryKeyLong();
            ReadState();

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            if ((int)this.GetPrimaryKeyLong() != 0)
            {
                UpdateState();
            }

            return base.OnDeactivateAsync();
        }

        public Task<bool> CreateAccount(Account account)
        {
            if(account != null)
            {
                _state = account;
                WriteState();
            }
            return Task.FromResult(true);
        }

        public Task<Account> GetAccountInfo()
        {
            ReadState();
            return Task.FromResult(_state);
        }

        public Task<List<int>> GetTransactions(bool? onlyRecent)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAccount()
        {
            DeleteState();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateAccountInfo(Account account)
        {
            try
            {
                _state = account;
                UpdateState();
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
            return Task.FromResult(true);
        }

        public void WriteState()
        {
            _repository.Add(_state);
        }

        public void ReadState()
        {
            var temp = _repository.Get((int)this.GetPrimaryKeyLong());
            if (temp != null)
            {
                _state = temp;
            }
        }

        public void UpdateState()
        {
            try
            {
                _repository.Update(_state);
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
        }

        public void DeleteState()
        {
            _repository.Delete(_state);
        }
    }
}
