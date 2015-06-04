using System;
using System.IO;
using System.Threading;
using Plainion;
using Plainion.AppFw.Wpf.Services;
using Plainion.Progress;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class ProjectService : ProjectService<Project>
    {
        protected override void InitializeProject( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            progress.Report( new UndefinedProgress( "Creating project at " + project.Location ));

            Thread.Sleep( 30 * 1000 );

            base.InitializeProject( project, progress, cancellationToken );
        }

        protected override Project Deserialize( string file, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            return new Project();
        }

        protected override void Serialize( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            Contract.RequiresNotNullNotEmpty( project.Location, "project.Location" );

            File.WriteAllText( project.Location, "DUMMY" );
        }
    }
}
