namespace TicTacToe.Infrastructure.Utils;

public static class Collections
{
    /// <summary>
    /// Iterates over an enumeration of items and executes the specified action
    /// passing it the item and the current index.
    /// </summary>
    public static void DoWithIndex<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var i = 0;

        foreach (var item in items)
        {
            action(item, i++);
        }
    }

    /// <summary>
    /// Iterates over an enumeration of items and executes the specified action.
    /// </summary>
    public static void Do<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
        {
            action(item);
        }
    }

    /// <summary>
    /// Iterates over an enumeration of Type/Attribute pairs and executes the specified action.
    /// </summary>
    public static void Do<TAttribute>(this IEnumerable<(Type, TAttribute)> items, Action<Type, TAttribute> action)
        where TAttribute : Attribute
    {
        foreach (var (type, attribute) in items)
        {
            action(type, attribute);
        }
    }
}