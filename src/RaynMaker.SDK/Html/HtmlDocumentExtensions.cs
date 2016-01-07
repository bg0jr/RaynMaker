using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RaynMaker.SDK.Html
{
    public static class HtmlDocumentExtensions
    {
        public static void WriteTo( this HtmlDocument self, string file )
        {
            File.WriteAllText( file, self.Body.Parent.OuterHtml, Encoding.GetEncoding( self.Encoding ) );
        }

        public static void OpenDocumentInExternalBrowser( this HtmlDocument self )
        {
            var file = Path.GetTempFileName() + ".html";

            self.WriteTo( file );

            Process.Start( file ).WaitForExit();

            File.Delete( file );
        }
    }
}
