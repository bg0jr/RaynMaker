using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Validation;
using Plainion.Windows.Controls;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class FiguresContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private List<IFigureSeries> myFigures;
        private Stock myStock;

        [ImportingConstructor]
        public FiguresContentPageModel( IProjectHost projectHost, ILutService lutService )
        {
            myProjectHost = projectHost;
            CurrenciesLut = lutService.CurrenciesLut;

            ImportCommand = new DelegateCommand<FigureSeries>( OnImport, CanImport );
            ImportPriceCommand = new DelegateCommand( OnImportPrice, CanImportPrice );
        }

        [Import( AllowDefault = true )]
        public IDataProvider DataProvider { get; set; }

        public ICurrenciesLut CurrenciesLut { get; private set; }

        public string Header { get { return "Figures"; } }

        public Stock Stock
        {
            get { return myStock; }
            set { SetProperty( ref myStock, value ); }
        }

        public void Initialize( Stock stock )
        {
            Stock = stock;

            myFigures = new List<IFigureSeries>();
            foreach( var figureType in Dynamics.AllFigures.Where( t => t != typeof( Price ) ) )
            {
                myFigures.Add( Dynamics.GetSeries( stock, figureType, false ) );
            }

            // we might have prices of different currencies - do only add latest
            {
                var series = new FigureSeries( typeof( Price ) );
                series.EnableCurrencyCheck = false;

                var currentPrice = stock.Prices.OrderByDescending( p => p.Period ).FirstOrDefault();
                if( currentPrice != null )
                {
                    series.Add( currentPrice );
                }

                myFigures.Add( series );
            }

            // data sanity - TODO: later move to creation of new DataSheet
            {
                var series = ( FigureSeries )myFigures.SeriesOf( typeof( Price ) );
                if( series.Current<Price>() == null )
                {
                    series.Add( new Price() );
                }
            }

            // defaultCurrency could be taken from any figure but Price
            Currency defaultCurrency = null;
            foreach( var type in Dynamics.AllFigures.Where( t => t != typeof( Price ) ) )
            {
                var series = ( FigureSeries )myFigures.SeriesOf( type );

                // TODO: today we only support yearly values here
                var currentYear = DateTime.Now.Year;
                var existingYears = series
                    .Select( v => ( ( YearPeriod )v.Period ).Year )
                    .ToList();

                if( defaultCurrency == null )
                {
                    defaultCurrency = series.Currency;
                }

                // select hard coded 11 years here as minimum to allow growth calc based on recent 10 years
                for( int i = currentYear - 10; i <= currentYear; ++i )
                {
                    if( !existingYears.Contains( i ) )
                    {
                        var figure = Dynamics.CreateFigure( Stock, type, new YearPeriod( i ),
                            series.Currency != null ? series.Currency : defaultCurrency );
                        series.Add( figure );
                    }
                }
            }

            OnPropertyChanged( () => Price );
            OnPropertyChanged( () => DataSeries );
        }

        public void Complete()
        {
            TextBoxBinding.ForceSourceUpdate();

            var ctx = myProjectHost.Project.GetAssetsContext();

            // validate currency consistancy
            foreach( FigureSeries series in myFigures.ToList() )
            {
                // we need to remove those items which have no value because those might 
                // have "wrong" default currency
                foreach( var figure in series.OfType<AbstractFigure>().Where( d => !d.Value.HasValue ).ToList() )
                {
                    series.Remove( figure );

                    if( figure.Id != 0 )
                    {
                        // TODO: remove from stock/company relationship to remove it finally from DB
                        //Dynamics.Remove(
                    }
                }

                RecursiveValidator.Validate( series );

                series.EnableCurrencyCheck = true;
                series.VerifyCurrencyConsistency();
            }

            foreach( FigureSeries series in myFigures.ToList() )
            {
                var figures = ( IList )Dynamics.GetRelationship( Stock, series.FigureType );

                foreach( AbstractFigure figure in series )
                {
                    if( figure.Id == 0 && figure.Value.HasValue )
                    {
                        figures.Add( figure );
                    }
                }
            }

            ctx.SaveChanges();
        }

        public void Cancel()
        {
        }

        public Price Price
        {
            get { return myFigures == null ? null : myFigures.SeriesOf( typeof( Price ) ).Current<Price>(); }
        }

        public IEnumerable<IFigureSeries> DataSeries
        {
            get
            {
                return myFigures == null ? null : myFigures
                    .Where( s => s.FigureType != typeof( Price ) )
                    .OrderBy( s => s.FigureType.Name );
            }
        }

        public DelegateCommand<FigureSeries> ImportCommand { get; private set; }

        private bool CanImport( FigureSeries series )
        {
            return DataProvider != null;
        }

        private void OnImport( FigureSeries series )
        {
            var currentYear = DateTime.Now.Year;

            var request = DataProviderRequest.Create( Stock, series.FigureType, currentYear - 10, currentYear );
            request.WithPreview = true;

            DataProvider.Fetch( request, series );
        }

        public DelegateCommand ImportPriceCommand { get; private set; }

        private bool CanImportPrice()
        {
            return DataProvider != null;
        }

        private void OnImportPrice()
        {
            var today = DateTime.Today;

            var request = DataProviderRequest.Create( Stock, typeof( Price ), today.Subtract( TimeSpan.FromDays( 7 ) ), today.AddDays( 1 ) );
            request.WithPreview = true;

            var series = new ObservableCollection<IFigure>();
            CollectionChangedEventManager.AddHandler( series, OnSeriesChanged );

            // fetch some more data because of weekends and public holidays
            // we will then take last one

            DataProvider.Fetch( request, series );
        }

        private void OnSeriesChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            // there might be no privider - check for null
            var price = ( Price )( ( IEnumerable<IFigure> )sender )
                .OrderByDescending( d => d.Period )
                .FirstOrDefault();

            if( price != null )
            {
                Price.Value = price.Value;
                Price.Currency = price.Currency;
                Price.Period = price.Period;
                Price.Source = price.Source;
            }
        }
    }
}
