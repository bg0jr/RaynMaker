using System;
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

            var current = doc.Body.GetRoot();

            foreach( var element in path.Elements )
            {
                current = GetNthChildWithTag( current, element.TagName, element.Position );

                if( current == null )
                {
                    return null;
                }
            }

            return current;
        }

        private static IHtmlElement GetNthChildWithTag( IHtmlElement parent, string tagName, int pos )
        {
            int childPos = 0;
            foreach( var child in parent.Children )
            {
                if( child.TagName.Equals( tagName, StringComparison.OrdinalIgnoreCase ) )
                {
                    if( childPos == pos )
                    {
                        return child;
                    }
                    childPos++;
                }
            }

            return null;
        }
    }
}
