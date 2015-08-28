using Blade;
using System;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes the semantical content of some data.
    /// e.g. the currency of a number value.
    /// </summary>
    [Serializable]
    public class DataContent
    {
        private DataContent()
        {
        }

        public DataContent( string currency )
        {
            this.Require( x => !string.IsNullOrEmpty( currency ) );

            Currency = currency;
        }

        public string Currency
        {
            get;
            private set;
        }

        public static DataContent FreeText = new DataContent();
    }
}
