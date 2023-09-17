using TicTacToe.Infrastructure.Integrity;

using View = System.Console;

namespace TicTacToe.Console;

/// <summary>
/// Represents a canvas for drawing. 
/// </summary>
public class Canvas
{
    #region private
    
    private readonly char[] _pixels;
    private readonly int _height;
    private readonly int _width;
    
    #endregion
    
    #region ctors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Canvas"/> class.
    /// </summary>
    public Canvas(int width, int height, string value)
    {
        (_width, _height) = (width, height);
        
        var length = width * height;
        
        Bob.Assumes.IsTrue(
            value.Length == 0 || value.Length == length, 
            "Value must be the same size as the canvas, or empty.");
        
        _pixels = new char[width * height];

        for(var i = 0; i < value.Length; i++)
        {
            _pixels[i] = value[i];
        }
    }
    
    #endregion
    
    #region public methods
    
    /// <summary>
    /// Puts a character at the specified location.
    /// </summary>
    public void Plot(char ch, int x, int y) 
        => _pixels[y * _width + x] = ch;

    /// <summary>
    /// Displays the canvas to the console.
    /// </summary>
    public void Display()
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var ch = _pixels[y * _width + x];
                View.Write(ch);
            }
            View.WriteLine();
        }
    }
    
    #endregion
}