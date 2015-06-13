using System;
using System.ComponentModel.Composition;
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
    class NewAssetViewModel : BindableBase, IInteractionRequestAware
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public NewAssetViewModel( IProjectHost host )
        {
            myProjectHost = host;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public string Name { get; set; }

        public string Isin { get; set; }

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

            Notification.TrySetConfirmed( true );
            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            Notification.TrySetConfirmed( false );
            FinishInteraction();
        }
    }
}
