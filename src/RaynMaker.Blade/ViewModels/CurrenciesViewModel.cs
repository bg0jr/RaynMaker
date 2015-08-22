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
using RaynMaker.Entities;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class CurrenciesViewModel : BindableBase, IInteractionRequestAware
    {
        private StorageService myStorageService;

        [ImportingConstructor]
        public CurrenciesViewModel( Project project, StorageService storageService )
        {
            Project = project;
            myStorageService = storageService;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            AddCurrencyCommand = new DelegateCommand( OnAddCurrency );
            RemoveCurrencyCommand = new DelegateCommand<Currency>( OnRemoveCurrency );

            AddTranslationCommand = new DelegateCommand<Currency>( OnAddTranslation );
            RemoveTranslationCommand = new DelegateCommand<Translation>( OnRemoveTranslation );
        }

        public Project Project { get; private set; }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public ICommand AddCurrencyCommand { get; private set; }

        private void OnAddCurrency()
        {
            Project.CurrenciesSheet.Currencies.Add( new Currency() );
        }

        public ICommand RemoveCurrencyCommand { get; private set; }

        private void OnRemoveCurrency( Currency currency )
        {
            Project.CurrenciesSheet.Currencies.Remove( currency );
        }

        public ICommand AddTranslationCommand { get; private set; }

        private void OnAddTranslation( Currency owner )
        {
            owner.Translations.Add( new Translation { Source = owner } );
        }

        public ICommand RemoveTranslationCommand { get; private set; }

        private void OnRemoveTranslation( Translation translation )
        {
            // just try to remove the translation from every currency - we will finally find the right owner.
            // not a nice approach but with current simplified design we cannot get owner currency directly so easy.
            foreach( var currency in Project.CurrenciesSheet.Currencies )
            {
                currency.Translations.Remove( translation );
            }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            myStorageService.SaveCurrencies( Project.CurrenciesSheet, Project.CurrenciesSheetLocation );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }
    }
}
