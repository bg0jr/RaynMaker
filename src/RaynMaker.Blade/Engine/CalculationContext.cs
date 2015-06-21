using System.IO;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class CalculationContext
    {
        public CalculationContext( Asset asset, TextWriter writer )
        {
            Asset = asset;
            Out = new Report( writer );
        }

        public Asset Asset { get; private set; }

        public Report Out { get; private set; }
    }
}
