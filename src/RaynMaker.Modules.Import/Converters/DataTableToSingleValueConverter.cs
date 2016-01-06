using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToSingleValueConverter : IDataTableToEntityConverter
    {
        private Spec.v2.Extraction.SingleValueDescriptorBase singleValueDescriptorBase;

        public DataTableToSingleValueConverter( Spec.v2.Extraction.SingleValueDescriptorBase singleValueDescriptorBase, Type entityType, string source )
        {
            // TODO: Complete member initialization
            this.singleValueDescriptorBase = singleValueDescriptorBase;
        }

        public IEnumerable<IDatum> Convert( DataTable table, Stock stock )
        {
            throw new NotImplementedException();
        }
    }
}
