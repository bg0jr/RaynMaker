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
        public FormatViewModelBase Create( IFigureExtractionDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );

            // needs to be before PathSeriesFormat for the moment because it is derived class of it
            if( format is PathCellExtractionDescriptor )
            {
                return new PathCellFormatViewModel( myLutService, format as PathCellExtractionDescriptor );
            }

            if( format is PathSeriesExtractionDescriptor )
            {
                return new PathSeriesFormatViewModel( format as PathSeriesExtractionDescriptor );
            }

            throw new NotSupportedException( "Unknown format: " + format.GetType().Name );
        }
    }
}
