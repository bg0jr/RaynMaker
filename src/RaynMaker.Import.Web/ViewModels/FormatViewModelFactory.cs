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

            // needs to be before PathSeriesFormat for the moment because it is derived class of it
            if( format is PathCellFormat )
            {
                return new PathCellFormatViewModel( format as PathCellFormat );
            }

            if( format is PathSeriesFormat )
            {
                return new PathSeriesFormatViewModel( format as PathSeriesFormat );
            }

            throw new NotSupportedException( "Unknown format: " + format.GetType().Name );
        }
    }
}
