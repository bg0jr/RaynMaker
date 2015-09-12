using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RaynMaker.Import.Html
{
    /// <summary>
    /// Specifies a path inside a HTML document from the root to a HTML element.
    /// </summary>
    public class HtmlPath
    {
        private const char PathSeparator = '/';

        /// <summary/>
        public HtmlPath()
        {
            Elements = new List<HtmlPathElement>();
        }

        /// <summary/>
        public HtmlPath( IEnumerable<HtmlPathElement> elements )
        {
            Elements = elements.ToList();
        }

        /// <summary/>
        public IList<HtmlPathElement> Elements { get; private set; }

        /// <summary/>
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

        /// <summary/>
        public bool PointsToTable
        {
            get { return Last.IsTableOrTBody; }
        }

        /// <summary/>
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

        /// <summary/>
        public override string ToString()
        {
            return PathSeparator + string.Join( PathSeparator.ToString(), Elements );
        }

        /// <summary/>
        public static HtmlPath Parse( string pathStr )
        {
            if( string.IsNullOrEmpty( pathStr ) )
            {
                throw new ArgumentException( "value is null or empty string", "pathStr" );
            }

            var pathElements = pathStr.Split( PathSeparator )
                .Where( token => !string.IsNullOrWhiteSpace( token ) )
                .Select( token => HtmlPathElement.Parse( token ) );

            return new HtmlPath( pathElements );
        }
    }
}
