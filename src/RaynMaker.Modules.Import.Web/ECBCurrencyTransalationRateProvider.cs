using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;
using RaynMaker.Entities;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Web
{
    [Export(typeof(ICurrencyTranslationRateProvider))]
    class ECBCurrencyTransalationRateProvider : ICurrencyTranslationRateProvider
    {

        private Lazy<Dictionary<string, double>> myRatesFromEuroToTarget;

        public ECBCurrencyTransalationRateProvider()
            : this(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml")
        {
        }

        /// <summary>
        /// For UT only!
        /// </summary>
        internal ECBCurrencyTransalationRateProvider(string ratesDocumentUri)
        {
            myRatesFromEuroToTarget = new Lazy<Dictionary<string, double>>(() => LoadRates(ratesDocumentUri));
        }

        private Dictionary<string, double> LoadRates(string uri)
        {
            var root = XElement.Load(uri);

            var cube = root
                .Element(XName.Get("Cube", @"http://www.ecb.int/vocabulary/2002-08-01/eurofxref"))
                .Element(XName.Get("Cube", @"http://www.ecb.int/vocabulary/2002-08-01/eurofxref"));

            var rates = cube.Elements(XName.Get("Cube", @"http://www.ecb.int/vocabulary/2002-08-01/eurofxref"))
                .Select(e => new
                {
                    Symbol = e.Attribute("currency").Value,
                    Rate = e.Attribute("rate").Value
                });

            return rates.ToDictionary(x => x.Symbol, x => double.Parse(x.Rate, CultureInfo.InvariantCulture));
        }

        public double GetRate(Currency source, Currency target)
        {
            if (source.Symbol == "EUR")
            {
                return myRatesFromEuroToTarget.Value[target.Symbol];
            }

            var rateToEuro = 1 / myRatesFromEuroToTarget.Value[source.Symbol];
            var targetRate = target.Symbol == "EUR" ? 1 : myRatesFromEuroToTarget.Value[target.Symbol];

            return rateToEuro * targetRate;
        }
    }
}
