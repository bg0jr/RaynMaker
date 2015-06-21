using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    [TypeConverter( typeof( CurrencyConverter ) )]
    public class Currency
    {
        public Currency( string name )
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}
