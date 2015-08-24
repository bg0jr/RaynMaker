using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Stock", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Overview ) ), KnownType( typeof( DatumSeries ) )]
    public class Stock : SerializableBindableBase
    {
        private string myName;
        private string myIsin;

        public Stock()
        {
            Overview = new Overview();
            Data = new ObservableCollection<IDatumSeries>();
        }

        [DataMember]
        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [DataMember]
        [Required]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value ); }
        }

        [DataMember]
        [Required, ValidateObject]
        public Overview Overview { get; private set; }

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
