using System.Windows.Forms;

namespace RaynMaker.Modules.Import.Design
{
    public class HtmlMarkupAutomationProvider
    {
        public static void SimulateClickOn( HtmlDocument document, string clickedElementId )
        {
            document.Body.SetAttribute( "rym_clicked_on", clickedElementId );

            document.GetElementById( clickedElementId ).InvokeMember( "Click" );

            document.Body.SetAttribute( "rym_clicked_on", string.Empty );
        }

        public static HtmlElement GetClickedElement( HtmlDocument document )
        {
            var id = document.Body.GetAttribute( "rym_clicked_on" );
            if ( string.IsNullOrEmpty( id ) )
            {
                return null;
            }

            return document.GetElementById( id );
        }
    }
}
