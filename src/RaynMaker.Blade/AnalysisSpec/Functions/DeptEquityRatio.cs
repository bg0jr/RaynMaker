using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class DeptEquityRatio : IFigureProvider
    {
        public string Name { get { return FunctionNames.DeptEquityRatio; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allDept = context.GetDatumSeries<Dept>();
            var allEquity = context.GetDatumSeries<Equity>();

            if( !allDept.Any() || !allEquity.Any() )
            {
                return null;
            }

            var dept = allDept
                .OrderByDescending( a => a.Year )
                .First();

            var equity = allEquity.SingleOrDefault( d => d.Year == dept.Year );
            if( equity == null )
            {
                return null;
            }

            var result = new DerivedDatum
            {
                Year = dept.Year,
                Value = dept.Value / equity.Value
            };
            result.Inputs.Add( dept );
            result.Inputs.Add( equity );
            return result;
        }
    }
}
