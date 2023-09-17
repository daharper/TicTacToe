namespace TicTacToe.Infrastructure.Integrity;

/// <summary>
/// Represents the result of an operation, a basic implementation of a
/// functional programming construct.
/// </summary>
public class Result
{
    private static readonly Result OkResult = new();

    /// <summary>
    /// Gets a value indicating whether the operation was a success.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation was a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error message string.
    /// </summary>
    public string Error { get; } = "";

    /// <summary>
    /// Gets a value indicating whether there is an error message.
    /// </summary>
    public bool IsError => IsFailure && !string.IsNullOrEmpty(Error);

    /// <summary>
    /// Initializes an instance of a successful result.
    /// </summary>
    protected Result() => IsSuccess = true;

    /// <summary>
    /// Initializes an instance of a failed result.
    /// </summary>
    protected Result(string error) => Error = error;

    /// <summary>
    /// Creates an instance of a failed result with the specified error message.
    /// </summary>
    public static Result Fail(string error = "") => new(error);

    /// <summary>
    /// Creates an instance of a failed result with the specified error message.
    /// </summary>
    public static Result<T> Fail<T>(string error = "") => new(error);

    /// <summary>
    /// Creates an instance of a successful result.
    /// </summary>
    public static Result Ok() => OkResult;

    /// <summary>
    /// Creates an instance of a successful result with the specified value.
    /// </summary>
    public static Result<T> Ok<T>(T value) => new(value);
}

/// <summary>
/// Extends Results to enable the return of a value.
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    /// <summary>
    /// Initializes a new instance of a failed result with the specified error.
    /// </summary>
    protected internal Result(string error)
        : base(error)
    {
    }

    /// <summary>
    /// Initializes a new instance of a successful result with the specified value.
    /// </summary>
    protected internal Result(T value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value of a specified 
    /// </summary>
    public T? Value
    {
        get
        {
            Bob.Expects.IsTrue(IsSuccess, "The result is a failure, it has no value.");
            return _value;
        }
    }
}

