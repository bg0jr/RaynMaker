using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Blade;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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

            EditCaptureRequest = new InteractionRequest<IConfirmation>();
            InputMacroValueRequest = new InteractionRequest<IConfirmation>();
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
                    string value = GetValue( macro );
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
        }

        private string GetValue( string macro )
        {
            var notification = new Confirmation();
            notification.Title = "Enter macro value";
            notification.Content = macro;

            string result = null;

            InputMacroValueRequest.Raise( notification, c =>
            {
                if( c.Confirmed )
                {
                    result = (  string )c.Content ;
                }
            } );

            return result;
        }

        public InteractionRequest<IConfirmation> InputMacroValueRequest { get; private set; }

        public ICommand EditCommand { get; private set; }

        private void OnEdit()
        {
            var notification = new Confirmation();
            notification.Title = "Edit capture";
            notification.Content = NavigationUrls;

            EditCaptureRequest.Raise( notification, c =>
            {
                if( c.Confirmed )
                {
                    NavigationUrls = ( string )c.Content;
                }
            } );
        }

        public InteractionRequest<IConfirmation> EditCaptureRequest { get; private set; }
    }
}
