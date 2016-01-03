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

            sheet.SetSources( dataSources );

            return sheet;
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
                locator.Fragments.Add( Migrate(navi) );
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
                return new v2.Locating.SubmitFormular(navi.UrlString, Migrate(navi.Formular) );
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
            return null;
        }
    }
}
