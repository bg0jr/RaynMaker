using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Plainion;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public abstract class AbstractMacroResolver : ILocatorMacroResolver
    {
        private static Lazy<Regex> myMacroPattern = new Lazy<Regex>( () => new Regex( @"(\$\{.+?\})" ) );

        private IList<string> myUnresolvedMacros;

        protected AbstractMacroResolver()
        {
            myUnresolvedMacros = new List<string>();
        }

        public IEnumerable<string> UnresolvedMacros { get { return myUnresolvedMacros; } }

        public DocumentLocationFragment Resolve( DocumentLocationFragment fragment )
        {
            Contract.RequiresNotNull( fragment, "fragment" );

            myUnresolvedMacros.Clear();

            var matches = myMacroPattern.Value.Matches( fragment.UrlString );
            if( matches.Count == 0 )
            {
                // no macro found
                return fragment;
            }

            var url = fragment.UrlString;
            foreach( Match match in matches )
            {
                var macro = match.Value;
                var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ) );

                if( value == null )
                {
                    myUnresolvedMacros.Add( macro );
                }
                else
                {
                    url = url.Replace( macro, value );
                }
            }

            if( url == fragment.UrlString )
            {
                return fragment;
            }
            else
            {
                return CreateFragment( fragment.GetType(), url );
            }
        }

        /// <summary>
        /// Returns the value for the given MacroId, if this is known to the resolver, null otherwise.
        /// </summary>
        protected abstract string GetMacroValue( string macroId );

        private DocumentLocationFragment CreateFragment( Type type, string url )
        {
            if( type == typeof( Request ) )
            {
                return new Request( url );
            }
            else if( type == typeof( Response ) )
            {
                return new Response( url );
            }
            else
            {
                throw new NotSupportedException( "Unknown fragment type: " + type );
            }
        }

        public int CalculateLocationUID( DocumentLocator locator )
        {
            int hashCode = 0;

            foreach( var fragment in locator.Fragments )
            {
                hashCode = ( hashCode + CalculateLocationUID( fragment ) ) * 251;
            }

            return hashCode;
        }

        private int CalculateLocationUID( DocumentLocationFragment fragment )
        {
            var matches = myMacroPattern.Value.Matches( fragment.UrlString );
            if( matches.Count == 0 )
            {
                // no macro found
                return fragment.UrlString.GetHashCode();
            }

            var url = fragment.UrlString;
            foreach( Match match in matches )
            {
                var macro = match.Value;
                var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ) );

                if( value != null )
                {
                    url = url.Replace( macro, value );
                }
            }
            return url.GetHashCode();
        }
    }
}
