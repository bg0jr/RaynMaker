using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Asset", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( DatumSeries ) )]
    public abstract class Asset
    {
        public Asset()
        {
            Data = new ObservableCollection<IDatumSeries>();
        }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }

        public IDatumSeries SeriesOf( Type datumType )
        {
            return Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == datumType )
                .SingleOrDefault();
        }
    }
}
