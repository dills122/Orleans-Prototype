using DataModels.Models;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface IAccount : IGrainWithIntegerKey
    {
        Task<Account> GetAccountInfo();
        Task<bool> CreateAccount(Account account);
        Task<bool> UpdateAccountInfo(Account account);
        Task<bool> RemoveAccount();

        Task<List<int>> GetTransactions(bool? onlyRecent);
    }
}
