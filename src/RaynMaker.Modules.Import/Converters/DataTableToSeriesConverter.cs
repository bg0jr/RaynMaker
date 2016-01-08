using System;
using System.Collections.Generic;
using System.Data;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToSeriesConverter : AbstractDataTableToEntityConverter<SeriesDescriptorBase>
    {
        public DataTableToSeriesConverter( SeriesDescriptorBase descriptor, Type entityType, string source, IEnumerable<Currency> currencies )
            :base(descriptor,entityType,source,currencies)
        {
        }

        public override IEnumerable<IDatum> Convert( DataTable table, Stock stock )
        {
            var series = new List<IDatum>();

            foreach( DataRow row in table.Rows )
            {
                if( row[ Descriptor.ValueFormat.Name ] == DBNull.Value )
                {
                    continue;
                }

                IPeriod period;
                if( Descriptor.TimeFormat != null )
                {
                    if( Descriptor.TimeFormat.Type == typeof( int ) )
                    {
                        var year = ( int )row[ Descriptor.TimeFormat.Name ];
                        period = new YearPeriod( year );
                    }
                    else
                    {
                        var date = (DateTime)row[ Descriptor.TimeFormat.Name ];
                        period = new DayPeriod( date );
                    }
                }
                else
                {
                    // TODO: is this a proper default?
                    period = new DayPeriod( DateTime.Now );
                }

                var entity = CreateEntity( stock, period, row[ Descriptor.ValueFormat.Name ] );
                series.Add( entity );
            }

            return series;
        }
    }
}
