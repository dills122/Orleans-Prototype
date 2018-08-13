using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
   public interface IUser :IGrainWithStringKey
    {
        Task CreateUser(User State);
        Task UpdateUser(User State);
        Task<List<Guid>> GetOrders();
        Task DeleteUser();
        Task<User> GetUser();
    }
}
