using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class CurrenciesViewModel : BindableBase, IInteractionRequestAware
    {
        private Project myProject;
        private StorageService myStorageService;

        [ImportingConstructor]
        public CurrenciesViewModel( Project project, StorageService storageService )
        {
            myProject = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( myProject, OnProjectPropertyChanged, PropertySupport.ExtractPropertyName( () => myProject.CurrenciesSheetLocation ) );
            OnProjectPropertyChanged( null, null );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            AddCurrencyCommand = new DelegateCommand( OnAddCurrency );
            RemoveCurrencyCommand = new DelegateCommand<Currency>( OnRemoveCurrency );

            AddTranslationCommand = new DelegateCommand<Currency>( OnAddTranslation );
            RemoveTranslationCommand = new DelegateCommand<Translation>( OnRemoveTranslation );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( string.IsNullOrEmpty( myProject.CurrenciesSheetLocation ) || !File.Exists( myProject.CurrenciesSheetLocation ) )
            {
                return;
            }

            if( Currencies.Sheet == null )
            {
                Currencies.Sheet = myStorageService.LoadCurrencies( myProject.CurrenciesSheetLocation );
            }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public CurrenciesSheet Sheet
        {
            get { return Currencies.Sheet; }
            set
            {
                if( Currencies.Sheet == value )
                {
                    return;
                }

                Currencies.Sheet = value;

                OnPropertyChanged( () => Sheet );
            }
        }

        public ICommand AddCurrencyCommand { get; private set; }

        private void OnAddCurrency()
        {
            Sheet.Currencies.Add( new Currency() );
        }

        public ICommand RemoveCurrencyCommand { get; private set; }

        private void OnRemoveCurrency( Currency currency )
        {
            Sheet.Currencies.Remove( currency );
        }

        public ICommand AddTranslationCommand { get; private set; }

        private void OnAddTranslation( Currency owner )
        {
            owner.Translations.Add( new Translation() );
        }

        public ICommand RemoveTranslationCommand { get; private set; }

        private void OnRemoveTranslation( Translation translation )
        {
            // just try to remove the translation from every currency - we will finally find the right owner.
            // not a nice approach but with current simplified design we cannot get owner currency directly so easy.
            foreach( var currency in Sheet.Currencies )
            {
                currency.Translations.Remove( translation );
            }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            myStorageService.SaveCurrencies( Sheet, myProject.CurrenciesSheetLocation );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }
    }
}
