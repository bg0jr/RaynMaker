using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Plainion.Collections;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Windows.Controls;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    [Export]
    class EditCaptureViewModel : BindableBase, IInteractionRequestAware
    {
        private DocumentLocationFragment mySelectedUrl;
        private INotification myNotification;

        public EditCaptureViewModel()
        {
            Urls = new ObservableCollection<DocumentLocationFragment>();

            DeleteUrlCommand = new DelegateCommand<DocumentLocationFragment>( OnDeleteUrl );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public ObservableCollection<DocumentLocationFragment> Urls { get; private set; }

        public DocumentLocationFragment SelectedUrl
        {
            get { return mySelectedUrl; }
            set { SetProperty( ref mySelectedUrl, value ); }
        }

        public ICommand DeleteUrlCommand { get; private set; }

        private void OnDeleteUrl( DocumentLocationFragment url )
        {
            Urls.Remove( url );
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            TextBoxBinding.ForceSourceUpdate();

            Notification.TrySetConfirmed( true );

            Notification.Content = Urls.ToList();

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

                Urls.Clear();
                Urls.AddRange( ( IEnumerable<DocumentLocationFragment> )myNotification.Content );
                SelectedUrl = Urls.FirstOrDefault();
            }
        }
    }
}
