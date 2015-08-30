using Plainion;

namespace RaynMaker.Analysis.Engine
{
    class MissingData : IFigureProviderFailure
    {
        public MissingData( string datum, object defaultValue )
        {
            Contract.RequiresNotNullNotEmpty( datum, "datum" );

            Datum = datum;
            DefaultValue = defaultValue;
        }

        public string Datum { get; private set; }

        public object DefaultValue { get; private set; }

        public override string ToString()
        {
            return string.Format( "No data found for '{0}'", Datum );
        }
    }
}
