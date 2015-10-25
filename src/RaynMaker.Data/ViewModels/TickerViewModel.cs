using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Data.Services;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
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

            TickerEntries = new ObservableCollection<TickerEntry>();
            
            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            TickerEntries.Clear();

            var ctx = myProjectHost.Project.GetAssetsContext();
            foreach( var stock in ctx.Stocks )
            {
                TickerEntries.Add( new TickerEntry( stock ) );
            }
        }

        [Import( AllowDefault = true )]
        public IDataProvider DataProvider { get; set; }
        
        public ObservableCollection<TickerEntry> TickerEntries { get; private set; }

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
            return DataProvider != null;// && DataProvider.CanFetch( typeof( Price ) );
        }

        private void OnUpdateAll()
        {
        //    var today = DateTime.Today;

        //    var series = new ObservableCollection<IDatum>();
        //    WeakEventManager<ObservableCollection<IDatum>, NotifyCollectionChangedEventArgs>.AddHandler( series, "CollectionChanged", OnSeriesChanged );

        //    // fetch some more data because of weekends and public holidays
        //    // we will then take last one

        //    DataProvider.Fetch( Stock,
        //        typeof( Price ),
        //        series,
        //        new DayPeriod( today.Subtract( TimeSpan.FromDays( 7 ) ) ),
        //        new DayPeriod( today.AddDays( 1 ) ) );
        //}

        //private void OnSeriesChanged( object sender, NotifyCollectionChangedEventArgs e )
        //{
        //    // there might be no privider - check for null
        //    var price = ( Price )( ( IEnumerable<IDatum> )sender )
        //        .OrderByDescending( d => d.Period )
        //        .FirstOrDefault();

        //    if( price != null )
        //    {
        //        Price.Value = price.Value;
        //        Price.Currency = price.Currency;
        //        Price.Period = price.Period;
        //        Price.Source = price.Source;
        //    }
        }
    }
}
