using System;
using System.Globalization;
using System.IO;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.ScenarioTests
{
    public class TestBase
    {
        private Lazy<IDocumentBrowser> myBrowser = null;

        public TestBase()
        {
            var assemblyPath = new Uri( Path.GetDirectoryName( GetType().Assembly.CodeBase ) ).LocalPath;
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

        protected IDocument LoadDocument( DocumentType docType, string name )
        {
            var file = Path.Combine( TestDataRoot, name );

            var navi = new DocumentLocator( new DocumentLocationFragment( UriType.Request, file ) );
            myBrowser.Value.Navigate( docType, navi );
            
            return myBrowser.Value.Document;
        }

        protected IHtmlDocument LoadHtmlDocument( string name )
        {
            return ( ( HtmlDocumentHandle )LoadDocument( DocumentType.Html, name ) ).Content;
        }
    }
}
