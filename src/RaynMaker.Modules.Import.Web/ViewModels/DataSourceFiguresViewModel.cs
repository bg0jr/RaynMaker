using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Web.Model;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourceFiguresViewModel : SpecDefinitionViewModelBase
    {
        private int mySelectedDescriptorIndex;
        private IDocument myDocument;
        private IDocumentBrowser myBrowser;
        private FigureDescriptorViewModelFactory myDescriptorViewModelFactory;

        public DataSourceFiguresViewModel( Session session, ILutService lutService )
            : base( session )
        {
            myDescriptorViewModelFactory = new FigureDescriptorViewModelFactory( lutService );

            PropertyChangedEventManager.AddHandler( Session, OnCurrentSourceChanged, PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) );

            mySelectedDescriptorIndex = -1;

            Descriptors = new ObservableCollection<IDescriptorViewModel>();

            AddCommand = new DelegateCommand( OnAdd );
            DescriptorSelectionRequest = new InteractionRequest<FigureDescriptorSelectionNotification>();

            RemoveCommand = new DelegateCommand( OnRemove );
            CopyCommand = new DelegateCommand( OnCopy );

            OnCurrentSourceChanged( null, null );
        }

        private void OnCurrentSourceChanged( object sender, PropertyChangedEventArgs e )
        {
            SelectedDescriptorIndex = -1;

            foreach( var descriptor in Descriptors )
            {
                descriptor.Document = null;
            }
            Descriptors.Clear();

            if( Session.CurrentSource != null )
            {
                foreach( var descriptor in Session.CurrentSource.Figures )
                {
                    Descriptors.Add( myDescriptorViewModelFactory.Create( descriptor ) );
                }

                SelectedDescriptorIndex = Descriptors.Count == 0 ? -1 : 0;
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

        public ObservableCollection<IDescriptorViewModel> Descriptors { get; private set; }

        public int SelectedDescriptorIndex
        {
            get { return mySelectedDescriptorIndex; }
            set
            {
                if( value == Descriptors.Count )
                {
                    // this is caused by binding this property to TabControl
                    value = -1;
                }

                var oldDescriptor = mySelectedDescriptorIndex;
                if( SetProperty( ref mySelectedDescriptorIndex, value ) )
                {
                    if( oldDescriptor != -1 )
                    {
                        Descriptors[ oldDescriptor ].Unmark();
                        Descriptors[ oldDescriptor ].Document = null;
                    }

                    if( mySelectedDescriptorIndex != -1 )
                    {
                        Session.CurrentFigureDescriptor = Descriptors[ mySelectedDescriptorIndex ].Descriptor;
                        Descriptors[ mySelectedDescriptorIndex ].Document = myDocument;
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

                if( 0 <= mySelectedDescriptorIndex && mySelectedDescriptorIndex < Descriptors.Count )
                {
                    Descriptors[ mySelectedDescriptorIndex ].Document = myDocument;
                }
            }
        }

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
            var notification = new FigureDescriptorSelectionNotification();
            notification.Title = "Figure descriptor selection";

            DescriptorSelectionRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    var descriptor = FigureDescriptorFactory.Create( n.DescriptorType );

                    Session.CurrentSource.Figures.Add( descriptor );

                    Descriptors.Add( myDescriptorViewModelFactory.Create( descriptor ) );

                    SelectedDescriptorIndex = Descriptors.Count - 1;
                }
            } );
        }

        public InteractionRequest<FigureDescriptorSelectionNotification> DescriptorSelectionRequest { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
            if( mySelectedDescriptorIndex == -1 )
            {
                return;
            }

            var descriptorVM = Descriptors[ SelectedDescriptorIndex ];

            SelectedDescriptorIndex = Descriptors.Count - 2;

            Session.CurrentSource.Figures.Remove( descriptorVM.Descriptor );
            Descriptors.Remove( descriptorVM );
        }

        public ICommand CopyCommand { get; private set; }

        private void OnCopy()
        {
            if( mySelectedDescriptorIndex == -1 )
            {
                return;
            }

            var descriptor = FigureDescriptorFactory.Clone( Descriptors[ SelectedDescriptorIndex ].Descriptor );

            // reset the Figure to enforce user interaction after clone (Figure is mandatory) 
            descriptor.Figure = null;

            Session.CurrentSource.Figures.Add( descriptor );

            Descriptors.Add( myDescriptorViewModelFactory.Create( descriptor ) );

            SelectedDescriptorIndex = Descriptors.Count - 1;
        }
    }
}
