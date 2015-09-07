using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Descripes the steps for automated site navigation.
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "Navigation" )]
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
            : this( docType, urls.ToList() )
        {
        }

        public Navigation( DocumentType docType, IEnumerable<NavigatorUrl> urls )
        {
            DocumentType = docType;
            Uris = urls.ToList();

            UrisHashCode = CreateUrisHashCode();
        }

        private int CreateUrisHashCode()
        {
            var uriStrings = Uris.Select( uri => uri.UrlString ).ToArray();
            var hashCodeString = string.Join( string.Empty, uriStrings );

            return hashCodeString.GetHashCode();
        }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public List<NavigatorUrl> Uris { get; private set; }

        [DataMember]
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
