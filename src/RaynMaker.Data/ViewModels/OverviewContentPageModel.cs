using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Windows.Controls;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class OverviewContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private Stock myStock;

        [ImportingConstructor]
        public OverviewContentPageModel( IProjectHost projectHost, ILutService lutService )
        {
            myProjectHost = projectHost;

            AddReferenceCommand = new DelegateCommand( OnAddReference );
            RemoveReferenceCommand = new DelegateCommand<Reference>( OnRemoveReference );
        }

        public ICurrenciesLut CurrenciesLut { get; private set; }

        public string Header { get { return "Overview"; } }

        public Stock Stock
        {
            get { return myStock; }
            set { SetProperty( ref myStock, value ); }
        }

        public void Initialize( Stock stock )
        {
            Stock = stock;
        }

        public void Complete()
        {
            TextBoxBinding.ForceSourceUpdate();

            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();
        }

        public void Cancel()
        {
        }

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            Stock.Company.References.Add( new Reference() );
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference( Reference reference )
        {
            Stock.Company.References.Remove( reference );
        }
    }
}
