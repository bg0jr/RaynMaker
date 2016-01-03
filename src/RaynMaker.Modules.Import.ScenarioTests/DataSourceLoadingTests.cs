using System.IO;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
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

                    Assert.That( dataSource.Figures.Single(), Is.InstanceOf<PathSeriesDescriptor>() );
                    var descriptor = ( PathSeriesDescriptor )dataSource.Figures.Single();
                    Assert.That( descriptor.Excludes, Is.EquivalentTo( new int[] { 6 } ) );
                    Assert.That( descriptor.Figure, Is.EqualTo( "Dividend" ) );
                    Assert.That( descriptor.InMillions, Is.False );
                    Assert.That( descriptor.Orientation, Is.EqualTo( SeriesOrientation.Row ) );
                    Assert.That( descriptor.Path, Is.EqualTo( "/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/TABLE[3]/TBODY[0]/TR[9]/TD[1]" ) );
                    Assert.That( descriptor.TimeFormat.Type, Is.EqualTo( typeof( int ) ) );
                    Assert.That( descriptor.TimeFormat.Format, Is.EqualTo( "0000" ) );
                    Assert.That( ( ( AbsolutePositionLocator )descriptor.TimesLocator ).SeriesPosition, Is.EqualTo( 0 ) );
                    Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
                    Assert.That( descriptor.ValueFormat.Format, Is.EqualTo( "000.000,0000" ) );
                    Assert.That( ( ( StringContainsLocator )descriptor.ValuesLocator ).Pattern, Is.EqualTo( "Dividendenaussch" ) );
                }

                {
                    var dataSource = sheet.GetSources<DataSource>().Single( ds => ds.Name == "Prices" );

                    Assert.That( dataSource.Vendor, Is.EqualTo( "Ariva" ) );
                    Assert.That( dataSource.DocumentType, Is.EqualTo( DocumentType.Html ) );

                    Assert.That( dataSource.Location.Fragments[ 0 ], Is.InstanceOf<Request>() );
                    Assert.That( dataSource.Location.Fragments[ 0 ].UrlString, Is.EqualTo( "http://www.ariva.de/search/search.m?searchname=${Isin}" ) );
                    Assert.That( dataSource.Location.Fragments[ 1 ], Is.InstanceOf<Response>() );
                    Assert.That( dataSource.Location.Fragments[ 1 ].UrlString, Is.EqualTo( "http://www.ariva.de/{(.*)}?" ) );
                    Assert.That( dataSource.Location.Fragments[ 2 ], Is.InstanceOf<Request>() );
                    Assert.That( dataSource.Location.Fragments[ 2 ].UrlString, Is.EqualTo( "http://www.ariva.de/{0}/kurs" ) );

                    Assert.That( dataSource.Figures.Single(), Is.InstanceOf<PathCellDescriptor>() );
                    var descriptor = ( PathCellDescriptor )dataSource.Figures.Single();
                    Assert.That( ( ( StringContainsLocator )descriptor.Column ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
                    Assert.That( ( ( StringContainsLocator )descriptor.Column ).Pattern, Is.EqualTo( "Letzter" ) );
                    Assert.That( descriptor.Currency, Is.EquivalentTo( "EUR" ) );
                    Assert.That( descriptor.Figure, Is.EqualTo( "Price" ) );
                    Assert.That( descriptor.InMillions, Is.False );
                    Assert.That( descriptor.Path, Is.EqualTo( "/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]" ) );
                    Assert.That( ( ( StringContainsLocator )descriptor.Row ).HeaderSeriesPosition, Is.EqualTo( 0 ) );
                    Assert.That( ( ( StringContainsLocator )descriptor.Row ).Pattern, Is.EqualTo( "Frankfurt" ) );
                    Assert.That( descriptor.ValueFormat.Type, Is.EqualTo( typeof( double ) ) );
                    Assert.That( descriptor.ValueFormat.Format, Is.EqualTo( "000.000,0000" ) );
                }
            }
        }
    }
}
