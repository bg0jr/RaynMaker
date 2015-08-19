using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Asset" ), ContentProperty( "Asset" )]
    public class DataSheet : DataTemplate, IFreezable
    {
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
