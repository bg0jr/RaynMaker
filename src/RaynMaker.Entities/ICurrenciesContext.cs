using System;
using System.Data.Entity;

namespace RaynMaker.Entities
{
    public interface ICurrenciesContext 
    {
        IDbSet<Currency> Currencies { get; }

        int SaveChanges();
    }
}
