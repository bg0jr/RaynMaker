using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plainion;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public static class MarkupFactory
    {
        public static IHtmlMarker CreateMarker<T>( T descriptor ) where T : IFigureDescriptor
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );

            IHtmlMarker marker = null;

            if ( ( marker = TryCreateMarker( descriptor as PathSeriesDescriptor ) ) != null ) return marker;
            if ( ( marker = TryCreateMarker( descriptor as PathCellDescriptor ) ) != null ) return marker;
            if ( ( marker = TryCreateMarker( descriptor as PathSingleValueDescriptor ) ) != null ) return marker;
            if ( ( marker = TryCreateMarker( descriptor as PathTableDescriptor ) ) != null ) return marker;

            throw new NotSupportedException( "Descriptor not supported: " + descriptor.GetType().Name );
        }

        private static IHtmlMarker TryCreateMarker( PathSeriesDescriptor descriptor )
        {
            if ( descriptor == null ) return null;

            var marker = new HtmlTableMarker();

            if ( descriptor.Orientation == SeriesOrientation.Row )
            {
                marker.ExpandRow = true;

                marker.ColumnHeaderRow = descriptor.TimesLocator.HeaderSeriesPosition;
                marker.RowHeaderColumn = descriptor.ValuesLocator.HeaderSeriesPosition;

                marker.SkipColumns = descriptor.Excludes.ToArray();
                marker.SkipRows = null;
            }
            else if ( descriptor.Orientation == SeriesOrientation.Column )
            {
                marker.ExpandColumn = true;

                marker.RowHeaderColumn = descriptor.TimesLocator.HeaderSeriesPosition;
                marker.ColumnHeaderRow = descriptor.ValuesLocator.HeaderSeriesPosition;

                marker.SkipColumns = null;
                marker.SkipRows = descriptor.Excludes.ToArray();
            }

            return marker;
        }

        private static IHtmlMarker TryCreateMarker( PathCellDescriptor descriptor )
        {
            if ( descriptor == null ) return null;

            var marker = new HtmlTableMarker();

            marker.ColumnHeaderRow = descriptor.Column.HeaderSeriesPosition;
            marker.RowHeaderColumn = descriptor.Row.HeaderSeriesPosition;

            return marker;
        }

        private static IHtmlMarker TryCreateMarker( PathSingleValueDescriptor descriptor )
        {
            if ( descriptor == null ) return null;

            return new HtmlElementMarker( HtmlTableMarker.DefaultCellColor );
        }

        private static IHtmlMarker TryCreateMarker( PathTableDescriptor descriptor )
        {
            if ( descriptor == null ) return null;

            return new HtmlElementMarker( HtmlTableMarker.DefaultCellColor );
        }

        public static IHtmlElement FindElementByDescriptor<T>( IHtmlDocument document, T descriptor ) where T : IFigureDescriptor
        {
            Contract.RequiresNotNull( document, "document" );
            Contract.RequiresNotNull( descriptor, "descriptor" );

            {
                var pathSeriesDescriptor = descriptor as PathSeriesDescriptor;
                if ( pathSeriesDescriptor != null )
                {
                    return TryFindElementByDescriptor( document, pathSeriesDescriptor );
                }
            }
            {
                var pathCellDescriptor = descriptor as PathCellDescriptor;
                if ( pathCellDescriptor != null )
                {
                    return TryFindElementByDescriptor( document, pathCellDescriptor );
                }
            }

            throw new NotSupportedException( "Descriptor not supported: " + descriptor.GetType().Name );
        }

        private static IHtmlElement TryFindElementByDescriptor( IHtmlDocument document, PathSeriesDescriptor descriptor )
        {
            if ( string.IsNullOrEmpty( descriptor.Path ) )
            {
                return null;
            }

            var table = HtmlTable.FindByPath( document, HtmlPath.Parse( descriptor.Path ) );
            if ( table == null )
            {
                return null;
            }

            if ( descriptor.Orientation == SeriesOrientation.Column )
            {
                int rowToScan = descriptor.ValuesLocator.HeaderSeriesPosition;
                if ( 0 > rowToScan || rowToScan >= table.Rows.Count )
                {
                    return null;
                }

                var colIdx = descriptor.ValuesLocator.FindIndex( table.GetRow( rowToScan ).Select( item => item.InnerText ) );
                if ( colIdx == -1 )
                {
                    return null;
                }

                int rowIdx = 0;
                if ( rowIdx == descriptor.ValuesLocator.HeaderSeriesPosition && rowIdx < table.Rows.Count )
                {
                    rowIdx++;
                }

                return table.GetCellAt( rowIdx, colIdx );
            }
            else if ( descriptor.Orientation == SeriesOrientation.Row )
            {
                var colToScan = descriptor.ValuesLocator.HeaderSeriesPosition;
                if ( 0 > colToScan || colToScan >= table.GetRow( 0 ).Count() )
                {
                    return null;
                }

                var rowIdx = descriptor.ValuesLocator.FindIndex( table.GetColumn( colToScan ).Select( item => item.InnerText ) );
                if ( rowIdx == -1 )
                {
                    return null;
                }

                int colIdx = 0;
                if ( colIdx == descriptor.ValuesLocator.HeaderSeriesPosition && colIdx < table.GetRow( rowIdx ).Count )
                {
                    colIdx++;
                }

                return table.GetCellAt( rowIdx, colIdx );
            }

            throw new NotSupportedException( "Unknown orientation: " + descriptor.Orientation );
        }

        private static IHtmlElement TryFindElementByDescriptor( IHtmlDocument document, PathCellDescriptor descriptor )
        {
            if ( string.IsNullOrEmpty( descriptor.Path ) )
            {
                return null;
            }

            var table = HtmlTable.FindByPath( document, HtmlPath.Parse( descriptor.Path ) );
            if ( table == null )
            {
                return null;
            }

            int rowToScan = descriptor.Column.HeaderSeriesPosition;
            if ( 0 > rowToScan || rowToScan >= table.Rows.Count )
            {
                return null;
            }

            var colIdx = descriptor.Column.FindIndex( table.GetRow( rowToScan ).Select( item => item.InnerText ) );
            if ( colIdx == -1 )
            {
                return null;
            }

            var colToScan = descriptor.Row.HeaderSeriesPosition;
            if ( 0 > colToScan || colToScan >= table.GetRow( 0 ).Count() )
            {
                return null;
            }

            var rowIdx = descriptor.Row.FindIndex( table.GetColumn( colToScan ).Select( item => item.InnerText ) );
            if ( rowIdx == -1 )
            {
                return null;
            }

            return table.GetCellAt( rowIdx, colIdx );
        }
    }
}
