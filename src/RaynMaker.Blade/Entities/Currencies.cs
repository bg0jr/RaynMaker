using System.Linq;
using Plainion;

namespace RaynMaker.Blade.Entities
{
    public static class Currencies
    {
        public static CurrenciesSheet Sheet { get; set; }

        internal static Currency Parse( string name )
        {
            Contract.Invariant( Sheet != null, "Currencies sheet not yet initialized" );

            var currency = Sheet.Currencies
                .FirstOrDefault( c => c.Name == name );

            Contract.Requires( currency != null, "No currency found with name: " + name );

            return currency;
        }
    }
}
