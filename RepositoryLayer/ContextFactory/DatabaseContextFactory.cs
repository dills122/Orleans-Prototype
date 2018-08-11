using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.ContextFactory
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        public DatabaseContextFactory()
        {
            
        }

        public DbContext Create()
        {
            return new OrleansContext();
        }
    }
}
