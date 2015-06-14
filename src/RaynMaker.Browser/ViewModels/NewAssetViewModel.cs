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

namespace RaynMaker.Browser.ViewModels
{
    [Export]
    class NewAssetViewModel : AttributesBasedValidatableBindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;
        private string myName;
        private string myIsin;

        [ImportingConstructor]
        public NewAssetViewModel( IProjectHost host )
        {
            myProjectHost = host;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            ValidateProperties();
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

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();

            var company = new Company();
            company.Name = Name;

            var stock = new Stock();
            stock.Isin = Isin;

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
    }
}
