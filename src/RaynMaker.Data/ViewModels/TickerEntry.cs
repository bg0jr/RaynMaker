using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Entities;

namespace RaynMaker.Data.ViewModels
{
    class TickerEntry
    {
        private Stock myStock;

        public TickerEntry( Stock stock )
        {
            myStock = stock;
        }

        public string Company { get { return myStock.Company.Name; } }

        public string Isin { get { return myStock.Isin; } }
    }
}
