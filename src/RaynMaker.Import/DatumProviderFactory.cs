using RaynMaker.Import.Providers;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public class DatumProviderFactory : IDatumProviderFactory
    {
        private IDocumentBrowser myBrowser;

        public DatumProviderFactory( IDocumentBrowser browser, IFetchPolicy fetchPolicy )
            : this( browser, fetchPolicy, null )
        {
        }

        public DatumProviderFactory( IDocumentBrowser browser, IFetchPolicy fetchPolicy, IResultPolicy resultPolicy )
        {
            myBrowser = browser;
            FetchPolicy = fetchPolicy;
            ResultPolicy = resultPolicy;

            LocatorRepository = new DatumLocatorRepository();
        }

        public IFetchPolicy FetchPolicy { get; private set; }

        public IResultPolicy ResultPolicy { get; private set; }

        public IDatumProvider Create( DatumLocator datumLocator )
        {
            return new GenericDatumProvider( myBrowser, datumLocator, FetchPolicy, ResultPolicy );
        }

        public IDatumProvider Create( string datum )
        {
            var locator = LocatorRepository.Get( datum );
            if( locator == null )
            {
                return null;
            }

            return Create( locator );
        }

        public DatumLocatorRepository LocatorRepository { get; private set; }
    }
}
