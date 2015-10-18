using System;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatSelectionNotification : Confirmation
    {
        public FormatSelectionNotification()
        {
            FormatType = typeof( PathSeriesFormat );
        }

        public Type FormatType { get; set; }
    }
}
