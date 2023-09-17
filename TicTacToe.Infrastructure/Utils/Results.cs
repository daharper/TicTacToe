using TicTacToe.Infrastructure.Integrity;

namespace TicTacToe.Infrastructure.Utils;

/// <summary>
/// Various methods for working with results, typically for chaining
/// methods or currying.
///
/// When the 2nd argument is an action, the result specified as the
/// 1st argument is always returned.
///
/// When the 2nd argument is a function, the returned value is 
/// either the 1st argument if the condition doesn't apply, or the
/// result of the invoked function if it does.
/// </summary>
public static class Results
{
    /// <summary>
    /// If the result is a failure, the specified action is executed.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result OnFail(this Result result, Action<Result> action)
    {
        if (result.IsError)
        {
            action(result);
        }

        return result;
    }

    /// <summary>
    /// If the result is a failure, the specified action is executed.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result<T> OnFail<T>(this Result<T> result, Action<Result<T>> action)
    {
        if (result.IsFailure)
        {
            action(result);
        }

        return result;
    }

    /// <summary>
    /// If the result is a failure, the specified function is executed.
    /// </summary>
    /// <returns>For a failure, the result of the function; otherwise the specified result.</returns>
    public static Result OnFail(this Result result, Func<Result, Result> func)
        => result.IsFailure ? func(result) : result;

    /// <summary>
    /// If the result is a failure, the specified function is executed.
    /// </summary>
    /// <returns>For a failure, the result of the function; otherwise the specified result.</returns>
    public static Result<T> OnFail<T>(this Result<T> result, Func<Result<T>, Result<T>> func)
        => result.IsFailure ? func(result) : result;

    /// <summary>
    /// If the result is a success, the specified action is executed.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }

        return result;
    }

    /// <summary>
    /// If the result is a success, the specified action is executed.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value!);
        }

        return result;
    }

    /// <summary>
    /// If the result is a success, the specified function is executed.
    /// </summary>
    /// <returns>For a success, the result of the function; otherwise the specified result.</returns>
    public static Result OnSuccess<T>(this Result result, Func<Result> func)
        => result.IsSuccess ? func() : result;

    /// <summary>
    /// If the result is a success, the specified function is executed. 
    /// </summary>
    /// <returns>For a success, the result of the function; otherwise the specified result.</returns>
    public static Result<T> OnSuccess<T>(this Result<T> result, Func<T, Result<T>> func)
        => result.IsSuccess ? func(result.Value!) : result;

    /// <summary>
    /// Executes the specified action.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result Then(this Result result, Action<Result> action)
    {
        action(result);
        return result;
    }

    /// <summary>
    /// Executes the specified action.
    /// </summary>
    /// <returns>The specified result.</returns>
    public static Result<T> Then<T>(this Result<T> result, Action<Result<T>> action)
    {
        action(result);
        return result;
    }

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <returns>The result of the specified function.</returns>
    public static Result Then(this Result result, Func<Result, Result> func)
        => func(result);

    /// <summary>
    /// Executes the specified function. 
    /// </summary>
    /// <returns>The result of the specified function.</returns>
    public static Result<T> Then<T>(this Result<T> result, Func<Result<T>, Result<T>> func)
        => func(result);

    /// <summary>
    /// Throws the specified exception if the result is a failure.
    /// </summary>
    /// <returns>For a success, the specified result; otherwise an exception is thrown.</returns>
    public static Result OnFailThrow<TException>(this Result result, params object[] args)
        where TException : Exception
    {
        if (result.IsSuccess) return result;
        throw Fault.Raise<TException>(args);
    }

    /// <summary>
    /// Throws the specified exception if the result is a failure.
    /// </summary>
    /// <returns>For a success, the specified result; otherwise an exception is thrown.</returns>
    public static Result<T> OnFailThrow<T, TException>(this Result<T> result, params object[] args)
        where TException : Exception
    {
        if (result.IsSuccess) return result;
        throw Fault.Raise<TException>(args);
    }
}