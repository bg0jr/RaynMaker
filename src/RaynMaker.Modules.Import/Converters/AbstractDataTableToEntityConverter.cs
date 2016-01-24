using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Converters
{
    abstract class AbstractDataTableToEntityConverter<T> : IDataTableToEntityConverter where T : IFigureDescriptor
    {
        public AbstractDataTableToEntityConverter( T descriptor, Type entityType, string source, IEnumerable<Currency> currencies )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );
            Contract.RequiresNotNull( entityType, "entityType" );
            Contract.RequiresNotNullNotEmpty( source, "source" );
            Contract.RequiresNotNull( currencies, "currencies" );

            Descriptor = descriptor;
            EntityType = entityType;
            Source = source;
            Currencies = currencies;
        }

        protected T Descriptor { get; private set; }

        protected Type EntityType { get; private set; }

        protected string Source { get; private set; }

        protected IEnumerable<Currency> Currencies { get; private set; }

        public abstract IEnumerable<IFigure> Convert( DataTable table, Stock stock );

        protected IFigure CreateEntity( Stock stock, IPeriod period, object value )
        {
            var currency = GetCurrency();

            var datum = Dynamics.CreateFigure( stock, EntityType, period, currency );

            datum.Source = Source;
            datum.Value = (double)value;

            return datum;
        }

        private Currency GetCurrency()
        {
            var currencyDescriptor = Descriptor as ICurrencyDescriptor;
            if ( currencyDescriptor == null )
            {
                return null;
            }

            return Currencies.SingleOrDefault( c => c.Symbol == currencyDescriptor.Currency );
        }
    }
}
