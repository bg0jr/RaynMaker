using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most table describing descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "TableDescriptorBase" )]
    public abstract class TableDescriptorBase : FigureDescriptorBase
    {
        protected TableDescriptorBase()
        {
            Columns = new List<FormatColumn>();
            SkipColumns = new List<int>();
            SkipRows = new List<int>();
        }

        [CollectionNotEmpty, ValidateObject]
        [DataMember]
        public IList<FormatColumn> Columns { get; private set; }

        [DataMember]
        public IList<int> SkipRows { get; private set; }

        [DataMember]
        public IList<int> SkipColumns { get; private set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            Columns = Columns.ToList();
            SkipRows = SkipRows.ToList();
            SkipColumns = SkipColumns.ToList();
        }
    }
}
