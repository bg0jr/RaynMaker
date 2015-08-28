using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using Blade;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Web
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
            e.Require( x => e != null );

            if ( e.IsMarked() )
            {
                return;
            }

            if ( e.InnerHtml == null )
            {
                // create a pseudo element
                e.AppendChild( e.Document.CreateElement( "SPAN" ) );
                e.InnerText = "&nbsp;";
            }

            string text = ( e.InnerText == null ? "&nbsp;" : e.InnerText.Trim() );

            e.InnerHtml = string.Format(
                "<span id=\"__maui_markup__\" style=\"color:black;background-color:{0}\">{1}</b>",
                ColorTranslator.ToHtml( color ), text );
        }

        /// <summary>
        /// Removes the markup applied by <see cref="MarkElement"/> from the given element.
        /// </summary>
        public static void UnmarkElement( this HtmlElement e )
        {
            e.Require( x => e != null );

            if ( !e.IsMarked() )
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
            e.Require( x => e != null );

            return ( e.Children.Count>0 && e.FirstChild.Id == "__maui_markup__" );
        }
    }
}
