using System;
using System.Text.RegularExpressions;
using Blade.Data;
using RaynMaker.Import;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.DatumLocators
{
    public partial class Bits
    {
        public class Formats
        {
            public class Ariva
            {
                public static IFormat FundamentalSeries( string name, string path )
                {
                    return new PathSeriesFormat( name )
                    {
                        Path = path,
                        TimeAxisPosition = 1,
                        Expand = CellDimension.Row,
                        SeriesNamePosition = 0,
                        // seriesname-contains="unverwässertes Ergebnis pro Aktie">
                        ValueFormat = new FormatColumn( "value", typeof( double ), Format.PriceDE ),
                        TimeAxisFormat = new FormatColumn( "year", typeof( int ), Format.Number )
                    };
                }

                public static IFormat HistoricalPrices
                {
                    get
                    {
                        var format = Formats.Generic.CsvPrices( "Ariva.Prices", ";", "yyyy-MM-dd", Format.PriceDE );
                        format.SkipRows = new int[] { 0 };
                        return format;
                    }
                }

                public static IFormat CurrentPrice( string exchange )
                {
                    return new PathSeriesFormat( exchange + ".CurrentPrice" )
                        {
                            //Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[6]/DIV[0]/TABLE[0]/TBODY[0]",
                            Path = @"/BODY[0]/DIV[2]/DIV[1]/DIV[7]/DIV[0]/TABLE[0]/TBODY[0]",
                            Anchor = Anchor.ForCell( new StringContainsLocator( 0, exchange ), new StringContainsLocator( 0, "Letzter" ) ),
                            TimeAxisPosition = 0,
                            Expand = CellDimension.None,
                            SeriesNamePosition = 0,
                            ValueFormat = new FormatColumn( "close", typeof( double ), Format.PriceDE, new Regex( @"^([0-9,.]+)" ) )
                        };
                }
            }

            public class Yahoo
            {
                public static IFormat HistoricalPrices
                {
                    get
                    {
                        var format = Formats.Generic.CsvPrices( "Yahoo.Prices", ",", "yyyy-MM-dd", Format.PriceEN );
                        format.SkipRows = new int[] { 0 };     // table header
                        format.SkipColumns = new int[] { 6 };  // adj close
                        return format;
                    }
                }

                public static IFormat CurrentPrice( string exchangeSymbol )
                {
                    return new PathSeriesFormat( exchangeSymbol + ".CurrentPrice" )
                    {
                        Path = @"/BODY[0]/DIV[1]/DIV[0]/DIV[1]/DIV[0]/DIV[0]/DIV[0]/DIV[0]/DIV[0]/TABLE[0]",
                        Anchor = Anchor.ForCell( new StringContainsLocator( 0, "." + exchangeSymbol ), new StringContainsLocator( 0, "Letzter Kurs" ) ),
                        TimeAxisPosition = 0,
                        Expand = CellDimension.None,
                        SeriesNamePosition = 0,
                        ValueFormat = new FormatColumn( "close", typeof( double ), Format.PriceDE )
                    };
                }
            }

            public class Taipan
            {
                public static IFormat HistoricalPrices
                {
                    get
                    {
                        return Formats.Generic.CsvPrices( "TaiPan.Prices", ";", "dd.MM.yyyy", Format.PriceDE );
                    }
                }
            }

            public class Sheets
            {
                public static IFormat SeriesFromManualInput( string name )
                {
                    return new SeparatorSeriesFormat( name )
                        {
                            Separator = ";",
                            Anchor = Anchor.ForRow( new StringContainsLocator( 1, @"${stock.isin}" ) ),
                            TimeAxisPosition = 0,
                            Expand = CellDimension.Row,
                            SeriesNamePosition = 1,
                            SkipColumns = new int[] { 0, 2, 3 },
                            SkipRows = new[] { 1 },
                            ValueFormat = new FormatColumn( "value", typeof( double ), Format.PriceDE ),
                            TimeAxisFormat = new FormatColumn( "year", typeof( int ), Format.Number )
                        };
                }
            }

            public class Generic
            {
                public static CsvFormat CsvPrices( string name, string separator, string dateFormat, string contentFormat )
                {
                    return new CsvFormat( name, separator,
                        new FormatColumn( "date", typeof( DateTime ), dateFormat ),
                        new FormatColumn( "open", typeof( double ), contentFormat ),
                        new FormatColumn( "high", typeof( double ), contentFormat ),
                        new FormatColumn( "low", typeof( double ), contentFormat ),
                        new FormatColumn( "close", typeof( double ), contentFormat ),
                        new FormatColumn( "volume", typeof( int ), contentFormat ) );
                }
            }
        }
    }
}
