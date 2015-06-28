using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class Row
    {
        [Required]
        public string Value { get; set; }

        public bool Round { get; set; }
        public bool InMillions { get; set; }
    }
}
