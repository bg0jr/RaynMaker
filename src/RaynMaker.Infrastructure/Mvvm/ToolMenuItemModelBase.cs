using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace RaynMaker.Infrastructure.Mvvm
{
    public class ToolMenuItemModelBase : BindableBase
    {
        private IProjectHost myProjectHost;
        private string myPopupTitle;

        protected ToolMenuItemModelBase( IProjectHost projectHost, string popupTitle )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += myProjectHost_Changed;

            myPopupTitle = popupTitle;

            InvokeCommand = new DelegateCommand( OnInvoke );
            InvokeRequest = new InteractionRequest<INotification>();
        }

        public bool IsEnabled { get { return myProjectHost.Project != null; } }

        private void myProjectHost_Changed()
        {
            OnPropertyChanged( () => IsEnabled );
        }

        private void OnInvoke()
        {
            var notification = new Notification();
            notification.Title = myPopupTitle;

            InvokeRequest.Raise( notification, c => { } );
        }

        public ICommand InvokeCommand { get; private set; }

        public InteractionRequest<INotification> InvokeRequest { get; private set; }
    }
}
