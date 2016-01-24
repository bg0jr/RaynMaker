using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourcesTreeViewModel : SpecDefinitionViewModelBase
    {
        private IEnumerable<Source> mySources;

        public DataSourcesTreeViewModel( Session session )
            : base( session )
        {
            CollectionChangedEventManager.AddHandler( Session.Sources, OnSourcesChanged );
            OnSourcesChanged( null, null );
        }

        private void OnSourcesChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Sources = Session.Sources
                .Select( s => new Source( Session, s ) )
                .ToList();
        }

        public IEnumerable<Source> Sources
        {
            get { return mySources; }
            private set { SetProperty( ref mySources, value ); }
        }

        internal class Source : SpecDefinitionViewModelBase
        {
            private bool myIsSelected;
            private IEnumerable<Figure> myFigures;

            public Source( Session session, DataSource model )
                : base( session )
            {
                Model = model;

                CollectionChangedEventManager.AddHandler( Model.Figures, OnFiguresChanged );
                OnFiguresChanged( null, null );
            }

            private void OnFiguresChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                Figures = Model.Figures
                    .Select( f => new Figure( Session, this, f ) )
                    .ToList();
            }

            public IEnumerable<Figure> Figures
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

        internal class Figure : SpecDefinitionViewModelBase
        {
            private Source myParent;
            private bool myIsSelected;

            public Figure( Session session, Source parent, IFigureDescriptor model )
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
        }
    }
}
