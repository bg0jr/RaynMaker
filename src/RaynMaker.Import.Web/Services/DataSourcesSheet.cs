using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;

namespace RaynMaker.Import.Web.Services
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSources" )]
    class DataSourcesSheet
    {
        [Required, ValidateObject]
        [DataMember]
        public IEnumerable<DataSource> Sources { get; set; }
    }
}
