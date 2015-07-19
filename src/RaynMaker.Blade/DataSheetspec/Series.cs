using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Values" ), ContentProperty( "Values" )]
    public class Series : List<IDatum>
    {
        [Required, ValidateObject]
        public List<IDatum> Values { get { return this; } }
    }
}
