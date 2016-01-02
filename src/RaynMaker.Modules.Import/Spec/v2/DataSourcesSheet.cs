using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DataSources" )]
    public class DataSourcesSheet
    {
        [Required, ValidateObject]
        [DataMember]
        public IEnumerable<DataSource> Sources { get; set; }
    }
}
