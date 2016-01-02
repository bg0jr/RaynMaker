using System;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Modules.Import.Spec.v2.Locating
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "SubmitFormular" )]
    public class SubmitFormular : DocumentLocationFragment
    {
        /// <summary>
        /// Url is the url of the docment containing the form.
        /// </summary>
        public SubmitFormular( string url, Formular form )
            : base( url )
        {
            Contract.RequiresNotNull( form, "form" );

            Formular = form;
        }

        /// <summary>
        /// Url is the url of the docment containing the form.
        /// </summary>
        public SubmitFormular( Uri url, Formular form )
            : base( url )
        {
            Contract.RequiresNotNull( form, "form" );
            
            Formular = form;
        }

        [DataMember]
        public Formular Formular { get; private set; }
    }
}
