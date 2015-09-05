using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    /// <summary>
    /// Defines how to use the data described in the Site
    /// object to extract data.
    /// </summary>
    public interface IFetchPolicy
    {
        /// <summary>
        /// Usually used to modify a navigation template right before the navigation 
        /// happens but keeping the original navigation template.
        /// </summary>
        Navigation GetNavigation( Navigation navigation );

        /// <summary>
        /// Usually used to modify a format template right before the extraction
        /// of the raw data happens but keeping the original format template.
        /// </summary>
        IFormat GetFormat( IFormat format );

        /// <summary>
        /// Allows any kind of preprocessing steps on the extracted raw data
        /// right before it gets transformed into formated data.
        /// </summary>
        DataTable ApplyPreprocessing( DataTable table );
    }

}
