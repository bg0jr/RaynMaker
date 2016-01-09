using System;
using Plainion;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class FigureDescriptorViewModelFactory
    {
        private ILutService myLutService;

        public FigureDescriptorViewModelFactory( ILutService lutService )
        {
            Contract.RequiresNotNull( lutService, "lutService" );

            myLutService = lutService;
        }

        public IDescriptorViewModel Create( IFigureDescriptor descriptor )
        {
            Contract.RequiresNotNull( descriptor, "descriptor " );

            if( descriptor is PathCellDescriptor )
            {
                return new PathCellDescriptorViewModel( myLutService, descriptor as PathCellDescriptor );
            }

            if( descriptor is PathSeriesDescriptor )
            {
                return new PathSeriesDescriptorViewModel( descriptor as PathSeriesDescriptor );
            }

            throw new NotSupportedException( "Unknown descriptor type: " + descriptor.GetType().Name );
        }
    }
}
