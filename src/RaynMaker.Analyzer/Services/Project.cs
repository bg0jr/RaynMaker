using System;
using System.Collections.Generic;
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
        private IAnalysisContext myAnalysisContext;
        private Dictionary<string, string> myUserData;

        public Project()
        {
            myUserData = new Dictionary<string, string>();
        }

        public string StorageRoot { get; private set; }

        public string DatabaseSource { get; private set; }

        protected override void OnLocationChanged()
        {
            Contract.RequiresNotNullNotEmpty( Location, "Location" );

            StorageRoot = Path.Combine( Path.GetDirectoryName( Location ), ".rym-" + Path.GetFileNameWithoutExtension( Location ) );
            DatabaseSource = Path.Combine( StorageRoot, "db.sqlite" );

            base.OnLocationChanged();
        }

        public void SetContextFactory( IContextFactory factory )
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

        public IAnalysisContext GetAnalysisContext()
        {
            if( myAnalysisContext == null )
            {
                myAnalysisContext = myContextFactory.CreateAnalysisContext();
            }

            return myAnalysisContext;
        }

        public void Dispose()
        {
            DestroyContext( ref myAssetsContext );
            DestroyContext( ref myAnalysisContext );
        }

        private void DestroyContext<T>( ref T context ) where T : class
        {
            if( context == null )
            {
                return;
            }

            var disposable = context as IDisposable;
            if( disposable != null )
            {
                disposable.Dispose();
            }
            context = null;
        }

        public IDictionary<string, string> UserData { get { return myUserData; } }
    }
}
