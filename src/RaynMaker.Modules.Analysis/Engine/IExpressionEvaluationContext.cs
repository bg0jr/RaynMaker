
namespace RaynMaker.Modules.Analysis.Engine
{
    interface IExpressionEvaluationContext
    {
        object ProvideValue( string providerName );
    }
}
