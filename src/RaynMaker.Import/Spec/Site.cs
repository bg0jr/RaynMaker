using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "Site" )]
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

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Navigation Navigation { get; set; }

        [DataMember]
        public IList<IFormat> Formats { get; private set; }
    }
}
