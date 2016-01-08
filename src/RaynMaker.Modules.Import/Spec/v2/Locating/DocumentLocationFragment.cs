using System;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Locating
{
    /// <summary>
    /// One fragment required to locate a document (e.g. a request or response URL). 
    /// Might be a template which needs to be evaluated with a specific stock.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DocumentLocationFragment" )]
    [KnownType( typeof( DocumentLocationFragment[] ) )]
    public abstract class DocumentLocationFragment : SerializableBindableBase
    {
        private string myUrlString;
        private Uri myUrl;

        protected DocumentLocationFragment( Uri url )
        {
            Contract.RequiresNotNull( url, "url" );

            myUrl = url;
            UrlString = url.ToString();
        }

        protected DocumentLocationFragment( string url )
        {
            Contract.RequiresNotNullNotEmpty( url, "url" );

            UrlString = url;
        }

        [DataMember( Name = "Url" )]
        public string UrlString
        {
            get { return myUrlString; }
            set
            {
                Contract.RequiresNotNull( value, "value" );
                if ( SetProperty( ref myUrlString, value ) )
                {
                    Url = null;
                }
            }
        }

        public Uri Url
        {
            get
            {
                // gets evaluated the first time this property is called
                // this way we allow partly-evaluated/partly-constructed url strings
                if ( myUrl == null )
                {
                    myUrl = new Uri( UrlString );
                }

                return myUrl;
            }
            private set { SetProperty( ref myUrl, value ); }
        }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            return GetType().Name + ": " + UrlString;
        }
    }
}
