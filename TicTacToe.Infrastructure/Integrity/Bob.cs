namespace TicTacToe.Infrastructure.Integrity;

public static class Bob
{
    public static readonly Expect Expects = new();

    public static readonly Assume Assumes = new(Expects);
}