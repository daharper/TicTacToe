namespace TicTacToe.Infrastructure.Utils;

/// <summary>
/// Contains a sequence of items which can be treated individually or as a whole.
/// Asking the sequence if it is empty or full is the same as asking if all the
/// items are empty or full. Contained items have the concept of being empty or
/// occupied. The sequence is immutable, but the contained items are not.
/// </summary>
/// <typeparam name="T">The type of item stored in the sequence.</typeparam>
public class Sequence<T> where T : ISequenceable
{
    #region private
    
    private readonly T[] _items;
    
    #endregion

    #region ctors

    /// <summary>
    /// Initializes a new instance of the <see cref="Sequence{T}"/> class.
    /// </summary>
    public Sequence(params T[] items) => _items = items;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Sequence{T}"/> class.
    /// </summary>
    public Sequence(IEnumerable<T> items) => _items = items.ToArray();
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the items in the sequence as an enumerable.
    /// </summary>
    public IEnumerable<T> Items => _items;
    
    /// <summary>
    /// Gets the item at the specified index.
    /// </summary>
    public T this[int index] => _items[index];
    
    /// <summary>
    /// Gets a value indicating whether all the items in the sequence are occupied.
    /// </summary>
    public bool IsFull => _items.All(s => s.IsOccupied);

    /// <summary>
    /// Gets a value indicating whether all the items in the sequence are the same.
    /// </summary>
    public bool IsSame => _items.All(s => s.IsSame(_items[0]));
    
    /// <summary>
    /// Gets a value indicating whether all the items in the sequence are the same or empty.
    /// </summary>
    public bool IsSameOrEmpty => _items.All(s => s.IsEmpty || s.IsSame(_items[0]));
    
    /// <summary>
    /// Gets the first item in the sequence that is empty.
    /// </summary>
    public T? FirstEmpty => _items.FirstOrDefault(s => s.IsEmpty);

    /// <summary>
    /// Gets the number of items in the sequence.
    /// </summary>
    public int Length => _items.Length;

    /// <summary>
    /// Gets a value indicating whether the sequence contains any items.
    /// </summary>
    public bool HasItems => _items.Length > 0;
    
    /// <summary>
    /// Gets a value indicating whether the sequence contains no items.
    /// </summary>
    public bool HasNoItems => _items.Length == 0;
    
    /// <summary>
    /// Gets the number of items in the sequence that are empty.
    /// </summary>
    public int EmptyCount => _items.Count(s => s.IsEmpty);

    /// <summary>
    /// Gets the number of items in the sequence that are occupied.
    /// </summary>
    public int OccupiedCount => _items.Count(s => s.IsOccupied);
    
    public T First => this[0];
    
    public T Second => this[1];
    
    public T Third => this[2];

    #endregion
    
    #region public methods
    
    /// <summary>
    /// Gets the number of items which match the predicate.
    /// </summary>
    public int CountOf(Func<T, bool> predicate) => _items.Count(predicate);
    
    /// <summary>
    /// Gets a new sequence with the empty items from this sequence.
    /// </summary>
    public Sequence<T> EmptySequence => new(_items.Where(s => s.IsEmpty));

    /// <summary>
    /// Gets a new sequence with the filtered items.
    /// </summary>
    public Sequence<T> Filter(Func<T, bool> predicate) => new(_items.Where(predicate));

    #endregion
}