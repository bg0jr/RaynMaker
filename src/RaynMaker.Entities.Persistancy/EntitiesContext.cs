using System.Collections.Generic;
using System.Data.Entity;

namespace RaynMaker.Entities.Persistancy
{
    class EntitiesContext : DbContext, IEntityContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        IEnumerable<Company> IEntityContext.Companies { get { return Companies; } }

        IEnumerable<Stock> IEntityContext.Stocks { get { return Stocks; } }
    }
}
