﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Entities
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
        
        [Required]
        public int Id { get; set; }

        [NotMapped]
        [DataMember]
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

        [NotMapped]
        [DataMember]
        public Overview Overview { get; set; }

        [NotMapped]
        [DataMember]
        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }

        public IDatumSeries SeriesOf( Type datumType )
        {
            return Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == datumType )
                .SingleOrDefault();
        }

        [DataMember]
        [Required]
        public Company Company { get; set; }
    }
}
