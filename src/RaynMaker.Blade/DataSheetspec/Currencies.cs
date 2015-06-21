using System.Linq;
using System.Reflection;
using Plainion;

namespace RaynMaker.Blade.DataSheetSpec
{
    public static class Currencies
    {
        public static readonly Currency Euro = new Currency( "Euro" );
        public static readonly Currency Dollar = new Currency( "Dollar" );

        internal static Currency Parse( string text )
        {
            var value = typeof( Currencies ).GetFields( BindingFlags.Static | BindingFlags.Public )
                .FirstOrDefault( f => f.Name == text );

            Contract.Requires( value != null, "No currency found with name: " + text );

            return (Currency)value.GetValue( null );
        }
    }
}
