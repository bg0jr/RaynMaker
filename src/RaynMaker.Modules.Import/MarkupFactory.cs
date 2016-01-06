using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plainion;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public static class MarkupFactory
    {
        public static IHtmlMarker Create<T>( T descriptor ) where T : IFigureDescriptor
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );

            IHtmlMarker marker = null;

            if( ( marker = TryCreate( descriptor as PathSeriesDescriptor ) ) != null ) return marker;
            if( ( marker = TryCreate( descriptor as PathCellDescriptor ) ) != null ) return marker;
            if( ( marker = TryCreate( descriptor as PathSingleValueDescriptor ) ) != null ) return marker;
            if( ( marker = TryCreate( descriptor as PathTableDescriptor ) ) != null ) return marker;
            if( ( marker = TryCreate( descriptor as CsvDescriptor ) ) != null ) return marker;
            if( ( marker = TryCreate( descriptor as SeparatorSeriesDescriptor ) ) != null ) return marker;

            throw new NotSupportedException( "Unknown descriptor type: " + descriptor.GetType().Name );
        }

        private static IHtmlMarker TryCreate( PathSeriesDescriptor descriptor )
        {
            var marker = new HtmlTableMarker();

            if( descriptor.Orientation == SeriesOrientation.Row )
            {
                marker.ExpandRow = true;

                marker.ColumnHeaderRow = descriptor.TimesLocator.HeaderSeriesPosition;
                marker.RowHeaderColumn = descriptor.ValuesLocator.HeaderSeriesPosition;

                marker.SkipColumns = null;
                marker.SkipRows = descriptor.Excludes.ToArray();
            }
            else if( descriptor.Orientation == SeriesOrientation.Column )
            {
                marker.ExpandColumn = true;

                marker.RowHeaderColumn = descriptor.TimesLocator.HeaderSeriesPosition;
                marker.ColumnHeaderRow = descriptor.ValuesLocator.HeaderSeriesPosition;

                marker.SkipColumns = descriptor.Excludes.ToArray();
                marker.SkipRows = null;
            }

            return marker;
        }

        private static IHtmlMarker TryCreate( PathCellDescriptor descriptor )
        {
            var marker = new HtmlTableMarker();

            marker.ColumnHeaderRow = descriptor.Column.HeaderSeriesPosition;
            marker.RowHeaderColumn = descriptor.Row.HeaderSeriesPosition;

            return marker;
        }

        private static IHtmlMarker TryCreate( PathSingleValueDescriptor descriptor )
        {
            return new HtmlElementMarker( HtmlTableMarker.DefaultCellColor );
        }

        private static IHtmlMarker TryCreate( PathTableDescriptor descriptor )
        {
            return new HtmlElementMarker( HtmlTableMarker.DefaultCellColor );
        }

        private static IHtmlMarker TryCreate( CsvDescriptor descriptor )
        {
            throw new NotImplementedException();
        }

        private static IHtmlMarker TryCreate( SeparatorSeriesDescriptor descriptor )
        {
            throw new NotImplementedException();
        }
    }
}
