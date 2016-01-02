using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec
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
                new Request( "http://test1.org" ),
                new Response( "http://test2.org" ) );
            dataSource.ExtractionSpec.Add( new CsvDescriptor( "dummy.csv", ";", new FormatColumn( "c1", typeof( double ), "0.00" ) ) );

            var clone = FigureDescriptorFactory.Clone( dataSource );

            Assert.That( clone.Vendor, Is.EqualTo( "vendor" ) );
            Assert.That( clone.Name, Is.EqualTo( "name" ) );
            Assert.That( clone.Quality, Is.EqualTo( 17 ) );
            Assert.That( clone.DocumentType, Is.EqualTo( DocumentType.Html ) );

            Assert.That( clone.LocatingSpec.Fragments[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );

            Assert.That( clone.ExtractionSpec[ 0 ].Figure, Is.EqualTo( "dummy.csv" ) );
        }

        [Test]
        public void Clone_WhenCalled_FormatsCollectionOfCloneIsMutable()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.LocatingSpec = new DocumentLocator( new Request( "http://test1.org" ) );
            dataSource.ExtractionSpec.Add( new CsvDescriptor( "dummy.csv", ";", new FormatColumn( "c1", typeof( double ), "0.00" ) ) );

            var clone = FigureDescriptorFactory.Clone( dataSource );

            clone.ExtractionSpec.Add( new PathSeriesDescriptor( "dummy.series" ) );

            Assert.That( clone.ExtractionSpec[ 0 ].Figure, Is.EqualTo( "dummy.csv" ) );
            Assert.That( clone.ExtractionSpec[ 1 ].Figure, Is.EqualTo( "dummy.series" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.LocatingSpec = new DocumentLocator( new Request( "http://test1.org" ) );

            RecursiveValidator.Validate( dataSource );
        }

        [Test]
        public void Validate_InvalidVendor_Throws( [Values( null, "" )]string vendor )
        {
            var dataSource = new DataSource();
            dataSource.Vendor = vendor;
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.LocatingSpec = new DocumentLocator( new Request( "http://test1.org" ) );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( dataSource ) );
            Assert.That( ex.Message, Is.StringContaining( "The Vendor field is required" ) );
        }

        [Test]
        public void Validate_InvalidName_Throws( [Values( null, "" )]string name )
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = name;
            dataSource.DocumentType = DocumentType.Html;
            dataSource.LocatingSpec = new DocumentLocator( new Request( "http://test1.org" ) );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( dataSource ) );
            Assert.That( ex.Message, Is.StringContaining( "The Name field is required" ) );
        }

        [Test]
        public void Validate_DocumentTypeIsNone_Throws()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.None;
            dataSource.LocatingSpec = new DocumentLocator( new Request( "http://test1.org" ) );

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( dataSource ) );
            Assert.That( ex.Message, Is.StringContaining( "DocumentType must not be DocumentType.None" ) );
        }

        [Test]
        public void Validate_LocatingSpecMissing_Throws()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.LocatingSpec = null;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( dataSource ) );
            Assert.That( ex.Message, Is.StringContaining( "The LocatingSpec field is required" ) );
        }
    }
}
