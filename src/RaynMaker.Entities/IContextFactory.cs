
namespace RaynMaker.Entities
{
    public interface IContextFactory
    {
        IAssetsContext CreateAssetsContext();
        IAnalysisContext CreateAnalysisContext();
        ICurrenciesContext CreateCurrenciesContext();
    }
}
