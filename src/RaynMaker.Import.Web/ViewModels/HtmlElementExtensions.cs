using System.Drawing;
using System.Windows.Forms;
using Plainion;

namespace RaynMaker.Import.Web.ViewModels
{
    /// <summary>
    /// Extensions to the Windows.Forms.HtmlElement class.
    /// </summary>
    public static class HtmlElementExtensions
    {
        /// <summary>
        /// Marks the given element with the given color.
        /// <remarks>
        /// Uses a HTML span element to apply the markup and so modifies
        /// the HTML DOM.
        /// </remarks>
        /// </summary>
        public static void MarkElement( this HtmlElement e, Color color )
        {
            Contract.RequiresNotNull( e != null, "e" );

            if( e.IsMarked() )
            {
                return;
            }

            if( e.InnerHtml == null )
            {
                // create a pseudo element
                e.AppendChild( e.Document.CreateElement( "SPAN" ) );
                e.InnerText = "&nbsp;";
            }

            string text = ( e.InnerText == null ? "&nbsp;" : e.InnerText.Trim() );

            e.InnerHtml = string.Format( "<SPAN id=\"__maui_markup__\" style=\"color:black;background-color:{0}\">{1}</SPAN>",
                ColorTranslator.ToHtml( color ), text );
        }

        /// <summary>
        /// Removes the markup applied by <see cref="MarkElement"/> from the given element.
        /// </summary>
        public static void UnmarkElement( this HtmlElement e )
        {
            Contract.RequiresNotNull( e != null, "e" );

            if( !e.IsMarked() )
            {
                return;
            }

            e.InnerHtml = e.InnerText;
        }

        /// <summary>
        /// Indicates whether a HTML element has been marked by <see cref="MarkElement"/>.
        /// </summary>
        public static bool IsMarked( this HtmlElement e )
        {
            Contract.RequiresNotNull( e != null, "e" );

            return ( e.Children.Count > 0 && e.Children[ 0 ].Id == "__maui_markup__" );
        }
    }
}
