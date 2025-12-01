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

    public class Snake
    {
        public bool IsAlive { get; private set; } = true;

        private Position Position { get; set; } = new(Console.WindowWidth / 2, Console.WindowHeight / 2);

        private Direction Direction { get; set; } = Direction.Up;


        public void Move()
        {
            Position = Direction switch
            {
                Direction.Up => Position = new Position(Position.X, Position.Y - 1),
                Direction.Down => Position = new Position(Position.X, Position.Y + 1),
                Direction.Left => Position = new Position(Position.X - 1, Position.Y),
                Direction.Right => Position = new Position(Position.X + 1, Position.Y),
                _ => Position
            };
        }

        public void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write("#");
        }

        private static Direction? GetDirection(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow => Direction.Up,
                ConsoleKey.DownArrow => Direction.Down,
                ConsoleKey.LeftArrow => Direction.Left,
                ConsoleKey.RightArrow => Direction.Right,
                _ => null
            };
        }


        public void ReadKey()
        {
            if (!Console.KeyAvailable)
                return;

            var inputKey = Console.ReadKey(true);

            if (inputKey.Key == ConsoleKey.Escape)
                IsAlive = false;

            var direction = GetDirection(inputKey.Key);


            if (direction is null) return;
            if (!IsValidDirection(direction.Value, Direction))
                return;
            Direction = direction.Value;
        }

        public bool IsOutOfBounds()
        {
            var rightEdge = Console.WindowWidth;
            var bottomEdge = Console.WindowHeight;

            return Position.X <= 0 || Position.X > rightEdge || Position.Y < 0 || Position.Y > bottomEdge;
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

    public static void GameLoop(Snake snake, int speed)
    {
        snake.ReadKey();
        snake.Move();

        if (snake.IsOutOfBounds())
            snake.Kill();

        snake.Draw();
        Thread.Sleep(speed);
    }

    public static void Main()
    {
        const int speed = 25;
        var snake = new Snake();

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;

        while (snake.IsAlive) GameLoop(snake, speed);
    }
}