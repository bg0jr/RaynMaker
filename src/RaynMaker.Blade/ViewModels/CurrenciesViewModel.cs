using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Xaml;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Model;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class CurrenciesViewModel : BindableBase
    {
        private Project myProject;
        private CurrenciesSheet mySheet;

        [ImportingConstructor]
        public CurrenciesViewModel( Project project )
        {
            myProject = project;
            PropertyChangedEventManager.AddHandler( myProject, OnProjectPropertyChanged, PropertySupport.ExtractPropertyName( () => myProject.CurrenciesSheetLocation ) );
            OnProjectPropertyChanged( null, null );
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( string.IsNullOrEmpty( myProject.CurrenciesSheetLocation ) || !File.Exists( myProject.CurrenciesSheetLocation ) )
            {
                return;
            }

            var reader = new ValidatingXamlReader();
            Sheet = reader.Read<CurrenciesSheet>( myProject.CurrenciesSheetLocation );
        }

        public CurrenciesSheet Sheet
        {
            get { return mySheet; }
            set { SetProperty( ref mySheet, value ); }
        }
    }
}
