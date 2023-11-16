namespace HelmerDemo.BlazorServer.Shared.Tools.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Downcast from the <see cref="Result">Result</see> superclass to the <see cref="Result{TValue}"/> subclass
    /// </summary>
    /// <typeparam name="TValue">The query object results return object</typeparam>
    /// <param name="result">The original Result</param>
    /// <param name="value">The query object results return object value</param>
    /// <returns></returns>
    public static Result<TValue> DownCast<TValue>(this Result result, TValue? value = default)
    {
        return new Result<TValue>(value, result.Messages.ToList(), result.StatusCode);
    }
}