namespace Core.Models;

public record Artist(string Id, string Name)
{
    public DateOnly DateAdded { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public List<Album> Albums { get; set; } = new List<Album>();
    public List<Song> Songs { get; set; } = new List<Song>();
}