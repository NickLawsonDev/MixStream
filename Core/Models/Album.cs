namespace Core.Models;

public record Album(string Id, string Name, int TotalTracks, string ReleaseDate)
{
    public List<Song> Songs { get; set; } = new List<Song>();
    public DateOnly DateAdded { get; init; } = DateOnly.FromDateTime(DateTime.Now);
}