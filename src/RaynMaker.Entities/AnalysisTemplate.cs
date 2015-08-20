using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class AnalysisTemplate
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Template { get; set; }
    }
}
