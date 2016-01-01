using System;
using System.Globalization;
using System.IO;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    public class TestBase
    {
        private Lazy<IDocumentBrowser> myBrowser = null;

        public TestBase()
        {
            var assemblyPath = new Uri( Path.GetDirectoryName( GetType().Assembly.Location ) ).LocalPath;
            TestDataRoot = Path.Combine( assemblyPath, "TestData", GetType().Name );

            myBrowser = new Lazy<IDocumentBrowser>( () => DocumentProcessorsFactory.CreateBrowser() );
        }

        protected string TestDataRoot { get; private set; }

        protected string DumpSpec<T>( T obj )
        {
            using( var writer = new StringWriter() )
            {
                FormatFactory.Dump( writer, obj );
                return writer.ToString();
            }
        }

        protected T LoadDocument<T>( string name ) where T : IDocument
        {
            var file = Path.Combine( TestDataRoot, name );

            return myBrowser.Value.LoadDocument<T>( file );
        }
    }
}
