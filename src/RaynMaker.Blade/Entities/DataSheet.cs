using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
{
    public class DataSheet
    {
        public DataSheet()
        {
            Data = new ObservableCollection<IDatumSeries>();
        }

        [ValidateObject]
        public ObservableCollection<IDatumSeries> Data { get; private set; }
    }
}
