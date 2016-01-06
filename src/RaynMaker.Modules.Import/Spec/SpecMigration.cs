using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Modules.Import.Spec;

namespace RaynMaker.Modules.Import.Spec
{
    class SpecMigration
    {
        public static DataSourcesSheet Migrate( DataSourcesSheet sheet )
        {
            var dataSources = sheet.GetSources<object>()
                .Select( ds => ds.GetType() == typeof( v1.DataSource ) ? Migrate( ( v1.DataSource )ds ) : ( v2.DataSource )ds )
                .ToList();

            // return a new sheet - we dont want to have the "old" datasources still in the sheet
            var result = new DataSourcesSheet();
            result.SetSources( dataSources );

            return result;
        }

        private static v2.DataSource Migrate( v1.DataSource source )
        {
            var target = new v2.DataSource();
            target.Vendor = source.Vendor;
            target.Name = source.Name;
            target.Quality = source.Quality;
            target.DocumentType = Migrate( source.LocationSpec.DocumentType );
            target.Location = Migrate( source.LocationSpec );

            foreach( var format in source.FormatSpecs )
            {
                target.Figures.Add( Migrate( format ) );
            }

            return target;
        }

        private static v2.DocumentType Migrate( v1.DocumentType documentType )
        {
            if( documentType == v1.DocumentType.Html )
            {
                return v2.DocumentType.Html;
            }
            else if( documentType == v1.DocumentType.Text )
            {
                return v2.DocumentType.Text;
            }
            else if( documentType == v1.DocumentType.None )
            {
                return v2.DocumentType.None;
            }

            throw new NotSupportedException( "Unknown DocumentType: " + documentType );
        }

        private static v2.Locating.DocumentLocator Migrate( v1.Navigation navigation )
        {
            var locator = new v2.Locating.DocumentLocator();

            foreach( var navi in navigation.Uris )
            {
                locator.Fragments.Add( Migrate( navi ) );
            }

            return locator;
        }

        private static v2.Locating.DocumentLocationFragment Migrate( v1.NavigationUrl navi )
        {
            if( navi.UrlType == v1.UriType.Request )
            {
                return new v2.Locating.Request( navi.UrlString );
            }
            else if( navi.UrlType == v1.UriType.Response )
            {
                return new v2.Locating.Response( navi.UrlString );
            }
            else if( navi.UrlType == v1.UriType.SubmitFormular )
            {
                return new v2.Locating.SubmitFormular( navi.UrlString, Migrate( navi.Formular ) );
            }

            throw new NotSupportedException( "Unknown UrlType: " + navi.UrlType );
        }

        private static v2.Locating.Formular Migrate( v1.Formular formular )
        {
            var form = new v2.Locating.Formular( formular.Name );

            foreach( var param in formular.Parameters )
            {
                form.Parameters[ param.Key ] = param.Value;
            }

            return form;
        }

        private static v2.Extraction.IFigureDescriptor Migrate( v1.IFormat format )
        {
            if( format is v1.PathCellFormat )
            {
                return Migrate( ( v1.PathCellFormat )format );
            }
            else if( format is v1.PathSeriesFormat )
            {
                return Migrate( ( v1.PathSeriesFormat )format );
            }

            throw new NotSupportedException( "Unknown format type: " + format.GetType() );
        }

        private static v2.Extraction.PathCellDescriptor Migrate( v1.PathCellFormat source )
        {
            var target = new v2.Extraction.PathCellDescriptor();

            target.Column = Migrate( source.Anchor.Column );
            target.Currency = source.Currency;
            target.Figure = source.Datum;
            target.InMillions = source.InMillions;
            target.Path = source.Path;
            target.Row = Migrate( source.Anchor.Row );
            target.ValueFormat = Migrate( source.ValueFormat );

            return target;
        }

        private static v2.Extraction.ISeriesLocator Migrate( v1.ICellLocator cellLocator )
        {
            var absolute = cellLocator as v1.AbsolutePositionLocator;
            if( absolute != null )
            {
                return new v2.Extraction.AbsolutePositionLocator { HeaderSeriesPosition = absolute.SeriesToScan, SeriesPosition = absolute.Position };
            }

            var stringContains = cellLocator as v1.StringContainsLocator;
            if( stringContains != null )
            {
                return new v2.Extraction.StringContainsLocator { HeaderSeriesPosition = stringContains.SeriesToScan, Pattern = stringContains.Pattern};
            }

            var regex = cellLocator as v1.RegexPatternLocator;
            if( regex != null )
            {
                return new v2.Extraction.RegexPatternLocator { HeaderSeriesPosition = regex.SeriesToScan, Pattern = regex.Pattern };
            }

            throw new NotSupportedException( "Unknown cell locator type: " + cellLocator.GetType() );
        }

        private static v2.Extraction.PathSeriesDescriptor Migrate( v1.PathSeriesFormat source )
        {
            var target = new v2.Extraction.PathSeriesDescriptor();

            foreach( var exclude in source.Expand == v1.CellDimension.Row ? source.SkipColumns : source.SkipRows )
            {
                target.Excludes.Add( exclude );
            }

            target.Figure = source.Datum;
            target.InMillions = source.InMillions;
            target.Orientation = source.Expand == v1.CellDimension.Row ? v2.Extraction.SeriesOrientation.Row : v2.Extraction.SeriesOrientation.Column;
            target.Path = source.Path;
            target.TimeFormat = Migrate( source.TimeAxisFormat );
            target.TimesLocator = new v2.Extraction.AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = source.TimeAxisPosition };
            target.ValueFormat = Migrate( source.ValueFormat );
            target.ValuesLocator = new v2.Extraction.StringContainsLocator { HeaderSeriesPosition = source.SeriesNamePosition, Pattern = source.SeriesName };

            return target;
        }

        private static v2.Extraction.FormatColumn Migrate( v1.FormatColumn source )
        {
            return new v2.Extraction.FormatColumn( source.Name, source.Type, source.Format )
            {
                ExtractionPattern = source.ExtractionPattern
            };
        }
    }
}
