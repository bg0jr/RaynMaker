using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using HtmlAgilityPack;
using Plainion.AppFw.Shell.Forms;
using Plainion.Logging;

namespace RaynMaker.SDK.Scripts
{
    public class MkOffline : FormsAppBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( MkOffline ) );

        private HtmlDocument myDocument = null;
        private string myOutputFile = null;

        [Argument( Short = "-n", Description = "Only download the given page" )]
        public bool DownloadOnly
        {
            get;
            set;
        }

        [Argument( Short = "-url", Description = "URL to the page to download" ), Required]
        public Uri InputUri
        {
            get;
            set;
        }

        protected override void Run()
        {
            LoadDocument();

            MakeDocumentOfflineUsable();

            StoreOfflineDocument();

            PrintFinishedBanner();
        }

        private void LoadDocument()
        {
            bool deleteInput = false;
            string inputFile = InputUri.ToString();

            if( InputUri.Scheme != Uri.UriSchemeFile )
            {
                deleteInput = true;
                inputFile = Path.GetTempFileName();

                WebUtil.DownloadTo( InputUri, inputFile );
            }

            myDocument = new HtmlDocument();
            myDocument.Load( inputFile );

            if( deleteInput )
            {
                File.Delete( inputFile );
            }
        }

        private void MakeDocumentOfflineUsable()
        {
            if( DownloadOnly )
            {
                return;
            }

            RemoveWebReferences( myDocument );
        }

        public static void RemoveWebReferences( HtmlDocument doc )
        {
            TraverseNodeTree( doc.DocumentNode, RemoveWebReferences );
        }

        public static void TraverseNodeTree( HtmlNode root, Action<HtmlNode> visitorAction )
        {
            foreach( HtmlNode node in root.ChildNodes )
            {
                visitorAction( node );

                TraverseNodeTree( node, visitorAction );
            }
        }

        public static void RemoveWebReferences( HtmlNode node )
        {
            RemoveScriptTag( node );
            RemoveImgTag( node );
            RemoveIFrameTag( node );
            RemoveLinkTag( node );
            RemoveEmbedTag( node );
            RemoveComment( node );
        }

        private static void RemoveScriptTag( HtmlNode node )
        {
            if( !node.Name.Equals( "script" ,StringComparison.OrdinalIgnoreCase) ) return;

            node.RemoveAll();
        }

        private static void RemoveImgTag( HtmlNode node )
        {
            if( !node.Name.Equals( "img", StringComparison.OrdinalIgnoreCase ) ) return;

            node.RemoveAll();
        }

        private static void RemoveIFrameTag( HtmlNode node )
        {
            if( !node.Name.Equals( "iframe", StringComparison.OrdinalIgnoreCase ) ) return;

            node.Attributes.RemoveAll();
        }

        private static void RemoveLinkTag( HtmlNode node )
        {
            if( !node.Name.Equals( "link", StringComparison.OrdinalIgnoreCase ) ) return;

            node.Attributes.RemoveAll();
        }

        private static void RemoveEmbedTag( HtmlNode node )
        {
            if( !node.Name.Equals( "embed", StringComparison.OrdinalIgnoreCase ) ) return;

            node.Attributes.RemoveAll();
        }

        private static void RemoveComment( HtmlNode node )
        {
            if( node.NodeType != HtmlNodeType.Comment ) return;

            node.RemoveAll();
        }

        private void StoreOfflineDocument()
        {
            myOutputFile = GetOutputFile( InputUri );
            myDocument.Save( myOutputFile );
        }

        private string GetOutputFile( Uri inputUrl )
        {
            return inputUrl.Scheme == Uri.UriSchemeFile
                ? inputUrl.LocalPath + ".offline.html"
                : "output.html";
        }

        private void PrintFinishedBanner()
        {
            Console.WriteLine( "Output written to: " + myOutputFile );

            Console.WriteLine( "" );
            Console.WriteLine( "Remaining steps:" );
            Console.WriteLine( "  - manually check for: <style type=\"text/css\"><!--@import url()--></style>" );
        }
    }
}

