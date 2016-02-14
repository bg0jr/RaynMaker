using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class TickerViewModel : BindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public TickerViewModel( IProjectHost projectHost )
        {
            myProjectHost = projectHost;

            UpdateAllCommand = new DelegateCommand( OnUpdateAll, CanUpdateAll );
            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            Entries = new ObservableCollection<TickerEntry>();

            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            Entries.Clear();

            var ctx = myProjectHost.Project.GetAssetsContext();
            foreach( var stock in ctx.Stocks )
            {
                Entries.Add( new TickerEntry( stock ) );
            }

            UpdateAllCommand.RaiseCanExecuteChanged();
        }

        [Import( AllowDefault = true )]
        public IDataProvider DataProvider { get; set; }

        public ObservableCollection<TickerEntry> Entries { get; private set; }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();

            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }

        public DelegateCommand UpdateAllCommand { get; private set; }

        private bool CanUpdateAll()
        {
            return myProjectHost.Project != null && DataProvider != null && DataProvider.CanFetch( typeof( Price ) );
        }

        private void OnUpdateAll()
        {
            var today = DateTime.Today;

            foreach( var entry in Entries )
            {
                var request = DataProviderRequest.Create( entry.Stock, typeof( Price ), today.Subtract( TimeSpan.FromDays( 7 ) ), today.AddDays( 1 ) );
                request.WithPreview = false;

                var series = new List<IFigure>();

                // fetch some more data because of weekends and public holidays
                // we will then take last one

                DataProvider.Fetch( request, series );

                entry.CurrentPrice = ( Price )series
                    .OrderByDescending( d => d.Period )
                    .First();

                // connect to stock so that we can save it later
                var existingPrice = entry.Stock.Prices.SingleOrDefault( p => p.Period.Equals( entry.CurrentPrice.Period ) );
                if( existingPrice != null )
                {
                    existingPrice.Value = entry.CurrentPrice.Value;
                    existingPrice.Currency = entry.CurrentPrice.Currency;
                    existingPrice.Source = entry.CurrentPrice.Source;
                }
                else
                {
                    entry.Stock.Prices.Add( entry.CurrentPrice );
                }
            }
        }
    }
}
