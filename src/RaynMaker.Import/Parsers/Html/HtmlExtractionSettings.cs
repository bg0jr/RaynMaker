
namespace RaynMaker.Import.Parsers.Html
{
    /// <summary>
    /// Defines the settings used to extract data from a Html document.
    /// See <c>ExtractTable</c> methods of the <see cref="HtmlDocumentExtensions"/>.
    /// </summary>
    public class HtmlExtractionSettings
    {
        /// <summary>
        /// Defines the defaults.
        /// Defaults: no link extraction.
        /// </summary>
        public HtmlExtractionSettings()
        {
            ExtractLinkUrl = false;
        }

        /// <summary>
        /// The href attribute of an A element will be extracted instead of the text of it.
        /// <remarks>Only the url of the first "A" child.</remarks>
        /// </summary>
        public bool ExtractLinkUrl { get; set; }
    }
}
