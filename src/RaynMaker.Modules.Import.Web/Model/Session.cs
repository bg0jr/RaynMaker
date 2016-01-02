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
        private IFigureDescriptor myCurrentFormat;

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
                        CurrentFormat = myCurrentSource.Figures.FirstOrDefault();
                    }
                }
            }
        }

        public IFigureDescriptor CurrentFormat
        {
            get { return myCurrentFormat; }
            set { SetProperty( ref myCurrentFormat, value ); }
        }

        public void Reset()
        {
            CurrentFormat = null;
            CurrentSource = null;

            Sources.Clear();
        }

        // TODO: workaround to allow selection from validation because
        // HtmlMarker is stateful
        public Action ApplyCurrentFormat { get; set; }
    }
}
