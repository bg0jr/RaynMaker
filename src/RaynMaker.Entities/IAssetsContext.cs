using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public interface IAssetsContext
    {
        IEnumerable<Company> Companies { get; }
        IEnumerable<Stock> Stocks { get; }
    }
}
