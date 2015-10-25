using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;

namespace RaynMaker.Data.ViewModels
{
    class CurrencyViewModel : BindableBase
    {
        private KeyValuePair<string, string> mySelected;

        public CurrencyViewModel( Currency model )
        {
            Model = model;

            Selected = All.SingleOrDefault( e => e.Key == Model.Symbol );
        }

        public Currency Model { get; private set; }

        public IReadOnlyDictionary<string, string> All { get { return KnownCurrencies.All; } }

        public KeyValuePair<string, string> Selected
        {
            get { return mySelected; }
            set
            {
                if( SetProperty( ref mySelected, value ) )
                {
                    Model.Symbol = mySelected.Key;
                    Model.Name = mySelected.Value;
                }
            }
        }
    }
}
