using RepositoryLayer.Abstraction;
using RepositoryLayer.RepositoryExtensions;
using System;
using DataModels.Models;

namespace RepositoryLayer.Repository
{
    public class TransactionRepository : GenericRepository<Transaction, int>, ITransactionRepository<Transaction,int>
    {
        protected override bool MatchEntity(Transaction source, Transaction target)
        {
            throw new NotImplementedException();
        }

        protected override bool MatchIntForGetList(Transaction source, int foreignKey)
        {
            throw new NotImplementedException();
        }

        protected override bool MatchKey(Transaction entity, int key) => entity.TransactionId == key;
    }
}
