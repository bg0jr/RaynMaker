using System;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class FormatSelectionNotification : Confirmation
    {
        public FormatSelectionNotification()
        {
            FormatType = typeof( PathSeriesDescriptor );
        }

        public Type FormatType { get; set; }
    }
}
