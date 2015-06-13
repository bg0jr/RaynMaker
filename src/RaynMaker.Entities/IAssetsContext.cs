using System;
using System.Data.Entity;

namespace RaynMaker.Entities
{
    public interface IAssetsContext : IDisposable
    {
        IDbSet<Company> Companies { get; }
        IDbSet<Stock> Stocks { get; }

        int SaveChanges();
    }
}
