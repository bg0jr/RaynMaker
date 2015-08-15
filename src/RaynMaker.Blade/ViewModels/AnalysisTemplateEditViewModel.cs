using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class AnalysisTemplateEditViewModel : BindableBase, IInteractionRequestAware
    {
        private Project myProject;
        private StorageService myStorageService;
        private TextDocument myDocument;

        [ImportingConstructor]
        public AnalysisTemplateEditViewModel( Project project, StorageService storageService )
        {
            myProject = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( myProject, OnProjectPropertyChanged,
                PropertySupport.ExtractPropertyName( () => myProject.AnalysisTemplateLocation ) );
            OnProjectPropertyChanged( null, null );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( string.IsNullOrEmpty( myProject.AnalysisTemplateLocation ) || !File.Exists( myProject.AnalysisTemplateLocation ) )
            {
                return;
            }

            Document = new TextDocument();
            Document.Text = File.ReadAllText( myProject.AnalysisTemplateLocation );
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
                using( var writer = new StreamWriter( myProject.AnalysisTemplateLocation ) )
                {
                    Document.WriteTextTo( writer );
                }
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
