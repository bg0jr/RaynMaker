using System;
using System.Diagnostics;
using System.IO;
using Plainion;

namespace RaynMaker.Infrastructure.Services
{
    static class HelpService
    {
        public static void ShowHelp( string topic, string section )
        {
            Contract.RequiresNotNull( topic, "topic" );

            var home = Path.GetDirectoryName( typeof( HelpService ).Assembly.Location );
            var topicFile = Path.Combine( home, "Help", topic + ".html" );

            // convert into file:// protocol
            topicFile = new Uri( topicFile ).AbsoluteUri;

            if( !string.IsNullOrEmpty( section ) )
            {
                topicFile += "#" + section;
            }

            // without specifying browser process document local anchors do not work
            Process.Start( "iexplore", topicFile );
        }
    }
}
