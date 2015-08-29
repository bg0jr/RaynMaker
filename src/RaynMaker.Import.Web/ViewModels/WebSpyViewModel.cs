﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Web.DatumLocationValidation;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase
    {
        private string myUrl;

        public WebSpyViewModel()
        {
            StartValidationCommand = new DelegateCommand( OnStartValidation );
            GoCommand = new DelegateCommand( OnGo );
        }

        public IBrowser Browser { get; set; }

        public ICommand StartValidationCommand { get; private set; }

        private void OnStartValidation()
        {
            var form = new ValidationForm( Browser );
            form.Show();
        }

        public string Url
        {
            get { return myUrl; }
            set { SetProperty( ref myUrl, value ); }
        }

        public ICommand GoCommand { get; private set; }

        private void OnGo()
        {
            if( string.IsNullOrEmpty( myUrl ) )
            {
                return;
            }

            Browser.Navigate( myUrl );
        }

    }
}