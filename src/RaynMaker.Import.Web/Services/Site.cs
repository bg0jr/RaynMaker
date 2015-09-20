using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.Services
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "Site" )]
     class Site
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

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            Formats = Formats.ToList();
        }
    }
}
