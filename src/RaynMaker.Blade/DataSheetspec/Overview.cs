using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Overview
    {
        public Overview()
        {
            References = new List<Reference>();
        }

        [Required]
        public string Homepage { get; set; }

        public string Sector { get; set; }
        
        public string Origin { get; set; }
    
        [ValidateObject]
        public List<Reference> References { get; private set; }
    }
}
