namespace snakecs;

using etc;

public class Game
{
    private readonly Snake _snake = new();
    private static readonly PlayArea PlayArea = new(Console.WindowWidth, Console.WindowHeight);
    private readonly Render _render = new(PlayArea);

    private const int Speed = 50;
    private bool IsRunning { get; set; } = true;

    private int Score { get; set; }

    private readonly List<Food> _foodList = [new(PlayArea, 1), new(PlayArea, 2), new(PlayArea, 3)];

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
            case ConsoleKey.Escape:
                IsRunning = false;
                break;

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

            case ConsoleKey.Spacebar:
                _snake.Grow();
                Score += 1;
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

    private void PickupFood(int foodId)
    {
        var pickedUpFood = _foodList.Find(food => food.Id == foodId);

        if (pickedUpFood != null)
            _foodList.Remove(pickedUpFood);

        _foodList.Add(new Food(PlayArea, foodId));

        _snake.Grow();
        Score += 1;
    }

    private int? IsOverFood()
    {
        foreach (var food in _foodList)
            if (_snake.HeadPosition == food.Position)
                return food.Id;
        return null;
    }


    public void Run()
    {
        _render.InitGame();


        while (IsRunning)
        {
            var pressedKey = ReadKey();
            HandleInput(pressedKey);

            var removedPos = _snake.Move();

            int? foodPickUpId = IsOverFood();
            if (foodPickUpId != null) PickupFood(foodPickUpId.Value);


            if (IsOutOfBounds()) _snake.Kill();

            if (!_snake.IsAlive)
            {
                Thread.Sleep(500);
                break;
            }

            _render.DrawFrame(_snake, removedPos, _foodList, Score);

            Thread.Sleep(Speed);
        }

        _render.DeathScreen();
    }
}