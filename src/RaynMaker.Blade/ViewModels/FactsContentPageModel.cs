﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class FactsContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private DataSheet myDataSheet;
        private Stock myStock;

        [ImportingConstructor]
        public FactsContentPageModel( IProjectHost projectHost, Project project, StorageService storageService )
        {
            myProjectHost = projectHost;
            Project = project;
            myStorageService = storageService;

            AddReferenceCommand = new DelegateCommand( OnAddReference );
            RemoveReferenceCommand = new DelegateCommand<Reference>( OnRemoveReference );
        }

        public Project Project { get; private set; }

        public string Header { get { return "Facts"; } }

        public Stock Stock
        {
            get { return myStock; }
            set { SetProperty( ref myStock, value ); }
        }

        public void Initialize( Stock stock )
        {
            Stock = stock;

            if( File.Exists( stock.Company.XdbPath ) )
            {
                myDataSheet = myStorageService.LoadDataSheet( stock );
            }
            else
            {
                myDataSheet = new DataSheet();
            }

            // data sanity - TODO: later move to creation of new DataSheet
            var price = myDataSheet.Data.SeriesOf( typeof( Price ) ).Current<Price>();
            if( price == null )
            {
                var series = new DatumSeries( typeof( Price ), new Price() );
                myDataSheet.Data.Add( series );
            }

            OnPropertyChanged( () => Price );

            foreach( var type in KnownDatums.AllExceptPrice )
            {
                var series = ( DatumSeries )myDataSheet.Data.SeriesOf( type );
                if( series == null )
                {
                    series = new DatumSeries( type );
                    myDataSheet.Data.Add( series );
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
                        var datum = ( AbstractDatum )Activator.CreateInstance( type );
                        datum.Period = new YearPeriod( i );

                        var currencyDatum = datum as AbstractCurrencyDatum;
                        if( currencyDatum != null )
                        {
                            currencyDatum.Currency = series.Currency;
                        }

                        series.Add( datum );
                    }
                }
            }
        }

        public void Complete()
        {
            // TODO: When to set timestamps? we could put it in the Entities now - whenever we change the value we update the timestamp
            // and handle deserialization separately
            // TODO: change "IFreezable" to "Validation" -  what is EF validation approach?

            foreach( DatumSeries series in myDataSheet.Data.ToList() )
            {
                foreach( var value in series.ToList() )
                {
                    if( !value.Value.HasValue )
                    {
                        series.Remove( value );
                    }
                }

                if( !series.Any() )
                {
                    myDataSheet.Data.Remove( series );
                }
            }

            myStorageService.SaveDataSheet( myStock, myDataSheet );
        }

        public void Cancel()
        {
        }

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            myStock.Company.References.Add( new Reference() );
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference( Reference reference )
        {
            myStock.Company.References.Remove( reference );
        }

        public Price Price
        {
            get { return myDataSheet.Data.SeriesOf( typeof( Price ) ).Current<Price>(); }
        }

        public IEnumerable<IDatumSeries> DataSeries
        {
            get
            {
                return myDataSheet.Data
                    .Where( s => s.DatumType != typeof( Price ) )
                    .OrderBy( s => s.DatumType.Name );
            }
        }
    }
}