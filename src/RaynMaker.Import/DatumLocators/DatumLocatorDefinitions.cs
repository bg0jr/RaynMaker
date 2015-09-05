using System.Collections.Generic;
using Blade;
using Blade.Collections;
using RaynMaker.Import;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.DatumLocators
{
    public static partial class DatumLocatorDefinitions
    {
        public static DatumLocator CurrentPrice = new DatumLocator( "current_price",
            // for US stocks - frankfurt is the better reference stock exchange
           new Site( "Ariva.F",
               Bits.Navigations.Ariva.TodayPrices,
               Bits.Formats.Ariva.CurrentPrice( "Frankfurt" ) ),
           new Site( "Ariva.DE",
               Bits.Navigations.Ariva.TodayPrices,
               Bits.Formats.Ariva.CurrentPrice( "Xetra" ) ),
           new Site( "Yahoo.DE",
               Bits.Navigations.Yahoo.TodayPrices,
               Bits.Formats.Yahoo.CurrentPrice( "DE" ) )
           );

        public static DatumLocator Eps = new DatumLocator( "eps",
            new Site( "Ariva",
                Bits.Navigations.Ariva.Fundamentals,
                Bits.Formats.Ariva.FundamentalSeries( "Ariva.Eps", @"/BODY[0]/DIV[5]/DIV[0]/DIV[3]/TABLE[7]/TBODY[0]/TR[5]/TD[1]" ) ),
            new Site( "Ariva",
                Bits.Navigations.Ariva.Fundamentals,
                Bits.Formats.Ariva.FundamentalSeries( "Ariva.Eps.Alt2", @"/BODY[0]/DIV[5]/DIV[0]/DIV[3]/TABLE[8]/TBODY[0]/TR[5]/TD[1]" ) ),
            new Site( "Sheet",
                new Navigation( DocumentType.Text, @"${EpsSheet}" ),
                Bits.Formats.Sheets.SeriesFromManualInput( "Sheet.Eps" ) )
            );

        public static DatumLocator Dividend = new DatumLocator( "dividend",
            new Site( "Ariva",
                Bits.Navigations.Ariva.Fundamentals,
                Bits.Formats.Ariva.FundamentalSeries( "Ariva.Dividend", @"/BODY[0]/DIV[5]/DIV[0]/DIV[3]/TABLE[7]/TBODY[0]/TR[3]/TD[1]" ) ),
            new Site( "Ariva",
                Bits.Navigations.Ariva.Fundamentals,
                Bits.Formats.Ariva.FundamentalSeries( "Ariva.Dividend.Alt2", @"/BODY[0]/DIV[5]/DIV[0]/DIV[3]/TABLE[8]/TBODY[0]/TR[3]/TD[1]" ) ),
            new Site( "Sheet",
                new Navigation( DocumentType.Text, @"${DividendSheet}" ),
                Bits.Formats.Sheets.SeriesFromManualInput( "Sheet.Dividend" ) )
            );

        public static DatumLocator StockPrice = new DatumLocator( "stock_price",
            new Site( "Ariva",
                Bits.Navigations.Ariva.HistoricalPrices,
                Bits.Formats.Ariva.HistoricalPrices ),
            new Site( "Yahoo.DE",
                Bits.Navigations.Yahoo.HistoricalPrices,
                Bits.Formats.Yahoo.HistoricalPrices ),
            new Site( "TaiPan",
                new Navigation( DocumentType.Text, @"${TaipanDir}/*/${stock.wpkn}.TXT" ),
                Bits.Formats.Taipan.HistoricalPrices )
            );

        public static IEnumerable<DatumLocator> Defines
        {
            get
            {
                return typeof( DatumLocatorDefinitions ).GetStaticMembers<DatumLocator>().Concat(
                    typeof( Standing ).GetStaticMembers<DatumLocator>() );
            }
        }
    }
}
