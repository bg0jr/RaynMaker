using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Analysis.Engine;

namespace RaynMaker.Analysis.AnalysisSpec
{
    [DefaultProperty( "Elements" ), ContentProperty( "Elements" )]
    public class AnalysisTemplate : DataTemplate
    {
        public AnalysisTemplate()
        {
            Elements = new List<IReportElement>();
        }

        [ValidateObject]
        public List<IReportElement> Elements { get; private set; }
    }
}
