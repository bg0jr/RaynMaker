using System.IO;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    class DataSourceLoadingTests : TestBase
    {
        [Test]
        public void Load_ImportSpecVersion1_WithoutException()
        {
            using( var stream = new FileStream( Path.Combine( TestDataRoot, "DataSources.xdb" ), FileMode.Open, FileAccess.Read ) )
            {
                var serializer = new ImportSpecSerializer();

                var dataSource = serializer.Read<DataSource>( stream );

                Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                Assert.That( dataSource.Location.Fragments[ 0 ].UrlString, Is.EqualTo( "http://www.ariva.de/search/search.m?searchname=${Isin}" ) );
                Assert.That( dataSource.Figures[ 0 ].Figure, Is.EqualTo( "Dividend" ) );
            }
        }
    }
}
