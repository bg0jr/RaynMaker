using System.Text.RegularExpressions;
using RaynMaker.Import;
using Blade.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.DatumLocators
{
    public static partial class DatumLocatorDefinitions
    {
        public class Standing
        {
            public static DatumLocator Wpkn = new DatumLocator( "Wpkn",
                new Site( "Ariva",
                    Bits.Navigations.Ariva.Overview,
                    new PathSingleValueFormat( "Ariva.Wpkn" )
                    {
                        Path = @"/BODY[0]/DIV[2]/DIV[1]/DIV[4]/DIV[2]/DIV[0]",                                 
                        ValueFormat = new ValueFormat( new Regex( @"WKN: ([\d\w\W]+)" ) )
                    },
                    Bits.Contents.FreeText )
                );

            public static DatumLocator StockSymbol = new DatumLocator( "Symbol",
                new Site("Ariva",
                    Bits.Navigations.Ariva.Overview,
                    new PathSingleValueFormat( "Ariva.Symbol" )
                    {
                        Path = @"/BODY[0]/DIV[2]/DIV[1]/DIV[4]/DIV[2]",
                        ValueFormat = new ValueFormat( new Regex( @"Symbol: (\w+)" ) )
                    },
                    Bits.Contents.FreeText ),
                new Site("Ariva.Ticker",
                    Bits.Navigations.Ariva.Fundamentals,
                    new PathSingleValueFormat("Ariva.Symbol")
                    {
                        Path = @"/BODY[0]/DIV[2]/DIV[1]/TABLE[1]/TBODY[0]/TR[0]/TD[1]",
                        ValueFormat = new ValueFormat(new Regex(@"(\w+)"))
                    },
                    Bits.Contents.FreeText),
                new Site("Yahoo",
                    Bits.Navigations.Yahoo.StocksExchangeList,
                    new PathSeriesFormat( "Yahoo.Symbol" )
                    {
                        Path = @"/BODY[0]/DIV[1]/DIV[0]/DIV[1]/DIV[0]/DIV[0]/DIV[0]/DIV[0]/DIV[0]/TABLE[0]",
                        Anchor = Anchor.ForCell(new RegexPatternLocator(0, new Regex(@"\.F$")), new AbsolutePositionLocator(0)),
                        TimeAxisPosition = 0,
                        Expand = CellDimension.None,
                        SeriesNamePosition = 0,
                        ValueFormat = new FormatColumn("symbol", typeof(string), string.Empty, new Regex(@"^(\w+)\."))
                    },
                    Bits.Contents.FreeText )
                );

            public static DatumLocator CompanyName = new DatumLocator( "CompanyName",
                new Site( "Ariva",
                    Bits.Navigations.Ariva.Overview,
                    new PathSingleValueFormat( "Ariva.CompanyName" )
                    {
                        Path = @"/BODY[0]/DIV[2]/DIV[1]/TABLE[0]/TBODY[0]/TR[0]/TD[0]/H1[0]",
                        ValueFormat = new ValueFormat( new Regex( @"^(.*)\s*(Aktie)?\s*$" ) )
                    },
                    Bits.Contents.FreeText )
                );

            public static DatumLocator Sector = new DatumLocator( "Sector",
                new Site( "Ariva",
                    Bits.Navigations.Ariva.Overview,
                    new PathSingleValueFormat( "Ariva.Sector" )
                    {
                        Path = @"/BODY[0]/DIV[2]/DIV[1]/DIV[7]/DIV[1]/TABLE[0]/TBODY[0]/TR[0]/TD[1]",
                        ValueFormat = new ValueFormat()
                    },
                    Bits.Contents.FreeText )
                );
        }
    }
}
