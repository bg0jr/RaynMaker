using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes location of a document. 
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DocumentLocator" )]
    public class DocumentLocator
    {
        public static DocumentLocator Empty = new DocumentLocator();

        private DocumentLocator()
            : this( DocumentType.None )
        {
        }

        public DocumentLocator( DocumentType docType, string url )
            : this( docType, new LocatingFragment( UriType.Request, url ) )
        {
        }

        public DocumentLocator( DocumentType docType, params LocatingFragment[] urls )
            : this( docType, urls.ToList() )
        {
        }

        public DocumentLocator( DocumentType docType, IEnumerable<LocatingFragment> urls )
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
        public List<LocatingFragment> Uris { get; private set; }

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
