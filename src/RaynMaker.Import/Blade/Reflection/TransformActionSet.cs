using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blade.Collections;

namespace Blade.Reflection
{
    /// <summary />
    public static class TransformActionSet
    {
        /// <summary />
        public static T ApplyTo<T>( this TransformAction[] rules, Expression<Func<object>> method )
        {
            TransformAction transformAction = null;
            if( rules != null )
            {
                transformAction = rules.FirstOrDefault( ( TransformAction rule ) => rule.Matcher.Matches( method ) );
            }
            return ( T )( ( object )( ( transformAction != null ) ? transformAction.Value : TransformActionSet.ApplyDefaultTo( rules, method ) ) );
        }
        private static object ApplyDefaultTo( TransformAction[] rules, Expression<Func<object>> method )
        {
            Type returnType = Reflector.GetMethodInfo( method ).ReturnType;
            object obj = method.Compile()();
            if( returnType == typeof( string ) || returnType.IsValueType )
            {
                return obj;
            }
            if( returnType.GetInterface( "IronF.Lists.IArray`1" ) != null || returnType.Name == "IArray`1" )
            {
                Type type = typeof( ActiveList<> ).MakeGenericType( returnType.GetGenericArguments() );
                return Activator.CreateInstance( type, new object[]
				{
					obj
				} );
            }
            if( returnType.GetInterface( "System.Collections.IList`1" ) != null || returnType.Name == "IList`1" || returnType.GetInterface( "System.Collections.IEnumerable`1" ) != null || returnType.Name == "IEnumerable`1" )
            {
                Type type2 = typeof( List<> ).MakeGenericType( returnType.GetGenericArguments() );
                return Activator.CreateInstance( type2, new object[]
				{
					obj
				} );
            }
            if( returnType.GetInterface( "System.Collections.IEnumerable" ) != null )
            {
                return Activator.CreateInstance( returnType, new object[]
				{
					obj
				} );
            }
            return Activator.CreateInstance( returnType, new object[]
			{
				Convert.ChangeType(obj, returnType),
				rules
			} );
        }
    }
}
