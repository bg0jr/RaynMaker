using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Plainion.Windows.Controls;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    class OverviewContentPageModel : BindableBase, IContentPage
    {
        private IProjectHost myProjectHost;
        private Stock myStock;

        [ImportingConstructor]
        public OverviewContentPageModel(IProjectHost projectHost)
        {
            myProjectHost = projectHost;

            AddReferenceCommand = new DelegateCommand(OnAddReference);
            RemoveReferenceCommand = new DelegateCommand<Reference>(OnRemoveReference);
        }

        public string Header { get { return "Overview"; } }

        public Stock Stock
        {
            get { return myStock; }
            set
            {
                if (SetProperty(ref myStock, value))
                {
                    RaisePropertyChanged(nameof(Tags));
                }
            }
        }

        public void Initialize(Stock stock)
        {
            Stock = stock;
        }

        public void Complete()
        {
            TextBoxBinding.ForceSourceUpdate();

            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();
        }

        public void Cancel()
        {
        }

        public ICommand AddReferenceCommand { get; private set; }

        private void OnAddReference()
        {
            Stock.Company.References.Add(new Reference());
        }

        public ICommand RemoveReferenceCommand { get; private set; }

        private void OnRemoveReference(Reference reference)
        {
            Stock.Company.References.Remove(reference);
        }

        public string Tags
        {
            get { return Stock != null ? string.Join(",", Stock.Company.Tags) : null; }
            set
            {
                if (Stock == null || string.Join(",", Stock.Company.Tags) == value)
                {
                    return;
                }

                Stock.Company.Tags.Clear();

                var ctx = myProjectHost.Project.GetAssetsContext();

                foreach (var tagName in value.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    var tag = ctx.Tags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        ctx.Tags.Add(tag);
                    }

                    Stock.Company.Tags.Add(tag);
                }
            }
        }
    }
}
