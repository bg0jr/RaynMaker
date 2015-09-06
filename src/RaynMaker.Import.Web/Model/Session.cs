using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.Model
{
    class Session : BindableBase
    {
        private IList<DatumLocator> myLocators;
        private DatumLocator myCurrentLocator;
        private Site myCurrentSite;

        public Session()
        {
            myLocators = new List<DatumLocator>();
        }

        public DatumLocator CurrentLocator
        {
            get { return myCurrentLocator; }
            set { SetProperty( ref myCurrentLocator, value ); }
        }

        public IEnumerable<DatumLocator> Locators { get { return myLocators; } }

        public void AddLocator( DatumLocator locator )
        {
            myLocators.Add( locator );
        }

        public Site CurrentSite
        {
            get { return myCurrentSite; }
            set { SetProperty( ref myCurrentSite, value ); }
        }

        public void Reset()
        {
            CurrentSite = null;
            CurrentLocator = null;
            
            myLocators.Clear();
        }
    }
}
