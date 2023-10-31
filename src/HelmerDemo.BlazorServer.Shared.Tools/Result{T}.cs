using System.Net;

namespace HelmerDemo.BlazorServer.Shared.Tools
{
    /// <summary>
    /// This class returns a result on a query-type function
    /// </summary>
    /// <typeparam name="T">Any class returning an object as a result from the query</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}" >Result<TValue></see> for a result that returns the object TValue
        /// </summary>
        /// <param name="value">The object to return for this result</param>
        /// <param name="isSuccess">Indicates a successfull result</param>
        /// <param name="messages">Messages explaining the result</param>
        /// <param name="statusCode">The HttpStatusCode</param>
        public Result(TValue? value, List<string> messages, HttpStatusCode statusCode) : base(messages, statusCode)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the object containg the query-results to return for this <see cref="Result{TValue}">Result</see>
        /// </summary>
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can not be accessed");
    }
}
