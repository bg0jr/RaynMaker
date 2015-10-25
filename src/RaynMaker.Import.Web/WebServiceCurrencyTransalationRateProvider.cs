using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using RaynMaker.Import.Web.CurrencyConverter;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web
{
    [Export( typeof( ICurrencyTranslationRateProvider ) )]
    class WebServiceCurrencyTransalationRateProvider : ICurrencyTranslationRateProvider
    {
        // http://www.webservicex.net/New/Home/ServiceDetail/10
        private CurrencyConvertorSoapClient myClient;

        public double GetRate( Entities.Currency source, Entities.Currency target )
        {
            if( myClient == null )
            {
                myClient = new CurrencyConvertorSoapClient( new BasicHttpBinding(), new EndpointAddress( "http://www.webservicex.net/CurrencyConvertor.asmx" ) );
            }

            return myClient.ConversionRate(
                ( CurrencyConverter.Currency )Enum.Parse( typeof( CurrencyConverter.Currency ), source.Symbol ),
                ( CurrencyConverter.Currency )Enum.Parse( typeof( CurrencyConverter.Currency ), target.Symbol ) );
        }
    }
}
