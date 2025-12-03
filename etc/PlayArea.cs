namespace snakecs.etc;

public readonly struct PlayArea(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int PaddingWidth { get; } = width / 10;
    public int PaddingHeight { get; } = height / 10;
}