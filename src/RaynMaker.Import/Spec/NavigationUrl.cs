using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Keep Immutable!!
    /// Because of transformation from Navigation object
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "NavigatorUrl" )]
    [KnownType( typeof( NavigationUrl[] ) )]
    public class NavigationUrl
    {
        private Uri myUrl = null;

        public NavigationUrl( UriType type, Uri url )
            : this( type, url.ToString() )
        {
            myUrl = url;
        }

        public NavigationUrl( UriType type, string url )
            : this( type, url, null )
        {
        }

        public NavigationUrl( Formular form )
            : this( UriType.SubmitFormular, string.Empty, form )
        {
        }

        public NavigationUrl( UriType uriType, string url, Formular form )
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

        public static NavigationUrl Parse( string str )
        {
            int pos = str.IndexOf( ':' );
            if( pos < 0 )
            {
                throw new ArgumentException( "Invalid string syntax: " + str );
            }

            var type = ( UriType )Enum.Parse( typeof( UriType ), str.Substring( 0, pos ).Trim(), true );
            return new NavigationUrl( type, str.Substring( pos + 1 ).Trim() );
        }

        public static NavigationUrl Request( string url )
        {
            return new NavigationUrl( UriType.Request, url );
        }

        public static NavigationUrl Response( string url )
        {
            return new NavigationUrl( UriType.Response, url );
        }

        public static NavigationUrl SubmitFormular( Formular form )
        {
            return new NavigationUrl( form );
        }

        // required also for be addable to Exception.Data
        public override string ToString()
        {
            return UrlType + ": " + UrlString;
        }
    }
}
