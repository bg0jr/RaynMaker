using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Plainion;

namespace RaynMaker.Import.Spec.v2.Locating
{
    /// <summary>
    /// Describes location of a document. 
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DocumentLocator" )]
    public class DocumentLocator
    {
        public static DocumentLocator Empty = new DocumentLocator();

        public DocumentLocator( string url )
            : this( new DocumentLocationFragment( UriType.Request, url ) )
        {
        }

        public DocumentLocator( params DocumentLocationFragment[] urls )
            : this( urls.ToList() )
        {
        }

        public DocumentLocator( IEnumerable<DocumentLocationFragment> urls )
        {
            Contract.RequiresNotNullNotEmpty( urls, "urls" );

            Uris = urls.ToList();

            UrisHashCode = CreateUrisHashCode();
        }

        private int CreateUrisHashCode()
        {
            var uriStrings = Uris.Select( uri => uri.UrlString ).ToArray();
            var hashCodeString = string.Join( string.Empty, uriStrings );

            return hashCodeString.GetHashCode();
        }

        // keep immutable because of stored UrisHashCode
        [DataMember]
        public IReadOnlyList<DocumentLocationFragment> Uris { get; private set; }

        [DataMember]
        public int UrisHashCode { get; private set; }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach( var uri in Uris )
            {
                sb.AppendLine( uri.ToString() );
            }

            return sb.ToString();
        }
    }
}
