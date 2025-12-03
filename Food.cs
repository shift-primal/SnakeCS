namespace snakecs;

using etc;

public abstract class Food
{
    private Position _position = new(Random.Shared.Next(1, Console.WindowWidth - 1),
        Random.Shared.Next(0, Console.WindowHeight - 1));
}