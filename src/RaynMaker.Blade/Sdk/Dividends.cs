using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using Plainion.Validation;

namespace RaynMaker.Blade.Sdk
{
    [DefaultProperty( "Values" ), ContentProperty( "Values" )]
    public class Dividends
    {
        public Dividends()
        {
            Values = new List<Dividend>();
        }

        [Required,ValidateObject]
        public List<Dividend> Values { get; private set; }
  }
}
