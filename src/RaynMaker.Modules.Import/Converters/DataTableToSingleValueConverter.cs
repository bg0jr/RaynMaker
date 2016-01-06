using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToSingleValueConverter : IDataTableToEntityConverter
    {
        private SingleValueDescriptorBase myDescriptor;
        private Type myEntityType;
        private string mySource;

        public DataTableToSingleValueConverter( SingleValueDescriptorBase descriptor, Type entityType, string source )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );
            Contract.RequiresNotNull( entityType, "entityType" );
            Contract.RequiresNotNullNotEmpty( source, "source" );

            myDescriptor = descriptor;
            myEntityType = entityType;
            mySource = source;
        }

        public IEnumerable<IDatum> Convert( DataTable table, Stock stock )
        {
            Contract.Requires( !( table.Rows.Count > 1 ), "Cannot convert table with more than one row" );
            Contract.Requires( table.Columns.Count == 1, "Can only convert table with exactly one column" );

            if( table.Rows.Count == 0 )
            {
                return Enumerable.Empty<IDatum>();
            }

            // TODO: is this a proper default?
            var period = new DayPeriod( DateTime.Now );

            var datum = Dynamics.CreateDatum( stock, myEntityType, period, null );
            datum.Source = mySource;
            datum.Value= ( double )table.Rows[ 0 ][ 0 ];;

            return new[] { datum };
        }
    }
}
