using System.IO;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Web;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    class CurrencyTranslationRateProviderScenarios : TestBase
    {
        [Test]
        public void EurToUsd_FromECB()
        {
            var provider = new ECBCurrencyTransalationRateProvider(Path.Combine(TestDataRoot, "Xml", "eurofxref-daily.xml"));

            var rate = provider.GetRate( new Currency { Symbol = "EUR" }, new Currency { Symbol = "USD" } );

            Assert.That( rate, Is.EqualTo( 1.1275d ) );
        }

        [Test]
        public void UsdToEur_FromECB()
        {
            var provider = new ECBCurrencyTransalationRateProvider(Path.Combine(TestDataRoot, "Xml", "eurofxref-daily.xml"));

            var rate = provider.GetRate( new Currency { Symbol = "USD" }, new Currency { Symbol = "EUR" } );

            Assert.That( rate, Is.EqualTo( 1 / 1.1275 ) );
        }

        [Test]
        public void UsdToNok_FromECB()
        {
            var provider = new ECBCurrencyTransalationRateProvider(Path.Combine(TestDataRoot, "Xml", "eurofxref-daily.xml"));

            var rate = provider.GetRate( new Currency { Symbol = "USD" }, new Currency { Symbol = "NOK" } );

            Assert.That( rate, Is.EqualTo( 1 / 1.1275 * 9.6773 ) );
        }
    }
}
