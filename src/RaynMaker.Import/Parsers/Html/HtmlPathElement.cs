using System;
using System.Globalization;
using System.Text;

using Plainion;

namespace RaynMaker.Import.Parsers.Html
{
    /// <summary>
    /// Defines an element of a <see cref="HtmlPath"/>.
    /// </summary>
    public class HtmlPathElement
    {
        private static readonly char[] OccuranceSeparator = { '[', ']' };

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="tagName">Name of the HTML tag</param>
        /// <param name="pos">position of the HTML element in the list of its parents children with the same tag name</param>
        public HtmlPathElement( string tagName, int pos )
        {
            tagName = tagName != null ? tagName.Trim() : null;

            Contract.RequiresNotNullNotEmpty( tagName, "tagName" );
            Contract.Requires( pos >= 0, "pos must be >= 0" );

            TagName = tagName.ToUpper();
            Position = pos;
        }

        /// <summary>
        /// HTML tag name.
        /// </summary>
        public string TagName { get; private set; }

        /// <summary>
        /// Position of the HTML element in its parents children with the same tag name.
        /// <example>
        /// If a parent has the following sequence of children: P, DIV, DIV, P, P then the 
        /// position of the second P is 2 (the DIV tags are omitted).
        /// </example>
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Indicates whether the element is a table/table body.
        /// </summary>
        public bool IsTableOrTBody
        {
            get { return TagName == "TBODY" || TagName == "TABLE"; }
        }

        /// <summary>
        /// Returns a string representation of this <see cref="HtmlPathElement"/>.
        /// <seealso cref="HtmlPath.ToString"/>
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( TagName );
            sb.Append( "[" );
            sb.Append( Position );
            sb.Append( "]" );

            return sb.ToString();
        }

        /// <summary>
        /// Parses the given string representation of a <see cref="HtmlPathElement"/>.
        /// <seealso cref="HtmlPath.Parse"/>
        /// </summary>
        public static HtmlPathElement Parse( string str )
        {
            string[] tokens = str.Split( OccuranceSeparator );
            int childPos = Convert.ToInt32( tokens[ 1 ], CultureInfo.InvariantCulture );

            return new HtmlPathElement( tokens[ 0 ], childPos );
        }
    }

}
