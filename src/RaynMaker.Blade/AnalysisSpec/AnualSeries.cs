using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Markup;
using Plainion;
using Plainion.Validation;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Rows" ), ContentProperty( "Rows" )]
    public class AnualSeries : IReportElement
    {
        public AnualSeries()
        {
            Rows = new List<Row>();
        }

        [ValidateObject]
        public List<Row> Rows { get; private set; }

        [Required]
        public int EndYear { get; set; }

        [Required]
        public int Count { get; set; }

        public void Report( ReportContext context )
        {
            context.Out.Write( "{0,20}", "" );
            for( int year = EndYear - Count + 1; year <= EndYear; ++year )
            {
                context.Out.Write( "{0,8}", year );
            }

            context.Out.WriteLine();

            foreach( var row in Rows )
            {
                var provider = context.GetProvider( row.Value );

                context.Out.Write( "{0,-20}", provider.Name );

                var series = ( Series )provider.ProvideValue( context.Asset );
                var values = series.Values
                    .Cast<AnualDatum>()
                    .ToList();

                for( int year = EndYear - Count + 1; year <= EndYear; ++year )
                {
                    var value = values.SingleOrDefault( v => v.Year == year );
                    if( value == null )
                    {
                        context.Out.Write( "{0,8}", "n.a." );
                    }
                    else
                    {
                        context.Out.Write( "{0,8:0.00}", value.Value );
                    }
                }
            
                context.Out.WriteLine();
            }
        }
    }
}
