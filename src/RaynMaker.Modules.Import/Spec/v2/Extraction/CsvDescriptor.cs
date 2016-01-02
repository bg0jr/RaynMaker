using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes extraction of an entire table from CSV formatted document.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "CsvDescriptor" )]
    public class CsvDescriptor : TableDescriptorBase
    {
        private string mySeparator;

        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Separator
        {
            get { return mySeparator; }
            set { SetProperty( ref mySeparator, value ); }
        }
    }
}
