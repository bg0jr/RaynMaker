
namespace RaynMaker.Analysis.Engine
{
    interface IExpressionEvaluationContext
    {
        object ProvideValue( string providerName );
    }
}
