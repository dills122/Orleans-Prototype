using Microsoft.EntityFrameworkCore;
using System;

namespace RepositoryLayer.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext context { get; }
        void Commit();
    }
}
