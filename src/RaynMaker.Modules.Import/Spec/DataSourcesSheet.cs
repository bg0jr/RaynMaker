using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using System.Linq;
using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSources" )]
    public class DataSourcesSheet
    {
        [Required, ValidateObject]
        [DataMember( Name = "Sources" )]
        private IEnumerable<object> mySources;

        public IEnumerable<T> GetSources<T>()
        {
            return mySources.Cast<T>();
        }

        public void SetSources( IEnumerable<DataSource> sources )
        {
            mySources = sources;
        }
    }
}
