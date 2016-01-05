using System.Drawing;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.UnitTests.Design
{
    [RequiresSTA]
    class HtmlMarkupBehaviorTests
    {
        private SafeWebBrowser myBrowser;

        private const string HtmlDocument1 = @"
<html>
    <body>
        <table>
            <tr>
                <td id='x11'>vx 11</td>
                <td id='x12'>vx 12</td>
            </tr>
            <tr>
                <td id='x21'>vx 21</td>
                <td id='x22'>vx 22</td>
            </tr>
        </table>
    </body>
</html>
";

        private const string HtmlDocument2 = @"
<html>
    <body>
        <p id='p1'>Not a table</p>
    </body>
</html>
";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            myBrowser = new SafeWebBrowser();
            myBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            myBrowser.Dispose();
        }

        [Test]
        public void Ctor_WhenCalled_MarkerIsSet()
        {
            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );

            Assert.That( behavior.Marker, Is.Not.Null );
        }

        [Test]
        public void SelectedElement_WhenSet_WhenCalled_MarkerIsSet()
        {
            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );

            Assert.That( behavior.Marker, Is.Not.Null );
        }
    }
}
