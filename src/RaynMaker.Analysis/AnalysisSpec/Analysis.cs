using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Analysis.Engine;

namespace RaynMaker.Analysis.AnalysisSpec
{
    [DefaultProperty( "Elements" ), ContentProperty( "Elements" )]
    public class Analysis
    {
        public Analysis()
        {
            Elements = new List<IReportElement>();
        }

        [ValidateObject]
        public List<IReportElement> Elements { get; private set; }
    }
}
