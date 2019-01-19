using BusinessLogic.GrainInterfaces;
using DataModels.Exceptions;
using DataModels.Models;
using Orleans;
using RepositoryLayer.Repository;
using RepositoryLayer.RepositoryExtensions;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class TransactionGrain : Grain, ITransaction, IState
    {
        private Transaction _state { get; set; }
        private ITransactionRepository<Transaction, int> _repository { get; set; }

        public override Task OnActivateAsync()
        {
            _repository = new TransactionRepository();
            _state = new Transaction();
            var transactionId = (int)this.GetPrimaryKeyLong(out string keyExtension);
            if (!String.IsNullOrEmpty(keyExtension))
            {
                _state.TransactionId = transactionId;
                _state.AccountId = Convert.ToInt32(keyExtension);
                ReadState();
            }

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            var transactionId = (int)this.GetPrimaryKeyLong(out string keyExtension);
            if (!String.IsNullOrEmpty(keyExtension))
            {
                UpdateState();
            }

            return base.OnDeactivateAsync();
        }

        public Task<bool> AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                _state = transaction;
                WriteState();
            }
            return Task.FromResult(true);
        }

        public Task<Transaction> GetTransaction()
        {
            return Task.FromResult(_state);
        }

        public Task<TransactionType> GetTransactionType()
        {
            if (_state.TransactionId <= 0 || _state.AccountId <= 0)
            {
                throw new NullReferenceException();
            }
            return Task.FromResult(_state.TransactionType);
        }

        public void WriteState()
        {
            _repository.Add(_state);
        }

        public void ReadState()
        {
            var transactionId = (int)this.GetPrimaryKeyLong(out string keyExtension);
            var temp = _repository.Get(transactionId);
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
