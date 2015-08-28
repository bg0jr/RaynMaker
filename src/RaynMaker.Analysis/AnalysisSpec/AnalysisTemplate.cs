using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Analysis.AnalysisSpec
{
    [DefaultProperty( "Analysis" ), ContentProperty( "Analysis" )]
    public class AnalysisTemplate : DataTemplate
    {
        [Required,ValidateObject]
        public Analysis Analysis { get; set; }
    }
}
