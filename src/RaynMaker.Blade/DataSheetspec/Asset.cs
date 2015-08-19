using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DataContract( Name = "Asset", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [DefaultProperty( "Data" ), ContentProperty( "Data" )]
    [KnownType( typeof( Series ) )]
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
