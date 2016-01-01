using System;
using System.IO;
using System.Net;
using System.Text;
using Plainion;
using Plainion.Logging;

namespace RaynMaker.Modules.Import.Documents
{
    static class WebUtil
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( WebUtil ) );

        /// <summary>
        /// Downloads the document specified by the remote Uri to the given local path.
        /// </summary>
        public static void DownloadTo( Uri remoteUri, string localPath )
        {
            var client = CreateWebClient();

            try
            {
                myLogger.Info( "Downloading from: {0}", remoteUri );
                client.DownloadFile( remoteUri, localPath );
            }
            catch ( WebException ex )
            {
                ex.AddContext( "URL", remoteUri );
                throw;
            }
        }

        public static WebClient CreateWebClient()
        {
            var client = new WebClient();
            client.UseDefaultCredentials = true;

            // if we download the file from the server we have to set a proper user-agent
            // otherwise we might get a different HTML page as requested by RaynMaker.Modules.Import.Web or IE
            client.Headers.Add( "user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0)" );

            return client;
        }

        /// <summary>
        /// Downloads the document specified by the remote Uri to a temp location.
        /// If the remoteUri is already a local path nothing is downloaded.
        /// </summary>
        public static string Download( Uri remoteUri )
        {
            if ( remoteUri.IsFile )
            {
                return remoteUri.LocalPath;
            }

            // TODO: we need to delete the file!!
            var localFile = Path.GetTempFileName();
            DownloadTo( remoteUri, localFile );

            return localFile;
        }
    }
}
