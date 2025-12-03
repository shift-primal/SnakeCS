namespace snakecs.etc;

public readonly struct Position(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}