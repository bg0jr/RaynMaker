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


        //public Site( Site site, params Func<object, object>[] transformations )
        //{
        //    Name = Clone<string>( transformations, "Name", () => site.Name );
        //    Navigation = Clone<Navigation>( transformations, "Navigation", site.Navigation );
        //    Format = site.Format;
        //    Content = site.Content;
        //}

        //public static T Clone<T>( Func<object, object>[] transformations, string path, Expression<Func<T>> defaultValue )
        //{
        //    var f = transformations.FirstOrDefault( f => f.Method.GetParameters()[ 0 ].Name == path );
        //    if ( f == null )
        //    {
        //        f = GetDefaultClone( transformations, defaultValue.GetType() );
        //    }

        //    return (T)f( defaultValue.GetType(), transformations );
        //}

        //public static Func<object, object> GetDefaultClone( Type type )
        //{
        //    if ( type == typeof( ValueType ) )
        //    {
        //        return f => f;
        //    }
        //    else
        //    {
        //        return f => Activator.CreateInstance( type, transformations );
        //    }
        //}

        //public Site( Site site )
        //    : this( site.Name, new Navigation( site.Navigation )
        //{
        //    Name = name;
        //    Navigation = navi;
        //    Format = format;
        //    Content = content;
        //}

        public string Name { get; private set; }
        public Navigation Navigation { get; set; }
        public IFormat Format { get; set; }
        public DataContent Content { get; set; }
    }
}
