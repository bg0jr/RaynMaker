using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Locating
{
    /// <summary>
    /// Identifies a formular in a document by name.
    /// Specified parameters will be set when submitting the form.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "Formular" )]
    public class Formular
    {
        public static Formular Empty = new Formular( string.Empty );

        public Formular( string name )
            : this( name, new Tuple<string, string>[] { } )
        {
        }

        public Formular( string name, params Tuple<string, string>[] parameters )
        {
            Name = name;
            Parameters = new Dictionary<string, string>();

            foreach( var param in parameters )
            {
                Parameters.Add( param.Item1, param.Item2 );
            }
        }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public IDictionary<string, string> Parameters { get; private set; }
    }
}
