using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "DataSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( DatumSeries ) )]
    public class DataSheet
    {
        public DataSheet()
        {
            Data = new ObservableCollection<IDatumSeries>();
        }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }
    }
}
