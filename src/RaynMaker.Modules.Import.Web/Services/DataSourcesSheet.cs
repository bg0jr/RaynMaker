using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import.Web.Services
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSources" )]
    class DataSourcesSheet
    {
        [Required, ValidateObject]
        [DataMember]
        public IEnumerable<DataSource> Sources { get; set; }
    }
}
