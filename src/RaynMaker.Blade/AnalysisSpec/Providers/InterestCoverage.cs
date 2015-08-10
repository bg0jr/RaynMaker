using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    /// <summary>
    /// InterestCoverage = EBIT / Interest Expense
    /// </summary>
    public class InterestCoverage : IFigureProvider
    {
        public string Name { get { return ProviderNames.InterestCoverage; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allEbit = context.GetDatumSeries<EBIT>();
            var allInterestExpense = context.GetDatumSeries<InterestExpense>();

            if( !allEbit.Any() || !allInterestExpense.Any() )
            {
                return null;
            }

            var ebit = allEbit
                .OrderByDescending( a => a.Year )
                .First();

            var interestExpense = allInterestExpense.SingleOrDefault( d => d.Year == ebit.Year );
            if( interestExpense == null )
            {
                return null;
            }

            var result = new DerivedDatum
            {
                Year = ebit.Year,
                Value = ebit.Value / interestExpense.Value
            };
            result.Inputs.Add( ebit );
            result.Inputs.Add( interestExpense );
            return result;
        }
    }
}
