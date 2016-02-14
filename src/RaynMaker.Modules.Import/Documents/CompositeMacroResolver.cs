using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    /// <summary>
    /// Calls Resolve() on all resolvers in the order they are passed to the constructor
    /// </summary>
    public class CompositeMacroResolver : ILocatorMacroResolver
    {
        private ILocatorMacroResolver[] myResolvers;

        public CompositeMacroResolver( params ILocatorMacroResolver[] resolvers )
        {
            myResolvers = resolvers;
        }

        public IEnumerable<string> UnresolvedMacros {get;private set;}

        public DocumentLocationFragment Resolve( DocumentLocationFragment fragment )
        {
            var current = fragment;

            UnresolvedMacros = Enumerable.Empty<string>();

            foreach( var resolver in myResolvers )
            {
                current = resolver.Resolve( current );
                UnresolvedMacros = resolver.UnresolvedMacros;
            }

            return current;
        }

        public int CalculateLocationUID( DocumentLocator locator )
        {
            int hashCode = 0;
            
            foreach( var resolver in myResolvers )
            {
                hashCode = ( hashCode + resolver.CalculateLocationUID( locator ) ) * 251;
            }

            return hashCode;
        }
    }
}
