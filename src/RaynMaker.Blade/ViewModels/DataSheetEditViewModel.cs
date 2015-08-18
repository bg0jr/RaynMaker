﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
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
                price = new Price();
                myStock.Data.Add( new Series( price ) );
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
            Sheet.Freeze();

            myStorageService.SaveDataSheet( Sheet, myProject.CurrenciesSheetLocation );
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
            get { return Entities.Currencies.Sheet.Currencies; }
        }

        public Price Price
        {
            get { return myStock.SeriesOf( typeof( Price ) ).Current<Price>(); }
        }

    }
}
