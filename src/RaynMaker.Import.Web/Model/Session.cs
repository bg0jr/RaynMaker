using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.Model
{
    class Session : BindableBase
    {
        private IList<DatumLocator> myLocators;
        private DatumLocator myCurrentLocator;
        private Site myCurrentSite;
        private PathSeriesFormat myCurrentFormat;

        public Session()
        {
            myLocators = new List<DatumLocator>();
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

        public IEnumerable<DatumLocator> Locators { get { return myLocators; } }

        public void AddLocator( DatumLocator locator )
        {
            myLocators.Add( locator );
        }

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
            
            myLocators.Clear();
        }
    }
}
