using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Modules.Analysis.AnalysisSpec
{
    public class Row
    {
        [Required]
        public string Caption { get; set; }

        [Required]
        public string Value { get; set; }

        public bool Round { get; set; }

        public bool InMillions { get; set; }
    }
}
