using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public interface IEntityContext
    {
        IEnumerable<Company> Companies { get; }
        IEnumerable<Stock> Stocks { get; }
    }
}
