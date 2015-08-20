using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade
{
    [Export( typeof( BladeMenuItemModel ) )]
    public class BladeMenuItemModel : BindableBase
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public BladeMenuItemModel( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += myProjectHost_Changed;

            InvokeCommand = new DelegateCommand( OnInvoke );
            InvokeRequest = new InteractionRequest<INotification>();
        }

        public bool IsEnabled { get { return myProjectHost.Project != null; } }

        void myProjectHost_Changed()
        {
            OnPropertyChanged( () => IsEnabled );
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
