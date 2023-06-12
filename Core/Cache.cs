using Core.Models;

namespace Core;

public enum CurrentType
{
    Playlist,
    Menu,
    Album
}

public static class Cache
{
    private static List<Playlist> _playlists { get; set; } = new List<Playlist>();
    private static int _currentPage = 0;
    public static CurrentType CurrentType { get; set; } = CurrentType.Menu;

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

    public static int GetCurrentPage() => _currentPage;
    public static void UpdateCurrentPage(int currentPage) => _currentPage = currentPage;
}