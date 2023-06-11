namespace Core.Models;

public record Song(string Id, string Title, int TrackNumber)
{
    public string FilePath { get; init; } = "";
    public DateOnly DateAdded { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public required IEnumerable<Artist> Artists { get; init; }
    public required Album Album { get; init; }
}