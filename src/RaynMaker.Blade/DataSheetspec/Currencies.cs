using System.Linq;
using Plainion;

namespace RaynMaker.Blade.DataSheetSpec
{
    public static class Currencies
    {
        public static CurrenciesSheet Sheet { get; set; }

        internal static Currency Parse( string name )
        {
            var currency = Sheet.Currencies
                .FirstOrDefault( c => c.Name == name );

            Contract.Requires( currency != null, "No currency found with name: " + name );

            return currency;
        }
    }
}
