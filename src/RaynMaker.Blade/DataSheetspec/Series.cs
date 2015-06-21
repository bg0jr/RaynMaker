using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Values" ), ContentProperty( "Values" )]
    public class Series
    {
        public Series()
        {
            Values = new List<object>();
        }

        [Required,ValidateObject]
        public List<object> Values { get; private set; }
  }
}
