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

                {
                    var dataSource = sheet.GetSources<DataSource>().Single( ds => ds.Name == "Fundamentals" );

                    Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                    Assert.That( dataSource.DocumentType, Is.EqualTo( DocumentType.Html ) );

                    Assert.That( dataSource.Location.Fragments[ 0 ], Is.InstanceOf<Request>() );
                    Assert.That( dataSource.Location.Fragments[ 0 ].UrlString, Is.EqualTo( "http://www.ariva.de/search/search.m?searchname=${Isin}" ) );
                    Assert.That( dataSource.Location.Fragments[ 1 ], Is.InstanceOf<Response>() );
                    Assert.That( dataSource.Location.Fragments[ 1 ].UrlString, Is.EqualTo( "http://www.ariva.de/{(.*)}?" ) );
                    Assert.That( dataSource.Location.Fragments[ 2 ], Is.InstanceOf<Request>() );
                    Assert.That( dataSource.Location.Fragments[ 2 ].UrlString, Is.EqualTo( "http://www.ariva.de/{0}/bilanz-guv" ) );
                }
                {
                    var dataSource = sheet.GetSources<DataSource>().Single( ds => ds.Name == "Prices" );

                    Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                }
            }
        }
    }
}
