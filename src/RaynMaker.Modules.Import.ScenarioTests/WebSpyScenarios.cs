using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Moq;
using NUnit.Framework;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Services;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [RequiresSTA]
    [TestFixture]
    class WebSpyScenarios : TestBase
    {
        private Mock<ILutService> myLutService;
        private Mock<IProjectHost> myProjectHost;

        [SetUp]
        public void SetUp()
        {
            myLutService = new Mock<ILutService> { DefaultValue = DefaultValue.Mock };
            Mock.Get( myLutService.Object.CurrenciesLut ).SetupGet( x => x.Currencies ).Returns( new ObservableCollection<Entities.Currency>() );
            myLutService.Object.CurrenciesLut.Currencies.Add( new Entities.Currency { Symbol = "EUR" } );

            myProjectHost = new Mock<IProjectHost> { DefaultValue = DefaultValue.Mock };
            var storageRoot = Path.Combine( TestDataRoot, "DataSources", "WebSpyScenarios" );
            myProjectHost.Setup( x => x.Project.StorageRoot ).Returns( storageRoot );

            if( Directory.Exists( storageRoot ) )
            {
                Directory.Delete( storageRoot, true );
                Thread.Sleep( 250 );
            }

            Directory.CreateDirectory( storageRoot );
        }

        /// <summary>
        /// Figure definition we make separately - this test focuses on overall workflow
        /// </summary>
        [Test]
        public void AddDataSource()
        {
            var storageService = new StorageService( myProjectHost.Object );
            var webSpy = new WebSpyViewModel( myProjectHost.Object, storageService, myLutService.Object );
            var browser = new SafeWebBrowser();
            webSpy.Browser = browser;

            Assert.That( webSpy.Session.Sources, Is.Empty );

            Assert.That( webSpy.DataSourcesNavigation.AddDataSourceCommand.CanExecute( null ), Is.True );
            webSpy.DataSourcesNavigation.AddDataSourceCommand.Execute( null );

            var dataSource = webSpy.Session.CurrentSource;
            dataSource.Vendor = "DummyVendor";
            dataSource.Name = "DummyName";
            dataSource.Quality = 1;

            webSpy.DocumentLocation.CaptureCommand.Execute( null );
            Assert.That( webSpy.DocumentLocation.IsCapturing, Is.True );

            var documentLocation = new Uri( Path.Combine( TestDataRoot, "html", "ariva.prices.DE0007664039.html" ) );
            browser.Load( documentLocation );

            webSpy.DocumentLocation.CaptureCommand.Execute( null );
            Assert.That( webSpy.DocumentLocation.IsCapturing, Is.False );

            Assert.That( dataSource.Location.Fragments.Count, Is.EqualTo( 2 ) );
            Assert.That( dataSource.Location.Fragments.First().Url, Is.EqualTo( documentLocation ) );

            webSpy.DataSourcesNavigation.DescriptorSelectionRequest.Raised += DescriptorSelectionRequest_Raised;
            webSpy.DataSourcesNavigation.AddFigureCommand.Execute();
            webSpy.DataSourcesNavigation.DescriptorSelectionRequest.Raised -= DescriptorSelectionRequest_Raised;

            var figure = ( IPathDescriptor )webSpy.Session.CurrentFigureDescriptor;
            Assert.That( figure, Is.Not.Null );

            HtmlMarkupAutomationProvider.SimulateClickOn( browser.Document, "rym_FrakfurtPrice" );

            Assert.That( figure.Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );

            webSpy.SaveCommand.Execute( null );

            dataSource = storageService.Load().Single();

            Assert.That( dataSource.Vendor, Is.EqualTo( "DummyVendor" ) );
            Assert.That( dataSource.Name, Is.EqualTo( "DummyName" ) );
            Assert.That( dataSource.Location.Fragments.First().Url, Is.EqualTo( documentLocation ) );
            Assert.That( ( ( IPathDescriptor )dataSource.Figures.Single() ).Path, Is.EqualTo( @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]" ) );
        }

        private void DescriptorSelectionRequest_Raised( object sender, InteractionRequestedEventArgs e )
        {
            var notification = ( FigureDescriptorSelectionNotification )e.Context;
            notification.DescriptorType = typeof( PathCellDescriptor );
            notification.Confirmed = true;

            e.Callback();
        }
    }
}
