using DataModels.Models;
using RepositoryLayer.Abstraction;
using RepositoryLayer.RepositoryExtensions;
using System;

namespace RepositoryLayer.Repository
{
    public class AccountRepository : GenericRepository<Account, int>, IAccountRepository<Account, int>
    {
        protected override bool MatchEntity(Account source, Account target)
        {
            throw new NotImplementedException();
        }

        protected override bool MatchIntForGetList(Account source, int foreignKey)
        {
            throw new NotImplementedException();
        }

        protected override bool MatchKey(Account entity, int key) => entity.AccountNumber == key;
    }
}
