using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v1
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSources" )]
    class DataSourcesSheet
    {
        [Required, ValidateObject]
        [DataMember]
        public IEnumerable<DataSource> Sources { get; set; }
    }
}
