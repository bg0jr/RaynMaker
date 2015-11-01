using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Plainion;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Keep immutable!
    /// TODO: maybe we should have a generic FormatColumn passing the type info via generic type param
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "FormatColumn" )]
    public class FormatColumn : ValueFormat
    {
        public FormatColumn( string name )
            : this( name, typeof( string ), null )
        {
        }

        public FormatColumn( string name, Type type )
            : this( name, type, null )
        {
        }

        public FormatColumn( string name, Type type, string format )
            : this( name, type, format, null )
        {
        }

        public FormatColumn( string name, Type type, string format, Regex extractionPattern )
            : base( type, format, extractionPattern )
        {
            Contract.RequiresNotNullNotEmpty( name, "name" );

            Name = name;
        }

        public FormatColumn( FormatColumn other )
            : base( other )
        {
            Contract.RequiresNotNull( other, "other" );

            Name = other.Name;
        }

        [DataMember]
        public string Name { get; private set; }
    }
}
