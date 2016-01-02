using System.IO;
using System.Linq;
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

                var sheet = serializer.ReadCompatible( stream );

                var dataSource = sheet.GetSources<DataSource>().First();

                // TODO: validate more
                Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                Assert.That( dataSource.Location.Fragments[ 0 ].UrlString, Is.EqualTo( "http://www.ariva.de/search/search.m?searchname=${Isin}" ) );
                Assert.That( dataSource.Figures[ 0 ].Figure, Is.EqualTo( "Dividend" ) );
            }
        }
    }
}
