namespace FapDown;

internal class Image
{
    public required Gallery Gallery { get; init; }
    public required Uri Uri { get; init; }
    public required DateOnly Date { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string Author { get; init; }
    public required string Title { get; init; }
}
