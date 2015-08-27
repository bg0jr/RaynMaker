using System.ComponentModel.Composition;

namespace RaynMaker.Infrastructure.Services
{
    [InheritedExport]
    public interface ILutService
    {
        ICurrenciesLut CurrenciesLut { get; }
    }
}
