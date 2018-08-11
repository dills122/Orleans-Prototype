using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext context { get; }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
