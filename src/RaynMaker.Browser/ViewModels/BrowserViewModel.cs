﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using System.Linq;

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

            myProjectHost.Changing += OnProjectChanging;
            myProjectHost.Changed += OnProjectChanged;

            NewCommand = new DelegateCommand( OnNew );
            NewAssetRequest = new InteractionRequest<IConfirmation>();
        }

        private void OnProjectChanging()
        {
            if( myContext != null )
            {
                myContext.Dispose();
            }
        }

        private void OnProjectChanged()
        {
            myContext = myProjectHost.Project.CreateAssetsContext();

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
    }
}
