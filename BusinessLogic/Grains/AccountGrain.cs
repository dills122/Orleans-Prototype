using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class AccountGrain : Grain, IAccount, IState
    {
        private Account _state { get; set; }

        public override Task OnActivateAsync()
        {
            //_repository = new OrderRepository();
            _state = new Account();

            if ((int)this.GetPrimaryKeyLong() != 0)
            {
                _state.AccountNumber = (int)this.GetPrimaryKeyLong();
                ReadState();
            }

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
            throw new NotImplementedException();
        }

        public Task<Account> GetAccountInfo()
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetTransactions(bool? onlyRecent)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAccount()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccountInfo(Account account)
        {
            throw new NotImplementedException();
        }

        public void WriteState()
        {
            throw new NotImplementedException();
        }

        public void ReadState()
        {
            throw new NotImplementedException();
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
