using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Overview
    {
        public Overview()
        {
            References = new ObservableCollection<Reference>();
        }

        [Required]
        public string Homepage { get; set; }

        public string Sector { get; set; }
        
        public string Origin { get; set; }
    
        [ValidateObject]
        public ObservableCollection<Reference> References { get; private set; }
    }
}
