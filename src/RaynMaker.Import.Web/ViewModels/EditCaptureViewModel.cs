using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Windows.Controls;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class EditCaptureViewModel : BindableBase, IInteractionRequestAware
    {
        private IEnumerable<NavigatorUrl> myUrls;
        private NavigatorUrl mySelectedUrl;
        private INotification myNotification;

        public EditCaptureViewModel()
        {
            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public IEnumerable<NavigatorUrl> Urls
        {
            get { return myUrls; }
            set { SetProperty( ref myUrls, value ); }
        }

        public NavigatorUrl SelectedUrl
        {
            get { return mySelectedUrl; }
            set { SetProperty( ref mySelectedUrl, value ); }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            TextBoxBinding.ForceSourceUpdate();

            Notification.TrySetConfirmed( true );

            Notification.Content = Urls;

            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            Notification.TrySetConfirmed( false );
            FinishInteraction();
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return myNotification; }
            set
            {
                myNotification = value;
                Urls = ( IEnumerable<NavigatorUrl> )myNotification.Content;
            }
        }
    }
}
