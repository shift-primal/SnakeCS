namespace snakecs;

using etc;

public class Render(PlayArea playArea)
{
    private readonly Dictionary<Direction, string> _headIcons = new()
    {
        { Direction.Up, "U" },
        { Direction.Down, "D" },
        { Direction.Left, "L" },
        { Direction.Right, "R" }
    };

    private const string BodyIcon = "B";
    private const string FoodIcon = "F";

    private void DrawPlayArea()
    {
        Console.BackgroundColor = ConsoleColor.Black;

        for (int x = playArea.PaddingWidth; x < playArea.Width - playArea.PaddingWidth; x++)
        for (int y = playArea.PaddingHeight - 5; y < playArea.PaddingHeight - 2; y++)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }

        for (int x = playArea.PaddingWidth; x < playArea.Width - playArea.PaddingWidth; x++)
        for (int y = playArea.PaddingHeight; y < playArea.Height - playArea.PaddingHeight; y++)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }
    }

    private void DrawUi(int score)
    {
        string scoreString = $"Score: {score}";

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(playArea.PaddingWidth + 2,
            playArea.PaddingHeight - 4);
        Console.WriteLine(scoreString);
    }

    public void InitGame()
    {
        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.Clear();
        DrawPlayArea();
    }

    private void DrawFood(List<Food> foodList)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        foreach (var food in foodList)
        {
            Console.SetCursorPosition(food.Position.X, food.Position.Y);
            Console.Write(FoodIcon);
        }
    }

    private void DrawSnake(Snake snake)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        foreach (var pos in snake.TailPositions)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(BodyIcon);
        }


        Console.SetCursorPosition(snake.HeadPosition.X, snake.HeadPosition.Y);
        Console.Write(_headIcons[snake.CurrentDirection]);
    }


    private void ClearSnakeTail(Position position)
    {
        Console.SetCursorPosition(position.X, position.Y);
        Console.Write(" ");
    }

    public void DrawFrame(Snake snake, Position? prevTailPos, List<Food> foodList, int score)
    {
        DrawSnake(snake);
        if (prevTailPos.HasValue) ClearSnakeTail(prevTailPos.Value);

        DrawFood(foodList);

        DrawUi(score);
    }

    public void DeathScreen()
    {
        const string gameOverMessage = "Game over!";

        DrawPlayArea();

        for (int i = 0; i < gameOverMessage.Length; i++)
        {
            Console.SetCursorPosition(
                playArea.Width / 2 - gameOverMessage.Length / 2 + i,
                playArea.Height / 2);
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