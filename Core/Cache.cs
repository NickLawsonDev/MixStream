using Core.Models;

namespace Core;

public static class Cache
{
    private static List<Playlist> _playlists { get; set; } = new List<Playlist>();

    public static void UpdatePlaylists(List<Playlist> playlists)
    {
        Guards.IsNotNull(playlists);
        Guards.IsNotEmpty<List<Playlist>>(playlists);
        _playlists = playlists.OrderByDescending(x => x.DateAdded).ToList();
    }

    public static Playlist GetPlaylist(string name)
    {
        Guards.IsNotNullOrEmpty(name);
        return _playlists.First(x => x.Name.ToLower() == name.ToLower());
    }

    public static List<Playlist> GetPlaylists() => _playlists.OrderByDescending(x => x.DateAdded).ToList();
}