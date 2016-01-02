using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Plainion;

namespace RaynMaker.Modules.Import.Spec.v2.Locating
{
    /// <summary>
    /// Describes location of a document. 
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DocumentLocator" )]
    public class DocumentLocator
    {
        public DocumentLocator( string url )
            : this( new Request( url ) )
        {
        }

        public DocumentLocator( params DocumentLocationFragment[] fragments )
            : this( ( IEnumerable<DocumentLocationFragment> )fragments )
        {
        }

        public DocumentLocator( IEnumerable<DocumentLocationFragment> fragments )
        {
            Contract.RequiresNotNull( fragments, "fragments" );

            Fragments = new ObservableCollection<DocumentLocationFragment>( fragments );
        }

        public int GetFragmentsHashCode()
        {
            var fragmentStrings = Fragments.Select( fragment => fragment.UrlString );
            var hashCodeString = string.Join( string.Empty, fragmentStrings );

            return hashCodeString.GetHashCode();
        }

        [DataMember]
        public IList<DocumentLocationFragment> Fragments { get; private set; }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach( var uri in Fragments )
            {
                sb.AppendLine( uri.ToString() );
            }

            return sb.ToString();
        }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            Fragments = new ObservableCollection<DocumentLocationFragment>( Fragments );
        }
    }
}
