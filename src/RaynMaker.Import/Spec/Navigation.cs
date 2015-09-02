using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Blade.Collections;
using Blade.Reflection;
using System.Diagnostics;

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
            : this( docType, ( IEnumerable<NavigatorUrl> )urls )
        {
        }

        public Navigation( DocumentType docType, IEnumerable<NavigatorUrl> urls )
        {
            DocumentType = docType;
            Uris = urls.ToArrayList();

            UrisHashCode = CreateUrisHashCode();
        }

        private int CreateUrisHashCode()
        {
            var uriStrings = Uris.Select( uri => uri.UrlString ).ToArray();
            var hashCodeString = string.Join( string.Empty, uriStrings );

            return hashCodeString.GetHashCode();
        }

        public Navigation( DocumentType docType, IArray<NavigatorUrl> uris )
        {
            DocumentType = docType;
            Uris = uris;

            UrisHashCode = CreateUrisHashCode();
        }

        public DocumentType DocumentType { get; private set; }
        public IArray<NavigatorUrl> Uris { get; private set; }

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
