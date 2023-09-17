namespace TicTacToe.Infrastructure.Utils;

/// <summary>
/// Utility methods for working with generics.
/// </summary>
public static class Generics<T>
{
    private static readonly IEqualityComparer<T> Comparer = EqualityComparer<T>.Default;

    /// <summary>
    /// Determines if the specified value is the default value for type T.
    /// </summary>
    public static bool IsDefault(T value) => Comparer.Equals(value, default!);

    /// <summary>
    /// Determines if the specified value is not the default value for type T.
    /// </summary>
    public static bool IsNotDefault(T value) => !Comparer.Equals(value, default!);

    /// <summary>
    /// Determines if the specified values are equal.
    /// </summary>
    public static bool IsEqual(T actual, T expected) => Comparer.Equals(actual, expected);

    /// <summary>
    /// Determines if the specified values are unequal.
    /// </summary>
    public static bool IsNotEqual(T actual, T expected) => Comparer.Equals(actual, expected);
}
