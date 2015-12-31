using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataFormatsViewModel : SpecDefinitionViewModelBase
    {
        private int mySelectedFormatIndex;
        private IDocument myDocument;
        private IDocumentBrowser myBrowser;
        private FormatViewModelFactory myFormatViewModelFactory;

        public DataFormatsViewModel( Session session, ILutService lutService )
            : base( session )
        {
            myFormatViewModelFactory = new FormatViewModelFactory( lutService );

            Session.ApplyCurrentFormat = ApplyCurrentFormat;

            PropertyChangedEventManager.AddHandler( Session, OnCurrentSourceChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) );

            mySelectedFormatIndex = -1;

            Formats = new ObservableCollection<FormatViewModelBase>();

            AddCommand = new DelegateCommand( OnAdd );
            FormatSelectionRequest = new InteractionRequest<FormatSelectionNotification>();

            RemoveCommand = new DelegateCommand( OnRemove );
            CopyCommand = new DelegateCommand( OnCopy );

            OnCurrentSourceChanged( null, null );
        }

        private void ApplyCurrentFormat()
        {
            if( SelectedFormatIndex != -1 )
            {
                Formats[ SelectedFormatIndex ].Apply();
            }
        }

        private void OnCurrentSourceChanged( object sender, PropertyChangedEventArgs e )
        {
            SelectedFormatIndex = -1;

            foreach( var format in Formats )
            {
                format.Document = null;
            }
            Formats.Clear();

            if( Session.CurrentSource != null )
            {
                foreach( var format in Session.CurrentSource.ExtractionSpec )
                {
                    Formats.Add( myFormatViewModelFactory.Create( format ) );
                }

                SelectedFormatIndex = Formats.Count == 0 ? -1 : 0;
            }
        }

        public IDocumentBrowser Browser
        {
            get { return myBrowser; }
            set
            {
                var oldBrowser = myBrowser;
                if( SetProperty( ref myBrowser, value ) )
                {
                    if( oldBrowser != null )
                    {
                        oldBrowser.DocumentCompleted -= BrowserDocumentCompleted;
                    }
                    if( myBrowser != null )
                    {
                        myBrowser.DocumentCompleted += BrowserDocumentCompleted;
                    }
                }
            }
        }

        private void BrowserDocumentCompleted( IDocument doc )
        {
            Document = doc;
        }

        public ObservableCollection<FormatViewModelBase> Formats { get; private set; }

        public int SelectedFormatIndex
        {
            get { return mySelectedFormatIndex; }
            set
            {
                if( value == Formats.Count )
                {
                    // this is caused by binding this property to TabControl
                    value = -1;
                }

                var oldFormat = mySelectedFormatIndex;
                if( SetProperty( ref mySelectedFormatIndex, value ) )
                {
                    if( oldFormat != -1 )
                    {
                        Formats[ oldFormat ].UnMark();
                        Formats[ oldFormat ].Document = null;
                    }

                    if( mySelectedFormatIndex != -1 )
                    {
                        Session.CurrentFormat = Formats[ mySelectedFormatIndex ].Format;
                        Formats[ mySelectedFormatIndex ].Document = myDocument;
                        Formats[ mySelectedFormatIndex ].Apply();
                    }
                }
            }
        }

        public IDocument Document
        {
            get { return myDocument; }
            set
            {
                // always force update because the document reference does NOT change!

                myDocument = value;

                if( 0 <= mySelectedFormatIndex && mySelectedFormatIndex < Formats.Count )
                {
                    Formats[ mySelectedFormatIndex ].Document = myDocument;
                }
            }
        }

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
            var notification = new FormatSelectionNotification();
            notification.Title = "Format selection";

            FormatSelectionRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    var format = FormatFactory.Create( n.FormatType );

                    Session.CurrentSource.ExtractionSpec.Add( format );

                    Formats.Add( myFormatViewModelFactory.Create( format ) );

                    SelectedFormatIndex = Formats.Count - 1;
                }
            } );
        }

        public InteractionRequest<FormatSelectionNotification> FormatSelectionRequest { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
            if( mySelectedFormatIndex == -1 )
            {
                return;
            }

            var formatVM = Formats[ SelectedFormatIndex ];

            SelectedFormatIndex = Formats.Count - 2;

            Session.CurrentSource.ExtractionSpec.Remove( formatVM.Format );
            Formats.Remove( formatVM );
        }

        public ICommand CopyCommand { get; private set; }

        private void OnCopy()
        {
            if( mySelectedFormatIndex == -1 )
            {
                return;
            }

            var format = FormatFactory.Clone( Formats[ SelectedFormatIndex ].Format );

            // reset the Datum to enforce user interaction after clone (Datum is mandatory) 
            format.Figure = null;

            Session.CurrentSource.ExtractionSpec.Add( format );

            Formats.Add( myFormatViewModelFactory.Create( format ) );

            SelectedFormatIndex = Formats.Count - 1;
        }
    }
}
