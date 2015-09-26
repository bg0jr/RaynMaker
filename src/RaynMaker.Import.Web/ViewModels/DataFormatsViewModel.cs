using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataFormatsViewModel : SpecDefinitionViewModelBase
    {
        private int mySelectedFormatIndex;
        private IDocument myDocument;
        private IDocumentBrowser myBrowser;

        public DataFormatsViewModel( Session session )
            : base( session )
        {
            Session.ApplyCurrentFormat = ApplyCurrentFormat;

            PropertyChangedEventManager.AddHandler( Session, OnCurrentSourceChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) );

            mySelectedFormatIndex = -1;

            Formats = new ObservableCollection<SingleFormatViewModel>();

            PreviousCommand = new DelegateCommand( OnPrevious, CanPrevious );
            NextCommand = new DelegateCommand( OnNext, CanNext );
            AddCommand = new DelegateCommand( OnAdd );
            RemoveCommand = new DelegateCommand( OnRemove );

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
                foreach( var format in Session.CurrentSource.FormatSpecs )
                {
                    Formats.Add( new SingleFormatViewModel( ( PathSeriesFormat )format ) );
                }

                if( Formats.Count == 0 )
                {
                    OnAdd();
                }
                else
                {
                    SelectedFormatIndex = 0;
                }
            }

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
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

        public ObservableCollection<SingleFormatViewModel> Formats { get; private set; }

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

        public DelegateCommand PreviousCommand { get; private set; }

        private bool CanPrevious() { return 0 < mySelectedFormatIndex; }

        private void OnPrevious()
        {
            SelectedFormatIndex--;

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand NextCommand { get; private set; }

        private bool CanNext() { return mySelectedFormatIndex < Formats.Count - 1; }

        private void OnNext()
        {
            SelectedFormatIndex++;

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
            var format = new PathSeriesFormat( string.Empty );
            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );

            Session.CurrentSource.FormatSpecs.Add( format );

            Formats.Add( new SingleFormatViewModel( format ) );

            SelectedFormatIndex = Formats.Count - 1;

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
            if( mySelectedFormatIndex == -1 )
            {
                return;
            }

            var formatVM = Formats[ mySelectedFormatIndex ];

            Session.CurrentSource.FormatSpecs.Remove( formatVM.Format );
            Formats.Remove( formatVM );

            SelectedFormatIndex = Formats.Count - 1;

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }
    }
}
