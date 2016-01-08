using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a figure within one cell of a table.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathCellDescriptor" )]
    public class PathCellDescriptor : SingleValueDescriptorBase, IPathDescriptor, ICurrencyDescriptor
    {
        private string myPath;
        private ISeriesLocator myColumn;
        private ISeriesLocator myRow;
        private string myCurrency;

        /// <summary>
        /// Gets or sets the path within the document to the table. 
        /// The path must point to the table itself not to any cell within the table.
        /// </summary>
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Path
        {
            get { return myPath; }
            set { SetProperty( ref myPath, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public ISeriesLocator Column
        {
            get { return myColumn; }
            set { SetProperty( ref myColumn, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public ISeriesLocator Row
        {
            get { return myRow; }
            set { SetProperty( ref myRow, value ); }
        }

        [DataMember]
        public string Currency
        {
            get { return myCurrency; }
            set { SetProperty( ref myCurrency, value ); }
        }
    }
}
