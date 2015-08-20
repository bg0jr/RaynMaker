using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Blade
{
    [Export( typeof( BladeMenuItemModel ) )]
    public class BladeMenuItemModel : BindableBase
    {
        public BladeMenuItemModel()
        {
            InvokeCommand = new DelegateCommand( OnInvoke );
            InvokeRequest = new InteractionRequest<INotification>();
        }

        private void OnInvoke()
        {
            var notification = new Notification();
            notification.Title = "RaynMaker.Blade";

            InvokeRequest.Raise( notification, c => { } );
        }

        public ICommand InvokeCommand { get; private set; }

        public InteractionRequest<INotification> InvokeRequest { get; private set; }
    }
}
