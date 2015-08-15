using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.DataSheetSpec;
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

        [ImportingConstructor]
        public DataSheetEditViewModel( Project project, StorageService storageService )
        {
            myProject = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( myProject, OnProjectPropertyChanged,
                PropertySupport.ExtractPropertyName( () => myProject.CurrenciesSheetLocation ) );
            OnProjectPropertyChanged( null, null );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( string.IsNullOrEmpty( myProject.CurrenciesSheetLocation ) || !File.Exists( myProject.CurrenciesSheetLocation ) )
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
            myStorageService.SaveDataSheet( Sheet, myProject.CurrenciesSheetLocation );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }
    }
}
