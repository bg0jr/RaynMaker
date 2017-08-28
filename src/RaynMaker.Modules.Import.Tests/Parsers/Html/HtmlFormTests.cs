using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents.AgilityPack;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.UnitTests.Html
{
    [TestFixture]
    public class HtmlFormTests
    {
        [Test]
        public void Ctor_NotAFormElement_Throws()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );
            var element = doc.GetElementById( "xx" );

            var ex = Assert.Throws<ArgumentException>( () => new HtmlForm( element ) );
            Assert.That( ex.Message, Does.Contain( "not a html form element" ) );
        }

        [Test]
        public void GetByName_NoFormWithThisName_ReturnsNull()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );

            var form = HtmlForm.GetByName( doc, "not-existing" );

            Assert.That( form, Is.Null );
        }

        [Test]
        public void GetByName_FormExists_FormFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><form name='xx' id='xx'/></body></html>" );

            var form = HtmlForm.GetByName( doc, "xx" );

            Assert.That( form.FormElement, Is.EqualTo( doc.GetElementById( "xx" ) ) );
        }

        [Test]
        public void GetByName_MultipleForms_CorrectFormFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div><p><form name='NO' id='NO'/></p><form name='xx' id='xx'/></div></body></html>" );

            var form = HtmlForm.GetByName( doc, "xx" );

            Assert.That( form.FormElement, Is.EqualTo( doc.GetElementById( "xx" ) ) );
        }
    }
}
