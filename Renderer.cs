namespace snakecs;

using etc;

public class Renderer(PlayArea playArea)
{
    private readonly Dictionary<Direction, string> _headIcons = new()
    {
        { Direction.Up, "U" },
        { Direction.Down, "D" },
        { Direction.Left, "L" },
        { Direction.Right, "R" }
    };

    private const string BodyIcon = "B";

    private void DrawPlayArea()
    {
        int paddingX = playArea.Width / 10;
        int paddingY = playArea.Height / 10;

        Console.BackgroundColor = ConsoleColor.Gray;

        for (int x = playArea.PaddingWidth; x < playArea.Width - playArea.PaddingWidth; x++)
        for (int y = playArea.PaddingHeight; y < playArea.Height - playArea.PaddingHeight; y++)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }
    }

    public void InitGame()
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        DrawPlayArea();
    }

    private void DrawSnake(Snake snake)
    {
        foreach (var pos in snake.TailPositions)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(BodyIcon);
        }

        Console.SetCursorPosition(snake.HeadPosition.X, snake.HeadPosition.Y);
        Console.Write(_headIcons[snake.CurrentDirection]);
    }


    private static void ClearSnakeTail(Position position)
    {
        Console.SetCursorPosition(position.X, position.Y);
        Console.Write(" ");
    }

    public void DrawFrame(Snake snake, Position? prevTailPos)
    {
        DrawSnake(snake);

        if (prevTailPos.HasValue) ClearSnakeTail(prevTailPos.Value);
    }

    public void DeathScreen()
    {
        const string gameOverMessage = "Game over!";

        Console.Clear();

        for (int i = 0; i < gameOverMessage.Length; i++)
        {
            Console.SetCursorPosition(
                Console.WindowWidth / 2 - gameOverMessage.Length / 2 + i,
                Console.WindowHeight / 2);
            Console.Write(gameOverMessage[i]);

            if (i == gameOverMessage.Length - 1)
            {
                Thread.Sleep(2000);
                Console.Clear();
                continue;
            }

            Thread.Sleep(100);
        }
    }
}