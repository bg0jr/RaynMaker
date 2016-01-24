using System.Windows.Input;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class FigureViewModel : SpecDefinitionViewModelBase
    {
        private DataSourceViewModel myParent;
        private bool myIsSelected;

        public FigureViewModel( Session session, DataSourceViewModel parent, IFigureDescriptor model )
            : base( session )
        {
            myParent = parent;
            Model = model;
        }

        public IFigureDescriptor Model { get; private set; }

        public bool IsSelected
        {
            get { return myIsSelected; }
            set
            {
                if ( SetProperty( ref myIsSelected, value ) )
                {
                    Session.CurrentSource = myParent.Model;
                    Session.CurrentFigureDescriptor = Model;
                }
            }
        }

        public ICommand RemoveCommand { get; private set; }
    }
}
