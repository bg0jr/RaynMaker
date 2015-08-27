using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Blade.Services
{
    [Export]
    class StorageService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public StorageService( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
        }

        public string LoadAnalysisTemplateText()
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();
            if( !ctx.AnalysisTemplates.Any() )
            {
                var template = new RaynMaker.Entities.AnalysisTemplate();
                template.Name = "Default";

                using( var stream = GetType().Assembly.GetManifestResourceStream( "RaynMaker.Blade.Resources.Analysis.xaml" ) )
                {
                    using( var reader = new StreamReader( stream ) )
                    {
                        template.Template = reader.ReadToEnd();
                    }
                }
                ctx.AnalysisTemplates.Add( template );

                ctx.SaveChanges();
            }

            return ctx.AnalysisTemplates.Single().Template;
        }

        public RaynMaker.Blade.AnalysisSpec.AnalysisTemplate LoadAnalysisTemplate( ICurrenciesLut lut )
        {
            var reader = new ValidatingXamlReader();

            var text = LoadAnalysisTemplateText();

            // required for currency translation during loading from text to entity
            CurrencyConverter.CurrenciesLut = lut;

            return reader.Read<RaynMaker.Blade.AnalysisSpec.AnalysisTemplate>( XElement.Parse( text ) );
        }

        public void SaveAnalysisTemplate( string template )
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();

            ctx.AnalysisTemplates.Single().Template = template;

            ctx.SaveChanges();
        }
    }
}
