using System;
using Plainion;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatViewModelFactory
    {
        public static FormatViewModelBase Create( IFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            if( format is PathSeriesFormat )
            {
                return new PathSeriesFormatViewModel( format as PathSeriesFormat );
            }

            throw new NotSupportedException( "Unknown format: " + format.GetType().Name );
        }
    }
}
