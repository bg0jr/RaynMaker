using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    /// <summary>
    /// ReturnOnEquity = NetIncome / Equity (in %)
    /// </summary>
    public class ReturnOnEquity : IFigureProvider
    {
        public string Name { get { return ProviderNames.ReturnOnEquity; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allNetIncome = context.GetDatumSeries<NetIncome>();
            var allEquity = context.GetDatumSeries<Equity>();

            if( !allNetIncome.Any() || !allEquity.Any() )
            {
                return new Series();
            }

            var result = new Series();

            foreach( var netIncome in allNetIncome )
            {
                var equity = allEquity.SingleOrDefault( e => e.Year == netIncome.Year );
                if( equity != null )
                {
                    var roe = new DerivedDatum
                    {
                        Year = netIncome.Year,
                        Value = netIncome.Value / equity.Value * 100
                    };
                    roe.Inputs.Add( netIncome );
                    roe.Inputs.Add( equity );
                    result.Values.Add( roe );
                }
            }

            return result;
        }
    }
}
