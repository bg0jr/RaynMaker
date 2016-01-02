using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most series describing descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "SeriesDescriptorBase" )]
    public abstract class SeriesDescriptorBase : FigureDescriptorBase
    {
        private SeriesOrientation myOrientation;
        private ISeriesLocator myValuesLocator;
        private FormatColumn myValueFormat;
        private ISeriesLocator myTimesLocator;
        private FormatColumn myTimeFormat;
        
        protected SeriesDescriptorBase()
        {
            Excludes = new List<int>();
        }

        [DataMember]
        public SeriesOrientation Orientation
                {
                    get { return myOrientation; }
                    set { SetProperty( ref myOrientation, value ); }
        }

        /// <summary>
        /// Describes the position of the values within a table.
        /// Orientation of the values series is described by the Orientation property.
        /// </summary>
        [Required, ValidateObject]
        [DataMember]
        public ISeriesLocator ValuesLocator 
        {
            get { return myValuesLocator; }
            set { SetProperty( ref myValuesLocator, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public FormatColumn ValueFormat 
        {
            get { return myValueFormat; }
            set { SetProperty( ref myValueFormat, value ); }
        }

        /// <summary>
        /// Describes the position of the times within a table.
        /// Orientation of the times series is described by the Orientation property.
        /// </summary>
        [ValidateObject]
        [DataMember]
        public ISeriesLocator TimesLocator 
        {
            get { return myTimesLocator; }
            set { SetProperty( ref myTimesLocator, value ); }
        }

        [ValidateObject]
        [DataMember]
        public FormatColumn TimeFormat 
        {
            get { return myTimeFormat; }
            set { SetProperty( ref myTimeFormat, value ); }
        }

        [DataMember]
        public IList<int> Excludes { get; private set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            Excludes = Excludes.ToList();
        }
    }
}
