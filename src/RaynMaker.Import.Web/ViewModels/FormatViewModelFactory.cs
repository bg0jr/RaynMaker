using System;
using Plainion;
using RaynMaker.Import.Spec;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatViewModelFactory
    {
        private ILutService myLutService;

        public FormatViewModelFactory( ILutService lutService )
        {
            Contract.RequiresNotNull( lutService, "lutService" );

            myLutService = lutService;
        }
        public FormatViewModelBase Create( IFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            // needs to be before PathSeriesFormat for the moment because it is derived class of it
            if( format is PathCellFormat )
            {
                return new PathCellFormatViewModel( myLutService, format as PathCellFormat );
            }

            if( format is PathSeriesFormat )
            {
                return new PathSeriesFormatViewModel( format as PathSeriesFormat );
            }

            throw new NotSupportedException( "Unknown format: " + format.GetType().Name );
        }
    }
}
