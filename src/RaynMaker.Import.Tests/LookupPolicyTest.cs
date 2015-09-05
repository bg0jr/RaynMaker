using System.Linq;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Providers
{
    [TestFixture]
    public class LookupPolicyTest : TestBase
    {
        [Test]
        public void TupleProperty()
        {
            var site = new Site( "Ariva",
                   new Navigation( DocumentType.Html, string.Empty ),
                   new SeparatorSeriesFormat( "Ariava.Prices" )
                       {
                           Anchor = Anchor.ForRow( new StringContainsLocator( 0, ">>${TableIndex}<<" ) )
                       });

            var fetchPolicy = new LookupPolicy();
            fetchPolicy.Lut[ "${TableIndex}" ] = "0";

            var format = ( SeparatorSeriesFormat )fetchPolicy.GetFormat( site.Formats.Single() );

            Assert.That( ( ( StringContainsLocator )format.Anchor.Row ).Pattern, Is.EqualTo( ">>0<<" ) );
        }
    }
}
