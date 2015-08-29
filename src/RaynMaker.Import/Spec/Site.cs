using Blade;
using Blade.Reflection;

namespace RaynMaker.Import.Spec
{
    public class Site : INamedObject
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

        public Site( Site site, params TransformAction[] rules )
        {
            Name = rules.ApplyTo<string>( () => site.Name );
            Navigation = rules.ApplyTo<Navigation>( () => site.Navigation );
            Format = rules.ApplyTo<IFormat>( () => site.Format );
            Content = rules.ApplyTo<DataContent>( () => site.Content );
        }

        public string Name { get; private set; }

        public Navigation Navigation { get; set; }

        public IFormat Format { get; set; }

        public DataContent Content { get; set; }
    }
}
