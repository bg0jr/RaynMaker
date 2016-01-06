using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class StockMacroResolver : ILocatorMacroResolver
    {
        private static Lazy<Regex> myMacroPattern = new Lazy<Regex>( () => new Regex( @"(\$\{.*\})" ) );
        private Stock myStock;

        public StockMacroResolver( Stock stock )
        {
            Contract.RequiresNotNull( stock, "stock" );

            myStock = stock;
        }

        public DocumentLocationFragment Resolve( DocumentLocationFragment fragment )
        {
            Contract.RequiresNotNull( fragment, "fragment" );

            var md = myMacroPattern.Value.Match( fragment.UrlString );
            if( !md.Success )
            {
                // no macro found
                return fragment;
            }

            var macro = md.Groups[ 1 ].Value;
            var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ), myStock );

            Contract.Invariant( value != null, "Macro resolution failed for macro '{0}',macro" );

            return CreateFragment( fragment.GetType(), fragment.UrlString.Replace( macro, value ) );
        }

        private string GetMacroValue( string macroId, Stock stock )
        {
            if( macroId.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Isin;
            }

            if( macroId.Equals( "Wpkn", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Wpkn;
            }

            if( macroId.Equals( "Symbol", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Symbol;
            }

            throw new NotSupportedException( "Unknown macro: " + macroId );
        }

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
    }
}
