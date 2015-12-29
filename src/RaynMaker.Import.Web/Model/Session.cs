using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.Model
{
    class Session : BindableBase
    {
        private DataSource myCurrentSource;
        private IFigureExtractionDescriptor myCurrentFormat;

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
                        CurrentFormat = myCurrentSource.FormatSpecs.FirstOrDefault();
                    }
                }
            }
        }

        public IFigureExtractionDescriptor CurrentFormat
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
