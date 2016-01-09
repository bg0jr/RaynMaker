using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
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
            set { SetProperty( ref myCurrentFigureDescriptor, value ); }
        }

        public void Reset()
        {
            CurrentFigureDescriptor = null;
            CurrentSource = null;

            Sources.Clear();
        }
    }
}
