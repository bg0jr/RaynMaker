using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Analysis.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analysis.ViewModels
{
    [Export]
    class AnalysisTemplateEditViewModel : BindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private TextDocument myDocument;

        [ImportingConstructor]
        public AnalysisTemplateEditViewModel(IProjectHost projectHost,  StorageService storageService )
        {
            myProjectHost = projectHost;
            myStorageService = storageService;

            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();

            Document = new TextDocument();

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            Document.Text = myStorageService.LoadAnalysisTemplateText();
        }
        
        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public TextDocument Document
        {
            get { return myDocument; }
            set { SetProperty( ref myDocument, value ); }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            if( Document != null )
            {
                myStorageService.SaveAnalysisTemplate( Document.Text );
            }

            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }
    }
}
