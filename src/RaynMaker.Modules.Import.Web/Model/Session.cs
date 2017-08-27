using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.Model
{
    class Session : BindableBase
    {
        private DataSource myCurrentSource;
        private IFigureDescriptor myCurrentFigureDescriptor;

        public Session()
        {
            Sources = new ObservableCollection<DataSource>();
        }

        public ObservableCollection<DataSource> Sources { get; private set; }

        public DataSource CurrentSource
        {
            get { return myCurrentSource; }
            set
            {
                if( SetProperty( ref myCurrentSource, value ) )
                {
                    if( myCurrentSource != null )
                    {
                        CurrentFigureDescriptor = myCurrentSource.Figures.FirstOrDefault();
                    }
                }
            }
        }

        public IFigureDescriptor CurrentFigureDescriptor
        {
            get { return myCurrentFigureDescriptor; }
            set
            {
                if( myCurrentFigureDescriptor != value )
                {
                    if( value != null )
                    {
                        // adjust related source. Do it before actually changing the figure so that listeners to the change notifications 
                        // get updates of "parent" before getting updates of "child" (see DataSourcesNavigation)
                        CurrentSource = Sources.Single( s => s.Figures.Any( f => f == value ) );
                    }
                }

                SetProperty( ref myCurrentFigureDescriptor, value );
            }
        }

        public void Reset()
        {
            CurrentFigureDescriptor = null;
            CurrentSource = null;

            Sources.Clear();
        }
    }
}
