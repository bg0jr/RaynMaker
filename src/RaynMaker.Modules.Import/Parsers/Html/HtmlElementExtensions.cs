using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Parsers.Html
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
            while( cur.Parent != null )
            {
                path.Elements.Insert( 0, new HtmlPathElement( cur.TagName, cur.GetChildOfTagPos() ) );

                cur = cur.Parent;
            }

            return path;
        }

        /// <summary>
        /// Returns the position of the HtmlElement in its parents children.
        /// </summary>
        public static int GetChildOfTagPos( this IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( element.Parent == null )
            {
                // assume its valid HTML with <html/> as root element
                return 0;
            }

            int childPos = 0;
            foreach( var child in element.Parent.Children )
            {
                if( child.TagName.Equals( element.TagName, StringComparison.OrdinalIgnoreCase ) )
                {
                    if( child == element )
                    {
                        return childPos;
                    }
                    childPos++;
                }
            }

            //throw new ArgumentException( "Could not find child pos for child: " + e.TagName );
            return -1;
        }

        public static int GetChildPos( this IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            if( element.Parent == null )
            {
                // assume its valid HTML with <html/> as root element
                return 0;
            }

            int childPos = 0;
            foreach( var child in element.Parent.Children )
            {
                if( child == element )
                {
                    return childPos;
                }
                childPos++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the pos'th child with the given tagName.
        /// </summary>
        public static IHtmlElement GetChildAt( this IHtmlElement parent, string tagName, int pos )
        {
            return parent.GetChildAt( new[] { tagName }, pos );
        }

        /// <summary>
        /// Returns the pos'th child with the given tagName.
        /// </summary>
        public static IHtmlElement GetChildAt( this IHtmlElement parent, string[] tagNames, int pos )
        {
            Contract.RequiresNotNull( parent, "parent" );
            Contract.RequiresNotNullNotEmpty( tagNames, "tagNames" );

            int childPos = 0;
            foreach( var child in parent.Children )
            {
                if( tagNames.Any( t => child.TagName.Equals( t, StringComparison.OrdinalIgnoreCase ) ) )
                {
                    if( childPos == pos )
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
            while( parent != null && !abortIf( parent ) )
            {
                if( cond( parent ) )
                {
                    return parent;
                }

                parent = parent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Recursively returns all inner elements
        /// </summary>
        public static IEnumerable<IHtmlElement> GetInnerElements( this IHtmlElement element )
        {
            var children = new List<IHtmlElement>();

            foreach( var child in element.Children )
            {
                children.Add( child );
                children.AddRange( child.GetInnerElements() );
            }

            return children;
        }
    }
}
