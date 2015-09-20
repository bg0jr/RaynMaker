using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using Plainion.Collections;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataSourceDefinitionViewModel : BindableBase
    {
        public DataSourceDefinitionViewModel( Session session )
        {
            Contract.RequiresNotNull( session, "session" );

            Session = session;

            AddCommand = new DelegateCommand( OnAdd );
            RemoveCommand = new DelegateCommand( OnRemove );
        }

        public Session Session { get; private set; }

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
            var source = new DataSource( );
            source.LocationSpec = new Navigation( DocumentType.Html );

            Session.Sources.Add( source );

            Session.CurrentSource = source;
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
            var source = Session.CurrentSource;
            Session.Sources.Remove( source );
            Session.CurrentSource = Session.Sources.FirstOrDefault();
        }
    }
}
