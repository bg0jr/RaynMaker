using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Figures" ), ContentProperty( "Figures" )]
    public class Analysis
    {
        public Analysis()
        {
            Figures = new List<IReportElement>();
        }

        [ValidateObject]
        public List<IReportElement> Figures { get; private set; }
    }
}
