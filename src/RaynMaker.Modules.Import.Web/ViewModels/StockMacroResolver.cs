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
    class StockMacroResolver : AbstractMacroResolver
    {
        private Stock myStock;

        public StockMacroResolver( Stock stock )
        {
            Contract.RequiresNotNull( stock, "stock" );

            myStock = stock;
        }

        protected override string GetMacroValue( string macroId )
        {
            if( macroId.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
            {
                return myStock.Isin;
            }

            if( macroId.Equals( "Wpkn", StringComparison.OrdinalIgnoreCase ) )
            {
                return myStock.Wpkn;
            }

            if( macroId.Equals( "Symbol", StringComparison.OrdinalIgnoreCase ) )
            {
                return myStock.Symbol;
            }

            return null;
        }
    }
}
