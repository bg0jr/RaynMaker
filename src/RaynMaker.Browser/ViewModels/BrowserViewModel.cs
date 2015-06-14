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

namespace RaynMaker.Browser.ViewModels
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private IProjectHost myProjectHost;
        private IAssetsContext myContext;

        [ImportingConstructor]
        public BrowserViewModel( IProjectHost host )
        {
            myProjectHost = host;

            myProjectHost.Changed += OnProjectChanged;

            NewCommand = new DelegateCommand( OnNew );
            NewAssetRequest = new InteractionRequest<IConfirmation>();

            DeleteCommand = new DelegateCommand<Stock>( OnDelete );
            DeletionConfirmationRequest = new InteractionRequest<IConfirmation>();
        }

        private void OnProjectChanged()
        {
            myContext = myProjectHost.Project.GetAssetsContext();

            OnPropertyChanged( () => Assets );
            OnPropertyChanged( () => HasProject );
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

                    ctx.Companies.Remove( stock.Company );
                    ctx.Stocks.Remove( stock );

                    ctx.SaveChanges();

                    OnPropertyChanged( () => Assets );
                }
            } );
        }

        public InteractionRequest<IConfirmation> DeletionConfirmationRequest { get; private set; }
    }
}
