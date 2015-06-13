using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Plainion;
using Plainion.AppFw.Wpf.Services;
using Plainion.Progress;
using RaynMaker.Entities.Persistancy;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class ProjectService : ProjectService<Project>, IProjectHost
    {
        private Storage myStorage;

        protected override void InitializeProject( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            progress.Report( new UndefinedProgress( "Creating project at " + project.Location ) );

            if( !Directory.Exists( project.StorageRoot ) )
            {
                Directory.CreateDirectory( project.StorageRoot );
            }

            var storage = new Storage( project.EntitiesSource );
            storage.Initialize();

            project.SetAssetsContextProvider( storage );
        }

        protected override Project Deserialize( string file, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            progress.Report( new UndefinedProgress( "Loading project at " + file ) );

            var project = new Project();
            project.Location = file;

            myStorage = new Storage( project.EntitiesSource );
            myStorage.Initialize();

            project.SetAssetsContextProvider( myStorage );

            return project;
        }

        protected override void Serialize( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            Contract.RequiresNotNullNotEmpty( project.Location, "project.Location" );

            File.WriteAllText( project.Location, "DUMMY" );
        }

        protected override void OnProjectChanging( Project oldProject )
        {
            if( Changing != null )
            {
                Changing();
            }

            myStorage.Dispose();
            myStorage = null;

            base.OnProjectChanging( oldProject );
        }

        protected override void OnProjectChanged( Project newProject )
        {
            base.OnProjectChanged( newProject );

            if( Changed != null )
            {
                Changed();
            }
        }

        IProject IProjectHost.Project { get { return Project; } }

        public event Action Changing;

        public event Action Changed;
    }
}
