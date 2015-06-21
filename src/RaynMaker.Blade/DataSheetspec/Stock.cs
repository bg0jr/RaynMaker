using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Data" ), ContentProperty( "Data" )]
    public class Stock : Asset
    {
        [Required]
        public string Isin { get; set; }

        [Required, ValidateObject]
        public Overview Overview { get; set; }
    }
}
