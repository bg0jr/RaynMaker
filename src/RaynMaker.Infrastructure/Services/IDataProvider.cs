using System;
using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Services
{
    public interface IDataProvider
    {
        bool CanFetch( Type figureType );

        /// <summary>
        /// From and To dates are included in the result series
        /// </summary>
        void Fetch( DataProviderRequest request, ICollection<IFigure> resultContainer );
    }
}
