using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Xaml;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Model;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class CurrenciesViewModel : BindableBase, IInteractionRequestAware
    {
        private Project myProject;
        private CurrenciesSheet mySheet;

        [ImportingConstructor]
        public CurrenciesViewModel( Project project )
        {
            myProject = project;
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

            var reader = new ValidatingXamlReader();
            Sheet = reader.Read<CurrenciesSheet>( myProject.CurrenciesSheetLocation );
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public CurrenciesSheet Sheet
        {
            get { return mySheet; }
            set { SetProperty( ref mySheet, value ); }
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
            using( var writer = XmlWriter.Create( myProject.CurrenciesSheetLocation + ".db" ) )
            {
                var sheet = new SerializableCurrenciesSheet( Sheet );
                var serializer = new DataContractSerializer( typeof( SerializableCurrenciesSheet ) );
                serializer.WriteObject( writer, sheet );
            }

            FinishInteraction();
        }

        [DataContract( Name = "CurrenciesSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
        [KnownType( typeof( Currency ) )]
        public class SerializableCurrenciesSheet
        {
            public SerializableCurrenciesSheet( CurrenciesSheet sheet )
            {
                MaxAgeInDays = sheet.MaxAgeInDays;
                Currencies = new ObservableCollection<Currency>( sheet.Currencies );
            }

            [DataMember]
            public ObservableCollection<Currency> Currencies { get; private set; }

            [DataMember]
            public int MaxAgeInDays { get; set; }
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishInteraction();
        }
    }
}
