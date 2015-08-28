using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blade
{
    /// <summary>
    /// Extensions to System.Type.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the path of the assembly this type has been loaded from.
        /// </summary>
        public static string GetAssemblyPath( this Type type )
        {
            type.Require( ( Type x ) => type != null );
            return new Uri( Path.GetDirectoryName( type.Assembly.CodeBase ) ).LocalPath;
        }
        /// <summary>
        /// Returns the assembly configuration of the assembly this type has been
        /// loaded from.
        /// </summary>
        public static string GetAssemblyConfig( this Type type )
        {
            type.Require( ( Type x ) => type != null );
            return new Uri( type.Assembly.CodeBase ).LocalPath + ".config";
        }
        /// <summary>
        /// Returns the values of all static members of the given type which 
        /// have the specified type.
        /// </summary>
        /// <param name="type">the type the members should be get from</param>
        /// <typeparam name="T">type of the members</typeparam>
        /// <returns>values of the found members</returns>
        public static IEnumerable<T> GetStaticMembers<T>( this Type type )
        {
            IEnumerable<FieldInfo> enumerable =
                from fi in type.GetFields( BindingFlags.Static | BindingFlags.Public )
                where fi.FieldType == typeof( T )
                select fi;
            foreach( FieldInfo current in enumerable )
            {
                yield return ( T )( ( object )current.GetValue( null ) );
            }
            yield break;
        }
        /// <summary>
        /// Returns true if the given type implements or extends the given interface.
        /// </summary>
        public static bool HasInterface( this Type type, Type iface )
        {
            return type.GetInterface( iface.ToString() ) != null;
        }
        /// <summary>
        /// Returns true if the given type has the given attribute applied.
        /// </summary>
        public static bool HasAttribute( this Type type, Type attr )
        {
            return Attribute.GetCustomAttribute( type, attr ) != null;
        }
    }
}
