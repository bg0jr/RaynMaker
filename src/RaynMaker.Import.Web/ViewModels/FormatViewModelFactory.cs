using System;
using Plainion;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;
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
        public FormatViewModelBase Create( IFigureDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );

            // needs to be before PathSeriesFormat for the moment because it is derived class of it
            if( format is PathCellDescriptor )
            {
                return new PathCellFormatViewModel( myLutService, format as PathCellDescriptor );
            }

            if( format is PathSeriesDescriptor )
            {
                return new PathSeriesFormatViewModel( format as PathSeriesDescriptor );
            }

            throw new NotSupportedException( "Unknown format: " + format.GetType().Name );
        }
    }
}
