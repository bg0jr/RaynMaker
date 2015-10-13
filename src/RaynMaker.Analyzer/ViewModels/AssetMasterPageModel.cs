using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Plainion;
using RaynMaker.Analyzer.Services;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Events;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Analyzer.ViewModels
{
    [Export]
    class AssetMasterPageModel : BindableBase, INavigationAware
    {
        private Stock myStock;
        private IAssetNavigation myNavigation;
        private IRegionManager myRegionManager;

        [ImportingConstructor]
        public AssetMasterPageModel( IAssetNavigation navigation, IRegionManager regionManager, IEventAggregator eventAggregator )
        {
            myNavigation = navigation;
            myRegionManager = regionManager;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            eventAggregator.GetEvent<AssetDeletedEvent>().Subscribe( OnStockDeleted );
        }

        private void OnStockDeleted( string guid )
        {
            if( myStock.Guid == guid )
            {
                myNavigation.ClosePage( this );
            }
        }

        public string Header
        {
            get { return myStock == null ? null : myStock.Company.Name; }
        }

        public bool IsNavigationTarget( NavigationContext navigationContext )
        {
            //var args = new AssetNavigationParameters( navigationContext.Parameters );
            //return myStock == args.Stock;
            return true;
        }

        public void OnNavigatedFrom( NavigationContext navigationContext )
        {
        }

        public void OnNavigatedTo( NavigationContext navigationContext )
        {
            var args = new AssetNavigationParameters( navigationContext.Parameters );

            myStock = args.Stock;
            OnPropertyChanged( () => Header );

            foreach( var contentPage in GetContentPages() )
            {
                contentPage.Initialize( args.Stock );
            }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            try
            {
                foreach( var contentPage in GetContentPages() )
                {
                    contentPage.Complete();
                }
            }
            catch( DbEntityValidationException ex )
            {
                var newEx = new InvalidOperationException( "Entity validation failed", ex );
                foreach( var result in ex.EntityValidationErrors )
                {
                    foreach( var error in result.ValidationErrors )
                    {
                        newEx.AddContext( result.Entry.Entity.GetType().Name + "." + error.PropertyName, error.ErrorMessage );
                    }
                }
                throw newEx;
            }

            myNavigation.ClosePage( this );
        }

        private IEnumerable<IContentPage> GetContentPages()
        {
            if( !myRegionManager.Regions.ContainsRegionWithName( RegionNames.AssetContentPages ) )
            {
                return Enumerable.Empty<IContentPage>();
            }

            var region = myRegionManager.Regions[ RegionNames.AssetContentPages ];

            return region.Views
                .OfType<FrameworkElement>()
                .Select( view => view.DataContext )
                .OfType<IContentPage>();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            foreach( var contentPage in GetContentPages() )
            {
                contentPage.Cancel();
            }

            myNavigation.ClosePage( this );
        }
    }
}
