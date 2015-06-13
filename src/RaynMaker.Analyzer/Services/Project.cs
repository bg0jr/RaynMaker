using System;
using System.IO;
using Plainion;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    class Project : ProjectBase, IProject, IDisposable
    {
        private IContextFactory myContextFactory;
        private IAssetsContext myAssetsContext;

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

        public IAssetsContext GetAssetsContext()
        {
            if( myAssetsContext == null )
            {
                myAssetsContext = myContextFactory.CreateAssetsContext();
            }

            return myAssetsContext;
        }

        public void Dispose()
        {
            if( myAssetsContext != null )
            {
                var disposable = myAssetsContext as IDisposable;
                if( disposable != null )
                {
                    disposable.Dispose();
                }
                myAssetsContext = null;
            }
        }
    }
}
