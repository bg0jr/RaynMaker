using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Blade;
using Blade.Collections;
using Blade.Reflection;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    /// <summary>
    /// Implements a fetch policy which applies a Lookup-and-Replace
    /// algorithm to the Navigation urls and to all strings of the format.
    /// <remarks>
    /// This policy can use a lookup table or a lookup function to do the lookup.
    /// A preprocessing is not done.
    /// </remarks>
    /// </summary>
    public class LookupPolicy : IFetchPolicy
    {
        public LookupPolicy()
            : this( new Dictionary<string, string>() )
        {
        }

        public LookupPolicy( IDictionary<string, string> lut )
        {
            Lookup = LookupTable;
            Lut = lut;
        }

        public LookupPolicy( Func<string, string> lookup )
        {
            Lut = null;
            Lookup = lookup;
        }

        /// <summary>
        /// The lookup table to use if the lookup function is not set.
        /// </summary>
        public IDictionary<string, string> Lut { get; private set; }

        /// <summary>
        /// The lookup function to use if the lookup table is not set.
        /// </summary>
        public Func<string, string> Lookup { get; private set; }

        public Navigation GetNavigation( Site site )
        {
            return new Navigation( site.Navigation.DocumentType, Update( site.Navigation.Uris ) );
        }

        public IFormat GetFormat( Site site )
        {
            var newFormat = site.Format.Clone();

            // XXX: find a better way!!
            //      go for transformation rules
            foreach( var pi in newFormat.GetType().GetProperties() )
            {
                if( !pi.CanWrite )
                {
                    continue;
                }

                if( pi.PropertyType == typeof( string ) )
                {
                    pi.SetValue( newFormat, LookupInternal( ( string )pi.GetValue( newFormat, null ) ), null );
                }
                else if( pi.PropertyType == typeof( Anchor ) )
                {
                    // xxx: hack to get string replacement in anchors running
                    var anchor = ( Anchor )pi.GetValue( newFormat, null );
                    if( anchor != null )
                    {
                        var colLocator = anchor.Column;
                        var rowLocator = anchor.Row;
                        var locator = colLocator as StringContainsLocator;
                        if( locator != null )
                        {
                            colLocator = new StringContainsLocator( locator.SeriesToScan, LookupInternal( locator.Pattern ) );
                        }
                        locator = rowLocator as StringContainsLocator;
                        if( locator != null )
                        {
                            rowLocator = new StringContainsLocator( locator.SeriesToScan, LookupInternal( locator.Pattern ) );
                        }
                        pi.SetValue( newFormat, Anchor.ForCell( rowLocator, colLocator ), null );
                    }
                }
                else if( pi.PropertyType.HasInterface( typeof( IEnumerable ) ) )
                {
                    var list = new List<object>();
                    bool changed = false;
                    foreach( var obj in ( IEnumerable )pi.GetValue( newFormat, null ) )
                    {
                        if( obj.GetType() == typeof( string ) )
                        {
                            list.Add( LookupInternal( ( string )obj ) );
                            changed = true;
                        }
                        else
                        {
                            list.Add( obj );
                        }
                    }

                    if( changed )
                    {
                        if( pi.PropertyType.IsArray )
                        {
                            throw new NotSupportedException( "Unable to handle properties of type System.Array" );
                        }

                        // XXX: the trick is how to find the proper instance
                        //      or how to find the proper constructor
                        var newObj = Activator.CreateInstance( pi.PropertyType, list.ToArray() );
                        pi.SetValue( newFormat, newObj, null );
                    }
                }
            }

            return newFormat;
        }

        public DataTable ApplyPreprocessing( DataTable table )
        {
            return table;
        }

        /// <summary>
        /// Provide a possibility for derived classes to change the 
        /// lookup order or behaviour.
        /// </summary>
        protected virtual string LookupInternal( string str )
        {
            return Lookup( str );
        }

        private IArray<NavigatorUrl> Update( IArray<NavigatorUrl> urls )
        {
            return urls.Select( url => TransformNavigatorUrl( url ) ).ToArrayList();
        }

        private NavigatorUrl TransformNavigatorUrl( NavigatorUrl url )
        {
            var transformedUrl = LookupInternal( url.UrlString );
            var transformedForm = TransformFormular( url.Formular );
            return new NavigatorUrl( url.UrlType, transformedUrl, transformedForm );
        }

        private Formular TransformFormular( Formular originalFormular )
        {
            if( originalFormular == null )
            {
                return null;
            }

            var transformedForm = new Formular( originalFormular.Name );
            foreach( var origParam in originalFormular.Parameters )
            {
                transformedForm.Parameters[ origParam.Key ] = LookupInternal( origParam.Value );
            }

            return transformedForm;
        }

        private string LookupTable( string str )
        {
            StringBuilder sb = new StringBuilder( str );
            foreach( var pair in Lut )
            {
                sb.Replace( pair.Key, pair.Value );
            }

            return sb.ToString();
        }
    }
}
