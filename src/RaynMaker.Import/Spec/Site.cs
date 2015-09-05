
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
            Format = format;
            Content = content;
        }

        public string Name { get; set; }

        public Navigation Navigation { get; set; }

        public IFormat Format { get; set; }

        public DataContent Content { get; set; }
    }
}
