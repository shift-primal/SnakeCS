namespace snakecs;

public static class Program
{
    private readonly struct Position(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }


    private class Snake
    {
        public bool IsAlive { get; private set; } = true;
        public Direction CurrentDirection { get; private set; } = Direction.Right;

        public Position HeadPosition = new(Console.WindowWidth / 2, Console.WindowHeight / 2);

        public int TailLength { get; private set; } = 5;
        public List<Position> TailPositions { get; set; } = [];

        public void Move()
        {
            HeadPosition = CurrentDirection switch
            {
                Direction.Up => new Position(HeadPosition.X, HeadPosition.Y - 1),
                Direction.Down => new Position(HeadPosition.X, HeadPosition.Y + 1),
                Direction.Left => new Position(HeadPosition.X - 1, HeadPosition.Y),
                Direction.Right => new Position(HeadPosition.X + 1, HeadPosition.Y),
                _ => HeadPosition
            };


            if (TailPositions.Count > TailLength) TailPositions.RemoveAt(TailPositions.Count - 1);
        }


        public void SwitchDirection(Direction direction)
        {
            if (!IsValidDirection(direction, CurrentDirection))
                return;
            CurrentDirection = direction;
        }


        public bool IsOutOfBounds()
        {
            int rightEdge = Console.WindowWidth;
            int bottomEdge = Console.WindowHeight;

            return HeadPosition.X < 0 || HeadPosition.X > rightEdge || HeadPosition.Y < 0 ||
                   HeadPosition.Y > bottomEdge;
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
    }

    private class Game
    {
        private readonly Snake _snake = new();

        private const int Speed = 50;

        private bool IsRunning { get; set; } = true;

        private readonly Dictionary<Direction, string> _icons = new()
        {
            { Direction.Up, "▲" },
            { Direction.Down, "▼" },
            { Direction.Left, "◀" },
            { Direction.Right, "▶" }
        };

        private const string BodyIcon = "■";


        private void DrawFrame()
        {
            Console.SetCursorPosition(_snake.HeadPosition.X, _snake.HeadPosition.Y);
            Console.Write(_icons[_snake.CurrentDirection]);

            foreach (var pos in _snake.TailPositions)
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.Write(BodyIcon);
            }
        }

        private static ConsoleKey? ReadKey()
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


        public void Run()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Clear();

            while (IsRunning)
            {
                var pressedKey = ReadKey();
                HandleInput(pressedKey);

                _snake.Move();


                if (_snake.IsOutOfBounds()) _snake.Kill();
                if (!_snake.IsAlive)
                {
                    Thread.Sleep(500);
                    break;
                }

                DrawFrame();


                Thread.Sleep(Speed);
            }

            DeathScreen();
        }
    }


    public static void DeathScreen()
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


    public static void Main()
    {
        Game game = new();
        game.Run();
    }
}