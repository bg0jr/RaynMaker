using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Import.Web.ViewModels
{
    // TODO: we do not support add, remove and edit of datums as they are currently fixed by entities model.
    // TODO: "Standing" datums also exists
    class DatumSelectionViewModel : BindableBase
    {
        private Type mySelectedDatum;

        public DatumSelectionViewModel()
        {
            Datums = Dynamics.AllDatums;
            SelectedDatum = typeof( Price );
        }

        public IEnumerable<Type> Datums { get; private set; }

        public Type SelectedDatum
        {
            get { return mySelectedDatum; }
            set { SetProperty( ref mySelectedDatum, value ); }
        }
    }
}
