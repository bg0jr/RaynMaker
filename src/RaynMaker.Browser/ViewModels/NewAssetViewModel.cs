using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Browser.ViewModels
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

            OkCommand = new DelegateCommand( OnOk, ()=> !HasErrors );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        [Required( ErrorMessage = "Company name is mandatory" )]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [Required( ErrorMessage = "Isin is mandatory" )]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value ); }
        }

        public string Wpkn
        {
            get { return myWpkn; }
            set { SetProperty( ref myWpkn, value ); }
        }

        public string Symbol
        {
            get { return mySymbol; }
            set { SetProperty( ref mySymbol, value ); }
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
            stock.Symbol= Symbol;

            company.Stocks.Add( stock );

            ctx.Companies.Add( company );

            ctx.SaveChanges();

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
