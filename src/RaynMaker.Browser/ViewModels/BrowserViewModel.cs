using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using System.Linq;
using System;
using Microsoft.Practices.Prism.PubSubEvents;
using RaynMaker.Infrastructure.Events;

namespace RaynMaker.Browser.ViewModels
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private IProjectHost myProjectHost;
        private IAssetsContext myContext;
        private IEventAggregator myEventAggregator;
        private Stock mySelectedAsset;

        [ImportingConstructor]
        public BrowserViewModel( IProjectHost host, IEventAggregator eventAggregator )
        {
            myProjectHost = host;
            myEventAggregator = eventAggregator;

            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();

            NewCommand = new DelegateCommand( OnNew );
            NewAssetRequest = new InteractionRequest<IConfirmation>();
            OpenAssetCommand = new DelegateCommand( OnOpenAsset );
            DeleteCommand = new DelegateCommand<Stock>( OnDelete );
            DeletionConfirmationRequest = new InteractionRequest<IConfirmation>();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            myContext = myProjectHost.Project.GetAssetsContext();

            OnPropertyChanged( () => Assets );
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
            var notification = new Confirmation();
            notification.Title = "New Asset";

            NewAssetRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    OnPropertyChanged( () => Assets );
                }
            } );
        }

        public InteractionRequest<IConfirmation> NewAssetRequest { get; private set; }

        public IEnumerable<Stock> Assets
        {
            get { return myContext != null ? myContext.Stocks.ToList() : Enumerable.Empty<Stock>(); }
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
            notification.Content = "Deletion cannot be undone. " + Environment.NewLine
                + "Do you really want to delete this asset?";

            DeletionConfirmationRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    var ctx = myProjectHost.Project.GetAssetsContext();

                    // force references loaded into RAM
                    // workaround for cascading delete issue :(
                    var ignore = stock.Company.References.Count;

                    if( stock.Company.Stocks.Count == 1 )
                    {
                        ctx.Companies.Remove( stock.Company );
                    }
                    else
                    {
                        ctx.Stocks.Remove( stock );
                    }

                    ctx.SaveChanges();

                    OnPropertyChanged( () => Assets );
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
    }
}
