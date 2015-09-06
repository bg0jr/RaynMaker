using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Blade.Collections;
using Blade.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Import.Html.WinForms;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.ViewModels
{
    class CompletionViewModel : BindableBase
    {
        private Session mySession;
        private IProjectHost myProjectHost;
        private Stock mySelectedStock;

        public CompletionViewModel( Session session, IProjectHost projectHost )
        {
            mySession = session;
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;

            Stocks = new ObservableCollection<Stock>();

            ValidateCommand = new DelegateCommand( OnValidate, CanValidate );
            ClearCommand = new DelegateCommand( OnClear );
            SaveCommand = new DelegateCommand( OnSave );

            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( myProjectHost.Project == null )
            {
                return;
            }

            var ctx = myProjectHost.Project.GetAssetsContext();
            Stocks.AddRange( ctx.Stocks );
            SelectedStock = Stocks.FirstOrDefault();
        }

        public IBrowser Browser { get; set; }

        public ObservableCollection<Stock> Stocks { get; private set; }

        public Stock SelectedStock
        {
            get { return mySelectedStock; }
            set
            {
                if( SetProperty( ref mySelectedStock, value ) )
                {
                    ValidateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ValidateCommand { get; private set; }

        private bool CanValidate() { return SelectedStock != null; }

        private void OnValidate()
        {
            if( mySession.CurrentSite == null )
            {
                return;
            }

            var macroPattern = new Regex( @"\$\{(.*)\}" );
            var filtered = new List<NavigatorUrl>();
            foreach( var navUrl in mySession.CurrentSite.Navigation.Uris )
            {
                var md = macroPattern.Match( navUrl.UrlString );
                if( md.Success )
                {
                    var macroId = md.Groups[ 1 ].Value;
                    var value = GetMacroValue( macroId );
                    if( value != null )
                    {
                        filtered.Add( new NavigatorUrl( navUrl.UrlType, navUrl.UrlString.Replace( macroId, value ) ) );
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

            var doc = Browser.LoadDocument( filtered );

            var markupDoc = new MarkupDocument();
            markupDoc.Document = ( ( HtmlDocumentAdapter )doc ).Document;
            markupDoc.Anchor = mySession.CurrentFormat.Path;
            markupDoc.Dimension = mySession.CurrentFormat.Expand;
            markupDoc.SeriesName = mySession.CurrentFormat.SeriesName;
            if( mySession.CurrentFormat.Expand == CellDimension.Row )
            {
                markupDoc.ColumnHeaderRow = mySession.CurrentFormat.TimeAxisPosition;
                markupDoc.RowHeaderColumn = mySession.CurrentFormat.SeriesNamePosition;
            }
            else if( mySession.CurrentFormat.Expand == CellDimension.Column )
            {
                markupDoc.RowHeaderColumn = mySession.CurrentFormat.TimeAxisPosition;
                markupDoc.ColumnHeaderRow = mySession.CurrentFormat.SeriesNamePosition;
            }
            markupDoc.SkipColumns = mySession.CurrentFormat.SkipColumns;
            markupDoc.SkipRows = mySession.CurrentFormat.SkipRows;

            markupDoc.Apply();
        }

        private string GetMacroValue( string macroId )
        {
            if( macroId.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
            {
                return SelectedStock.Isin;
            }

            throw new NotSupportedException( "Unknown macro: " + macroId );
        }

        public ICommand ClearCommand { get; private set; }

        private void OnClear()
        {
            mySession.Reset();
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {

        }
    }
}
