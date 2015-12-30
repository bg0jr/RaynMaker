using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Import.Spec.v2.Locating
{
    /// <summary>
    /// One fragment required to locate a document (e.g. a request or response URL). 
    /// Might be a template which needs to be evaluated with a specific stock.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "LocatingFragment" )]
    [KnownType( typeof( LocatingFragment[] ) )]
    public class LocatingFragment
    {
        private Uri myUrl = null;

        public LocatingFragment( UriType type, Uri url )
            : this( type, url.ToString() )
        {
            myUrl = url;
        }

        public LocatingFragment( UriType type, string url )
            : this( type, url, null )
        {
        }

        public LocatingFragment( Formular form )
            : this( UriType.SubmitFormular, string.Empty, form )
        {
        }

        public LocatingFragment( UriType uriType, string url, Formular form )
        {
            Contract.Requires( uriType != UriType.None, "uriType must NOT be None" );

            UrlType = uriType;
            UrlString = url;
            Formular = form;
        }

        [DataMember]
        public UriType UrlType { get; private set; }

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

        [DataMember]
        public Formular Formular { get; private set; }

        public static LocatingFragment Parse( string str )
        {
            int pos = str.IndexOf( ':' );
            if( pos < 0 )
            {
                throw new ArgumentException( "Invalid string syntax: " + str );
            }

            var type = ( UriType )Enum.Parse( typeof( UriType ), str.Substring( 0, pos ).Trim(), true );
            return new LocatingFragment( type, str.Substring( pos + 1 ).Trim() );
        }

        public static LocatingFragment Request( string url )
        {
            return new LocatingFragment( UriType.Request, url );
        }

        public static LocatingFragment Response( string url )
        {
            return new LocatingFragment( UriType.Response, url );
        }

        public static LocatingFragment SubmitFormular( Formular form )
        {
            return new LocatingFragment( form );
        }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            return UrlType + ": " + UrlString;
        }
    }
}
