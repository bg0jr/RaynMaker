using System;
using HtmlAgilityPack;

namespace RaynMaker.Modules.Import.Documents.AgilityPack
{
    /// <summary>
    /// Attention: Currently it still has a lot of trouble parsing HTML !! HtmlPath from WindowsExplorer does not match!
    /// </summary>
    class HtmlDocumentLoader : IDocumentLoader
    {
        public IDocument Load( Uri uri )
        {
            TuneHtmlParser();

            return LoadDocument( uri );
        }

        private static void TuneHtmlParser()
        {
            // http://htmlagilitypack.codeplex.com/workitem/23074
            HtmlNode.ElementsFlags.Remove( "form" );
        }

        private IHtmlDocument LoadDocument( Uri url )
        {
            var client = WebUtil.CreateWebClient();

            var doc = new HtmlAgilityPack.HtmlDocument();
            using ( var stream = client.OpenRead( url ) )
            {
                doc.Load( stream );
            }
            return new HtmlDocumentAdapter( url, doc );
        }

        public static IHtmlDocument LoadHtml( string html )
        {
            var doc = new HtmlDocument();
            doc.LoadHtml( html );

            return new HtmlDocumentAdapter( new Uri( "about:blank" ), doc );
        }
    }
}
