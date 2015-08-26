﻿using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Plainion.Validation;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.Entities;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;

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

        public CurrenciesSheet LoadCurrencies()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            if( !ctx.Currencies.Any() )
            {
                ctx.Currencies.Add( new Currency { Name = "Euro" } );
                ctx.Currencies.Add( new Currency { Name = "Dollar" } );

                ctx.SaveChanges();
            }

            var dbSheet = new CurrenciesSheet();
            foreach( var currency in ctx.Currencies.Include( c => c.Translations ) )
            {
                dbSheet.Currencies.Add( currency );
            }
            return dbSheet;
        }

        public void SaveCurrencies( CurrenciesSheet sheet )
        {
            var ctx = myProjectHost.Project.GetAssetsContext();

            foreach( var currency in sheet.Currencies.Where( c => c.Id == 0 ) )
            {
                // TODO: ensure that Translation.Source is set - cleanup soon
                foreach( var translation in currency.Translations )
                {
                    translation.Source = currency;
                }

                ctx.Currencies.Add( currency );
            }

            ctx.SaveChanges();
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

        public RaynMaker.Blade.AnalysisSpec.AnalysisTemplate LoadAnalysisTemplate( CurrenciesSheet sheet )
        {
            var reader = new ValidatingXamlReader();

            var text = LoadAnalysisTemplateText();

            // required for currency translation during loading from text to entity
            CurrencyConverter.Sheet = sheet;

            return reader.Read<RaynMaker.Blade.AnalysisSpec.AnalysisTemplate>( XElement.Parse( text ) );
        }

        public void SaveAnalysisTemplate( string template )
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();

            ctx.AnalysisTemplates.Single().Template = template;

            ctx.SaveChanges();
        }

        public DataSheet LoadDataSheet( Stock stock )
        {
            var sheet = new DataSheet();

            foreach( var datumType in Dynamics.AllDatums )
            {
                sheet.Data.Add( Dynamics.GetDatumSeries( stock, datumType ) );
            }

            return sheet;
        }
    }
}
