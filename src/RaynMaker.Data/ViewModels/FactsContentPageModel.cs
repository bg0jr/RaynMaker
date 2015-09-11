using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using Plainion.Validation;
using Plainion.Windows.Controls;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class FactsContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private List<IDatumSeries> myDatums;
        private Stock myStock;

        [ImportingConstructor]
        public FactsContentPageModel( IProjectHost projectHost, ILutService lutService )
        {
            myProjectHost = projectHost;
            CurrenciesLut = lutService.CurrenciesLut;

            AddReferenceCommand = new DelegateCommand( OnAddReference );
            RemoveReferenceCommand = new DelegateCommand<Reference>( OnRemoveReference );

            ImportCommand = new DelegateCommand<DatumSeries>( OnImport, CanImport );
        }

        [Import( AllowDefault = true )]
        public IDataProvider DataProvider { get; set; }

        public ICurrenciesLut CurrenciesLut { get; private set; }

        public string Header { get { return "Facts"; } }

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
                myDatums.Add( Dynamics.GetDatumSeries( stock, datumType ) );
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
                        var datum = ( AbstractDatum )Activator.CreateInstance( type );
                        datum.Period = new YearPeriod( i );

                        var currencyDatum = datum as AbstractCurrencyDatum;
                        if( currencyDatum != null )
                        {
                            if( series.Currency == null )
                            {
                                // this will also set series.Currency behind the scenes
                                currencyDatum.Currency = defaultCurrency;
                            }
                            else
                            {
                                currencyDatum.Currency = series.Currency;
                            }
                        }

                        var foreignKey = type.GetProperty( "Stock" );
                        if( foreignKey != null )
                        {
                            foreignKey.SetValue( datum, Stock );
                        }
                        else
                        {
                            foreignKey = type.GetProperty( "Company" );

                            Contract.Invariant( foreignKey != null, "ForeignKey detection failed" );

                            foreignKey.SetValue( datum, Stock.Company );
                        }

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

            // TODO: When to set timestamps? we could put it in the Entities now - whenever we change the value we update the timestamp
            // and handle deserialization separately
            // TODO: change "IFreezable" to "Validation" -  what is EF validation approach?

            var ctx = myProjectHost.Project.GetAssetsContext();

            var datumsToValidate = myDatums
                .SelectMany( series => series )
                .OfType<AbstractDatum>()
                .Where( datum => datum.Id != 0 || datum.Value.HasValue );

            RecursiveValidator.Validate( datumsToValidate );

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

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            Stock.Company.References.Add( new Reference() );
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference( Reference reference )
        {
            Stock.Company.References.Remove( reference );
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
            var values = DataProvider.Get( Stock, series.DatumType, new YearPeriod( currentYear - 10 ), new YearPeriod( currentYear ) );
        }
    }
}
