using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "DataSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Asset ) ), KnownType( typeof( Stock ) )]
    public class DataSheet : IFreezable
    {
        [DataMember]
        [Required, ValidateObject]
        public Asset Asset { get; set; }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            foreach( var freeezable in Asset.Data.OfType<IFreezable>() )
            {
                freeezable.Freeze();
            }

            IsFrozen = true;
        }
    }
}
