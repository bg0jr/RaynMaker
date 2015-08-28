using RaynMaker.Import;
using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.DatumLocators
{
    public partial class Bits
    {
        public class Navigations
        {
            public class Ariva
            {
                public static Navigation Fundamentals = new Navigation( DocumentType.Html,
                    NavigatorUrl.Request( @"http://www.ariva.de/search/search.m?searchname=${stock.isin}" ),
                    NavigatorUrl.Response( @"http://www.ariva.de/{(.*)}" ),
                    NavigatorUrl.Request( @"http://www.ariva.de/{0}/bilanz-guv" ) );

                public static Navigation TodayPrices = new Navigation( DocumentType.Html,
                    NavigatorUrl.Request( @"http://www.ariva.de/search/search.m?searchname=${stock.isin}" ),
                    NavigatorUrl.Response( @"http://www.ariva.de/{(.*)}" ),
                    NavigatorUrl.Request( @"http://www.ariva.de/{0}/kurs" ) );

                public static Navigation Overview = new Navigation( DocumentType.Html,
                    NavigatorUrl.Request( @"http://www.ariva.de/search/search.m?searchname=${stock.isin}" ) );

                // Xetra: boerse_id = 6 
                // Frankfurt: boerse_id = 1 
                // Nasdaq: boerse_id = 40 
                // NYSE: boerse_id = 21 
                public static Navigation HistoricalPrices = new Navigation( DocumentType.Text,
                    NavigatorUrl.Request( @"http://www.ariva.de/search/search.m?searchname=${stock.isin}" ),
                    NavigatorUrl.Response( @"http://www.ariva.de/{(.*)}" ),
                    NavigatorUrl.Request( @"http://www.ariva.de/{0}/historische_kurse" ),
                    NavigatorUrl.SubmitFormular( new Formular( "histcsv",
                        Tuple.Create( "boerse_id", "${ariva.stockexchange.id}" ),
                        Tuple.Create( "clean_split", "1" ),
                        Tuple.Create( "clean_bezug", "1" ),
                        Tuple.Create( "min_time", "1.1.1980" ),
                        Tuple.Create( "max_time", "${today.de}" ),
                        Tuple.Create( "trenner", ";" )
                        ) )
                    );
            }

            public class Yahoo
            {
                public static Navigation HistoricalPrices = new Navigation( DocumentType.Text,
                    @"http://ichart.yahoo.com/table.csv?s=${stock.symbol}.${stock.exchange.symbol}&ignore=.csv" );

                public static Navigation TodayPrices = new Navigation( DocumentType.Html,
                    @"http://de.finance.yahoo.com/q?s=${stock.isin}&m=a" );

                public static Navigation StocksExchangeList = new Navigation( DocumentType.Html,
                    @"http://de.finance.yahoo.com/lookup?s=${stock.isin}" );
            }
        }
    }
}
