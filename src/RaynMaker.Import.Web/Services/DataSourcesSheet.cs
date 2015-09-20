using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Import.Spec;

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
