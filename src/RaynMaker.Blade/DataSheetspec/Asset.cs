using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Data" ), ContentProperty( "Data" )]
    public abstract class Asset 
    {
        public Asset()
        {
            Data = new List<object>();
        }

        [Required]
        public string Name { get; set; }
    
        [ValidateObject]
        public List<object> Data { get; private set; }
    }
}
