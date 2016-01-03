using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSources" )]
    public class DataSourcesSheet
    {
        [DataMember( Name = "Sources" )]
        private IEnumerable<v1.DataSource> SourcesV1 { get; set; }

        [Required, ValidateObject]
        [DataMember( Name = "SourcesV2" )]
        private IEnumerable<v2.DataSource> SourcesV2 { get; set; }

        public IEnumerable<T> GetSources<T>()
        {
            if( SourcesV2 != null )
            {
                return SourcesV2.Cast<T>();
            }
            return SourcesV1.Cast<T>();
        }

        public void SetSources( IEnumerable<DataSource> sources )
        {
            SourcesV2 = sources;
        }
    }
}
