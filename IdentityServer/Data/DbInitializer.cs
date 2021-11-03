using Microsoft.EntityFrameworkCore;
using System;
namespace IdentityServer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
