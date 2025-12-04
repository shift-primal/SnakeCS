namespace snakecs;

using etc;

public class Food(PlayArea playArea)
{
    public Position Position = new(Random.Shared.Next(playArea.PaddingWidth, playArea.Width - playArea.PaddingWidth),
        Random.Shared.Next(playArea.PaddingHeight, playArea.Height - playArea.PaddingHeight));
}