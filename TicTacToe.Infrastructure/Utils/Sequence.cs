namespace TicTacToe.Infrastructure.Utils;

/// <summary>
/// Contains a sequence of items which can be treated individually or as a whole.
/// Asking the sequence if it is empty or full is the same as asking if all the
/// items are empty or full. Contained items have the concept of being empty or
/// occupied. The sequence is immutable, but the contained items are not.
/// </summary>
/// <typeparam name="T">The type of item stored in the sequence.</typeparam>
public class Sequence<T> where T : ISequenceValue
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
    /// Gets a value indicating whether all the items in the sequence are empty.
    /// </summary>
    public bool IsEmpty => _items.All(s => s.IsEmpty);

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
    /// Gets the index of the first item in the sequence that is occupied, or -1 if none are empty.
    /// </summary>
    public int FirstEmptyIndex
    {
        get
        {
            for(var i = 0; i < _items.Length; i++)
            {
                if (_items[i].IsEmpty)
                {
                    return i;
                }
            }

            return -1;
        }
    }
    
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
    
    /// <summary>
    /// Gets a list of indexes of items in the sequence that are empty.
    /// </summary>
    public List<int> EmptyIndexes => IndexesFor(false);

    /// <summary>
    /// Gets a list of indexes of items in the sequence that are occupied.
    /// </summary>
    public List<int> OccupiedIndexes => IndexesFor(true);
    
    public T First => this[0];
    
    public T Second => this[1];
    
    public T Third => this[2];

    #endregion
    
    #region public methods
    
    public bool Any(Func<T, bool> predicate) => _items.Any(predicate);
    
    /// <summary>
    /// Gets the number of items in the sequence that are the same as the specified item.
    /// </summary>
    public int SameCount(T item) => _items.Count(s => s.IsSame(item));

    /// <summary>
    /// Gets the number of items which match the predicate.
    /// </summary>
    public int CountOf(Func<T, bool> predicate) => _items.Count(predicate);
    
    /// <summary>
    /// Gets the number of items in the sequence that are different from the specified item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int DiffCount(T item) => _items.Count(s => s.IsSame(item) == false);
    
    /// <summary>
    /// Gets a new sequence with the occupied items from this sequence.
    /// </summary>
    public Sequence<T> OccupiedSequence => new(_items.Where(s => s.IsOccupied));
    
    /// <summary>
    /// Gets a new sequence with the empty items from this sequence.
    /// </summary>
    public Sequence<T> EmptySequence => new(_items.Where(s => s.IsEmpty));

    /// <summary>
    /// Gets a new sequence with the items from this sequence that are the same as the specified item.
    /// </summary>
    public Sequence<T> SameSequence(T item) => new(_items.Where(s => s.IsSame(item)));

    /// <summary>
    /// Gets a new sequence with the items from this sequence that are different from the specified item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Sequence<T> DiffSequence(T item) => new(_items.Where(s => s.IsSame(item) == false));

    /// <summary>
    /// Gets a new sequence with the filtered items.
    /// </summary>
    public Sequence<T> Filter(Func<T, bool> predicate) => new(_items.Where(predicate));
    
    /// <summary>
    /// Get indexes of items in the sequence that are the same as the specified item.
    /// </summary>
    public List<int> IndexesFor(T item)
    {
        var indexes = new List<int>();
        
        for (var i = 0; i < _items.Length; ++i)
        {
            var it = _items[i];
            
            if (it.IsSame(item))
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    /// <summary>
    /// Gets indexes of items in the sequence that are the same as the specified items.
    /// The indexes are grouped by the specified items and in the same order.
    /// </summary>
    public List<List<int>> GroupedIndexesFor(params T[] items)
        => items.Select(IndexesFor).ToList();

    public List<Sequence<T>> SameItems(params T[] items)
    {
        var sequences = new List<Sequence<T>>();

        foreach (var item in items)
        {
            sequences.Add(new Sequence<T>(_items.Where(s => s.IsSame(item))));
        }
        
        return sequences;    
    }
    
    #endregion

    #region private methods

    /// <summary>
    /// Gets a list of indexes of items in the sequence that are empty or occupied.
    /// </summary>
    private List<int> IndexesFor(bool isOccupied)
    {
        var indexes = new List<int>();
        
        for(var i = 0; i < _items.Length; i++)
        {
            var item = _items[i];

            if ((isOccupied == false && item.IsEmpty) || (isOccupied && item.IsOccupied))
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }
    
    #endregion
}