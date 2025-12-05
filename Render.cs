namespace snakecs;

using etc;

public class Render(PlayArea playArea)
{
    private Snake? _currentSnake;
    private List<Food>? _currentFoodList;
    private int _currentScore;
    private Position? _prevTailPos;

    public void DrawFrame(Snake snake, List<Food> foodList, int score, Position? prevTailPos)
    {
        _currentSnake = snake;
        _currentFoodList = foodList;
        _currentScore = score;
        _prevTailPos = prevTailPos;

        DrawSnake();
        DrawFood();
        DrawUi();

        if (prevTailPos.HasValue)
            ClearSnakeTail();
    }

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

    private void DrawUi()
    {
        string scoreString = $"Score: {_currentScore}";

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

    private void DrawFood()
    {
        if (_currentFoodList == null) return;

        Console.ForegroundColor = ConsoleColor.DarkRed;
        foreach (var food in _currentFoodList)
        {
            Console.SetCursorPosition(food.Position.X, food.Position.Y);
            Console.Write(FoodIcon);
        }
    }

    private void DrawSnake()
    {
        if (_currentSnake == null) return;

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        foreach (var pos in _currentSnake.TailPositions)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(BodyIcon);
        }


        Console.SetCursorPosition(_currentSnake.HeadPosition.X, _currentSnake.HeadPosition.Y);
        Console.Write(_headIcons[_currentSnake.CurrentDirection]);
    }


    private void ClearSnakeTail()
    {
        if (_prevTailPos == null) return;

        Console.SetCursorPosition(_prevTailPos.Value.X, _prevTailPos.Value.Y);
        Console.Write(" ");
    }

    private void ClearSnake()
    {
        if (_currentSnake == null) return;

        Console.SetCursorPosition(_currentSnake.HeadPosition.X, _currentSnake.HeadPosition.Y);
        Console.Write(" ");

        foreach (var tailPos in _currentSnake.TailPositions)
        {
            Console.SetCursorPosition(tailPos.X, tailPos.Y);
            Console.Write(" ");
        }
    }

    private void ClearFood()
    {
        if (_currentFoodList == null) return;

        foreach (var food in _currentFoodList)
        {
            Console.SetCursorPosition(food.Position.X, food.Position.Y);
            Console.Write(" ");
        }
    }


    public void HandleDeath()
    {
        if (_currentSnake == null || _currentFoodList == null) return;

        for (int i = 0; i < 5; i++)
        {
            DrawSnake();
            DrawFood();
            Thread.Sleep(250);

            ClearFood();
            ClearSnake();
            Thread.Sleep(250);
        }

        DeathScreen();
    }

    private void DeathScreen()
    {
        const string gameOverMessage = "Game over!";

        ClearFood();
        ClearSnake();

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