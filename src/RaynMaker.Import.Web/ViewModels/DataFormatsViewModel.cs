using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataFormatsViewModel : BindableBase
    {
        private Session mySession;
        private int mySelectedFormatIndex;
        private HtmlDocument myDocument;
        private IBrowser myBrowser;

        public DataFormatsViewModel( Session session )
        {
            Contract.RequiresNotNull( session, "session" );

            mySession = session;

            PropertyChangedEventManager.AddHandler( mySession, OnCurrentSiteChanged, PropertySupport.ExtractPropertyName( () => mySession.CurrentSite ) );

            mySelectedFormatIndex = -1;

            Formats = new ObservableCollection<SingleFormatViewModel>();

            PreviousCommand = new DelegateCommand( OnPrevious, CanPrevious );
            NextCommand = new DelegateCommand( OnNext, CanNext );
            AddCommand = new DelegateCommand( OnAdd );
            RemoveCommand = new DelegateCommand( OnRemove );

            OnCurrentSiteChanged( null, null );
        }

        private void OnCurrentSiteChanged( object sender, PropertyChangedEventArgs e )
        {
            Formats.Clear();

            if( mySession.CurrentSite != null )
            {
                foreach( var format in mySession.CurrentSite.Formats )
                {
                    Formats.Add( new SingleFormatViewModel( ( PathSeriesFormat )format ) );
                }

                if( Formats.Count == 0 )
                {
                    OnAdd();
                }

                SelectedFormatIndex = 0;
            }
        }

        public IBrowser Browser
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

        private void BrowserDocumentCompleted( HtmlDocument doc )
        {
            Document = doc;
        }

        public ObservableCollection<SingleFormatViewModel> Formats { get; private set; }

        public int SelectedFormatIndex
        {
            get { return mySelectedFormatIndex; }
            set
            {
                if( SetProperty( ref mySelectedFormatIndex, value ) )
                {
                    if( mySelectedFormatIndex != -1 )
                    {
                        mySession.CurrentFormat = Formats[ mySelectedFormatIndex ].Format;
                        Formats[ mySelectedFormatIndex ].Document = myDocument;
                        Formats[ mySelectedFormatIndex ].Apply();
                    }
                }
            }
        }

        public HtmlDocument Document
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

            mySession.CurrentSite.Formats.Add( format );

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

            mySession.CurrentSite.Formats.Remove( formatVM.Format );
            Formats.Remove( formatVM );

            SelectedFormatIndex = Formats.Count - 1;

            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }
    }
}
