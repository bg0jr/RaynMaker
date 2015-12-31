using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Locating
{
    /// <summary>
    /// One fragment required to locate a document (e.g. a request or response URL). 
    /// Might be a template which needs to be evaluated with a specific stock.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DocumentLocationFragment" )]
    [KnownType( typeof( DocumentLocationFragment[] ) )]
    public abstract class DocumentLocationFragment
    {
        private Uri myUrl = null;

        protected DocumentLocationFragment( Uri url )
        {
            myUrl = url;
            UrlString = url.ToString();
        }

        protected DocumentLocationFragment( string url )
        {
            UrlString = url;
        }

        [DataMember( Name = "Url" )]
        public string UrlString { get; set; }

        public Uri Url
        {
            get
            {
                // gets evaluated the first time this property is called
                // this way we allow partly-evaluated/partly-constructed url strings
                if( myUrl == null )
                {
                    myUrl = new Uri( UrlString );
                }

                return myUrl;
            }
        }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            return GetType().Name + ": " + UrlString;
        }
    }
}
