using System.Linq;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class Price : IReportElement
    {
        public void Report( CalculationContext context )
        {
            var price = context.Asset.Data.OfType<DataSheetSpec.Price>().SingleOrDefault();
            if( price == null )
            {
                context.Out.Write("Current price", "n.a." );
            }
            else
            {
                context.Out.Write( "Current price", price );
            }
        }
    }
}
