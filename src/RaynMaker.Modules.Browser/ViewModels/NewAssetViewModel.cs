using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Modules.Browser.ViewModels
{
    [Export]
    class NewAssetViewModel : AttributesBasedValidatableBindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;
        private string myName;
        private string myIsin;
        private string myWpkn;
        private string mySymbol;

        [ImportingConstructor]
        public NewAssetViewModel( IProjectHost host )
        {
            myProjectHost = host;

            OkCommand = new DelegateCommand( OnOk, () => !HasErrors );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        [Required( ErrorMessage = "Company name is mandatory" )]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value != null ? value.Trim() : null ); }
        }

        [Required( ErrorMessage = "Isin is mandatory" )]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value != null ? value.Trim() : null ); }
        }

        public string Wpkn
        {
            get { return myWpkn; }
            set { SetProperty( ref myWpkn, value != null ? value.Trim() : null ); }
        }

        public string Symbol
        {
            get { return mySymbol; }
            set { SetProperty( ref mySymbol, value != null ? value.Trim() : null ); }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public DelegateCommand OkCommand { get; private set; }

        private void OnOk()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();

            var company = new Company();
            company.Name = Name;

            var stock = new Stock();
            stock.Isin = Isin;
            stock.Wpkn = Wpkn;
            stock.Symbol = Symbol;

            company.Stocks.Add( stock );

            ctx.Companies.Add( company );

            ctx.SaveChanges();

            ( ( NewAssetNotification )Notification ).Result = stock;

            Close( confirmed: true );
        }

        private void Close( bool confirmed )
        {
            Name = null;
            Isin = null;

            Notification.TrySetConfirmed( true );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            Close( confirmed: false );
        }

        protected override void OnErrorsChanged( System.ComponentModel.DataErrorsChangedEventArgs e )
        {
            base.OnErrorsChanged( e );

            OkCommand.RaiseCanExecuteChanged();
        }
    }
}
