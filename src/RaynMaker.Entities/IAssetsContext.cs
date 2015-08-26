using System;
using System.Data.Entity;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities
{
    public interface IAssetsContext 
    {
        IDbSet<Currency> Currencies { get; }
        
        IDbSet<Company> Companies { get; }

        IDbSet<Stock> Stocks { get; }

        int SaveChanges();
    }
}
