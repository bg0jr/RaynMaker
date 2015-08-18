using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Markup;
using Plainion.Validation;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Data" ), ContentProperty( "Data" )]
    public abstract class Asset 
    {
        public Asset()
        {
            Data = new ObservableCollection<IDatumSeries>();
        }

        [Required]
        public string Name { get; set; }
    
        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }

        public IDatumSeries SeriesOf( Type datumType )
        {
            return Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == datumType )
                .SingleOrDefault();
        }
    }
}
