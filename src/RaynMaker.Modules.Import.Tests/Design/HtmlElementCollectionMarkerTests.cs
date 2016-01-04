using System.Drawing;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    [TestFixture]
    class HtmlElementCollectionMarkerTests : HtmlMarkupTestBase
    {
        protected override string GetHtml()
        {
            return @"
<html>
    <body>
        <ul>
            <li id='a1'>bullet one</li>
            <li id='a2'>bullet two</li>
            <li id='a3'>bullet three</li>
        </ul>
    </body>
</html>
";
        }

        private HtmlElementCollectionMarker myMarker;

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlElementCollectionMarker( Color.Yellow );
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.Reset();
            myMarker = null;
        }

        [Test]
        public void Mark_MultipleElements()
        {
            var firstBullet = Document.GetElementById( "a1" );
            myMarker.Mark( firstBullet );

            var thirdBullet = Document.GetElementById( "a3" );
            myMarker.Mark( thirdBullet );

            Assert_IsMarked( firstBullet );
            Assert_IsMarked( thirdBullet );

            myMarker.Unmark();

            Assert_IsUnmarked( firstBullet, null );
            Assert_IsUnmarked( thirdBullet, null );
        }
    }
}
