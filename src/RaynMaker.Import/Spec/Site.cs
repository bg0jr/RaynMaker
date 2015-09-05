using System.Collections.Generic;

namespace RaynMaker.Import.Spec
{
    public class Site
    {
        public Site( string name )
            : this( name, null, null, null )
        {
        }

        public Site( string name, Navigation navi, IFormat format, DataContent content )
        {
            Name = name;
            Navigation = navi;
            Formats = new List<IFormat> { format };
            Content = content;
        }

        public string Name { get; set; }

        public Navigation Navigation { get; set; }

        public IList<IFormat> Formats { get; private set; }

        public DataContent Content { get; set; }
    }
}
