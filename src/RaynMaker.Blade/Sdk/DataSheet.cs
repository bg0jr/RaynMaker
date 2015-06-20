using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.Sdk
{
    [DefaultProperty( "Asset" ), ContentProperty( "Asset" )]
    public class DataSheet : DataTemplate
    {
        [Required,ValidateObject]
        public Asset Asset { get;  set; }
    }
}
