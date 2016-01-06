using System.Collections.Generic;
using System.Data;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Import
{
    public interface IDataTableToEntityConverter
    {
        IEnumerable<IDatum> Convert( DataTable table, Stock stock );
    }
}
