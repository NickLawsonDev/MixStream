namespace Core.Models;

public record Playlist (string Id, string Name)
{
    public List<Song> Songs { get; set; } = new List<Song>();
    public DateOnly DateAdded { get; init; } = DateOnly.FromDateTime(DateTime.Now);
}