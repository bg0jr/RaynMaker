using Plainion;
using RaynMaker.Infrastructure.Model;

namespace RaynMaker.Analyzer.Model
{
    class AnalysisProject : IProject
    {
        public AnalysisProject( string location )
        {
            Contract.RequiresNotNull( location, "location" );

            Location = location;
        }

        public string Location { get; private set; }
    }
}
