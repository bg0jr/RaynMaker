using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToSingleValueConverter : AbstractDataTableToEntityConverter<SingleValueDescriptorBase>
    {
        public DataTableToSingleValueConverter( SingleValueDescriptorBase descriptor, Type entityType, string source, IEnumerable<Currency> currencies )
            : base( descriptor, entityType, source, currencies )
        {
        }

        public override IEnumerable<IDatum> Convert( DataTable table, Stock stock )
        {
            Contract.Requires( !( table.Rows.Count > 1 ), "Cannot convert table with more than one row" );
            Contract.Requires( table.Columns.Count == 1, "Can only convert table with exactly one column" );

            if ( table.Rows.Count == 0 )
            {
                return Enumerable.Empty<IDatum>();
            }

            // TODO: is this a proper default?
            var period = new DayPeriod( DateTime.Now );
            var entity = CreateEntity( stock, period, table.Rows[ 0 ][ 0 ] );

            return new[] { entity };
        }

    }
}
