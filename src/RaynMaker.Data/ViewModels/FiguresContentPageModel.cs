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
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class FiguresContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private List<IDatumSeries> myDatums;
        private Stock myStock;

        [ImportingConstructor]
        public FiguresContentPageModel( IProjectHost projectHost, ILutService lutService )
        {
            myProjectHost = projectHost;
            CurrenciesLut = lutService.CurrenciesLut;

            ImportCommand = new DelegateCommand<DatumSeries>( OnImport, CanImport );
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

            myDatums = new List<IDatumSeries>();
            foreach( var datumType in Dynamics.AllDatums )
            {
                var series = Dynamics.GetDatumSeries( stock, datumType );
                ( ( DatumSeries )series ).EnableCurrencyCheck = false;
                myDatums.Add( series );
            }

            // data sanity - TODO: later move to creation of new DataSheet
            Currency defaultCurrency = null;
            {
                var series = ( DatumSeries )myDatums.SeriesOf( typeof( Price ) );
                if( series.Current<Price>() == null )
                {
                    series.Add( new Price() );
                }
                else
                {
                    defaultCurrency = series.Current<Price>().Currency;
                }
            }

            foreach( var type in Dynamics.AllDatums.Where( t => t != typeof( Price ) ) )
            {
                var series = ( DatumSeries )myDatums.SeriesOf( type );

                // TODO: today we only support yearly values here
                var currentYear = DateTime.Now.Year;
                var existingYears = series
                    .Select( v => ( ( YearPeriod )v.Period ).Year )
                    .ToList();
                // select hard coded 11 years here as minimum to allow growth calc based on recent 10 years
                for( int i = currentYear - 10; i <= currentYear; ++i )
                {
                    if( !existingYears.Contains( i ) )
                    {
                        var datum = Dynamics.CreateDatum( Stock, type, new YearPeriod( i ),
                            series.Currency != null ? series.Currency : defaultCurrency );
                        series.Add( datum );
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

            var datumsToValidate = myDatums
                .SelectMany( series => series )
                .OfType<AbstractDatum>()
                .Where( datum => datum.Id != 0 || datum.Value.HasValue );

            RecursiveValidator.Validate( datumsToValidate );

            // validate currency consistancy
            foreach( DatumSeries series in myDatums.ToList() )
            {
                series.EnableCurrencyCheck = true;
                series.VerifyCurrencyConsistency();
            }

            foreach( DatumSeries series in myDatums.ToList() )
            {
                var datums = ( IList )Dynamics.GetRelationship( Stock, series.DatumType );

                foreach( AbstractDatum datum in series )
                {
                    if( datum.Id == 0 && datum.Value.HasValue )
                    {
                        datums.Add( datum );
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
            get { return myDatums == null ? null : myDatums.SeriesOf( typeof( Price ) ).Current<Price>(); }
        }

        public IEnumerable<IDatumSeries> DataSeries
        {
            get
            {
                return myDatums == null ? null : myDatums
                    .Where( s => s.DatumType != typeof( Price ) )
                    .OrderBy( s => s.DatumType.Name );
            }
        }

        public DelegateCommand<DatumSeries> ImportCommand { get; private set; }

        private bool CanImport( DatumSeries series )
        {
            return DataProvider != null;
        }

        private void OnImport( DatumSeries series )
        {
            var currentYear = DateTime.Now.Year;

            var request = DataProviderRequest.Create( Stock, series.DatumType, currentYear - 10, currentYear );
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

            var series = new ObservableCollection<IDatum>();
            WeakEventManager<ObservableCollection<IDatum>, NotifyCollectionChangedEventArgs>.AddHandler( series, "CollectionChanged", OnSeriesChanged );

            // fetch some more data because of weekends and public holidays
            // we will then take last one

            DataProvider.Fetch( request, series );
        }

        private void OnSeriesChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            // there might be no privider - check for null
            var price = ( Price )( ( IEnumerable<IDatum> )sender )
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
