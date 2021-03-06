﻿using System;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Design
{
    [Apartment(ApartmentState.STA)]
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            myBrowser = new SafeWebBrowser();
            myBrowser.DownloadControlFlags = DocumentLoaderFactory.DownloadControlFlags;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
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
        public void AttachTo_SetToDocumentToWhichAnotherBehaviorIsAlreadyAttached_Throws()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior1 = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior1.AttachTo( document );

            var behavior2 = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Red ) );
            var ex = Assert.Throws<InvalidOperationException>( () => behavior2.AttachTo( document ) );
            Assert.That( ex.Message, Does.Contain( "Only one attached HtmlMarkupBehavior per HtmlDocument supported" ) );
        }

        [Test]
        public void Detach_WhenCalled_AnotherBehaviorCanBeAttached()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior1 = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior1.AttachTo( document );
            behavior1.Detach();

            var behavior2 = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Red ) );
            behavior2.AttachTo( document );

            Assert.That( behavior1.Document, Is.Null );
            Assert.That( behavior2.Document, Is.EqualTo( document ) );
        }

        /// <summary>
        /// Checking for "same document" with System.Windows.Forms.WebBrowser is not that easy as the WebBrowser control
        /// reuses HtmlDocument instances. So we currently consider every HtmlDocument to be a new one.
        /// </summary>
        [Test]
        public void AttachTo_WhenCalled_SelectedElementAndPathNulled()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.SelectedElement = ( HtmlElementAdapter )document.GetElementById( "x11" );

            myBrowser.LoadHtml( HtmlDocument2 );
            document = new HtmlDocumentAdapter( myBrowser.Document );
            behavior.AttachTo( document );

            Assert.That( behavior.SelectedElement, Is.Null );
            Assert.That( behavior.PathToSelectedElement, Is.Null );
        }

        [Test]
        public void Detach_WhenCalled_SelectedElementAndPathAndDocumentNulled()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.SelectedElement = ( HtmlElementAdapter )document.GetElementById( "x11" );

            behavior.Detach();

            Assert.That( behavior.Document, Is.Null );
            Assert.That( behavior.SelectedElement, Is.Null );
            Assert.That( behavior.PathToSelectedElement, Is.Null );
        }

        [Test]
        public void SelectedElement_NoDocumentAttached_Throws()
        {
            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );

            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );
            var ex = Assert.Throws<InvalidOperationException>( () => behavior.SelectedElement = ( HtmlElementAdapter )document.GetElementById( "x11" ) );
            Assert.That( ex.Message, Does.Contain( "Document not attached" ) );
        }

        [Test]
        public void PathToSelectedElement_NoDocumentAttached_PathSet()
        {
            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );

            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );
            var path = document.GetElementById( "x11" ).GetPath().ToString();

            behavior.PathToSelectedElement = path;

            Assert.That( behavior.PathToSelectedElement, Is.EqualTo( path ) );
        }

        [Test]
        public void SelectedElement_SetToNotNull_PathToSelectedElementAdjusted()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.SelectedElement = ( HtmlElementAdapter )document.GetElementById( "x11" );

            Assert.That( behavior.PathToSelectedElement, Is.EqualTo( document.GetElementById( "x11" ).GetPath().ToString() ) );
        }

        [Test]
        public void SelectedElement_SetToNull_PathToSelectedElementAdjusted()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.SelectedElement = null;

            Assert.That( behavior.PathToSelectedElement, Is.Null );
        }

        [Test]
        public void PathToSelectedElement_SetToNotNull_SelectedElementAdjusted()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.PathToSelectedElement = document.GetElementById( "x11" ).GetPath().ToString();

            Assert.That( behavior.SelectedElement, Is.SameAs( document.GetElementById( "x11" ) ) );
        }

        [Test]
        public void PathToSelectedElement_SetToNull_SelectedElementAdjusted()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            behavior.PathToSelectedElement = null;

            Assert.That( behavior.SelectedElement, Is.Null );
        }

        [Test]
        public void SelectedElement_WhenSet_SelectionChangedRaised()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            bool selectionChangedRaised = false;
            behavior.SelectionChanged += ( s, e ) => selectionChangedRaised = true;

            behavior.SelectedElement = ( HtmlElementAdapter )document.GetElementById( "x11" );

            Assert.That( selectionChangedRaised, Is.True );
        }

        [Test]
        public void PathToSelectedElement_WhenSet_SelectionChangedRaised()
        {
            myBrowser.LoadHtml( HtmlDocument1 );
            var document = new HtmlDocumentAdapter( myBrowser.Document );

            var behavior = new HtmlMarkupBehavior<HtmlElementMarker>( new HtmlElementMarker( Color.Yellow ) );
            behavior.AttachTo( document );

            bool selectionChangedRaised = false;
            behavior.SelectionChanged += ( s, e ) => selectionChangedRaised = true;

            behavior.PathToSelectedElement = document.GetElementById( "x11" ).GetPath().ToString(); ;

            Assert.That( selectionChangedRaised, Is.True );
        }
    }
}
