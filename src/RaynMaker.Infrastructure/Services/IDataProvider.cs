using System;
using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public interface IDataProvider
    {
        /// <summary>
        /// From and To dates are included in the result series
        /// </summary>
        IEnumerable<IDatum> Get( Stock stock, Type datum, IPeriod from, IPeriod to );
    }
}
