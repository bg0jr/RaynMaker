using System.Drawing;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    [TestFixture]
    class HtmlTableMarkerTests : HtmlMarkupTestBase
    {
        protected override string GetHtml()
        {
            return @"
<html>
    <body>
        <table>
            <thead>
                <tr>
                    <th/>
                    <th>Column 1</th>
                    <th>Column 2</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>Row 1</th>
                    <td>c1</td>
                    <td>c2</td>
                </tr>
                <tr>
                    <th>Row 2</th>
                    <td id='a1'>c3</td>
                    <td>c4</td>
                </tr>
            </tbody>
        </table>
    </body>
</html>
";
        }

        private HtmlTableMarker myMarker;

        [SetUp]
        public void SetUp()
        {
            myMarker = new HtmlTableMarker( Color.Yellow, Color.SteelBlue );
        }

        [TearDown]
        public void TearDown()
        {
            myMarker.Reset();
            myMarker = null;
        }

        [Test]
        public void Mark_SingleCell()
        {
            var cell1= Document.GetElementById( "a1" );
            myMarker.Mark( cell1 );
            
            Assert_IsMarked( cell1 );

            ShowMarkedDocument();
            myMarker.Unmark();
            ShowMarkedDocument();

            Assert_IsUnmarked( cell1, null );
        }
    }
}
