using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using RaynMaker.Entities;

namespace RaynMaker.Browser.ViewModels
{
    class NewAssetNotification : Confirmation
    {
        public Stock Result { get; set; }
    }
}
