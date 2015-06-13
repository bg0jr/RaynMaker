using System.IO;
using Plainion;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class Project : ProjectBase, IProject
    {
        private IContextProvider myContextProvider;

        public string StorageRoot { get; private set; }

        public string EntitiesSource { get; private set; }

        protected override void OnLocationChanged()
        {
            Contract.RequiresNotNullNotEmpty( Location, "Location" );

            StorageRoot = Path.Combine( Path.GetDirectoryName( Location ), ".rym-" + Path.GetFileNameWithoutExtension( Location ) );
            EntitiesSource = Path.Combine( StorageRoot, "db.sqlite" );

            base.OnLocationChanged();
        }

        public void SetAssetsContextProvider( IContextProvider provider )
        {
            Contract.RequiresNotNull( provider, "factory" );

            myContextProvider = provider;
        }

        public IAssetsContext GetAssetsContext()
        {
            return myContextProvider.GetAssetsContext();
        }
    }
}
