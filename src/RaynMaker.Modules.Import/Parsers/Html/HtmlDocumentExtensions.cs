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
                root = root.GetChildAt( element.TagName, element.Position );

                if( root == null )
                {
                    return null;
                }
            }

            return root;
        }
    }
}
