using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;

namespace RaynMaker.Browser.ViewModels
{
    [Export]
    class NewAssetViewModel : BindableBase, IInteractionRequestAware
    {
        public NewAssetViewModel()
        {
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public ICommand CancelCommand { get; private set; }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        private void OnCancel()
        {
            Notification.TrySetConfirmed( false );
            FinishInteraction();
        }
    }
}
