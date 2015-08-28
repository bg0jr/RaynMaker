using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Import.Core;
using RaynMaker.Import.DatumLocators;
using Blade.Collections;
using System.Data;

namespace RaynMaker.Import
{
    class Importer
    {
        private IDatumProviderFactory myFactory;

        public Importer()
        {
            myFactory = CreateDatumProviderFactory();
        }

        private IDatumProviderFactory CreateDatumProviderFactory()
        {
            var browser = DocumentBrowserFactory.Create();

            var factory = new DatumProviderFactory(
                browser,
                new ScopeLookupPolicy(),
                new ValidRangePolicy( DateTime.MinValue /*() => Context.Scope.TryFrom*/, DateTime.MaxValue /*() => Context.Scope.TryTo */ )
                );

            DatumLocatorDefinitions.Defines.Foreach( factory.LocatorRepository.Add );

            //var datumLocatorsRoot = Path.Combine( MauiEnvironment.Root, "DatumLocators" );
            //factory.LocatorRepository.Load( datumLocatorsRoot );

            return factory;
        }

        public SingleResultValue<T> FetchSingle<T>(  string isin, string datum )
        {
            var provider = myFactory.Create( datum );
            return provider.FetchSingle<T>();
        }

        public DataTable Fetch( string isin, string datum )
        {
            var provider = myFactory.Create( datum );
            return provider.Fetch().ResultTable;
        }
    }
}
