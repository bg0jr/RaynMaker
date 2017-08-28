using System;
using System.IO;
using System.Linq;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    public class TestBase
    {
        private Lazy<IDocumentBrowser> myBrowser = null;

        public TestBase()
        {
            var assemblyPath = new Uri( Path.GetDirectoryName( GetType().Assembly.Location ) ).LocalPath;
            TestDataRoot = Path.Combine( assemblyPath, "TestData" );

            myBrowser = new Lazy<IDocumentBrowser>( () => DocumentProcessingFactory.CreateBrowser() );
        }

        protected string TestDataRoot { get; private set; }

        protected T LoadDocument<T>( params string[] paths ) where T : IDocument
        {
            var file = Path.Combine( new[] { TestDataRoot }.Concat( paths ).ToArray() );

            return myBrowser.Value.LoadDocument<T>( file );
        }
    }
}
