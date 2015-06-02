using System.IO;
using Plainion;
using Plainion.AppFw.Wpf.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class ProjectService : ProjectService<Project>
    {
        protected override Project Deserialize( string file )
        {
            return new Project();
        }

        protected override void Serialize( Project project )
        {
            Contract.RequiresNotNullNotEmpty( project.Location, "project.Location" );

            File.WriteAllText( project.Location, "DUMMY" );
        }
    }
}
