﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes extraction of an entire table which can be located using an explicit path.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathTableDescriptor" )]
    public class PathTableDescriptor : TableDescriptorBase, IPathDescriptor
    {
        private string myPath;

        /// <summary>
        /// Gets or sets the path within the document to the table.
        /// </summary>
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Path
        {
            get { return myPath; }
            set { SetProperty( ref myPath, value ); }
        }
    }
}
