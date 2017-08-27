using System;
using System.ComponentModel.Composition;
using System.Windows.Documents;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace RaynMaker.Modules.Notes.ViewModels
{
    [Export]
    class NotesViewModel : BindableBase, IInteractionRequestAware
    {
        private StorageService myStorageService;
        private FlowDocument myDocument;

        [ImportingConstructor]
        public NotesViewModel( StorageService storageService )
        {
            myStorageService = storageService;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            myStorageService.Store( Document);

            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }

        public FlowDocument Document
        {
            get { return myDocument; }
            set
            {
                if( SetProperty( ref myDocument, value ) )
                {
                    if( myDocument != null )
                    {
                        myStorageService.Load( myDocument);
                    }
                }
            }
        }
    }
}
