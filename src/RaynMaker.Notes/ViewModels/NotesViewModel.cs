﻿using System;
using System.ComponentModel.Composition;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Notes.ViewModels
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
            myStorageService.Store( new TextRange( myDocument.ContentStart, myDocument.ContentEnd ) );

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
                        myStorageService.Load( new TextRange( myDocument.ContentStart, myDocument.ContentEnd ) );
                    }
                }
            }
        }
    }
}
