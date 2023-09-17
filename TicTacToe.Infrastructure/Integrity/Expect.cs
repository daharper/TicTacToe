using System.Runtime.CompilerServices;

namespace TicTacToe.Infrastructure.Integrity;

/// <summary>
/// 
/// Used to assert runtime expectations in code.
///
/// Expectations can be chained:
///
///     I.Expect.NotBlank(text).NotEqual("list", text);
/// 
/// </summary>
public class Expect
{
    /// <summary>
    /// Expects the condition to be true.
    /// </summary>
    public Expect IsTrue(
        bool condition,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(!condition, message, "Assertion was false", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the condition to be false.
    /// </summary>
    public Expect IsFalse(
        bool condition,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(condition, message, "Assertion was true", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the instance to be null.
    /// </summary>
    public Expect IsNull(
        object? instance,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(!ReferenceEquals(null, instance), message, "Null assertion failed", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the instance to be non-null.
    /// </summary>
    public Expect IsNotNull(
        object? instance,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(ReferenceEquals(null, instance), message, "Non-null assertion failed", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the two strings to be equal.
    /// The comparison is case-sensitive.
    /// </summary>
    public Expect IsEqual(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(expected != actual, $"Expected '{expected ?? string.Empty}' got '{actual ?? string.Empty}", message, filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the two strings to be different.
    /// The comparison is case-sensitive.
    /// </summary>
    public Expect IsNotEqual(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(expected == actual, $"Did not expect '{expected ?? string.Empty }', it should be different.", message, filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the string to be null or empty.
    /// </summary>
    public Expect IsEmpty(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(!string.IsNullOrEmpty(actual), message, $"Expected an empty value but got '{actual}'", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the string to have a value.
    /// </summary>
    public Expect IsNotEmpty(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(string.IsNullOrEmpty(actual), message, "Assertion was true", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the string to have a value that is not whitespace.
    /// </summary>
    public Expect IsNotBlank(
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(string.IsNullOrWhiteSpace(actual), message, "Expected the string to have a non-whitespace value", filename, member, line);
        return this;
    }

    /// <summary>
    /// Expects the two strings to be equal.
    /// The comparison is case insensitive.
    /// </summary>
    public Expect IsEqualIgnoreCase(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        RaiseIf(
            string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) != 0,
            $"Expected '{expected}' got '{actual}",
            message,
            filename,
            member,
            line);

        return this;
    }

    /// <summary>
    /// Expects the two strings to be different.
    /// The comparison is case insensitive.
    /// </summary>
    public Expect IsNotEqualIgnoreCase(
        string? expected,
        string? actual,
        string message = "",
        [CallerFilePath] string filename = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {

        RaiseIf(
            string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0,
            $"Did not expect '{expected}', it should be different.",
            message,
            filename,
            member,
            line);

        return this;
    }

    #region private methods

    private static void RaiseIf(
        bool condition,
        string error,
        string message,
        string filename,
        string member,
        int line)
    {
        if (!condition) return;

        var text = $"{error}.{Environment.NewLine}{message}.{Environment.NewLine}Error in {filename} at line {line} in {member}.";

        throw Fault.Raise(new ArgumentException(text));
    }

    #endregion
}

