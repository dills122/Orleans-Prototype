using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.ContextFactory
{
    public interface IDatabaseContextFactory
    {
        DbContext Create();
    }
}
