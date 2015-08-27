using Plainion;

namespace RaynMaker.Blade.Engine
{
    class MissingData : IFigureProviderFailure
    {
        public MissingData( string datum )
        {
            Contract.RequiresNotNullNotEmpty( datum, "datum" );
            
            Datum = datum;
        }

        public string Datum { get; private set; }

        public override string ToString()
        {
            return string.Format( "No data found for '{0}'", Datum );
        }
    }
}
