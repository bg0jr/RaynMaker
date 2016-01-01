using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Plainion;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    /// <summary>
    /// Specifies a path inside a HTML document from the root to a HTML element.
    /// </summary>
    public class HtmlPath
    {
        private const char PathSeparator = '/';

        public HtmlPath()
        {
            Elements = new List<HtmlPathElement>();
        }

        public HtmlPath( IEnumerable<HtmlPathElement> elements )
        {
            Elements = elements.ToList();
        }

        public IList<HtmlPathElement> Elements { get; private set; }

        public HtmlPathElement Last
        {
            get
            {
                if( Elements.Count == 0 )
                {
                    return null;
                }
                return Elements[ Elements.Count - 1 ];
            }
        }

        public bool PointsToTable
        {
            get { return Last.IsTableOrTBody; }
        }

        public bool PointsToTableCell
        {
            get { return Last.TagName == "TD"; }
        }

        /// <summary>
        /// Returns the position of the table cell in the cell the
        /// path is pointing to.
        /// If the path does not point into a table or the position could
        /// not be calculated an empty point is returned.
        /// </summary>
        public Point GetTableCellPosition()
        {
            if( !PointsToTableCell )
            {
                return Point.Empty;
            }

            Point p = new Point();
            p.X = Last.Position;

            foreach( HtmlPathElement tr in Elements.Reverse() )
            {
                if( tr.IsTableOrTBody )
                {
                    // then this is a currupt path
                    return Point.Empty;
                }

                if( tr.TagName == "TR" )
                {
                    p.Y = tr.Position;
                    return p;
                }
            }

            // no TR found -> path corrupt
            return Point.Empty;
        }

        public override string ToString()
        {
            return PathSeparator + string.Join( PathSeparator.ToString(), Elements );
        }

        public static HtmlPath TryParse( string pathStr )
        {
            Contract.RequiresNotNull( pathStr, "pathStr" );

            var pathElements = pathStr.Split( PathSeparator )
                .Where( token => !string.IsNullOrWhiteSpace( token ) )
                .Select( token => HtmlPathElement.TryParse( token ) )
                .ToList();

            if( pathElements.Any( e => e == null ) )
            {
                return null;
            }

            return new HtmlPath( pathElements );
        }

        public static HtmlPath Parse( string pathStr )
        {
            var path = TryParse( pathStr );

            Contract.Requires( path != null, "Failed to parse '{0}'", pathStr );

            return path;
        }
    }
}
