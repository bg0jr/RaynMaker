using System;
using Plainion;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public class DataProviderRequest
    {
        public DataProviderRequest( Stock stock, Type datumType, IPeriod from, IPeriod to )
        {
            Contract.RequiresNotNull( stock, "stock" );
            Contract.RequiresNotNull( datumType, "datumType" );
            Contract.RequiresNotNull( from, "from" );
            Contract.RequiresNotNull( to, "to" );

            Stock = stock;
            DatumType = datumType;
            From = from;
            To = to;
        }

        public Stock Stock { get; private set; }

        public Type DatumType { get; private set; }

        public IPeriod From { get; private set; }

        public IPeriod To { get; private set; }

        public bool WithPreview { get; set; }

        public bool ThrowOnError { get; set; }
        
        public static DataProviderRequest Create( Stock stock, Type datumType, int fromYear, int toYear )
        {
            return new DataProviderRequest( stock, datumType, new YearPeriod( fromYear ), new YearPeriod( toYear ) );
        }

        public static DataProviderRequest Create( Stock stock, Type datumType, DateTime fromDay, DateTime toDay)
        {
            return new DataProviderRequest( stock, datumType, new DayPeriod( fromDay ), new DayPeriod( toDay ) );
        }
    }
}
