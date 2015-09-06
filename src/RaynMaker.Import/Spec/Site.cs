using System.Collections.Generic;
using System.Linq;

namespace RaynMaker.Import.Spec
{
    public class Site
    {
        public Site( string name )
            : this( name, null )
        {
        }

        public Site( string name, Navigation navi, params IFormat[] formats )
        {
            Name = name;
            Navigation = navi;
            Formats = formats.ToList();
        }

        public string Name { get; set; }

        public Navigation Navigation { get; set; }

        public IList<IFormat> Formats { get; private set; }
    }
}
