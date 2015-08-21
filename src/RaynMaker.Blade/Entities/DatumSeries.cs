using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Series", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class DatumSeries : IDatumSeries
    {
        private bool myCurrencyIsFrozen;
        private IComparer<IDatum> myComparer;

        [Required, ValidateObject]
        [DataMember( Name = "Values" )]
        private List<IDatum> myValues;

        public static IDatumSeries Empty = new DatumSeries( null, "Empty" );

        private DatumSeries( Type datumType, string name )
        {
            DatumType = datumType;
            Name = name;

            myValues = new List<IDatum>();
        }

        public DatumSeries( Type datumType )
            : this( datumType, "CollectionOf" + datumType.Name )
        {
        }

        internal DatumSeries( Type datumType, params IDatum[] items )
            : this( datumType )
        {
            foreach( var item in items )
            {
                Add( item );
            }
        }

        public Type DatumType { get; private set; }

        public string Name { get; private set; }

        public Currency Currency { get; private set; }

        public void Add( IDatum datum )
        {
            Contract.RequiresNotNull( datum, "datum" );
            Contract.Requires( datum.GetType() == DatumType, "[{0}] DatumType mismatch: expected={1}, actual={2}", Name, DatumType, datum.GetType() );

            var currencyDatum = datum as ICurrencyDatum;

            if( !myCurrencyIsFrozen )
            {
                if( currencyDatum != null )
                {
                    Currency = currencyDatum.Currency;
                }

                myCurrencyIsFrozen = true;
            }

            if( currencyDatum != null )
            {
                // we cannot enforce collection currency != null because of DerivedDatum
                Contract.Requires( Currency == currencyDatum.Currency,
                    "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, currencyDatum.Currency );
            }
            else
            {
                Contract.Requires( Currency == null, "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, null );
            }

            if( myComparer == null )
            {
                myComparer = new DatumByPeriodComparer();
            }

            var index = myValues.BinarySearch( datum, myComparer );
            if( index < 0 ) index = ~index;
            myValues.Insert( index, datum );
        }

        public void Remove( IDatum datum )
        {
            Contract.RequiresNotNull( datum, "datum" );
            myValues.Remove( datum );
        }

        public int Count
        {
            get { return myValues.Count; }
        }

        public IEnumerator<IDatum> GetEnumerator()
        {
            return myValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return myValues.GetEnumerator();
        }

        [OnSerializing]
        private void OnSerializing( StreamingContext context )
        {
            Contract.Invariant( myValues.Count > 0, "Empty series cannot be serialized" );
        }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // empty lists will not be serialized
            DatumType = myValues.First().GetType();
            Name = "CollectionOf" + DatumType.Name;

            var currencyDatum = myValues.First() as ICurrencyDatum;
            if( currencyDatum != null )
            {
                Currency = currencyDatum.Currency;
            }

            myCurrencyIsFrozen = true;
        }
    }
}
