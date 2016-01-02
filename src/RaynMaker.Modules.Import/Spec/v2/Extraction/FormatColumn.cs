using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Keep immutable!
    /// TODO: maybe we should have a generic FormatColumn passing the type info via generic type param
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "FormatColumn" )]
    public class FormatColumn : ValueFormat
    {
        private string myName;

        public FormatColumn()
        {
        }

        public FormatColumn( string name, Type type )
            : this( name, type, null )
        {
        }

        public FormatColumn( string name, Type type, string format )
            : base( type, format )
        {
            Name = name;
        }

        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }
    }
}
