using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class CurrenciesViewModel : BindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;
        private ILutService myLutService;

        [ImportingConstructor]
        public CurrenciesViewModel( IProjectHost projectHost, ILutService lutService )
        {
            myProjectHost = projectHost;
            myLutService = lutService;

            UpdateAllCommand = new DelegateCommand( OnUpdateAll, CanUpdateAll );
            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            AddCurrencyCommand = new DelegateCommand( OnAddCurrency );
            RemoveCurrencyCommand = new DelegateCommand<CurrencyViewModel>( OnRemoveCurrency );

            AddTranslationCommand = new DelegateCommand<CurrencyViewModel>( OnAddTranslation );
            RemoveTranslationCommand = new DelegateCommand<Translation>( OnRemoveTranslation );

            myProjectHost.Changed += OnProjectChanged;
            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            OnPropertyChanged( PropertySupport.ExtractPropertyName( () => CurrenciesLut ) );
            OnPropertyChanged( PropertySupport.ExtractPropertyName( () => Currencies ) );
        }

        [Import( AllowDefault = true )]
        public ICurrencyTranslationRateProvider TranslationRateProvider { get; set; }

        public ICurrenciesLut CurrenciesLut { get { return myLutService.CurrenciesLut; } }

        public IEnumerable<CurrencyViewModel> Currencies
        {
            get
            {
                if( myLutService.CurrenciesLut == null )
                {
                    return null;
                }

                return myLutService.CurrenciesLut.Currencies
                    .Select( c => new CurrencyViewModel( c ) )
                    .ToList();
            }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public ICommand AddCurrencyCommand { get; private set; }

        private void OnAddCurrency()
        {
            myLutService.CurrenciesLut.Currencies.Add( new Currency() );
        }

        public ICommand RemoveCurrencyCommand { get; private set; }

        private void OnRemoveCurrency( CurrencyViewModel currency )
        {
            myLutService.CurrenciesLut.Currencies.Remove( currency.Model );
        }

        public ICommand AddTranslationCommand { get; private set; }

        private void OnAddTranslation( CurrencyViewModel owner )
        {
            owner.Model.Translations.Add( new Translation { Source = owner.Model } );
        }

        public ICommand RemoveTranslationCommand { get; private set; }

        private void OnRemoveTranslation( Translation translation )
        {
            // just try to remove the translation from every currency - we will finally find the right owner.
            // not a nice approach but with current simplified design we cannot get owner currency directly so easy.
            foreach( var currency in myLutService.CurrenciesLut.Currencies )
            {
                currency.Translations.Remove( translation );
            }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            myLutService.CurrenciesLut.Save();
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
            return TranslationRateProvider != null;
        }

        private void OnUpdateAll()
        {
            foreach( var currency in myLutService.CurrenciesLut.Currencies )
            {
                foreach( var translation in currency.Translations )
                {
                    translation.Rate = TranslationRateProvider.GetRate( currency, translation.Target );
                }
            }
        }
    }
}
