using System;
using System.Runtime.Serialization;
using Blade;
using Plainion;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Keep Immutable!!
    /// Because of transformation from Navigation object
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "NavigatorUrl" )]
    public class NavigatorUrl
    {
        private Uri myUrl = null;

        public NavigatorUrl( UriType type, Uri url )
            : this( type, url.ToString() )
        {
            myUrl = url;
        }

        public NavigatorUrl( UriType type, string url )
            : this( type, url, null )
        {
        }

        public NavigatorUrl( Formular form )
            : this( UriType.SubmitFormular, string.Empty, form )
        {
        }

        public NavigatorUrl( UriType uriType, string url, Formular form )
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

        public override string ToString()
        {
            // dont return the Url property here because it may still be a
            // template and so not a valid url
            return string.Format( "{0}: {1}", UrlType, UrlString );
        }

        public static NavigatorUrl Parse( string str )
        {
            int pos = str.IndexOf( ':' );
            if( pos < 0 )
            {
                throw new ArgumentException( "Invalid string syntax: " + str );
            }

            var type = ( UriType )Enum.Parse( typeof( UriType ), str.Substring( 0, pos ).Trim(), true );
            return new NavigatorUrl( type, str.Substring( pos + 1 ).Trim() );
        }

        public static NavigatorUrl Request( string url )
        {
            return new NavigatorUrl( UriType.Request, url );
        }

        public static NavigatorUrl Response( string url )
        {
            return new NavigatorUrl( UriType.Response, url );
        }

        public static NavigatorUrl SubmitFormular( Formular form )
        {
            return new NavigatorUrl( form );
        }
    }
}
