using System.IO;
using System.Threading;
using NUnit.Framework;
using Plainion;

namespace RaynMaker.Entities.Persistancy.Tests
{
    [TestFixture]
    class DatabaseTestsBase
    {
        protected DatabaseService Db { get; private set; }

        [SetUp]
        public void BaseSetUp()
        {
            var dbFile = Path.GetTempFileName() + ".rymdb";

            Contract.Requires( !File.Exists( dbFile ), "DB file already/still exists" );

            Db = new DatabaseService( dbFile );
            Db.Initialize();
        }

        [TearDown]
        public void BaseTearDown()
        {
            Db.Shutdown();

            // TODO: no way found until now to get rid of the file locks from SQlite :(
            // -> delete all files matching our file name pattern to at least do some partial cleanup

            foreach( var file in Directory.GetFiles( Path.GetTempPath(), "*.rymdb", SearchOption.TopDirectoryOnly ) )
            {
                try
                {
                    File.Delete( file );
                }
                catch( IOException ) { }
            }
        }
    }
}
