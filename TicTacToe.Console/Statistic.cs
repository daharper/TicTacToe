using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Console;

public class Statistic
{
    public Statistic(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public int Games { get; set; }

    public int Draws => Games - Wins - Losses;

    public int Losses { get; set; }

    public int Wins { get; set; }

    public double WinPercentage => Games == 0 || Wins == 0 ? 0.0 : (double)Wins / Games;

    public override string ToString() => $"{Name, -10} {Games, -3} {Wins, -3} {Draws, -3} {Losses, -3} {WinPercentage:P}";
}