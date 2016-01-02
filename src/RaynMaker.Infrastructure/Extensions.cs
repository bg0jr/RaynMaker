using System.Collections.Generic;

namespace RaynMaker
{
    /// <summary>
    /// Collects varios extension methods used in all modules
    /// </summary>
    public static class Extensions
    {
        public static void AddRange<T>( this IList<T> self, params T[] items )
        {
            foreach( var item in items )
            {
                self.Add( item );
            }
        }
    }
}
