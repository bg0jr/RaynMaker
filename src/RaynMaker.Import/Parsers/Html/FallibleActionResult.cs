using System;

namespace RaynMaker.Import.Parsers.Html
{
    /// <summary>
    /// Used to return a result from a function which could fail and 
    /// an exception doesnt seem to be useful.
    /// <remarks>
    /// This tries to overcome the "DoIt" and "TryDoIt" functions like
    /// <c>int.Parse()</c> and <c>int.TryParse()</c>. Furthermore it 
    /// avoids the usage of <c>out</c> parameters.
    /// </remarks>
    /// <example>
    /// <code>
    /// public FallibleActionResult&lt;int&gt; MyParse( string s )
    /// {
    ///     ...
    ///
    ///     return FallibleActionResult&lt;int&gt;.CreateFailureResult( "Invalid format" );
    /// }
    ///
    /// public void TheCaller()
    /// {
    ///     var result = MyParse( "abc" );
    ///     if ( !result.Success )
    ///     {
    ///         // return a default value or try s.th. else
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    class FallibleActionResult<T> where T : class
    {
        /// <summary>
        /// The return value of the functions in the case of a successful
        /// execution of the function.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }
        /// <summary>
        /// The failure reason in caseof a failed execution of the function.
        /// </summary>
        public string FailureReason
        {
            get;
            private set;
        }
        /// <summary>
        /// Indicates whether the function was executed successfully and the
        /// return value is valid.
        /// </summary>
        public bool Success
        {
            get
            {
                return this.Value != null;
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        protected FallibleActionResult( T result, string failureReason )
        {
            this.Value = result;
            this.FailureReason = failureReason;
        }
        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static FallibleActionResult<T> CreateSuccessResult( T result )
        {
            return new FallibleActionResult<T>( result, null );
        }
        /// <summary>
        /// Creates a failure result.
        /// </summary>
        public static FallibleActionResult<T> CreateFailureResult( string reason )
        {
            return new FallibleActionResult<T>( default( T ), reason );
        }
        /// <summary>
        /// Throws an exception with the failure reason as message if 
        /// this return object indicates a failure.
        /// </summary>
        public void Raise()
        {
            if( !this.Success )
            {
                throw new Exception( this.FailureReason );
            }
        }
    }
}
