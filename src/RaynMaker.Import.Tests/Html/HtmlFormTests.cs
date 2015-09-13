using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Parsers.Html;

namespace RaynMaker.Import.Tests.Html
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlFormTests : TestBase
    {
        [Test]
        public void CreateSubmitUrl_EmptyFormular()
        {
            var doc = LoadDocument( "ariva.historicalprices.DE0008404005.html" );
            var form = doc.GetFormByName( "histcsv" );

            var submitUrl = form.CreateSubmitUrl();
            Assert.That( submitUrl.ToString(), Is.EqualTo( "file:///quote/historic/historic.csv?secu=292&boerse_id=6&clean_split=1&clean_payout=0&clean_bezug=0&min_time=25.2.2011&max_time=25.2.2012&trenner=%3b" ) );
        }

        [Test]
        public void CreateSubmitUrl_FilledFormular()
        {
            var doc = LoadDocument( "ariva.historicalprices.DE0008404005.html" );
            var form = doc.GetFormByName( "histcsv" );

            var formular = new Formular( "histcsv",
                Tuple.Create( "boerse_id", "1" ),
                Tuple.Create( "min_time", "1.1.1980" ),
                Tuple.Create( "max_time", "3.3.2012" )
                );

            var submitUrl = form.CreateSubmitUrl( formular );

            Assert.That( submitUrl.ToString(), Is.EqualTo( "file:///quote/historic/historic.csv?secu=292&boerse_id=1&clean_split=1&clean_payout=0&clean_bezug=0&min_time=1.1.1980&max_time=3.3.2012&trenner=%3b" ) );
        }
    }
}
