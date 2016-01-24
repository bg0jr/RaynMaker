using System;
using Plainion;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public class DataProviderRequest
    {
        public DataProviderRequest( Stock stock, Type figureType, IPeriod from, IPeriod to )
        {
            Contract.RequiresNotNull( stock, "stock" );
            Contract.RequiresNotNull( figureType, "figureType" );
            Contract.RequiresNotNull( from, "from" );
            Contract.RequiresNotNull( to, "to" );

            Stock = stock;
            FigureType = figureType;
            From = from;
            To = to;
        }

        public Stock Stock { get; private set; }

        public Type FigureType { get; private set; }

        public IPeriod From { get; private set; }

        public IPeriod To { get; private set; }

        public bool WithPreview { get; set; }

        public bool ThrowOnError { get; set; }
        
        public static DataProviderRequest Create( Stock stock, Type figureType, int fromYear, int toYear )
        {
            return new DataProviderRequest( stock, figureType, new YearPeriod( fromYear ), new YearPeriod( toYear ) );
        }

        public static DataProviderRequest Create( Stock stock, Type figureType, DateTime fromDay, DateTime toDay)
        {
            return new DataProviderRequest( stock, figureType, new DayPeriod( fromDay ), new DayPeriod( toDay ) );
        }
    }
}
