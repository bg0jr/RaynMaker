using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;

namespace RaynMaker.Import.Parsers.Html
{
    /// <summary>
    /// Extensions to the Windows.Forms.HtmlElement class.
    /// </summary>
    public static class HtmlElementExtensions
    {
        /// <summary>
        /// Returns the root element of the document the HtmlElement belongs too.
        /// </summary>
        public static IHtmlElement GetRoot( this IHtmlElement child )
        {
            while( child != null && !child.TagName.Equals( "html", StringComparison.OrdinalIgnoreCase ) )
            {
                child = child.Parent;
            }

            return child;
        }

        /// <summary>
        /// Returns the <see cref="HtmlPath"/> of the HtmlElement up to root.
        /// </summary>
        public static HtmlPath GetPath( this IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            HtmlPath path = new HtmlPath();

            var cur = element;
            while ( cur.Parent != null )
            {
                path.Elements.Insert( 0, new HtmlPathElement( cur.TagName, cur.GetChildPos() ) );

                cur = cur.Parent;
            }

            return path;
        }

        /// <summary>
        /// Returns the position of the HtmlElement in its parents children.
        /// </summary>
        public static int GetChildPos( this IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if ( element.Parent == null )
            {
                // assume its valid HTML with <html/> as root element
                return 0;
            }

            int childPos = 0;
            foreach ( var child in element.Parent.Children )
            {
                if ( child.TagName.Equals( element.TagName, StringComparison.OrdinalIgnoreCase ) )
                {
                    if ( child == element )
                    {
                        return childPos;
                    }
                    childPos++;
                }
            }

            //throw new ArgumentException( "Could not find child pos for child: " + e.TagName );
            return -1;
        }

        /// <summary>
        /// Returns the pos'th child with the given tagName.
        /// </summary>
        public static IHtmlElement GetChildAt( this IHtmlElement parent, string tagName, int pos )
        {
            Contract.RequiresNotNull( parent, "parent" );
            Contract.RequiresNotNullNotEmpty( tagName, "tagName" );

            int childPos = 0;
            foreach ( var child in parent.Children )
            {
                if ( child.TagName.Equals( tagName, StringComparison.OrdinalIgnoreCase ) )
                {
                    if ( childPos == pos )
                    {
                        return child;
                    }
                    childPos++;
                }
            }

            return null;

            // TODO: this could happen if the site has been changed and the 
            // path is no longer valid
            //throw new ArgumentException( "Could not find child for path: " + tagName + "[" + pos + "]" );
        }

        /// <summary>
        /// Searches for the parent of the given elment for which <c>cond</c> 
        /// gets <c>true</c>.
        /// </summary>
        /// <returns>the parent found if any, null otherwise</returns>
        public static IHtmlElement FindParent( this IHtmlElement start, Predicate<IHtmlElement> cond )
        {
            return FindParent( start, cond, p => false );
        }

        /// <summary>
        /// Searches for the parent of the given elment for which <c>cond</c> 
        /// gets <c>true</c>.
        /// Stops the search and returns null if <c>abortIf</c> gets true before
        /// <c>cond</c> gets true.
        /// The given element must not fullfil the abort condition.
        /// </summary>
        /// <returns>the parent found if any, null otherwise</returns>
        public static IHtmlElement FindParent( this IHtmlElement start, Predicate<IHtmlElement> cond, Predicate<IHtmlElement> abortIf )
        {
            Contract.RequiresNotNull( start, "start" );
            Contract.Requires( !abortIf( start ), "start must not be already the target" );

            var parent = start.Parent;
            while ( parent != null && !abortIf( parent ) )
            {
                if ( cond( parent ) )
                {
                    return parent;
                }

                parent = parent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Searches for the table which embedds the given element.
        /// If the start element is a TABLE element, this one is returned.
        /// </summary>
        public static HtmlTable FindEmbeddingTable( this IHtmlElement start )
        {
            Contract.RequiresNotNull( start, "start" );

            if ( start.TagName == "TABLE" )
            {
                return new HtmlTable( start );
            }

            var table = start.FindParent( p => p.TagName == "TABLE" );

            return (table == null ? null : new HtmlTable( table ));
        }

        /// <summary>
        /// If the given element is a html link or it contains
        /// a child which is a html link then the url of the first 
        /// link found is returned. Otherwise the InnerText of the element
        /// is returned.
        /// </summary>
        public static string FirstLinkOrInnerText( this IHtmlElement element )
        {
            if ( element.TagName == "A" )
            {
                return element.GetAttribute( "HREF" );
            }

            IHtmlElement link = element.Children.FirstOrDefault( child => child.TagName == "A" );
            return (link != null ? link.GetAttribute( "HREF" ) : element.InnerText);
        }

        /// <summary>
        /// Returns true if the given element is either a table or a tbody element.
        /// </summary>
        public static bool IsTableOrTBody( this IHtmlElement e )
        {
            return (e.TagName == "TBODY" || e.TagName == "TABLE");
        }

        /// <summary>
        /// Recursively returns all inner elements
        /// </summary>
        public static IEnumerable<IHtmlElement> GetInnerElements( this IHtmlElement element )
        {
            var children = new List<IHtmlElement>();

            foreach ( var child in element.Children )
            {
                children.Add( child );
                children.AddRange( child.GetInnerElements() );
            }

            return children;
        }
    }
}
