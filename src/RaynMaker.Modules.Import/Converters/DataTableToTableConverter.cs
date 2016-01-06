using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Import.Converters
{
    class DataTableToTableConverter : IDataTableToEntityConverter
    {
        private Spec.v2.Extraction.TableDescriptorBase tableDescriptorBase;

        public DataTableToTableConverter( Spec.v2.Extraction.TableDescriptorBase tableDescriptorBase, Type entityType, string source )
        {
            // TODO: Complete member initialization
            this.tableDescriptorBase = tableDescriptorBase;
        }

        public IEnumerable<IDatum> Convert( DataTable table, Stock stock )
        {
            throw new NotImplementedException();
        }
    }
}
