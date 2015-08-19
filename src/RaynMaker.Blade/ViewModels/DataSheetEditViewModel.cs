using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class DataSheetEditViewModel : BindableBase, IInteractionRequestAware
    {
        private Project myProject;
        private StorageService myStorageService;
        private DataSheet myDataSheet;
        private Stock myStock;

        [ImportingConstructor]
        public DataSheetEditViewModel( Project project, StorageService storageService )
        {
            myProject = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( myProject, OnProjectPropertyChanged,
                PropertySupport.ExtractPropertyName( () => myProject.DataSheetLocation ) );
            OnProjectPropertyChanged( null, null );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            AddReferenceCommand = new DelegateCommand( OnAddReference );
            RemoveReferenceCommand = new DelegateCommand<Reference>( OnRemoveReference );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( string.IsNullOrEmpty( myProject.DataSheetLocation ) || !File.Exists( myProject.DataSheetLocation ) )
            {
                return;
            }

            if( File.Exists( myProject.DataSheetLocation ) )
            {
                Sheet = myStorageService.LoadDataSheet( myProject.DataSheetLocation );
                Sheet.Freeze();
            }
            else
            {
                Sheet = new DataSheet
                {
                    Asset = new Stock
                    {
                        Overview = new Overview()
                    }
                };
            }

            myStock = ( Stock )Sheet.Asset;
            Contract.Invariant( myStock != null, "No stock found in DataSheet" );

            // data sanity - TODO: later move to creation of new DataSheet
            var price = myStock.SeriesOf( typeof( Price ) ).Current<Price>();
            if( price == null )
            {
                var series = new Series( new Price() );
                series.Freeze();
                myStock.Data.Add( series );
            }

            OnPropertyChanged( () => Price );

            foreach( var type in KnownDatums.AllExceptPrice )
            {
                var series = ( Series )myStock.SeriesOf( type );
                if( series == null )
                {
                    series = new Series();
                    myStock.Data.Add( series );
                }
                else
                {
                    series.Unfreeze();
                }

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
                        var datum = ( Datum )Activator.CreateInstance( type );
                        datum.Period = new YearPeriod( i );

                        var currencyDatum = datum as CurrencyDatum;
                        if( currencyDatum != null )
                        {
                            currencyDatum.Currency = series.Currency;
                        }

                        series.Values.Add( datum );
                    }
                }

                series.Freeze();
            }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public DataSheet Sheet
        {
            get { return myDataSheet; }
            set { SetProperty( ref myDataSheet, value ); }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            // TODO: When to set timestamps? we could put it in the Entities now - whenever we change the value we update the timestamp
            // and handle deserialization separately
            // TODO: change "IFreezable" to "Validation" -  what is EF validation approach?

            foreach( Series series in Sheet.Asset.Data.ToList() )
            {
                series.Unfreeze();
                foreach( var value in series.ToList() )
                {
                    if( !value.Value.HasValue )
                    {
                        series.Values.Remove( value );
                    }
                }

                if( series.Values.Any() )
                {
                    series.Freeze();
                }
                else
                {
                    Sheet.Asset.Data.Remove( series );
                }
            }

            myStorageService.SaveDataSheet( Sheet, myProject.DataSheetLocation );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            myStock.Overview.References.Add( new Reference() );
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference( Reference reference )
        {
            myStock.Overview.References.Remove( reference );
        }

        public ObservableCollection<Currency> Currencies
        {
            get { return Entities.Currencies.Sheet != null ? Entities.Currencies.Sheet.Currencies : null; }
        }

        public Price Price
        {
            get { return myStock.SeriesOf( typeof( Price ) ).Current<Price>(); }
        }

        public IEnumerable<IDatumSeries> DataSeries
        {
            get
            {
                return myStock.Data
                    .Where( s => s.DatumType != typeof( Price ) )
                    .OrderBy( s => s.DatumType.Name );
            }
        }
    }
}
