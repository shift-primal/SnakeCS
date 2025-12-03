namespace snakecs;

using etc;

public class Game
{
    private readonly Snake _snake = new();
    private static readonly PlayArea PlayArea = new(Console.WindowWidth, Console.WindowHeight);
    private readonly Renderer _renderer = new(PlayArea);

    private const int Speed = 50;
    private bool IsRunning { get; set; } = true;

    private ConsoleKey? ReadKey()
    {
        if (!Console.KeyAvailable)
            return null;

        return Console.ReadKey(true).Key;
    }

    private void HandleInput(ConsoleKey? key)
    {
        if (key is null) return;

        switch (key.Value)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
                _snake.SwitchDirection(Direction.Up);
                break;

            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
                _snake.SwitchDirection(Direction.Down);
                break;

            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                _snake.SwitchDirection(Direction.Left);
                break;

            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                _snake.SwitchDirection(Direction.Right);
                break;
        }
    }

    private bool IsOutOfBounds()
    {
        int rightEdge = PlayArea.Width - PlayArea.PaddingWidth;
        int bottomEdge = PlayArea.Height - PlayArea.PaddingHeight;

        var headPos = _snake.HeadPosition;

        return headPos.X < PlayArea.PaddingWidth ||
               headPos.X >= rightEdge ||
               headPos.Y < PlayArea.PaddingHeight ||
               headPos.Y >= bottomEdge;
    }

    public void Run()
    {
        _renderer.InitGame();

        while (IsRunning)
        {
            var pressedKey = ReadKey();
            HandleInput(pressedKey);

            var removedPos = _snake.Move();

            if (IsOutOfBounds()) _snake.Kill();
            if (!_snake.IsAlive)
            {
                Thread.Sleep(500);
                break;
            }

            _renderer.DrawFrame(_snake, removedPos);


            Thread.Sleep(Speed);
        }

        _renderer.DeathScreen();
    }
}