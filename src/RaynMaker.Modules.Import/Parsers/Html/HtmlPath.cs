using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Plainion;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    /// <summary>
    /// Specifies a path inside a HTML document from the root to a HTML element.
    /// A leading HtmlPathElement with tag HTML will be ignored
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
            Contract.RequiresNotNull( elements, "elements" );

            var clone = elements.ToList();

            if( clone.Count > 0 && clone[ 0 ].TagName == "HTML" )
            {
                clone.RemoveAt( 0 );
            }

            Elements = clone;
        }

        public IReadOnlyList<HtmlPathElement> Elements { get; private set; }

        public bool PointsToTable
        {
            get { return Elements.Count > 0 && Elements[ Elements.Count - 1 ].IsTable; }
        }

        public bool PointsToTableCell
        {
            get { return Elements.Count > 0 && Elements[ Elements.Count - 1 ].IsTableCell; }
        }

        /// <summary>
        /// Returns a new HtmlPath pointing to the table element this instance is pointing into.
        /// If this instance is not pointing into any table at all null is returned;
        /// </summary>
        public HtmlPath GetPathToTable()
        {
            var result = new HtmlPath( Elements );

            while ( result.Elements.Count > 0 )
            {
                if ( result.PointsToTable )
                {
                    return result;
                }

                result = new HtmlPath( result.Elements.Take( result.Elements.Count - 1 ) );
            }

            return result.Elements.Any() ? result : null;
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
