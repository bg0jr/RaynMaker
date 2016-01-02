using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most figure descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "FigureDescriptorBase" )]
    public abstract class FigureDescriptorBase : SerializableBindableBase, IFigureDescriptor
    {
        private string myFigure;
        private bool myInMillions;

        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Figure
        {
            get { return myFigure; }
            set { SetProperty( ref myFigure, value ); }
        }

        [DataMember]
        public bool InMillions
        {
            get { return myInMillions; }
            set { SetProperty( ref myInMillions, value ); }
        }
    }
}
