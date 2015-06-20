using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.Sdk
{
    [DefaultProperty( "Data" ), ContentProperty( "Data" )]
    public class Stock : Asset
    {
        public Stock()
        {
            Data = new List<object>();
        }

        [Required]
        public string Isin { get; set; }

        [Required, ValidateObject]
        public Overview Overview { get; set; }

        [ValidateObject]
        public List<object> Data { get; private set; }
    }
}
