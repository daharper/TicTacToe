using System.Diagnostics;
using System.Runtime.CompilerServices;
using TicTacToe.Infrastructure.Utils;

namespace TicTacToe.Infrastructure.Integrity;

/// <summary>
/// Used to assert debug time assumptions in code.
///
/// Proxies to Expect, so the same operations are available,
/// plus the following unique to assert:

/// 
/// Assume can be used liberally since it is only used in debug.
/// </summary>
public class Assume
{
    private readonly Expect _expect;

    public Assume(Expect expect) => _expect = expect;

    /// <summary>
    /// Assumes the string value is lowercase.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsLower(
        string? actual,
        string message = "expected a lowercase value.",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        if (string.IsNullOrWhiteSpace(actual)) return;
        _expect.IsEqual(actual, actual?.ToLowerInvariant(), message, filename, member, line);
    }

    /// <summary>
    /// Assumes the values of type T are the same.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsEqual<T>(
        T actual,
        T expected,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsTrue(Generics<T>.IsEqual(actual, expected), message, filename, member, line);
    }

    /// <summary>
    /// Assumes the values of type T are different.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotEqual<T>(
        T actual,
        T expected,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsTrue(Generics<T>.IsNotEqual(actual, expected), message, filename, member, line);
    }

    /// <summary>
    /// Assumes the value is the default for type T.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsDefault<T>(
        T actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsTrue(Generics<T>.IsDefault(actual), message, filename, member, line);
    }

    /// <summary>
    /// Assumes the value is not the default for type T.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotDefault<T>(
        T actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsTrue(Generics<T>.IsNotDefault(actual), message, filename, member, line);
    }

    /// <summary>
    /// Assumes the condition to be true.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsTrue(
        bool condition,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsTrue(condition, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the condition to be false.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsFalse(
        bool condition,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsFalse(condition, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the instance to be null.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNull(
        object? instance,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNull(instance, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the instance to be non-null.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotNull(
        object? instance,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNotNull(instance, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the two strings to be equal.
    /// The comparison is case-sensitive.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsEqual(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsEqual(expected, actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the two strings to be different.
    /// The comparison is case-sensitive.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotEqual(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNotEqual(expected, actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the string to be null or empty.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsEmpty(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsEmpty(actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the string to have a value.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotEmpty(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNotEmpty(actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the string to have a value that is not whitespace.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotBlank(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNotBlank(actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the two strings to be equal.
    /// The comparison is case insensitive.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsEqualIgnoreCase(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsEqualIgnoreCase(expected, actual, message, filename, member, line);
    }

    /// <summary>
    /// Assumes the two strings to be different.
    /// The comparison is case insensitive.
    /// </summary>
    [Conditional("DEBUG")]
    public void IsNotEqualIgnoreCase(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        _expect.IsNotEqualIgnoreCase(expected, actual, message, filename, member, line);
    }
}
