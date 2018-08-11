using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext context { get; }
        void Commit();
    }
}
