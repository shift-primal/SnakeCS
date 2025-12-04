namespace snakecs;

using etc;

public class Snake
{
    public bool IsAlive { get; private set; } = true;
    public Direction CurrentDirection { get; private set; } = Direction.Right;

    public Position HeadPosition = new(Console.WindowWidth / 2, Console.WindowHeight / 2);

    private int TailLength { get; set; } = 5;
    public List<Position> TailPositions { get; set; } = [];

    public Position? Move()
    {
        HeadPosition = CurrentDirection switch
        {
            Direction.Up => new Position(HeadPosition.X, HeadPosition.Y - 1),
            Direction.Down => new Position(HeadPosition.X, HeadPosition.Y + 1),
            Direction.Left => new Position(HeadPosition.X - 1, HeadPosition.Y),
            Direction.Right => new Position(HeadPosition.X + 1, HeadPosition.Y),
            _ => HeadPosition
        };

        TailPositions.Insert(0, HeadPosition);

        if (TailPositions.Count <= TailLength)
            return null;

        var removedPos = TailPositions[^1];
        TailPositions.RemoveAt(TailPositions.Count - 1);
        return removedPos;
    }


    public void SwitchDirection(Direction direction)
    {
        if (!IsValidDirection(direction, CurrentDirection))
            return;
        CurrentDirection = direction;
    }


    private static bool IsHorizontal(Direction direction)
    {
        return direction is not (Direction.Up or Direction.Down);
    }

    private static bool IsValidDirection(Direction dir1, Direction dir2)
    {
        return IsHorizontal(dir1) != IsHorizontal(dir2);
    }

    public void Kill()
    {
        IsAlive = false;
    }

    public void Grow()
    {
        TailLength += 2;
    }
}