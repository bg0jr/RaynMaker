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
        private DatumLocator myCurrentLocator;
        private Site myCurrentSite;
        private PathSeriesFormat myCurrentFormat;

        public Session()
        {
            Locators = new ObservableCollection<DatumLocator>();
        }

        public DatumLocator CurrentLocator
        {
            get { return myCurrentLocator; }
            set
            {
                if( SetProperty( ref myCurrentLocator, value ) )
                {
                    if( myCurrentLocator != null )
                    {
                        CurrentSite = myCurrentLocator.Sites.FirstOrDefault();
                    }
                }
            }
        }

        public ObservableCollection<DatumLocator> Locators { get; private set; }

        public Site CurrentSite
        {
            get { return myCurrentSite; }
            set
            {
                if( SetProperty( ref myCurrentSite, value ) )
                {
                    if( myCurrentSite != null )
                    {
                        CurrentFormat = (PathSeriesFormat)myCurrentSite.Formats.FirstOrDefault();
                    }
                }
            }
        }

        public PathSeriesFormat CurrentFormat
        {
            get { return myCurrentFormat; }
            set { SetProperty( ref myCurrentFormat, value ); }
        }

        public void Reset()
        {
            CurrentFormat = null;
            CurrentSite = null;
            CurrentLocator = null;

            Locators.Clear();
        }

        // TODO: workaround to allow selection from validation because
        // HtmlMarker is stateful
        public Action ApplyCurrentFormat { get; set; }
    }
}
