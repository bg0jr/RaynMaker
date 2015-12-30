using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Tests
{
    [TestFixture]
    class ImportSpecSerializerTests : TestBase
    {
        [Test]
        public void Read_WithReleasedDataSourceFormat_CanDeserialize()
        {
            using( var stream = new FileStream( Path.Combine( TestDataRoot, "DataSources.xdb" ), FileMode.Open, FileAccess.Read ) )
            {
                var serializer = new ImportSpecSerializer();
                var dataSource = serializer.Read<DataSource>( stream );

                Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                Assert.That( dataSource.LocatingSpec.DocumentType, Is.EqualTo( DocumentType.Html ) );
                Assert.That( dataSource.ExtractionSpec[ 0 ].Datum, Is.EqualTo( "Dividend" ) );
            }
        }
    }
}
