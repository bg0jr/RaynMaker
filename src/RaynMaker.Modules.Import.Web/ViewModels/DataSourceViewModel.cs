using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourceViewModel : SpecDefinitionViewModelBase
    {
        private bool myIsSelected;
        private IEnumerable<FigureViewModel> myFigures;

        public DataSourceViewModel( Session session, DataSource model )
            : base( session )
        {
            Model = model;

            CollectionChangedEventManager.AddHandler( Model.Figures, OnFiguresChanged );
            OnFiguresChanged( null, null );
        }

        private void OnFiguresChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Figures = Model.Figures
                .Select( f => new FigureViewModel( Session, this, f ) )
                .ToList();
        }

        public IEnumerable<FigureViewModel> Figures
        {
            get { return myFigures; }
            private set { SetProperty( ref myFigures, value ); }
        }

        public DataSource Model { get; private set; }

        public bool IsSelected
        {
            get { return myIsSelected; }
            set
            {
                if ( SetProperty( ref myIsSelected, value ) )
                {
                    Session.CurrentSource = Model;
                }
            }
        }
    }
}
