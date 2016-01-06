﻿using System;
using System.Collections.Generic;
using System.Data;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToSeriesConverter : IDataTableToEntityConverter
    {
        private SeriesDescriptorBase myDescriptor;
        private Type myEntityType;
        private string mySource;

        public DataTableToSeriesConverter( SeriesDescriptorBase descriptor, Type entityType, string source )
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
            var series = new List<IDatum>();

            foreach( DataRow row in table.Rows )
            {
                if( row[ myDescriptor.ValueFormat.Name ] == DBNull.Value )
                {
                    continue;
                }

                IPeriod period;
                if( myDescriptor.TimeFormat != null )
                {
                    if( myDescriptor.TimeFormat.Type == typeof( int ) )
                    {
                        var year = ( int )row[ myDescriptor.TimeFormat.Name ];
                        period = new YearPeriod( year );
                    }
                    else
                    {
                        var date = ( DateTime )row[ myDescriptor.TimeFormat.Name ];
                        period = new DayPeriod( date );
                    }
                }
                else
                {
                    // TODO: is this a proper default?
                    period = new DayPeriod( DateTime.Now );
                }

                var datum = Dynamics.CreateDatum( stock, myEntityType, period, null );
                datum.Source = mySource;
                datum.Value = ( double )row[ myDescriptor.ValueFormat.Name ];;

                series.Add( datum );
            }

            return series;
        }
    }
}
