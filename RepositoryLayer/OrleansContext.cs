using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer
{
    public class OrleansContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public DbSet<Account> accounts { get; set; }

        public DbSet<Transaction> transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=OrleansCustomState;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
