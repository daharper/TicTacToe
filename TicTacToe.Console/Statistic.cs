﻿namespace TicTacToe.Console;

/// <summary>
/// Represents a player's statistics.
/// </summary>
public class Statistic
{
    public Statistic(string name) => Name = name;

    public string Name { get; set; }
    
    public int Games { get; set; }
    public int Draws => Games - Wins - Losses;
    public int Losses { get; set; }
    public int Wins { get; set; }
    
    public double Points => Wins * 3 + Draws;
    public double WinPercentage => Games == 0 || Wins == 0 ? 0.0 : (double)Wins / Games;

    public string WinPercentageText => $"{WinPercentage:P}";
    
    public override string ToString() => $"{Name, -8} {Games, 3} {Wins, 3} {Draws, 3} {Losses, 3} {Points,6} {WinPercentageText, 7}";
}