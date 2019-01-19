using DataModels.Models;
using Orleans;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface ITransaction : IGrainWithIntegerCompoundKey
    {
        Task<Transaction> GetTransaction();
        Task<bool> AddTransaction(Transaction transaction);
        Task<TransactionType> GetTransactionType();
    }
}
