using Plainion;

namespace RaynMaker.Modules.Analysis.Engine
{
    class MissingData : IFigureProviderFailure
    {
        public MissingData( string figure, object defaultValue )
        {
            Contract.RequiresNotNullNotEmpty( figure, "figure" );

            Figure = figure;
            DefaultValue = defaultValue;
        }

        public string Figure { get; private set; }

        public object DefaultValue { get; private set; }

        public override string ToString()
        {
            return string.Format( "No data found for '{0}'", Figure );
        }
    }
}
