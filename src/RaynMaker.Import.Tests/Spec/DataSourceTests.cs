using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Spec.v2.Locating;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Spec
{
    [TestFixture]
    public class DataSourceTests 
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.Quality = 17;
            dataSource.DocumentType = DocumentType.Html;
            dataSource.LocatingSpec = new DocumentLocator(
                new DocumentLocationFragment( UriType.Request, "http://test1.org" ),
                new DocumentLocationFragment( UriType.Response, "http://test2.org" ) );
            dataSource.ExtractionSpec.Add( new CsvDescriptor( "dummy.csv", ";" ) );

            var clone = FormatFactory.Clone( dataSource );

            Assert.That( clone.Vendor, Is.EqualTo( "vendor" ) );
            Assert.That( clone.Name, Is.EqualTo( "name" ) );
            Assert.That( clone.Quality, Is.EqualTo( 17 ) );
            Assert.That( clone.DocumentType, Is.EqualTo( DocumentType.Html ) );

            Assert.That( clone.LocatingSpec.Uris[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );

            Assert.That( clone.ExtractionSpec[ 0 ].Figure, Is.EqualTo( "dummy.csv" ) );
        }

        [Test]
        public void Clone_WhenCalled_FormatsCollectionOfCloneIsMutable()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.LocatingSpec = new DocumentLocator( new DocumentLocationFragment( UriType.Request, "http://test1.org" ) );
            dataSource.ExtractionSpec.Add( new CsvDescriptor( "dummy.csv", ";" ) );

            var clone = FormatFactory.Clone( dataSource );

            clone.ExtractionSpec.Add( new PathSeriesDescriptor( "dummy.series" ) );

            Assert.That( clone.ExtractionSpec[ 0 ].Figure, Is.EqualTo( "dummy.csv" ) );
            Assert.That( clone.ExtractionSpec[ 1 ].Figure, Is.EqualTo( "dummy.series" ) );
        }
    }
}
