
namespace RaynMaker.Import.Spec.v1
{
    /// <summary>
    /// Defines the part of a table under inspection.
    /// </summary>
    public enum CellDimension
    {
        /// <summary>
        /// No dimension. The single cell.
        /// </summary>
        None,
        /// <summary>
        /// Complete row.
        /// </summary>
        Row,
        /// <summary>
        /// Complete column.
        /// </summary>
        Column
    }
}
