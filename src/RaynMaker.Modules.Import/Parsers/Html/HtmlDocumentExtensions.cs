using System;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    /// <summary>
    /// Extensions for the Windows.Forms.HtmlDocument class.
    /// </summary>
    public static class HtmlDocumentExtensions
    {
        /// <summary>
        /// Returns the element specified by the given <see cref="HtmlPath"/>.
        /// </summary>
        public static IHtmlElement GetElementByPath( this IHtmlDocument doc, HtmlPath path )
        {
            Contract.RequiresNotNull( doc, "doc" );
            Contract.RequiresNotNull( path, "path" );

            var root = doc.Body.GetRoot();
            if( root == null )
            {
                return null;
            }

            foreach( var element in path.Elements )
            {
                root = GetChildAt(root, element.TagName, element.Position );

                if( root == null )
                {
                    return null;
                }
            }

            return root;
        }

        /// <summary>
        /// Returns the pos'th child with the given tagName.
        /// </summary>
        private static IHtmlElement GetChildAt( IHtmlElement parent, string tagName, int pos )
        {
            return GetChildAt( parent, new[] { tagName }, pos );
        }

        /// <summary>
        /// Returns the pos'th child with the given tagName.
        /// </summary>
        private static IHtmlElement GetChildAt( IHtmlElement parent, string[] tagNames, int pos )
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
    }
}
