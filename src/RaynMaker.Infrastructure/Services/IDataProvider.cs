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
        void Fetch( Stock stock, Type datum, ICollection<IDatum> series, IPeriod from, IPeriod to );
    }
}
