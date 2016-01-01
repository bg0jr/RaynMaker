using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class HtmlFormSubmissionTests : TestBase
    {
        private IHtmlDocument myDocument = null;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            myDocument = LoadDocument<IHtmlDocument>( "ariva.historicalprices.DE0008404005.html" );
        }
        
        [Test]
        public void CreateSubmitUrl_EmptyFormular()
        {
            var form = myDocument.GetFormByName( "histcsv" );

            var submitUrl = form.CreateSubmitUrl();

            Assert.That( submitUrl.AbsoluteUri, Is.EqualTo( "file:///quote/historic/historic.csv?secu=292&boerse_id=6&clean_split=1&clean_payout=0&clean_bezug=0&min_time=25.2.2011&max_time=25.2.2012&trenner=%3b" ) );
        }

        [Test]
        public void CreateSubmitUrl_FilledFormular()
        {
            var form = myDocument.GetFormByName( "histcsv" );

            var formular = new Formular( "histcsv",
                Tuple.Create( "boerse_id", "1" ),
                Tuple.Create( "min_time", "1.1.1980" ),
                Tuple.Create( "max_time", "3.3.2012" )
                );

            var submitUrl = form.CreateSubmitUrl( formular );

            Assert.That( submitUrl.AbsoluteUri, Is.EqualTo( "file:///quote/historic/historic.csv?secu=292&boerse_id=1&clean_split=1&clean_payout=0&clean_bezug=0&min_time=1.1.1980&max_time=3.3.2012&trenner=%3b" ) );
        }

        [Test]
        public void GetFormByName()
        {
            var form = myDocument.GetFormByName( "histcsv" );

            Assert.That( form, Is.Not.Null );
            Assert.That( form.Name, Is.EqualTo( "histcsv" ).IgnoreCase );
        }
    }
}
