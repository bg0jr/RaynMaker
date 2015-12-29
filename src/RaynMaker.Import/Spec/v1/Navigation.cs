using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RaynMaker.Import.Spec.v1
{
    /// <summary>
    /// Descripes the steps for automated site navigation.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "Navigation" )]
    public class Navigation
    {
        public static Navigation Empty = new Navigation();

        private Navigation()
            : this( DocumentType.None )
        {
        }

        public Navigation( DocumentType docType, string url )
            : this( docType, new NavigationUrl( UriType.Request, url ) )
        {
        }

        public Navigation( DocumentType docType, params NavigationUrl[] urls )
            : this( docType, urls.ToList() )
        {
        }

        public Navigation( DocumentType docType, IEnumerable<NavigationUrl> urls )
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

        // keep immutable because of stored UrisHashCode
        [DataMember]
        public List<NavigationUrl> Uris { get; private set; }

        [DataMember]
        public int UrisHashCode { get; private set; }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append( "DocType: " );
            sb.AppendLine( DocumentType.ToString() );

            foreach( var uri in Uris )
            {
                sb.AppendLine( uri.ToString() );
            }

            return sb.ToString();
        }
    }
}
