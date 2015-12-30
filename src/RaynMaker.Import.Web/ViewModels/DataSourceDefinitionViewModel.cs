using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataSourceDefinitionViewModel : SpecDefinitionViewModelBase
    {
        public DataSourceDefinitionViewModel( Session session )
            : base( session )
        {
            AddCommand = new DelegateCommand( OnAdd );
            RemoveCommand = new DelegateCommand( OnRemove );
        }

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
            var source = new DataSource();
            source.LocatingSpec = new DocumentLocator();

            Session.Sources.Add( source );

            Session.CurrentSource = source;
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
            var source = Session.CurrentSource;
            Session.Sources.Remove( source );
            Session.CurrentSource = Session.Sources.FirstOrDefault();
        }
    }
}
