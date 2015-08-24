using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "DataSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Stock ) )]
    public class DataSheet
    {
        public DataSheet()
        {
            Data = new ObservableCollection<IDatumSeries>();
        }

        [DataMember]
        [Required, ValidateObject]
        public Company Company { get; set; }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }
    }
}
