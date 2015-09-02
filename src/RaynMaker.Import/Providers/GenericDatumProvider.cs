using System;
using System.Linq;
using Blade.Data;
using RaynMaker.Import.Spec;
using Plainion.Logging;

namespace RaynMaker.Import.Providers
{
    public class GenericDatumProvider : IDatumProvider
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( GenericDatumProvider ) );

        private IDocumentBrowser myBrowser;

        public GenericDatumProvider( IDocumentBrowser browser, DatumLocator locator, IFetchPolicy fetchPolicy, IResultPolicy resultPolicy )
        {
            myBrowser = browser;
            Locator = locator;
            FetchPolicy = fetchPolicy ?? new PassThroughPolicy();
            ResultPolicy = resultPolicy ?? new FirstNonNullPolicy();
        }

        public DatumLocator Locator { get; private set; }

        /// <summary>
        /// Gets/sets the <see cref="IResultPolicy"/>.
        /// Default: <see cref="FirstNonNullPolicy"/>
        /// If null is passed in the default is used again.
        /// </summary>
        public IResultPolicy ResultPolicy { get; private set; }

        /// <summary>
        /// Gets/sets the <see cref="IFetchPolicy"/>.
        /// Default: <see cref="PassThroughPolicy"/>
        /// If null is passed in the default is used again.
        /// </summary>
        public IFetchPolicy FetchPolicy { get; private set; }

        public string Datum { get { return Locator.Datum; } }

        /// <summary>
        /// Tries to fetch data from the given sites.
        /// If a site result passes the ResultValidator the fetching stops.
        /// If no site result passes the ResultValidator null is returned. 
        /// </summary>
        public IResultPolicy Fetch()
        {
            Site site = Locator.Sites.FirstOrDefault( s => Fetch( s, Locator.Datum ) );
            if( site == null )
            {
                return null;
            }

            // finish the table
            if( ResultPolicy.ResultTable != null )
            {
                // XXX: find a better way
                ResultPolicy.ResultTable.TableName = Locator.Datum;
            }

            return ResultPolicy;
        }

        private bool Fetch( Site site, string datum )
        {
            Navigation modifiedNavigation = null;
            IFormat modifiedFormat = null;

            try
            {
                modifiedNavigation = FetchPolicy.GetNavigation( site );

                var doc = myBrowser.GetDocument( modifiedNavigation );
                if( doc == null )
                {
                    throw new Exception( "Failed to navigate to the document" );
                }

                modifiedFormat = FetchPolicy.GetFormat( site );
                var result = FetchPolicy.ApplyPreprocessing( doc.ExtractTable( modifiedFormat ) );

                // valid result? stop fetching?
                if( ResultPolicy.Validate( site, result ) )
                {
                    return true;
                }

                throw new Exception( "Result not valid" );
            }
            catch( Exception ex )
            {
                ex.Data[ "Datum" ] = datum;
                ex.Data[ "SiteName" ] = site.Name;
                ex.Data[ "OriginalFormat" ] = site.Format;
                ex.Data[ "OriginalNavigation" ] = site.Navigation;
                ex.Data[ "ModifiedFormat" ] = modifiedFormat;
                ex.Data[ "ModifiedNavigation" ] = modifiedNavigation;

                myLogger.Warning( ex, "Failed to fetch '{0}' from site {1}", datum, site.Name );
            }

            return false;
        }

        /// <summary>
        /// Returns the fetch result as scalar if the fetched result
        /// consists of only one value (row and column), throws 
        /// an exception otherwise.
        /// </summary>
        public SingleResultValue<T> FetchSingle<T>()
        {
            var result = Fetch();

            try
            {
                if( result == null || result.ResultTable == null )
                {
                    throw new InvalidOperationException( "Could not get any result" );
                }

                if( result.ResultTable.Rows.Count > 1 )
                {
                    throw new InvalidOperationException( "Result contains non-scalar data" );
                }

                var valueCols = result.ResultTable.Columns.ToSet().Where( c => !c.IsDateColumn() ).ToList();
                if( valueCols.Count == 0 )
                {
                    throw new InvalidOperationException( "Could not find a value column in result" );
                }
                else if( valueCols.Count > 1 )
                {
                    throw new InvalidOperationException( "Result contains more than one value column" );
                }

                var value = result.ResultTable.Rows[ 0 ][ valueCols[ 0 ].ColumnName ];
                if( value == DBNull.Value )
                {
                    throw new InvalidOperationException( "Could not get any result (got empty table)" );
                }

                return new SingleResultValue<T>( result.Sites.Single(), ( T )value );
            }
            catch( Exception ex )
            {
                var exception = new ApplicationException( ex.Message, ex );
                exception.Data[ "Locator.Datum" ] = Locator.Datum;

                throw exception;
            }
        }
    }
}
