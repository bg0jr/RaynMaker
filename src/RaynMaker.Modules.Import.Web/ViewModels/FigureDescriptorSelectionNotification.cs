using System;
using Prism.Interactivity.InteractionRequest;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class FigureDescriptorSelectionNotification : Confirmation
    {
        public FigureDescriptorSelectionNotification()
        {
            DescriptorType = typeof( PathSeriesDescriptor );
        }

        public Type DescriptorType { get; set; }
    }
}
