using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Events;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Events;

namespace RaynMaker.Modules.Browser.ViewModels
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private IProjectHost myProjectHost;
        private IAssetsContext myContext;
        private IEventAggregator myEventAggregator;
        private Stock mySelectedAsset;
        private ICollectionView myAssetsView;
        private string myAssetsFilter;
        private bool myIsTagFilterOpen;
        private IEnumerable<TagFilter> myTagFilters;
        private IEnumerable<TagFilter> myCommittedTagFilters;

        [ImportingConstructor]
        public BrowserViewModel( IProjectHost host, IEventAggregator eventAggregator )
        {
            myProjectHost = host;
            myEventAggregator = eventAggregator;

            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();

            NewCommand = new DelegateCommand( OnNew );
            NewAssetRequest = new InteractionRequest<NewAssetNotification>();
            OpenAssetCommand = new DelegateCommand( OnOpenAsset );
            DeleteCommand = new DelegateCommand<Stock>( OnDelete );
            DeletionConfirmationRequest = new InteractionRequest<IConfirmation>();

            OpenTagFilterCommand = new DelegateCommand( OnOpenTagFilter );
            ApplyTagFilterCommand = new DelegateCommand( OnApplyTagFilter );
            CancelTagFilterCommand = new DelegateCommand( OnCancelTagFilter );
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            myContext = myProjectHost.Project.GetAssetsContext();

            myTagFilters = new[] { new TagFilter( TagFilter.Blank ) }
                .Concat( myContext.Tags
                    .ToList()
                    .Select( t => new TagFilter( t ) ) )
                .ToList();

            myAssetsView = null;
            Assets.Refresh();

            OnPropertyChanged( () => HasProject );
        }

        public string Header
        {
            get { return "Browser"; }
        }

        public bool HasProject { get { return myProjectHost.Project != null; } }

        public ICommand NewCommand { get; private set; }

        private void OnNew()
        {
            var notification = new NewAssetNotification();
            notification.Title = "New Asset";

            NewAssetRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    myAssetsView = null;
                    Assets.Refresh();

                    SelectedAsset = n.Result;
                    OnOpenAsset();
                }
            } );
        }

        public InteractionRequest<NewAssetNotification> NewAssetRequest { get; private set; }

        public ICollectionView Assets
        {
            get
            {
                if( myAssetsView == null && myContext != null )
                {
                    myAssetsView = CollectionViewSource.GetDefaultView( myContext.Stocks.ToList() );
                    myAssetsView.Filter = x => FilterAssets( ( Stock )x );
                    myAssetsView.SortDescriptions.Add( new SortDescription( "Company.Name", ListSortDirection.Ascending ) );

                    OnPropertyChanged( "Assets" );
                }
                return myAssetsView;
            }
        }

        private bool FilterAssets( Stock stock )
        {
            return MatchesNameIsinFilter( stock ) && MatchesTagFilter( stock );
        }

        private bool MatchesNameIsinFilter( Stock stock )
        {
            if( string.IsNullOrWhiteSpace( myAssetsFilter ) )
            {
                return true;
            }

            return stock.Company.Name.Contains( myAssetsFilter, StringComparison.OrdinalIgnoreCase )
                || stock.Isin.Contains( myAssetsFilter, StringComparison.OrdinalIgnoreCase );
        }

        private bool MatchesTagFilter( Stock stock )
        {
            if( !stock.Company.Tags.Any() && Tags.Single( f => f.Name == TagFilter.Blank ).IsChecked )
            {
                return true;
            }

            return stock.Company.Tags.Any( t => myTagFilters.Any( f => f.Name == t.Name && f.IsChecked ) );
        }

        public string AssetsFilter
        {
            get { return myAssetsFilter; }
            set
            {
                if( SetProperty( ref myAssetsFilter, value ) )
                {
                    Assets.Refresh();
                }
            }
        }

        public Stock SelectedAsset
        {
            get { return mySelectedAsset; }
            set { SetProperty( ref mySelectedAsset, value ); }
        }

        public ICommand DeleteCommand { get; private set; }

        private void OnDelete( Stock stock )
        {
            var notification = new Confirmation();
            notification.Title = "Confirmation";
            notification.Content = "Deletion cannot be undone. "
                + Environment.NewLine
                + Environment.NewLine
                + "Do you really want to delete this asset?";

            DeletionConfirmationRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    var ctx = myProjectHost.Project.GetAssetsContext();

                    var companyGuid = stock.Company.Guid;

                    if( stock.Company.Stocks.Count == 1 )
                    {
                        ctx.Companies.Remove( stock.Company );
                    }
                    else
                    {
                        ctx.Stocks.Remove( stock );
                    }

                    ctx.SaveChanges();

                    myAssetsView = null;
                    Assets.Refresh();

                    myEventAggregator.GetEvent<AssetDeletedEvent>().Publish( stock.Guid );

                    if( stock.Company == null )
                    {
                        myEventAggregator.GetEvent<CompanyDeletedEvent>().Publish( companyGuid );
                    }
                }
            } );
        }

        public InteractionRequest<IConfirmation> DeletionConfirmationRequest { get; private set; }

        public ICommand OpenAssetCommand { get; private set; }

        public void OnOpenAsset()
        {
            if( SelectedAsset != null )
            {
                myEventAggregator.GetEvent<AssetSelectedEvent>().Publish( SelectedAsset );
            }
        }

        public IEnumerable<TagFilter> Tags
        {
            get { return myTagFilters; }
        }

        public bool IsTagFilterOpen
        {
            get { return myIsTagFilterOpen; }
            set { SetProperty( ref myIsTagFilterOpen, value ); }
        }

        public ICommand OpenTagFilterCommand { get; private set; }

        private void OnOpenTagFilter()
        {
            myCommittedTagFilters = myTagFilters
                .Select( f => new TagFilter( f ) )
                .ToList();

            OnPropertyChanged( () => Tags );
        }

        public ICommand ApplyTagFilterCommand { get; private set; }

        private void OnApplyTagFilter()
        {
            Assets.Refresh();

            IsTagFilterOpen = false;

            OnPropertyChanged( () => IsFilteredByTags );
        }

        public ICommand CancelTagFilterCommand { get; private set; }

        private void OnCancelTagFilter()
        {
            myTagFilters = myCommittedTagFilters
                .Select( f => new TagFilter( f ) )
                .ToList();

            IsTagFilterOpen = false;
        }

        public bool IsFilteredByTags
        {
            get { return myTagFilters != null ? myTagFilters.Any( f => !f.IsChecked ) : false; }
        }
    }
}
