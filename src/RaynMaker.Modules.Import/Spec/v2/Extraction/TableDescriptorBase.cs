using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
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
            Columns = new ObservableCollection<FormatColumn>();
            SkipColumns = new ObservableCollection<int>();
            SkipRows = new ObservableCollection<int>();
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
            Columns = new ObservableCollection<FormatColumn>( Columns );
            SkipRows = new ObservableCollection<int>( SkipRows );
            SkipColumns = new ObservableCollection<int>( SkipColumns );
        }
    }
}
