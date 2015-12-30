using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Spec.v2.Locating;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Spec
{
    [TestFixture]
    public class DataSourceTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.Quality = 17;
            dataSource.LocationSpec = new DocumentLocator( DocumentType.Html,
                new LocatingFragment( UriType.Request, "http://test1.org" ),
                new LocatingFragment( UriType.Response, "http://test2.org" ) );
            dataSource.FormatSpecs.Add( new CsvFormat( "dummy.csv", ";" ) );

            var clone = FormatFactory.Clone( dataSource );

            Assert.That( clone.Vendor, Is.EqualTo( "vendor" ) );
            Assert.That( clone.Name, Is.EqualTo( "name" ) );
            Assert.That( clone.Quality, Is.EqualTo( 17 ) );

            Assert.That( clone.LocationSpec.DocumentType, Is.EqualTo( DocumentType.Html ) );
            Assert.That( clone.LocationSpec.Uris[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );

            Assert.That( clone.FormatSpecs[ 0 ].Datum, Is.EqualTo( "dummy.csv" ) );
        }

        [Test]
        public void Clone_WhenCalled_FormatsCollectionOfCloneIsMutable()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.LocationSpec = new DocumentLocator( DocumentType.Html );
            dataSource.FormatSpecs.Add( new CsvFormat( "dummy.csv", ";" ) );

            var clone = FormatFactory.Clone( dataSource );

            clone.FormatSpecs.Add( new PathSeriesFormat( "dummy.series" ) );

            Assert.That( clone.FormatSpecs[ 0 ].Datum, Is.EqualTo( "dummy.csv" ) );
            Assert.That( clone.FormatSpecs[ 1 ].Datum, Is.EqualTo( "dummy.series" ) );
        }
    }
}
