using System;
using System.IO;
using System.Threading;
using Plainion;
using Plainion.AppFw.Wpf.Services;
using Plainion.Progress;
using RaynMaker.Entities.Persistancy;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class ProjectService : ProjectService<Project>, IProjectHost
    {
        private DatabaseService myDbService;

        protected override void InitializeProject( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            progress.Report( new UndefinedProgress( "Creating project at " + project.Location ) );

            if( !Directory.Exists( project.StorageRoot ) )
            {
                Directory.CreateDirectory( project.StorageRoot );
            }

            myDbService = new DatabaseService( project.DatabaseSource );
            myDbService.Initialize();

            project.SetContextFactory( myDbService );
        }

        protected override Project Deserialize( string file, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            progress.Report( new UndefinedProgress( "Loading project at " + file ) );

            var project = new Project();
            project.Location = file;

            using( var reader = new StreamReader( project.Location ) )
            {
                while( !reader.EndOfStream )
                {
                    var line = reader.ReadLine();
                    var tokens = line.Split( new[] { "/;:;/" }, StringSplitOptions.RemoveEmptyEntries );

                    project.UserData[ tokens[ 0 ] ] = tokens[ 1 ];
                }
            }

            myDbService = new DatabaseService( project.DatabaseSource );
            myDbService.Initialize();

            project.SetContextFactory( myDbService );

            return project;
        }

        protected override void Serialize( Project project, IProgress<IProgressInfo> progress, CancellationToken cancellationToken )
        {
            Contract.RequiresNotNullNotEmpty( project.Location, "project.Location" );

            using( var writer = new StreamWriter( project.Location ) )
            {
                foreach( var entry in project.UserData )
                {
                    writer.WriteLine( "{0}/;:;/{1}", entry.Key, entry.Value );
                }
            }
        }

        protected override void OnProjectChanging( Project oldProject )
        {
            if( Changing != null )
            {
                Changing();
            }

            if( oldProject != null )
            {
                oldProject.Dispose();
            }

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
