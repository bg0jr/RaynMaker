using System.IO;
using Plainion;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class Project : ProjectBase, IProject
    {
        private IContextFactory myContextFactory;

        public string StorageRoot { get; private set; }

        public string EntitiesSource { get; private set; }

        protected override void OnLocationChanged()
        {
            Contract.RequiresNotNullNotEmpty( Location, "Location" );

            StorageRoot = Path.Combine( Path.GetDirectoryName( Location ), ".rym-" + Path.GetFileNameWithoutExtension( Location ) );
            EntitiesSource = Path.Combine( StorageRoot, "db.sqlite" );

            base.OnLocationChanged();
        }

        public void SetAssetsContextFactory( IContextFactory factory )
        {
            Contract.RequiresNotNull( factory, "factory" );

            myContextFactory = factory;
        }

        public IAssetsContext CreateAssetsContext()
        {
            return myContextFactory.CreateAssetsContext();
        }
    }
}
