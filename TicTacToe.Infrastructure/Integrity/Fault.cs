namespace TicTacToe.Infrastructure.Integrity;

/// <summary>
/// Central place for raising exceptions. The two calling idioms:
///
/// throw Error.Raise(e);  
/// Error.Raise(e);
/// 
/// Any error observers will be notified of the exception.
/// </summary>
public static class Fault
{
    private static event Action<Exception> Observers = null!;

    /// <summary>
    /// Gets or sets subscribers to be notified of exceptions.
    /// </summary>
    public static event Action<Exception> OnError
    {
        add => Observers += value;
        remove => Observers -= value;
    }

    /// <summary>
    /// Raises an argument exception with the specified message.
    /// </summary>
    public static ArgumentException Raise(string message)
        => (ArgumentException)Raise(new ArgumentException(message));

    /// <summary>
    /// Raises an argument null exception with the specified message.
    /// </summary>
    public static ArgumentNullException RaiseNull(string name, string message = "")
        => (ArgumentNullException)Raise(new ArgumentNullException(name, message));

    /// <summary>
    /// Creates a new exception of the specified type and arguments,
    /// and notifies observers that is has been raised.
    /// </summary>
    public static T Raise<T>(params object[] args)
        where T : Exception
    {
        var e = Activator.CreateInstance(typeof(T), args) as T;
        return (T)Raise(e!);
    }

    /// <summary>
    /// Notifies observers that the specified exception occurred.
    /// </summary>
    public static Exception Raise(Exception e)
    {
        Observers?.Invoke(e);
        return e;
    }
}
