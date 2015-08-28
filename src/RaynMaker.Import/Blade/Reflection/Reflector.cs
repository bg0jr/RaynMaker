using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Reflection
{
    /// <summary />
    public static class Reflector
    {
        /// <summary />
        public static MethodInfo GetMethodInfo( Expression<Func<object>> method )
        {
            if( method == null )
            {
                throw new ArgumentNullException( "method" );
            }
            Expression expression = method.Body;
            if( expression.NodeType == ExpressionType.Convert )
            {
                expression = ( ( UnaryExpression )method.Body ).Operand;
            }
            MethodCallExpression methodCallExpression = null;
            if( expression.NodeType == ExpressionType.Call )
            {
                methodCallExpression = ( expression as MethodCallExpression );
            }
            else
            {
                if( expression.NodeType == ExpressionType.MemberAccess )
                {
                    return ( ( PropertyInfo )( ( MemberExpression )expression ).Member ).GetGetMethod();
                }
            }
            if( methodCallExpression == null )
            {
                throw new ArgumentException( "method", method.ToString() );
            }
            return methodCallExpression.Method;
        }
        /// <summary />
        public static bool Is( this MethodInfo mi, Expression<Func<object>> method )
        {
            return Reflector.GetMethodInfo( method ) == mi;
        }
        /// <summary />
        public static bool IsCollection( this Type type )
        {
            return !( type == typeof( string ) ) && ( type.IsArray || type.Implements( typeof( IEnumerable ) ) );
        }
        /// <summary />
        public static bool Implements( this Type type, Type iface )
        {
            return type.GetInterfaces().Any( ( Type t ) => t == iface );
        }
        /// <summary />
        public static IEnumerable<T> GetAttributes<T>( this MemberInfo member, bool inherit )
        {
            return member.GetCustomAttributes( typeof( T ), inherit ).OfType<T>().ToList<T>();
        }
        /// <summary />
        public static T GetAttribute<T>( this MemberInfo member, bool inherit )
        {
            return member.GetAttributes<T>( inherit ).SingleOrDefault<T>();
        }
        /// <summary />
        public static object CreateGeneric( this Type generic, Type innerType, params object[] args )
        {
            Type type = generic.MakeGenericType( new Type[]
			{
				innerType
			} );
            return Activator.CreateInstance( type, args );
        }
    }
}
