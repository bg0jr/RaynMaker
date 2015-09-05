using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Collections;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Descripes the steps for automated site navigation.
    /// </summary>
    [Serializable]
    public class Navigation
    {
        public static Navigation Empty = new Navigation();

        private Navigation()
            : this( DocumentType.None )
        {
        }

        public Navigation( DocumentType docType, string url )
            : this( docType, new NavigatorUrl( UriType.Request, url ) )
        {
        }

        public Navigation( DocumentType docType, params NavigatorUrl[] urls )
            : this( docType, ( IReadOnlyList<NavigatorUrl> )urls )
        {
        }

        public Navigation( DocumentType docType, IReadOnlyList<NavigatorUrl> urls )
        {
            DocumentType = docType;
            Uris = urls;

            UrisHashCode = CreateUrisHashCode();
        }

        private int CreateUrisHashCode()
        {
            var uriStrings = Uris.Select( uri => uri.UrlString ).ToArray();
            var hashCodeString = string.Join( string.Empty, uriStrings );

            return hashCodeString.GetHashCode();
        }

        public DocumentType DocumentType { get; set; }
        public IReadOnlyList<NavigatorUrl> Uris { get; set; }

        public int UrisHashCode { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( GetType().FullName );
            sb.Append( " (DocType: " );
            sb.Append( DocumentType );

            sb.Append( ", Uris: [" );
            foreach( var uri in Uris )
            {
                sb.Append( uri.ToString() );
                sb.Append( "," );
            }
            sb.Append( "])" );

            return sb.ToString();
        }
    }
}
