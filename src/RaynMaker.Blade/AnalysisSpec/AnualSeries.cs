using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec
{
    [DefaultProperty( "Items" ), ContentProperty( "Items" )]
    public class AnualSeries : IReportElement
    {
        public AnualSeries()
        {
            Items = new List<IReportElement>();
        }

        [ValidateObject]
        public List<IReportElement> Items { get; private set; }

        [Required]
        public int Count { get; set; }

        public void Report( ReportContext context )
        {
            //int year = DateTime.Today.Year;
            //for( int i = 0; i < Count; ++i )
            //{
            //    context.Out.Write( year - i );
            //}
            
            //foreach( var item in Items )
            //{
            //    context.Out.Write(item.Name);
            //    for( int i = 0; i < Count; ++i )
            //    {
            //        context.Out.Write( item.ProvideValue( context, year - i ) );
            //    }
            //}
        }
    }
}
