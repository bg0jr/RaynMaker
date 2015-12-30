using System;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Web.ViewModels
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
