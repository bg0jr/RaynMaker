using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Blade;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Views;

namespace RaynMaker.Import.Web.ViewModels
{
    class NavigationViewModel : BindableBase
    {
        private bool myIsCapturing;
        private string myNavigationUrls;

        public NavigationViewModel()
        {
            CaptureCommand = new DelegateCommand( OnCapture );
            ReplayCommand = new DelegateCommand( OnReplay );
            EditCommand = new DelegateCommand( OnEdit );
        }

        public IBrowser Browser { get; set; }
        
        public string NavigationUrls
        {
            get { return myNavigationUrls; }
            set { SetProperty( ref myNavigationUrls, value ); }
        }

        public bool IsCapturing
        {
            get { return myIsCapturing; }
            set { SetProperty( ref myIsCapturing, value ); }
        }

        public ICommand CaptureCommand { get; private set; }

        private void OnCapture()
        {
            IsCapturing = !IsCapturing;
        }

        private IList<NavigatorUrl> GetNavigationSteps()
        {
            var q = from token in NavigationUrls.Split( new string[] { Environment.NewLine }, StringSplitOptions.None )
                    where !token.IsNullOrTrimmedEmpty()
                    select NavigatorUrl.Parse( token );

            // the last url must be a request
            return ( from navUrl in q
                     where !( navUrl == q.Last() && navUrl.UrlType == UriType.Response )
                     select navUrl ).ToList();
        }

        public ICommand ReplayCommand { get; private set; }

        private void OnReplay()
        {
            var q = GetNavigationSteps();

            var macroPattern = new Regex( @"(\$\{.*\})" );
            var filtered = new List<NavigatorUrl>();
            foreach( NavigatorUrl navUrl in q )
            {
                var md = macroPattern.Match( navUrl.UrlString );
                if( md.Success )
                {
                    string macro = md.Groups[ 1 ].Value;
                    string value = InputForm.Show( "Enter macro value", "Enter value for macro " + macro );
                    if( value != null )
                    {
                        filtered.Add( new NavigatorUrl( navUrl.UrlType, navUrl.UrlString.Replace( macro, value ) ) );
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filtered.Add( navUrl );
                }
            }

            Browser.LoadDocument( filtered );

            /*
Request: http://www.ariva.de/search/search.m?searchname=${stock.Symbol}&url=/quote/profile.m
Response: http://www.ariva.de/quote/profile.m?secu={(\d+)}
Request: http://www.ariva.de/statistics/facunda.m?secu={0}&page=-1             
             */
        }

        public ICommand EditCommand { get; private set; }

        private void OnEdit()
        {
            EditCaptureForm form = new EditCaptureForm();
            form.NavUrls = NavigationUrls;
            var result = form.ShowDialog();

            if( result == System.Windows.Forms.DialogResult.OK )
            {
                NavigationUrls = form.NavUrls;
            }
        }
    }
}
