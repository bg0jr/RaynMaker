using System.IO;
using Plainion;
using Plainion.AppFw.Wpf.Infrastructure;

namespace RaynMaker.Infrastructure
{
    public class Project : ProjectBase
    {
        public string StorageRoot { get; private set; }

        public string EntitiesSource { get; private set; }

        protected override void OnLocationChanged()
        {
            Contract.RequiresNotNullNotEmpty( Location, "Location" );

            StorageRoot = Path.Combine( Path.GetDirectoryName( Location ), ".rym" );
            EntitiesSource = Path.Combine( StorageRoot, "db.sqlite" );

            base.OnLocationChanged();
        }
    }
}
