
namespace RaynMaker.Blade.Engine
{
    interface IExpressionEvaluationContext
    {
        object ProvideValue( string providerName );
    }
}
