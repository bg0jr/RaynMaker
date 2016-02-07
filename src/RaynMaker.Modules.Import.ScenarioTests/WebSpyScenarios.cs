using System.Collections.ObjectModel;
using System.IO;
using Moq;
using NUnit.Framework;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Web.Services;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.ScenarioTests
{
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
            myProjectHost.Setup( x => x.Project.StorageRoot ).Returns( Path.Combine( TestDataRoot, "DataSources", "WebSpyScenarios" ) );
        }

        [Test]
        public void AddDataSource()
        {
            var webSpy = new WebSpyViewModel( myProjectHost.Object, new StorageService( myProjectHost.Object ), myLutService.Object );

            Assert.That( webSpy.Session.Sources, Is.Empty );

            Assert.That( webSpy.DataSourcesNavigation.AddDataSourceCommand.CanExecute( null ), Is.True );
            webSpy.DataSourcesNavigation.AddDataSourceCommand.Execute( null );


        }
    }
}
